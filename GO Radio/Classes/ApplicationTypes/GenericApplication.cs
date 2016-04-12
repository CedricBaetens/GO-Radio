using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace GO_Radio.Classes.ApplicationTypes
{
    [ImplementPropertyChanged]
    class GenericApplication : ProgramSelection
    {
        public SoundLoaderDevice SoundLoader { get; set; }

        public GenericApplication()
        {
            SoundLoader = new SoundLoaderDevice();
        }

        public override void Start(CategoryList data)
        {
            base.Start(data);

            SoundLoader.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
