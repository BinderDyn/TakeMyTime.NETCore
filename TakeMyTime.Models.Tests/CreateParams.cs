using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;

namespace TakeMyTime.Models.Tests
{
    public class CreateParams
    {
        public class AssignmentCreateParam : Assignment.ICreateParam
        {
            public Project Project { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime? DatePlanned { get; set; }
            public DateTime? DateDue { get; set; }
            public long? DurationPlannedAsTicks { get; set; }
        }

        public class SubtaskCreateParam : Subtask.ICreateParam
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public EnumDefinition.SubtaskPriority Priority { get; set; }
        }
    }
}
