using HouseKeeperApi.Entities;

namespace HouseKeeperApi.Models
{
    public class IssueDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int HouseId { get; set; }
        public string Priority { get; set; }
        public List<string> ParticipantsName { get; set; }
        public List<int> ParticipantsId { get; set; }
        public string Status { get; set; }
        public List<Message> Messages { get; set; }
        public int CreatorId { get; set; }
        public List<int> ViewedBy { get; set; }
    }
}
