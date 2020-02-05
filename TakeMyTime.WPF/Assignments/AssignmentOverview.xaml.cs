using BinderDyn.LoggingUtility;
using Common;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using TakeMyTime.Common.Exceptions;
using TakeMyTime.WPF.Entries;
using TakeMyTime.WPF.Subtasks;
using TakeMyTime.WPF.Utility;

namespace TakeMyTime.WPF.Assignments
{
    /// <summary>
    /// Interaktionslogik für AssignmentOverview.xaml
    /// </summary>
    public partial class AssignmentOverview : Page
    {
        public AssignmentOverview()
        {
            InitializeComponent();
            this.Load();
        }

        private void Load()
        {
            var assignmentLogic = new AssignmentLogic();
            var projectLogic = new ProjectLogic();
            this.AssignmentViewModels = assignmentLogic.GetAllAssignments().Select(a => new AssignmentViewModel(a));
            this.FilteredAssignmentViewModels = this.AssignmentViewModels.ToList();
            this.ProjectViewModels = projectLogic.GetAllProjects()
                .Where(p => p.ProjectStatus == EnumDefinition.ProjectStatus.Active)
                .Select(p => new Projects.ProjectViewModel(p)).ToList();
            string projectAllSelectItemName = ResourceStringManager.GetResourceByKey("ProjectsAll");
            this.ProjectViewModels.Insert(0, new Projects.ProjectViewModel { Name = projectAllSelectItemName, Id = 0 });
            this.LoadFromAllProjects = true;
            this.lv_Assignments.ItemsSource = this.FilteredAssignmentViewModels;
            this.cb_ProjectSelection.ItemsSource = this.ProjectViewModels;
            this.cb_ProjectSelection.SelectedItem = this.ProjectViewModels.FirstOrDefault(p => p.Id == 0);
            this.cb_StatusFilter.SelectedItem = this.cbi_All;
        }

        #region GUI Events

        private void RefreshBindings()
        {
            this.lv_Assignments.ItemsSource = this.FilteredAssignmentViewModels;
        }

        private void btn_NewAssignment_Click(object sender, RoutedEventArgs e)
        {
            ShowAddAssignmentDialog(false);
        }

        private void ShowAddAssignmentDialog(bool editMode)
        {
            AddAssignment addAssignmentWindow = null;
            var projectLogic = new ProjectLogic();
            var project = projectLogic.GetProjectById(this.SelectedProject.Id);
            projectLogic.Dispose();

            if (editMode)
            {
                var assignmentLogic = new AssignmentLogic();
                var selectedAssignment = assignmentLogic.GetAssignmentById(this.SelectedAssignment.Id);
                assignmentLogic.Dispose();
                addAssignmentWindow = new AddAssignment(selectedAssignment.Id, selectedAssignment.Project_Id.Value);
            }
            else
            {
                addAssignmentWindow = new AddAssignment(project.Id);
            }

            addAssignmentWindow.ShowDialog();
            this.Load();
        }

        private void btn_EditAssignment_Click(object sender, RoutedEventArgs e)
        {
            ShowAddAssignmentDialog(true);
        }

        private void lv_Assignments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.SelectedAssignment != null && this.SelectedAssignment.HasSubtasks)
            {
                var addEntryDialog = new AddEntry(this.SelectedAssignment.ProjectId, this.SelectedAssignment.Id);
                addEntryDialog.ShowDialog();
            }
        }

        private void btn_SetDone_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log(string.Format("{0}.btn_SetDone_Click", GetType().FullName));
            if (this.SelectedAssignment != null)
            {
                try
                {
                    var assignmentLogic = new AssignmentLogic();
                    assignmentLogic.SetDone(this.SelectedAssignment.Id);
                    assignmentLogic.Dispose();
                    this.Load();
                }
                catch (CannotChangeStatusException ex)
                {
                    Logger.LogException(ex);
                    ShowErrorOnStatusChangeDialog();
                }
            }
        }

        private void btn_SetAborted_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log(string.Format("{0}.btn_SetAborted_Click", GetType().FullName));
            if (this.SelectedAssignment != null)
            {
                try
                {
                    var assignmentLogic = new AssignmentLogic();
                    assignmentLogic.SetAborted(this.SelectedAssignment.Id);
                    assignmentLogic.Dispose();
                    this.Load();
                }
                catch (CannotChangeStatusException ex)
                {
                    Logger.LogException(ex);
                    ShowErrorOnStatusChangeDialog();
                }
            }
        }

        [Refactor]
        private void cb_ProjectSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this.SelectedProject = e.AddedItems[0] as Projects.ProjectViewModel;
                if (this.SelectedProject.Id != 0 && this.SelectedFilter != EnumDefinition.AssignmentStatus.Default)
                {
                    this.FilteredAssignmentViewModels = this.FilteredAssignmentViewModels
                        .Where(avm => avm.ProjectId == SelectedProject.Id && avm.StatusAsEnum == this.SelectedFilter);
                    this.LoadFromAllProjects = false;
                }
                else if (this.SelectedProject.Id == 0 && this.SelectedFilter != EnumDefinition.AssignmentStatus.Default)
                {
                    this.FilteredAssignmentViewModels = this.AssignmentViewModels.Where(avm => avm.StatusAsEnum == this.SelectedFilter); ;
                    this.LoadFromAllProjects = true;
                }
                else if (this.SelectedProject.Id != 0 && this.SelectedFilter == EnumDefinition.AssignmentStatus.Default)
                {
                    this.FilteredAssignmentViewModels = this.AssignmentViewModels.Where(avm => avm.ProjectId == this.SelectedProject.Id);
                }
                else if (this.SelectedProject.Id == 0 && this.SelectedFilter == EnumDefinition.AssignmentStatus.Default)
                {
                    this.FilteredAssignmentViewModels = this.AssignmentViewModels;
                }
            }

            this.btn_NewAssignment.IsEnabled = this.SelectedProject != null && this.SelectedProject.Id != 0;
            RefreshBindings();
        }

        [Refactor]
        private void cb_StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || this.SelectedProject == null)
            {
                this.FilteredAssignmentViewModels = this.AssignmentViewModels;
                return;
            }
            else
            {
                var selectedFilter = GetStatusByItemName(((e.AddedItems[0] as ComboBoxItem).Name));
                if (selectedFilter != EnumDefinition.AssignmentStatus.Default && SelectedProject.Id != 0)
                {
                    this.FilteredAssignmentViewModels = this.AssignmentViewModels
                                        .Where(avm => avm.StatusAsEnum == selectedFilter &&
                                    avm.ProjectId == SelectedProject.Id);
                }
                else if (selectedFilter == EnumDefinition.AssignmentStatus.Default && !this.LoadFromAllProjects)
                {
                    this.FilteredAssignmentViewModels = this.AssignmentViewModels
                                        .Where(avm => avm.ProjectId == SelectedProject.Id);
                }
                else if (selectedFilter == EnumDefinition.AssignmentStatus.Default && this.LoadFromAllProjects)
                {
                    this.FilteredAssignmentViewModels = this.AssignmentViewModels;
                }
                else
                {
                    this.FilteredAssignmentViewModels = this.AssignmentViewModels
                        .Where(avm => avm.StatusAsEnum == selectedFilter);
                }
                this.SelectedFilter = selectedFilter;
            }
            RefreshBindings();
        }

        private void lv_Assignments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool selectionNotNull = e.AddedItems.Count > 0 && e.AddedItems[0] != null;
            if (selectionNotNull)
            {
                this.SelectedAssignment = e.AddedItems[0] as AssignmentViewModel;
            }
            this.btn_EditAssignment.IsEnabled = selectionNotNull;
            this.btn_EditSubtasks.IsEnabled = selectionNotNull;
            this.btn_DeleteAssignment.IsEnabled = selectionNotNull;
            this.btn_SetAborted.IsEnabled = selectionNotNull;
            this.btn_SetDone.IsEnabled = selectionNotNull;
        }

        private void btn_EditSubtasks_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedAssignment != null)
            {
                var editSubtaskDialog = new SubtaskList(this.SelectedAssignment.Id);
                editSubtaskDialog.ShowDialog();
            }
        }

        private void btn_DeleteAssignment_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedAssignment != null)
            {
                try
                {
                    var assignmentLogic = new AssignmentLogic();
                    assignmentLogic.DeleteAssignment(this.SelectedAssignment.Id);
                    assignmentLogic.Dispose();
                    this.Load();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                } 
            }
        }

        #endregion

        #region Utility

        private EnumDefinition.AssignmentStatus GetStatusByItemName(string itemName)
        {
            return itemName switch
            {
                "cbi_All" => EnumDefinition.AssignmentStatus.Default,
                "cbi_Active" => EnumDefinition.AssignmentStatus.InProgress,
                "cbi_Future" => EnumDefinition.AssignmentStatus.Future,
                "cbi_Done" => EnumDefinition.AssignmentStatus.Done,
                "cbi_Aborted" => EnumDefinition.AssignmentStatus.Aborted,
                "cbi_Postponed" => EnumDefinition.AssignmentStatus.Postponed,
                _ => EnumDefinition.AssignmentStatus.Default
            };
        }

        private void ShowErrorOnStatusChangeDialog()
        {
            string title = ResourceStringManager.GetResourceByKey("CannotSetDoneOrAbortedErrorTitle");
            string message = ResourceStringManager.GetResourceByKey("CannotSetDoneErrorMessage");
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region Properties

        public List<Projects.ProjectViewModel> ProjectViewModels { get; set; }
        public IEnumerable<AssignmentViewModel> AssignmentViewModels { get; set; }
        public IEnumerable<AssignmentViewModel> FilteredAssignmentViewModels { get; set; }
        public EnumDefinition.AssignmentStatus SelectedFilter { get; set; }
        public Projects.ProjectViewModel SelectedProject { get; set; }
        public AssignmentViewModel SelectedAssignment { get; set; }
        public bool LoadFromAllProjects { get; set; }


        #endregion


    }
}
