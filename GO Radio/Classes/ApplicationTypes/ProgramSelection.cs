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

        protected CategoryList data;

        public ProgramSelection()
        {
            Setting = new ProgramSelectionSetting();
        }

        public virtual void Start(){}
        public virtual void Stop(){}

        public void Load(CategoryList data)
        {
            this.data = data;
        }
    }
}
