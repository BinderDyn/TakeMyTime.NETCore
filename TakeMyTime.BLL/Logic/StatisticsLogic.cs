﻿using BinderDynamics.TakeMyTime.Biz.ViewModels;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DAL.uow;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.BLL.Logic
{
    public class StatisticsLogic
    {
        private UnitOfWork uow = new UnitOfWork();

        public IEnumerable<Tuple<int, string, double>> GetAssignmentSharesOfProject(int project_id)
        {
            return this.uow.Statistics.GetAssignmentSharesOfProject(project_id);
        }

        public IEnumerable<Tuple<int, string, double>> GetProjectShares()
        {
            return this.uow.Statistics.GetProjectTotalShares();
        }
    }
}
