﻿using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.WPF.Entries
{
    public class EntryUpdateParam : Entry.IUpdateParam
    {
        public string Name { get; set; }
        public string Comment { get; set; }
    }
}
