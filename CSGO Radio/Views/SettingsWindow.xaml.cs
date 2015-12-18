using HLDJ_Advanced.Classes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HLDJ_Advanced.Views
{
    [ImplementPropertyChanged]
    public partial class SettingsWindow : Window
    {
        public string PathSounds { get; set; }
        public string PathCsgo { get; set; }


        public SettingsWindow()
        {
            InitializeComponent();

            PathSounds = ProgramSettings.PathSounds;
            PathCsgo = ProgramSettings.PathCsgo;

            DataContext = this;
        }

        private void BrowseSoundPath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog()
            {
                Description = "Please select the sound folder."
            };

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathSounds = fbd.SelectedPath;
            }
        }
        private void BrowseCsgoPath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog()
            {
                Description = "Please select the csgo folder."
            };
            
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathCsgo = fbd.SelectedPath;
            }           
        }

        public ICommand CommandBrowseSoundPath { get { return new RelayCommand(BrowseSoundPath); } }
        public ICommand CommandBrowseCsgoPath { get { return new RelayCommand(BrowseCsgoPath); } }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProgramSettings.PathCsgo = PathCsgo;
            ProgramSettings.PathSounds = PathSounds;
        }
    }
}
