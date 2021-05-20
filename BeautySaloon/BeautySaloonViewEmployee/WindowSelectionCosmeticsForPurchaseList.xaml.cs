using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.ViewModels;
using System.Windows;
using Unity;

namespace BeautySaloonViewEmployee
{
    /// <summary>
    /// Логика взаимодействия для WindowSelectionCosmeticsForPurchaseList.xaml
    /// </summary>
    public partial class WindowSelectionCosmeticsForPurchaseList : Window
    {

        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { get { return (ComboBoxCosmeticName.SelectedItem as CosmeticViewModel).Id; } set { id = value; } }

        public string CosmeticName { get { return (ComboBoxCosmeticName.SelectedItem as CosmeticViewModel).CosmeticName; } }

        public int EmployeeId { set { employeeId = value; } }

        private int? id;

        private int? employeeId;

        private readonly CosmeticLogic logic;

        public WindowSelectionCosmeticsForPurchaseList(CosmeticLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void WindowSelectionCosmeticsForPurchaseList_Loaded(object sender, RoutedEventArgs e)
        {
            var list = logic.Read(new CosmeticBindingModel { EmployeeId = employeeId });
            if (list != null)
            {
                ComboBoxCosmeticName.ItemsSource = list;
            }
            if (id != null)
            {
                ComboBoxCosmeticName.SelectedItem = SetValue(id.Value);
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxCosmeticName.SelectedValue == null)
            {
                MessageBox.Show("Выберите косметику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private CosmeticViewModel SetValue(int value)
        {
            foreach (var item in ComboBoxCosmeticName.Items)
            {
                if ((item as CosmeticViewModel).Id == value)
                {
                    return item as CosmeticViewModel;
                }
            }
            return null;
        }
    }
}