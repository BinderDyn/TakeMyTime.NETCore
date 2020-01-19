using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.WPF.Assignments
{
    public class AssignmentCreateViewModel : Assignment.ICreateParam
    {
        public Project Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DatePlanned { get; set; }
        public DateTime? DateDue { get; set; }
        public long? DurationPlannedAsTicks { get; set; }
    }
}
