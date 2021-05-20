using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.HelperModels
{
    class ExcelInfoEmployee
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportPurchaseCosmeticViewModel> Purchases { get; set; }
    }
}