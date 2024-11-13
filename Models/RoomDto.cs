using HouseKeeperApi.Entities;

namespace HouseKeeperApi.Models
{
    public class RoomDto
    {
        public required int Id { get; set; }
        public required string RoomName { get; set; }
        public int? TenantId { get; set; }
        public DateOnly? StartOfTheRent { get; set; }
        public DateOnly? EndOfTheRent { get; set; }
        public int? MonthlyCost { get; set; }
        public List<Equipment> Equipments { get; set; }
        public int HouseId { get; set; }
    }
}
