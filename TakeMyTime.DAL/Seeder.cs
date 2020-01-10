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

            var hasProjectTypes = context.ProjectTypes != null && context.ProjectTypes.Any();
            if (!hasProjectTypes)
            {
                var projectTypes = ProjectTypeFactory.SeedProjectTypes();
                context.ProjectTypes.AddRange(projectTypes);
                context.SaveChanges();

                var projects = ProjectFactory.SeedProjects(projectTypes.ToArray());
                context.Projects.AddRange(projects);
                context.SaveChanges();
            }
        }


    }
}
