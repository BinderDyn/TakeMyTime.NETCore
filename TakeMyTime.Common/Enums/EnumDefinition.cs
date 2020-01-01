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
        /// Provides the Assignment-Class with a discriminator for the current status
        /// </summary>
        public enum AssignmentStatus
        {
            [Description("All")]
            Default = -1,
            [Description("Active")]
            Active = 0,
            [Description("Future")]
            Future = 1,
            [Description("Done")]
            Done = 2,
            [Description("Aborted")]
            Aborted = 3,
            [Description("Postponed")]
            Postponed = 4,
            [Description("Upcoming")]
            Upcoming = 5
        }

        /// <summary>
        /// Discriminator for project types
        /// </summary>
        //public enum ProjectType
        //{
        //    [Description("Standard")]
        //    Default = -1,
        //    [Description("Writing")]
        //    Book = 1,
        //    [Description("Language")]
        //    Language = 2,
        //    [Description("Programming")]
        //    Programming = 3
        //}

        public enum TimekeeperStatus
        {
            [Description("In time")]
            InTime = 0,
            [Description("Not in time")]
            NotInTime = 1
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
            Project = 3
        }
    }
}
