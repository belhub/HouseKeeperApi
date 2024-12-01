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
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Notification> Notifications { get; set; }
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


            modelBuilder.Entity<House>()
                .HasMany(h => h.Tenants)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "HouseTenant",
                    ht => ht.HasOne<User>().WithMany().HasForeignKey("TenantId").OnDelete(DeleteBehavior.Cascade),
                    ht => ht.HasOne<House>().WithMany().HasForeignKey("HouseId").OnDelete(DeleteBehavior.Cascade),
                    ht =>
                    {
                        ht.HasKey("HouseId", "TenantId");
                    }
                );

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

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);

                // Relacja z User (SendFrom)
                entity.HasOne(t => t.Payer)
                      .WithMany() // Zakładamy, że User nie ma nawigacji do Transakcji jako "nadawca"
                      .HasForeignKey(t => t.PayerId)
                      .OnDelete(DeleteBehavior.NoAction); // Unikamy usuwania użytkownika wraz z transakcją

                // Relacja z User (SendTo)
                entity.HasOne(t => t.Receiver)
                      .WithMany() // Zakładamy, że User nie ma nawigacji do Transakcji jako "odbiorca"
                      .HasForeignKey(t => t.ReceiverId)
                      .OnDelete(DeleteBehavior.NoAction);

                // Relacja z House
                entity.HasOne(t => t.House)
                      .WithMany()
                      .HasForeignKey(t => t.HouseId)
                      .OnDelete(DeleteBehavior.Cascade); // Usunięcie domu usuwa związane transakcje

                entity.Property(t => t.Status)
                      .IsRequired()
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasOne(s => s.House)
                    .WithMany()
                    .HasForeignKey(s => s.HouseId)
                    .OnDelete(DeleteBehavior.Cascade); // Usunięcie domu usuwa związane grafiki

                entity.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Usenięcie Usera usuwa jego notyfikacje

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

    }
}
