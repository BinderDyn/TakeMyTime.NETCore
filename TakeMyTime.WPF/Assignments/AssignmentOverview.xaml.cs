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
            this.FilteredAssignmentViewModels = this.AssignmentViewModels.ToList();
            this.ProjectViewModels = projectLogic.GetAllProjects()
                .Where(p => p.ProjectStatus == EnumDefinition.ProjectStatus.Active)
                .Select(p => new Projects.ProjectViewModel(p));
            this.lv_Assignments.ItemsSource = this.FilteredAssignmentViewModels;
            this.cb_ProjectSelection.ItemsSource = this.ProjectViewModels;
            this.cb_ProjectSelection.SelectedItem = this.ProjectViewModels.FirstOrDefault();
            this.cb_StatusFilter.SelectedItem = this.cbi_All;
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
        private void cb_ProjectSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                this.FilteredAssignmentViewModels = this.AssignmentViewModels.Where(avm => avm.StatusAsEnum.HasFlag(this.SelectedFilter) &&
                avm.ProjectId == SelectedProject.Id);
            }
        }

        private void cb_StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                this.FilteredAssignmentViewModels = this.AssignmentViewModels;
                return;
            }
            else
            {
                var selectedFilter = GetStatusByItemName(((e.AddedItems[0] as ComboBoxItem).Name));
                this.FilteredAssignmentViewModels = this.AssignmentViewModels.Where(avm => avm.StatusAsEnum.HasFlag(selectedFilter) &&
                avm.ProjectId == SelectedProject.Id);
                this.SelectedFilter = selectedFilter;
            }
        }

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

        public IEnumerable<Projects.ProjectViewModel> ProjectViewModels { get; set; }
        public IEnumerable<AssignmentViewModel> AssignmentViewModels { get; set; }
        public IEnumerable<AssignmentViewModel> FilteredAssignmentViewModels { get; set; }
        public EnumDefinition.AssignmentStatus SelectedFilter { get; set; }
        public Projects.ProjectViewModel SelectedProject { get; set; }
    }
}
