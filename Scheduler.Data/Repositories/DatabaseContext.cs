using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// import the Models (representing structure of tables in database)
using Scheduler.Core.Models; 

namespace Scheduler.Data.Repositories
{
    // The Context is How EntityFramework communicates with the database
    // We define DbSet properties for each table in the database
    public class DatabaseContext :DbContext
    {
         // authentication store
        public DbSet<User> Users { get; set; }

        // Schedule Events in Rooms
        public DbSet<Event> Events { get; set; }
        public DbSet<Room> Rooms { get; set; }

        // Configure the context to use Specified database. We are using 
        // Sqlite database as it does not require any additional installations.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder                  
                .UseSqlite("Filename=data.db")
                .LogTo(Console.WriteLine, LogLevel.Information) // remove in production
                .EnableSensitiveDataLogging()                   // remove in production
                ;
        }

        // Convenience method to recreate the database thus ensuring  the new database takes 
        // account of any changes to the Models or DatabaseContext
        public void Initialise()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

    }
}
