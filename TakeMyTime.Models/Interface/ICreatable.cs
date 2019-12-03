using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.DOM.Interfaces
{
    public interface ICreatable<T>
    {
        T Create();
    }
}
