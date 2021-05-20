using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BeautySaloonDatabaseImplement.Models
{
    public class Procedure
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        [Required]
        public string ProcedureName { get; set; }

        //Продолжительность в минутах
        [Required]
        public int Duration { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("ProcedureId")]
        public virtual List<ProcedurePurchase> ProcedurePurchase { get; set; }

        [ForeignKey("ProcedureId")]
        public virtual List<ProcedureVisit> ProcedureVisit { get; set; }

        public virtual Client Client { get; set; }
    }
}