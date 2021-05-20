using System.ComponentModel;

namespace BeautySaloonBusinessLogic.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [DisplayName("Фамилия")]
        public string F_Name { get; set; }

        [DisplayName("Имя")]
        public string L_Name { get; set; }

        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Адрес эл. почты")]
        public string EMail { get; set; }

        [DisplayName("Сотовый")]
        public string PhoneNumber { get; set; }
    }
}