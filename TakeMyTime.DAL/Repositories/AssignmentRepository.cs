using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DAL.Interfaces;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.Repositories
{
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        TakeMyTimeDbContext context;

        public AssignmentRepository(TakeMyTimeDbContext context) : base(context)
        {
            this.context = context;
        }

        public TakeMyTimeDbContext TakeMyTimeDbContext
        {
            get { return DbContext as TakeMyTimeDbContext; }
        }

        public Assignment GetAssignmentById(int id)
        {
            return this.context.Assignments
                .Include(a => a.Subtasks)
                .SingleOrDefault(a => a.Id == id);
        }

        public IEnumerable<Assignment> GetAssignmentsByProjectId(int project_id)
        {
            return this.context.Assignments
                .Include(a => a.Subtasks)
                .Where(a => a.Project_Id == project_id);
        }

        public IEnumerable<Assignment> GetAllAssignmentsLoadFull()
        {
            return this.context.Assignments
                .Include(a => a.Subtasks)
                .ToList();
        }

        public void DeleteSubtask(int id, int subtaskId)
        {
            var entity = this.context.Assignments
                .Include(a => a.Subtasks)
                .SingleOrDefault(a => a.Id == id);
            if (entity != null)
            {
                entity.Subtasks.Remove(entity.Subtasks.SingleOrDefault(st => st.Id == subtaskId));
            }
        }

        

        //public bool CheckForTimePlan(int id)
        //{
        //    Assignment assignment = context.Assignments.FirstOrDefault(a => a.Id == id);

        //    if (assignment.Entries.Any())
        //    {
        //        TimeSpan? realDuration = TimeSpan.Zero;
        //        long? ticks = 0;

        //        foreach (var entry in assignment.Entries)
        //        {
        //            ticks += entry.DurationAsTicks;
        //        }

        //        realDuration = TimeSpan.FromTicks((long)ticks);

        //        if (realDuration.Value.Ticks > assignment.DurationPlannedAsTicks)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }

        //    return true;
        //}

        //public TimeSpan? GetActualDuration(int id)
        //{
        //    Assignment assignment = context.Assignments.FirstOrDefault(a => a.Id == id);

        //    if (assignment.Entries.Any())
        //    {
        //        TimeSpan? actualDuration = TimeSpan.Zero;
        //        long? ticks = 0;

        //        foreach (var entry in assignment.Entries)
        //        {
        //            ticks += entry.DurationAsTicks;
        //        }
        //        actualDuration = TimeSpan.FromTicks((long)ticks);
        //        return actualDuration;
        //    }

        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
