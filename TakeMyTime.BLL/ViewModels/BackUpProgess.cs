using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMyTime.Biz.ViewModels
{
    public class BackUpProgess
    {
        public BackUpProgess(int progress, string entityName, string currentStep)
        {
            this.ProgressPercentage = progress;
            this.EntityName = entityName;
            this.CurrentStep = currentStep;
        }

        public int ProgressPercentage { get; set; }
        public string EntityName { get; set; }
        public string CurrentStep { get; set; }
    }
}
