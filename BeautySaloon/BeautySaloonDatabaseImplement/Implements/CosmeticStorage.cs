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
    public class CosmeticStorage : ICosmeticStorage
    {
        public List<CosmeticViewModel> GetFullList()
        {
            using (var context = new BeautySaloonDatabase())
            {
                return context.Cosmetics
                .Include(rec => rec.Employee)
                .Select(rec => new CosmeticViewModel
                {
                    Id = rec.Id,
                    CosmeticName = rec.CosmeticName,
                    Price = rec.Price,
                    EmployeeId = rec.EmployeeId
                })
                .ToList();
            }
        }

        public List<CosmeticViewModel> GetFilteredList(CosmeticBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                return context.Cosmetics
                .Include(rec => rec.Employee)
                .Where(rec => rec.EmployeeId == model.EmployeeId || rec.CosmeticName.Contains(model.CosmeticName))
                .Select(rec => new CosmeticViewModel
                {
                    Id = rec.Id,
                    CosmeticName = rec.CosmeticName,
                    Price = rec.Price,
                    EmployeeId = rec.EmployeeId
                })
                .ToList();
            }
        }

        public CosmeticViewModel GetElement(CosmeticBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                var tmp = model.EmployeeId;
                var cosmetic = context.Cosmetics
                .Include(rec => rec.Employee)
                .FirstOrDefault(rec => rec.CosmeticName == model.CosmeticName || rec.Id == model.Id);
                return cosmetic != null ?
                new CosmeticViewModel
                {
                    Id = cosmetic.Id,
                    CosmeticName = cosmetic.CosmeticName,
                    Price = cosmetic.Price,
                    EmployeeId = cosmetic.EmployeeId
                } :
                null;
            }
        }

        public void Insert(CosmeticBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                context.Cosmetics.Add(CreateModel(model, new Cosmetic()));
                context.SaveChanges();
            }
        }

        public void Update(CosmeticBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                var element = context.Cosmetics.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }

        public void Delete(CosmeticBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                Cosmetic element = context.Cosmetics.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Cosmetics.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Cosmetic CreateModel(CosmeticBindingModel model, Cosmetic cosmetic)
        {
            cosmetic.CosmeticName = model.CosmeticName;
            cosmetic.Price = model.Price;
            cosmetic.EmployeeId = (int)model.EmployeeId;
            return cosmetic;
        }
    }
}