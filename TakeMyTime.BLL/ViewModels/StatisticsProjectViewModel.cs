using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderDynamics.TakeMyTime.Biz.ViewModels
{
    public class StatisticsProjectViewModel
    {
        public StatisticsProjectViewModel()
        {
            this.GraphPoints = new List<StatisticsProjectGraphViewModel>();
        }

        public IEnumerable<StatisticsProjectGraphViewModel> GraphPoints { get; set; }
        public TimeSpan AverageEntryLength { get; set; }
        public DateTime Created { private get; set; }
        public string CreatedAsString { get => string.Format("{0:dd.MM.yyyy}", Created); }
        public DateTime LastWorkedOn { private get; set; }
        public string LastWorkedOnAsString { get => string.Format("{0:dd.MM.yyyy}", LastWorkedOn); }
        public string ProjectName { get; set; }
    }
}
