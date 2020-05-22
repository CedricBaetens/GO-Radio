using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GO_Radio.Views;
using Newtonsoft.Json;
using PropertyChanged;
using GO_Radio.Classes;
using System.Windows.Media;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Media;
using System.Reflection;
using System.Threading;
using NAudio.Wave;
using System.Deployment.Application;
using GO_Radio.Classes.Settings;

namespace GO_Radio
{
    [ImplementPropertyChanged]
    public partial class MainWindow : Window
    {
        public MainViewModel MainController { get; set; }

        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            //Initialize
            MainController = mainViewModel;

            // Binding
            DataContext = MainController;
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MainController.Load();
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            MainController.Save();
        }


        // Fix scrollwheel on datagrid
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
        }
    }
}
