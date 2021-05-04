using GoRadio.Logic.Database;
using GoRadio.Logic.Database.Entities;
using NAudio.Wave;
using System.Collections.Generic;
using System.Linq;

namespace GoRadio.Logic.Services
{
    public class SoundService
    {
        private readonly DatabaseContext _databaseContext;

        public SoundService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<Sound> GetAll()
        {
            var sounds = _databaseContext.Sounds.ToList();
            return sounds;
        }

        public void Play(int id)
        {
            //var sound = _databaseContext.Sounds.SingleOrDefault(x => x.Id == id);
            //if (sound == null)
            //    return;

            var url = "http://media.ch9.ms/ch9/2876/fd36ef30-cfd2-4558-8412-3cf7a0852876/AzureWebJobs103.mp3";
            var mf = new MediaFoundationReader(url);
            var wo = new WaveOutEvent();
            wo.Init(mf);
            wo.Play();
        }
    }
}
