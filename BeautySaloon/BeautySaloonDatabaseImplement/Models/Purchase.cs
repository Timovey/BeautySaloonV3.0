using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySaloonDatabaseImplement.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("PurchaseId")]
        public virtual List<ProcedurePurchase> ProcedurePurchase { get; set; }
        
        public int? ReceiptId { get; set; }
        
        public virtual Receipt Receipt { get; set; }

        public virtual Client Client { get; set; }
    }
}