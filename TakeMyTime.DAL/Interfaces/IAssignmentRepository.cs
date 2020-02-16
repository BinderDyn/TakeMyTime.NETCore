using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        // bool CheckForTimePlan(int id);
        // TimeSpan? GetActualDuration(int id);
        Assignment GetAssignmentById(int id);
        void DeleteSubtask(int id, int subtaskId);
        IEnumerable<Assignment> GetAllAssignmentsLoadFull();
        IEnumerable<Assignment> GetAssignmentsByProjectId(int project_id);
    }
}
