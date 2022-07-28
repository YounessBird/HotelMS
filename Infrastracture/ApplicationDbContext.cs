
using HotelMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>, AppUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.UseSerialColumns();

            modelbuilder.Entity<AppUserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId);
        });
            modelbuilder.Entity<AppUser>(b =>
        {
            // // Indexes for "normalized" username and email, to allow efficient lookups
            b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique(false);
            b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex").IsUnique();
            b.Property(u => u.Id).ValueGeneratedOnAdd();
        });
            modelbuilder.Entity<AppRole>(b => b.Property(u => u.Id).ValueGeneratedOnAdd());
        }
        public DbSet<Category> CategoryTb { get; set; }
        public DbSet<Room> RoomTb { get; set; }
        public DbSet<Booking> BookingTb { get; set; }
    }
}