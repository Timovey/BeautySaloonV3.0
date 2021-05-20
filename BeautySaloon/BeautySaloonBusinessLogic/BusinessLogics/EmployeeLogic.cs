using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BusinessLogics
{
    public class EmployeeLogic
    {
        private readonly IEmployeeStorage _employeeStorage;

        public EmployeeLogic(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public List<EmployeeViewModel> Read(EmployeeBindingModel model)
        {
            if (model == null)
            {
                return _employeeStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<EmployeeViewModel> { _employeeStorage.GetElement(model) };
            }
            return _employeeStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(EmployeeBindingModel model)
        {
            var element = _employeeStorage.GetElement(new EmployeeBindingModel { Login = model.Login, EMail = model.EMail, PhoneNumber = model.PhoneNumber });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть пользователь с такими данными");
            }
            if (model.Id.HasValue)
            {
                _employeeStorage.Update(model);
            }
            else
            {
                _employeeStorage.Insert(model);
            }
        }

        public void Delete(EmployeeBindingModel model)
        {
            var element = _employeeStorage.GetElement(new EmployeeBindingModel { Id = model.Id });
            if (element == null)
            {
                throw new Exception("Пользователь не найден");
            }
            _employeeStorage.Delete(model);
        }
    }
}