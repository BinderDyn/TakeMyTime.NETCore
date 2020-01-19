using Common.Enums;
using System;
using System.Collections.Generic;
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
using TakeMyTime.WPF.Assignments;
using TakeMyTime.WPF.Utility;

namespace TakeMyTime.WPF.Subtasks
{
    /// <summary>
    /// Interaktionslogik für AddSubtask.xaml
    /// </summary>
    public partial class AddSubtask : Window
    {
        public AddSubtask(AddAssignment parent)
        {
            InitializeComponent();
            this.cb_PrioritySelect.SelectedItem = this.cbi_Medium;
            this.SelectedPriority = EnumDefinition.SubtaskPriority.Medium;
            this.ParentWindow = parent;
        }

        public AddSubtask(AddAssignment parent, Subtask subtask)
        {
            InitializeComponent();
            this.EditMode = true;
            this.EditableSubtask = subtask;
            this.cb_PrioritySelect.SelectedItem = GetItemByPriority(subtask.Priority);
            this.tb_SubtaskDescription.Text = subtask.Description;
            this.tb_SubtaskName.Text = subtask.Name;
            this.ParentWindow = parent;
        }

        #region GUI Events
        private void b_Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btn_AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            var param = new SubtaskCreateViewModel
            {
                Description = this.tb_SubtaskDescription.Text,
                Name = this.tb_SubtaskName.Text,
                Priority = this.SelectedPriority
            };

            if (!EditMode)
            {
                var subtask = Subtask.Create(param);

                this.ParentWindow.Subtasks.Add(subtask);
                this.Close();
            }
            else
            {
                if (this.EditableSubtask.Id > 0)
                {
                    var subtaskLogic = new SubtaskLogic();
                    subtaskLogic.Update(EditableSubtask.Id, param);
                    subtaskLogic.Dispose();
                } else
                {
                    this.EditableSubtask.Name = param.Name;
                    this.EditableSubtask.Description = param.Description;
                    this.EditableSubtask.Priority = param.Priority;
                }
                this.Close();
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cb_PrioritySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this.SelectedPriority = GetPriorityByItemName((e.AddedItems[0] as ComboBoxItem).Name);
            }
        }

        private void tb_SubtaskName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.btn_AddSubtask.IsEnabled = !string.IsNullOrWhiteSpace(this.tb_SubtaskName.Text);
        }

        #endregion

        #region Utility

        private EnumDefinition.SubtaskPriority GetPriorityByItemName(string itemName)
        {
            return itemName switch
            {
                "cbi_Lowest" => EnumDefinition.SubtaskPriority.Lowest,
                "cbi_Low" => EnumDefinition.SubtaskPriority.Low,
                "cbi_Medium" => EnumDefinition.SubtaskPriority.Medium,
                "cbi_High" => EnumDefinition.SubtaskPriority.High,
                "cbi_Highest" => EnumDefinition.SubtaskPriority.Highest,
                _ => EnumDefinition.SubtaskPriority.Lowest
            };
        }

        private ComboBoxItem GetItemByPriority(EnumDefinition.SubtaskPriority priority)
        {
            return priority switch
            {
                EnumDefinition.SubtaskPriority.Lowest => this.cbi_Lowest,
                EnumDefinition.SubtaskPriority.Low => this.cbi_Low,
                EnumDefinition.SubtaskPriority.Medium => this.cbi_Medium,
                EnumDefinition.SubtaskPriority.High => this.cbi_High,
                EnumDefinition.SubtaskPriority.Highest => this.cbi_Highest,
                _ => this.cbi_Medium
            };
        }

        #endregion

        public AddAssignment ParentWindow { get; set; }
        public EnumDefinition.SubtaskPriority SelectedPriority { get; set; }
        public Subtask EditableSubtask { get; set; }
        public bool EditMode { get; set; } = false;


    }
}
