#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;
using Services.ViewModels.Customers;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Model.Store.Interfaces;
using Model.Store;
using Infrastructure.Querying;
#endregion

namespace Services.Implementations
{
    public class NetworkCenterService : INetworkCenterService
    {
        #region Declares
        private readonly INetworkCenterRepository _networkCenterRepository;
        private readonly ICenterRepository _centerRepository;
        private readonly INetworkRepository _networkRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public NetworkCenterService(INetworkCenterRepository networkCenterRepository,
             IUnitOfWork uow)
        {
            _networkCenterRepository = networkCenterRepository;
            _uow = uow;
        }

        public NetworkCenterService(INetworkCenterRepository networkCenterRepository,
            ICenterRepository centerRepository, IUnitOfWork uow,
            INetworkRepository networkRepository, IEmployeeRepository employeeRepository)
            : this(networkCenterRepository, uow)
        {
            _centerRepository = centerRepository;
            _networkRepository = networkRepository;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Add
        public GeneralResponse AddNetworkCenter(AddNetworkCenterRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                NetworkCenter networkCenter = new NetworkCenter();
                networkCenter.Network = _networkRepository.FindBy(request.NetworkID);
                networkCenter.Center = _centerRepository.FindBy(request.CenterID);
                networkCenter.CreateDate = PersianDateTime.Now;
                networkCenter.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                networkCenter.Status = (NetworkCenterStatus)request.Status;
                networkCenter.CanSale = request.CanSale;
                networkCenter.RowVersion = 1;

                _networkCenterRepository.Add(networkCenter);
                _uow.Commit();

                ////response.success = true;

                // Validation
                if (networkCenter.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in networkCenter.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditNetworkCenter(EditNetworkCenterRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            NetworkCenter networkCenter = new NetworkCenter();
            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criteria1 = new Criterion("Network.ID", request.NetworkID, CriteriaOperator.Equal);
            Criterion criteria2 = new Criterion("Center.ID", request.CenterID, CriteriaOperator.Equal);
            query.Add(criteria1); query.Add(criteria2);

            networkCenter = _networkCenterRepository.FindBy(query).FirstOrDefault();

            if (networkCenter != null)
            {
                try
                {
                    networkCenter.Status = (NetworkCenterStatus)request.Status;
                    networkCenter.CanSale = request.CanSale;

                    if (networkCenter.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in networkCenter.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _networkCenterRepository.Save(networkCenter);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {

                response.ErrorMessages.Add("NoItemToEditKey");
            }
            return response;
        }
        #endregion

        #region Delete
        public GeneralResponse DeleteNetworkCenter(DeleteRequest2 request)
        {
            GeneralResponse response = new GeneralResponse();

            

            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

            Criterion NetID = new Criterion("Network.ID", request.ID1, CriteriaOperator.Equal);

            Criterion CenID = new Criterion("Center.ID", request.ID2, CriteriaOperator.Equal);

            query.Add(NetID);
            query.Add(CenID);


            IEnumerable<NetworkCenter> networkCenter = _networkCenterRepository.FindBy(query);

            if (networkCenter != null)
            {   
                try
                {
                    foreach(NetworkCenter cn in networkCenter)
                    {
                    _networkCenterRepository.Remove(cn);
                    _uow.Commit();
                    }
                    

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }


        #endregion

        #region Get One
        public GetNetworkCenterResponse GetNetworkCenter(GetRequest2 request)
        {
            GetNetworkCenterResponse response = new GetNetworkCenterResponse();

            try
            {
                NetworkCenter networkCenter = new NetworkCenter();
                NetworkCenterView networkCenterView = networkCenter.ConvertToNetworkCenterView();

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria1 = new Criterion("Network.ID", request.ID1, CriteriaOperator.Equal);
                Criterion criteria2 = new Criterion("Center.ID", request.ID2, CriteriaOperator.Equal);
                query.Add(criteria1); query.Add(criteria2);

                networkCenter = _networkCenterRepository.FindBy(query).FirstOrDefault();
                if (networkCenter != null)
                    networkCenterView = networkCenter.ConvertToNetworkCenterView();

                response.NetworkCenterView = networkCenterView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        public GetNetworkCentersResponse GetNetworkCenters(GetNetworkCentersRequest request)
        {
            GetNetworkCentersResponse response = new GetNetworkCentersResponse();

            try
            {
                IEnumerable<NetworkCenter> networkCenters;
                if (request.CenterID != null && request.CenterID != Guid.Empty)
                {
                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion criterion = new Criterion("Center.ID", request.CenterID, CriteriaOperator.Equal);
                    query.Add(criterion);

                    networkCenters = _networkCenterRepository.FindBy(query);
                }
                else if (request.NetworkID != null && request.NetworkID != Guid.Empty)
                {
                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion criterion = new Criterion("Network.ID", request.NetworkID, CriteriaOperator.Equal);
                    query.Add(criterion);

                    networkCenters = _networkCenterRepository.FindBy(query);
                }
                else
                {
                    networkCenters = _networkCenterRepository.FindAll();
                }

                response.NetworkCenterViews = networkCenters.ConvertToNetworkCenterViews();
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion
    }
}
