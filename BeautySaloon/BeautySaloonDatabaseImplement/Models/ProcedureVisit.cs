namespace BeautySaloonDatabaseImplement.Models
{
    public class ProcedureVisit
    {
        public int Id { get; set; }

        public int ProcedureId { get; set; }

        public int VisitId { get; set; }

        public virtual Procedure Procedure { get; set; }

        public virtual Visit Visit { get; set; }
    }
}