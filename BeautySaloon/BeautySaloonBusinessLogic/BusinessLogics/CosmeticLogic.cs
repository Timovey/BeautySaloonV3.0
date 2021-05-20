using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BusinessLogics
{
    public class CosmeticLogic
    {
        private readonly ICosmeticStorage _cosmeticStorage;

        public CosmeticLogic(ICosmeticStorage cosmeticStorage)
        {
            _cosmeticStorage = cosmeticStorage;
        }

        public List<CosmeticViewModel> Read(CosmeticBindingModel model)
        {
            if (model == null)
            {
                return _cosmeticStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<CosmeticViewModel> { _cosmeticStorage.GetElement(model) };
            }
            return _cosmeticStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(CosmeticBindingModel model)
        {
            var element = _cosmeticStorage.GetElement(new CosmeticBindingModel { CosmeticName = model.CosmeticName });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть косметика с таким названием");
            }
            if (model.Id.HasValue)
            {
                _cosmeticStorage.Update(model);
            }
            else
            {
                _cosmeticStorage.Insert(model);
            }
        }

        public void Delete(CosmeticBindingModel model)
        {
            var element = _cosmeticStorage.GetElement(new CosmeticBindingModel { Id = model.Id });
            if (element == null)
            {
                throw new Exception("Косметика не найдена");
            }
            _cosmeticStorage.Delete(model);
        }
    }
}