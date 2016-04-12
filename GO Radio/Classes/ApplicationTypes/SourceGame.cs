using PropertyChanged;
using System;
using System.IO;
using System.Windows.Forms;

namespace GO_Radio.Classes.ApplicationTypes
{
    [ImplementPropertyChanged]
    class SourceGame : ProgramSelection
    {
        public SoundLoader SoundLoader { get; set; }

        private const string processName = "Counter-Strike: Global Offensive";
        private readonly Cfg _cfg;
        private readonly ConsoleChecker _consoleChecker;

        public SourceGame()
        {
            // Instance
            SoundLoader = new SoundLoader();
            _cfg = new Cfg();
            _consoleChecker = new ConsoleChecker();

            // Events
            Keyboard.ButtonPressed += Keyboard_ButtonPressed;
            Keyboard.IdEntered += Keyboard_IdEntered;
            _consoleChecker.OnCommandDetected += ConsoleChecker_OnCommandDetected;
        }

        private void Keyboard_IdEntered(object sender, KeyboardController.IdEventArgs e)
        {
            SoundLoader.LoadSong(Data.GetSoundById(e.Input));
        }
        private void Keyboard_ButtonPressed(object sender, KeyboardController.ButtonEventArgs e)
        {
            if (ActiveProcess.IsSame(processName))
            {
                switch (e.Key)
                {
                    case KeyboardController.PressedKey.PlayPauze:
                        SoundLoader.PlayPause();
                        break;
                    case KeyboardController.PressedKey.PlayStop:
                        SoundLoader.PlayStop();
                        break;
                }
            }
        }
        private void ConsoleChecker_OnCommandDetected(object sender, ConsoleChecker.ProgressEventArgs e)
        {
            switch (e.Detected.Command)
            {
                case Commandos.LOAD:
                    break;
                case Commandos.TTS:
                    SoundLoader.LoadSong(Tts.GetSound(e.Detected.Response));
                    break;
                case Commandos.UNKNOWN:
                    break;
            }
        }

        public override void Start(CategoryList data)
        {
            base.Start(data);

            // Check Gamepath
            if (!Directory.Exists(Setting.GamePath))
            {
                // Search for CSGO
                if (Directory.Exists(@"C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive"))
                    Setting.GamePath = @"C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive";
                else
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog()
                    {
                        Description = @"Please select the csgo folder."
                    };
                    fbd.ShowDialog();


                    if (!string.IsNullOrEmpty(fbd.SelectedPath))
                    {
                        Setting.GamePath = fbd.SelectedPath;
                    }
                    else
                    {
                        Environment.Exit(1);
                    }
                }
            }

            SoundLoader.Start(Setting.GamePath);
            Tts.Start(data.Path);
            _cfg.Start(Setting.GamePath, Keyboard.KeyBindings);
            _consoleChecker.Start(Setting.GamePath);
        }
        public override void Stop()
        {
            base.Stop();

            SoundLoader.Stop();
            _cfg.Stop();
            _consoleChecker.Stop();
        }
    }
}
