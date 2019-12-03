using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderDynamics.TakeMyTime.Biz.ViewModels
{
    public class StatisticsAssignmentGraphViewModel
    {
        public StatisticsAssignmentGraphViewModel(string assignmentName, long? workingTime)
        {
            this.AssignmentName = assignmentName;
            if (workingTime.HasValue)
            {
                this.WorkingTime = decimal.Round((decimal)new TimeSpan(workingTime.Value).TotalHours, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                this.WorkingTime = 0;
            }

        }

        public string AssignmentName { get; set; }
        public decimal WorkingTime { get; set; }
    }
}
