using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class ReceiptViewModel
    {
        [DisplayName("Номер чека")]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [DisplayName("Общая стоимость")]
        public decimal TotalCost { get; set; }

        [DisplayName("Дата покупки")]
        public DateTime PurchaseDate { get; set; }

        public Dictionary<int, (string, int)> ReceiptCosmetics { get; set; }
    }
}