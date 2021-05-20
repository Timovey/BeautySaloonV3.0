using System.Windows;
using Unity;

namespace BeautySaloonViewClient
{
    /// <summary>
    /// Логика взаимодействия для WindowInitial.xaml
    /// </summary>
    public partial class WindowInitial : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public WindowInitial()
        {
            InitializeComponent();
        }

        private void buttonAuthorization_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowAutorization>();
            window.ShowDialog();
        }

        private void buttonRegistration_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowRegistration>();
            window.ShowDialog();
        }
    }
}
