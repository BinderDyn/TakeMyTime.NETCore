using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TakeMyTime.BLL.Logic;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;
using TakeMyTime.WPF.Subtasks;

namespace TakeMyTime.WPF.Assignments
{
    /// <summary>
    /// Interaktionslogik für AddAssignment.xaml
    /// </summary>
    public partial class AddAssignment : Window
    {
        public AddAssignment(Project project)
        {
            InitializeComponent();
            this.Project = project;
        }

        public AddAssignment(Assignment assignment, Project project)
        {
            InitializeComponent();
            this.Assignment = assignment;
            this.Project = project;
            this.EditMode = true;
            Load();
        }

        private void Load()
        {
            if(!EditMode)
            {
                this.SubtasksViewModels = this.Subtasks.OrderByDescending(s => s.Priority)
                                                       .Select(s => new SubtaskGridViewModel(s))
                                                       .ToList();
            }
            else
            {
                var assignmentLogic = new AssignmentLogic();
                this.Assignment = assignmentLogic.GetAssignmentById(this.Assignment.Id);
                assignmentLogic.Dispose();
                this.Subtasks = this.Assignment.Subtasks.ToList();
                this.SubtasksViewModels = this.Subtasks.OrderByDescending(s => s.Priority)
                                                       .Select(s => new SubtaskGridViewModel(s))
                                                       .ToList();

                this.tb_AssignmentDescription.Text = this.Assignment.Description;
                this.tb_AssignmentName.Text = this.Assignment.Name;
                this.tb_AssignmentPlannedDurationHours.Text = Assignment.DurationPlannedAsTicks.HasValue ? new TimeSpan(this.Assignment.DurationPlannedAsTicks.Value).Hours.ToString() : null;
                this.tb_AssignmentPlannedDurationMinutes.Text = Assignment.DurationPlannedAsTicks.HasValue ? new TimeSpan(this.Assignment.DurationPlannedAsTicks.Value).Minutes.ToString() : null;
                this.dp_AssignmentDue.SelectedDate = this.Assignment.DateDue;
                this.dp_Planned.SelectedDate = this.Assignment.DatePlanned;
            }

            this.lv_Subtasks.ItemsSource = this.SubtasksViewModels;
        }

        #region GUI Events

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

        private void btn_AddAssignment_Click(object sender, RoutedEventArgs e)
        {
            var assignmentLogic = new AssignmentLogic();
            Assignment assignment = null;
            var durationPlanned = ParseDuration();

            if (this.EditMode)
            {
                var param = new AssignmentUpdateParam
                {
                    DateDue = this.dp_AssignmentDue.SelectedDate,
                    DatePlanned = this.dp_Planned.SelectedDate,
                    Description = this.tb_AssignmentDescription.Text,
                    DurationPlannedAsTicks = durationPlanned,
                    Name = this.tb_AssignmentName.Text
                };
                assignmentLogic.UpdateAssignment(this.Assignment.Id, param);
                assignment = assignmentLogic.GetAssignmentById(this.Assignment.Id);
            }
            else
            {
                var param = new AssignmentCreateParam
                {
                    DateDue = this.dp_AssignmentDue.SelectedDate,
                    DatePlanned = this.dp_Planned.SelectedDate,
                    Description = this.tb_AssignmentDescription.Text,
                    DurationPlannedAsTicks = durationPlanned,
                    Name = this.tb_AssignmentName.Text,
                    Project = this.Project
                };

                assignment = assignmentLogic.AddAssignment(param);
            }

            assignmentLogic.SetSubtasksForAssigment(assignment.Id, this.Subtasks);
            assignmentLogic.Dispose();
            this.Close();
        }

        private void btn_DeleteSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedSubtask != null)
            {
                this.Subtasks.Remove(SelectedSubtask);
                if (this.SelectedSubtask.Assignment != null)
                {
                    var assignmentLogic = new AssignmentLogic();
                    assignmentLogic.DeleteSubtask(this.Assignment, this.SelectedSubtask);
                    assignmentLogic.Dispose();
                }
                this.SelectedSubtask = null;
                Load();
            }
        }

        private void btn_EditSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSubtask != null)
            {
                AddSubtask addSubtaskWindow = null;
                if (this.EditMode)
                {
                    addSubtaskWindow = new AddSubtask(this, this.SelectedSubtask.Id);
                }
                else
                {
                    addSubtaskWindow = new AddSubtask(this, this.SelectedSubtask);
                }
                 
                addSubtaskWindow.ShowDialog();
                
                Load();
            }
        }

        private void btn_AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            AddSubtask addSubtaskWindow = null;
            if (this.EditMode)
            {
                addSubtaskWindow = new AddSubtask(this, this.Assignment);
            }
            else
            {
                addSubtaskWindow = new AddSubtask(this);
            }

            addSubtaskWindow.ShowDialog();

            Load();
        }

        private void lv_Subtasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedViewModel = (e.AddedItems[0] as SubtaskGridViewModel);
                if(selectedViewModel.Id != 0)
                {
                    this.SelectedSubtask = this.Subtasks.Single(s => s.Id == selectedViewModel.Id);
                } 
                else
                {
                    this.SelectedSubtask = (this.Subtasks
                        .FirstOrDefault(s => s.Name == selectedViewModel.Name &&
                                s.Description == selectedViewModel.Description));
                }
            }
        }

        private void tb_AssignmentPlannedDuration_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void tb_AssignmentPlannedDuration_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void tb_AssignmentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.btn_AddAssignment.IsEnabled = !string.IsNullOrWhiteSpace(tb_AssignmentName.Text);
        }

        #endregion

        #region Utility

        // https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        public long? ParseDuration()
        {
            long? duration = null;
            if (!string.IsNullOrWhiteSpace(tb_AssignmentPlannedDurationHours.Text) && !string.IsNullOrWhiteSpace(tb_AssignmentPlannedDurationMinutes.Text))
            {
                duration = 0;
                if (!string.IsNullOrWhiteSpace(tb_AssignmentPlannedDurationHours.Text))
                {
                    duration += new TimeSpan(int.Parse(tb_AssignmentPlannedDurationHours.Text), 0 ,0).Ticks;
                }

                if (!string.IsNullOrWhiteSpace(tb_AssignmentPlannedDurationHours.Text))
                {
                    duration += new TimeSpan(0, int.Parse(tb_AssignmentPlannedDurationMinutes.Text), 0).Ticks;
                }
            }
            return duration;
        }

        #endregion

        public List<Subtask> Subtasks { get; set; } = new List<Subtask>();
        public List<SubtaskGridViewModel> SubtasksViewModels { get; set; } = new List<SubtaskGridViewModel>();
        public Subtask SelectedSubtask { get; set; }
        public Assignment Assignment { get; set; }
        public Project Project { get; set; }
        public bool EditMode { get; set; } = false;

        private void lv_Subtasks_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
