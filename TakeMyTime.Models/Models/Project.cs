using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.Enums.EnumDefinition;
using TakeMyTime.DOM.Interfaces;
using TakeMyTime.Models.Models;

namespace TakeMyTime.Models.Models
{
    /// <summary>
    /// The DB model of Project
    /// </summary>
    public class Project : Entity<Project>
    {
        public Project() { }

        public static Project Create(ICreateParam createParam)
        {
            Project project = new Project();
            project.Init(createParam.Name, createParam.Description, createParam.ProjectType);
            return project;
        }

        public void Init(string name, string description, ProjectType projectType)
        {
            this.Name = name;
            this.Description = description;
            this.ProjectType = projectType;
            this.ProjectType_Id = projectType.Id;
            this.Entries = new HashSet<Entry>();
            this.Assignments = new HashSet<Assignment>();
            this.ProjectStatus = ProjectStatus.Active;
            this.SetCreated();
        }

        public void Update(IUpdateParam param)
        {
            this.Description = param.Description;
            this.Name = param.Name;
            this.SetEdited();
        }

        public void SetStatus(ProjectStatus status)
        {
            this.ProjectStatus = status;
        }

        private bool CheckCanDelete()
        {
            bool allAssignmentsDoneOrAborted = 
                this.Assignments == null ||
                this.Assignments.Any(a => a.AssignmentStatus != AssignmentStatus.Done || a.AssignmentStatus != AssignmentStatus.Aborted)
                ||
                this.Assignments.Count == 0;
            return this.ProjectStatus == ProjectStatus.Archived && allAssignmentsDoneOrAborted;
        }

        public void ClearAssignmentsAndEntries()
        {
            this.Assignments.Clear();
            this.Entries.Clear();
        }

        public interface IUpdateParam
        {
            string Description { get; set; }
            string Name { get; set; }
        }

        public interface ICreateParam : IUpdateParam
        {
            ProjectType ProjectType { get; set; }
        }

        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual string Description { get; set; }
        public virtual long? WorkingTimeAsTicks { get; set; }
        public virtual ProjectStatus ProjectStatus { get; set; }
        [ForeignKey("ProjectType")]
        public virtual int ProjectType_Id { get; set; }
        public virtual ProjectType ProjectType { get; set; }
        [NotMapped]
        public bool CanDelete { get => this.CheckCanDelete(); }
    }
}
