using System;
using System.Collections.Generic;
using System.Text;

namespace TakeMyTime.Models.Models
{
    public class ProductivityViewModel
    {
        public DateTime X { get; set; }
        public double XAsDouble { get => Math.Round((double)X.Ticks / TimeSpan.FromHours(1).Ticks, 1); }
        public TimeSpan Y { get; set; }
        public double YAsDouble { get => Math.Round((double)Y.TotalHours, 1); }
    }
}
