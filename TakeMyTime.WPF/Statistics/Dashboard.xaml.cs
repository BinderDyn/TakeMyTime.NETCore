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

namespace TakeMyTime.WPF.Statistics
{
    /// <summary>
    /// Interaktionslogik für Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        StatisticsLogic statisticsLogic;

        public Dashboard()
        {
            InitializeComponent();
            this.statisticsLogic = new StatisticsLogic();
            Load();
        }

        private void Load()
        {
            LoadSharesOfProjects();
        }

        #region GUI Events

        #endregion

        #region Charts

        public void LoadSharesOfProjects()
        {
            this.ProjectShareTuples = this.statisticsLogic.GetProjectShares();
            this.ShareOfProjects = new SeriesCollection();
            foreach (var tuple in this.ProjectShareTuples)
            {
                this.ShareOfProjects.Add(new PieSeries
                {
                    Values = new ChartValues<double> { tuple.Item3 },
                    Title = tuple.Item2,
                    ToolTip = tuple.Item1.ToString()
                });
            }

            this.pc_ProjectShare.Series = this.ShareOfProjects;
        }

        public void LoadSharesOfAssignments(int project_id)
        {

        }

        #endregion

        #region Properties

        public SeriesCollection ShareOfProjects { get; set; }
        public SeriesCollection ShareOfAssignments { get; set; }
        public IEnumerable<Tuple<int, string, double>> AssignmentShareTuples { get; set; }
        public IEnumerable<Tuple<int, string, double>> ProjectShareTuples { get; set; }

        #endregion

        private void pc_ProjectShare_DataClick(object sender, ChartPoint chartPoint)
        {
        }
    }
}
