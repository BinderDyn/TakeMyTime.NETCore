using Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TakeMyTime.WPF.Resources;
using TakeMyTime.BLL.Logic;

using System.Resources;
using TakeMyTime.WPF.Utility;
using BinderDyn.LoggingUtility;

namespace TakeMyTime.WPF.Projects
{
    /// <summary>
    /// Interaktionslogik für ProjectOverview.xaml
    /// </summary>
    public partial class ProjectOverview : Page
    {
        public ProjectOverview()
        {
            InitializeComponent();
            PagingManager = new PagingManager<ProjectViewModel>(18);
            Load();
            RefreshBindings(1);
        }

        private void RefreshBindings(int page)
        {
            this.lv_Projects.ItemsSource = PagingManager.Page(page);
            this.btn_CurrentPage.Content = this.PagingManager.CurrentPage;
            this.btn_allPages.Content = this.PagingManager.MaxPage;
            this.btn_PagingForward.IsEnabled = this.PagingManager.CanPageForward;
            this.btn_PagingBack.IsEnabled = this.PagingManager.CanPageBack;
        }

        private void Load()
        {
            var projectLogic = new ProjectLogic();
            var loadedProjects = projectLogic.GetAllProjects();
            var viewModels = loadedProjects.Select(lp => new ProjectViewModel(lp));
            this.Projects = new List<ProjectViewModel>(viewModels);
            PagingManager.Data = this.Projects.ToList();
            lv_Projects.ItemsSource = PagingManager.Page(this.PagingManager.CurrentPage);
        }

        #region GUI Events

        private void btn_NewProject_Click(object sender, RoutedEventArgs e)
        {
            ShowAddEditProjectDialog(false);
        }

        private void btn_EditProject_Click(object sender, RoutedEventArgs e)
        {
            ShowAddEditProjectDialog(true);
        }

        private void btn_DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                string.Format("{0}{1}", ResourceStringManager.GetResourceByKey("ConfirmDeleteMessageBoxMessage"), this.SelectedProject.Name),
                ResourceStringManager.GetResourceByKey("ConfirmDeleteMessageBoxTitle"),
                System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var projectLogic = new ProjectLogic();
                try
                {
                    projectLogic.DeleteProject(this.SelectedProject.Id);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    MessageBox.Show(ResourceStringManager.GetResourceByKey("DeleteProjectFailedTitle"),
                                    ResourceStringManager.GetResourceByKey("DeleteProjectFailedMessage"), MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
                projectLogic.Dispose();
                this.lv_Projects.SelectedItem = null;
                this.Load();
                this.RefreshBindings(this.PagingManager.CurrentPage);
            }
        }

        private void ShowAddEditProjectDialog(bool editMode)
        {

            AddProject addProjectWindow = null;
            if (editMode)
            {
                addProjectWindow = new AddProject(this.SelectedProject.Id, this.SelectedProject.Name, this.SelectedProject.Description);
            }
            else
            {
                addProjectWindow = new AddProject();
            }

            addProjectWindow.ShowDialog();
            this.Load();
            this.RefreshBindings(this.PagingManager.CurrentPage);
        }

        private void lv_Projects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0 && e.AddedItems[0] != null)
            {
                this.SelectedProject = e.AddedItems[0] as ProjectViewModel;
            }
            else
            {
                this.SelectedProject = null;
            }

            ToggleButtons();
        }

        private void ToggleButtons()
        {
            bool canBeEnabled = this.SelectedProject != null;
            bool isArchivedProject = this.SelectedProject?.Status == EnumDefinition.ProjectStatus.Archived;

            this.btn_EditProject.IsEnabled = canBeEnabled && !isArchivedProject;
            this.btn_DeleteProject.IsEnabled = canBeEnabled && isArchivedProject;
            this.btn_ToggleStatus.IsEnabled = canBeEnabled;
        }

        private void btn_ToggleStatus_Click(object sender, RoutedEventArgs e)
        {
            var projectLogic = new ProjectLogic();
            projectLogic.ToggleProjectStatus(this.SelectedProject.Id);
            projectLogic.Dispose();
            this.Load();
        }

        private void btn_PagingForward_Click(object sender, RoutedEventArgs e)
        {
            RefreshBindings(this.PagingManager.CurrentPage + 1);
        }

        private void btn_PagingBack_Click(object sender, RoutedEventArgs e)
        {
            RefreshBindings(this.PagingManager.CurrentPage - 1);
        }

        #endregion

        #region Properties

        public IList<ProjectViewModel> Projects { get; set; }
        public ProjectViewModel SelectedProject { get; set; }
        public PagingManager<ProjectViewModel> PagingManager { get; set; }

        #endregion

        
    }
}
