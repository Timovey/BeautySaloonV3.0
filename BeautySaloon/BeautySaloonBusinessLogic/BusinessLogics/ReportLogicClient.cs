using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.HelperModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BeautySaloonBusinessLogic.BusinessLogics
{
    public class ReportLogicClient
    {
        private readonly IClientStorage _clientStorage;
        private readonly IVisitStorage _visitStorage;
        private readonly IPurchaseStorage _purchaseStorage;
        private readonly IProcedureStorage _procedureStorage;
        private readonly IDistributionStorage _distributionStorage;

        public ReportLogicClient(IClientStorage clientStorage, IVisitStorage visitStorage,
            IPurchaseStorage purchaseStorage, IProcedureStorage procedureStorage, IDistributionStorage distributionStorage)
        {
            _clientStorage = clientStorage;
            _visitStorage = visitStorage;
            _purchaseStorage = purchaseStorage;
            _procedureStorage = procedureStorage;
            _distributionStorage = distributionStorage;
        }
        /// <summary>
        /// Получение списка процедур за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportProceduresViewModel> GetProcedures(ReportBindingModelClient model)
        {
            var listAll = new List<ReportProceduresViewModel>();
            var client = _clientStorage.GetElement(new ClientBindingModel { Id = model.ClientId });


            var listPurchases = _purchaseStorage.GetFilteredList(new PurchaseBindingModel { ClientId = model.ClientId, DateFrom = model.DateFrom, DateTo = model.DateTo });
            foreach (var purchase in listPurchases)
            {
                foreach (var pp in purchase.PurchaseProcedures)
                {
                    listAll.Add(new ReportProceduresViewModel
                    {
                        TypeOfService = "Покупка",
                        DateOfService = purchase.Date,
                        ProcedureName = pp.Value.Item1,
                        Price = pp.Value.Item2,
                    });
                }
            }
            var listVisits = _visitStorage.GetFilteredList(new VisitBindingModel { ClientId = model.ClientId, DateFrom = model.DateFrom, DateTo = model.DateTo });
            foreach (var visit in listVisits)
            {
                foreach (var vp in visit.VisitProcedures)
                {
                    listAll.Add(new ReportProceduresViewModel
                    {
                        TypeOfService = "Посещение",
                        DateOfService = visit.Date,
                        ProcedureName = vp.Value,
                    });
                }
            }
            return listAll;
        }

        /// <summary>
        /// Получение списка выдач по выбранным процедурам
        /// </summary>
        /// <returns></returns>
        public List<ReportDistributionProcedureViewModel> GetDistributionList(ReportDistributionProcedureBindingModel model)
        {
            var listVisits = _visitStorage.GetFullList();
            var list = new List<ReportDistributionProcedureViewModel>();

            foreach (var visit in listVisits)
            {
                foreach (var vp in visit.VisitProcedures)
                {
                    if (vp.Value == model.ProcedureName)
                    {
                        var listDistribution = _distributionStorage.GetFilteredList(new DistributionBindingModel { VisitId = visit.Id });
                        if (listDistribution.Count > 0 && listDistribution != null)
                        {
                            foreach (var distribution in listDistribution)
                            {
                                list.Add(new ReportDistributionProcedureViewModel
                                {
                                    ProcedureName = vp.Value,
                                    Date = distribution.IssueDate,
                                    EmployeeId = distribution.EmployeeId
                                });

                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Сохранение покупок в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public void SavePurchaseListToWordFile(ReportBindingModelClient model, string name)
        {
            //SaveToWordClient.CreateDoc(new WordInfoClient
            //{
            //    FileName = model.FileName,
            //    Title = "Сведения по выдачам",
            //    Distributions = GetDistributionList(new ReportDistributionProcedureBindingModel { ProcedureName = name })
            //});
        }
        /// <summary>
        /// Сохранение покупок в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SavePurchaseListToExcelFile(ReportBindingModelClient model, string name)
        {
            //SaveToExcelClient.CreateDoc(new ExcelInfoClient
            //{
            //    FileName = model.FileName,
            //    Title = "Сведения по выдачам",
            //    Distributions = GetDistributionList(new ReportDistributionProcedureBindingModel { ProcedureName = name })
            //});
        }
        /// <summary>
        /// Сохранение процедур в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        public void SaveProceduresToPdfFile(ReportBindingModelClient model)
        {
            SaveToPdfClient.CreateDoc(new PdfInfoClient
            {
                FileName = model.FileName,
                Title = "Список процедур",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Procedures = GetProcedures(model)
            });
        }
    }
}