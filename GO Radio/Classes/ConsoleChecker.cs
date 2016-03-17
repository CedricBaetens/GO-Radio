using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GO_Radio
{
    enum Commandos
    {
        LOAD,
        TTS,
        UNKNOWN
    }

    class ConsoleChecker
    {
        // Used for event
        public delegate void StatusUpdateHandler(object sender, ProgressEventArgs e);
        public event StatusUpdateHandler OnCommandDetected;

        // Variables
        private Timer checkTimer;

        // Constructor
        public ConsoleChecker()
        {
            checkTimer = new Timer();
            checkTimer.Elapsed += CheckTimer_Elapsed;
            checkTimer.Interval = 1000;
            checkTimer.Start();
        }

        private void CheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var text = GetLastLine();
            if (!string.IsNullOrEmpty(text))
            {
                checkTimer.Stop();
                ConsoleCommand command = new ConsoleCommand(text);
                if (IsValidCommand(command))
                {
                    CommandDetected(command);
                }
                checkTimer.Start();
            }
        }

        private string GetLastLine()
        {
            if (File.Exists(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt"))
            {
                var lastLine = File.ReadLines(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt").Last();
                File.Delete(ProgramSettings.PathCsgo + "\\csgo\\condump000.txt");
                return lastLine;
            }
            return "";
        }
        private bool IsValidCommand(ConsoleCommand input)
        {
            return input.Command != Commandos.UNKNOWN ? true : false;
        }

        // Used for firing event    
        private void CommandDetected(ConsoleCommand command)
        {
            // Make sure someone is listening to event
            if (OnCommandDetected == null) return;

            ProgressEventArgs args = new ProgressEventArgs(command);
            OnCommandDetected(this, args);
        }
        public class ProgressEventArgs : EventArgs
        {
            public ConsoleCommand Detected { get; set; }
            public ProgressEventArgs(ConsoleCommand command)
            {
                Detected = command;
            }
        }
    }

    class ConsoleCommand
    {
        public string Response { get; private set; }
        public Commandos Command { get; set; }

        public ConsoleCommand(string input)
        {
            var split = input.Split(new[] { ' ' }, 3);
            try
            {
                Command = (Commandos)Enum.Parse(typeof(Commandos), split[1].ToUpper());
            }
            catch (Exception e)
            {
                Command = Commandos.UNKNOWN;
            }
            Response = split[2];
        }
    }
}
