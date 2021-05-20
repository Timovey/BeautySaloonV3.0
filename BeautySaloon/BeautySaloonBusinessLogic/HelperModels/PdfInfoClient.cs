using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;


namespace BeautySaloonBusinessLogic.HelperModels
{
    class PdfInfoClient
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public List<ReportProceduresViewModel> Procedures { get; set; }
    }
}