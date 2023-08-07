using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data {
    public class DataContextEF : DbContext {


        public virtual DbSet<User>? Users {get; set;}
        public DbSet<UserJobInfo>? UserJobInfo {get; set;}
        public DbSet<UserSalary>? UserSalary {get; set;}
        private IConfiguration _config;

        public DataContextEF (IConfiguration config) {
            _config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured) {
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection"), 
                options => options.EnableRetryOnFailure());
            }
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");
            modelBuilder.Entity<User>().ToTable("Users","TutorialAppSchema").HasKey(u => u.UserId);
            modelBuilder.Entity<UserJobInfo>().HasKey(ui => ui.UserId);
            modelBuilder.Entity<UserSalary>().HasKey(us => us.UserId);

         }
    }
}