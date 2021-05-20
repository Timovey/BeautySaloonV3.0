using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeautySaloonBusinessLogic.BusinessLogics
{
    public class StatisticLogicEmployee
    {
        private readonly IStatisticStorage _statisticStorage;

        public StatisticLogicEmployee(IStatisticStorage statisticStorage)
        {
            _statisticStorage = statisticStorage;
        }

        public List<Tuple<string, int>> GetReceiptStatistic(ReportBindingModelEmployee model)
        {
            var list = new List<ReportCosmeticsViewModel>();
            if (model.EmployeeId != 0)
            {
                list = _statisticStorage.GetReceipts(model);
            }
            else
            {
                list = _statisticStorage.GetReceiptsForAll(model);
            }
            return list.OrderBy(rec => rec.CosmeticName).GroupBy(rec => new { rec.CosmeticName, rec.Count }).Select(rec => new Tuple<string, int>(rec.Key.CosmeticName, rec.Key.Count)).ToList();
        }

        public List<Tuple<string, int>> GetDistributionStatistic(ReportBindingModelEmployee model)
        {
            var list = new List<ReportCosmeticsViewModel>();
            if (model.EmployeeId != 0)
            {
                list = _statisticStorage.GetDistributions(model);
            }
            else
            {
                list = _statisticStorage.GetDistributionsForAll(model);
            }
            return list.OrderBy(rec => rec.CosmeticName).GroupBy(rec => new { rec.CosmeticName, rec.Count }).Select(rec => new Tuple<string, int>(rec.Key.CosmeticName, rec.Key.Count)).ToList();
        }
    }
}