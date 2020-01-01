using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.SeederFactories
{
    public class ProjectTypeFactory
    {
        public static IEnumerable<ProjectType> SeedProjectTypes()
        {
            return new ProjectType[4]
            {
                ProjectType.Create(new ProjectTypeCreateParam { Name = "Creative Writing", Description = "A project for creative writing" }),
                ProjectType.Create(new ProjectTypeCreateParam { Name = "Programming", Description = "A project for creating new applications" }),
                ProjectType.Create(new ProjectTypeCreateParam { Name = "Sports", Description = "Steeling your body" }),
                ProjectType.Create(new ProjectTypeCreateParam { Name = "Personal Management", Description = "Get your stuff organized" })
            };
        }

        private class ProjectTypeCreateParam : ProjectType.ICreateParam
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
