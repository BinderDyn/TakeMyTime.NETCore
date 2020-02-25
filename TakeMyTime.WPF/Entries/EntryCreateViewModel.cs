using System;
using System.Collections.Generic;
using System.Text;

using TakeMyTime.Models.Models;

namespace TakeMyTime.WPF.Entries
{
    public class EntryCreateViewModel : Entry.ICreateParam
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
}
