using System;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class ReportProceduresViewModel
    {
        public string TypeOfService { get; set; }

        public DateTime DateOfService { get; set; }

        public string ProcedureName { get; set; }

        public decimal? Price { get; set; }

    }
}