using Domain.IAM.Models.Aggregates;
using Domain.IAM.Models.ValueObjects.Auth;
using Domain.IAM.Models.ValueObjects.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class MaceTechDataCenterContext : DbContext
{
    public MaceTechDataCenterContext() { }

    public MaceTechDataCenterContext(
        DbContextOptions<MaceTechDataCenterContext> options
    ) : base(options) { }
    
    public DbSet<User> Users { get; init; }
    public DbSet<RefreshTokenRecord> RefreshTokenRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //  |: User
        var user = modelBuilder.Entity<User>();
        user.HasKey(u => u.UserId);
        user.Property(u => u.UserId)
            .HasConversion(
                id => id.Value, 
                value => new UserId(value))   
            .ValueGeneratedOnAdd();           

        user.OwnsOne(u => u.FullName, fn =>
        {
            fn.Property(x => x.Name)     .HasColumnName("FirstName");
            fn.Property(x => x.LastNames).HasColumnName("LastName");
        });
        user.Property(u => u.Email)
            .HasColumnName("Email")
            .HasConversion(
                e => e.Value,
                v => new Email(v));

        user.Property(u => u.HashPassword)
            .HasConversion(
                p => p.Value,
                v => new HashPassword(v))
            .HasColumnName("PasswordHash")
            .HasMaxLength(100);
        modelBuilder.Entity<User>().ToTable("User");
        
        //  |: Token
        modelBuilder.Entity<RefreshTokenRecord>().ToTable("RefreshTokenRecord");
    }
}