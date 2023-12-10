﻿using System.Collections.Generic;
using System.Net;
using System;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections;
using System.Numerics;

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
            //for a new computer, please get connection string data from 1047860503 qq mail box, searching "AppDbContext"


        }

        public AppDbContext() : base()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }

}