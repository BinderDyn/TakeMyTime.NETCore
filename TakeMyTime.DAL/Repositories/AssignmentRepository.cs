using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DAL.Interfaces;
using TakeMyTime.DOM.Models;

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

        public bool CheckForTimePlan(int id)
        {
            Assignment assignment = context.Assignments.FirstOrDefault(a => a.Id == id);

            if (assignment.Entries.Any())
            {
                TimeSpan? realDuration = TimeSpan.Zero;
                long? ticks = 0;

                foreach (var entry in assignment.Entries)
                {
                    ticks += entry.DurationAsTicks;
                }

                realDuration = TimeSpan.FromTicks((long)ticks);

                if (realDuration.Value.Ticks > assignment.DurationPlannedAsTicks)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return true;
        }

        public TimeSpan? GetActualDuration(int id)
        {
            Assignment assignment = context.Assignments.FirstOrDefault(a => a.Id == id);

            if (assignment.Entries.Any())
            {
                TimeSpan? actualDuration = TimeSpan.Zero;
                long? ticks = 0;

                foreach (var entry in assignment.Entries)
                {
                    ticks += entry.DurationAsTicks;
                }
                actualDuration = TimeSpan.FromTicks((long)ticks);
                return actualDuration;
            }

            else
            {
                return null;
            }
        }
    }
}
