using System.ComponentModel.DataAnnotations;

namespace BeautySaloonDatabaseImplement.Models
{
    public class DistributionCosmetic
    {
        public int Id { get; set; }

        public int DistributionId { get; set; }

        public int CosmeticId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Distribution Distribution { get; set; }

        public virtual Cosmetic Cosmetic { get; set; }
    }
}