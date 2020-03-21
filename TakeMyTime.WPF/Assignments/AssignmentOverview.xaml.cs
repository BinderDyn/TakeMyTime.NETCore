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
            const int DEFAULT_PAGE_SIZE = 15;
            InitializeComponent();
            this.PagingManager = new PagingManager<AssignmentViewModel>(DEFAULT_PAGE_SIZE);
            this.LoadProjectViewModels();
            this.Load();
            this.RefreshBindings(1);
            this.SetDefaultFilters();
        }

        private void SetDefaultFilters()
        {
            this.cb_ProjectSelection.SelectedItem = this.ProjectViewModels.Single(pvm => pvm.Id == 0);
            this.cb_StatusFilter.SelectedItem = cbi_Future;
        }

        private void LoadProjectViewModels()
        {
            var projectLogic = new ProjectLogic();
            this.ProjectViewModels = projectLogic.GetAllProjects()
                .Where(p => p.ProjectStatus == EnumDefinition.ProjectStatus.Active)
                .Select(p => new Projects.ProjectViewModel(p)).ToList();
            string projectAllSelectItemName = ResourceStringManager.GetResourceByKey("ProjectsAll");
            this.ProjectViewModels.Insert(0, new Projects.ProjectViewModel { Name = projectAllSelectItemName, Id = 0 });
            this.cb_ProjectSelection.ItemsSource = this.ProjectViewModels;
            this.cb_ProjectSelection.SelectedItem = this.ProjectViewModels.Where(p => p.Id == 0);
        }

        private void Load()
        {
            var assignmentLogic = new AssignmentLogic();
            this.AssignmentViewModels = assignmentLogic.GetAllAssignments().Select(a => new AssignmentViewModel(a));
            this.FilteredAssignmentViewModels = PipeThroughFilter(this.AssignmentViewModels);
            assignmentLogic.Dispose();
            this.PagingManager.Data = this.FilteredAssignmentViewModels.ToList();
            this.LoadFromAllProjects = true;
            this.lv_Assignments.ItemsSource = this.PagingManager.Page(this.PagingManager.CurrentPage);
            if (this.cb_ProjectSelection.SelectedItem != null) this.cb_ProjectSelection.SelectedItem = this.SelectedProject;
            if (this.cb_StatusFilter.SelectedItem != null) this.cb_StatusFilter.SelectedItem = this.SelectedFilter;
        }

        #region GUI Events

        private void RefreshBindings(int page)
        {
            this.PagingManager.Data = PipeThroughFilter(this.AssignmentViewModels).ToList();
            this.lv_Assignments.ItemsSource = this.PagingManager.Page(page);
            this.btn_CurrentPage.Content = this.PagingManager.CurrentPage;
            this.btn_allPages.Content = this.PagingManager.MaxPage;
            this.btn_PagingForward.IsEnabled = this.PagingManager.CanPageForward;
            this.btn_PagingBack.IsEnabled = this.PagingManager.CanPageBack;
        }

        private IEnumerable<AssignmentViewModel> PipeThroughFilter(IEnumerable<AssignmentViewModel> viewModels)
        {
            return viewModels.Where(GetFilterCondition()).ToList();
        }

        private Func<AssignmentViewModel, bool> GetFilterCondition()
        {
            Func<AssignmentViewModel, bool> filterByProject = fav => fav.ProjectId == this.SelectedProject.Id;
            Func<AssignmentViewModel, bool> filterByStatus = fav => this.SelectedFilter.HasFlag(fav.StatusAsEnum);
            Func<AssignmentViewModel, bool> finalCondition = fav => filterByStatus(fav); ;
            if (this.SelectedProject != null && this.SelectedProject.Id > 0)
            {
                finalCondition = fav => filterByProject(fav) && filterByStatus(fav);
            }
            return finalCondition;
        }

        private void btn_NewAssignment_Click(object sender, RoutedEventArgs e)
        {
            ShowAddAssignmentDialog(false);
        }

        private void ShowAddAssignmentDialog(bool editMode)
        {
            AddAssignment addAssignmentWindow = null;
            var projectLogic = new ProjectLogic();
            int? projectId = null;
            if (this.SelectedProject != null) projectId = this.SelectedProject.Id;
            if (this.SelectedAssignment != null) projectId = this.SelectedAssignment.ProjectId;
            var project = projectLogic.GetProjectById(projectId.Value);
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
            this.RefreshBindings(this.PagingManager.CurrentPage);
        }

        private void btn_EditAssignment_Click(object sender, RoutedEventArgs e)
        {
            ShowAddAssignmentDialog(true);
        }

        private void lv_Assignments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.SelectedAssignment != null)
            {
                CheckForSubtasksForAssignmentAndCreateIfNecessary(this.SelectedAssignment.Id);
                var addEntryDialog = new AddEntry(this.SelectedAssignment.ProjectId, this.SelectedAssignment.Id);
                addEntryDialog.ShowDialog();
            }
        }

        private void CheckForSubtasksForAssignmentAndCreateIfNecessary(int assignment_id) 
        {
            try
            {
                var subtaskLogic = new SubtaskLogic();
                var existingSubtasks = subtaskLogic.GetByAssignmentId(assignment_id);
                if (existingSubtasks == null || existingSubtasks.Count() == 0)
                {
                    var assignmentLogic = new AssignmentLogic();
                    var assignment = assignmentLogic.GetAssignmentById(assignment_id);
                    var defaultSubtask = new SubtaskCreateViewModel
                    {
                        Name = assignment.Name,
                        Description = assignment.Description,
                        Priority = EnumDefinition.SubtaskPriority.Medium
                    };
                    assignmentLogic.AddSubtask(assignment_id, defaultSubtask);
                    assignmentLogic.Dispose();
                }
                subtaskLogic.Dispose();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                MessageBox.Show(e.Message);
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
                    this.RefreshBindings(this.PagingManager.CurrentPage);
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
                    this.RefreshBindings(this.PagingManager.CurrentPage);
                }
                catch (CannotChangeStatusException ex)
                {
                    Logger.LogException(ex);
                    ShowErrorOnStatusChangeDialog();
                }
            }
        }

        private void cb_ProjectSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this.SelectedProject = e.AddedItems[0] as Projects.ProjectViewModel;
            }

            this.btn_NewAssignment.IsEnabled = this.SelectedProject != null && this.SelectedProject.Id != 0;
            RefreshBindings(this.PagingManager.CurrentPage);
        }

        private void cb_StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || this.SelectedProject == null)
            {
                this.SelectedFilter = EnumDefinition.AssignmentStatus.All;
            }
            else
            {
                var selectedFilter = GetStatusByItemName((e.AddedItems[0] as ComboBoxItem).Name);
                this.SelectedFilter = selectedFilter;
            }
            RefreshBindings(this.PagingManager.CurrentPage);
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
                    this.RefreshBindings(this.PagingManager.CurrentPage);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        private void btn_PagingBack_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshBindings(this.PagingManager.CurrentPage - 1);
        }

        private void btn_PagingForward_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshBindings(this.PagingManager.CurrentPage + 1);
        }

        #endregion

        #region Utility

        private EnumDefinition.AssignmentStatus GetStatusByItemName(string item)
        {
            return item switch
            {
                "cbi_All" => EnumDefinition.AssignmentStatus.All,
                "cbi_Active" => EnumDefinition.AssignmentStatus.InProgress,
                "cbi_Future" => EnumDefinition.AssignmentStatus.Future,
                "cbi_Done" => EnumDefinition.AssignmentStatus.Done,
                "cbi_Aborted" => EnumDefinition.AssignmentStatus.Aborted,
                "cbi_Postponed" => EnumDefinition.AssignmentStatus.Postponed,
                _ => EnumDefinition.AssignmentStatus.None
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
    public PagingManager<AssignmentViewModel> PagingManager { get; set; }



    #endregion


}
}
