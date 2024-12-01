namespace HouseKeeperApi.Models
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool? IssueNotification { get; set; }
        public bool? TransactionNotification { get; set; }
    }
}
