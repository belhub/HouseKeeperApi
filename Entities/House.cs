namespace HouseKeeperApi.Entities
{
    public class House
    {
        public int Id { get; set; }
        public string HouseName { get; set; }
        public int OwnerId { get; set; }
        public virtual required User Owner { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public virtual List<Room> Rooms { get; set; } = new List<Room>();

    }
}
