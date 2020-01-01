using BinderDyn.LoggingUtility;
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
using System.Windows.Shapes;
using TakeMyTime.BLL.Logic;

namespace TakeMyTime.WPF.Projects
{
    /// <summary>
    /// Interaktionslogik für AddProject.xaml
    /// </summary>
    public partial class AddProject : Window
    {
        string className;
        bool editMode = false;
        int projectId;

        public AddProject()
        {
            this.className = this.GetType().FullName;
            InitializeComponent();
            LoadProjectTypes();
        }

        // Called on edit
        public AddProject(int id, string name, string description)
        {
            this.className = this.GetType().FullName;
            InitializeComponent();
            LoadProjectTypes();
            this.tb_projectDescription.Text = description;
            this.tb_projectDesignation.Text = name;
            this.projectId = id;
            this.editMode = true;
            this.cb_ProjectTypes.IsEnabled = false;
        }

        private void LoadProjectTypes()
        {
            try
            {
                Logger.Log(string.Format("{0}.LoadProjectTypes()", className));
                var bll = new ProjectTypeLogic();
                this.ProjectTypes = new ObservableCollection<ProjectTypeViewModel>(bll.GetProjectTypes().Select(pt => new ProjectTypeViewModel(pt)));
                cb_ProjectTypes.ItemsSource = this.ProjectTypes;
                bll.Dispose();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        #region GUI EVENTS

        private void sp_Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void btn_AddProject_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedProjectType != null && this.CanCreateProject && !editMode)
            {
                var bllProjectTypes = new ProjectTypeLogic();
                var bllProjects = new ProjectLogic();
                var projectType = bllProjectTypes.GetProjectType(this.SelectedProjectType.Id);
                var viewModel = new ProjectCreateViewModel
                {
                    Description = tb_projectDescription.Text,
                    Name = tb_projectDesignation.Text,
                    ProjectType = projectType
                };

                bllProjects.InsertProject(viewModel);
                bllProjects.Dispose();
                bllProjectTypes.Dispose();
                this.Close();
            }
            else if (this.SelectedProjectType != null && this.CanCreateProject && editMode)
            {
                var bllProjectTypes = new ProjectTypeLogic();
                var bllProjects = new ProjectLogic();
                var projectType = bllProjectTypes.GetProjectType(this.SelectedProjectType.Id);
                var viewModel = new ProjectUpdateViewModel
                {
                    Description = this.tb_projectDescription.Text,
                    Name = this.tb_projectDesignation.Text
                };

                bllProjects.UpdateProject(viewModel, projectId);
                bllProjects.Dispose();
                bllProjectTypes.Dispose();
                this.Close();
            }
        }

        private void cb_ProjectTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = e.AddedItems[0];
            if (selection != null)
            {
                this.SelectedProjectType = (ProjectTypeViewModel)selection;
            }
        }

        #endregion

        public ObservableCollection<ProjectTypeViewModel> ProjectTypes { get; set; }
        public ProjectTypeViewModel SelectedProjectType { get; set; }
        public bool CanCreateProject { get => !string.IsNullOrWhiteSpace(tb_projectDesignation.Text); }

        
    }
}
