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
using Infrastructure.Domain;
#endregion

namespace Services.Implementations
{
    public class CenterService : ICenterService
    {
        #region Declares

        private readonly ICenterRepository _centerRepository;

        private readonly ICodeRepository _codeRepository;

        private readonly INetworkCenterRepository _networkCenterRepository;

        private readonly IUnitOfWork _uow;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly INetworkCenterService _networkCenterService;

        private readonly INetworkRepository _networkRepository;

        private readonly ISpecialNumberRepository _specialNumberRepository;

        #endregion

        #region Ctor

        public CenterService(ICenterRepository centerRepository, ICodeRepository codeRepository
            , INetworkCenterRepository networkCenterRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository, INetworkCenterService networkCenterService
            , INetworkRepository networkRepository, ISpecialNumberRepository specialNumberRepository)
        {
            _centerRepository = centerRepository;
            _codeRepository = codeRepository;
            _networkCenterRepository = networkCenterRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
            _networkCenterService = networkCenterService;
            _networkRepository = networkRepository;
            _specialNumberRepository = specialNumberRepository;
        }
        #endregion

        #region Edit

        #endregion



        #region Get One
        public GetCenterResponse GetCenter(GetRequest request)
        {
            GetCenterResponse response = new GetCenterResponse();

            try
            {
                Center center = _centerRepository.FindBy(request.ID);

                if (center != null)
                {
                    CenterView centerView = center.ConvertToCenterView();

                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion criteria = new Criterion("Center.ID", center.ID, CriteriaOperator.Equal);
                    query.Add(criteria);
                    IEnumerable<NetworkCenter> networkCenters = _networkCenterRepository.FindBy(query);
                    IEnumerable<NetworkCenterView> networkCenterViews = networkCenters.ConvertToNetworkCenterViews();

                    centerView.NetworkCenters = networkCenterViews;


                    response.CenterView = centerView;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        #endregion

        #region Get Info

        public GetCenterInfoResponse GetCenterInfo(string adslPhone, int codeLengh)
        {
            GetCenterInfoResponse response = new GetCenterInfoResponse();

            try
            {
                IList<Center> centers = new List<Center>();

                response.hasCenter = false;

                // اگر مقدار تلفن عددی نبود هیچ عملیاتی انجام نده
                int adslInt = -1;
                if (!int.TryParse(adslPhone, out adslInt))
                {
                    response.hasCenter = false;
                    return response;
                }

                // اگر شماره خاص بود اینجا مشخص می شود
                string HqlSpecial = string.Format("From SpecialNumber s where s.FromNumber <= {0} and s.ToNumber >= {0}", adslInt);
                IEnumerable<SpecialNumber> specialNumbers = _specialNumberRepository.FindAll(HqlSpecial).data;
                if (specialNumbers.Count() > 0)
                {
                    response.Status = "عدم پوشش (شماره خاص)‏";
                    response.hasCenter = true;
                }

                // یافتن مرکز مخابراتی
                string Hql = String.Format(@"Select c From Code co Join co.Center c Where co.CodeName Like {0}%", adslPhone.Substring(0, codeLengh));
                centers = _centerRepository.FindAll(Hql).data.ToList();

                if (centers.Count == 1)
                {
                    Center center = centers.FirstOrDefault();

                    // agar shomareye khas bashad hasCenter = true ast va Status nabayad por shavad
                    if (!response.hasCenter)
                        response.Status = center.Status;

                    response.CenterName = center.ConvertToCenterView().CenterName;
                    response.CenterID = center.ID;
                    response.Center = center;
                    response.hasCenter = true;
                }
                else
                {
                    response.hasCenter = false;
                }
            }
            catch (Exception ex)
            {
                response.Status = "ConnectionError‏";
                response.hasCenter = false;
                return response;
            }

            return response;
        }
        #endregion

        #region Get All

        public GetCentersResponse GetCenters()
        {
            GetCentersResponse response = new GetCentersResponse();

            try
            {
                IEnumerable<CenterView> centers = _centerRepository.FindAll()
                    .ConvertToCenterViews();

                response.CenterViews = centers;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetCentersResponse GetCenters(IList<Sort> sort)
        {
            GetCentersResponse response = new GetCentersResponse();

            try
            {
                Response<Center> centers = _centerRepository.FindAllWithSort(-1,-1,sort);


                response.CenterViews = centers.data.ConvertToCenterViews();
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #region New Methods

        #region Read


        public GetGeneralResponse<IEnumerable<CenterView>> GetCenters(int pageSize, int pageNumber,string CenterName,IList<Sort> sort,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<CenterView>> response = new GetGeneralResponse<IEnumerable<CenterView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                Response<Center> centersResponse = new Response<Center>();
                if (CenterName != null)
                {
                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion CriteriaCentrName = new Criterion("CenterName", CenterName, CriteriaOperator.Contains);
                    query.Add(CriteriaCentrName);
                    centersResponse = _centerRepository.FindBy(query, -1, -1, null);

                }
                else
                {
                    if (index < 0 || count < 0)
                    {
                        string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Center", sort);
                        centersResponse = _centerRepository.FindAll(query);
                    }
                    else
                    {
                        string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Center", sort);
                        centersResponse = _centerRepository.FindAll(query,index,count);
                    }
                }
                response.data = centersResponse.data.ConvertToCenterViews();
                response.totalCount = centersResponse.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion

        #region Insert

        public GeneralResponse AddCenter(IEnumerable<AddCenterRequest> requests, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    Center center = new Center();
                    center.ID = Guid.NewGuid();
                    center.CreateDate = PersianDateTime.Now;
                    center.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    center.CenterName = request.CenterName;
                    center.Note = request.Note;
                    center.RowVersion = 1;

                    _centerRepository.Add(center);

                    // be jaye Triggere After Insert. NetworkCenter-ha insert mishavad
                    IList<NetworkCenter> networkCenters = new List<NetworkCenter>();
                    foreach (Network network in _networkRepository.FindAll())
                    {
                        NetworkCenter networkCenter = new NetworkCenter();
                        networkCenter.Network = network;
                        networkCenter.Center = center;
                        networkCenter.CreateDate = PersianDateTime.Now;
                        networkCenter.CreateEmployee = center.CreateEmployee;
                        networkCenter.Status = NetworkCenterStatus.NotDefined;
                        networkCenter.RowVersion = 1;

                        networkCenters.Add(networkCenter);
                    }

                    center.NetworkCenters = networkCenters;

                    #region Validation

                    if (center.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in center.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _centerRepository.Add(center);
                }
                _uow.Commit();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion

        #region Edit

        public GeneralResponse EditCenter(IEnumerable<EditCenterRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (var request in requests)
            {
                Center center = new Center();
                center = _centerRepository.FindBy(request.ID);

                if (center != null)
                {
                    try
                    {
                        center.ModifiedDate = PersianDateTime.Now;
                        center.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                        if (request.CenterName != null)
                            center.CenterName = request.CenterName;
                        if (request.Note != null)
                            center.Note = request.Note;

                        #region RowVersion
                        if (center.RowVersion != request.RowVersion)
                        {
                            response.ErrorMessages.Add("EditConcurrencyKey");
                            return response;
                        }
                        else
                        {
                            center.RowVersion += 1;
                        }

                        #endregion

                        #region Validation
                        if (center.GetBrokenRules().Count() > 0)
                        {

                            foreach (BusinessRule businessRule in center.GetBrokenRules())
                            {
                                response.ErrorMessages.Add(businessRule.Rule);
                            }

                            return response;
                        }

                        #endregion

                        _centerRepository.Save(center);
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessages.Add(ex.Message);
                        return response;
                    }
                }
                else
                {
                    response.ErrorMessages.Add("NoItemToEditKey");
                    return response;
                }
            }

            try
            {
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

        public GeneralResponse DeleteCenter(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (var request in requests)
            {
                Center center = new Center();
                center = _centerRepository.FindBy(request.ID);

                if (center != null)
                {
                    try
                    {
                        #region Remove Dependencies
                        Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                        Criterion criterion = new Criterion("Center.ID", center.ID, CriteriaOperator.Equal);
                        query.Add(criterion);
                        IEnumerable<Code> codesOfThisCenter = _codeRepository.FindBy(query);

                        if (codesOfThisCenter != null)
                            _codeRepository.Remove(codesOfThisCenter);

                        Infrastructure.Querying.Query query2 = new Infrastructure.Querying.Query();
                        Criterion criterion2 = new Criterion("Center.ID", center.ID, CriteriaOperator.Equal);
                        query.Add(criterion);
                        IEnumerable<NetworkCenter> networkCenters = _networkCenterRepository.FindBy(query);

                        if (networkCenters != null)
                            _networkCenterRepository.Remove(networkCenters);
                        _uow.Commit();
                        #endregion

                        _centerRepository.Remove(center);
                        _uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessages.Add(ex.Message);
                        if (ex.InnerException != null)
                        {
                            //response.ErrorMessages.Add("INNER EX: " + ex.InnerException.Message);
                            response.ErrorMessages.Add("این مرکز شامل یک یا تعدادی مشتری ثبت شده میباشد");
                        }

                        return response;
                    }
                }
                else
                {
                    response.ErrorMessages.Add("NoItemToDeleteKey");
                    return response;
                }
            }

            return response;
        }

        #endregion

        #region Coverage

        #region Read

        public GetGeneralResponse<IEnumerable<NetworkCenterView>> GetCoverage(Guid CenterID, int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<NetworkCenterView>> response = new GetGeneralResponse<IEnumerable<NetworkCenterView>>();
            try
            {
                Center center = _centerRepository.FindBy(CenterID);
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                if (count != -1)
                    if (sort != null)
                    {
                        foreach (var item in sort)
                        {

                            if (item.SortColumn == "NetworkName")
                            {
                                if(item.Asc)
                                response.data = center.NetworkCenters.Skip(index).Take(count).ConvertToNetworkCenterViews().OrderBy(x => x.CenterName);
                                else
                                    response.data = center.NetworkCenters.Skip(index).Take(count).ConvertToNetworkCenterViews().OrderByDescending(x => x.CenterName);
                            }
                            else if (item.SortColumn == "CreateDate")
                            {
                                if (item.Asc)
                                    response.data = center.NetworkCenters.Skip(index).Take(count).ConvertToNetworkCenterViews().OrderBy(x => x.CreateDate);
                                else
                                    response.data = center.NetworkCenters.Skip(index).Take(count).ConvertToNetworkCenterViews().OrderByDescending(x => x.CreateDate);
                            }
                            else
                            {
                                response.data = center.NetworkCenters.Skip(index).Take(count).ConvertToNetworkCenterViews();
                            }
                        }
                    }
                    else
                        response.data = center.NetworkCenters.ConvertToNetworkCenterViews();

                response.totalCount = center.NetworkCenters.Count();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion
          
        #region Edit

        public GeneralResponse EditCoverage(IEnumerable<NetworkCenterView> requests, Guid CenterID)
        {
            GeneralResponse response = new GeneralResponse();
            foreach (var request in requests)
            {
                try
                {
                    NetworkCenter networkCenter = new NetworkCenter();

                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion criteria = new Criterion("Network.ID", request.NetworkID, CriteriaOperator.Equal);
                    query.Add(criteria);
                    criteria = new Criterion("Center.ID", CenterID, CriteriaOperator.Equal);
                    query.Add(criteria);

                    networkCenter = _networkCenterRepository.FindBy(query).FirstOrDefault();
                    networkCenter.Status = (NetworkCenterStatus)request.StatusInt;

                    _networkCenterRepository.Save(networkCenter);
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }
            }
            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return response;
            }
            return response;
        }

        #endregion

        #endregion

        #endregion
    }
}
