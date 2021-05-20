using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BindingModels
{
    public class ReportBindingModelEmployee
    {
        public string FileName { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public int? EmployeeId { get; set; }

        public List<int> purchaseCosmetics { get; set; }
    }
}