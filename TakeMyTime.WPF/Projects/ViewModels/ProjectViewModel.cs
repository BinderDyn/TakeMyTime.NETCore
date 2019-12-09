using Common.Enums;
using System.Drawing;
using System.Resources;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.WPF.Projects.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectViewModel() {}

        public ProjectViewModel(Project project)
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EnumDefinition.ProjectStatus Status { get; set; }
        public Bitmap StatusImage { get; set; }
        public static Image GetImageByStatus(EnumDefinition.ProjectStatus status)
        {
            return status switch
            {
                //EnumDefinition.ProjectStatus.Active => Resources.Images.activeIcon,
                //EnumDefinition.ProjectStatus.Archived => Resources.Images.archiveIcon,
                _ => null
            };
        }
    }
}
