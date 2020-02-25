using System;
using System.Collections.Generic;
using System.Text;

namespace TakeMyTime.WPF.Utility
{
    public class DateTimeCultureConverter
    {
        public static string ConvertToLocalDateTimeFormat(DateTime? date)
        {
            string result = "-";
            if (date.HasValue)
            {
                var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                result = date.Value.ToString("G", currentCulture);
            }
            return result;
        }

        public static string GetCalendarWeek(DateTime? date)
        {
            string result = "-";
            if(date.HasValue)
            {
                var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                result = currentCulture.Calendar
                    .GetWeekOfYear(date.Value, currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek)
                    .ToString();
            }
            return result;
        }

        public static string GetCalendarWeek()
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            return currentCulture.Calendar
                .GetWeekOfYear(DateTime.Now, currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek)
                .ToString();
        }
    }
}
