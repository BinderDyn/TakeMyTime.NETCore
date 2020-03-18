using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

using TakeMyTime.Models.Models;
using TakeMyTime.WPF.Utility;

namespace TakeMyTime.WPF.Subtasks
{
    public class SubtaskGridViewModel
    {
        private Assignment assignment = null;
        public SubtaskGridViewModel(Subtask subtask)
        {
            this.Id = subtask.Id;
            this.Name = subtask.Name;
            this.Description = subtask.Description;
            this.Priority = ResourceStringManager.GetResourceBySubtaskPriority(subtask.Priority);
            this.assignment = subtask.Assignment;
            this.StatusAsImage = GetImageByStatus(subtask.Status);
        }

        public int Id { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Priority { get; private set; }
        public Uri StatusAsImage { get; private set; }
        public bool HasAssignment { get => assignment != null; }

        private Uri GetImageByStatus(EnumDefinition.SubtaskStatus status)
        {
            return status switch
            {
                EnumDefinition.SubtaskStatus.NotYetDone => new Uri("pack://application:,,,/Images/assignmentActiveIconSmall.png"),
                EnumDefinition.SubtaskStatus.Aborted => new Uri("pack://application:,,,/Images/assignmentAbortedIconSmall.png"),
                EnumDefinition.SubtaskStatus.Done => new Uri("pack://application:,,,/Images/assignmentDoneIconSmall.png"),
                _ => new Uri("pack://application:,,,/Images/assignmentActiveIconSmall.png")
            };
        }
    }
}
