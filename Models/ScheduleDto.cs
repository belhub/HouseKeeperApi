namespace HouseKeeperApi.Models
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public int HouseId { get; set; }
        public int UserId { get; set; }
        public int UserName { get; set; }
        public DateOnly WeekStartDate { get; set; }
        public DateOnly WeekEndDate { get; set; }
    }
}
