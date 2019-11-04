using Infrastructure.Domain;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Services.Implementations
{
    public class QueueService:IQueueService
    {
        #region Declare

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IQueueRepository _queueRepository;

        private readonly IQueueLocalPhoneStoreRepository _queueLocalPhoneRepository;

        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public QueueService(IEmployeeRepository employeeRepository, IQueueRepository queueRepository, IUnitOfWork uow, IQueueLocalPhoneStoreRepository queueLocalPhoneRepository)

        {
            _employeeRepository = employeeRepository;
            _queueRepository = queueRepository;
            _queueLocalPhoneRepository = queueLocalPhoneRepository;
            _uow = uow;
        }

        #endregion

        #region Get Queues From Asterisk

        public GeneralResponse GetQueuesFromAsterisk()
        {
            GeneralResponse response = new GeneralResponse();

            try
            {

                var monitoring = new MonitoringControllerService();
                string Token = monitoring.login("saman", "102030");

                List<QueueWSDL> queues = monitoring.getQueue(Token).ToList<QueueWSDL>();
                List<Queue> savedQueues = _queueRepository.FindAll().ToList<Queue>();

                foreach (var item in queues)
                {
                    foreach (var _item in savedQueues)
                    {
                        if (item.id == _item.AsteriskID)
                        {
                            _item.QueueName = item.name;
                            _queueRepository.Save(_item);

                        }
                    }
                    
                }

                if (queues.Count() > savedQueues.Count())
                {
                    var result = queues.Where(x => savedQueues.All(s => s.AsteriskID != x.id));

                    foreach (var item in result)
                    {
                        var queue = new Queue();
                        queue.AsteriskID = item.id;
                        queue.ID = Guid.NewGuid();
                        queue.QueueName = item.name;

                        _queueRepository.Add(queue);
                    }
                    
                }

                if (queues.Count() < savedQueues.Count())
                {

                    IEnumerable<QueueLocalPhoneStore> queueLocalPhone = _queueLocalPhoneRepository.FindAll();

                    var result1 = queueLocalPhone.Where(x => queues.All(s => s.id != x.Queue.AsteriskID));

                    foreach (var item in result1)
                    {
                        _queueLocalPhoneRepository.Remove(item);
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

        #region Update persian Name

        public GeneralResponse QueueUpdatePersianName(IEnumerable<EditQueueRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    Queue queue=new Queue();
                    queue = _queueRepository.FindBy(request.ID);
                    if (queue != null)
                        queue.PersianName = request.PersianName;
                    _queueRepository.Save(queue);
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

        public GetGeneralResponse<IEnumerable<QueueView>> GetAllQueue(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<QueueView>> response=new GetGeneralResponse<IEnumerable<QueueView>>();

            try
            {
                int index = (pageNumber - 1)*pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Queue", sort);
                Response<Queue> queues = _queueRepository.FindAll(query, index, count);

                response.data = queues.data.ConvertToQueueViews();
                response.totalCount = queues.totalCount;


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
