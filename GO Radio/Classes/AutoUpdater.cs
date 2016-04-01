using GO_Radio.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace GO_Radio.Classes
{
    class AutoUpdater
    {
        private Version currentVersion;
        private Version latestVersion;
        private string url;
        private string latesestVersionUrl;

        public AutoUpdater(string url, Version currentVersion)
        {
            this.url = url;
            this.currentVersion = currentVersion;
        }

        public void CheckForUpdate()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                webClient.Headers.Add("Cache-Control", "no-cache");

                Random r = new Random();
                var random = r.Next();

                var textFromFile = webClient.DownloadString(url + "?rand=" + random);

                // Xml
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(textFromFile);
                XmlNode versionNode = doc.DocumentElement.SelectSingleNode("version");
                XmlNode urlNode = doc.DocumentElement.SelectSingleNode("url");

                latestVersion = new Version(versionNode.InnerText);
                latesestVersionUrl = urlNode.InnerText;

                if (latestVersion > currentVersion)
                {
                    if (MessageBox.Show("New version availible, would you like to download it?", "Update!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        DownloadWindow dw = new DownloadWindow(latesestVersionUrl, latestVersion);
                        dw.ShowDialog();
                    }
                }
            }
            catch (Exception)
            {
                //
            }          
        }
    }
}
