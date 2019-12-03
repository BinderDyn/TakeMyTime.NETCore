using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.DAL.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        bool CheckForTimePlan(int id);
        TimeSpan? GetActualDuration(int id);
    }
}
