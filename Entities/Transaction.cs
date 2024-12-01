using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseKeeperApi.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Value { get; set; }
        public DateTime TransacitonDate { get; set; }
        public string Status { get; set; }
        public int PayerId { get; set; }
        public virtual User Payer { get; set; }
        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }
        public int HouseId { get; set; }
        public virtual House House { get; set; }
    }
}
