using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Store.Interfaces;
using Model.Fiscals.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Model.Store;
using Services.ViewModels.Store;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;

namespace Services.Implementations
{
    public class NetworkCreditService : INetworkCreditService
    {
        private readonly INetworkCreditRepository _networkCreditRepository;
        private readonly INetworkRepository _networkRepository;
        private readonly IMoneyAccountRepository _moneyAccountRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        public NetworkCreditService(INetworkCreditRepository networkCreditRepository, IUnitOfWork uow)
        {
            _networkCreditRepository = networkCreditRepository;
            _uow = uow;
        }

        public NetworkCreditService(INetworkCreditRepository networkCreditRepository, INetworkRepository networkRepository,
            IMoneyAccountRepository moneyAccountRepository, IUnitOfWork uow, IEmployeeRepository employeeRepository)
            : this(networkCreditRepository, uow)
        {
            this._networkRepository = networkRepository;
            this._moneyAccountRepository = moneyAccountRepository;
            _employeeRepository = employeeRepository;
        }

        #region Old Methods

        public GeneralResponse AddNetworkCredit(AddNetworkCreditRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                NetworkCredit networkCredit = new NetworkCredit();
                networkCredit.ID = Guid.NewGuid();
                networkCredit.CreateDate = PersianDateTime.Now;
                networkCredit.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                networkCredit.Amount = request.Amount;
                networkCredit.FromAccount = this._moneyAccountRepository.FindBy(request.FromAccountID);
                networkCredit.InvestDate = request.InvestDate;
                networkCredit.Network = this._networkRepository.FindBy(request.NetworkID);
                networkCredit.Note = request.Note;
                networkCredit.ToAccount = request.ToAccount;
                networkCredit.TransactionNo = request.TransactionNo;
                networkCredit.RowVersion = 1;

                _networkCreditRepository.Add(networkCredit);
                _uow.Commit();

                ////response.success = true;

                // Validation
                if (networkCredit.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in networkCredit.GetBrokenRules())
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

        public GeneralResponse EditNetworkCredit(EditNetworkCreditRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            NetworkCredit networkCredit = new NetworkCredit();
            networkCredit = _networkCreditRepository.FindBy(request.ID);

            if (networkCredit != null)
            {
                try
                {
                    networkCredit.ModifiedDate = PersianDateTime.Now;
                    networkCredit.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                        networkCredit.Amount = request.Amount;
                    networkCredit.FromAccount = this._moneyAccountRepository.FindBy(request.FromAccountID);
                    if (request.InvestDate != null)
                        networkCredit.InvestDate = request.InvestDate;
                    if (request.NetworkID != null)
                        networkCredit.Network = this._networkRepository.FindBy(request.NetworkID);
                    networkCredit.Note = request.Note;
                    networkCredit.ToAccount = request.ToAccount;
                    networkCredit.TransactionNo = request.TransactionNo;

                    if (networkCredit.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        networkCredit.RowVersion += 1;
                    }

                    if (networkCredit.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in networkCredit.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _networkCreditRepository.Save(networkCredit);
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

        public GeneralResponse DeleteNetworkCredit(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            NetworkCredit networkCredit = new NetworkCredit();
            networkCredit = _networkCreditRepository.FindBy(request.ID);

            if (networkCredit != null)
            {
                try
                {
                    _networkCreditRepository.Remove(networkCredit);
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

        public GetNetworkCreditResponse GetNetworkCredit(GetRequest request)
        {
            GetNetworkCreditResponse response = new GetNetworkCreditResponse();

            try
            {
                NetworkCredit networkCredit = new NetworkCredit();
                NetworkCreditView networkCreditView = networkCredit.ConvertToNetworkCreditView();

                networkCredit = _networkCreditRepository.FindBy(request.ID);
                if (networkCredit != null)
                    networkCreditView = networkCredit.ConvertToNetworkCreditView();

                response.NetworkCreditView = networkCreditView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetNetworkCreditsResponse GetNetworkCredits(GetNetworkCreditsRequest request)
        {
            GetNetworkCreditsResponse response = new GetNetworkCreditsResponse();

            try
            {
                IEnumerable<NetworkCreditView> networkCredits = _networkCreditRepository.FindAll()
                    .Where(s => s.Network.ID == request.NetworkID)
                    .ConvertToNetworkCreditViews();

                response.NetworkCreditViews = networkCredits;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        #endregion

        #region New Methods

        public GeneralResponse AddNetworkCredit(AddNetworkCreditRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                NetworkCredit networkCredit = new NetworkCredit();
                networkCredit.ID = Guid.NewGuid();
                networkCredit.CreateDate = PersianDateTime.Now;
                networkCredit.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                if (request.Type)
                    networkCredit.Amount = request.Amount;
                else
                    networkCredit.Amount = -request.Amount;
                networkCredit.FromAccount = this._moneyAccountRepository.FindBy(request.FromAccountID);
                networkCredit.InvestDate = request.InvestDate;
                
                networkCredit.Network = this._networkRepository.FindBy(request.NetworkID);
                networkCredit.Note = request.Note;
                networkCredit.ToAccount = request.ToAccount;
                networkCredit.TransactionNo = request.TransactionNo;
                networkCredit.RowVersion = 1;
                networkCredit.Balance += networkCredit.Network.Balance + networkCredit.Amount;
                _networkCreditRepository.Add(networkCredit);

                Network network = networkCredit.Network;
                network.Balance += request.Amount;
                _networkRepository.Save(network);

                _uow.Commit();

                ////response.success = true;

                // Validation
                if (networkCredit.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in networkCredit.GetBrokenRules())
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

        public GeneralResponse EditNetworkCredit(EditNetworkCreditRequest request,Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            NetworkCredit networkCredit = new NetworkCredit();
            networkCredit = _networkCreditRepository.FindBy(request.ID);

            if (networkCredit != null)
            {
                try
                {
                    networkCredit.ModifiedDate = PersianDateTime.Now;
                    networkCredit.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    networkCredit.Amount = request.Amount;
                    if (networkCredit.FromAccount.ID !=request.FromAccountID)
                    networkCredit.FromAccount = this._moneyAccountRepository.FindBy(request.FromAccountID);
                    if (request.InvestDate != null)
                        networkCredit.InvestDate = request.InvestDate;
                    if (request.NetworkID != networkCredit.Network.ID)
                        networkCredit.Network = this._networkRepository.FindBy(request.NetworkID);
                    networkCredit.Note = request.Note;
                    networkCredit.ToAccount = request.ToAccount;
                    networkCredit.TransactionNo = request.TransactionNo;

                    if (networkCredit.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        networkCredit.RowVersion += 1;
                    }

                    if (networkCredit.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in networkCredit.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _networkCreditRepository.Save(networkCredit);
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

        public GeneralResponse DeleteNetworkCredits(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {
                    _networkCreditRepository.RemoveById(request.ID);
                }
                _uow.Commit();
            }

            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }


        public GetGeneralResponse<NetworkCreditView> GetNetworkCredit(Guid ID)
        {
            GetGeneralResponse<NetworkCreditView> response= new GetGeneralResponse<NetworkCreditView>();

            try
            {
                NetworkCredit networkCredit = new NetworkCredit();
                NetworkCreditView networkCreditView = networkCredit.ConvertToNetworkCreditView();

                networkCredit = _networkCreditRepository.FindBy(ID);
                if (networkCredit != null)
                    networkCreditView = networkCredit.ConvertToNetworkCreditView();

                response.data = networkCreditView;
                
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<NetworkCreditView>> GetNetworkCredits(Guid ID)
        {
            GetGeneralResponse<IEnumerable<NetworkCreditView>> response = new GetGeneralResponse<IEnumerable<NetworkCreditView>>();

            try
            {
                IEnumerable<NetworkCreditView> networkCredits = _networkCreditRepository.FindAll()
                    .Where(s => s.Network.ID == ID)
                    .ConvertToNetworkCreditViews();

                response.data = networkCredits;
                response.totalCount = networkCredits.Count();
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion
    }
}
