using Unity.Lifetime;
using System.Windows;
using BeautySaloonBusinessLogic.BusinessLogics;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonDatabaseImplement.Implements;
using System;
using Unity;

namespace BeautySaloonViewClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer currentContainer = BuildUnityContainer();

            var mainWindow = currentContainer.Resolve<WindowInitial>();
            mainWindow.Show();
        }


        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IVisitStorage, VisitStorage>(new 
                HierarchicalLifetimeManager());
            currentContainer.RegisterType<IClientStorage, ClientStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IProcedureStorage, ProcedureStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPurchaseStorage, PurchaseStorage>(new
HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReceiptStorage, ReceiptStorage>(new
HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDistributionStorage, DistributionStorage>(new
HierarchicalLifetimeManager());
            currentContainer.RegisterType<VisitLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<ReportLogicClient>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ClientLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ProcedureLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<PurchaseLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<ReceiptLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<DistributionLogic>(new
           HierarchicalLifetimeManager());
            return currentContainer;
        }

    }

}
