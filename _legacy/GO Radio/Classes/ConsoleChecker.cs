﻿using System;
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
        private string path;

        // Constructor
        public ConsoleChecker()
        {
            checkTimer = new Timer(250);
            checkTimer.Elapsed += CheckTimer_Elapsed;
        }

        public void Start(string path)
        {
            this.path = path;
            checkTimer.Start();
            ClearAllDumpFiles();
        }
        public void Stop()
        {
            checkTimer.Stop();
        }

        private void CheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            checkTimer.Stop();

            var text = GetLastLine();
            if (!string.IsNullOrEmpty(text))
            {             
                ConsoleCommand command = new ConsoleCommand(text);
                if (IsValidCommand(command))
                {
                    CommandDetected(command);
                }
            }
            checkTimer.Start();
        }

        private void ClearAllDumpFiles()
        {
            var dumpfiles = Directory.GetFiles(path + "\\csgo", "*condump*");
            foreach (var file in dumpfiles)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
        private string GetLastLine()
        {
            if (File.Exists(path + "\\csgo\\condump000.txt"))
            {
                var lastLine = File.ReadLines(path + "\\csgo\\condump000.txt").Last();
                File.Delete(path + "\\csgo\\condump000.txt");
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
            var split = input.Replace("] ", "").Split(new[] { ' ' }, 2);
            try
            {
                Command = (Commandos)Enum.Parse(typeof(Commandos), split[0].ToUpper());
            }
            catch (Exception)
            {
                Command = Commandos.UNKNOWN;
            }
            Response = split[1];
        }
    }
}
