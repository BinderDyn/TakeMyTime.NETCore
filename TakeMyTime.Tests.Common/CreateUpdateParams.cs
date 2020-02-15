using Common.Enums;
using System;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;

namespace TakeMyTime.Tests.Common
{
    public class CreateUpdateParams
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

        public class AssignmentUpdateParam : Assignment.IUpdateParam
        {
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

        public class EntryCreateParam : Entry.ICreateParam
        {
            public Subtask Subtask { get; set; }
            public Project Project { get; set; }
            public long? DurationAsTicks { get; set; }
            public DateTime Date { get; set; }
            public DateTime? Started { get; set; }
            public DateTime? Ended { get; set; }
            public string Name { get; set; }
            public string Comment { get; set; }
        }

        public class EntryUpdateParam : Entry.IUpdateParam
        {
            public string Name { get; set; }
            public string Comment { get; set; }
        }

        public class ProjectCreateParam : Project.ICreateParam
        {
            public ProjectType ProjectType { get; set; }
            public string Description { get; set; }
            public string Name { get; set; }
        }

        public class ProjectTypeCreateParam : ProjectType.ICreateParam
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class SubtaskUpdateParam : SubtaskCreateParam, Subtask.IUpdateParam { }
    }
}
