using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.Common.Exceptions;
using TakeMyTime.DOM.Interfaces;
using TakeMyTime.Models.Models;
using static Common.Enums.EnumDefinition;

namespace TakeMyTime.Models.Models
{
    public class Assignment : Entity<Assignment>
    {
        public static Assignment Create(ICreateParam param)
        {
            Assignment assignment = new Assignment();
            assignment.Init(param.Name, param.Description, param.DatePlanned, param.DateDue, param.DurationPlannedAsTicks, param.Project);
            return assignment;
        }

        public void Init(string name,
            string description,
            DateTime? datePlanned,
            DateTime? dateDue,
            long? durationPlannedAsTicks,
            Project project)
        {
            this.Name = name;
            this.Description = description;
            this.DatePlanned = datePlanned;
            this.DateDue = dateDue;
            this.Project_Id = project.Id;
            this.DurationPlannedAsTicks = durationPlannedAsTicks;
            this.Subtasks = new HashSet<Subtask>();
            this.AssignmentStatus = AssignmentStatus.Future;
            this.SetCreated();
        }

        public void Update(IUpdateParam param)
        {
            this.Name = param.Name;
            this.Description = param.Description;
            this.DateDue = param.DateDue;
            this.DatePlanned = param.DatePlanned;
            this.DurationPlannedAsTicks = param.DurationPlannedAsTicks;
            this.SetEdited();
        }

        public void UpdateStatus(AssignmentStatus status)
        {
            if (CanSetStatus(status))
            {
                this.AssignmentStatus = status;
                this.SetEdited();
            }
        }

        private bool CanSetStatus(AssignmentStatus status)
        {
            if (this.AssignmentStatus == AssignmentStatus.Aborted) throw new CannotChangeStatusException("Cannot change status if already set to aborted");
            if (this.AssignmentStatus == AssignmentStatus.Done) throw new CannotChangeStatusException("Cannot change status if already set to done");
            return true;
        }

        public bool CanDelete()
        {
            return !this.Subtasks.Any();
        }

        /// <summary>
        /// Removes all subtasks from assignment. The subtasks should be consequently deleted from the database to avoid garbage
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Subtask> ClearSubtasks()
        {
            var subtasks = this.Subtasks.ToArray();
            this.Subtasks.Clear();
            return subtasks;
        }

        public bool IsOvertime()
        {
            bool isOvertime = false;
            if (this.DurationPlannedAsTicks.HasValue)
            {
                isOvertime = this.Subtasks?.Sum(s => s.Entries?.Sum(e => e.DurationAsTicks)) > this.DurationPlannedAsTicks;
            }
            return isOvertime;
        }

        public interface IUpdateParam
        {
            string Name { get; set; }
            string Description { get; set; }
            DateTime? DatePlanned { get; set; }
            DateTime? DateDue { get; set; }
            long? DurationPlannedAsTicks { get; set; }
        }

        public interface ICreateParam : IUpdateParam
        {
            Project Project { get; set; }
        }

        [ForeignKey("Project")]
        public int? Project_Id { get; set; }
        public virtual Project Project { get; set; }
        public DateTime? DatePlanned { get; set; }
        public DateTime? DateDue { get; set; }
        public long? DurationPlannedAsTicks { get; set; }
        public AssignmentStatus AssignmentStatus { get; set; }
        public string Description { get; set; }
        public int TimesPushed { get; set; }
        public virtual ICollection<Subtask> Subtasks { get; set; }
    }


}
