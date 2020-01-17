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

namespace TakeMyTime.WPF.Subtasks
{
    /// <summary>
    /// Interaktionslogik für AddSubtask.xaml
    /// </summary>
    public partial class AddSubtask : Window
    {
        public AddSubtask()
        {
            InitializeComponent();
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

        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void cb_PrioritySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion

        #region Utility

        private EnumDefinition.SubtaskPriority GetStatusByItemName(string itemName)
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

        #endregion

        
    }
}
