using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BeautySaloonDatabaseImplement.Implements
{
    public class StatisticStorage : IStatisticStorage
    {
        public List<ReportCosmeticsViewModel> GetDistributions(ReportBindingModelEmployee model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                return (
                from c in context.Cosmetics
                join dc in context.DistributionCosmetics on c.Id equals dc.CosmeticId
                join d in context.Distributions on dc.DistributionId equals d.Id
                where d.EmployeeId == model.EmployeeId
                where d.IssueDate >= model.DateFrom
                where d.IssueDate <= model.DateTo
                select new ReportCosmeticsViewModel
                {
                    TypeOfService = "Выдача",
                    DateOfService = d.IssueDate,
                    CosmeticName = c.CosmeticName,
                    Count = dc.Count
                })
                .ToList();
            }
        }

        public List<ReportCosmeticsViewModel> GetDistributionsForAll(ReportBindingModelEmployee model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                return (
                from c in context.Cosmetics
                join dc in context.DistributionCosmetics on c.Id equals dc.CosmeticId
                join d in context.Distributions on dc.DistributionId equals d.Id
                where d.IssueDate >= model.DateFrom
                where d.IssueDate <= model.DateTo
                select new ReportCosmeticsViewModel
                {
                    TypeOfService = "Выдача",
                    DateOfService = d.IssueDate,
                    CosmeticName = c.CosmeticName,
                    Count = dc.Count
                })
                .ToList();
            }
        }

        public List<ReportCosmeticsViewModel> GetReceipts(ReportBindingModelEmployee model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                return (
                from c in context.Cosmetics
                join rc in context.ReceiptCosmetics on c.Id equals rc.CosmeticId
                join r in context.Receipts on rc.ReceiptId equals r.Id
                where r.EmployeeId == model.EmployeeId
                where r.PurchaseDate >= model.DateFrom
                where r.PurchaseDate <= model.DateTo
                select new ReportCosmeticsViewModel
                {
                    TypeOfService = "Чек",
                    DateOfService = r.PurchaseDate,
                    CosmeticName = c.CosmeticName,
                    Count = rc.Count
                })
                .ToList();
            }
        }

        public List<ReportCosmeticsViewModel> GetReceiptsForAll(ReportBindingModelEmployee model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                return (
                from c in context.Cosmetics
                join rc in context.ReceiptCosmetics on c.Id equals rc.CosmeticId
                join r in context.Receipts on rc.ReceiptId equals r.Id
                where r.PurchaseDate >= model.DateFrom
                where r.PurchaseDate <= model.DateTo
                select new ReportCosmeticsViewModel
                {
                    TypeOfService = "Чек",
                    DateOfService = r.PurchaseDate,
                    CosmeticName = c.CosmeticName,
                    Count = rc.Count
                })
                .ToList();
            }
        }
    }
}