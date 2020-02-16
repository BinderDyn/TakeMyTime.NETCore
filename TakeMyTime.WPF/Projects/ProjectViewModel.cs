using Common.Enums;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using TakeMyTime.Models.Models;
using TakeMyTime.WPF.Utility;

namespace TakeMyTime.WPF.Projects
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        private string description;
        private EnumDefinition.ProjectStatus status;
        private string name;

        public ProjectViewModel() { }

        public ProjectViewModel(Project project)
        {
            this.Id = project.Id;
            this.name = project.Name;
            this.description = project.Description;
            this.status = project.ProjectStatus;
            this.created = project.Created;
            this.edited = project.Edited;
            this.Type = project.ProjectType.Name;
        }

        public int Id { get; set; }

        

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                NotifyPropertyChanged();
            }
        }
        public string Description 
        {
            get 
                {
                    return this.description;
                } 
            set 
            {
                this.description = value;
                NotifyPropertyChanged();
            }
        }
        public string Type { get; set; }
        public EnumDefinition.ProjectStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
                NotifyPropertyChanged();
            }
        }
        private DateTime created { get; set; }
        private DateTime? edited { get; set; }
        public string CreatedAsString { get => DateTimeCultureConverter.ConvertToLocalDateTimeFormat(this.created); }
        public string EditedAsString { get => DateTimeCultureConverter.ConvertToLocalDateTimeFormat(this.edited); }
        public string StatusTooltip { get => GetStatusString(this.Status); }
        public Uri StatusImage { get => GetImageByStatus(this.Status); }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
