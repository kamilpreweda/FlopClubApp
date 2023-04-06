
namespace FlopClub.Dtos.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int HandsPlayed { get; set; }
        public decimal TotalWinnings { get; set; }
        public List<int> LobbyIds { get; set; } = new List<int>();
        public List<GetRoleDto> Roles { get; set; } = new List<GetRoleDto>();
    }
}
