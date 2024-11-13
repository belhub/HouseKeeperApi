namespace HouseKeeperApi.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }
        public string TextMessage { get; set; }
        public DateTime SendDate { get; set; }
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }
    }
}