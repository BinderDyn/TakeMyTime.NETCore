using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.SeederFactories
{
    public class EntryFactory
    {
        public static IEnumerable<Entry> Create(Subtask subtask)
        {
            var rnd = new Random();
            var entries = new List<Entry>
            {
                Entry.Create(new EntryCreateParam
                {
                    DurationAsTicks = durations[rnd.Next(0, 6)].Ticks,
                    Date = started[rnd.Next(0, 6)],
                    Name = subtask.Name,
                    Project = subtask.Assignment.Project,
                    Subtask = subtask,
                    Started = started[rnd.Next(0, 6)]
                }),
                Entry.Create(new EntryCreateParam
                {
                    DurationAsTicks = durations[rnd.Next(0, 6)].Ticks,
                    Date = started[rnd.Next(0, 6)],
                    Name = subtask.Name,
                    Project = subtask.Assignment.Project,
                    Subtask = subtask,
                    Started = started[rnd.Next(0, 6)]
                }),
                Entry.Create(new EntryCreateParam
                {
                    DurationAsTicks = durations[rnd.Next(0, 6)].Ticks,
                    Date = started[rnd.Next(0, 6)],
                    Name = subtask.Name,
                    Project = subtask.Assignment.Project,
                    Subtask = subtask,
                    Started = started[rnd.Next(0, 6)]
                }),
                Entry.Create(new EntryCreateParam
                {
                    DurationAsTicks = durations[rnd.Next(0, 6)].Ticks,
                    Date = started[rnd.Next(0, 6)],
                    Name = subtask.Name,
                    Project = subtask.Assignment.Project,
                    Subtask = subtask,
                    Started = started[rnd.Next(0, 6)]
                }),
                Entry.Create(new EntryCreateParam
                {
                    DurationAsTicks = durations[rnd.Next(0, 6)].Ticks,
                    Date = started[rnd.Next(0, 6)],
                    Name = subtask.Name,
                    Project = subtask.Assignment.Project,
                    Subtask = subtask,
                    Started = started[rnd.Next(0, 6)]
                }),
                Entry.Create(new EntryCreateParam
                {
                    DurationAsTicks = durations[rnd.Next(0, 6)].Ticks,
                    Date = started[rnd.Next(0, 6)],
                    Name = subtask.Name,
                    Project = subtask.Assignment.Project,
                    Subtask = subtask,
                    Started = started[rnd.Next(0, 6)]
                }),
                Entry.Create(new EntryCreateParam
                {
                    DurationAsTicks = durations[rnd.Next(0, 6)].Ticks,
                    Date = started[rnd.Next(0, 6)],
                    Name = subtask.Name,
                    Project = subtask.Assignment.Project,
                    Subtask = subtask,
                    Started = started[rnd.Next(0, 6)]
                }),
            };

            return entries.Take(rnd.Next(0, 8));
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

        static DateTime[] started = { new DateTime(2019, 08, 12), new DateTime(2019, 09, 13), new DateTime(2019, 12, 12), new DateTime(2020, 1, 2),
        new DateTime(2020, 1, 5), new DateTime(2020, 2, 1) };

        static TimeSpan[] durations = { new TimeSpan(1, 2, 30), new TimeSpan(0, 20, 35), new TimeSpan(2, 20, 5), new TimeSpan(6, 0, 53), 
            new TimeSpan(0, 46, 11), new TimeSpan(0, 10, 11), new TimeSpan(0, 4, 30) };
    }
}
