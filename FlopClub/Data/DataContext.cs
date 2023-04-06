namespace FlopClub.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Card> Cards => Set<Card>();

        public DbSet<Player> Players => Set<Player>();

        public DbSet<Game> Games => Set<Game>();

        public DbSet<Lobby> Lobbies => Set<Lobby>();

        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Lobby)
                .WithOne()
                .HasForeignKey<Lobby>(l => l.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "GameAdmin" },
                new Role { Id = 2, Name = "GamePlayer" },
                new Role { Id = 3, Name = "GameUser" });
        }
    }
}
