using GoRadio.Logic.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoRadio.Logic.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Sound> Sounds { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)=> options.UseSqlite(@"Data Source=database.db");
    }
}
