using System.ComponentModel.DataAnnotations.Schema;

namespace FlopClub.Models
{
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
