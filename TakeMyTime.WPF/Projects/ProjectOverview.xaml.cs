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
using TakeMyTime.Biz.Logic;
using TakeMyTime.DOM.Models;

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
        }

        #region GUI Events

        private void btn_NewProject_Click(object sender, RoutedEventArgs e)
        {
            ToggleFrameVisibility();
        }

        private void btn_EditProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_DeleteProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleFrameVisibility()
        {
            this.b_Toolbar.Visibility = Visibility.Collapsed;
            this.lv_Projects.Visibility = Visibility.Collapsed;
            this.fr_formDisplay.Navigate(new AddProject());
        }

        #endregion

        #region Properties

        public ObservableCollection<ProjectViewModel> Projects { get; set; }

        #endregion


    }
}
