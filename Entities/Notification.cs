namespace HouseKeeperApi.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IssueNotification { get; set; }
        public bool TransactionNotification { get; set; }
    }
}
