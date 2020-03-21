using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeMyTime.Models.Models
{
    public class Entry : Entity<Entry>
    {
        public static Entry Create(ICreateParam param)
        {
            var entry = new Entry();
            entry.Init(param.Name, param.Comment, param.Started, param.Ended, param.Subtask, param.Project, param.DurationAsTicks);
            return entry;
        }

        public void Init(string name,
            string comment,
            DateTime? started,
            DateTime? ended,
            Subtask subtask = null,
            Project project = null,
            long? duration = null)
        {
            this.Name = name;
            this.Comment = comment;
            this.Date = started.HasValue ? started.Value : DateTime.Now;
            this.Started = started;
            this.Ended = ended;
            this.Project = project;
            this.Project_Id = project?.Id ?? null;
            this.Subtask = subtask;
            this.DurationAsTicks = duration; 
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
        }

        public interface IUpdateParam
        {
            public string Name { get; set; }
            public string Comment { get; set; }
        }

        public interface ICreateParam : IUpdateParam
        {
            Subtask Subtask { get; set; }
            Project Project { get; set; }
            long? DurationAsTicks { get; set; }
            DateTime Date { get; set; }
            DateTime? Started { get; set; }
            DateTime? Ended { get; set; }
        }

        public void CalculateDuration()
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
        public int? Subtask_Id { get; set; }
        public virtual Subtask Subtask { get; set; }
        public int? Assignment_Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }
        public long? DurationAsTicks { get; set; }
        public string Comment { get; set; }


    }
}
