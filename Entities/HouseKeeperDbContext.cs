using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Entities
{
    public class HouseKeeperDbContext : DbContext
    {
        public HouseKeeperDbContext(DbContextOptions<HouseKeeperDbContext> options) : base(options) // Przekazanie options do klasy bazowej
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacja Issue -> Messages
            modelBuilder.Entity<Issue>()
                .HasMany(i => i.Messages)
                .WithOne(m => m.Issue)
                .HasForeignKey(m => m.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja User -> Message (brak kaskadowego usuwania)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);


            // Relacja User -> Issues (brak kaskadowego usuwania)
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Creator)
                .WithMany()
                .HasForeignKey(i => i.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relacja House -> Rooms
            modelBuilder.Entity<House>()
                .HasMany(h => h.Rooms)
                .WithOne(r => r.House)
                .HasForeignKey(r => r.HouseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja User -> House (brak kaskadowego usuwania)
            modelBuilder.Entity<House>()
                .HasOne(h => h.Owner)
                .WithMany()
                .HasForeignKey(h => h.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relacja Equipments -> Room
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Equipments)
                .WithOne(e => e.Room)
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
            // Relacja User -> Rooms (brak kaskadowego usuwania)
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Tenant)
                .WithMany()
                .HasForeignKey(r => r.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relacja User -> Equipment (brak kaskadowego usuwania)
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

    }
}
