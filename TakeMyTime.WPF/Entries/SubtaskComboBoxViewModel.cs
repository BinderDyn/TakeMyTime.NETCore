using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.WPF.Entries
{
    public class SubtaskComboBoxViewModel
    {

        public SubtaskComboBoxViewModel()
        {

        }

        public SubtaskComboBoxViewModel(Subtask subtask)
        {
            this.Id = subtask.Id;
            this.Name = subtask.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
