using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.Enums.EnumDefinition;
using TakeMyTime.DOM.Interfaces;

namespace TakeMyTime.DOM.Models
{
    /// <summary>
    /// The DB model of Project
    /// </summary>
    public class Project : Entity<Project>, IInitiable, ICreatable<Project>
    {
        public void Init()
        {
            this.Entries = new HashSet<Entry>();
            this.Assignments = new HashSet<Assignment>();
            this.ProjectStatus = ProjectStatus.Active;
        }

        public Project Create()
        {
            Project project = new Project();
            project.Init();
            return project;
        }

        [Key]
        new public int Id { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public string Description { get; set; }
        public long? WorkingTimeAsTicks { get; set; }
        public ProjectType? ProjectType { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
    }
}
