using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.Interfaces
{
    public interface IReceiptStorage
    {
        List<ReceiptViewModel> GetFullList();

        List<ReceiptViewModel> GetFilteredList(ReceiptBindingModel model);

        ReceiptViewModel GetElement(ReceiptBindingModel model);

        void Insert(ReceiptBindingModel model);

        void Update(ReceiptBindingModel model);

        void Delete(ReceiptBindingModel model);
    }
}