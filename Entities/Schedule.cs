namespace HouseKeeperApi.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public int HouseId { get; set; }
        public virtual House House { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public DateOnly WeekStartDate { get; set; }
        public DateOnly WeekEndDate { get; set; }
    }
}
