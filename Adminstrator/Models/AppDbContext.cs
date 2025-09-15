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
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many: Event <-> Participant (implicit join table)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Participants)
                .WithMany(p => p.Events);
                

            modelBuilder.Entity<User>()
                .HasOne(u => u.Admin)
                .WithOne(a => a.User)
                .HasForeignKey<Administrator>(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Speaker <-> User (one-to-one)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Speak)
                .WithOne(s => s.User)
                .HasForeignKey<Speaker>(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Participant <-> User (one-to-one)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Parti)
                .WithOne(p => p.User)
                .HasForeignKey<Participant>(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Topic -> Event (one-to-many)
            modelBuilder.Entity<Topic>()
                .HasMany(t => t.Events)
                .WithOne(e => e.Topic)
                .HasForeignKey(e => e.TopicId)
                .OnDelete(DeleteBehavior.NoAction);

            //Speaker->Event(one - to - many)
            modelBuilder.Entity<Speaker>()
                .HasMany(s => s.Events)
                .WithOne(e => e.Speaker)
                .HasForeignKey(e => e.SpeakerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Location -> Event (one-to-many)
            modelBuilder.Entity<Location>()
                .HasMany(l => l.Events)
                .WithOne(e => e.Location)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.NoAction);

            // Event -> Feedback (one-to-many)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Feedbacks)
                .WithOne(f => f.Event)
                .HasForeignKey(f => f.EventID)
                .OnDelete(DeleteBehavior.NoAction);

            // Speaker -> Feedback (one-to-many)
            modelBuilder.Entity<Speaker>()
                .HasMany(s => s.Feedbacks)
                .WithOne(f => f.Speaker)
                .HasForeignKey(f => f.SpeakerID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
