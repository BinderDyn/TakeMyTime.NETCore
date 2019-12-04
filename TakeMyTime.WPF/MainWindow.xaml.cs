using BinderDyn.LoggingUtility;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace TakeMyTime.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitLogger();
            InitializeComponent();
            txt_Title.Text = "TakeMyTime " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public void InitLogger()
        {
            if (!Logger.FolderStructureCreated)
            {
                Logger.InitializeLogger();
                Logger.PrepareLogging();
            }
            if (!Logger.FolderStructureCreated) throw new Exception("Logger initializiation failed.");
            Logger.Log(string.Format("{0}.InitLogger()", this.GetType().FullName));
        }

        private void btn_AppMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_AppClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void sp_Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btn_ToggleWindow_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }
    }
}
