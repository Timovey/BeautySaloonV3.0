using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using BeautySaloonDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeautySaloonDatabaseImplement.Implements
{
    public class ProcedureStorage : IProcedureStorage
    {
        public List<ProcedureViewModel> GetFullList()
        {
            using (var context = new BeautySaloonDatabase())
            {
                return context.Procedures
                .Select(CreateModel)
                .ToList();
            }
        }

        public List<ProcedureViewModel> GetFilteredList(ProcedureBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                return context.Procedures
                .Where(rec => rec.ClientId == model.ClientId || rec.ProcedureName.Contains(model.ProcedureName))
                .Select(CreateModel)
                .ToList();
            }
        }

        public ProcedureViewModel GetElement(ProcedureBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                var procedure = context.Procedures
                .FirstOrDefault(rec => rec.ProcedureName == model.ProcedureName || rec.Id == model.Id);
                return procedure != null ?
                CreateModel(procedure) :
               null;
            }
        }

        public void Insert(ProcedureBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                context.Procedures.Add(CreateModel(model, new Procedure()));
                context.SaveChanges();
            }
        }

        public void Update(ProcedureBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                var element = context.Procedures.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }

        public void Delete(ProcedureBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                Procedure element = context.Procedures.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Procedures.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private ProcedureViewModel CreateModel(Procedure procedure)
        {
            return new ProcedureViewModel
            {
                Id = procedure.Id,
                ProcedureName = procedure.ProcedureName,
                Duration = procedure.Duration,
                Price = procedure.Price,
                ClientId = procedure.ClientId
            };
        }

        private Procedure CreateModel(ProcedureBindingModel model, Procedure procedure)
        {
            procedure.ProcedureName = model.ProcedureName;
            procedure.Price = model.Price;
            procedure.Duration = model.Duration;
            procedure.ClientId = model.ClientId.Value;
            return procedure;
        }
    }
}