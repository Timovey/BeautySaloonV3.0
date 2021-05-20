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
    /// Логика взаимодействия для WindowDistribution.xaml
    /// </summary>
    public partial class WindowDistribution : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        public int EmployeeId { set { employeeId = value; } }

        private readonly DistributionLogic logic;

        private int? id;

        private int? employeeId;

        private Dictionary<int, (string, int)> distributionCosmetics;

        public WindowDistribution(DistributionLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void WindowDistribution_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    DistributionViewModel view = logic.Read(new DistributionBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        TextBoxIssueDate.Text = view.IssueDate.ToString();
                        distributionCosmetics = view.DistributionCosmetics;
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
                distributionCosmetics = new Dictionary<int, (string, int)>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (distributionCosmetics != null)
                {
                    dataGrid.Columns.Clear();
                    var list = new List<DataGridDistributionItemViewModel>();
                    foreach (var dc in distributionCosmetics)
                    {
                        list.Add(new DataGridDistributionItemViewModel()
                        {
                            Id = dc.Key,
                            CosmeticName = dc.Value.Item1,
                            Count = dc.Value.Item2
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
            TextBoxIssueDate.Text = (DateTime.Now).ToString();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowSelectionCosmetics>();
            window.EmployeeId = (int)employeeId;
            if (window.ShowDialog().Value)
            {
                if (distributionCosmetics.ContainsKey(window.Id))
                {
                    distributionCosmetics[window.Id] = (window.CosmeticName, window.Count);
                }
                else
                {
                    distributionCosmetics.Add(window.Id, (window.CosmeticName, window.Count));
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells.Count != 0)
            {
                var window = Container.Resolve<WindowSelectionCosmetics>();
                window.Id =((DataGridDistributionItemViewModel)dataGrid.SelectedCells[0].Item).Id;
                window.Count = ((DataGridDistributionItemViewModel)dataGrid.SelectedCells[0].Item).Count;
                window.EmployeeId = (int)employeeId;
                if (window.ShowDialog().Value)
                {
                    distributionCosmetics[window.Id] = (window.CosmeticName, window.Count);
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
                        distributionCosmetics.Remove(((DataGridDistributionItemViewModel)dataGrid.SelectedCells[0].Item).Id);
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
            if (distributionCosmetics == null || distributionCosmetics.Count == 0)
            {
                MessageBox.Show("Заполните косметику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new DistributionBindingModel
                {
                    Id = id,
                    IssueDate = DateTime.Now,
                    DistributionCosmetics = distributionCosmetics,
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