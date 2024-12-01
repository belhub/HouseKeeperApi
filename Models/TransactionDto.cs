namespace HouseKeeperApi.Models
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public DateTime TransacitonDate { get; set; }
        public string Status { get; set; }
        public int PayerId { get; set; }
        public int ReceiverId { get; set; }
        public int HouseId { get; set; }
    }
}
