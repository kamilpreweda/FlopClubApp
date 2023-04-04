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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lobby>()
                .HasOne(l => l.Game)
                .WithOne(g => g.Lobby)
                .HasForeignKey<Game>(g => g.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
