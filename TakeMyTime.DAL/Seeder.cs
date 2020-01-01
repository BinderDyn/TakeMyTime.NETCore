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

            var hasProjects = context.ProjectTypes != null && context.ProjectTypes.Any();
            if(!hasProjects)
            {
                context.ProjectTypes.AddRange(ProjectTypeFactory.SeedProjectTypes());
            }

            context.SaveChanges();
        }

        
    }
}
