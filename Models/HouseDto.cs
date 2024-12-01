using HouseKeeperApi.Entities;

namespace HouseKeeperApi.Models
{
    public class HouseDto
    {
        public int Id { get; set; }
        public string HouseName { get; set; }
        public int OwnerId { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
        //public virtual List<User> Tenants { get; set; } = new List<User>();

    }
}
