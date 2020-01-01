using BinderDyn.LoggingUtility;
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
            Logger.Debug(string.Format("{0}.Complete() : {1}", GetType().FullName, GetTrackedEntitiesAsString(context)));
            return context.SaveChanges();
        }

        private string GetTrackedEntitiesAsString(TakeMyTimeDbContext context)
        {
            var sb = new StringBuilder();
            foreach (var entity in context.ChangeTracker.Entries())
            {
                sb.AppendLine(string.Format("Tracked entity: {0}", entity));
            }
            return sb.ToString();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
