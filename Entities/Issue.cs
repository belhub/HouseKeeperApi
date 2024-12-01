namespace HouseKeeperApi.Entities
{
    public class Issue
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int HouseId { get; set; }
        public virtual House House { get; set; }
        public string Priority { get; set; }
        public List<string> ParticipantsName { get; set; }
        public List<int> ParticipantsId { get; set; }
        public string Status { get; set; }
        public virtual List<Message> Messages { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public DateTime CreationDate { get; set; }
        public List<int> ViewedBy { get; set; }
    }
}
