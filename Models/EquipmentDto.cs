namespace HouseKeeperApi.Models
{
    public class EquipmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Worth { get; set; }
        public DateOnly DateAdded { get; set; }
        public bool BelongToHouse { get; set; }
        public int OwnerId { get; set; }
        public int RoomId { get; set; }
    }
}
