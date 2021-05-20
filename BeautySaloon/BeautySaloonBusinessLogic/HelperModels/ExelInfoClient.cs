using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.HelperModels
{
    public class ExcelInfoClient
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportDistributionProcedureViewModel> Destributions { get; set; }
    }
}