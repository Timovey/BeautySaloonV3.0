using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using Unity;

namespace BeautySaloonViewEmployee
{
    /// <summary>
    /// Логика взаимодействия для WindowStatistic.xaml
    /// </summary>
    public partial class WindowStatistic : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly StatisticLogicEmployee logic;

        public int Id { set { id = value; } }

        private int? id;

        private EmployeeLogic employeeLogic;

        public WindowStatistic(StatisticLogicEmployee logic, EmployeeLogic logicE)
        {
            InitializeComponent();
            this.logic = logic;
            employeeLogic = logicE;
        }

        private void LoadData()
        {
            ((ColumnSeries)mcChart.Series[0]).ItemsSource = logic.GetDistributionStatistic(new ReportBindingModelEmployee
            {
                DateFrom = datePickerFrom.SelectedDate,
                DateTo = datePickerTo.SelectedDate,
                EmployeeId = id
            });
            ((ColumnSeries)mcChart.Series[1]).ItemsSource = logic.GetReceiptStatistic(new ReportBindingModelEmployee
            {
                DateFrom = datePickerFrom.SelectedDate,
                DateTo = datePickerTo.SelectedDate,
                EmployeeId = id
            });
            ((ColumnSeries)mcChartAll.Series[0]).ItemsSource = logic.GetDistributionStatistic(new ReportBindingModelEmployee
            {
                DateFrom = datePickerFrom.SelectedDate,
                DateTo = datePickerTo.SelectedDate,
                EmployeeId = 0
            });
            ((ColumnSeries)mcChartAll.Series[1]).ItemsSource = logic.GetReceiptStatistic(new ReportBindingModelEmployee
            {
                DateFrom = datePickerFrom.SelectedDate,
                DateTo = datePickerTo.SelectedDate,
                EmployeeId = 0
            });
        }

        private void buttonMake_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void WindowStatistic_Loaded(object sender, RoutedEventArgs e)
        {
            var employee = employeeLogic.Read(new EmployeeBindingModel { Id = id })?[0];
            lbl_Employee.Content = "Косметика сотрудника: " + employee.F_Name + " " + employee.L_Name;
        }
    }
}