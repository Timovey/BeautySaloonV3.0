using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class VisitViewModel
    {
        public int? Id { get; set; }

        public int ClientId { get; set; }

        public int? VisitId { get; set; }

        [DisplayName("Дата посещения")]
        public DateTime Date { get; set; }

        public Dictionary<int, string> VisitProcedures { get; set; }
    }
}