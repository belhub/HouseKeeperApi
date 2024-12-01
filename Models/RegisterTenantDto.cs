namespace HouseKeeperApi.Models
{
    public class RegisterTenantDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; } = 1;
        public int RoomId { get; set; }
        public int HouseId { get; set; }
    }
}
