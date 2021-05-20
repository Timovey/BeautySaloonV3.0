using System.ComponentModel;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class ProcedureViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        [DisplayName("Имя")]
        public string ProcedureName { get; set; }

        [DisplayName("Продолжительность (мин)")]
        //Продолжительность в минутах
        public int Duration { get; set; }

        [DisplayName("Цена (руб)")]
        public decimal Price { get; set; }
    }
}