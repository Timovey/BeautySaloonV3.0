using System;
using System.Windows;
using BeautySaloonBusinessLogic.BindingModels;
using Unity;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.ViewModels;

namespace BeautySaloonViewClient
{
    /// <summary>
    /// Логика взаимодействия для WindowBindingProcedure.xaml
    /// </summary>
    public partial class WindowBindingProcedure : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id
        {
            get { return (ComboBoxProcedures.SelectedItem as ProcedureViewModel).Id; }
            set { id = value; }
        }
        private int? id;
        public string ProcedureName { get { return (ComboBoxProcedures.SelectedItem as ProcedureViewModel).ProcedureName; } }
        public decimal ProcedurePrice { get { return (ComboBoxProcedures.SelectedItem as ProcedureViewModel).Price; } }

        public int ClientId { set { clientId = value; } }

        private int? clientId;

        private readonly ProcedureLogic logic;

        public WindowBindingProcedure(ProcedureLogic logic)
        {
            InitializeComponent();
            this.logic = logic;

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            if (ComboBoxProcedures.SelectedValue == null)
            {
                MessageBox.Show("Выберите процедуру", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            this.DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private ProcedureViewModel SetValue(int value)
        {
            foreach(var item in ComboBoxProcedures.Items)
            {
                if((item as ProcedureViewModel).Id == value)
                {
                    return item as ProcedureViewModel;
                }
            }
            return null;
        }

        private void WindowBindingProcedure_Loaded(object sender, RoutedEventArgs e)
        {
            var list = logic.Read(new ProcedureBindingModel
            {
                ClientId = clientId
            });
            if(list != null)
            {
                ComboBoxProcedures.ItemsSource = list;
            }
            if(id != null)
            {
                ComboBoxProcedures.SelectedItem = SetValue(id.Value);
                id = null;
            }
        }
    }
}
