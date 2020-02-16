using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.Interfaces
{
    public interface IEntryRepository : IRepository<Entry>
    {
        IEnumerable<Entry> LoadAll();
    }
}
