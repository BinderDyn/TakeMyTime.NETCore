using System;
using System.Collections.Generic;
using System.Text;

using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL
{
    public class WeekdayProductivityManager
    {
        public WeekdayProductivityManager()
        {
        }

        public void ProcessEntry(Entry entry)
        {
            var durationAsHours = entry.DurationAsTicks.HasValue ? TimeSpan.FromTicks(entry.DurationAsTicks.Value).TotalHours : 0;
            if(durationAsHours > 0)
            {
                TotalWorkingHours += durationAsHours;
                var date = entry.Started.HasValue ? entry.Started.Value.DayOfWeek : entry.Date.DayOfWeek;
                switch (date)
                {
                    case DayOfWeek.Monday:
                        MondayTotalWorkingHours += durationAsHours;
                        this.MondayAverageWorkingHours = Math.Round(durationAsHours / MondayTotalWorkingHours, 2);
                        this.MondayValueShare = Math.Round(MondayAverageWorkingHours / TotalWorkingHours, 2);
                        break;
                    case DayOfWeek.Tuesday:
                        TuesdayTotalWorkingHours += durationAsHours;
                        this.TuesdayAverageWorkingHours = Math.Round(durationAsHours / TuesdayTotalWorkingHours, 2);
                        this.TuesdayValueShare = Math.Round(TuesdayAverageWorkingHours / TotalWorkingHours, 2);
                        break;
                    case DayOfWeek.Wednesday:
                        WednesdayTotalWorkingHours += durationAsHours;
                        this.WednesdayAverageWorkingHours = Math.Round(durationAsHours / WednesdayTotalWorkingHours, 2);
                        this.WednesdayValueShare = Math.Round(WednesdayTotalWorkingHours / TotalWorkingHours, 2);
                        break;
                    case DayOfWeek.Thursday:
                        ThursdayTotalWorkingHours += durationAsHours;
                        this.ThursdayAverageWorkingHours = Math.Round(durationAsHours / ThursdayTotalWorkingHours, 2);
                        this.ThursdayValueShare = Math.Round(ThursdayTotalWorkingHours / TotalWorkingHours, 2);
                        break;
                    case DayOfWeek.Friday:
                        FridayTotalWorkingHours += durationAsHours;
                        this.FridayAverageWorkingHours = Math.Round(durationAsHours / FridayTotalWorkingHours, 2);
                        this.FridayValueShare = Math.Round(FridayAverageWorkingHours / TotalWorkingHours, 2);
                        break;
                    case DayOfWeek.Saturday:
                        SaturdayTotalWorkingHours += durationAsHours;
                        this.SaturdayAverageWorkingHours = Math.Round(durationAsHours / SaturdayTotalWorkingHours, 2);
                        this.SaturdayValueShare = Math.Round(SaturdayAverageWorkingHours / TotalWorkingHours, 2);
                        break;
                    case DayOfWeek.Sunday:
                        SundayTotalWorkingHours += durationAsHours;
                        this.SundayAverageWorkingHours = Math.Round(durationAsHours / SundayTotalWorkingHours, 2);
                        this.SundayValueShare = Math.Round(SundayAverageWorkingHours / TotalWorkingHours, 2);
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
                    Value = MondayValueShare,
                    AverageHours = MondayAverageWorkingHours,
                    TotalHours = MondayTotalWorkingHours
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Tuesday,
                    Value = TuesdayValueShare,
                    AverageHours = TuesdayAverageWorkingHours,
                    TotalHours = TuesdayTotalWorkingHours
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Wednesday,
                    Value = WednesdayValueShare,
                    AverageHours = WednesdayAverageWorkingHours,
                    TotalHours = WednesdayTotalWorkingHours
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Thursday,
                    Value = ThursdayValueShare,
                    AverageHours = ThursdayAverageWorkingHours,
                    TotalHours = ThursdayTotalWorkingHours
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Friday,
                    Value = FridayValueShare,
                    AverageHours = FridayAverageWorkingHours,
                    TotalHours = FridayTotalWorkingHours
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Saturday,
                    Value = SaturdayValueShare,
                    AverageHours = SaturdayAverageWorkingHours,
                    TotalHours = SaturdayTotalWorkingHours
                },
                new MostProductiveWeekDaysViewModel
                {
                    Day = DayOfWeek.Sunday,
                    Value = SundayValueShare,
                    AverageHours = SundayAverageWorkingHours,
                    TotalHours = SundayTotalWorkingHours
                },
            };
        }

        public double TotalWorkingHours { get; private set; }
        public double MondayTotalWorkingHours { get; private set; }
        public double MondayValueShare { get; private set; }
        public double MondayAverageWorkingHours { get; private set; }
        public double TuesdayTotalWorkingHours { get; private set; }
        public double TuesdayValueShare { get; private set; }
        public double TuesdayAverageWorkingHours { get; private set; }
        public double WednesdayTotalWorkingHours { get; private set; }
        public double WednesdayValueShare { get; private set; }
        public double WednesdayAverageWorkingHours { get; private set; }
        public double ThursdayTotalWorkingHours { get; private set; }
        public double ThursdayValueShare { get; private set; }
        public double ThursdayAverageWorkingHours { get; private set; }
        public double FridayTotalWorkingHours { get; private set; }
        public double FridayValueShare { get; private set; }
        public double FridayAverageWorkingHours { get; private set; }
        public double SaturdayTotalWorkingHours { get; private set; }
        public double SaturdayValueShare { get; private set; }
        public double SaturdayAverageWorkingHours { get; private set; }
        public double SundayTotalWorkingHours { get; private set; }
        public double SundayValueShare { get; private set; }
        public double SundayAverageWorkingHours { get; private set; }
    }
}
