using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DAL.Interfaces;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.DAL.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository 
    {
        TakeMyTimeDbContext context;

        public ProjectRepository(TakeMyTimeDbContext context) : base(context)
        {
            this.context = context;
        }

        public TakeMyTimeDbContext TakeMyTimeDbContext
        {
            get { return DbContext as TakeMyTimeDbContext; }
        }

        public void ArchiveProject(int projectId)
        {
            var project = context.Projects.SingleOrDefault(p => p.Id == projectId);
            project.ProjectStatus = EnumDefinition.ProjectStatus.Archived;
        }

        public TimeSpan RetrieveWorkingTime(int projectId)
        {
            var project = context.Projects.SingleOrDefault(p => p.Id == projectId);
            TimeSpan completeWorkingTime = new TimeSpan();
            if (project != null)
            {
                long duration = project.Entries.Where(e => e.DurationAsTicks.HasValue).Sum(e => e.DurationAsTicks.Value);
                completeWorkingTime = TimeSpan.FromTicks(duration);
            }

            return completeWorkingTime;
        }
    }
}
