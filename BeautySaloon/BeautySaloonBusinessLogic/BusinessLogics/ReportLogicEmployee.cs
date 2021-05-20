using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.HelperModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BusinessLogics
{
    public class ReportLogicEmployee
    {
        private readonly IReportStorage _reportStorage;

        public ReportLogicEmployee(IReportStorage reportStorage)
        {
            _reportStorage = reportStorage;
        }
        /// <summary>
        /// Получение списка покупок по выбранной косметике
        /// </summary>
        /// <returns></returns>
        public List<ReportPurchaseCosmeticViewModel> GetPurchaseList(ReportBindingModelEmployee model)
        {
            var list = new List<ReportPurchaseCosmeticViewModel>();
            decimal totalCost = 0;

            foreach (var cosmetic in model.purchaseCosmetics)
            {
                list.AddRange(_reportStorage.GetPurchaseList(cosmetic));
            }

            foreach(var reportPurchaseCosmetic in list)
            {
                totalCost += reportPurchaseCosmetic.Price * reportPurchaseCosmetic.Count;
            }

            list[0].TotalCost = totalCost;
            return list;
        }
        /// <summary>
        /// Получение списка косметики за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportCosmeticsViewModel> GetCosmetics(ReportBindingModelEmployee model)
        {
            return _reportStorage.GetCosmetics(model);
        }
        /// <summary>
        /// Сохранение покупок в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public void SavePurchaseListToWordFile(ReportBindingModelEmployee model)
        {
            SaveToWordEmployee.CreateDoc(new WordInfoEmployee
            {
                FileName = model.FileName,
                Title = "Сведения по покупкам",
                Purchases = GetPurchaseList(model)
            });
        }
        /// <summary>
        /// Сохранение покупок в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SavePurchaseListToExcelFile(ReportBindingModelEmployee model)
        {
            SaveToExcelEmployee.CreateDoc(new ExcelInfoEmployee
            {
                FileName = model.FileName,
                Title = "Сведения по покупкам",
                Purchases = GetPurchaseList(model)
            });
        }
        /// <summary>
        /// Сохранение косметики в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        public void SaveCosmeticsToPdfFile(ReportBindingModelEmployee model)
        {
            SaveToPdfEmployee.CreateDoc(new PdfInfoEmployee
            {
                FileName = model.FileName,
                Title = "Список косметики",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                EmployeeId = model.EmployeeId.Value,
                Cosmetics = GetCosmetics(model)
            });
        }
    }
}