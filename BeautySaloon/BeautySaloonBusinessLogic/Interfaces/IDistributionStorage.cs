using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.Interfaces
{
    public interface IDistributionStorage
    {
        List<DistributionViewModel> GetFullList();

        List<DistributionViewModel> GetFilteredList(DistributionBindingModel model);

        DistributionViewModel GetElement(DistributionBindingModel model);

        void Insert(DistributionBindingModel model);

        void Update(DistributionBindingModel model);

        void Delete(DistributionBindingModel model);
    }
}