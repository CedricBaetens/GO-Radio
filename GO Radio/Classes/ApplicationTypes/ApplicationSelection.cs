using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio.Classes
{
    [ImplementPropertyChanged]
    public class ApplicationSelection
    {
        public enum ApplicationState
        {
            STANDBY,
            RUNNING,
            UNDEFINED
        };

        // Diffrent Application Selections
        public ObservableCollection<ApplicationType> Configurations { get; set; }
        public ApplicationType ActiveConfiguration { get; set; }
        public ApplicationState State { get; set; }

        // Data
        public CategoryList Data { get; set; }

        // Constructor
        public ApplicationSelection()
        {
            // Instanciete 
            Data = new CategoryList();

            Configurations = new ObservableCollection<ApplicationType>
            {
                new SourceGame(Data)
                {
                    Name ="Counter Strike: Global Offensive"
                },
                new Other()
                {
                    Name ="Other (Skype, ...) (Still in development)",
                    IsSelectable = false
                }
            };

            ActiveConfiguration = Configurations[0];
        }

        

        // Interface Methods
        public void Start()
        {
            State = ApplicationState.RUNNING;
            ActiveConfiguration.Start();
        }
        public void Stop()
        {
            State = ApplicationState.STANDBY;
            ActiveConfiguration.Stop();
        }

        public void Exit()
        {
            //Keyboard.UnHook();
        }

        public bool IsIdle()
        {
            return State == ApplicationState.STANDBY ? true : false;
        }
    }
}
