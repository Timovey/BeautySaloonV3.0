using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;


namespace BeautySaloonBusinessLogic.Interfaces
{
    public interface IVisitStorage
    {
        List<VisitViewModel> GetFullList();

        List<VisitViewModel> GetFilteredList(VisitBindingModel model);

        VisitViewModel GetElement(VisitBindingModel model);

        void Insert(VisitBindingModel model);

        void Update(VisitBindingModel model);

        void Delete(VisitBindingModel model);
    }
}