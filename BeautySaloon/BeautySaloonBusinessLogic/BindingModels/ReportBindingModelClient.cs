using System;

namespace BeautySaloonBusinessLogic.BindingModels
{
    public class ReportBindingModelClient
    {
        public string FileName { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public int? ClientId { get; set; }
    }
}
