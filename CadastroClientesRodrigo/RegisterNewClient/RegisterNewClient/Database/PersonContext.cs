using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RegisterNewClient.Database
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public static string DatabasePath { get;set; }

        public PersonContext()
        {
        }

        public PersonContext(String databasePath)
        {
            DatabasePath = databasePath;
            Database.EnsureCreated();
        }

        public PersonContext(DbContextOptions<PersonContext> options)
            : base(options)
        {
            Database.Migrate(); //Same as Database.EnsureCreated, but applies pending migrations on DB or creates a new DB with migration support, while the previous don't
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            if (string.IsNullOrWhiteSpace(DatabasePath))
                throw new InvalidOperationException("The database path should be initialized prior the OnConfiguring() method call");

            optionsBuilder.UseSqlite($"Filename={DatabasePath}");

        }

        //https://stackoverflow.com/questions/45775267/how-to-validate-models-before-savechanges-in-entityframework-core-2
        // Alternative version: https://github.com/aspnet/EntityFrameworkCore/issues/3680#issuecomment-155502539
        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker
                .Entries()
                .Where(_ => _.State == EntityState.Added || _.State == EntityState.Modified);

            return base.SaveChanges();
            // FIXME: Don't forget to override SaveChangesAsync
        }

        public void UpdateEntity<T>(T sourceFrom, T targetTo) where T : class
        {
            // https://blog.oneunicorn.com/2012/05/03/the-key-to-addorupdate/
            Entry(targetTo).CurrentValues.SetValues(sourceFrom);
        }

        public double GetDatabaseFileSize()
        {
            if (File.Exists(DatabasePath))
                return new FileInfo(DatabasePath).Length;
            return double.NaN;
        }

    }
}
