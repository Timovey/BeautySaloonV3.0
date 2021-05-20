using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Enums;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BusinessLogics
{
    public class VisitLogic
    {
        private readonly IVisitStorage _visitStorage;

        public VisitLogic(IVisitStorage visitStorage)
        {
            _visitStorage = visitStorage;
        }

        public List<VisitViewModel> Read(VisitBindingModel model)
        {
            if (model == null)
            {
                return _visitStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<VisitViewModel> { _visitStorage.GetElement(model) };
            }
            return _visitStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(VisitBindingModel model)
        {
            var element = _visitStorage.GetElement(new VisitBindingModel
            {
                Date = model.Date
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть посещение на это время");
            }
            if (model.Id.HasValue)
            {
                _visitStorage.Update(model);
            }
            else
            {
                _visitStorage.Insert(model);
            }
        }

        public void Delete(VisitBindingModel model)
        {
            var element = _visitStorage.GetElement(new VisitBindingModel
            {
                Id =
           model.Id
            });
            if (element == null)
            {
                throw new Exception("Посещение не найдено");
            }
            _visitStorage.Delete(model);
        }

        public List<DateTime> GetPickDate(VisitBindingModel model)
        {

            var list = _visitStorage.GetFilteredList(model);

            List<DateTime> dateOfDay = new List<DateTime>();
            bool end = false;
            TimeVisit time = TimeVisit.ten;
            while (!end) // создали список дат на этот день (потом их будем удалять если они совпадают с эл-ми list)
            {
                DateTime date = new DateTime(model.Date.Year, model.Date.Month, model.Date.Day, Convert.ToInt32(time), 0, 0);
                dateOfDay.Add(date);
                time++;
                if (time > TimeVisit.fourteen)
                {
                    end = true;
                }
            }

            foreach (var item in list)
            {
                DateTime date = new DateTime(model.Date.Year, model.Date.Month, model.Date.Day, item.Date.Hour, 0, 0);
                if (dateOfDay.Contains(date))
                {
                    dateOfDay.Remove(date);
                }
            }
            return dateOfDay;
        }
    }
}