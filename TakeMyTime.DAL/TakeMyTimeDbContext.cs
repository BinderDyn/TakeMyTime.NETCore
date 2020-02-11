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

        public TakeMyTimeDbContext(DbContextOptions<TakeMyTimeDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TakeMyTimeDebug"];
            optionsBuilder.UseSqlite("Data Source=TakeMyTimeDebug.db;");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Project>().HasKey(p => p.Id);
            //modelBuilder.Entity<Project>().HasMany(p => p.Assignments)
            //    .WithOne(a => a.Project)
            //    .HasForeignKey(fk => fk.Project_Id);
            //modelBuilder.Entity<Project>().HasMany(p => p.Entries)
            //    .WithOne(a => a.Project)
            //    .HasForeignKey(fk => fk.Project_Id);

            //modelBuilder.Entity<Assignment>().HasKey(a => a.Id);
            //modelBuilder.Entity<Assignment>().HasMany(a => a.Entries)
            //    .WithOne(e => e.Assignment)
            //    .HasForeignKey(e => e.Assigment_Id);

            //modelBuilder.Entity<Entry>().HasKey(e => e.Id);

            //modelBuilder.Entity<ProjectType>().HasKey(pt => pt.Id);
            //modelBuilder.Entity<ProjectType>().HasMany(p => p.Projects)
            //    .WithOne(a => a.ProjectType)
            //    .HasForeignKey(fk => fk.ProjectType_Id);
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
        public DbSet<Subtask> Subtasks { get; set; }

    }
}
