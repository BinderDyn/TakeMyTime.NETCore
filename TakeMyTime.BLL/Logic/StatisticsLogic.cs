using BinderDynamics.TakeMyTime.Biz.ViewModels;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DAL.uow;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;

namespace TakeMyTime.BLL.Logic
{
    public class StatisticsLogic
    {
        private UnitOfWork uow = new UnitOfWork();

        public Dictionary<string, double> GetAssignmentSharesOfProject(int project_id)
        {
            return this.uow.Statistics.GetAssignmentSharesOfProject(project_id);
        }

        public Dictionary<string, double> GetProjectShares()
        {
            return this.uow.Statistics.GetProjectTotalShares();
        }

        public IEnumerable<ProductivityViewModel> GetProjectProductiveEntries(int project_id)
        {
            return this.uow.Statistics.GetProjectProductiveDays(project_id);
        }
    }
}
