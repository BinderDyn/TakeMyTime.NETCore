using BinderDynamics.TakeMyTime.Biz.ViewModels;
using System;

namespace BinderDynamics.TakeMyTime.Biz.ViewModels
{
    public class EntryViewModel
    {
        public string Name { get; set; }
        public int? ProjectId { get; set; }
        public ProjectViewModel Project {get; set;}
        public AssignmentViewModel Assignment { get; set; }
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? Pages { get; set; }
        public int? Words { get; set; }
        public string Comment { get; set; }
        public bool ParentIsBookProject { get; set; }
        public string DisplayName { get => string.Format("{0:ddd dd.MM.yyyy HH:mm} | {1}", Date, Name); }
    }
}
