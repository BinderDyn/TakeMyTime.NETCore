﻿using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.WPF.Projects
{
    public class ProjectUpdateViewModel : Project.IUpdateParam
    {
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
