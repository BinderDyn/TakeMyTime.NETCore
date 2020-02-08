using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;
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
using TakeMyTime.Models.Models;
using TakeMyTime.WPF.Projects;
using TakeMyTime.WPF.Utility;

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
            LoadMostProductiveDays();
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
                LoadProductiveDays((e.AddedItems[0] as ProjectViewModel).Id);
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

        public void LoadProductiveDays(int project_id)
        {
            this.ProductiveDays.Clear();
            this.ProductiveEntryDays = this.statisticsLogic.GetProjectProductiveEntries(project_id);
            var mapper = Mappers.Xy<ProductivityViewModel>()
                .X(v => v.XAsDouble)
                .Y(v => v.YAsDouble);
            var series = new LineSeries(mapper);
            series.Values = this.ProductiveEntryDays.AsChartValues();
            series.Fill = Brushes.Transparent;
            this.ProductiveDays.Add(series);
            Formatter = value => value >= 0 ? new System.DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString("d") : "";
            this.cc_Productivity.Series = this.ProductiveDays;
            this.dateAxis.LabelFormatter = Formatter;
            this.cc_Productivity.Visibility = Visibility.Visible;
            DataContext = this;
        }

        public void LoadMostProductiveDays()
        {
            this.MostProductiveWeekdays.Clear();
            this.MostProductiveWeekDaysViewModels = this.statisticsLogic.GetMostProductiveWeekDays();
            var series = new ColumnSeries
            {
                Values = this.MostProductiveWeekDaysViewModels.Select(vm => Math.Round(vm.Value, 2)).AsChartValues()
            };
            series.Fill = Brushes.Orange;
            this.MostProductiveWeekdays.Add(series);
            this.cc_MostProductiveWeekdays.Series = this.MostProductiveWeekdays;

            this.WeekdayLabels = ResolveWeekdayNames(this.MostProductiveWeekDaysViewModels);
            this.weekdays_x_Axis.Labels = this.WeekdayLabels;
            DataContext = this;
        }

        private string[] ResolveWeekdayNames(IEnumerable<MostProductiveWeekDaysViewModel> viewModels)
        {
            var labels = new List<string>();
            foreach (var vm in viewModels)
            {
                labels.Add(ResourceStringManager.GetResourceByKey(vm.Day.ToString()));
            }

            return labels.ToArray();
        }

        #endregion

        #region Properties

        public SeriesCollection ShareOfProjects { get; set; } = new SeriesCollection();
        public SeriesCollection ShareOfAssignments { get; set; } = new SeriesCollection();
        public SeriesCollection ProductiveDays { get; set; } = new SeriesCollection();
        public SeriesCollection MostProductiveWeekdays { get; set; } = new SeriesCollection();
        public IEnumerable<ProjectViewModel> ProjectViewModels { get; set; }
        public Dictionary<string, double> AssignmentShares { get; set; }
        public Dictionary<string, double> ProjectShares { get; set; }
        public IEnumerable<ProductivityViewModel> ProductiveEntryDays { get; set; }
        public Func<double, string> Formatter { get; set; }
        public IEnumerable<MostProductiveWeekDaysViewModel> MostProductiveWeekDaysViewModels  { get; set; }
        public string[] WeekdayLabels { get; set; }

        #endregion


    }
}
