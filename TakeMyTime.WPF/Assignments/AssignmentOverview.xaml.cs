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
            this.PagingManager = new PagingManager<AssignmentViewModel>(15);
            this.LoadProjectViewModels();
            this.Load();
            this.RefreshBindings(1);
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
            assignmentLogic.Dispose();
            if (this.SelectedProject != null)
            {
                this.FilteredAssignmentViewModels = this.AssignmentViewModels
                .Where(av => av.ProjectId == this.SelectedProject?.Id && av.StatusAsEnum == this.SelectedFilter)
                .ToList();
            }
            else
            {
                this.FilteredAssignmentViewModels = this.AssignmentViewModels;
            }
            this.PagingManager.Data = this.FilteredAssignmentViewModels.ToList();
            this.LoadFromAllProjects = true;
            this.lv_Assignments.ItemsSource = this.PagingManager.Page(this.PagingManager.CurrentPage);
            if (this.cb_ProjectSelection.SelectedItem != null) this.cb_ProjectSelection.SelectedItem = this.SelectedProject;
            if (this.cb_StatusFilter.SelectedItem != null) this.cb_StatusFilter.SelectedItem = this.SelectedFilter;
        }

        #region GUI Events

        private void RefreshBindings(int page)
        {
            this.PagingManager.Data = this.FilteredAssignmentViewModels.ToList();
            this.lv_Assignments.ItemsSource = this.PagingManager.Page(page);
            this.btn_CurrentPage.Content = this.PagingManager.CurrentPage;
            this.btn_allPages.Content = this.PagingManager.MaxPage;
            this.btn_PagingForward.IsEnabled = this.PagingManager.CanPageForward;
            this.btn_PagingBack.IsEnabled = this.PagingManager.CanPageBack;
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
            this.RefreshBindings(this.PagingManager.CurrentPage);
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
            RefreshBindings(this.PagingManager.CurrentPage);
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
        public PagingManager<AssignmentViewModel> PagingManager { get; set; }



        #endregion

        
    }
}
