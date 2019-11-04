using Infrastructure.Domain;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Employees;

namespace Services.Implementations
{
    public class LocalPhoneStoreService:ILocalPhoneStoreService
    {

        #region Declare

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ILocalPhoneStoreRepository _localPhoneStoreRepository;


        private readonly IQueueLocalPhoneStoreRepository _queueLocalPhoneRepository;

        private readonly IUnitOfWork _uow;
        #endregion

        #region Ctor

        public LocalPhoneStoreService(IEmployeeRepository employeeRepository, ILocalPhoneStoreRepository localPhoneStoreRepository, IQueueLocalPhoneStoreRepository queueLocalPhoneRepository,IUnitOfWork uow)
        {
            _employeeRepository = employeeRepository;
            _localPhoneStoreRepository = localPhoneStoreRepository;
            _queueLocalPhoneRepository = queueLocalPhoneRepository;
            _uow = uow;

        }

        #endregion

        #region Get Local Phones From Asterisk

        public GeneralResponse GetLocalPhoneStoresFromAsterisk()
        {
            GeneralResponse response = new GeneralResponse();

            try
            {

                MonitoringControllerService monitoring = new MonitoringControllerService();
                string Token = monitoring.login("saman", "102030");
                List<DeviceWSDL> devices = monitoring.getDevice(Token).ToList<DeviceWSDL>();
                List<LocalPhoneStore> savedLocalPhoneStores = _localPhoneStoreRepository.FindAll().ToList<LocalPhoneStore>();

                foreach (var item in devices)
                {
                    foreach (var _item in savedLocalPhoneStores)
                    {
                        if (item.dialString == _item.AsteriskID)
                        {
                            _item.LocalPhoneStoreNumber = item.name;
                            _localPhoneStoreRepository.Save(_item);

                        }
                    }
                }

                if (devices.Count() > savedLocalPhoneStores.Count())
                {
                    var result = devices.Where(x => savedLocalPhoneStores.All(s => s.AsteriskID != x.dialString));

                    foreach (var item in result)
                    {
                        LocalPhoneStore localPhoneStore = new LocalPhoneStore();
                        localPhoneStore.AsteriskID = item.dialString ?? string.Empty;
                        localPhoneStore.ID = Guid.NewGuid();
                        localPhoneStore.LocalPhoneStoreNumber = item.name;
                        localPhoneStore.Reserved = false;
                        _localPhoneStoreRepository.Add(localPhoneStore);
                    }

                }

                if (devices.Count() < savedLocalPhoneStores.Count())
                {


                    IEnumerable<LocalPhoneStore> queueLocalPhone = _localPhoneStoreRepository.FindAll();

                    var result = queueLocalPhone.Where(x => devices.All(s => s.id != x.AsteriskID));

                    foreach (var item in result)
                    {
                        _localPhoneStoreRepository.Remove(item);
                    }

                }
                _uow.Commit();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }
            return response;
        }

        #endregion

        #region Read All

        public GetGeneralResponse<IEnumerable<LocalPhoneStoreView>> GetLocalPhoneStores(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort)
        {
            var response=new GetGeneralResponse<IEnumerable<LocalPhoneStoreView>>();
            try
            {
                int index = (pageNumber - 1)*pageSize;
                int count = pageSize;
                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "LocalPhoneStore", sort);

                Response<LocalPhoneStore> localPhoneStores = _localPhoneStoreRepository.FindAll(query, index, count);

                response.data = localPhoneStores.data.ConvertToLocalPhoneStoreViews();
                response.totalCount = localPhoneStores.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<LocalPhoneStoreView>> GetUnReservedLocalPhoneStores()
        {
            var response = new GetGeneralResponse<IEnumerable<LocalPhoneStoreView>>();

            IList<FilterData> filter=new List<FilterData>();
            filter.Add(new FilterData()
            {
                data = new data()
                {
                    comparison = "eq",
                    type = "boolean",
                    value = new []{bool.FalseString}
                },
                field = "Reserved"
            });
            try
            {
                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "LocalPhoneStore",null);

                Response<LocalPhoneStore> localPhoneStores = _localPhoneStoreRepository.FindAll(query);

                response.data = localPhoneStores.data.ConvertToLocalPhoneStoreViews().OrderBy(x=>x.LocalPhoneNumber);
                response.totalCount = localPhoneStores.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion
    }
}
