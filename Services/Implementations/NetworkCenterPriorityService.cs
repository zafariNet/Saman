using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Interfaces;
using Model.Customers.Validations.Interfaces;
using Model.Employees.Interfaces;
using Model.Store.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;
using Query = Infrastructure.Querying.Query;

namespace Services.Implementations
{
    public class NetworkCenterPriorityService:INetworkCenterPriorityService
    {

        #region Declare

        private readonly IEmployeeRepository _employeeRepository;
        private readonly INetworkRepository _networkRepository;
        private readonly ICenterRepository _centerRepository;
        private readonly IUnitOfWork _uow;
        private readonly INetworkCenterPriorityRepository _networkCenterPriorityRepository;

        #endregion

        #region Ctor

        public NetworkCenterPriorityService(IEmployeeRepository employeeRepository, INetworkRepository networkRepository,
            ICenterRepository centerRepository, IUnitOfWork uow, INetworkCenterPriorityRepository networkCenterPriorityRepository)
        {

            _employeeRepository = employeeRepository;
            _networkRepository = networkRepository;
            _centerRepository = centerRepository;
            _uow = uow;
            _networkCenterPriorityRepository = networkCenterPriorityRepository;
        }

        #endregion

        #region Read

        public GetGeneralResponse<IEnumerable<NetworkCenterPriorityView>> GetNetworkCenter(Guid CenterID)
        {
            GetGeneralResponse<IEnumerable<NetworkCenterPriorityView>> response =
                new GetGeneralResponse<IEnumerable<NetworkCenterPriorityView>>();

            try
            {
                Query query=new Query();
                Criterion ctr=new Criterion("Center.ID",CenterID,CriteriaOperator.Equal);
                query.Add(ctr);

                Response<NetworkCenterPriority> networkCenterPriorities = _networkCenterPriorityRepository.FindByQuery(query);

                response.data = networkCenterPriorities.data.ConvertToNetworkCenterPriorityViews();
                response.totalCount = networkCenterPriorities.totalCount;

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        

        #region Update All

        public GeneralResponse UpdateAll()
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                

                var centers = _centerRepository.FindAll();
                var networks = _networkRepository.FindAll();
                foreach (var center in centers)
                {
                    int counter = 0;
                    foreach (var network in networks)
                    {
                        counter++;
                        NetworkCenterPriority networkCenterPriority = new NetworkCenterPriority();
                        networkCenterPriority.Center = center;
                        networkCenterPriority.Network = network;
                        networkCenterPriority.SalePriority = counter;
                        _networkCenterPriorityRepository.Add(networkCenterPriority);
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

        #region Moving

        public MoveResponse MoveUp(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            NetworkCenterPriority currentnetNetworkCenterPriority = _networkCenterPriorityRepository.FindBy(request.ID);

            NetworkCenterPriority previewsNetworkCenterPriority = new NetworkCenterPriority();
            try
            {
                previewsNetworkCenterPriority = _networkCenterPriorityRepository.FindAll()
                                .Where(s => s.SalePriority < currentnetNetworkCenterPriority.SalePriority)
                                .OrderByDescending(s => s.SalePriority)
                                .FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            if (currentnetNetworkCenterPriority != null && previewsNetworkCenterPriority != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = currentnetNetworkCenterPriority.SalePriority;
                    int previews = previewsNetworkCenterPriority.SalePriority;

                    currentnetNetworkCenterPriority.SalePriority = previews;
                    previewsNetworkCenterPriority.SalePriority = current;

                    _networkCenterPriorityRepository.Save(currentnetNetworkCenterPriority);
                    _networkCenterPriorityRepository.Save(previewsNetworkCenterPriority);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }
            }
            return response;
        }

        public MoveResponse MoveDown(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            NetworkCenterPriority currentneNetworkCenterPriority = new NetworkCenterPriority();
            currentneNetworkCenterPriority = _networkCenterPriorityRepository.FindBy(request.ID);

            NetworkCenterPriority nextNetworkCenterPriority = new NetworkCenterPriority();
            try
            {
                nextNetworkCenterPriority = _networkCenterPriorityRepository.FindAll()
                                .Where(s => s.SalePriority > currentneNetworkCenterPriority.SalePriority)
                                .OrderBy(s => s.SalePriority)
                                .FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            if (currentneNetworkCenterPriority != null && nextNetworkCenterPriority != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = currentneNetworkCenterPriority.SalePriority;
                    int previews = nextNetworkCenterPriority.SalePriority;

                    currentneNetworkCenterPriority.SalePriority = previews;
                    nextNetworkCenterPriority.SalePriority = current;

                    _networkCenterPriorityRepository.Save(currentneNetworkCenterPriority);
                    _networkCenterPriorityRepository.Save(nextNetworkCenterPriority);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }
            }

            return response;
        }

        #endregion
    }
}
