using System;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class ReportPurchaseCosmeticViewModel
    {
        public string CosmeticName { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public int Count { get; set; }

        public int ClientId { get; set; }

        public decimal TotalCost { get; set; }
    }
}