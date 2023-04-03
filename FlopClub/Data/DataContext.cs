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
    }
}
