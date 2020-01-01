using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;

namespace TakeMyTime.BLL.ViewModels
{
    public class ProjectCreateViewModel : Project.ICreateParam
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectType ProjectType { get; set; }
    }
}
