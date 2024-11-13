namespace HouseKeeperApi.Entities
{
    public class Room
    {
        public required int Id { get; set; }
        public required string RoomName { get; set; }
        public int? TenantId { get; set; }
        public virtual required User Tenant { get; set; }
        public DateOnly? StartOfTheRent { get; set; }
        public DateOnly? EndOfTheRent { get; set; }
        public int? MonthlyCost { get; set; }
        public virtual List<Equipment> Equipments { get; set; } = new List<Equipment>();
        public int HouseId { get; set; }
        public virtual House House { get; set; }
    }
}