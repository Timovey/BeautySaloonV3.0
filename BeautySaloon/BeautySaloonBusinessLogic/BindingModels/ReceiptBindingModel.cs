using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BindingModels
{
    public class ReceiptBindingModel
    {
        public int? Id { get; set; }

        public int? EmployeeId { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime PurchaseDate { get; set; }

        public Dictionary<int, (string, int)> ReceiptCosmetics { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}