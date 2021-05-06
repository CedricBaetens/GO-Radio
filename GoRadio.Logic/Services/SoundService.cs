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
    }
}
