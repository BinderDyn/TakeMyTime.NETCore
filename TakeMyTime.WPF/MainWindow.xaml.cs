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
using TakeMyTime.WPF.Utility.Commands;

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
                MessageBox.Show(e.Message);
#if DEBUG
                throw;
#endif
            }

            InitDataDirectory();
            InitializeComponent();
            DataContext = this;
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
            using (TakeMyTimeDbContext context = new TakeMyTimeDbContext())
            {
                context.Database.Migrate();
            }
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
            NavigateToProjectOverview();
        }

        private void btn_Assignments_Click(object sender, RoutedEventArgs e)
        {
            NavigateToAssignmentOverview();
        }

        private void btn_LogEntries_Click(object sender, RoutedEventArgs e)
        {
            NavigateToEntryOverview();
        }

        private void btn_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            NavigateToDashboardOverview();
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            NavigateToProjectTypes();
        }

        private void btn_About_Click(object sender, RoutedEventArgs e)
        {
            NavigateToAbout();
        }

        public void NavigateToProjectOverview()
        {
            fr_Content.Navigate(new Projects.ProjectOverview());
        }

        public void NavigateToAssignmentOverview()
        {
            fr_Content.Navigate(new Assignments.AssignmentOverview());
        }

        public void NavigateToEntryOverview()
        {
            fr_Content.Navigate(new Entries.EntryOverview());
        }

        public void NavigateToDashboardOverview()
        {
            fr_Content.Navigate(new Dashboard());
        }

        public void NavigateToProjectTypes()
        {
            fr_Content.Navigate(new ProjectTypeOverview());
        }

        public void NavigateToAbout()
        {
            fr_Content.Navigate(new About.About());
        }



        #endregion

        #endregion

        public string CurrentCalendarWeek { get => string.Format("{0}: {1}", ResourceStringManager.GetResourceByKey("CalendarWeek"), DateTimeCultureConverter.GetCalendarWeek()); }

        #region Commands

        public NavigationCommand DashboardCommand { get => new NavigationCommand(() => this.NavigateToDashboardOverview()); }
        public NavigationCommand ProjectOverviewCommand { get => new NavigationCommand(() => this.NavigateToProjectOverview()); }
        public NavigationCommand ProjectTypeCommand { get => new NavigationCommand(() => this.NavigateToProjectTypes()); }
        public NavigationCommand AssignmentOverviewCommand { get => new NavigationCommand(() => this.NavigateToAssignmentOverview()); }
        public NavigationCommand EntryCommand { get => new NavigationCommand(() => this.NavigateToEntryOverview()); }
        public NavigationCommand AboutCommand { get => new NavigationCommand(() => this.NavigateToAbout()); }
        public NavigationCommand ExitCommand { get => new NavigationCommand(() => Application.Current.Shutdown()); }

        #endregion
    }
}
