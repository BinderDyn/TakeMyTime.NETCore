using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TakeMyTime.BLL.Logic;
using TakeMyTime.WPF.Utility;

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
            this.PagingManager = new PagingManager<ProjectTypeViewModel>();
            this.Load();
            this.RefreshBindings(1);
        }

        private void Load()
        {
            var projectTypeLogic = new ProjectTypeLogic();
            this.ProjectTypeViewModels = projectTypeLogic.GetProjectTypes().Select(pt => new ProjectTypeViewModel(pt)).ToList();
            this.PagingManager.Data = this.ProjectTypeViewModels.ToList();
        }

        private void RefreshBindings(int page)
        {
            this.dg_ProjectTypes.ItemsSource = this.PagingManager.Page(page);
            this.btn_CurrentPage.Content = this.PagingManager.CurrentPage;
            this.btn_allPages.Content = this.PagingManager.MaxPage;
            this.btn_PagingForward.IsEnabled = this.PagingManager.CanPageForward;
            this.btn_PagingBack.IsEnabled = this.PagingManager.CanPageBack;
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
            var addEditProjectTypeDialog = new AddEditProjectType();
            addEditProjectTypeDialog.ShowDialog();
            this.Load();
            this.RefreshBindings(1);
        }

        private void btn_EditProjectType_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var addEditProjectTypeDialog = new AddEditProjectType(this.SelectedProjectType.Id);
            addEditProjectTypeDialog.ShowDialog();
            this.Load();
            this.RefreshBindings(this.PagingManager.CurrentPage);
        }

        private void btn_DeleteProjectType_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.SelectedProjectType != null)
            {
                var projectTypeLogic = new ProjectTypeLogic();
                projectTypeLogic.DeleteProjectType(this.SelectedProjectType.Id);
                this.SelectedProjectType = null;
                this.Load();
                this.RefreshBindings(this.PagingManager.CurrentPage);
            }
        }

        private void btn_PagingForward_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.RefreshBindings(this.PagingManager.CurrentPage + 1);
        }

        private void btn_PagingBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.RefreshBindings(this.PagingManager.CurrentPage - 1);
        }

        #endregion

        #region Properties

        public IEnumerable<ProjectTypeViewModel> ProjectTypeViewModels { get; set; }
        public ProjectTypeViewModel SelectedProjectType { get; set; }
        public PagingManager<ProjectTypeViewModel> PagingManager { get; set; }

        #endregion
    }
}
