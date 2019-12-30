using System;
using System.Collections.Generic;
using System.Text;

namespace TakeMyTime.WPF.Utility
{
    public class DateTimeCultureConverter
    {
        public static string ConvertToLocalDateTimeFormat(DateTime date)
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            return date.ToString("G", currentCulture);
        }
    }
}
