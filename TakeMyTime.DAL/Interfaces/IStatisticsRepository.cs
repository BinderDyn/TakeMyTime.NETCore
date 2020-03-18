using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.Interfaces
{
    public interface IStatisticsRepository
    {
        long? GetTotalWorktimeOfAllActiveProjects();
        long? GetTotalWorktimeOfSpecificProject(int project_id);
        IEnumerable<Tuple<int, string, double>> GetAssignmentSharesOfProject(int project_id);
        IEnumerable<Tuple<int, string, double>> GetProjectTotalShares();
        IEnumerable<ProductivityViewModel> GetProjectProductiveDays(int project_id);
        IEnumerable<MostProductiveWeekDaysViewModel> GetMostProductiveDays();
    }
}
