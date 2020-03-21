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
                        MondayEntryCount++;
                        this.MondayAverageWorkingHours = Math.Round(MondayTotalWorkingHours / MondayEntryCount, 2);
                        
                        break;
                    case DayOfWeek.Tuesday:
                        TuesdayTotalWorkingHours += durationAsHours;
                        TuesdayEntryCount++;
                        this.TuesdayAverageWorkingHours = Math.Round(TuesdayTotalWorkingHours / TuesdayEntryCount, 2);
                        
                        break;
                    case DayOfWeek.Wednesday:
                        WednesdayTotalWorkingHours += durationAsHours;
                        WednesdayEntryCount++;
                        this.WednesdayAverageWorkingHours = Math.Round(WednesdayTotalWorkingHours / WednesdayEntryCount, 2);
                        
                        break;
                    case DayOfWeek.Thursday:
                        ThursdayTotalWorkingHours += durationAsHours;
                        ThursdayEntryCount++;
                        this.ThursdayAverageWorkingHours = Math.Round(ThursdayTotalWorkingHours / ThursdayEntryCount, 2);
                        
                        break;
                    case DayOfWeek.Friday:
                        FridayTotalWorkingHours += durationAsHours;
                        FridayEntryCount++;
                        this.FridayAverageWorkingHours = Math.Round(FridayTotalWorkingHours / FridayEntryCount, 2);
                        
                        break;
                    case DayOfWeek.Saturday:
                        SaturdayTotalWorkingHours += durationAsHours;
                        SaturdayEntryCount++;
                        this.SaturdayAverageWorkingHours = Math.Round(SaturdayTotalWorkingHours / SaturdayEntryCount, 2);
                        
                        break;
                    case DayOfWeek.Sunday:
                        SundayTotalWorkingHours += durationAsHours;
                        SundayEntryCount++;
                        this.SundayAverageWorkingHours = Math.Round(SundayTotalWorkingHours / SundayEntryCount, 2);
                        break;
                };

                CalculateShares();
            }
        }

        private void CalculateShares()
        {
            this.MondayValueShare = Math.Round(MondayTotalWorkingHours / TotalWorkingHours, 2);
            this.TuesdayValueShare = Math.Round(TuesdayTotalWorkingHours / TotalWorkingHours, 2);
            this.WednesdayValueShare = Math.Round(WednesdayTotalWorkingHours / TotalWorkingHours, 2);
            this.ThursdayValueShare = Math.Round(ThursdayTotalWorkingHours / TotalWorkingHours, 2);
            this.FridayValueShare = Math.Round(FridayTotalWorkingHours / TotalWorkingHours, 2);
            this.SaturdayValueShare = Math.Round(SaturdayTotalWorkingHours / TotalWorkingHours, 2);
            this.SundayValueShare = Math.Round(SundayTotalWorkingHours / TotalWorkingHours, 2);
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
        public double MondayEntryCount { get; set; }
        public double TuesdayTotalWorkingHours { get; private set; }
        public double TuesdayValueShare { get; private set; }
        public double TuesdayAverageWorkingHours { get; private set; }
        public double TuesdayEntryCount { get; set; }
        public double WednesdayTotalWorkingHours { get; private set; }
        public double WednesdayValueShare { get; private set; }
        public double WednesdayAverageWorkingHours { get; private set; }
        public double WednesdayEntryCount { get; set; }
        public double ThursdayTotalWorkingHours { get; private set; }
        public double ThursdayValueShare { get; private set; }
        public double ThursdayAverageWorkingHours { get; private set; }
        public double ThursdayEntryCount { get; set; }
        public double FridayTotalWorkingHours { get; private set; }
        public double FridayValueShare { get; private set; }
        public double FridayAverageWorkingHours { get; private set; }
        public double FridayEntryCount { get; set; }
        public double SaturdayTotalWorkingHours { get; private set; }
        public double SaturdayValueShare { get; private set; }
        public double SaturdayAverageWorkingHours { get; private set; }
        public double SaturdayEntryCount { get; set; }
        public double SundayTotalWorkingHours { get; private set; }
        public double SundayValueShare { get; private set; }
        public double SundayAverageWorkingHours { get; private set; }
        public double SundayEntryCount { get; set; }
    }
}
