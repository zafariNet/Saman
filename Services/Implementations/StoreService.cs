using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Store.Interfaces;
using Model.Employees.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.StoreCatalogService;
using Model.Store;
using Services.ViewModels.Store;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Infrastructure.Domain;
using Infrastructure.Querying;
namespace Services.Implementations
{
    public class StoreService : IStoreService
    {
        #region Declares
        private readonly IStoreRepository _storeRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStoreProductRepository _storeProductRepository;
        private readonly IUnitOfWork _uow;
        #endregion

        #region Ctor
        public StoreService(IStoreRepository storeRepository,IStoreProductRepository storeProductRepository, IUnitOfWork uow)
        {
            _storeRepository = storeRepository;
            _storeProductRepository = storeProductRepository;
            _uow = uow;
        }

        public StoreService(IStoreRepository storeRepository, IEmployeeRepository employeeRepository,IStoreProductRepository storeProductRepository, IUnitOfWork uow)
            : this(storeRepository, storeProductRepository,uow)
        {
            _employeeRepository = employeeRepository;
            _storeProductRepository = storeProductRepository;
            _uow = uow;
        }
        #endregion

        #region Old Methods

        #region Add

        public GeneralResponse AddStore(AddStoreRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Store store = new Store();
                store.ID = Guid.NewGuid();
                store.CreateDate = PersianDateTime.Now;
                store.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                store.OwnerEmployee = this._employeeRepository.FindBy(request.OwnerEmployeeID);
                store.StoreName = request.StoreName;
                store.Note = request.Note;

                store.RowVersion = 1;

                // Validation
                if (store.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in store.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                _storeRepository.Add(store);
                _uow.Commit();

                ////response.success = true;
            
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditStore(EditStoreRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            Store store = new Store();
            store = _storeRepository.FindBy(request.ID);

            if (store != null)
            {
                try
                {
                    store.ModifiedDate = PersianDateTime.Now;
                    store.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.OwnerEmployeeID != null)
                        store.OwnerEmployee = this._employeeRepository.FindBy(request.OwnerEmployeeID);
                    if (request.StoreName != null)
                        store.StoreName = request.StoreName;
                    if (request.Note != null)
                        store.Note = request.Note;

                    if (store.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        store.RowVersion += 1;
                    }

                    if (store.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in store.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _storeRepository.Save(store);
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
        public GeneralResponse DeleteStore(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Store store = new Store();
            store = _storeRepository.FindBy(request.ID);

            if (store != null)
            {
                try
                {
                    _storeRepository.Remove(store);
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

        #region Get One
        public GetStoreResponse GetStore(GetRequest request)
        {
            GetStoreResponse response = new GetStoreResponse();

            try
            {
                Store store = new Store();
                StoreView storeView = store.ConvertToStoreView();

                store = _storeRepository.FindBy(request.ID);
                if (store != null)
                    storeView = store.ConvertToStoreView();

                response.StoreView = storeView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        public GetStoresResponse GetStores()
        {
            GetStoresResponse response = new GetStoresResponse();

            try
            {
                IEnumerable<StoreView> stores = _storeRepository.FindAll()
                    .ConvertToStoreViews();

                response.StoreViews = stores;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetStoresResponse GetStores(AjaxGetRequest request)
        {
            GetStoresResponse response = new GetStoresResponse();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                Infrastructure.Domain.Response<Store> storesResponse = _storeRepository.FindAll(index, count);

                IEnumerable<StoreView> stores = storesResponse.data.ConvertToStoreViews();

                response.StoreViews = stores;
                response.Count = storesResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #endregion

        #region New Methods

        #region Read

        public GetGeneralResponse<IEnumerable<StoreView>> GetStores(int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<StoreView>> response = new GetGeneralResponse<IEnumerable<StoreView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Store> store = _storeRepository.FindAllWithSort(index, count,sort);
                response.data = store.data.ConvertToStoreViews();
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

        #region Insert

        public GeneralResponse AddStores(IEnumerable<AddStoreRequest> requests,Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (AddStoreRequest request in requests)
                {
                    Infrastructure.Querying.Query query = new Query();
                    Criterion CriteriaOwnerID = new Criterion("OwnerEmployee.ID", request.OwnerEmployeeID, CriteriaOperator.Equal);
                    query.Add(CriteriaOwnerID);

                    Store _store = _storeRepository.FindBy(query).FirstOrDefault();

                    if (_store == null)
                    {
                        Store store = new Store();

                        store.ID = Guid.NewGuid();
                        store.CreateDate = PersianDateTime.Now;
                        store.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                        store.StoreName = request.StoreName;
                        store.OwnerEmployee = _employeeRepository.FindBy(request.OwnerEmployeeID);
                        store.Note = request.Note;
                        store.RowVersion = 1;



                        #region Validation

                        if (store.GetBrokenRules().Count() > 0)
                        {


                            foreach (BusinessRule businessRule in store.GetBrokenRules())
                            {
                                response.ErrorMessages.Add(businessRule.Rule);
                            }

                            return response;
                        }

                        #endregion

                        _storeRepository.Add(store);
                    }
                    else
                    {
                        response.ErrorMessages.Add("برای " + _store.OwnerEmployee.Name + " قبلا یک انبار ثبت شده است ");
                        return response;
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

        #region Edit

        public GeneralResponse EditStores(IEnumerable<EditStoreRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditStoreRequest request in requests)
                {

                    Infrastructure.Querying.Query query = new Query();
                    Criterion CriteriaOwnerID = new Criterion("OwnerEmployee.ID", request.OwnerEmployeeID, CriteriaOperator.Equal);
                    query.Add(CriteriaOwnerID);

                    Store _store = _storeRepository.FindBy(query).FirstOrDefault();

                    if (_store == null)
                    {

                        Store store = _storeRepository.FindBy(request.ID);

                        store.ModifiedDate = PersianDateTime.Now;
                        store.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                        if (request.OwnerEmployeeID != store.OwnerEmployee.ID)
                            store.OwnerEmployee = this._employeeRepository.FindBy(request.OwnerEmployeeID);
                        if (request.StoreName != null)
                            store.StoreName = request.StoreName;
                        if (request.Note != null)
                            store.Note = request.Note;

                        if (store.RowVersion != request.RowVersion)
                        {

                            response.ErrorMessages.Add("EditConcurrencyKey");
                            return response;
                        }
                        else
                        {
                            store.RowVersion += 1;
                        }

                        if (store.GetBrokenRules().Count() > 0)
                        {

                            foreach (BusinessRule businessRule in store.GetBrokenRules())
                            {
                                response.ErrorMessages.Add(businessRule.Rule);
                            }

                            return response;
                        }
                        _storeRepository.Save(store);
                    }
                    else
                    {
                        response.ErrorMessages.Add("برای " + _store.OwnerEmployee.Name + " قبلا یک انبار ثبت شده است ");
                        return response;
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

        #region Delete

        public GeneralResponse DeleteStores(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {

                    Infrastructure.Querying.Query query = new Query();
                    Criterion CriteriaStoreID = new Criterion("Store.ID", request.ID, CriteriaOperator.Equal);
                    query.Add(CriteriaStoreID);

                    StoreProduct storeProduct = _storeProductRepository.FindBy(query).FirstOrDefault();
                    if (storeProduct == null)
                    {
                        Store store = new Store();

                        store = _storeRepository.FindBy(request.ID);

                        _storeRepository.Remove(store);
                    }
                    else
                    {
                        response.ErrorMessages.Add("در سیستم برای انبار " + storeProduct.Store.StoreName + " تراکنش وجود داد. لذا امکان حذف این انبار وجود ندارد");
                        return response;
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

        #endregion
    }
}
