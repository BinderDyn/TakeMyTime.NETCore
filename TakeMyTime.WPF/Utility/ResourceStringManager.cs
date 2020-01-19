﻿using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace TakeMyTime.WPF.Utility
{
    public class ResourceStringManager
    {
        public static string GetResourceByKey(string resourceKey)
        {
            return resourceKey switch
            {
                "ConfirmDeleteMessageBoxTitle" => Resources.ProjectOverview.ConfirmDeleteMessageBoxTitle,
                "ConfirmDeleteMessageBoxMessage" => Resources.ProjectOverview.ConfirmDeleteMessageBoxMessage,
                "ProjectsAll" => Resources.AssignmentOverview.ProjectsAll,
                "CalendarWeek" => Resources.MainWindow.CurrentCalendarWeek,
                "NameNotEmptyBoxMessage" => Resources.AddSubtask.NameNotEmptyBoxMessage,
                "NameNotEmptyBoxTitle" => Resources.AddSubtask.NameNotEmptyBoxTitle,
                _ => string.Empty
            };
        }

        public static string GetResourceBySubtaskPriority(EnumDefinition.SubtaskPriority priority)
        {
            return priority switch
            {
                EnumDefinition.SubtaskPriority.Lowest => Resources.Shared.PriorityLowest,
                EnumDefinition.SubtaskPriority.Low => Resources.Shared.PriorityLow,
                EnumDefinition.SubtaskPriority.Medium => Resources.Shared.PriorityMedium,
                EnumDefinition.SubtaskPriority.High => Resources.Shared.PriorityHigh,
                EnumDefinition.SubtaskPriority.Highest => Resources.Shared.PriorityHighest,
                _ => Resources.Shared.PriorityMedium
            };
        }
    }
}
