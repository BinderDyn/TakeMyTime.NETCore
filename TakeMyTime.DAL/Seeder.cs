using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.DAL.SeederFactories;
using TakeMyTime.DAL.uow;

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
        }


    }
}
