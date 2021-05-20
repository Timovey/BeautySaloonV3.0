using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.HelperModels;
using BeautySaloonBusinessLogic.Interfaces;
using Microsoft.Reporting.WinForms;
using System;
using System.Windows;
using Unity;

namespace BeautySaloonViewEmployee
{
    /// <summary>
    /// Логика взаимодействия для WindowReportCosmetics.xaml
    /// </summary>
    public partial class WindowReportCosmetics : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly ReportLogicEmployee logic;

        private readonly IEmployeeStorage _employeeStorage;

        public int Id { set { id = value; } }

        private int? id;

        public WindowReportCosmetics(ReportLogicEmployee logic, IEmployeeStorage employeeStorage)
        {
            InitializeComponent();
            this.logic = logic;
            _employeeStorage = employeeStorage;
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            reportViewer.LocalReport.ReportPath = "../../ReportCosmetics.rdlc";
        }

        private void buttonMake_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerFrom.SelectedDate >= datePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                "c " + datePickerFrom.SelectedDate.Value.ToShortDateString() +
                " по " + datePickerTo.SelectedDate.Value.ToShortDateString());
                reportViewer.LocalReport.SetParameters(parameter);
                var dataSource = logic.GetCosmetics(new ReportBindingModelEmployee
                {
                    DateFrom = datePickerFrom.SelectedDate,
                    DateTo = datePickerTo.SelectedDate,
                    EmployeeId = id
                });
                ReportDataSource source = new ReportDataSource("DataSetCosmetics", dataSource);
                reportViewer.LocalReport.DataSources.Add(source);
                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonToPdf_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerFrom.SelectedDate >= datePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.SaveCosmeticsToPdfFile(new ReportBindingModelEmployee
                {
                    FileName = "D:\\Otchet.pdf",
                    DateFrom = datePickerFrom.SelectedDate,
                    DateTo = datePickerTo.SelectedDate,
                    EmployeeId = id
                });
                MailLogic.MailSendAsync(new MailSendInfo
                {
                    MailAddress = _employeeStorage.GetElement(new EmployeeBindingModel { Id = id })?.EMail,
                    Subject = $"Отчет",
                    Text = "Отчет по косметике за период c " + datePickerFrom.SelectedDate.Value.ToShortDateString() +
                    " по " + datePickerTo.SelectedDate.Value.ToShortDateString(),
                    File = "D:\\Otchet.pdf"
                });
                MessageBox.Show("Сообщение отправлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}