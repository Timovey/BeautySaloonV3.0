using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.Interfaces
{
    public interface IProcedureStorage
    {
        List<ProcedureViewModel> GetFullList();

        List<ProcedureViewModel> GetFilteredList(ProcedureBindingModel model);

        ProcedureViewModel GetElement(ProcedureBindingModel model);

        void Insert(ProcedureBindingModel model);

        void Update(ProcedureBindingModel model);

        void Delete(ProcedureBindingModel model);
    }
}