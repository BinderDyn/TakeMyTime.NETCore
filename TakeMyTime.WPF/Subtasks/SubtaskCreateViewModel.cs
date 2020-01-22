using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.WPF.Subtasks
{
    public class SubtaskCreateViewModel : Subtask.IUpdateParam
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EnumDefinition.SubtaskPriority Priority { get; set; }
    }
}
