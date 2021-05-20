using System.ComponentModel;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class CosmeticViewModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [DisplayName("Название косметики")]
        public string CosmeticName { get; set; }

        [DisplayName("Стоимость")]
        public decimal Price { get; set; }
    }
}
