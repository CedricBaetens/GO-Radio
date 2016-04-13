using GO_Radio.Classes.Settings;

namespace GO_Radio.Classes.ApplicationTypes
{
    public class ProgramSelection
    {
        public ProgramSelector.ApplicationState State { get; set; }
        public string Name { get; set; }
        public ProgramSelectionSetting Setting { get; set; }
        public bool IsSelectable { get; set; } = true;

        public KeyboardController Keyboard { get; set; }
        protected CategoryList Data;
        protected TextToSpeech Tts;

        public ProgramSelection()
        {
            Setting = new ProgramSelectionSetting();
            Keyboard = new KeyboardController();
            Tts = new TextToSpeech();
            State = ProgramSelector.ApplicationState.STANDBY;
        }
        public void Load(ProgramSelectionSetting settings)
        {
            Setting = settings;
        }
        public virtual void Start(CategoryList data)
        {
            Data = data;
            Keyboard.Hook();
        }
        public virtual void Stop()
        {
            Keyboard.UnHook();
            State = ProgramSelector.ApplicationState.STANDBY;
        }
    }
}
