using GoRadio.Logic.Database;
using GoRadio.Logic.Database.Entities;
using System.Collections.Generic;
using System.IO;
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

        public void Add(string name, byte[] data)
        {
            _databaseContext.Sounds.Add(new Logic.Database.Entities.Sound() { Name = name, Data = data });
            _databaseContext.SaveChanges();
        }
    }
}
