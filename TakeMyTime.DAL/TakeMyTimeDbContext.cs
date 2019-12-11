using BinderDyn.LoggingUtility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.DAL
{
    public class TakeMyTimeDbContext : DbContext
    {

#if DEBUG
        public TakeMyTimeDbContext() : base()
        {
            try
            {
                Database.Migrate();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["TakeMyTimeDebug"].ConnectionString);
        }
#endif

#if !DEBUG
        public TakeMyTimeDbContext() : base()
        {
            try
            {
                Database.Migrate();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["TakeMyTime"].ConnectionString);
        }
#endif

        public DbSet<Project> Projects { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

    }
}
