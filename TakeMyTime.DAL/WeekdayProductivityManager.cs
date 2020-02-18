using System;
using System.Collections.Generic;
using System.Text;

using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL
{
    public class WeekdayProductivityManager
    {
        public WeekdayProductivityManager(int totalEntryCount)
        {
            this.TotalEntryCount = totalEntryCount;
        }

        public void ProcessEntry(Entry entry)
        {
            if(CanProcessEntries)
            {
                var date = entry.Started.HasValue ? entry.Started.Value.DayOfWeek : entry.Date.DayOfWeek;
                switch (date)
                {
                    case DayOfWeek.Monday:
                        MondayEntryCount++;
                        this.MondayValueShare = Math.Round((double)MondayEntryCount / (double)TotalEntryCount, 2);
                        break;
                    case DayOfWeek.Tuesday:
                        TuesdayEntryCount++;
                        this.TuesdayValueShare = Math.Round((double)TuesdayEntryCount / (double)TotalEntryCount, 2);
                        break;
                    case DayOfWeek.Wednesday:
                        WednesdayEntryCount++;
                        this.WednesdayValueShare = Math.Round((double)WednesdayEntryCount / (double)TotalEntryCount, 2);
                        break;
                    case DayOfWeek.Thursday:
                        ThursdayEntryCount++;
                        this.ThursdayValueShare = Math.Round((double)ThursdayEntryCount / (double)TotalEntryCount, 2);
                        break;
                    case DayOfWeek.Friday:
                        FridayEntryCount++;
                        this.FridayValueShare = Math.Round((double)FridayEntryCount / (double)TotalEntryCount, 2);
                        break;
                    case DayOfWeek.Saturday:
                        SaturdayEntryCount++;
                        this.SaturdayValueShare = Math.Round((double)SaturdayEntryCount / (double)TotalEntryCount, 2);
                        break;
                    case DayOfWeek.Sunday:
                        SundayEntryCount++;
                        this.SundayValueShare = Math.Round((double)SundayEntryCount / (double)TotalEntryCount, 2);
                        break;
                };
            }
        }

        public IEnumerable<MostProductiveWeekDaysViewModel> GetResults()
        {
            return new List<MostProductiveWeekDaysViewModel>
            {
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Monday,
                    Value = MondayValueShare
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Tuesday,
                    Value = TuesdayValueShare
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Wednesday,
                    Value = WednesdayValueShare
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Thursday,
                    Value = ThursdayValueShare
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Friday,
                    Value = FridayValueShare
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Saturday,
                    Value = SaturdayValueShare
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Sunday,
                    Value = SundayValueShare
                },
            };
        }

        public int TotalEntryCount { get; private set; }
        public int MondayEntryCount { get; private set; }
        public double MondayValueShare { get; private set; }
        public int TuesdayEntryCount { get; private set; }
        public double TuesdayValueShare { get; private set; }
        public int WednesdayEntryCount { get; private set; }
        public double WednesdayValueShare { get; private set; }
        public int ThursdayEntryCount { get; private set; }
        public double ThursdayValueShare { get; private set; }
        public int FridayEntryCount { get; private set; }
        public double FridayValueShare { get; private set; }
        public int SaturdayEntryCount { get; private set; }
        public double SaturdayValueShare { get; private set; }
        public int SundayEntryCount { get; private set; }
        public double SundayValueShare { get; private set; }
        public bool CanProcessEntries { get => this.TotalEntryCount > 0; }
    }
}
