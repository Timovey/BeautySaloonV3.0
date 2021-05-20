using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySaloonDatabaseImplement.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientSurame { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        public string Tel { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [ForeignKey("ClientId")]
        public List<Procedure> Procedure { get; set; }

        [ForeignKey("ClientId")]
        public List<Visit> Visit { get; set; }

        [ForeignKey("ClientId")]
        public List<Purchase> Purchase { get; set; }
    }
}