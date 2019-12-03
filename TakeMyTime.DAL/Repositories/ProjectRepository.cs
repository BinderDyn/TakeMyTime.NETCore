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
            project.ProjectStatus = Common.Enums.EnumDefinition.ProjectStatus.Archived;
        }

        public int GetPages(int projectId)
        {
            var entries = context.Entries.Where(e => e.ProjectId == projectId).ToList();
            int allPages = 0;
            foreach (var entry in entries)
            {
                if (entry.Pages != null)
                {
                    allPages += (int)entry.Pages;
                }
            }
            return allPages;
        }

        public int GetWords(int projectId)
        {
            var entries = context.Entries.Where(e => e.ProjectId == projectId).ToList();
            int allWords = 0;
            foreach (var entry in entries)
            {
                if (entry.Words != null)
                {
                    allWords += (int)entry.Words;
                }
            }
            return allWords;
        }

        public TimeSpan RetrieveWorkingTime(int projectId)
        {
            var entries = context.Entries.Where(e => e.ProjectId == projectId).ToList();
            var completeWorkingTime = new TimeSpan();
            foreach (var entry in entries)
            {
                if (entry.DurationAsTicks != null)
                {
                    completeWorkingTime += TimeSpan.FromTicks(entry.DurationAsTicks.Value);
                }
            }
            return completeWorkingTime;
        }
    }
}
