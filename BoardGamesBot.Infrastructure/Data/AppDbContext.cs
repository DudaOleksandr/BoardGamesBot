using BoardGamesBot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesBot.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Room> Rooms { get; init; }
    public DbSet<RoomMember> RoomMembers { get; init; }
    public DbSet<Event> Events { get; init; }
    public DbSet<EventParticipant> EventParticipants { get; init; }
    
    public DbSet<Notification> Notifications { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Room -> Members
        modelBuilder.Entity<Room>()
            .HasMany(r => r.Members)
            .WithOne(m => m.Room)
            .HasForeignKey(m => m.RoomId);

        // Room -> Events
        modelBuilder.Entity<Room>()
            .HasMany(r => r.Events)
            .WithOne(e => e.Room)
            .HasForeignKey(e => e.RoomId);

        // Event -> Participants
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Participants)
            .WithOne(ep => ep.Event)
            .HasForeignKey(ep => ep.EventId);

        // EventParticipant -> RoomMember
        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.RoomMember)
            .WithMany()
            .HasForeignKey(ep => ep.RoomMemberId);

        // Unique constraint for Room.Name
        modelBuilder.Entity<Room>()
            .HasIndex(r => r.Name)
            .IsUnique();
        
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Notifications)
            .WithOne()
            .HasForeignKey(n => n.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for Event Name within a Room
        modelBuilder.Entity<Event>()
            .HasIndex(e => new { e.RoomId, e.Name })
            .IsUnique();
    }
}