using BinderDyn.LoggingUtility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using TakeMyTime.DOM.Models;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL
{
    public class TakeMyTimeDbContext : DbContext
    {

#if DEBUG
        public TakeMyTimeDbContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TakeMyTimeDebug"];
            optionsBuilder.UseSqlite("Data Source=TakeMyTimeDebug.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<Project>().HasMany(p => p.Assignments)
                .WithOne(a => a.Project)
                .HasForeignKey(fk => fk.ProjectId);
            modelBuilder.Entity<Project>().HasMany(p => p.Entries)
                .WithOne(a => a.Project)
                .HasForeignKey(fk => fk.ProjectId);

            modelBuilder.Entity<Assignment>().HasKey(a => a.Id);
            modelBuilder.Entity<Assignment>().HasMany(a => a.Entries)
                .WithOne(e => e.Assignment)
                .HasForeignKey(e => e.AssignmentId);

            modelBuilder.Entity<Entry>().HasKey(e => e.Id);

            modelBuilder.Entity<ProjectType>().HasKey(pt => pt.Id);
            modelBuilder.Entity<ProjectType>().HasMany(p => p.Projects)
                .WithOne(a => a.ProjectType)
                .HasForeignKey(fk => fk.ProjectType_Id);
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
        public DbSet<ProjectType> ProjectTypes { get; set; }

    }
}
