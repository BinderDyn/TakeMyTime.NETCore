using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DOM.Interfaces;

namespace TakeMyTime.DOM.Models
{
    public class Entry : Entity<Entry>
    {
        public static Entry Create(ICreateParam param)
        {
            var entry = new Entry();
            entry.Init(param.Name, param.Comment, param.Started, param.Ended, param.Assignment, param.Project);
            return entry;
        }

        public void Init(string name,
            string comment,
            DateTime? started,
            DateTime? ended,
            Assignment assignment = null,
            Project project = null)
        {
            this.Name = name;
            this.Comment = comment;
            this.Date = DateTime.Now;
            this.Started = started;
            this.Ended = ended;
            this.Assignment = assignment;
            this.Project = project;
            CalculateDuration();
            this.SetCreated();
        }

        public void Update(IUpdateParam param)
        {
            this.Name = param.Name;
            this.Comment = param.Comment;
        }

        public void UpdateDate(DateTime date, DateTime? started, DateTime? ended)
        {
            this.Date = date;
            this.Started = started;
            this.Ended = ended;
            CalculateDuration();
        }

        public interface IUpdateParam
        {
            public string Name { get; set; }
            public string Comment { get; set; }
        }

        public interface ICreateParam : IUpdateParam
        {
            Assignment Assignment { get; set; }
            Project Project { get; set; }
            long? DurationAsTicks { get; set; }
            DateTime Date { get; set; }
            DateTime? Started { get; set; }
            DateTime? Ended { get; set; }
        }

        private void CalculateDuration()
        {
            bool canCalculateDuration = this.Started.HasValue && this.Ended.HasValue;
            if (canCalculateDuration)
            {
                TimeSpan duration = this.Ended.Value - this.Started.Value;
                this.DurationAsTicks = duration.Ticks;
            }
        }

        [ForeignKey("Project")]
        public int? Project_Id { get; set; }
        public virtual Project Project { get; set; }
        [ForeignKey("Assignment")]
        public int? Assigment_Id { get; set; }
        public virtual Assignment Assignment { get; set; }
        public DateTime Date { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }
        public long? DurationAsTicks { get; set; }
        public string Comment { get; set; }


    }
}
