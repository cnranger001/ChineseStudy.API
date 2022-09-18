using System.Collections.Generic;
using System.Net;
using System;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<Idiom> Idioms { get; set; }
        public DbSet<Poem> Poems { get; set; }
        public DbSet<Article> Articles { get; set; }

        public DbSet<StudyHistory> StudyHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server = .; Database = ChineseStudy; Integrated Security=True");
           optionsBuilder.UseSqlServer("Server = 124.70.79.233; Database = ChineseStudy; user=admin; password=123");

        }

        public AppDbContext() : base()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }

}