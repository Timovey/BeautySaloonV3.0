using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BindingModels
{
    public class VisitBindingModel
    {
        public int? Id { get; set; }

        public int? ClientId { get; set; }

        public DateTime Date { get; set; }

        public Dictionary<int, string> VisitProcedures { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
