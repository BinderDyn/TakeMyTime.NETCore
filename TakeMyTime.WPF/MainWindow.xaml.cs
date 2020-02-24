using BinderDyn.LoggingUtility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TakeMyTime.DAL;
using TakeMyTime.DAL.uow;
using TakeMyTime.WPF.ProjectTypes;
using TakeMyTime.WPF.Statistics;
using TakeMyTime.WPF.Utility;

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

            try
            {
                ApplyMigrations();
#if DEBUG
                Seeder.Seed();
#endif
            }
            catch (Exception e)
            {
                Logger.LogException(e);
#if DEBUG
                throw e;
#endif
            }

            InitDataDirectory();
            InitializeComponent();
            txt_Title.Text = "TakeMyTime " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5);
            tb_CalendarWeek.Text = this.CurrentCalendarWeek;
            fr_Content.Navigate(new Dashboard());
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

        private void InitDataDirectory()
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }

        private void ApplyMigrations()
        {
            TakeMyTimeDbContext context = new TakeMyTimeDbContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        #region GUI Events

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

        #region Menu Navigation

        private void btn_Projects_Click(object sender, RoutedEventArgs e)
        {
            fr_Content.Navigate(new Projects.ProjectOverview());
        }

        private void btn_Assignments_Click(object sender, RoutedEventArgs e)
        {
            fr_Content.Navigate(new Assignments.AssignmentOverview());
        }

        private void btn_LogEntries_Click(object sender, RoutedEventArgs e)
        {
            fr_Content.Navigate(new Entries.EntryOverview());
        }

        private void btn_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            fr_Content.Navigate(new Dashboard());
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            fr_Content.Navigate(new ProjectTypeOverview());
        }

        #endregion

        #endregion

        public string CurrentCalendarWeek { get => string.Format("{0}: {1}", ResourceStringManager.GetResourceByKey("CalendarWeek"), DateTimeCultureConverter.GetCalendarWeek()); }

        
    }
}
