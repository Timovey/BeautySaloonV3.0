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
    public class PurchaseStorage : IPurchaseStorage
    {
        public List<PurchaseViewModel> GetFullList()
        {
            using (var context = new BeautySaloonDatabase())
            {
                return context.Purchases
                .Include(rec => rec.Client)
                .Include(rec => rec.Receipt)
                .Include(rec => rec.ProcedurePurchase)
                .ThenInclude(rec => rec.Procedure)
                .ToList()
                .Select(rec => new PurchaseViewModel
                {
                    Id = rec.Id,
                    ClientId = rec.ClientId,
                    Date = rec.Date,
                    Price = rec.Price,
                    PurchaseProcedures = rec.ProcedurePurchase.ToDictionary(recPP => recPP.ProcedureId, recPP => (recPP.Procedure?.ProcedureName, recPP.Procedure.Price)),
                    ReceiptId = rec.ReceiptId
                })
               .ToList();

            }
        }

        public List<PurchaseViewModel> GetFilteredList(PurchaseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                return context.Purchases
                .Include(rec => rec.Client)
                .Include(rec => rec.Receipt)
                .Include(rec => rec.ProcedurePurchase)
                .ThenInclude(rec => rec.Procedure)
                .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.ClientId == model.ClientId || rec.Date == model.Date || rec.ReceiptId == model.ReceiptId) ||
                (model.DateFrom.HasValue && model.DateTo.HasValue && (rec.ClientId == model.ClientId
                && rec.Date.Date >= model.DateFrom.Value.Date && rec.Date.Date <= model.DateTo.Value.Date)))
                .ToList()
                .Select(rec => new PurchaseViewModel
                {
                    Id = rec.Id,
                    ClientId = rec.ClientId,
                    Date = rec.Date,
                    Price = rec.Price,
                    PurchaseProcedures = rec.ProcedurePurchase.ToDictionary(recPP => recPP.ProcedureId, recPP => (recPP.Procedure?.ProcedureName, recPP.Procedure.Price)),
                    ReceiptId = rec.ReceiptId

                }).ToList();
            }
        }
       
        public PurchaseViewModel GetElement(PurchaseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                var visit = context.Purchases
                .Include(rec => rec.Client)
                .Include(rec => rec.Receipt)
                .Include(rec => rec.ProcedurePurchase)
                .ThenInclude(rec => rec.Procedure)
                .FirstOrDefault(rec => rec.Date == model.Date || rec.Id == model.Id);
                return visit != null ?
                 new PurchaseViewModel
                 {
                     Id = visit.Id,
                     ClientId = visit.ClientId,
                     Date = visit.Date,
                     Price = visit.Price,
                     PurchaseProcedures = visit.ProcedurePurchase.ToDictionary(recPP => recPP.ProcedureId, recPP => (recPP.Procedure?.ProcedureName, recPP.Procedure.Price)),
                     ReceiptId = visit.ReceiptId
                 } :
               null;
            }
        }

        public void Insert(PurchaseBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Purchase(), context);
                        context.SaveChanges();
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

        public void Update(PurchaseBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Purchases.FirstOrDefault(rec => rec.Id ==
                       model.Id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        CreateModel(model, element, context);
                        context.SaveChanges();
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

        public void Delete(PurchaseBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                Purchase element = context.Purchases.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Purchases.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Purchase CreateModel(PurchaseBindingModel model, Purchase purchase, BeautySaloonDatabase context)
        {
            purchase.Date = model.Date;
            purchase.ClientId = (int)model.ClientId;
            purchase.Price = model.Price;
            if (model.ReceiptId != null)
            {
                purchase.ReceiptId = model.ReceiptId;
            }
            if (purchase.Id == 0)
            {
                context.Purchases.Add(purchase);
                context.SaveChanges();
            }
            if (model.Id.HasValue)
            {
                var PurchaseComponents = context.ProcedurePurchases.Where(rec => rec.PurchaseId == model.Id.Value).ToList();
                context.ProcedurePurchases.RemoveRange(PurchaseComponents.Where(rec =>
                !model.PurchaseProcedures.ContainsKey(rec.ProcedureId)).ToList());

                foreach (var updateProcedure in PurchaseComponents)
                {

                    model.PurchaseProcedures.Remove(updateProcedure.ProcedureId);
                }

                context.SaveChanges();

            }
            // добавили новые
            foreach (var pp in model.PurchaseProcedures)
            {
                context.ProcedurePurchases.Add(new ProcedurePurchase
                {
                    PurchaseId = purchase.Id,
                    ProcedureId = pp.Key,

                });
                context.SaveChanges();
            }
            return purchase;
        }
    }
}