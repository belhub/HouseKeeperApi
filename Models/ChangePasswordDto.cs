namespace HouseKeeperApi.Models
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; } // ID użytkownika
        public string CurrentPassword { get; set; } // Aktualne hasło
        public string NewPassword { get; set; } // Nowe hasło
        public string ConfirmNewPassword { get; set; } // Potwierdzenie nowego hasła
    }
}
