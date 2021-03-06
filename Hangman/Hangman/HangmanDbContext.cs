﻿using System.Data.Entity;
using Database;

namespace Hangman
{
    public class HangmanContext : DbContext
    {
        public HangmanContext()
            : base("HangmanDb")
        {
        }
        public virtual DbSet<Users> Users { get; set; }

        public virtual DbSet<Words> Words { get; set; }
    }
}
