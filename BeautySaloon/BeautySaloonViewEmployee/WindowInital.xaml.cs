using System.Windows;
using Unity;

namespace BeautySaloonViewEmployee
{
    /// <summary>
    /// Логика взаимодействия для WindowInital.xaml
    /// </summary>
    public partial class WindowInital : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public WindowInital()
        {
            InitializeComponent();
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowRegistration>();
            window.ShowDialog();
        }

        private void buttonAuthorization_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<WindowAuthorization>();
            window.ShowDialog();
        }
    }
}