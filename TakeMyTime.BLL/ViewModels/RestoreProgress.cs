using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMyTime.Biz.ViewModels
{
    public class RestoreProgress : BackUpProgess
    {
        public RestoreProgress(int progress, string entityName, string currentStep) : base(progress, entityName, currentStep)
        {
        }
    }
}
