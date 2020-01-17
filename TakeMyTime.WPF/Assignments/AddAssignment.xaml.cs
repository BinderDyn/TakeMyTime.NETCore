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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        }

        private void btn_EditSubtask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_AddSubtask_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
