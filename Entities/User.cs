using System.ComponentModel.DataAnnotations;

namespace HouseKeeperApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(50)]
        public required string Name { get; set; }
        [StringLength(50)]
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required int Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
