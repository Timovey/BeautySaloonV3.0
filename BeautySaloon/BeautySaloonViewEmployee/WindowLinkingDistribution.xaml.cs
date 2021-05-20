using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Windows;
using Unity;

namespace BeautySaloonViewEmployee
{
    /// <summary>
    /// Логика взаимодействия для WindowLinkingDistribution.xaml
    /// </summary>
    public partial class WindowLinkingDistribution : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int VisitId { get { return (int)(ComboBoxVisit.SelectedItem as VisitViewModel).Id; } }

        public int DistributionId { get { return (ComboBoxDistribution.SelectedItem as DistributionViewModel).Id; } set { distributionId = value; } }

        public int EmployeeId { set { employeeId = value; } }

        private int? distributionId;

        private int? employeeId;

        private readonly VisitLogic logicVisit;

        private readonly DistributionLogic logicDistribution;

        public WindowLinkingDistribution(VisitLogic logicVisit, DistributionLogic logicDistribution)
        {
            InitializeComponent();
            this.logicDistribution = logicDistribution;
            this.logicVisit = logicVisit;
        }

        private void WindowLinkingDistribution_Loaded(object sender, RoutedEventArgs e)
        {
            var listVisit = logicVisit.Read(null);
            if (listVisit != null)
            {
                ComboBoxVisit.ItemsSource = listVisit;
            }
            var listDistribution = logicDistribution.Read(new DistributionBindingModel { EmployeeId = employeeId });
            if (listDistribution != null)
            {
                ComboBoxDistribution.ItemsSource = listDistribution;
            }
            if (distributionId != null)
            {
                ComboBoxDistribution.SelectedItem = SetValueDistribution(distributionId.Value);
            }
        }

        private void buttonLinking_Click(object sender, RoutedEventArgs e)
        {

            if (ComboBoxVisit.SelectedValue == null)
            {
                MessageBox.Show("Выберите посещение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ComboBoxDistribution.SelectedValue == null)
            {
                MessageBox.Show("Выберите выдачу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicDistribution.Linking(new DistributionLinkingBindingModel
                {
                    VisitId = (int)(ComboBoxVisit.SelectedItem as VisitViewModel).Id,
                    DistributionId = (ComboBoxDistribution.SelectedItem as DistributionViewModel).Id
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

        private DistributionViewModel SetValueDistribution(int value)
        {
            foreach (var item in ComboBoxDistribution.Items)
            {
                if ((item as DistributionViewModel).Id == value)
                {
                    return item as DistributionViewModel;
                }
            }
            return null;
        }
    }
}