using BinderDyn.LoggingUtility;
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

namespace TakeMyTime.WPF.Projects
{
    /// <summary>
    /// Interaktionslogik für AddProject.xaml
    /// </summary>
    public partial class AddProject : Window
    {
        string className;

        public AddProject()
        {
            this.className = this.GetType().FullName;
            LoadProjectTypes();
            InitializeComponent();
            InitLogger();
        }

        private void InitLogger()
        {
            Logger.Log(string.Format("{0}.InitLogger()", className));
        }

        private void LoadProjectTypes()
        {
            try
            {
                Logger.Log(string.Format("{0}.LoadProjectTypes()", className));
                var bll = new ProjectTypeLogic();
                this.ProjectTypes = bll.GetProjectTypes().Select(pt => new ProjectTypeViewModel(pt)).ToList();
                cb_ProjectTypes.ItemsSource = this.ProjectTypes;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        private void sp_Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void btn_AddProject_Click(object sender, RoutedEventArgs e)
        {

        }

        public List<ProjectTypeViewModel> ProjectTypes { get; set; }
        public ProjectTypeViewModel SelectedProjectType { get; set; }
    }
}
