using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Domain.Entities;
using THT.JMS.Utilities;
using static THT.JMS.Utilities.Enums;

namespace THT.JMS.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminRoleId = Guid.NewGuid();
            var userRoleId = Guid.NewGuid();
            var adminUserId = Guid.NewGuid();

            // Seed roles
            modelBuilder.Entity<Roles>().HasData(
                new Roles { Id = adminRoleId, RoleName = "Admin" },
                new Roles { Id = userRoleId, RoleName = "User" }
            );

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = adminUserId,
                    UserName = "admin",
                    FirstName="Tri",
                    LastName="Tran",
                    Code="AD-01",
                    IdentityCard="12345",
                    IdentityCardAddress="Binh Phuoc",
                    IdentityCardDate= new DateTime(2015, 12, 25),
                    Phone="0333444555",
                    RefreshToken=null,
                    FullName = "Tran Huu Tri",
                    Password = SecurityUtilities.HashSHA1("23312331"), //hashed password
                    Email = "admin@gmail.com",
                    Address = "Binh Phuoc",
                    IsActive = true,
                    Deleted = false,
                    IsAdmin = true,
                    Created = DateTime.UtcNow.AddHours(7),
                    CreatedBy = Guid.Empty,
                    Status = (int)UserStatus.Active,
                    Gender = (int)UserGender.Female
                }
            );
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
