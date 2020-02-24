using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TakeMyTime.BLL.Logic;

namespace TakeMyTime.WPF.ProjectTypes
{
    /// <summary>
    /// Interaktionslogik für ProjectTypeOverview.xaml
    /// </summary>
    public partial class ProjectTypeOverview : Page
    {
        public ProjectTypeOverview()
        {
            InitializeComponent();
            this.Load();
            this.RefreshBindings();
        }

        private void Load()
        {
            var projectTypeLogic = new ProjectTypeLogic();
            this.ProjectTypeViewModels = projectTypeLogic.GetProjectTypes().Select(pt => new ProjectTypeViewModel(pt)).ToList();
        }

        private void RefreshBindings()
        {
            this.dg_ProjectTypes.ItemsSource = this.ProjectTypeViewModels;
        }

        #region GUI Events

        private void dg_ProjectTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool selectionNotNull = e.AddedItems != null && e.AddedItems.Count > 0;
            btn_EditProjectType.IsEnabled = selectionNotNull;
            btn_DeleteProjectType.IsEnabled = selectionNotNull;
            if (selectionNotNull)
            {
                this.SelectedProjectType = e.AddedItems[0] as ProjectTypeViewModel;
            }
            else
            {
                this.SelectedProjectType = null;
            }
        }

        private void btn_AddProjectType_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btn_EditProjectType_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btn_DeleteProjectType_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.SelectedProjectType != null)
            {
                var projectTypeLogic = new ProjectTypeLogic();
                projectTypeLogic.DeleteProjectType(this.SelectedProjectType.Id);
                this.SelectedProjectType = null;
                this.Load();
                this.RefreshBindings();
            }
        }

        #endregion

        #region Properties

        public IEnumerable<ProjectTypeViewModel> ProjectTypeViewModels { get; set; }
        public ProjectTypeViewModel SelectedProjectType { get; set; }


        #endregion

        
    }
}
