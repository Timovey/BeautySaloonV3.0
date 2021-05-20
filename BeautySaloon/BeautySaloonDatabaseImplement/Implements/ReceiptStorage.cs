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
    public class ReceiptStorage : IReceiptStorage
    {
        public List<ReceiptViewModel> GetFullList()
        {
            using (var context = new BeautySaloonDatabase())
            {
                return context.Receipts
                .Include(rec => rec.Employee)
                .Include(rec => rec.ReceiptCosmetics)
                .ThenInclude(rec => rec.Cosmetic)
                .ToList()
                .Select(rec => new ReceiptViewModel
                {
                    Id = rec.Id,
                    TotalCost = rec.TotalCost,
                    PurchaseDate = rec.PurchaseDate,
                    ReceiptCosmetics = rec.ReceiptCosmetics.ToDictionary(recRC => recRC.CosmeticId, recRC => (recRC.Cosmetic?.CosmeticName, recRC.Count)),
                    EmployeeId = rec.EmployeeId
                })
                .ToList();
            }
        }

        public List<ReceiptViewModel> GetFilteredList(ReceiptBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                return context.Receipts
                .Include(rec => rec.Employee)
                .Include(rec => rec.ReceiptCosmetics)
                .ThenInclude(rec => rec.Cosmetic)
                .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.EmployeeId == model.EmployeeId) ||
                (model.DateFrom.HasValue && model.DateTo.HasValue && rec.EmployeeId == model.EmployeeId && rec.PurchaseDate.Date >= model.DateFrom.Value.Date && rec.PurchaseDate.Date <= model.DateTo.Value.Date))
                .ToList()
                .Select(rec => new ReceiptViewModel
                {
                    Id = rec.Id,
                    TotalCost = rec.TotalCost,
                    PurchaseDate = rec.PurchaseDate,
                    ReceiptCosmetics = rec.ReceiptCosmetics.ToDictionary(recRC => recRC.CosmeticId, recRC => (recRC.Cosmetic?.CosmeticName, recRC.Count)),
                    EmployeeId = rec.EmployeeId
                })
                .ToList();
            }
        }

        public ReceiptViewModel GetElement(ReceiptBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new BeautySaloonDatabase())
            {
                Receipt receipt = context.Receipts
                .Include(rec => rec.Employee)
                .Include(rec => rec.ReceiptCosmetics)
                .ThenInclude(rec => rec.Cosmetic)
                .FirstOrDefault(rec => rec.PurchaseDate == model.PurchaseDate || rec.Id == model.Id);
                return receipt != null ? new ReceiptViewModel
                {
                    Id = receipt.Id,
                    TotalCost = receipt.TotalCost,
                    PurchaseDate = receipt.PurchaseDate,
                    ReceiptCosmetics = receipt.ReceiptCosmetics.ToDictionary(recRC => recRC.CosmeticId, recRC => (recRC.Cosmetic?.CosmeticName, recRC.Count)),
                    EmployeeId = receipt.EmployeeId
                } : null;
            }
        }

        public void Insert(ReceiptBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Receipt(), context);
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

        public void Update(ReceiptBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Receipt element = context.Receipts.FirstOrDefault(rec => rec.Id == model.Id);

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

        public void Delete(ReceiptBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                Receipt element = context.Receipts.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Receipts.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Receipt CreateModel(ReceiptBindingModel model, Receipt receipt, BeautySaloonDatabase context)
        {
            receipt.TotalCost = model.TotalCost;
            receipt.PurchaseDate = model.PurchaseDate;
            receipt.EmployeeId = (int)model.EmployeeId;

            if (receipt.Id == 0)
            {
                context.Receipts.Add(receipt);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var receiptCosmetics = context.ReceiptCosmetics.Where(rec => rec.ReceiptId == model.Id.Value).ToList();
                context.ReceiptCosmetics.RemoveRange(receiptCosmetics.Where(rec => !model.ReceiptCosmetics.ContainsKey(rec.CosmeticId)).ToList());
                context.SaveChanges();

                foreach (var updateCosmetic in receiptCosmetics)
                {
                    updateCosmetic.Count = model.ReceiptCosmetics[updateCosmetic.CosmeticId].Item2;
                    model.ReceiptCosmetics.Remove(updateCosmetic.CosmeticId);
                }
                context.SaveChanges();
            }
            foreach (var rc in model.ReceiptCosmetics)
            {
                context.ReceiptCosmetics.Add(new ReceiptCosmetic
                {
                    ReceiptId = receipt.Id,
                    CosmeticId = rc.Key,
                    Count = rc.Value.Item2
                });
                context.SaveChanges();
            }
            return receipt;
        }
    }
}