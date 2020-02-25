using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.Interfaces
{
    public interface IProjectTypeRepository : IRepository<ProjectType>
    {
        IEnumerable<ProjectType> GetProjectTypesLoaded();
        ProjectType GetProjectTypeByIdLoaded(int id);
    }
}
