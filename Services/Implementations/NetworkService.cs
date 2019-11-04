#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Store.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Model.Store;
using Services.ViewModels.Store;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Model.Customers.Interfaces;
using Model.Customers;
using Services.Messaging.CustomerCatalogService;
using Infrastructure.Domain;
using Infrastructure.Querying;
#endregion

namespace Services.Implementations
{
    public class NetworkService : INetworkService
    {
        #region Declares
        private readonly INetworkRepository _networkRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly INetworkCenterService _networkCenterService;
        private readonly ICenterRepository _centerRepository;
        #endregion

        #region Ctor
        public NetworkService(INetworkRepository networkRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository
            , ICenterRepository centerRepository
            , INetworkCenterService networkCenterService)
        {
            _networkRepository = networkRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
            _centerRepository = centerRepository;
            _networkCenterService = networkCenterService;
        }
        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddNetwork(AddNetworkRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Network network = new Network();
                network.ID = Guid.NewGuid();
                network.CreateDate = PersianDateTime.Now;
                network.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                network.DeliverWhenCreditLow = request.DeliverWhenCreditLow;
                network.NetworkName = request.NetworkName;
                network.Note = request.Note;

                network.RowVersion = 1;

                // Validation
                if (network.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in network.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                // be jaye Triggere After Insert. NetworkCenter-ha insert mishavad
                IList<NetworkCenter> networkCenters = new List<NetworkCenter>();
                foreach (Center center in _centerRepository.FindAll())
                {
                    NetworkCenter networkCenter = new NetworkCenter();
                    networkCenter.Network = network;
                    networkCenter.Center = center;
                    networkCenter.CreateDate = PersianDateTime.Now;
                    networkCenter.CreateEmployee = network.CreateEmployee;
                    networkCenter.Status = NetworkCenterStatus.NotDefined;
                    networkCenter.RowVersion = 1;

                    networkCenters.Add(networkCenter);
                }

                network.NetworkCenters = networkCenters;
                _networkRepository.Add(network);

                _uow.Commit();

                ////response.success = true;
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add("FIRST INNER EXPCEPTION: " + ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                        response.ErrorMessages.Add("SECOND INNER EXPCEPTION: " + ex.InnerException.InnerException.Message);
                }
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditNetwork(EditNetworkRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            Network network = new Network();
            network = _networkRepository.FindBy(request.ID);

            if (network != null)
            {
                try
                {
                    network.ModifiedDate = PersianDateTime.Now;
                    network.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    network.DeliverWhenCreditLow = request.DeliverWhenCreditLow;
                    if (request.NetworkName != null)
                        network.NetworkName = request.NetworkName;
                    if (request.Note != null)
                        network.Note = request.Note;

                    if (network.RowVersion != request.RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        network.RowVersion += 1;
                    }

                    if (network.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in network.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _networkRepository.Save(network);
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
                
                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;
        }
        #endregion

        #region Delete
        public GeneralResponse DeleteNetwork(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Network network = new Network();
            network = _networkRepository.FindBy(request.ID);

            if (network != null)
            {
                try
                {
                    _networkRepository.Remove(network);
                    _uow.Commit();

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

        #region Get

        public GetNetworkResponse GetNetwork(GetRequest request)
        {
            GetNetworkResponse response = new GetNetworkResponse();

            try
            {
                Network network = new Network();
                NetworkView networkView = network.ConvertToNetworkView();

                network = _networkRepository.FindBy(request.ID);
                if (network != null)
                    networkView = network.ConvertToNetworkView();

                response.NetworkView = networkView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetNetworksResponse GetNetworks()
        {
            GetNetworksResponse response = new GetNetworksResponse();

            try
            {
                IEnumerable<NetworkView> networks = _networkRepository.FindAll()
                    .ConvertToNetworkViews();

                response.NetworkViews = networks;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        

        public IEnumerable<Network> GetRawNetworks()
        {
            return _networkRepository.FindAll();
        }


        #endregion

        #endregion

        #region New Methods

        #region Read

        public GetGeneralResponse<IEnumerable<NetworkSummaryView>> GetNetworks(int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<NetworkSummaryView>> response = new GetGeneralResponse<IEnumerable<NetworkSummaryView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Network> networks = _networkRepository.FindAllWithSort(index, count,sort);

                response.data = networks.data.ConvertToNetworkSummaryViews();
                response.totalCount = networks.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }


        public GetGeneralResponse<IEnumerable<NetworkSummaryView>> GetNetworks(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<NetworkSummaryView>> response = new GetGeneralResponse<IEnumerable<NetworkSummaryView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Network> networks = _networkRepository.FindAll(index, count);

                response.data = networks.data.ConvertToNetworkSummaryViews();
                response.totalCount = networks.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }


        #endregion

        #region Insert

        public GeneralResponse AddNetworks(IEnumerable<AddNetworkRequest> requests, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (AddNetworkRequest request in requests)
                {
                    Network network = new Network();
                    network.ID = Guid.NewGuid();
                    network.CreateDate = PersianDateTime.Now;
                    network.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    network.DeliverWhenCreditLow = request.DeliverWhenCreditLow;
                    network.NetworkName = request.NetworkName;
                    network.Note = request.Note;
                    network.SortOrder = GetSortOrder();
                    network.Discontinued = request.Discontinued;
                    network.Alias = request.Alias;
                    network.RowVersion = 1;

                    // Validation
                    if (network.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in network.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    // be jaye Triggere After Insert. NetworkCenter-ha insert mishavad
                    IList<NetworkCenter> networkCenters = new List<NetworkCenter>();
                    foreach (Center center in _centerRepository.FindAll())
                    {
                        NetworkCenter networkCenter = new NetworkCenter();
                        networkCenter.Network = network;
                        networkCenter.Center = center;
                        networkCenter.CreateDate = PersianDateTime.Now;
                        networkCenter.CreateEmployee = network.CreateEmployee;
                        networkCenter.Status = NetworkCenterStatus.NotDefined;
                        networkCenter.RowVersion = 1;

                        networkCenters.Add(networkCenter);
                    }

                    network.NetworkCenters = networkCenters;
                    _networkRepository.Add(network);
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

        #region Edit

        public GeneralResponse EditNetworks(IEnumerable<EditNetworkRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditNetworkRequest request in requests)
                {
                    Network network = _networkRepository.FindBy(request.ID);
                    network.ModifiedDate = PersianDateTime.Now;
                    network.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    network.DeliverWhenCreditLow = request.DeliverWhenCreditLow;
                    network.Discontinued = request.Discontinued;
                    network.Alias = request.Alias;
                    if (request.NetworkName != null)
                        network.NetworkName = request.NetworkName;
                    if (request.Note != null)
                        network.Note = request.Note;

                    if (network.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        network.RowVersion += 1;
                    }

                    if (network.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in network.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _networkRepository.Save(network);
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

        #region Delete

        public GeneralResponse DeleteNetworks(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {
                    Network network = new Network();
                    network = _networkRepository.FindBy(request.ID);
                    if (network.NetworkCenters.Count() > 0)
                    {
                        network.NetworkCenters = null;
                    }
                    _networkRepository.Remove(network);
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

            // Current Agency is the Request
            Network curretNetwork = new Network();
            curretNetwork = _networkRepository.FindBy(request.ID);

            // Find the Previews Agency
            Network previewsNetwork = new Network();
            try
            {
                previewsNetwork = _networkRepository.FindAll()
                                .Where(s => s.SortOrder < curretNetwork.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (curretNetwork != null && previewsNetwork != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = curretNetwork.SortOrder;
                    int previews = previewsNetwork.SortOrder;

                    curretNetwork.SortOrder = previews;
                    previewsNetwork.SortOrder = current;

                    _networkRepository.Save(curretNetwork);
                    _networkRepository.Save(previewsNetwork);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }

        public MoveResponse MoveDown(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Agency is the Request
            Network currentNetwork = new Network();
            currentNetwork = _networkRepository.FindBy(request.ID);

            // Find the Previews Agency
            Network nextNetwork = new Network();
            try
            {
                nextNetwork = _networkRepository.FindAll()
                                .Where(s => s.SortOrder > currentNetwork.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentNetwork != null && nextNetwork != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = currentNetwork.SortOrder;
                    int previews = nextNetwork.SortOrder;

                    currentNetwork.SortOrder = previews;
                    nextNetwork.SortOrder = current;

                    _networkRepository.Save(currentNetwork);
                    _networkRepository.Save(nextNetwork);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }


        #endregion

        #endregion

        #region private Method

        private int GetSortOrder()
        {
            try
            {
                IEnumerable<Network> CreditServices = _networkRepository.FindAll();
                return CreditServices.Max(s => s.SortOrder) + 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        #endregion
    }
}
