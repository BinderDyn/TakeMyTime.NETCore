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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TakeMyTime.BLL.Logic;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;
using TakeMyTime.WPF.Subtasks;

namespace TakeMyTime.WPF.Assignments
{
    /// <summary>
    /// Interaktionslogik für AddAssignment.xaml
    /// </summary>
    public partial class AddAssignment : Window
    {
        public AddAssignment()
        {
            InitializeComponent();
        }

        private void Load()
        {
            if(!EditMode)
            {
                this.SubtasksViewModels = this.Subtasks.OrderByDescending(s => s.Priority)
                                                       .Select(s => new SubtaskGridViewModel(s))
                                                       .ToList();
            }
            else
            {
                this.Subtasks = this.Assignment.Subtasks.ToList();
                this.SubtasksViewModels = this.Subtasks.OrderByDescending(s => s.Priority)
                                                       .Select(s => new SubtaskGridViewModel(s))
                                                       .ToList();
            }

            this.lv_Subtasks.ItemsSource = this.SubtasksViewModels;
        }

        #region GUI Events

        private void b_Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_AddAssignment_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btn_DeleteSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedSubtask != null)
            {
                this.Subtasks.Remove(SelectedSubtask);
                if (this.SelectedSubtask.Assignment != null)
                {
                    var subtaskLogic = new SubtaskLogic();
                    subtaskLogic.Delete(SelectedSubtask);
                    subtaskLogic.Dispose();
                }
                this.SelectedSubtask = null;
                Load();
            }
        }

        private void btn_EditSubtask_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSubtask != null)
            {
                AddSubtask addSubtaskWindow = new AddSubtask(this, this.SelectedSubtask);
                addSubtaskWindow.ShowDialog();
                Load();
            }
        }

        private void btn_AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            AddSubtask addSubtaskWindow = new AddSubtask(this);
            addSubtaskWindow.ShowDialog();
            Load();
        }

        private void lv_Subtasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedViewModel = (e.AddedItems[0] as SubtaskGridViewModel);
                if(selectedViewModel.Id != 0)
                {
                    this.SelectedSubtask = this.Subtasks.Single(s => s.Id == selectedViewModel.Id);
                } 
                else
                {
                    this.SelectedSubtask = (this.Subtasks
                        .FirstOrDefault(s => s.Name == selectedViewModel.Name &&
                                s.Description == selectedViewModel.Description));
                }
            }
        }

        #endregion



        public List<Subtask> Subtasks { get; set; } = new List<Subtask>();
        public List<SubtaskGridViewModel> SubtasksViewModels { get; set; } = new List<SubtaskGridViewModel>();
        public Subtask SelectedSubtask { get; set; }
        public Assignment Assignment { get; set; }
        public bool EditMode { get; set; } = false;

       
    }
}
