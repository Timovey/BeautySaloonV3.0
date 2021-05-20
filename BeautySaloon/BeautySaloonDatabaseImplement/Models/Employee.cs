using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySaloonDatabaseImplement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string F_Name { get; set; }

        [Required]
        public string L_Name { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string EMail { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual List<Cosmetic> Cosmetic { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual List<Distribution> Distribution { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual List<Receipt> Receipt { get; set; }
    }
}