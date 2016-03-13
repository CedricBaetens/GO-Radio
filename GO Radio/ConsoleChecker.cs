using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GO_Radio
{
    class ConsoleChecker
    {
        private Timer checkTimer;

        public ConsoleChecker()
        {
            checkTimer = new Timer();
            checkTimer.Elapsed += CheckTimer_Elapsed;
            checkTimer.Interval = 1000;
            checkTimer.Start();
        }

        private void CheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var text = GetText();
            if (!string.IsNullOrEmpty(text))
            {
                checkTimer.Stop();
                var foundText = Find(text, "Loaded");
                int a = 0;
            }
        }

        private static string GetText()
        {
            if (File.Exists(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt"))
            {
                var lastLine = File.ReadLines(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt").Last();
                File.Delete(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt");
                return lastLine;
            }
            return "";
        }

        private static string Find(string input, string tofind)
        {
            if (input.Contains(tofind))
            {
                string id = input.Substring(7, 4);
                return id;
            }
            return "";
        }
    }
}
