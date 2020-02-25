using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.DAL.Interfaces;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.Repositories
{
    public class ProjectTypeRepository : Repository<ProjectType>, IProjectTypeRepository
    {
        TakeMyTimeDbContext context;

        public ProjectTypeRepository(TakeMyTimeDbContext context) : base(context)
        {
            this.context = context;
        }
        public TakeMyTimeDbContext TakeMyTimeDbContext
        {
            get { return DbContext as TakeMyTimeDbContext; }
        }

        public IEnumerable<ProjectType> GetProjectTypesLoaded()
        {
            return this.context.ProjectTypes.Include(pt => pt.Projects);
        }

        public ProjectType GetProjectTypeByIdLoaded(int id)
        {
            return this.context.ProjectTypes.Include(pt => pt.Projects).SingleOrDefault(pt => pt.Id == id);
        }
    }
}
