using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.WPF.Projects
{
    public class ProjectTypeViewModel : ProjectType.ICreateParam
    {
        public ProjectTypeViewModel()
        {
        }

        public ProjectTypeViewModel(ProjectType projectType)
        {
            this.Description = projectType.Description;
            this.Name = projectType.Name;
            this.Id = projectType.Id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
