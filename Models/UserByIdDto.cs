namespace HouseKeeperApi.Models
{
    public class UserByIdDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required int Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
    }
}
