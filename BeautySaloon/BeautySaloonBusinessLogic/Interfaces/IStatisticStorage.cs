using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.Interfaces
{
    public interface IStatisticStorage
    {
        List<ReportCosmeticsViewModel> GetReceipts(ReportBindingModelEmployee model);

        List<ReportCosmeticsViewModel> GetDistributions(ReportBindingModelEmployee model);

        List<ReportCosmeticsViewModel> GetReceiptsForAll(ReportBindingModelEmployee model);

        List<ReportCosmeticsViewModel> GetDistributionsForAll(ReportBindingModelEmployee model);
    }
}