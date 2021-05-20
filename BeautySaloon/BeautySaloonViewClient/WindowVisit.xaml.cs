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
    /// Логика взаимодействия для WindowVisit.xaml
    /// </summary>
    public partial class WindowVisit : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        public int ClientId { set { clientId = value; } }
        private readonly VisitLogic logic;
        private int? id;
        private int? clientId;
        private Dictionary<int, string> visitsProcedures;
        private DateTime oldDate = DateTime.MinValue;

        public WindowVisit(VisitLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void WindowVisit_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    VisitViewModel view = logic.Read(new VisitBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    if (view != null)
                    {
                        CalendarVisit.DisplayDate = view.Date;
                        CalendarVisit.SelectedDate = view.Date;
                        oldDate = view.Date;
                        visitsProcedures = view.VisitProcedures;
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
                visitsProcedures = new Dictionary<int, string>();
                CalendarVisit.SelectedDate = DateTime.Now;
            }
            var list = logic.GetPickDate(new VisitBindingModel
            {
                Date = CalendarVisit.SelectedDate.Value
            });
            ComboBoxTime.ItemsSource = list;
            if(oldDate != DateTime.MinValue)
            {
               list.Add(oldDate);
               ComboBoxTime.SelectedItem = oldDate;
            }
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                if (visitsProcedures != null)
                {

                    DataGridProcedures.ItemsSource = visitsProcedures.ToList();
                    DataGridProcedures.Columns[0].Header = "Id";
                    DataGridProcedures.Columns[0].Width = 0;
                    DataGridProcedures.Columns[0].Visibility = Visibility.Hidden; 
                    DataGridProcedures.Columns[1].Header = "Название процедуры:";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowBindingProcedure>();
            window.ClientId = (int)clientId;
            if (window.ShowDialog().Value)
            {
                if (!visitsProcedures.ContainsKey(window.Id)) { 
                    visitsProcedures.Add(window.Id, window.ProcedureName);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridProcedures.SelectedCells.Count != 0)
            {
                var window = Container.Resolve<WindowBindingProcedure>();
                var conv = ((DataGridProcedures.SelectedItem as KeyValuePair<int, string>?));
                int id = 0;
                foreach (var p in visitsProcedures)
                {
                    if(p.Equals(conv))
                    {
                        id = p.Key;
                        break;
                    }
                }
                visitsProcedures.Remove(id);
                window.Id = id;
                window.ClientId = (int)clientId;
                if (window.ShowDialog().Value)
                {
                    if(!visitsProcedures.ContainsValue(window.ProcedureName))
                    {
                        visitsProcedures[window.Id] = (window.ProcedureName);
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
                        var conv = (DataGridProcedures.SelectedItem as KeyValuePair<int, string>?);
                        foreach (var p in visitsProcedures)
                        {
                            if (p.Equals(conv))
                            {
                                visitsProcedures.Remove(p.Key);
                                break;
                            }
                        }
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
            if (ComboBoxTime.SelectedItem == null)
            {
                MessageBox.Show("Заполните время", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }

            if (CalendarVisit.SelectedDate == null)
            {
                MessageBox.Show("Заполните дату", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            if (visitsProcedures == null || visitsProcedures.Count == 0)
            {
                MessageBox.Show("Заполните процедуры", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new VisitBindingModel
                {
                    Id = id,
                    Date = ((DateTime)ComboBoxTime.SelectedItem),
                    VisitProcedures = visitsProcedures,
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
        /// Логика обработки смены даты в календаре
        /// </summary>
        private void CalendarVisit_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
                if(!CalendarVisit.SelectedDate.HasValue)
                {
                return;
                }
                if(CalendarVisit.SelectedDate.Value < DateTime.Today)
                {
                    MessageBox.Show("Выберете корректную дату", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                CalendarVisit.SelectedDate = DateTime.Now;
                    return;
                }

                CalendarVisit.DisplayDate = CalendarVisit.SelectedDate.Value;
                var list = logic.GetPickDate(new VisitBindingModel
                {
                    Date = CalendarVisit.SelectedDate.Value
                });
                if (oldDate != DateTime.MinValue && oldDate.Day == CalendarVisit.SelectedDate.Value.Day)
                {
                    list.Add(oldDate);
                    ComboBoxTime.SelectedItem = oldDate;
                }
            ComboBoxTime.ItemsSource = list;
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
