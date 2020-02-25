using Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.DAL.Interfaces;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.Repositories
{
    public class SubtaskRepository : Repository<Subtask>, ISubtaskRepository
    {
        TakeMyTimeDbContext context;

        public SubtaskRepository(TakeMyTimeDbContext dbContext) : base(dbContext)
        {
            this.context = dbContext;
        }

        public void SetStatus(int id, EnumDefinition.SubtaskStatus status)
        {
            var edit = context.Subtasks.FirstOrDefault(s => s.Id == id);

            if (edit == null) throw new Exception("No entity found for given id");

            edit.SetStatus(status);
        }

        public Subtask GetSubtaskFullyLoaded(int id)
        {
            return this.context.Subtasks
                .Include(s => s.Entries)
                .FirstOrDefault(s => s.Id == id);
        }
    }
}
