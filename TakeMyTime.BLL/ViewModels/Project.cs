using BinderDynamics.TakeMyTime.Biz.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.Enums.EnumDefinition;

namespace BinderDynamics.TakeMyTime.Biz.ViewModels
{
    /// <summary>
    /// ViewModel for the (single) Project class
    /// </summary>
    /// 
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public ICollection<EntryViewModel> Entries { get; set; }
        public ICollection<AssignmentViewModel> Assignments { get; set; }
        public TimeSpan? WorkingTime { get; set; }
        public ProjectType? ProjectType { get; set; }
        public int? AllPages { get; set; }
        public int? AllWords { get; set; }
        public bool HasEntries { get; set; }
        [NotMapped]
        public int AssignmentCount { get; set; }
        [NotMapped]
        public int OpenAssignmentCount { get; set; }
        public ProjectStatus ProjectStatus { get; set; }


    }

    /// <summary>
    /// ViewModel for many Projects
    /// </summary>
    public class ProjectListViewModel
    {
        public ProjectListViewModel()
        {
            Projects = new List<ProjectViewModel>();
        }

        public List<ProjectViewModel> Projects { get; set; }
    }
}
