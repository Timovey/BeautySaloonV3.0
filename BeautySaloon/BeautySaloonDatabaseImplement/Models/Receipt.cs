using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySaloonDatabaseImplement.Models
{
    public class Receipt
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        [ForeignKey("ReceiptId")]
        public virtual List<Purchase> Purchases { get; set; }

        [Required]
        public decimal TotalCost { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [ForeignKey("ReceiptId")]
        public virtual List<ReceiptCosmetic> ReceiptCosmetics { get; set; }
    }
}