using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySaloonDatabaseImplement.Models
{
    public class Cosmetic
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        [Required]
        public string CosmeticName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("CosmeticId")]
        public virtual List<ReceiptCosmetic> ReceiptCosmetics { get; set; }

        [ForeignKey("CosmeticId")]
        public virtual List<DistributionCosmetic> DistributionCosmetics { get; set; }
    }
}