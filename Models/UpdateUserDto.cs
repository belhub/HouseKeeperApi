namespace HouseKeeperApi.Models
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; } = null;
        public DateTime? DateOfBirth { get; set; }
        public string CurrentPassword { get; set; } // Aktualne hasło do weryfikacji
    }
}
