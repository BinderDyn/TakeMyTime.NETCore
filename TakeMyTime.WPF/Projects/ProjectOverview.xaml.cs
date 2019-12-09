using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }

        private void Load()
        {
            var projectLogic = new ProjectLogic();
            var loadedProjects = projectLogic.GetAllProjects();
            this.Projects = new ObservableCollection<Project>(loadedProjects);
        }

        #region GUI Events

        private void btn_NewProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_EditProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_DeleteProject_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Properties

        public ObservableCollection<Project> Projects { get; set; }

        #endregion


    }
}
