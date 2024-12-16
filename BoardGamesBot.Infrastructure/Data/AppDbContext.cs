using BoardGamesBot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesBot.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Room> Rooms { get; init; }
    public DbSet<RoomMember> RoomMembers { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>()
            .HasMany(r => r.Members)
            .WithOne(m => m.Room)
            .HasForeignKey(m => m.RoomId);
        
        modelBuilder.Entity<Room>()
            .HasIndex(r => r.Name)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}