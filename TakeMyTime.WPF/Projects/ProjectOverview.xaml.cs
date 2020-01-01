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
using TakeMyTime.DOM.Models;
using System.Resources;
using TakeMyTime.WPF.Utility;

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
            Load();
        }

        private void Load()
        {
            var projectLogic = new ProjectLogic();
            var loadedProjects = projectLogic.GetAllProjects();
            var viewModels = loadedProjects.Select(lp => new ProjectViewModel(lp));
            this.Projects = new ObservableCollection<ProjectViewModel>(viewModels);
            lv_Projects.ItemsSource = this.Projects;
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
                projectLogic.DeleteProject(this.SelectedProject.Id);
                projectLogic.Dispose();
                this.lv_Projects.SelectedItem = null;
                this.Load();
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

        #endregion

        #region Properties

        public ObservableCollection<ProjectViewModel> Projects { get; set; }
        public ProjectViewModel SelectedProject { get; set; }

        #endregion

    }
}
