using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.Interfaces
{
    public interface IReportStorage
    {
        List<ReportPurchaseCosmeticViewModel> GetPurchaseList(int cosmeticId);

        List<ReportCosmeticsViewModel> GetCosmetics(ReportBindingModelEmployee model);
    }
}