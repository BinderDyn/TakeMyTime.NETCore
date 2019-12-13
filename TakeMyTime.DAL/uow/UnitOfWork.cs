using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DAL.Interfaces;
using TakeMyTime.DAL.Repositories;

namespace TakeMyTime.DAL.uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TakeMyTimeDbContext context;

        public UnitOfWork(/*TakeMyTimeDbContext context*/)
        {
            this.context = new TakeMyTimeDbContext();

            Projects = new ProjectRepository(context);
            Entries = new EntryRepository(context);
            Assignments = new AssignmentRepository(context);
            ProjectTypes = new ProjectTypeRepository(context);
        }

        public IProjectRepository Projects { get; private set; }
        public IEntryRepository Entries { get; private set; }
        public IAssignmentRepository Assignments { get; private set; }
        public IProjectTypeRepository ProjectTypes { get; private set; }

        public int Complete()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
