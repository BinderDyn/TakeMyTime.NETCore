using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums
{
    /// <summary>
    /// Contains all enums shared around the application
    /// </summary>
    public class EnumDefinition
    {
        /// <summary>
        /// Status of assignments
        /// </summary>
        public enum AssignmentStatus
        {
            [Description("All")]
            Default = ~0,
            [Description("Active")]
            InProgress = 0,
            [Description("Future")]
            Future = 1,
            [Description("Done")]
            Done = 2,
            [Description("Aborted")]
            Aborted = 3,
            [Description("Postponed")]
            Postponed = 4,
        }

        public enum TimekeeperStatus
        {
            [Description("On time")]
            OnTime = 0,
            [Description("Not on time")]
            Delayed = 1
        }

        public enum ProjectStatus
        {
            [Description("Active")]
            Active = 0,
            [Description("Archived")]
            Archived = 1
        }

        public enum OperationState
        {
            [Description("Backup")]
            Backup = 0,
            [Description("Restore")]
            Restore = 1,
            [Description("Delete")]
            Delete = 2
        }
        
        public enum BackupEntityType
        {
            [Description("Unknown")]
            Unknown = 0,
            [Description("Entry")]
            Entry = 1,
            [Description("Assignment")]
            Assignment = 2,
            [Description("Project")]
            Project = 3,
            [Description("Subtask")]
            Subtask = 4
        }

        public enum SubtaskStatus
        {
            NotYetDone = 0,
            Done = 1,
            Aborted = 2
        }

        public enum SubtaskPriority
        {
            Lowest = 0,
            Low = 1,
            Medium = 2,
            High = 3,
            Highest = 4
        }
    }
}
