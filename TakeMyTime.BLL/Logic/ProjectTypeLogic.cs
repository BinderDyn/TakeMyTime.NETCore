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

        public void UpdateProjectType(int id, ProjectType.IUpdateParam param)
        {
            var projectType = this.unitOfWork.ProjectTypes.Get(id);
            projectType.Update(param);
            unitOfWork.Complete();
        }

        public void DeleteProjectType(int id)
        {
            var projectType = this.unitOfWork.ProjectTypes.Get(id);
            if (projectType.CanDelete())
            {
                this.unitOfWork.ProjectTypes.Remove(projectType);
                this.unitOfWork.Complete();
            }
            else
            {
                throw new Exception("Cannot delete project type used in existing projects");
            }
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }
    }
}
