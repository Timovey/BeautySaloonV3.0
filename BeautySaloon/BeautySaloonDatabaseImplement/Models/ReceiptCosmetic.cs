using System.ComponentModel.DataAnnotations;

namespace BeautySaloonDatabaseImplement.Models
{
    public class ReceiptCosmetic
    {
        public int Id { get; set; }

        public int ReceiptId { get; set; }

        public int CosmeticId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Receipt Receipt { get; set; }

        public virtual Cosmetic Cosmetic { get; set; }
    }
}