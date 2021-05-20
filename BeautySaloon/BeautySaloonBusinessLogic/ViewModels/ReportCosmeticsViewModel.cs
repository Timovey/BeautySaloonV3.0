using System;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class ReportCosmeticsViewModel
    {
        public string TypeOfService { get; set; }

        public DateTime DateOfService { get; set; }

        public string CosmeticName { get; set; }

        public int Count { get; set; }
    }
}