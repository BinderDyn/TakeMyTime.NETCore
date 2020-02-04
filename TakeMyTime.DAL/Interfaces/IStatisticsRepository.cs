using System;
using System.Collections.Generic;
using System.Text;

namespace TakeMyTime.DAL.Interfaces
{
    public interface IStatisticsRepository
    {
        long? GetTotalWorktimeOfAllActiveProjects();
        long? GetTotalWorktimeOfSpecificProject(int project_id);
        Dictionary<string, double> GetAssignmentSharesOfProject(int project_id);
        Dictionary<string, double> GetProjectTotalShares();
    }
}
