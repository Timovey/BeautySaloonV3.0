namespace BeautySaloonBusinessLogic.BindingModels
{
    /// <summary>
    /// Клиент
    /// </summary>
    public class ClientBindingModel
    {
        public int? Id { get; set; }

        public string ClientName { get; set; }

        public string ClientSurname { get; set; }

        public string Mail { get; set; }

        public string Tel { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
