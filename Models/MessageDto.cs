namespace HouseKeeperApi.Models
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string TextMessage { get; set; }
        public DateTime SendDate { get; set; }
        public int IssueId { get; set; }
    }
}
