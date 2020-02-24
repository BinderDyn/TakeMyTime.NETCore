using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.WPF.ProjectTypes
{
    public class ProjectTypeViewModel : ProjectType.IUpdateParam
    {
        public ProjectTypeViewModel()
        {

        }

        public ProjectTypeViewModel(ProjectType projectType)
        {
            this.Id = projectType.Id;
            this.Name = projectType.Name;
            this.Description = projectType.Description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
