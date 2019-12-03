using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderDynamics.TakeMyTime.Biz.ViewModels
{
    public class StatisticsProjectGraphViewModel
    {
        public StatisticsProjectGraphViewModel(DateTime? xcoord, long? ycoord)
        {
            if (xcoord != null && ycoord != null)
            {
                this.Date = xcoord.Value;
                this.WorkingTimeInHours = decimal.Round((decimal)new TimeSpan(ycoord.Value).TotalHours, 2, MidpointRounding.AwayFromZero);
            }

        }

        public DateTime Date { get; set; }
        public decimal WorkingTimeInHours { get; set; }
    }
}
