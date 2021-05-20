using System.ComponentModel;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class DataGridDistributionItemViewModel
    {
        public int Id { get; set; }

        [DisplayName("Косметика")]
        public string CosmeticName { get; set; }

        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
