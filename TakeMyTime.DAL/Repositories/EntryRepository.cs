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
    public class EntryRepository : Repository<Entry>, IEntryRepository
    {
        public EntryRepository(TakeMyTimeDbContext context) : base(context)
        {

        }

        public TakeMyTimeDbContext TakeMyTimeDbContext
        {
            get { return DbContext as TakeMyTimeDbContext; }
        }

        public IEnumerable<Entry> LoadAll()
        {
            return this.TakeMyTimeDbContext.Entries
                .Include(e => e.Project)
                .Include(e => e.Subtask);
        }
    }
}
