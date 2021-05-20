using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using Microsoft.Reporting.WinForms;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows;
using Unity;


namespace BeautySaloonViewClient
{
    /// <summary>
    /// Логика взаимодействия для WindowReportProcedures.xaml
    /// </summary>
    public partial class WindowReportProcedures : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int ClientId { set { clientId = value; } }

        private int? clientId;

        private readonly ReportLogicClient logic;

        private readonly ClientLogic logicClient;

        public WindowReportProcedures(ReportLogicClient logic, ClientLogic clientLogic)
        {
            InitializeComponent();
            this.logic = logic;
            logicClient = clientLogic;
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            reportViewer.LocalReport.ReportPath = "../../ReportProcedures.rdlc";
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
                ReportParameter parameterPeriod = new ReportParameter("ReportParameterPeriod",
                "c " + datePickerFrom.SelectedDate.Value.ToShortDateString() +
                " по " + datePickerTo.SelectedDate.Value.ToShortDateString());
                reportViewer.LocalReport.SetParameters(parameterPeriod);

                var client = logicClient.Read(new ClientBindingModel
                {
                    Id = (int)clientId
                });
                ReportParameter parameterClient = new ReportParameter("ReportParameterClient",
                client[0].ClientSurame + " " + client[0].ClientName);
                reportViewer.LocalReport.SetParameters(parameterClient);

                var dataSource = logic.GetProcedures(new ReportBindingModelClient
                {
                    DateFrom = datePickerFrom.SelectedDate,
                    DateTo = datePickerTo.SelectedDate,
                    ClientId = clientId
                });
                ReportDataSource source = new ReportDataSource("DataSetProcedures", dataSource);
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
            if (string.IsNullOrEmpty(TextBoxEmail.Text))
            {
                MessageBox.Show("Заполните адрес электронной почты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MailMessage msg = new MailMessage();
            SmtpClient client = new SmtpClient();
            try
            {
                msg.Subject = "Отчет по косметике";
                msg.Body = "Отчет по косметике за период c " + datePickerFrom.SelectedDate.Value.ToShortDateString() +
                " по " + datePickerTo.SelectedDate.Value.ToShortDateString();
                msg.From = new MailAddress("ksenia.pochta30052001@gmail.com");
                msg.To.Add(TextBoxEmail.Text);
                msg.IsBodyHtml = true;
                logic.SaveProceduresToPdfFile(new ReportBindingModelClient
                {
                    FileName = "D:\\Otchet.pdf",
                    DateFrom = datePickerFrom.SelectedDate,
                    DateTo = datePickerTo.SelectedDate
                });
                string file = "D:\\Otchet.pdf";
                Attachment attach = new Attachment(file, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attach.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                msg.Attachments.Add(attach);
                client.Host = "smtp.gmail.com";
                NetworkCredential basicauthenticationinfo = new NetworkCredential("pochta30052001@gmail.com", "пароль");
                client.Port = int.Parse("587");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicauthenticationinfo;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);
                MessageBox.Show("Сообщение отправлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBoxEmail_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBoxEmail.Clear();
        }
    }
}
