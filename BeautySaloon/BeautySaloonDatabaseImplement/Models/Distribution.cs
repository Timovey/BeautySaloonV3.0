using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySaloonDatabaseImplement.Models
{
    public class Distribution
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [ForeignKey("DistributionId")]
        public virtual List<DistributionCosmetic> DistributionCosmetics { get; set; }

        public int? VisitId { get; set; }

        public virtual Visit Visit { get; set; }
    }
}