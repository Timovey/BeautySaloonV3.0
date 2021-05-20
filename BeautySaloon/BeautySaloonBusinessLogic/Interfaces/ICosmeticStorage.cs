using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.Interfaces
{
    public interface ICosmeticStorage
    {
        List<CosmeticViewModel> GetFullList();

        List<CosmeticViewModel> GetFilteredList(CosmeticBindingModel model);

        CosmeticViewModel GetElement(CosmeticBindingModel model);

        void Insert(CosmeticBindingModel model);

        void Update(CosmeticBindingModel model);

        void Delete(CosmeticBindingModel model);
    }
}