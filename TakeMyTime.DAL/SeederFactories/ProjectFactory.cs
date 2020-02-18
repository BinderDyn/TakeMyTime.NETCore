using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.SeederFactories
{
    public class ProjectFactory
    {
        public static IEnumerable<Project> SeedProjects(ProjectType[] seededProjectTypes)
        {
            return new Project[6]
            {
                Project.Create(new ProjectCreateParam { ProjectType = seededProjectTypes[0], Name = "My book", Description = "My first book to be published" }),
                Project.Create(new ProjectCreateParam { ProjectType = seededProjectTypes[0], Name = "Top book", Description = "This book is gonna be great" }),
                Project.Create(new ProjectCreateParam { ProjectType = seededProjectTypes[1], Name = "TakeMyTime", Description = "The greatest app of all time" }),
                Project.Create(new ProjectCreateParam { ProjectType = seededProjectTypes[1], Name = "SuperHack", Description = "This hack is going places!!!" }),
                Project.Create(new ProjectCreateParam { ProjectType = seededProjectTypes[2], Name = "Weekly training", Description = "I'm going to be strong!" }),
                Project.Create(new ProjectCreateParam { ProjectType = seededProjectTypes[3], Name = "Organisation & more", Description = "I need to get more organized" }),
            };
        }

        private class ProjectCreateParam : Project.ICreateParam
        {
            public ProjectType ProjectType { get; set; }
            public string Description { get; set; }
            public string Name { get; set; }
        }


    }
}
