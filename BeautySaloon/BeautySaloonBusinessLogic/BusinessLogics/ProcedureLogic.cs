using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BusinessLogics
{
    public class ProcedureLogic
    {
        private readonly IProcedureStorage _procedureStorage;

        public ProcedureLogic(IProcedureStorage procedureStorage)
        {
            _procedureStorage = procedureStorage;
        }

        public List<ProcedureViewModel> Read(ProcedureBindingModel model)
        {
            if (model == null)
            {
                return _procedureStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ProcedureViewModel> { _procedureStorage.GetElement(model) };
            }
            return _procedureStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(ProcedureBindingModel model)
        {
            var element = _procedureStorage.GetElement(new ProcedureBindingModel
            {
                ProcedureName = model.ProcedureName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть процедура с таким названием");
            }
            if (model.Id.HasValue)
            {
                _procedureStorage.Update(model);
            }
            else
            {
                _procedureStorage.Insert(model);
            }
        }

        public void Delete(ProcedureBindingModel model)
        {
            var element = _procedureStorage.GetElement(new ProcedureBindingModel
            {
                Id =
           model.Id
            });
            if (element == null)
            {
                throw new Exception("Процедура не найдена");
            }
            _procedureStorage.Delete(model);
        }
    }
}