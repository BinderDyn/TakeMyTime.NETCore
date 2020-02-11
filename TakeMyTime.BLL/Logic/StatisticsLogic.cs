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
        private UnitOfWork unitOfWork;

        public StatisticsLogic(UnitOfWork uow = null)
        {
            if (uow != null)
            {
                this.unitOfWork = uow;
            }
            else
            {
                this.unitOfWork = new UnitOfWork();
            }
        }

        public Dictionary<string, double> GetAssignmentSharesOfProject(int project_id)
        {
            return this.unitOfWork.Statistics.GetAssignmentSharesOfProject(project_id);
        }

        public Dictionary<string, double> GetProjectShares()
        {
            return this.unitOfWork.Statistics.GetProjectTotalShares();
        }

        public IEnumerable<ProductivityViewModel> GetProjectProductiveEntries(int project_id)
        {
            return this.unitOfWork.Statistics.GetProjectProductiveDays(project_id);
        }

        public IEnumerable<MostProductiveWeekDaysViewModel> GetMostProductiveWeekDays()
        {
            return this.unitOfWork.Statistics.GetMostProductiveDays();
        }
    }
}
