using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BindingModels
{
    public class DistributionBindingModel
    {
        public int? Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? VisitId { get; set; }

        public DateTime IssueDate { get; set; }

        public Dictionary<int, (string, int)> DistributionCosmetics { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}