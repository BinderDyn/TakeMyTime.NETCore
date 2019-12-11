using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DOM.Models;
using TakeMyTime.DAL.uow;

namespace TakeMyTime.Biz.Logic
{
    public class ProjectLogic
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ProjectLogic()
        {

        }

        public Project GetProjectById(int id)
        {
            var result = unitOfWork?.Projects?.Get(id);


            IList<Entry> entries = new List<Entry>();

            foreach (var element in result.Entries)
            {
                Entry entry = new Entry();
                entry.Id = element.Id;
                entry.ProjectId = element.ProjectId;
                entry.Started = element.Started;
                entry.Ended = element.Ended;
                entry.DurationAsTicks = element.DurationAsTicks;
                entry.Comment = element.Comment;
                entry.Name = element.Name;

                entries.Add(entry);
            }

            Project project = new Project();
            project.Id = result.Id;
            project.Description = result.Description;
            project.Name = result.Name;
            project.WorkingTimeAsTicks = result.WorkingTimeAsTicks;
            project.Entries = entries;
            project.Created = result.Created;
            project.ProjectStatus = result.ProjectStatus;

            return project;

        }

        public List<Project> GetAllProjects()
        {
            var projects = unitOfWork?.Projects?.GetAll().ToList();
            return projects;
        }

        public void InsertProject(Project project)
        {
            unitOfWork.Projects.Add(project);
            unitOfWork.Complete();
        }

        public void InsertProjects(IEnumerable<Project> projects)
        {
            unitOfWork.Projects.AddRange(projects);
            unitOfWork.Complete();
        }

        public void UpdateProject(Project project)
        {
            var edit = unitOfWork.Projects.Find(p => p.Id == project.Id).FirstOrDefault();

            if (edit != null)
            {
                edit.Name = project.Name;
                edit.Edited = DateTime.Now;
                edit.Description = project.Description;
                edit.Name = project.Name;
            }

            unitOfWork.Complete();

        }

        public void UpdateProjects(IEnumerable<Project> projects)
        {
            var edits = unitOfWork.Projects.GetAll();

            foreach (var edit in edits)
            {
                foreach (var project in projects)
                {
                    if (edit.Id == project.Id)
                    {
                        edit.Name = project.Name;
                        edit.Description = project.Description;
                        //edit.WorkingTime = project.WorkingTime;
                        edit.WorkingTimeAsTicks = project.WorkingTimeAsTicks;
                        edit.Edited = DateTime.Now;
                    }
                }
            }

            unitOfWork.Complete();
            Dispose();
        }

        public void DeleteProject(int projectId)
        {
            var toBeDeleted = unitOfWork.Projects.Get(projectId);

            unitOfWork.Entries.RemoveRange(toBeDeleted.Entries);
            unitOfWork.Assignments.RemoveRange(toBeDeleted.Assignments);
            unitOfWork.Projects.Remove(toBeDeleted);
            unitOfWork.Complete();
        }

        public void DeleteProjects(IEnumerable<Project> projects)
        {
            IList<Project> entities = new List<Project>();
            foreach (var a in projects)
            {
                var entity = unitOfWork.Projects.Get(a.Id);
                if (entity != null) entities.Add(entity);
            }

            unitOfWork.Projects.RemoveRange(entities);

            unitOfWork.Complete();
        }

        /// <summary>
        /// Returns the complete Working time of the project and all of its entries
        /// </summary>
        /// <returns>The sum of the complete project working time</returns>
        public TimeSpan RetrieveWorkingTime(int projectId)
        {
            return unitOfWork.Projects.RetrieveWorkingTime(projectId);
        }

        public int RetrievePages(int projectId)
        {
            throw new NotImplementedException();
        }

        public int RetrieveWords(int projectId)
        {
            throw new NotImplementedException();
        }

        public void ArchiveProject(int projectId)
        {
            unitOfWork.Projects.ArchiveProject(projectId);
            _ = unitOfWork.Complete();
            Dispose();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
