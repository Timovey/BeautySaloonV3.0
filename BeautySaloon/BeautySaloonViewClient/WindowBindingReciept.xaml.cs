using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Windows;
using Unity;


namespace BeautySaloonViewClient
{
    /// <summary>
    /// Логика взаимодействия для WindowBindingReciept.xaml
    /// </summary>
    public partial class WindowBindingReciept : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int ReceiptId
        {
            get { return (int)(ComboBoxReceipt.SelectedItem as ReceiptViewModel).Id; }
            set { ComboBoxReceipt.SelectedItem = SetValueReceipt(value); }
        }

        public int PurchaseId
        {
            get { return (ComboBoxPurchase.SelectedItem as PurchaseViewModel).Id; }
            set { ComboBoxPurchase.SelectedItem = SetValuePurchase(value); }
        }

        public int ClientId { set { clientId = value; } }

        private int? clientId;

        private readonly ReceiptLogic logicReceipt;

        private readonly PurchaseLogic logicPurchase;

        public WindowBindingReciept(ReceiptLogic logicReceipt, PurchaseLogic logicPurchase)
        {
            InitializeComponent();
            this.logicReceipt = logicReceipt;
            this.logicPurchase = logicPurchase;
        }

        private void WindowBindingReciept_Loaded(object sender, RoutedEventArgs e)
        {
            var listReceipt = logicReceipt.Read(null);
            if (logicReceipt != null)
            {
                ComboBoxReceipt.ItemsSource = listReceipt;
            }
            var listPurchase = logicPurchase.Read(new PurchaseBindingModel { ClientId = clientId });
            if (listPurchase != null)
            {
                ComboBoxPurchase.ItemsSource = listPurchase;
            }
        }

        private void buttonBinding_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxReceipt.SelectedValue == null)
            {
                MessageBox.Show("Выберите посещение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ComboBoxPurchase.SelectedValue == null)
            {
                MessageBox.Show("Выберите выдачу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicPurchase.Linking(new PurchaseLinkingBindingModel
                {
                    ReceiptId = (int)(ComboBoxReceipt.SelectedItem as ReceiptViewModel).Id,
                    PurchaseId = (ComboBoxPurchase.SelectedItem as PurchaseViewModel).Id
                });
                MessageBox.Show("Привязка прошла успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private ReceiptViewModel SetValueReceipt(int value)
        {
            foreach (var item in ComboBoxReceipt.Items)
            {
                if ((item as ReceiptViewModel).Id == value)
                {
                    return item as ReceiptViewModel;
                }
            }
            return null;
        }

        private PurchaseViewModel SetValuePurchase(int value)
        {
            foreach (var item in ComboBoxPurchase.Items)
            {
                if ((item as PurchaseViewModel).Id == value)
                {
                    return item as PurchaseViewModel;
                }
            }
            return null;
        }
    }
}
