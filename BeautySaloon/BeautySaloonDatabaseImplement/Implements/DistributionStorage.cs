using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using BeautySaloonDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeautySaloonDatabaseImplement.Implements
{
    public class DistributionStorage : IDistributionStorage
    {
        public List<DistributionViewModel> GetFullList()
        {
            using (var context = new BeautySaloonDatabase())
            {
                return context.Distributions
                .Include(rec => rec.Employee)
                .Include(rec => rec.Visit)
                .Include(rec => rec.DistributionCosmetics)
                .ThenInclude(rec => rec.Cosmetic)
                .ToList()
                .Select(rec => new DistributionViewModel
                {
                    Id = rec.Id,
                    IssueDate = rec.IssueDate,
                    DistributionCosmetics = rec.DistributionCosmetics.ToDictionary(recDC => recDC.CosmeticId, recDC => (recDC.Cosmetic?.CosmeticName, recDC.Count)),
                    EmployeeId = rec.EmployeeId,
                    VisitId = rec.VisitId
                })
                .ToList();
            }
        }

        public List<DistributionViewModel> GetFilteredList(DistributionBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                return context.Distributions
                .Include(rec => rec.Employee)
                .Include(rec => rec.Visit)
                .Include(rec => rec.DistributionCosmetics)
                .ThenInclude(rec => rec.Cosmetic)
                .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.EmployeeId == model.EmployeeId) ||
                (model.DateFrom.HasValue && model.DateTo.HasValue && rec.EmployeeId == model.EmployeeId && rec.IssueDate.Date >= model.DateFrom.Value.Date && rec.IssueDate.Date <= model.DateTo.Value.Date))
                .ToList()
                .Select(rec => new DistributionViewModel
                {
                    Id = rec.Id,
                    IssueDate = rec.IssueDate,
                    DistributionCosmetics = rec.DistributionCosmetics.ToDictionary(recDC => recDC.CosmeticId, recDC => (recDC.Cosmetic?.CosmeticName, recDC.Count)),
                    EmployeeId = rec.EmployeeId,
                    VisitId = rec.VisitId
                })
                .ToList();
            }
        }

        public DistributionViewModel GetElement(DistributionBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new BeautySaloonDatabase())
            {
                Distribution distribution = context.Distributions
                .Include(rec => rec.Employee)
                .Include(rec => rec.Visit)
                .Include(rec => rec.DistributionCosmetics)
                .ThenInclude(rec => rec.Cosmetic)
                .FirstOrDefault(rec => rec.IssueDate == model.IssueDate || rec.Id == model.Id);
                return distribution != null ? new DistributionViewModel
                {
                    Id = distribution.Id,
                    IssueDate = distribution.IssueDate,
                    DistributionCosmetics = distribution.DistributionCosmetics.ToDictionary(recDC => recDC.CosmeticId, recDC => (recDC.Cosmetic?.CosmeticName, recDC.Count)),
                    EmployeeId = distribution.EmployeeId,
                    VisitId = distribution.VisitId
                } : null;
            }
        }

        public void Insert(DistributionBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Distribution(), context);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(DistributionBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Distribution element = context.Distributions.FirstOrDefault(rec => rec.Id == model.Id);

                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }

                        CreateModel(model, element, context);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(DistributionBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                Distribution element = context.Distributions.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Distributions.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Distribution CreateModel(DistributionBindingModel model, Distribution distribution, BeautySaloonDatabase context)
        {
            distribution.IssueDate = model.IssueDate;
            distribution.EmployeeId = (int)model.EmployeeId;
            distribution.VisitId = model.VisitId;

            if (distribution.Id == 0)
            {
                context.Distributions.Add(distribution);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var distributionCosmetics = context.DistributionCosmetics.Where(rec => rec.DistributionId == model.Id.Value).ToList();
                context.DistributionCosmetics.RemoveRange(distributionCosmetics.Where(rec => !model.DistributionCosmetics.ContainsKey(rec.CosmeticId)).ToList());
                context.SaveChanges();

                foreach (var updateCosmetic in distributionCosmetics)
                {
                    updateCosmetic.Count = model.DistributionCosmetics[updateCosmetic.CosmeticId].Item2;
                    model.DistributionCosmetics.Remove(updateCosmetic.CosmeticId);
                }
                context.SaveChanges();
            }
            foreach (var dc in model.DistributionCosmetics)
            {
                context.DistributionCosmetics.Add(new DistributionCosmetic
                {
                    DistributionId = distribution.Id,
                    CosmeticId = dc.Key,
                    Count = dc.Value.Item2
                });
                context.SaveChanges();
            }
            return distribution;
        }
    }
}