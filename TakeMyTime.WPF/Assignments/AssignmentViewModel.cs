using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace TakeMyTime.WPF.Assignments
{
    public class AssignmentViewModel
    {
        public AssignmentViewModel(DOM.Models.Assignment assignment)
        {
            this.StatusAsEnum = assignment.AssignmentStatus;
            this.Id = assignment.Id;
            this.Name = assignment.Name;
            this.Created = assignment.Created;
            this.Edited = assignment.Edited;
            this.DueDate = assignment.DateDue;
            this.Description = assignment.Description;
            this.ProjectId = assignment.Project_Id.Value;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDueDate { get => this.DueDate.HasValue; }
        public DateTime? DueDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Edited { get; set; }
        public int ProjectId { get; set; }
        public EnumDefinition.AssignmentStatus StatusAsEnum { get; set; }
        public Uri Status { get => GetImageByStatus(this.StatusAsEnum); }

        private Uri GetImageByStatus(EnumDefinition.AssignmentStatus status)
        {
            return status switch
            {
                EnumDefinition.AssignmentStatus.InProgress => new Uri("pack://application:,,,/Images/assignmentActiveIconSmall.png"),
                EnumDefinition.AssignmentStatus.Aborted => new Uri("pack://application:,,,/Images/assignmentAbortedIconSmall.png"),
                EnumDefinition.AssignmentStatus.Future => new Uri("pack://application:,,,/Images/assignmentFutureIconSmall.png"),
                EnumDefinition.AssignmentStatus.Postponed => new Uri("pack://application:,,,/Images/assignmentPostponedIconSmall.png"),
                _ => null
            };
        }
    }
}
