using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DAL.uow;
using TakeMyTime.Models.Models;

namespace TakeMyTime.BLL.Logic
{
    public class ProjectTypeLogic
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<ProjectType> GetProjectTypes()
        {
            return unitOfWork.ProjectTypes.GetAll();
        }

        public ProjectType GetProjectType(int id)
        {
            return unitOfWork.ProjectTypes.Get(id);
        }

        public void AddProjectType()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }
    }
}
