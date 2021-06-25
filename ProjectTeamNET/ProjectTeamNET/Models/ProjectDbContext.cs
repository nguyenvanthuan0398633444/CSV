using m_theme;
using m_user;
using m_user_screen_item;
using m_work_contents;
using m_work_contents_class;
using Microsoft.EntityFrameworkCore;
using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using t_manhour;

namespace ProjectTeamNET.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserScreenItem> UserScreenItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<WorkContents> WorkContents { get; set; }
        public DbSet<WorkContentsClass> WorkContentsClasses { get; set; }
        public DbSet<Manhour> Manhours { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WorkContents>()
                   .HasKey(b => b.Work_contents_code);
            modelBuilder.Entity<Manhour>()
                   .HasKey(b => b.Year);
        }
    }
}
