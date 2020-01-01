using BinderDynamics.TakeMyTime.Biz.ViewModels;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DAL.uow;

namespace TakeMyTime.BLL.Logic
{
    public class StatisticsLogic
    {
        private UnitOfWork uow = new UnitOfWork();

        #region Project

        private IEnumerable<StatisticsProjectGraphViewModel> RetrieveGraphData(int projectId)
        {
            var entries = this.uow.Entries.Find(e => e.Project_Id == projectId).ToList();
            var graphData = new List<StatisticsProjectGraphViewModel>();

            foreach (var e in entries)
            {
                var point = new StatisticsProjectGraphViewModel(e.Date, e.DurationAsTicks);
                graphData.Add(point);
            }
            return graphData;
        }

        private TimeSpan CalculateAverageEntryLength(int projectId)
        {
            TimeSpan averageValue = TimeSpan.MinValue;
            var entries = this.uow.Entries.Find(e => e.Project_Id == projectId).ToList();
            if (entries != null && entries.Any())
            {
                long ticksTotal = entries.Sum(e => e.DurationAsTicks.HasValue ? e.DurationAsTicks.Value : 0); //Complete Duration
                averageValue = ticksTotal > 0 ? new TimeSpan(ticksTotal / entries.Count()) : new TimeSpan();
            }
            return averageValue;
        }

        public StatisticsProjectViewModel RetrieveProjectStatistics(int projectId)
        {
            var project = uow.Projects.Get(projectId);
            var projectStatistics = new StatisticsProjectViewModel();
            if (project != null)
            {
                projectStatistics = new StatisticsProjectViewModel
                {
                    AverageEntryLength = CalculateAverageEntryLength(projectId),
                    GraphPoints = RetrieveGraphData(projectId),
                    Created = project.Created,
                    //LastWorkedOn = project.Entries.OrderByDescending(e => e.Created).FirstOrDefault()?.Date.Value ?? DateTime.Now,
                    ProjectName = project.Name
                };
            }
            return projectStatistics;
        }

        #endregion

        #region Assignments

        public StatisticsAssignmentViewModel RetrieveAssignmentStatistics(int projectId)
        {
            var points = RetrieveAssignmentGraphData(projectId);
            decimal deadlineEfficiency = CalculateDeadlineEfficiency(projectId);
            decimal predictionAccuracy = CalculatePredictionAccuracy(projectId);
            string predictionAccuracyAbsolute = string.Format("{0} / {1} Assignment workloads predicted correctly", GetCorrectPredictions(projectId), GetDoneAssignments(projectId).Count());
            int assignmentsDone = GetDoneAssignments(projectId).Count();
            int assignmentsAborted = GetAbortedAssignments(projectId).Count();
            var viewModel = new StatisticsAssignmentViewModel(points, deadlineEfficiency, assignmentsDone, assignmentsAborted, predictionAccuracy, predictionAccuracyAbsolute);
            return viewModel;
        }

        private IEnumerable<StatisticsAssignmentGraphViewModel> RetrieveAssignmentGraphData(int projectId)
        {
            var assignments = GetDoneAssignments(projectId);
            var points = new List<StatisticsAssignmentGraphViewModel>();
            foreach (var ass in assignments)
            {
                points.Add(new StatisticsAssignmentGraphViewModel(ass.Name, ass.Entries.Sum(e => e.DurationAsTicks)));
            }
            return points.OrderByDescending(p => p.WorkingTime).Take(10);
        }

        private IEnumerable<DOM.Models.Assignment> GetDoneAssignments(int projectId)
        {
            return uow.Assignments.Find(a => a.Project_Id == projectId && a.AssignmentStatus == EnumDefinition.AssignmentStatus.Done).ToList();
        }

        private IEnumerable<DOM.Models.Assignment> GetAbortedAssignments(int projectId)
        {
            return uow.Assignments.Find(a => a.Project_Id == projectId && a.AssignmentStatus == EnumDefinition.AssignmentStatus.Aborted).ToList();
        }

        private decimal CalculateDeadlineEfficiency(int projectId)
        {
            var assignments = GetDoneAssignments(projectId);
            var assignmentsFinishedInTime = assignments
                .Where(a => a.Edited <= a.DateDue && a.AssignmentStatus == EnumDefinition.AssignmentStatus.Done)
                .Count();
            return (decimal)assignments.Count() != 0 ? (decimal)assignmentsFinishedInTime / (decimal)assignments.Count() : 0;
        }

        private decimal CalculatePredictionAccuracy(int projectId)
        {
            var assignments = GetDoneAssignments(projectId);
            return (decimal)assignments.Count() != 0 ? (decimal)assignments.Where(a => a.Entries.Sum(e => e.DurationAsTicks) <= a.DurationPlannedAsTicks).Count() / (decimal)assignments.Count() : 0;
        }

        private int GetCorrectPredictions(int projectId)
        {
            return GetDoneAssignments(projectId).Where(a => a.DurationPlannedAsTicks >= a.Entries.Sum(e => e.DurationAsTicks)).Count();
        }
        
        //ToDo: Book data

        #endregion

    }
}
