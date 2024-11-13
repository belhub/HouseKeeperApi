namespace HouseKeeperApi.Entities
{
    public enum RoleNameEnum
    {
        Landlord,
        Tenant,
        Admin
    }

    public class Role
    {
        public int Id { get; set; }
        public required string RoleName { get; set; }
    }
}