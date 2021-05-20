namespace BeautySaloonDatabaseImplement.Models
{
    public class ProcedurePurchase
    {
        public int Id { get; set; }

        public int ProcedureId { get; set; }

        public int PurchaseId { get; set; }

        public virtual Procedure Procedure { get; set; }

        public virtual Purchase Purchase { get; set; }
    }
}