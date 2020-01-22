using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DOM.Models;
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
        }

        public int Id { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Priority { get; private set; }
        public bool HasAssignment { get => assignment != null; }
    }
}
