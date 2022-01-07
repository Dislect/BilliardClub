﻿using BilliardClub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BilliardClub.App_Data
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PoolTable> PoolTables { get; set; }
        public DbSet<RestaurantMenu> RestaurantMenus { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StatusTable> StatusTables { get; set; }
        public DbSet<TableRotation> TableRotations { get; set; }
        public DbSet<TypeTable> TypeTables { get; set; }
    }
}