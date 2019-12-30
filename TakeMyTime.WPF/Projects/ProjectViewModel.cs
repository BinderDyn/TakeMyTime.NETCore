using Common.Enums;
using System;
using System.Drawing;
using TakeMyTime.DOM.Models;
using TakeMyTime.WPF.Utility;

namespace TakeMyTime.WPF.Projects
{
    public class ProjectViewModel
    {
        public ProjectViewModel() { }

        public ProjectViewModel(Project project)
        {
            this.Id = Id;
            this.Name = project.Name;
            this.Description = project.Description;
            this.Status = project.ProjectStatus;
            this.created = project.Created;
            this.Type = project.ProjectType.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public EnumDefinition.ProjectStatus Status { get; set; }
        private DateTime created { get; set; }
        public string CreatedAsString { get => DateTimeCultureConverter.ConvertToLocalDateTimeFormat(this.created); }
        public string StatusTooltip { get => GetStatusString(this.Status); }
        public Uri StatusImage { get => GetImageByStatus(this.Status); }
        private Uri GetImageByStatus(EnumDefinition.ProjectStatus status)
        {
            return status switch
            {
                EnumDefinition.ProjectStatus.Active => new Uri("pack://application:,,,/Images/activeIconSmall.png"),
                EnumDefinition.ProjectStatus.Archived => new Uri("pack://application:,,,/Images/archiveIconSmall.png"),
                _ => null
            };
        }

        private string GetStatusString(EnumDefinition.ProjectStatus status)
        {
            return status switch
            {
                EnumDefinition.ProjectStatus.Active => Resources.ProjectOverview.StatusTooltipActive,
                EnumDefinition.ProjectStatus.Archived => Resources.ProjectOverview.StatusTooltipArchived,
                _ => null
            };
        }
    }
}
