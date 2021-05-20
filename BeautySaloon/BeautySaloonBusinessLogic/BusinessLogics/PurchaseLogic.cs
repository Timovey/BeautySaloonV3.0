using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BusinessLogics
{
    public class PurchaseLogic
    {
        private readonly IPurchaseStorage _purchaseStorage;

        private readonly IReceiptStorage _receiptStorage;

        public PurchaseLogic(IPurchaseStorage purchaseStorage, IReceiptStorage receiptStorage)
        {
            _purchaseStorage = purchaseStorage;
            _receiptStorage = receiptStorage;
        }

        public List<PurchaseViewModel> Read(PurchaseBindingModel model)
        {
            if (model == null)
            {
                return _purchaseStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<PurchaseViewModel> { _purchaseStorage.GetElement(model) };
            }
            return _purchaseStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(PurchaseBindingModel model)
        {
            var element = _purchaseStorage.GetElement(new PurchaseBindingModel
            {
                Date = model.Date
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть покупка на это время");
            }
            if (model.Id.HasValue)
            {
                _purchaseStorage.Update(model);
            }
            else
            {
                _purchaseStorage.Insert(model);
            }
        }

        public void Delete(PurchaseBindingModel model)
        {
            var element = _purchaseStorage.GetElement(new PurchaseBindingModel
            {
                Id =
           model.Id
            });
            if (element == null)
            {
                throw new Exception("Покупка  не найдена");
            }
            _purchaseStorage.Delete(model);
        }

        public void Linking(PurchaseLinkingBindingModel model)
        {
            var purchase = _purchaseStorage.GetElement(new PurchaseBindingModel
            {
                Id = model.PurchaseId
            });

            var receipt = _receiptStorage.GetElement(new ReceiptBindingModel
            {
                Id = model.ReceiptId
            });

            if (purchase == null)
            {
                throw new Exception("Не найдена покупка");
            }

            if (receipt == null)
            {
                throw new Exception("Не найден чек");
            }

            if (purchase.ReceiptId.HasValue)
            {
                throw new Exception("Данная покупка уже привязана к чеку");
            }

            _purchaseStorage.Update(new PurchaseBindingModel
            {
                Id = purchase.Id,
                Date = purchase.Date,
                Price = purchase.Price,
                PurchaseProcedures = purchase.PurchaseProcedures,
                ClientId = purchase.ClientId,
                ReceiptId = model.ReceiptId
            });
        }
    }
}