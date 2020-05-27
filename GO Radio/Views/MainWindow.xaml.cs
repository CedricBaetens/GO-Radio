using GO_Radio.Classes;
using PropertyChanged;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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
