﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TakeMyTime.Models.Models
{
    public class MostProductiveWeekDaysViewModel
    {
        public DayOfWeek Day { get; set; }
        public double Value { get; set; }
        public double AverageHours { get; set; }
        public double TotalHours { get; set; }
    }
}
