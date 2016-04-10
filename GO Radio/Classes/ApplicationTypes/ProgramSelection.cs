using GO_Radio.Classes.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes.ApplicationTypes
{
    public class ProgramSelection
    {
        public string Name { get; set; }
        public ProgramSelectionSetting Setting { get; set; }
        public bool IsSelectable { get; set; } = true;

        public KeyboardController Keyboard { get; set; }

        protected CategoryList data;     

        public ProgramSelection()
        {
            Setting = new ProgramSelectionSetting();
            Keyboard = new KeyboardController();
        }

        public void Load(ProgramSelectionSetting settings)
        {
            Setting = settings;
        }

        public virtual void Start(CategoryList data)
        {
            this.data = data;
            Keyboard.Hook();
        }
        public virtual void Stop()
        {
            Keyboard.UnHook();
        }
    }
}
