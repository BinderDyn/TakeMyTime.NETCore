using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        private DateTime? started = null;
        private bool alreadyStarted = false;
        private Timer timer;

        public AddEntry(int projectId, int assignmentId)
        {
            InitializeComponent();
            this.project_id = projectId;
            this.assignment_id = assignmentId;
            timer = new Timer();
            timer.Interval = 1000;
            this.btn_StartStop.Content = ResourceStringManager.GetResourceByKey("ButtonTextStart");
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
            if (!string.IsNullOrWhiteSpace(this.tb_Name.Text))
            {
                CreateEntry();
            }
        }
        private void btn_StartStop_Click(object sender, RoutedEventArgs e)
        {
            this.StartStopTimer();
        }

        #endregion

        public void Load()
        {
            var subtaskLogic = new SubtaskLogic();
            var subtasks = subtaskLogic.GetByAssignmentId(assignment_id);
            this.SubtaskViewModels = subtasks
                .Select(s => new SubtaskComboBoxViewModel(s))
                .ToList();
            this.cb_Subtask.ItemsSource = this.SubtaskViewModels;
            this.cb_Subtask.IsEnabled = this.SubtaskViewModels.Count > 0;
        }

        public void CreateEntry()
        {
            var projectLogic = new ProjectLogic();
            var project = projectLogic.GetProjectById(project_id);
            projectLogic.Dispose();
            var subtaskLogic = new SubtaskLogic();
            var subtask = subtaskLogic.Get(this.SelectedSubtask.Id);

            var entryCreateViewModel = new EntryCreateViewModel
            {
                Comment = this.tb_Description.Text,
                Date = DateTime.Now,
                Name = this.tb_Name.Text,
                Project = project,
                Subtask = subtask,
                DurationAsTicks = this.DurationElapsed.Ticks,
                Ended = this.started + this.DurationElapsed,
                Started = this.started
            };

            subtaskLogic.AddEntry(this.SelectedSubtask.Id, entryCreateViewModel);
            subtaskLogic.Dispose();
        }

        public void StartStopTimer()
        {
            if (!timer.Enabled)
            {
                if (!this.alreadyStarted)
                {
                    this.alreadyStarted = true;
                    this.started = DateTime.Now;
                }

                timer.Start();
                timer.Elapsed += Timer_Elapsed;
                this.btn_StartStop.Background = Brushes.DarkRed;
                this.btn_StartStop.Content = ResourceStringManager.GetResourceByKey("ButtonTextStop");
            }
            else
            {
                timer.Stop();
                this.btn_StartStop.Content = ResourceStringManager.GetResourceByKey("ButtonTextStart");
                this.btn_StartStop.Background = Brushes.Green;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.DurationElapsed = e.SignalTime - this.started.Value;
            this.UpdateUI();
        }

        public void UpdateUI()
        {
            this.ElapsedAsString = this.DurationElapsed.ToString(@"hh\:mm\:ss");
        }

        public List<SubtaskComboBoxViewModel> SubtaskViewModels { get; set; }
        public SubtaskComboBoxViewModel SelectedSubtask { get; set; }
        public TimeSpan DurationElapsed { get; set; } = new TimeSpan();
        public string ElapsedAsString { get; set; }
        
    }
}
