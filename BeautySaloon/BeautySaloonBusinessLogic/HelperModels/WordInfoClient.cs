using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.HelperModels
{
    class WordInfoClient
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportDistributionProcedureViewModel> Destributions { get; set; }
    }
}