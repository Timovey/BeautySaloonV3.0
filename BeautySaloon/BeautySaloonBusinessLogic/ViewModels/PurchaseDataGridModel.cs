using System.ComponentModel;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class PurchaseDataGridModel
    {
        public int Id { get; set; }

        [DisplayName("Название процедуры")]
        public string ProcedureName { get; set; }

        [DisplayName("Цена")]
        public decimal ProcedurePrice { get; set; }
    }
}