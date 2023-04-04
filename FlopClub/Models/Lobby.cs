using FlopClub.Migrations;

namespace FlopClub.Models
{
    public class Lobby
    {
        public int Id { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        [JsonIgnore]
        public Game? Game { get; set; }
    }
}
