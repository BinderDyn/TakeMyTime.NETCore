using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TakeMyTime.DOM.Models;
using static Common.Enums.EnumDefinition;

namespace TakeMyTime.Models.Models
{
    public class Subtask : Entity<Subtask>
    {
        private Subtask()
        {
        }

        public static Subtask Create(ICreateParam param)
        {
            var subtask = new Subtask();
            subtask.Init(param);
            return subtask;
        }

        public void SetStatus(SubtaskStatus status)
        {
            if(CanSetStatus(status))
            {
                this.Status = status;
            }
        }

        public void Update(IUpdateParam param)
        {
            this.Name = param.Name;
            this.Description = param.Description;
            this.SetEdited();
        }

        private bool CanSetStatus(SubtaskStatus status)
        {
            if (this.Status == status) return false;
            if (this.Status == SubtaskStatus.Aborted) return false;
            else return true;
        }

        private void Init(ICreateParam param)
        {
            this.Name = param.Name;
            this.Description = param.Description;
            this.Status = SubtaskStatus.NotYetDone;
            this.SetCreated();
        }

        public interface IUpdateParam : ICreateParam
        {
        }

        public interface ICreateParam
        {
            string Name { get; set; }
            string Description { get; set; }
        }

        public SubtaskStatus Status { get; set; }
        public long? DurationTicks { get; set; }
        public string Description { get; set; }
        [ForeignKey("Assignment")]
        public virtual int Assignment_Id { get; set; }
        public virtual Assignment Assignment { get; set; }
        public ICollection<Entry> Entries { get; set; }
    }
}
