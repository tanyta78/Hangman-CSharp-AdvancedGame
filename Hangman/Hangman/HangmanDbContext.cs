using System.Data.Entity;
using Database;

namespace Hangman
{
    public class HangmanContext : DbContext
    {
        public HangmanContext()
            : base("HangmanDb")
        {
        }
        public DbSet<Users> Users { get; set; }

        public DbSet<Words> Words { get; set; }
    }
}
