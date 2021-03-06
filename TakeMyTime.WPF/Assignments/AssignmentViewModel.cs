﻿using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.Models.Models;
using TakeMyTime.WPF.Utility;

namespace TakeMyTime.WPF.Assignments
{
    public class AssignmentViewModel
    {
        public AssignmentViewModel()
        {

        }

        public AssignmentViewModel(Assignment assignment)
        {
            this.StatusAsEnum = assignment.AssignmentStatus;
            this.Id = assignment.Id;
            this.Name = assignment.Name;
            this.Created = assignment.Created;
            this.Edited = assignment.Edited;
            this.DueDate = assignment.DateDue;
            this.Description = assignment.Description;
            this.ProjectId = assignment.Project_Id.Value;
            this.Planned = assignment.DatePlanned;
            this.HasSubtasks = assignment.Subtasks.Count() > 0;
            this.DurationPlanned = assignment.DurationPlannedAsTicks.HasValue ? TimeSpan.FromTicks(assignment.DurationPlannedAsTicks.Value) : (TimeSpan?)null;

            this.StatusImage = GetImageByStatus(assignment.AssignmentStatus);
            this.StatusTooltip = GetStatusString(assignment.AssignmentStatus);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDueDate { get => this.DueDate.HasValue; }
        public DateTime? DueDate { get; set; }
        public string DueDateAsString { get => DateTimeCultureConverter.ConvertToLocalDateTimeFormat(this.DueDate); }
        public DateTime Created { get; set; }
        public string CreatedAsString { get => DateTimeCultureConverter.ConvertToLocalDateTimeFormat(this.Created); }
        public DateTime? Edited { get; set; }
        public string EditedAsString { get => DateTimeCultureConverter.ConvertToLocalDateTimeFormat(this.Edited); }
        public DateTime? Planned { get; set; }
        public string PlannedAsString { get => DateTimeCultureConverter.GetCalendarWeek(this.Planned); }
        public TimeSpan? DurationPlanned { get; set; }
        public string DurationPlannedAsString { get => this.DurationPlanned.HasValue ? this.DurationPlanned.Value.ToString(@"hh\:mm") : "-"; }
        public int ProjectId { get; set; }
        public EnumDefinition.AssignmentStatus StatusAsEnum { get; set; }
        public Uri StatusImage { get; private set; }
        public string StatusTooltip { get; private set; }

        

        public bool HasSubtasks { get; private set; }

        private Uri GetImageByStatus(EnumDefinition.AssignmentStatus status)
        {
            return status switch
            {
                EnumDefinition.AssignmentStatus.InProgress => new Uri("pack://application:,,,/Images/assignmentActiveIconSmall.png"),
                EnumDefinition.AssignmentStatus.Future => new Uri("pack://application:,,,/Images/assignmentFutureIconSmall.png"),
                EnumDefinition.AssignmentStatus.None => new Uri("pack://application:,,,/Images/assignmentActiveIconSmall.png"),
                EnumDefinition.AssignmentStatus.Aborted => new Uri("pack://application:,,,/Images/assignmentAbortedIconSmall.png"),
                EnumDefinition.AssignmentStatus.Postponed => new Uri("pack://application:,,,/Images/assignmentPostponedIconSmall.png"),
                EnumDefinition.AssignmentStatus.Done => new Uri("pack://application:,,,/Images/assignmentDoneIconSmall.png"),
                _ => new Uri("pack://application:,,,/Images/assignmentActiveIconSmall.png")
            };
        }

        private string GetStatusString(EnumDefinition.AssignmentStatus statusAsEnum)
        {
            return statusAsEnum switch
            {
                EnumDefinition.AssignmentStatus.InProgress => Resources.Shared.AssignmentActive,
                EnumDefinition.AssignmentStatus.Future => Resources.Shared.AssignmentFuture,
                EnumDefinition.AssignmentStatus.None => Resources.Shared.AssignmentFuture,
                EnumDefinition.AssignmentStatus.Aborted => Resources.Shared.AssignmentAborted,
                EnumDefinition.AssignmentStatus.Postponed => Resources.Shared.AssignmentPostponed,
                _ => null
            };
        }
    }
}
