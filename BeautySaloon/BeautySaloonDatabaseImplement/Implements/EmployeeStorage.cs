using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using BeautySaloonDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeautySaloonDatabaseImplement.Implements
{
    public class EmployeeStorage : IEmployeeStorage
    {
        public List<EmployeeViewModel> GetFullList()
        {
            using (var context = new BeautySaloonDatabase())
            {
                return context.Employees
                .Select(rec => new EmployeeViewModel
                {
                    Id = rec.Id,
                    F_Name = rec.F_Name,
                    L_Name = rec.L_Name,
                    Login = rec.Login,
                    Password = rec.Password,
                    EMail = rec.EMail,
                    PhoneNumber = rec.PhoneNumber
                })
                .ToList();
            }
        }

        public List<EmployeeViewModel> GetFilteredList(EmployeeBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                return context.Employees
                .Where(rec => rec.Login == model.Login && rec.Password == model.Password)
                .Select(rec => new EmployeeViewModel
                {
                    Id = rec.Id,
                    F_Name = rec.F_Name,
                    L_Name = rec.L_Name,
                    Login = rec.Login,
                    Password = rec.Password,
                    EMail = rec.EMail,
                    PhoneNumber = rec.PhoneNumber
                })
                .ToList();
            }
        }

        public EmployeeViewModel GetElement(EmployeeBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                var employee = context.Employees
                .FirstOrDefault(rec => rec.Login == model.Login || rec.Id == model.Id);
                return employee != null ?
                new EmployeeViewModel
                {
                    Id = employee.Id,
                    F_Name = employee.F_Name,
                    L_Name = employee.L_Name,
                    Login = employee.Login,
                    Password = employee.Password,
                    EMail = employee.EMail,
                    PhoneNumber = employee.PhoneNumber
                } :
                null;
            }
        }

        public void Insert(EmployeeBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                context.Employees.Add(CreateModel(model, new Employee()));
                context.SaveChanges();
            }
        }

        public void Update(EmployeeBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                var element = context.Employees.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }

        public void Delete(EmployeeBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                Employee element = context.Employees.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Employees.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Employee CreateModel(EmployeeBindingModel model, Employee employee)
        {
            employee.F_Name = model.F_Name;
            employee.L_Name = model.L_Name;
            employee.Login = model.Login;
            employee.Password = model.Password;
            employee.EMail = model.EMail;
            employee.PhoneNumber = model.PhoneNumber;
            return employee;
        }
    }
}