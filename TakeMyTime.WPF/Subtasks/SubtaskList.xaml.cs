using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TakeMyTime.BLL.Logic;

using TakeMyTime.Models.Models;
using TakeMyTime.WPF.Utility;

namespace TakeMyTime.WPF.Subtasks
{
    /// <summary>
    /// Interaktionslogik für SubtaskList.xaml
    /// </summary>
    public partial class SubtaskList : Window
    {
        public SubtaskList(int assignment_id)
        {
            InitializeComponent();
            this.PagingManager = new PagingManager<SubtaskGridViewModel>(6);
            Load(assignment_id);
            this.RefreshBindings(1);
        }

        private void RefreshBindings(int page)
        {
            this.lv_Subtasks.ItemsSource = this.PagingManager.Page(page);
            this.btn_CurrentPage.Content = this.PagingManager.CurrentPage;
            this.btn_allPages.Content = this.PagingManager.MaxPage;
            this.btn_PagingForward.IsEnabled = this.PagingManager.CanPageForward;
            this.btn_PagingBack.IsEnabled = this.PagingManager.CanPageBack;
        }

        private void Load(int assignment_id)
        {
            var assignmentLogic = new AssignmentLogic();
            this.Assignment = assignmentLogic.GetAssignmentById(assignment_id);
            this.GridViewModels = Assignment.Subtasks.Select(s => new SubtaskGridViewModel(s)).ToList();
            this.PagingManager.Data = this.GridViewModels;
            assignmentLogic.Dispose();
        }

        #region GUI Events

        private void b_Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void lv_Subtasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool selectionNotNull = e.AddedItems.Count > 0 && e.AddedItems[0] != null;
            if (selectionNotNull)
            {
                this.SelectedSubtask = e.AddedItems[0] as SubtaskGridViewModel;
            }
            this.btn_EditSubtask.IsEnabled = selectionNotNull;
            this.btn_DeleteSubtask.IsEnabled = selectionNotNull;
            this.btn_TickSubtask.IsEnabled = selectionNotNull;
            this.btn_AbortSubtask.IsEnabled = selectionNotNull;
        }

        private void btn_DeleteSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedSubtask != null)
            {
                var assignmentLogic = new AssignmentLogic();
                assignmentLogic.DeleteSubtask(this.Assignment.Id, this.SelectedSubtask.Id);
                assignmentLogic.Dispose();
                Load(this.Assignment.Id);
                RefreshBindings(this.PagingManager.CurrentPage);
            }
        }

        private void btn_EditSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedSubtask != null)
            {
                var subtaskEditDialog = new AddSubtask(this.SelectedSubtask.Id, this.Assignment.Id);
                subtaskEditDialog.ShowDialog();
                Load(this.Assignment.Id);
                RefreshBindings(this.PagingManager.CurrentPage);
            }
        }

        private void btn_AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            var subtaskAddDialog = new AddSubtask(this.Assignment.Id);
            subtaskAddDialog.ShowDialog();
            Load(this.Assignment.Id);
            RefreshBindings(this.PagingManager.CurrentPage);
        }

        private void btn_PagingBack_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshBindings(this.PagingManager.CurrentPage - 1);
        }

        private void btn_PagingForward_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshBindings(this.PagingManager.CurrentPage + 1);
        }

        private void btn_SaveSubtasks_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_AbortSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedSubtask != null)
            {
                var subtaskLogic = new SubtaskLogic();
                subtaskLogic.SetSubtaskAborted(this.SelectedSubtask.Id);
                subtaskLogic.Dispose();
                this.Load(this.Assignment.Id);
                RefreshBindings(this.PagingManager.CurrentPage);
            }
        }

        private void btn_TickSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedSubtask != null)
            {
                var subtaskLogic = new SubtaskLogic();
                subtaskLogic.SetSubtaskTickedOff(this.SelectedSubtask.Id);
                subtaskLogic.Dispose();
                this.Load(this.Assignment.Id);
                RefreshBindings(this.PagingManager.CurrentPage);
            }
        }

        #endregion

        public Assignment Assignment { get; set; }
        public List<SubtaskGridViewModel> GridViewModels { get; set; }
        public SubtaskGridViewModel SelectedSubtask { get; set; }
        public PagingManager<SubtaskGridViewModel> PagingManager { get; set; }

        
    }
}
