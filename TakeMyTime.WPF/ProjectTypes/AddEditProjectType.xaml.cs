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

namespace TakeMyTime.WPF.ProjectTypes
{
    /// <summary>
    /// Interaktionslogik für AddEditProjectType.xaml
    /// </summary>
    public partial class AddEditProjectType : Window
    {
        public AddEditProjectType()
        {
            InitializeComponent();
        }

        public AddEditProjectType(int projectType_id)
        {
            InitializeComponent();
            if(projectType_id > 0)
            {
                var projectTypeLogic = new ProjectTypeLogic();
                this.ProjectTypeViewModel = new ProjectTypeViewModel(projectTypeLogic.GetProjectType(projectType_id));
                this.txt_Name.Text = ProjectTypeViewModel.Name;
                this.txt_Description.Text = ProjectTypeViewModel.Description;
                projectTypeLogic.Dispose();
            }
        }

        #region GUI Events

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            var projectTypeLogic = new ProjectTypeLogic();
            if (this.ProjectTypeViewModel != null)
            {
                
                var updateParam = new ProjectTypeViewModel
                {
                    Name = this.txt_Name.Text,
                    Description = this.txt_Description.Text
                };
                projectTypeLogic.UpdateProjectType(this.ProjectTypeViewModel.Id, updateParam);
                projectTypeLogic.Dispose();
                this.Close();
            }
            else
            {
                var createParam = new ProjectTypeViewModel
                {
                    Name = this.txt_Name.Text,
                    Description = this.txt_Description.Text
                };
                projectTypeLogic.AddProjectType(createParam);
                projectTypeLogic.Dispose();
                this.Close();
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void b_Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void txt_Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            this.btn_Save.IsEnabled = !string.IsNullOrWhiteSpace(this.txt_Name.Text);
        }

        #endregion

        public ProjectTypeViewModel ProjectTypeViewModel { get; set; }

        
    }
}
