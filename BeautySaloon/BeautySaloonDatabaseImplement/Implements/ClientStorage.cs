using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using BeautySaloonDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeautySaloonDatabaseImplement.Implements
{
    public class ClientStorage : IClientStorage
    {
        public List<ClientViewModel> GetFullList()
        {
            using (var context = new BeautySaloonDatabase())
            {
                return context.Clients.Select(rec => new ClientViewModel
                {
                    Id = rec.Id,
                    ClientName = rec.ClientName,
                    ClientSurame = rec.ClientSurame,
                    Login = rec.Login,
                    Password = rec.Password,
                    Mail = rec.Mail,
                    Tel = rec.Tel
                })
                .ToList();
            }
        }

        public List<ClientViewModel> GetFilteredList(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                return context.Clients
                //.Include(x => x.Procedure).Include(x => x.Purchase).Include(x => x.Visit)
                .Where(rec => rec.Login == model.Login && rec.Password == rec.Password)
                .Select(rec => new ClientViewModel
                {
                    Id = rec.Id,
                    ClientName = rec.ClientName,
                    ClientSurame = rec.ClientSurame,
                    Login = rec.Login,
                    Password = rec.Password,
                    Mail = rec.Mail,
                    Tel = rec.Tel
                })
                .ToList();
            }
        }

        public ClientViewModel GetElement(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new BeautySaloonDatabase())
            {
                var client = context.Clients
                //.Include(x => x.Procedure).Include(x => x.Purchase).Include(x => x.Visit)
                .FirstOrDefault(rec => rec.Login == model.Login ||
                rec.Id == model.Id);
                return client != null ?
                new ClientViewModel
                {
                    Id = client.Id,
                    ClientName = client.ClientName,
                    ClientSurame = client.ClientSurame,
                    Login = client.Login,
                    Password = client.Password,
                    Mail = client.Mail,
                    Tel = client.Tel
                } :
                null;
            }
        }

        public void Insert(ClientBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                context.Clients.Add(CreateModel(model, new Client(), context));
                context.SaveChanges();
            }
        }

        public void Update(ClientBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                var element = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Клиент не найден");
                }
                CreateModel(model, element, context);
                context.SaveChanges();
            }
        }

        public void Delete(ClientBindingModel model)
        {
            using (var context = new BeautySaloonDatabase())
            {
                Client element = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Clients.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Клиент не найден");
                }
            }
        }

        private Client CreateModel(ClientBindingModel model, Client client, BeautySaloonDatabase database)
        {
            client.ClientName = model.ClientName;
            client.ClientSurame = model.ClientSurname;
            client.Login = model.Login;
            client.Password = model.Password;
            client.Mail = model.Login;
            client.Tel = model.Tel;
            return client;
        }
    }
}