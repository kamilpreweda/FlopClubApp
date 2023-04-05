
namespace FlopClub.Models
{
    public class Lobby
    {
        public int Id { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public int GameId { get; set; }
    }
}
