using BinderDyn.LoggingUtility;
using System;
using System.Windows;

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
    }
}
