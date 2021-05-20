using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using System.Windows;
using Unity;

namespace BeautySaloonViewEmployee
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private int? id;

        private EmployeeLogic logic;

        public MainWindow(EmployeeLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            var employee = logic.Read(new EmployeeBindingModel { Id = id })?[0];
            lbl_Employee.Content = "Сотрудник: " + employee.F_Name + " " + employee.L_Name;
        }

        private void CosmeticMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowCosmetics>();
            window.Id = (int)id;
            window.ShowDialog();
        }

        private void ReceiptMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowReceipts>();
            window.Id = (int)id;
            window.ShowDialog();
        }

        private void DistributionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowDistributions>();
            window.Id = (int)id;
            window.ShowDialog();
        }

        private void PurchaseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowPurchaseList>();
            window.Id = (int)id;
            window.ShowDialog();
        }

        private void ReportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowReportCosmetics>();
            window.Id = (int)id;
            window.ShowDialog();
        }

        private void StatisticMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowStatistic>();
            window.Id = (int)id;
            window.ShowDialog();
        }
    }
}