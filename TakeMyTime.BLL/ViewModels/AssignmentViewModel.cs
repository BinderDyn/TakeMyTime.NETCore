using BinderDynamics.TakeMyTime.Biz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DOM.Models;
using static Common.Enums.EnumDefinition;

namespace BinderDynamics.TakeMyTime.Biz.ViewModels
{
    public class AssignmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProjectId { get; set; }
        public ICollection<EntriesViewModel> Entries { get; set; }
        public DateTime? DatePlanned { get; set; }
        public DateTime? DateDue { get; set; }
        public TimeSpan? DurationPlanned { get; set; }
        public TimeSpan? ActualDuration { get; set; }
        public AssignmentStatus AssignmentStatus { get; set; }
        public TimekeeperStatus Timekeeper { get; set; }
        public int? Pages { get; set; }
        public int? Words { get; set; }
        public string Comment { get; set; }
        public string DisplayName { get => "Planned/Due: " + string.Format("{0:dd.MM.yyyy}/{1:dd.MM.yyyy}", DatePlanned.Value ,DateDue.Value) + "| " + Name; }
    }
}
