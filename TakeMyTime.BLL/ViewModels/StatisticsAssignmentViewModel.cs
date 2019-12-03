using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinderDynamics.TakeMyTime.Biz.ViewModels;

namespace BinderDynamics.TakeMyTime.Biz.ViewModels
{
    public class StatisticsAssignmentViewModel
    {
        public StatisticsAssignmentViewModel(IEnumerable<StatisticsAssignmentGraphViewModel> points, 
            decimal deadlineEfficiency,
            int assignmentsDone,
            int assignmentsAborted,
            decimal predictionAccuracy,
            string predictionAccuracyAbsolute)
        {
            InitBase(deadlineEfficiency, assignmentsDone, assignmentsAborted, predictionAccuracy, predictionAccuracyAbsolute);
            this.Points = points;
        }

        public StatisticsAssignmentViewModel(IEnumerable<StatisticsAssignmentGraphViewModel> points,
            decimal deadlineEfficiency,
            int assignmentsDone,
            int assignmentsAborted,
            decimal predictionAccuracy,
            string predictionAccuracyAbsolute,
            decimal avgWph,
            decimal avgPph,
            decimal avgWordOnPages)
        {
            InitBase(deadlineEfficiency, assignmentsDone, assignmentsAborted, predictionAccuracy, predictionAccuracyAbsolute);
            InitWithBook(avgWph, avgPph, avgWordOnPages);
            this.Points = points;
        }

        private void InitBase(decimal deadlineEfficiency, int assignmentsDone, int assignmentsAborted, decimal predictionAccuracy, string predictionAccuracyAbsolute)
        {
            this.DeadlineEfficiency = string.Format("{0}%", decimal.Round(deadlineEfficiency * 100, 2, MidpointRounding.AwayFromZero));
            this.PredictionAccuracy = string.Format("{0}%", decimal.Round(predictionAccuracy * 100, 2, MidpointRounding.AwayFromZero));
            this.AssignmentsAborted = assignmentsAborted.ToString();
            this.AssignmentsDone = assignmentsDone.ToString();
            this.PredictionAccuracyAbsolute = predictionAccuracyAbsolute;
            this.Points = new List<StatisticsAssignmentGraphViewModel>();
        }

        private void InitWithBook(decimal avgWph, decimal avgPph, decimal avgWordOnPages)
        {
            this.AvgWph = string.Format("{0}%", avgWph);
            this.AvgPph = string.Format("{0}%", avgPph);
            this.AvgWordsOnPages = string.Format("{0}%", avgWordOnPages);
        }

        public IEnumerable<StatisticsAssignmentGraphViewModel> Points { get; set; }
        public string DeadlineEfficiency { get; private set; }
        public string AssignmentsDone { get; private set; }
        public string AssignmentsAborted { get; private set; }
        public string PredictionAccuracy { get; private set; }
        public string AvgWph { get; private set; }
        public string AvgPph { get; private set; }
        public string AvgWordsOnPages { get; private set; }
        public string PredictionAccuracyAbsolute { get; private set; }
    }
}
