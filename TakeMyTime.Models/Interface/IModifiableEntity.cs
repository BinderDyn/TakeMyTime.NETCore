using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMyTime.DOM.Interfaces
{
    /// <summary>
    /// Interface to guarantee that there is no diversion between ViewModels and Entities (expandable by needs)
    /// </summary>
    public interface IModifiableEntity
    {
        string Name { get; set; }
    }
}
