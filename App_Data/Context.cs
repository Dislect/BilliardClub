using BilliardClub.Models;
using Microsoft.EntityFrameworkCore;

namespace BilliardClub.App_Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<PoolTable> PoolTables { get; set; }
        public DbSet<RentalHistory> RentalHistories { get; set; }
        public DbSet<RestaurantMenu> RestaurantMenus { get; set; }
        public DbSet<RestaurantOrderHistory> RestaurantOrderHistories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StatusTable> StatusTables { get; set; }
        public DbSet<TableRotation> TableRotations { get; set; }
        public DbSet<TypeTable> TypeTables { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}