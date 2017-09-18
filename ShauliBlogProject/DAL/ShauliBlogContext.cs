using ShauliBlogProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ShauliBlogProject.DAL
{
    public class ShauliBlogContext : DbContext
    {
        public ShauliBlogContext() : base("ShauliBlogContext") { }
        public DbSet<FansClub> FansClub { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Maps> Maps { get; set; }
  
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<ShauliBlogProject.Models.Fan> Fans { get; set; }
    }
}