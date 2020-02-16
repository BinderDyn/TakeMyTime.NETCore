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
            Load(assignment_id);
        }

        private void Load(int assignment_id)
        {
            var assignmentLogic = new AssignmentLogic();
            this.Assignment = assignmentLogic.GetAssignmentById(assignment_id);
            this.GridViewModels = Assignment.Subtasks.Select(s => new SubtaskGridViewModel(s)).ToList();
            this.lv_Subtasks.ItemsSource = this.GridViewModels;
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
        }

        private void btn_DeleteSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedSubtask != null)
            {
                var assignmentLogic = new AssignmentLogic();
                assignmentLogic.DeleteSubtask(this.Assignment.Id, this.SelectedSubtask.Id);
                assignmentLogic.Dispose();
                Load(this.Assignment.Id);
            }
        }

        private void btn_EditSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedSubtask != null)
            {
                var subtaskEditDialog = new AddSubtask(this.SelectedSubtask.Id, this.Assignment.Id);
                subtaskEditDialog.ShowDialog();
                Load(this.Assignment.Id);
            }
        }

        private void btn_AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            var subtaskAddDialog = new AddSubtask(this.Assignment.Id);
            subtaskAddDialog.ShowDialog();
            Load(this.Assignment.Id);
        }

        private void btn_SaveSubtasks_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        public Assignment Assignment { get; set; }
        public List<SubtaskGridViewModel> GridViewModels { get; set; }
        public SubtaskGridViewModel SelectedSubtask { get; set; }
    }
}
