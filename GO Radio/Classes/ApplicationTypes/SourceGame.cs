using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class SourceGame : ApplicationType
    {
        //ConsoleChecker consoleChecker;

        //public SourceGame(CategoryList data) : base (data)
        //{
        //    // Instanciate
        //    SoundLoader = new SoundLoader(ProgramSettings.Instance.PathCsgo);
        //    consoleChecker = new ConsoleChecker();

        //    // Events
        //    consoleChecker.OnCommandDetected += ConsoleChecker_OnCommandDetected;
        //}

        //private void ConsoleChecker_OnCommandDetected(object sender, ConsoleChecker.ProgressEventArgs e)
        //{
        //    switch (e.Detected.Command)
        //    {
        //        case Commandos.LOAD:
        //            SoundLoader.LoadSong(Data.GetSoundById(Convert.ToInt32(e.Detected.Response)));
        //            break;
        //        case Commandos.TTS:
        //            SoundLoader.LoadSong(Tts.GetSound(e.Detected.Response));
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //public override void Start()
        //{
        //    base.Start();

        //    MessageBox.Show("Dont forget to type 'exec radio' in the console!");
        //    Cfg.Create.Init(Keyboard.KeyBindings);
        //    Cfg.Create.CategoryList(Data);

        //}
    }
}
