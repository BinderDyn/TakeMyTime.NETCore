using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.BLL.ViewModels
{
    public class ProjectUpdateViewModel : Project.IUpdateParam
    {
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
