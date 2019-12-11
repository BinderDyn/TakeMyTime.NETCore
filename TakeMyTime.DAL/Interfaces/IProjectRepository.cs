using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.DAL.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        TimeSpan RetrieveWorkingTime(int projectId);
        void ArchiveProject(int projectId);
    }
}
