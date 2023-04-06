
namespace FlopClub.Models
{
    public class UserRole
    {
        [Key]
        public int UserId { get; set; }
        public User User { get; set; }
        [Key]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
