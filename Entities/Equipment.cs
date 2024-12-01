using System.Text.Json.Serialization;

namespace HouseKeeperApi.Entities
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Worth { get; set; }
        public DateOnly DateAdded { get; set; }
        public bool BelongToHouse { get; set; }
        public int OwnerId { get; set; }
        public virtual required User Owner { get; set; }
        public int RoomId { get; set; }
        [JsonIgnore]
        public virtual Room Room { get; set; }
    }
}