using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using System.Windows;
using Unity;

namespace BeautySaloonViewClient
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

        private ClientLogic logic;

        public MainWindow(ClientLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void MenuItemProcedure_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowProcedures>();
            window.Id = (int)id;
            window.ShowDialog();
        }

        private void MenuItemVisit_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowVisits>();
            window.Id = (int)id;
            window.ShowDialog();

        }
        private void MenuItemPurchase_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowPurchases>();
            window.Id = (int)id;
            window.ShowDialog();

        }

        private void MenuItemBindingReceipt_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowBindingReciept>();
            window.ClientId = (int)id;
            window.ShowDialog();

        }

        private void MenuItemGetList_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowDistributionList>();
            window.ShowDialog();

        }

        private void MenuItemGetReport_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowReportProcedures>();
            window.ClientId = (int)id;
            window.ShowDialog();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var client= logic.Read(new ClientBindingModel { Id = id })?[0];
            labelClient.Content = "Клиент: " + client.ClientName + " " + client.ClientSurame;
        }
    }
}
