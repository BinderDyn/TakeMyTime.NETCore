﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TakeMyTime.DOM.Interfaces;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.Models.Models
{
    public class ProjectType : Entity<ProjectType>, IModifiableEntity
    {
        public static ProjectType Create(ICreateParam param)
        {
            var type = new ProjectType();
            type.Init(param.Name, param.Description);
            return type;
        }

        public interface ICreateParam
        {
            string Name { get; set; }
            string Description { get; set; }
        }

        public void Init(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Projects = new HashSet<Project>();
            this.SetCreated();
        }

        public void Update(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.SetEdited();
        }

        public void AddProject(Project project)
        {
            this.Projects.Add(project);
        }

        public void RemoveProject(Project project)
        {
            this.Projects.Remove(project);
        }

        [Key]
        new public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<Project> Projects { get; private set; }
    }
}
