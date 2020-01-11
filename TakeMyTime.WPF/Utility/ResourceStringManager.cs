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
                _ => string.Empty
            };
        }
    }
}
