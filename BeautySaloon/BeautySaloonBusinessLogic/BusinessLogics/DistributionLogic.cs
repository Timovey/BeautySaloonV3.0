using BeautySaloonBusinessLogic.BindingModels;
using BeautySaloonBusinessLogic.Interfaces;
using BeautySaloonBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BeautySaloonBusinessLogic.BusinessLogics
{
    public class DistributionLogic
    {
        private readonly IDistributionStorage _distributionStorage;

        private readonly IVisitStorage _visitStorage;

        public DistributionLogic(IDistributionStorage distributionStorage, IVisitStorage visitStorage)
        {
            _distributionStorage = distributionStorage;
            _visitStorage = visitStorage;
        }

        public List<DistributionViewModel> Read(DistributionBindingModel model)
        {
            if (model == null)
            {
                return _distributionStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<DistributionViewModel> { _distributionStorage.GetElement(model) };
            }
            return _distributionStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(DistributionBindingModel model)
        {
            var element = _distributionStorage.GetElement(new DistributionBindingModel { IssueDate = model.IssueDate });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже произведена выдача в данное время");
            }
            if (model.Id.HasValue)
            {
                _distributionStorage.Update(model);
            }
            else
            {
                _distributionStorage.Insert(model);
            }
        }

        public void Delete(DistributionBindingModel model)
        {
            var element = _distributionStorage.GetElement(new DistributionBindingModel { Id = model.Id });
            if (element == null)
            {
                throw new Exception("Выдача не найдена");
            }
            _distributionStorage.Delete(model);
        }

        public void Linking(DistributionLinkingBindingModel model)
        {
            var distribution = _distributionStorage.GetElement(new DistributionBindingModel
            {
                Id = model.DistributionId
            });

            var visit = _visitStorage.GetElement(new VisitBindingModel
            {
                Id = model.VisitId
            });

            if (distribution == null)
            {
                throw new Exception("Не найдена выдача");
            }

            if (visit == null)
            {
                throw new Exception("Не найдено посещение");
            }

            if (distribution.VisitId.HasValue)
            {
                throw new Exception("Данная выдача уже привязана к посещению");
            }

            _distributionStorage.Update(new DistributionBindingModel
            {
                Id = distribution.Id,
                IssueDate = distribution.IssueDate,
                DistributionCosmetics = distribution.DistributionCosmetics,
                EmployeeId = distribution.EmployeeId,
                VisitId = model.VisitId
            });
        }
    }
}