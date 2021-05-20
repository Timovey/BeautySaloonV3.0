namespace BeautySaloonBusinessLogic.BindingModels
{
    public class CosmeticBindingModel
    {
        public int? Id { get; set; }

        public int? EmployeeId { get; set; }

        public string CosmeticName { get; set; }

        public decimal Price { get; set; }
    }
}
