using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.ViewModels;
using Microsoft.Win32;
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
    /// Логика взаимодействия для WindowPurchaseList.xaml
    /// </summary>
    public partial class WindowPurchaseList : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly ReportLogicEmployee report;

        public int Id { set { id = value; } }

        private int? id;

        private Dictionary<int, string> purchaseCosmetics;

        public WindowPurchaseList(ReportLogicEmployee logicEmployee)
        {
            InitializeComponent();
            report = logicEmployee;
        }

        private void LoadData()
        {
            try
            {
                if (purchaseCosmetics != null)
                {
                    dataGrid.Columns.Clear();
                    var list = new List<DataGridPurchaseListItemViewModel>();
                    foreach (var pc in purchaseCosmetics)
                    {
                        list.Add(new DataGridPurchaseListItemViewModel()
                        {
                            Id = pc.Key,
                            CosmeticName = pc.Value
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
        }

        private void WindowPurchaseList_Loaded(object sender, RoutedEventArgs e)
        {
            purchaseCosmetics = new Dictionary<int, string>();
        }

        private void buttonWord_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "docx|*.docx";
            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    report.SavePurchaseListToWordFile(new ReportBindingModelEmployee { FileName = dialog.FileName, purchaseCosmetics = new List<int>(purchaseCosmetics.Keys), EmployeeId = id });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonExcel_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "xlsx|*.xlsx";
            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    report.SavePurchaseListToExcelFile(new ReportBindingModelEmployee { FileName = dialog.FileName, purchaseCosmetics = new List<int>(purchaseCosmetics.Keys), EmployeeId = id });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowSelectionCosmeticsForPurchaseList>();
            window.EmployeeId = (int)id;
            if (window.ShowDialog().Value)
            {
                if (purchaseCosmetics.ContainsKey(window.Id))
                {
                    purchaseCosmetics[window.Id] = window.CosmeticName;
                }
                else
                {
                    purchaseCosmetics.Add(window.Id, window.CosmeticName);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells.Count != 0)
            {
                var window = Container.Resolve<WindowSelectionCosmeticsForPurchaseList>();
                window.Id = ((DataGridPurchaseListItemViewModel)dataGrid.SelectedCells[0].Item).Id;
                window.EmployeeId = (int)id;
                if (window.ShowDialog().Value)
                {
                    purchaseCosmetics[window.Id] = window.CosmeticName;
                    LoadData();
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
                        purchaseCosmetics.Remove(((DataGridPurchaseListItemViewModel)dataGrid.SelectedCells[0].Item).Id);
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