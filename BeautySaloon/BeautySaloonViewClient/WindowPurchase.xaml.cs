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
using BeautySaloonBusinessLogic.ViewModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Enums;
using Unity;
using System.Data;
using System.ComponentModel;
using System.Reflection;

namespace BeautySaloonViewClient
{
    /// <summary>
    /// Логика взаимодействия для WindowPurchase.xaml
    /// </summary>
    public partial class WindowPurchase : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        public int ClientId { set { clientId = value; } }
        private readonly PurchaseLogic logic;
        private int? id;
        private int? clientId;
        private Dictionary<int, (string, decimal)> purchasesProcedures;

        public WindowPurchase(PurchaseLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void WindowPurchase_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    PurchaseViewModel view = logic.Read(new PurchaseBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    if (view != null)
                    {
                        purchasesProcedures = view.PurchaseProcedures;
                        textBoxSum.Text = view.Price.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }
            }
            else
            {
                purchasesProcedures = new Dictionary<int, (string, decimal)>();
            }
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                if (purchasesProcedures != null)
                {
                    List<PurchaseDataGridModel> list = new List<PurchaseDataGridModel>();
                    foreach(var item in purchasesProcedures)
                    {
                        list.Add(new PurchaseDataGridModel() { Id = item.Key, 
                           ProcedureName = item.Value.Item1, ProcedurePrice = item.Value.Item2 });
                    }
                    DataGridProcedures.ItemsSource = list;

                    DataGridProcedures.Columns[0].Header = "Id";
                    DataGridProcedures.Columns[0].Width = 0;
                    DataGridProcedures.Columns[0].Visibility = Visibility.Hidden;
                    DataGridProcedures.Columns[1].Header = "Название процедуры:";
                    DataGridProcedures.Columns[2].Header = "Цена:";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }

            if (purchasesProcedures != null)
            {
                try
                {
                    decimal sum = 0;
                    foreach(var item in purchasesProcedures.Values)
                    {
                        sum += item.Item2;
                    }
                    textBoxSum.Text = sum.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowBindingProcedure>();
            window.ClientId = (int)clientId;
            if (window.ShowDialog().Value)
            {
                if (!purchasesProcedures.ContainsKey(window.Id)) { 
                    purchasesProcedures.Add(window.Id, (window.ProcedureName, window.ProcedurePrice));
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridProcedures.SelectedCells.Count != 0)
            {
                var window = Container.Resolve<WindowBindingProcedure>();
                var cellInfo = DataGridProcedures.SelectedCells[0];
                PurchaseDataGridModel content = (PurchaseDataGridModel)(cellInfo.Item);

                purchasesProcedures.Remove(content.Id);
                window.Id = content.Id;
                window.ClientId = (int)clientId;
                if (window.ShowDialog().Value)
                {
                    if (!purchasesProcedures.ContainsValue((window.ProcedureName, window.ProcedurePrice)))
                    {
                        purchasesProcedures[window.Id] = (window.ProcedureName, window.ProcedurePrice);
                    }
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridProcedures.SelectedCells.Count != 0)
            {
                var result = MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo,
              MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var cellInfo = DataGridProcedures.SelectedCells[0];
                        PurchaseDataGridModel content = (PurchaseDataGridModel)(cellInfo.Item);
                        int id = content.Id;

                        purchasesProcedures.Remove(id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (purchasesProcedures == null || purchasesProcedures.Count == 0)
            {
                MessageBox.Show("Заполните процедуры", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new PurchaseBindingModel
                {
                    Id = id,
                    Date = DateTime.Now,
                    Price = Convert.ToDecimal(textBoxSum.Text),
                    PurchaseProcedures = purchasesProcedures,
                    ClientId = clientId
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
               MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
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

        /// <summary>
        /// Данные для привязки DisplayName к названиям столбцов
        /// </summary>
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
            new DataGridLength(1, DataGridLengthUnitType.Star); // честно я хз, но вроде это растягивает последний столбец до полной ширины

        }
        /// <summary>
        /// метод привязки DisplayName к названию столбца
        /// </summary>
        public static string GetPropertyDisplayName(object descriptor)
        {

            PropertyDescriptor pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                DisplayNameAttribute displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                PropertyInfo pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        DisplayNameAttribute displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }
            return null;
        }

    }
}
