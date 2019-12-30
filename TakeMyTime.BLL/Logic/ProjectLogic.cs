using System;
using System.Collections.Generic;
using System.Linq;
using TakeMyTime.DOM.Models;
using TakeMyTime.DAL.uow;
using TakeMyTime.BLL.ViewModels;
using static Common.Enums.EnumDefinition;

namespace TakeMyTime.BLL.Logic
{
    public class ProjectLogic
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        public ProjectLogic()
        {
        }

        public Project GetProjectById(int id)
        {
            return unitOfWork.Projects.Get(id);
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return unitOfWork.Projects.LoadAll();
        }

        public void InsertProject(Project.ICreateParam viewModel)
        {
            var insert = Project.Create(viewModel);
            var projectType = unitOfWork.ProjectTypes.Get(viewModel.ProjectType.Id);
            projectType.AddProject(insert);
            unitOfWork.Complete();
        }

        public void InsertProjects(IEnumerable<ProjectCreateViewModel> projects)
        {
            if (projects != null && projects.Any())
            {
                var mapped = projects.Select(p => Project.Create(p));
                unitOfWork.Projects.AddRange(mapped);
                unitOfWork.Complete();
            }
        }

        public void UpdateProject(ProjectUpdateViewModel param, int project_id)
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
            var toBeDeleted = unitOfWork.Projects.Get(project_id);
            if (toBeDeleted != null && toBeDeleted.CanDelete)
            {
                unitOfWork.Projects.Remove(toBeDeleted);
                unitOfWork.Complete();
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

        public void ArchiveProject(int project_id)
        {
            unitOfWork.Projects.ArchiveProject(project_id);
            _ = unitOfWork.Complete();
            Dispose();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
