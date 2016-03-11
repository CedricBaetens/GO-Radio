using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GO_Radio.Views
{
    /// <summary>
    /// Interaction logic for DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        private WebClient webClient;
        private string downloadPath;

        public DownloadWindow(string url, Version version)
        {
            InitializeComponent();

            webClient = new WebClient();
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;

            try
            {
                downloadPath = string.Format("{1}/setup_csgo_radio{0}.exe", version, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                webClient.DownloadFileAsync(new Uri(url), downloadPath);
            }
            catch (Exception)
            {
                MessageBox.Show("Download failed...");
                Close();
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            pbDowload.Value = e.ProgressPercentage;
            tbPercentage.Text = string.Format("{1} of {2} KB. {3} % complete...",
        (string)e.UserState,
        e.BytesReceived/1000,
        e.TotalBytesToReceive/1000,
        e.ProgressPercentage);
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Process.Start(downloadPath);
            Environment.Exit(1);
        }
    }
}
