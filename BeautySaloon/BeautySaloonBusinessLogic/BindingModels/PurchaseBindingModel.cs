using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BindingModels
{
    public class PurchaseBindingModel
    {
        public int? Id { get; set; }

        public int? ClientId { get; set; }

        public int? ReceiptId { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public Dictionary<int, (string, decimal)> PurchaseProcedures { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
