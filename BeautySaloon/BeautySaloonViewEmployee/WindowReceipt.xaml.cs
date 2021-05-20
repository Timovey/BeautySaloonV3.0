using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace BeautySaloonViewEmployee
{
    /// <summary>
    /// Логика взаимодействия для WindowReceipt.xaml
    /// </summary>
    public partial class WindowReceipt : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        public int EmployeeId { set { employeeId = value; } }

        private readonly ReceiptLogic logicR;

        private readonly CosmeticLogic logicC;

        private int? id;

        private int? employeeId;

        private Dictionary<int, (string, int)> receiptCosmetics;

        public WindowReceipt(ReceiptLogic logicR, CosmeticLogic logicC)
        {
            InitializeComponent();
            this.logicR = logicR;
            this.logicC = logicC;
        }

        private void WindowReceipt_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ReceiptViewModel view = logicR.Read(new ReceiptBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        TextBoxTotalCost.Text = view.TotalCost.ToString();
                        TextBoxPurchaseDate.Text = view.PurchaseDate.ToString();
                        receiptCosmetics = view.ReceiptCosmetics;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                receiptCosmetics = new Dictionary<int, (string, int)>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (receiptCosmetics != null)
                {
                    dataGrid.Columns.Clear();
                    var list = new List<DataGridReceiptItemViewModel>();
                    foreach (var rc in receiptCosmetics)
                    {
                        list.Add(new DataGridReceiptItemViewModel()
                        {
                            Id = rc.Key,
                            CosmeticName = rc.Value.Item1,
                            Price = logicC.Read(new CosmeticBindingModel { Id = rc.Key })?[0].Price,
                            Count = rc.Value.Item2
                        });
                    }
                    dataGrid.ItemsSource = list;
                    dataGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            TextBoxPurchaseDate.Text = (DateTime.Now).ToString();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowSelectionCosmetics>();
            window.EmployeeId = (int)employeeId;
            if (window.ShowDialog().Value)
            {
                if (receiptCosmetics.ContainsKey(window.Id))
                {
                    receiptCosmetics[window.Id] = (window.CosmeticName, window.Count);
                }
                else
                {
                    receiptCosmetics.Add(window.Id, (window.CosmeticName, window.Count));
                }
                LoadData();
                CalcTotalCost();
            }
        }

        private void CalcTotalCost()
        {
            try
            {
                int totalCost = 0;
                foreach (var rc in receiptCosmetics)
                {
                    totalCost += rc.Value.Item2 * (int)logicC.Read(new CosmeticBindingModel { Id = rc.Key })?[0].Price;
                }
                TextBoxTotalCost.Text = totalCost.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells.Count != 0)
            {
                var window = Container.Resolve<WindowSelectionCosmetics>();
                window.Id = ((DataGridReceiptItemViewModel)dataGrid.SelectedCells[0].Item).Id;
                window.Count = ((DataGridReceiptItemViewModel)dataGrid.SelectedCells[0].Item).Count;
                window.EmployeeId = (int)employeeId;

                if (window.ShowDialog().Value)
                {
                    receiptCosmetics[window.Id] = (window.CosmeticName, window.Count);
                    LoadData();
                    CalcTotalCost();
                }
            }
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        receiptCosmetics.Remove(((DataGridReceiptItemViewModel)dataGrid.SelectedCells[0].Item).Id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (receiptCosmetics == null || receiptCosmetics.Count == 0)
            {
                MessageBox.Show("Заполните косметику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicR.CreateOrUpdate(new ReceiptBindingModel
                {
                    Id = id,
                    PurchaseDate = DateTime.Now,
                    TotalCost = Convert.ToInt32(TextBoxTotalCost.Text),
                    ReceiptCosmetics = receiptCosmetics,
                    EmployeeId = employeeId
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
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