using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class DistributionViewModel
    {
        [DisplayName("Номер выдачи")]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int? VisitId { get; set; }

        [DisplayName("Дата выдачи")]
        public DateTime IssueDate { get; set; }

        public Dictionary<int, (string, int)> DistributionCosmetics { get; set; }
    }
}