using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using WebAPi_JWT.WebApi.Core.Domian;

namespace WebAPi_JWT.Infrastructure
{
    public class DataAccess:DbContext
    {
        public DataAccess(DbContextOptions dataAccess) :base(dataAccess)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AuthUser> AuthUsers { get; set; }
        public virtual DbSet<RefreshToken> Token { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("tbl_User");
            modelBuilder.Entity<AuthUser>().ToTable("tbl_Auth");
            modelBuilder.Entity<RefreshToken>().ToTable("tbl_Token");
        }

    }
}
