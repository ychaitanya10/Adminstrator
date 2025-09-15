using Microsoft.EntityFrameworkCore;

namespace Adminstrator.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PromotionCode> PromotionCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many: Event <-> Participant (implicit join table)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Participants)
                .WithMany(p => p.Events);

            // One-to-one: User <-> Participant
            modelBuilder.Entity<User>()
                .HasOne(u => u.ParticipantProfile)
                .WithOne(p => p.User)
                .HasForeignKey<Participant>(p => p.UserId);

            // One-to-one: User <-> Speaker
            modelBuilder.Entity<User>()
                .HasOne(u => u.SpeakerProfile)
                .WithOne(s => s.User)
                .HasForeignKey<Speaker>(s => s.UserId);

            // One-to-one: User <-> Administrator
            modelBuilder.Entity<User>()
                .HasOne(u => u.AdministratorProfile)
                .WithOne(a => a.User)
                .HasForeignKey<Administrator>(a => a.UserId);

            // Topic -> Event (one-to-many)
            modelBuilder.Entity<Topic>()
                .HasMany(t => t.Events)
                .WithOne(e => e.Topic)
                .HasForeignKey(e => e.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            // Speaker -> Event (one-to-many)
            modelBuilder.Entity<Speaker>()
                .HasMany(s => s.Events)
                .WithOne(e => e.Speaker)
                .HasForeignKey(e => e.SpeakerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Location -> Event (one-to-many)
            modelBuilder.Entity<Location>()
                .HasMany(l => l.Events)
                .WithOne(e => e.Location)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
