using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TakeMyTime.BLL.Logic;
using TakeMyTime.WPF.Assignments;
using TakeMyTime.WPF.Projects;

namespace TakeMyTime.WPF.Entries
{
    /// <summary>
    /// Interaktionslogik für EntryOverview.xaml
    /// </summary>
    public partial class EntryOverview : Page
    {
        public EntryOverview()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            this.LoadProjects();
            this.cb_ProjectFilter.ItemsSource = this.ProjectViewModels;
        }

        private void RefreshEntries()
        {
            this.LoadEntriesWhereFiltersHit();
            this.lv_Entries.ItemsSource = this.FilteredViewModels;
        }

        #region GUI Events

        private void cb_ProjectFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cb_AssignmentFilter.IsEnabled = false;
            this.cb_SubtaskFilter.IsEnabled = false;
            if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
            {
                if (this.SelectedProject != e.AddedItems[0] as ProjectViewModel)
                {
                    this.cb_AssignmentFilter.SelectedItem = null;
                    this.cb_SubtaskFilter.SelectedItem = null;
                }
                this.SelectedProject = e.AddedItems[0] as ProjectViewModel;
                this.cb_AssignmentFilter.IsEnabled = this.SelectedProject != null;
                this.LoadAssignmentsForProject(this.SelectedProject.Id);
                this.cb_AssignmentFilter.ItemsSource = this.AssignmentViewModels;
                this.RefreshEntries();
            }
            else
            {
                this.SelectedProject = null;
            }
        }

        private void cb_AssignmentFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cb_SubtaskFilter.IsEnabled = false;
            if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
            {
                if (this.SelectedAssignment != e.AddedItems[0] as AssignmentViewModel)
                {
                    this.cb_SubtaskFilter.SelectedItem = null;
                }
                this.SelectedAssignment = e.AddedItems[0] as AssignmentViewModel;
                this.cb_SubtaskFilter.IsEnabled = this.SelectedAssignment != null;
                this.LoadSubtasksForAssignment(this.SelectedAssignment.Id);
                this.cb_SubtaskFilter.ItemsSource = this.SubtaskViewModels;
                this.RefreshEntries();
            }
            else
            {
                this.SelectedAssignment = null;
            }
        }

        private void cb_SubtaskFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
            {
                this.SelectedSubtask = e.AddedItems[0] as SubtaskComboBoxViewModel;
                this.RefreshEntries();
            }
            else
            {
                this.SelectedSubtask = null;
            }
        }

        #endregion

        #region Utility

        private void LoadProjects()
        {
            var projectLogic = new ProjectLogic();
            this.ProjectViewModels = projectLogic.GetAllProjects().Where(p => p.ProjectStatus != EnumDefinition.ProjectStatus.Archived)
                .Select(p => new ProjectViewModel(p))
                .ToList();
            projectLogic.Dispose();
        }

        private void LoadAssignmentsForProject(int project_id)
        {
            var assignmentLogic = new AssignmentLogic();
            this.AssignmentViewModels = assignmentLogic.GetAssignmentsByProjectId(project_id)
                .Select(a => new AssignmentViewModel(a))
                .ToList();
            assignmentLogic.Dispose();
        }

        private void LoadSubtasksForAssignment(int assignment_id)
        {
            var subtaskLogic = new SubtaskLogic();
            this.SubtaskViewModels = subtaskLogic.GetByAssignmentId(assignment_id)
                .Select(s => new SubtaskComboBoxViewModel(s))
                .ToList();
            subtaskLogic.Dispose();
        }

        private void LoadEntriesWhereFiltersHit()
        {
            var entryLogic = new EntryLogic();
            this.EntryViewModels = entryLogic.GetAllEntries()
                .Select(e => new EntryViewModel(e))
                .ToList();
            entryLogic.Dispose();

            Func<EntryViewModel, bool> projectCondition = e => e != null;
            Func<EntryViewModel, bool> assignmentCondition = e => e != null;
            Func<EntryViewModel, bool> subtaskCondition = e => e != null;

            if (this.SelectedProject != null)
            {
                projectCondition = e => e.ProjectId == this.SelectedProject.Id;
            }

            if (this.SelectedAssignment != null)
            {
                assignmentCondition = e => e.Subtask.Assignment_Id == this.SelectedAssignment.Id;
            }

            if (this.SelectedSubtask != null)
            {
                subtaskCondition = e => e.Subtask.Id == this.SelectedSubtask.Id;
            }

            this.FilteredViewModels = this.EntryViewModels.Where(projectCondition).ToList();
            this.FilteredViewModels = this.FilteredViewModels.Where(assignmentCondition).ToList();
            this.FilteredViewModels = this.FilteredViewModels.Where(subtaskCondition).ToList();
        }

        private void btn_EditEntry_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Open EntryCreate dialog...
        }

        private void btn_DeleteEntry_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Open confirmation dialog and delete...
        }

        private void lv_Entries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
            {
                this.SelectedEntry = e.AddedItems[0] as EntryViewModel;
                this.btn_EditEntry.IsEnabled = this.SelectedEntry != null;
                this.btn_DeleteEntry.IsEnabled = this.SelectedEntry != null;
            }
        }

        #endregion

        #region Properties

        public List<EntryViewModel> EntryViewModels { get; set; }
        public List<EntryViewModel> FilteredViewModels { get; set; }
        public List<ProjectViewModel> ProjectViewModels { get; set; }
        public List<AssignmentViewModel> AssignmentViewModels { get; set; }
        public List<SubtaskComboBoxViewModel> SubtaskViewModels { get; set; }
        public ProjectViewModel SelectedProject { get; set; }
        public AssignmentViewModel SelectedAssignment { get; set; }
        public SubtaskComboBoxViewModel SelectedSubtask { get; set; }
        public EntryViewModel SelectedEntry { get; set; }

        #endregion
    }
}
