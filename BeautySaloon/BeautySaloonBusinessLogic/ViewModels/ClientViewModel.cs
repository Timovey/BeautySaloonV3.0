using System.ComponentModel;

namespace BeautySaloonBusinessLogic.ViewModels
{
    /// <summary>
	/// Клиент
	/// </summary>
	public class ClientViewModel
    {
        public int Id { get; set; }

        [DisplayName("Имя")]
        public string ClientName { get; set; }

        [DisplayName("Фамилия")]
        public string ClientSurame { get; set; }

        [DisplayName("Почта")]
        public string Mail { get; set; }

        [DisplayName("Телефон")]
        public string Tel { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}