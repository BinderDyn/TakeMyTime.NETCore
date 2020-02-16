using System;
using System.Collections.Generic;
using System.Linq;
using TakeMyTime.DAL.uow;
using TakeMyTime.BLL.ViewModels;
using static Common.Enums.EnumDefinition;
using TakeMyTime.Models.Models;

namespace TakeMyTime.BLL.Logic
{
    public class ProjectLogic
    {
        private readonly UnitOfWork unitOfWork;

        public ProjectLogic(UnitOfWork uow = null)
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

        public Project GetProjectById(int id)
        {
            return unitOfWork.Projects.Get(id);
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return this.unitOfWork.Projects.LoadAll();
        }

        public void InsertProject(Project.ICreateParam viewModel)
        {
            var insert = Project.Create(viewModel);
            var projectType = unitOfWork.ProjectTypes.Get(viewModel.ProjectType.Id);
            projectType.AddProject(insert);
            unitOfWork.Complete();
        }

        public void UpdateProject(Project.IUpdateParam param, int project_id)
        {
            var toBeUpdated = unitOfWork.Projects.Get(project_id);
            if (toBeUpdated != null)
            {
                toBeUpdated.Update(param);
                unitOfWork.Complete();
            }
        }

        public void SetStatus(int project_id, ProjectStatus status)
        {
            var toBeUpdated = unitOfWork.Projects.Get(project_id);
            if (toBeUpdated != null)
            {
                toBeUpdated.SetStatus(status);
                unitOfWork.Complete();
            }
        }

        public void DeleteProject(int project_id)
        {
            var toBeDeleted = unitOfWork.Projects.GetProjectById(project_id);
            if (toBeDeleted != null && toBeDeleted.CanDelete)
            {
                unitOfWork.Projects.Remove(toBeDeleted);
                unitOfWork.Complete();
            }
            else
            {
                throw new Exception("Cannot delete project if is not archived or contains not done assignments!");
            }
        }

        public void DeleteProjects(IEnumerable<Project> projects)
        {
            IList<Project> toBeDeleted = new List<Project>();
            foreach (var p in projects)
            {
                var entity = unitOfWork.Projects.Get(p.Id);
                if (entity != null) toBeDeleted.Add(entity);
            }

            unitOfWork.Projects.RemoveRange(toBeDeleted);
            unitOfWork.Complete();
        }

        /// <summary>
        /// Returns the complete Working time of the project and all of its entries
        /// </summary>
        /// <returns>The sum of the complete project working time</returns>
        public TimeSpan RetrieveWorkingTime(int project_id)
        {
            return unitOfWork.Projects.RetrieveWorkingTime(project_id);
        }

        public void ToggleProjectStatus(int project_id)
        {
            unitOfWork.Projects.ToggleProjectStatus(project_id);
            _ = unitOfWork.Complete();
            Dispose();
        }

        public IEnumerable<Project> GetAllActiveProjects()
        {
            return unitOfWork.Projects.LoadAll().Where(p => p.ProjectStatus == ProjectStatus.Active);
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
