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
    }
}
