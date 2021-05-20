using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.BindingModels;
using Unity;

namespace BeautySaloonViewClient
{
    /// <summary>
    /// Логика взаимодействия для WindowProcedure.xaml
    /// </summary>
    public partial class WindowProcedure : Window
    {

        [Dependency]
        public IUnityContainer Container { get; set; } 
        public int Id { set { id = value; } }
        public int ClientId { set { clientId = value; } }
        private readonly ProcedureLogic logic;
        private int? id;
        private int? clientId;
        public WindowProcedure(ProcedureLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void WindowProcedure_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new ProcedureBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.ProcedureName;
                        textBoxPrice.Text = view.Price.ToString();
                        textBoxDuration.Text = view.Duration.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
             MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxDuration.Text))
            {
                MessageBox.Show("Введите продолжительность", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Введите цену", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new ProcedureBindingModel
                {
                    Id = id,
                    ProcedureName = textBoxName.Text,
                    Duration = Convert.ToInt32(textBoxDuration.Text),
                    Price = Convert.ToDecimal(textBoxPrice.Text),
                    ClientId = clientId
                });
              
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
