using LiveCharts;
using LiveCharts.Wpf;
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
using TakeMyTime.WPF.Projects;

namespace TakeMyTime.WPF.Statistics
{
    /// <summary>
    /// Interaktionslogik für Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        StatisticsLogic statisticsLogic;
        ProjectLogic projectLogic;

        public Dashboard()
        {
            InitializeComponent();
            this.statisticsLogic = new StatisticsLogic();
            this.projectLogic = new ProjectLogic();
            Load();
        }

        private void Load()
        {
            LoadSharesOfProjects();
            this.ProjectViewModels = this.projectLogic.GetAllActiveProjects()
                .ToList()
                .Select(p => new ProjectViewModel(p));
            this.cb_ProjectFilter.ItemsSource = this.ProjectViewModels;
            this.cb_ProjectFilter.SelectedItem = this.ProjectViewModels.FirstOrDefault();
        }

        #region GUI Events

        private void cb_ProjectFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
            {
                LoadSharesOfAssignments((e.AddedItems[0] as ProjectViewModel).Id);
            }
        }

        #endregion

        #region Charts

        public void LoadSharesOfProjects()
        {
            this.ProjectShares = this.statisticsLogic.GetProjectShares();
            this.ShareOfProjects = new SeriesCollection();
            foreach (var entry in this.ProjectShares)
            {
                this.ShareOfProjects.Add(new PieSeries
                {
                    Values = new ChartValues<double> { entry.Value },
                    Title = entry.Key
                });
            }

            this.pc_ProjectShare.Series = this.ShareOfProjects;
        }

        public void LoadSharesOfAssignments(int project_id)
        {
            this.AssignmentShares = this.statisticsLogic.GetAssignmentSharesOfProject(project_id);
            this.ShareOfAssignments = new SeriesCollection();
            foreach (var entry in this.AssignmentShares)
            {
                this.ShareOfAssignments.Add(new PieSeries
                {
                    Values = new ChartValues<double> { entry.Value },
                    Title = entry.Key
                });
            }

            this.pc_AssignmentShare.Series = this.ShareOfAssignments;
            this.pc_AssignmentShare.Visibility = Visibility.Visible;
        }

        #endregion

        #region Properties

        public SeriesCollection ShareOfProjects { get; set; }
        public SeriesCollection ShareOfAssignments { get; set; }
        public IEnumerable<ProjectViewModel> ProjectViewModels { get; set; }
        public Dictionary<string, double> AssignmentShares { get; set; }
        public Dictionary<string, double> ProjectShares { get; set; }

        #endregion

        
    }
}
