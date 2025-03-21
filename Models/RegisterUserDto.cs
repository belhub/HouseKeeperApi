﻿namespace HouseKeeperApi.Models
{
    public class RegisterUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; } = 2;
    }
}
