using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DAL.uow;
using TakeMyTime.Models.Models;

namespace TakeMyTime.BLL.Logic
{
    public class ProjectTypeLogic
    {
        private readonly UnitOfWork unitOfWork;

        public ProjectTypeLogic(UnitOfWork uow = null)
        {
            if (uow != null)
            {
                this.unitOfWork = uow;
            }
            else
            {
                this.unitOfWork = new UnitOfWork();
            }
        }

        public IEnumerable<ProjectType> GetProjectTypes()
        {
            return unitOfWork.ProjectTypes.GetAll();
        }

        public ProjectType GetProjectType(int id)
        {
            return unitOfWork.ProjectTypes.Get(id);
        }

        public void AddProjectType(ProjectType.ICreateParam param)
        {
            var projectType = ProjectType.Create(param);
            unitOfWork.ProjectTypes.Add(projectType);
            unitOfWork.Complete();
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }
    }
}
