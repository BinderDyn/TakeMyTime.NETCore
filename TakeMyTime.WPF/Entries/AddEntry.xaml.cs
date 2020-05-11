using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TakeMyTime.BLL.Logic;
using TakeMyTime.Models.Models;
using TakeMyTime.WPF.Utility;

namespace TakeMyTime.WPF.Entries
{
    /// <summary>
    /// Interaktionslogik für AddEntry.xaml
    /// </summary>
    public partial class AddEntry : Window
    {
        private readonly int project_id;
        private readonly int assignment_id;
        private readonly int? entry_id;
        private DateTime? started = null;
        private DateTime? stopped = null;
        private DispatcherTimer timer;

        public AddEntry(int projectId, int assignmentId)
        {
            InitializeComponent();
            this.project_id = projectId;
            this.assignment_id = assignmentId;
            timer = new DispatcherTimer(DispatcherPriority.Send);
            timer.Interval = new TimeSpan(0, 0, 1);
            this.btn_StartStop.Content = ResourceStringManager.GetResourceByKey("ButtonTextStart");
            this.Load();
            this.cb_Subtask.SelectedItem = this.SubtaskViewModels.FirstOrDefault(s => s.Id > 0);
            if (!this.SubtaskViewModels.Any()) this.chebo_FinishesSubtask.IsEnabled = false;
        }

        public AddEntry(int entry_id)
        {
            InitializeComponent();
            this.chebo_FinishesSubtask.IsEnabled = false;
            var entryLogic = new EntryLogic();
            var entry = entryLogic.GetEntryById(entry_id);
            this.tb_Description.Text = entry.Comment;
            this.tb_Name.Text = entry.Name;
            this.cb_Subtask.IsEnabled = false;
            this.btn_StartStop.IsEnabled = false;
            this.entry_id = entry_id;
        }

        #region GUI EVENTS

        private void b_Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.tb_Name.Text)) return;
            if (this.entry_id.HasValue)
            {
                this.EditAndSaveEntry();
            }
            else
            {
                this.stopped = DateTime.Now;
                this.CreateEntry();
            }
        }

        private void btn_StartStop_Click(object sender, RoutedEventArgs e)
        {
            this.StartStopTimer();
        }

        private void cb_Subtask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
            {
                this.SelectedSubtask = e.AddedItems[0] as SubtaskComboBoxViewModel;
                this.tb_Name.Text = this.SelectedSubtask.Name;
            }
        }

        private void tb_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.btn_Save.IsEnabled = !string.IsNullOrWhiteSpace(this.tb_Name.Text);
        }


        #endregion

        #region Utility

        public void Load()
        {
            var subtaskLogic = new SubtaskLogic();
            var subtasks = subtaskLogic.GetByAssignmentId(assignment_id).Where(s => s.Status == EnumDefinition.SubtaskStatus.NotYetDone);
            this.SubtaskViewModels = subtasks
                .Select(s => new SubtaskComboBoxViewModel(s))
                .ToList();
            this.cb_Subtask.ItemsSource = this.SubtaskViewModels;
            this.cb_Subtask.IsEnabled = this.SubtaskViewModels.Count > 0;
        }

        public void EditAndSaveEntry()
        {
            if (!string.IsNullOrWhiteSpace(this.tb_Name.Text))
            {
                var entryLogic = new EntryLogic();
                entryLogic.UpdateEntry(this.entry_id.Value, new EntryUpdateParam { Name = this.tb_Name.Text, Comment = this.tb_Description.Text });
                entryLogic.Dispose();
                this.Close();
            }
        }

        public void CreateEntry()
        {
            var projectLogic = new ProjectLogic();
            var project = projectLogic.GetProjectById(project_id);
            projectLogic.Dispose();
            var subtaskLogic = new SubtaskLogic();
            var subtask = subtaskLogic.GetById(this.SelectedSubtask.Id);
            ChangeParentAssignmentToInProgress(subtask);

            var entryCreateViewModel = new EntryCreateViewModel
            {
                Comment = this.tb_Description.Text,
                Date = DateTime.Now,
                Name = this.tb_Name.Text,
                Project = project,
                Subtask = subtask,
                DurationAsTicks = this.DurationElapsed.Ticks,
                Ended = this.stopped,
                Started = this.started
            };

            subtaskLogic.AddEntry(this.SelectedSubtask.Id, entryCreateViewModel, this.chebo_FinishesSubtask.IsChecked.Value);
            subtaskLogic.Dispose();

            this.Close();
        }

        private static void ChangeParentAssignmentToInProgress(Subtask subtask)
        {
            // Update AssignmentStatus to InProgress
            if (subtask == null) return;
            var assignmentLogic = new AssignmentLogic();
            assignmentLogic.UpdateAssignmentStatus(subtask.Assignment_Id.GetValueOrDefault(0), EnumDefinition.AssignmentStatus.InProgress);
            assignmentLogic.Dispose();
        }

        public void StartStopTimer()
        {
            if (!timer.IsEnabled)
            {
                this.started = DateTime.Now;
                timer.Start();

                timer.Tick += Timer_Elapsed;
                this.btn_StartStop.Background = Brushes.DarkRed;
                this.btn_StartStop.Content = ResourceStringManager.GetResourceByKey("ButtonTextStop");
            }
            else
            {
                timer.Tick -= Timer_Elapsed;
                timer.Stop();
                timer.IsEnabled = false;
                this.btn_StartStop.Content = ResourceStringManager.GetResourceByKey("ButtonTextStart");
                this.btn_StartStop.Background = Brushes.Green;
            }
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            if (timer.IsEnabled)
            {
                this.DurationElapsed = this.DurationElapsed.Add(new TimeSpan(0, 0, 1));
                this.UpdateUI();
            }
        }

        public void UpdateUI()
        {
            this.tb_Elapsed.Text = this.ElapsedAsString;
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion

        #region Properties

        public List<SubtaskComboBoxViewModel> SubtaskViewModels { get; set; }
        public SubtaskComboBoxViewModel SelectedSubtask { get; set; }
        public TimeSpan DurationElapsed { get; set; } = new TimeSpan();
        public string ElapsedAsString { get => this.DurationElapsed.ToString(@"hh\:mm\:ss"); }

        #endregion
    }
}
