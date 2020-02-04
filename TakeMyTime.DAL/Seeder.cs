using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.DAL.SeederFactories;
using TakeMyTime.DAL.uow;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL
{
    public class Seeder
    {
        public static void Seed()
        {
            TakeMyTimeDbContext context = new TakeMyTimeDbContext();

            bool hasProjectTypes = context.ProjectTypes != null && context.ProjectTypes.Any();
            if (!hasProjectTypes)
            {
                var projectTypes = ProjectTypeFactory.SeedProjectTypes();
                context.ProjectTypes.AddRange(projectTypes);
                context.SaveChanges();

                var projects = ProjectFactory.SeedProjects(projectTypes.ToArray());
                context.Projects.AddRange(projects);
                context.SaveChanges();
            }

            bool hasAssignments = context.Assignments != null && context.Assignments.Any();
            if(!hasAssignments)
            {
                var projects = context.Projects.ToList();
                var assignments = AssignmentFactory.CreateAssignments(projects.ToArray());
                context.Assignments.AddRange(assignments);
                context.SaveChanges();
            }

            bool hasSubtasks = context.Subtasks != null && context.Subtasks.Any();
            if (!hasSubtasks)
            {
                var assignments = context.Assignments.ToList();
                var subtasks = new List<Subtask>();
                foreach (var assignment in assignments)
                {
                    subtasks.AddRange(SubtaskFactory.Create(assignment));
                }

                context.Subtasks.AddRange(subtasks);
                context.SaveChanges();
            }

            bool hasEntries = context.Entries != null && context.Entries.Any();
            if (!hasEntries)
            {
                var subtasks = context.Subtasks
                    .Include(s => s.Assignment)
                    .Include(s => s.Assignment.Project)
                    .ToList();
                var entries = new List<Entry>();
                foreach (var subtask in subtasks)
                {
                    entries.AddRange(EntryFactory.Create(subtask));
                }

                context.Entries.AddRange(entries);
                context.SaveChanges();
            }
        }


    }
}
