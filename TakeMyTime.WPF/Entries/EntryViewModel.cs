using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;
using TakeMyTime.WPF.Utility;

namespace TakeMyTime.WPF.Entries
{
    public class EntryViewModel
    {
        public EntryViewModel(Entry entry)
        {
            this.Name = entry.Name;
            this.Description = entry.Comment;
            this.Start = entry.Started;
            this.End = entry.Ended;
            this.Duration = entry.DurationAsTicks;
            this.Project = entry.Project;
            this.Subtask = entry.Subtask;
            this.Id = entry.Id;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public string StartAsString { get => DateTimeCultureConverter.ConvertToLocalDateTimeFormat(this.Start); }
        public DateTime? End { get; set; }
        public string EndAsString { get => DateTimeCultureConverter.ConvertToLocalDateTimeFormat(this.End); }
        public long? Duration { get; set; }
        public string DurationAsString { get => this.Duration.HasValue ? new TimeSpan(this.Duration.Value).ToString(@"hh\:mm\:ss") : "-"; }
        public Project Project { get; set; }
        public string ProjectName { get => this.Project != null ? this.Project.Name : "-"; }
        public int ProjectId { get => this.Project != null ? this.Project.Id : 0; }
        public Subtask Subtask { get; set; }
        public string SubtaskName { get => this.Subtask != null ? this.Subtask.Name : "-"; }
    }
}
