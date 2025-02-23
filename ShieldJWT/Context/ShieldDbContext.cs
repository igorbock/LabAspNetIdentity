﻿namespace ShieldJWT.Context;

public class ShieldDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ChangedPassword> ChangedPasswords { get; set; }

    public ShieldDbContext(DbContextOptions<ShieldDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Email = "noreply@gmail.com", Username = "admin", Hash = "" });
    }
}
