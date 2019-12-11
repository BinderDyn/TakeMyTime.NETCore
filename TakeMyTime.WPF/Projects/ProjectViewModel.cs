using Common.Enums;
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
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EnumDefinition.ProjectStatus Status { get; set; }
        public Bitmap StatusImage { get => GetImageByStatus(this.Status); }
        private Bitmap GetImageByStatus(EnumDefinition.ProjectStatus status)
        {
            return status switch
            {
                EnumDefinition.ProjectStatus.Active => BitmapConverter.ConvertFromBytes(Resources.Images.activeIcon),
                EnumDefinition.ProjectStatus.Archived => BitmapConverter.ConvertFromBytes(Resources.Images.archiveIcon),
                _ => null
            };
        }
    }
}
