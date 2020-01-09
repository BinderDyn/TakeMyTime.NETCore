using Common.Enums;
using System;
using System.Collections.Generic;
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
            this.ProjectViewModels = projectLogic.GetAllProjects()
                .Where(p => p.ProjectStatus == EnumDefinition.ProjectStatus.Active)
                .Select(p => new Projects.ProjectViewModel(p));
            this.lv_Assignments.ItemsSource = this.AssignmentViewModels;
            this.cb_ProjectSelection.ItemsSource = this.ProjectViewModels;
        }

        private void btn_NewAssignment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_EditAssignment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lv_Assignments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btn_SetDone_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_SetAborted_Click(object sender, RoutedEventArgs e)
        {

        }

        public IEnumerable<Projects.ProjectViewModel> ProjectViewModels { get; set; }
        public IEnumerable<AssignmentViewModel> AssignmentViewModels { get; set; }

        private void cb_ProjectSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
