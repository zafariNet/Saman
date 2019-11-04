using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Customers;
using Model.Customers.Interfaces;
using Model.Employees.Interfaces;
using Model.Support;
using Model.Support.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Customers;
using Services.ViewModels.Sales;
using Services.ViewModels.Support;
using Services.ViewModels.Reports;
using Query = Infrastructure.Querying.Query;


namespace Services.Implementations
{
    public class SupportService:ISupportService
    {
        #region Declares

        private readonly ISupportRepository _supportRepository;

        private readonly ISupportStatusRepository _supportStatusRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ISupportExpertDispatchRepository _supportExpertDispatchRepository;

        private readonly ISaleService _saleService;

        private readonly IEmployeeService _employeeService;

        private readonly IUnitOfWork _uow;

        

        #endregion

        #region Ctor

        public SupportService(ISupportRepository supportRepository, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,
            IUnitOfWork uow, ISupportStatusRepository supportRelationRepository, ISupportExpertDispatchRepository supportExpertDispatchRepository
            , ISaleService saleService, IEmployeeService employeeService
            )
        {
            _supportRepository = supportRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _supportStatusRepository = supportRelationRepository;
            _supportExpertDispatchRepository = supportExpertDispatchRepository;
            _employeeService = employeeService;
            _saleService = saleService;
            
            _uow = uow;
        }

        #endregion

        #region Read

        #region Read All

        public GetGeneralResponse<IEnumerable<SupportView>> GetSupports()
        {
            GetGeneralResponse<IEnumerable<SupportView>> response=new GetGeneralResponse<IEnumerable<SupportView>>();

            try
            {
                IEnumerable<Support> supports = _supportRepository.FindAll();

                response.data = supports.ConvertToSupportViews();
                response.totalCount = supports.Count();
            }
            catch (Exception ex)
            {
               response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<SupportView>> GetSupports(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort,int LastState)
        {
            GetGeneralResponse<IEnumerable<SupportView>> response = new GetGeneralResponse<IEnumerable<SupportView>>();

            try
            {

                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                IList<FilterData> Filters=new List<FilterData>();
                if(filter!=null)
                    foreach(var item in filter)
                        Filters.Add(item);
                if (LastState == 0)
                {
                }
                if (LastState == 1)
                {
                    Filters.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.TrueString }
                        },
                        field = "SupportStatus.IsLastSupportState"
                    });
                }
                if (LastState == 2)
                {
                    Filters.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.FalseString }
                        },
                        field = "SupportStatus.IsLastSupportState"
                    });
                }

                string query = FilterUtilityService.GenerateFilterHQLQuery(Filters, "Support", sort);
                Response<Support> supports=new Response<Support>();

     
                    supports = _supportRepository.FindAll(query, index, count);

                    response.data = supports.data.ConvertToSupportViews();
                
                response.totalCount = supports.totalCount;
            }
            catch (Exception ex)
            {

                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }
            return response;
        }


        public GetGeneralResponse<IEnumerable<SupportOwnView>> GetOwnSupports(Guid EmployeeID, int pageSize, int pageNumber, IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<SupportOwnView>> response = new GetGeneralResponse<IEnumerable<SupportOwnView>>();

            try
            {
                
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                IList<FilterData> _filter = new List<FilterData>();

                //_filter.Add(new FilterData()
                //{
                //    data = new data() {
                //        comparison="eq",
                //        type="date",
                //        value=new[]{PersianDateTime.Now.Substring(0,10)}
                //    },
                //    field="DispatchDate"
                //});
                var employees = _employeeService.GetAllChilOfAnEmployee(EmployeeID);

                IList<string> Ids = new List<string>();
                if (employees.data.Any())
                {
                    foreach (var item in employees.data)
                    {
                        Ids.Add(item.ID.ToString());
                    }
                    Ids.Add(EmployeeID.ToString());
                    FilterData Filter = new FilterData();
                    Filter.field = "ExpertEmployee.ID";
                    Filter.data = new data()
                    {
                        comparison="eq",
                        type = "list",
                        value = Ids.ToArray()
                    };
                    _filter.Add(Filter);
                }
                //_filter.Add(new FilterData()
                //{
                //    data = new data()
                //    {
                //        comparison = "eq",
                //        type = "string",
                //        value = new[] { EmployeeID.ToString() }
                //    },
                //    field = "ExpertEmployee.ID"
                //});
                if (!_filter.Any())
                {
                    _filter.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "string",
                            value = new[] { EmployeeID.ToString() }
                        },
                        field = "ExpertEmployee.ID"
                    });
                }
                if(filter !=null)
                foreach (var item in filter)
                {
                    _filter.Add(item);
                }

                string query = FilterUtilityService.GenerateFilterHQLQuery(_filter, "SupportExpertDispatch", sort);

                Response<SupportExpertDispatch> supportExpertDispatch = _supportExpertDispatchRepository.FindAll(query,index,count);

                

                
                response.data = supportExpertDispatch.data.ConvertToSupportOwnViews();

                foreach (var item in supportExpertDispatch.data)
                {
                    foreach (var _item in response.data)
                    {
                        if(item.ID==_item.ID)
                            if (item.Support.Customer.Sales.Where(x => x.ProductSaleDetails.Where(v => v.IsRollbackDetail != true).Where(v => v.Delivered != true).Count()>0).Count() > 0)
                            {
                                _item.HasNotDeliveredProducts = true;
                            }
                    }
                }
                response.totalCount = supportExpertDispatch.totalCount;

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GetGeneralResponse<SupportView> GetOneSupport(Guid SupportID)
        {
            GetGeneralResponse<SupportView> response = new GetGeneralResponse<SupportView>();

            try
            {

                Support supports = _supportRepository.FindBy(SupportID);

                response.data = supports.ConverttoSupportView();
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        public GetGeneralResponse<IEnumerable<SupportView>> GetSupports(int pageSize, int pageNumber,
            Guid customerID, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<SupportView>> response = new GetGeneralResponse<IEnumerable<SupportView>>();

            try
            {
                Query query=new Query();
                Criterion CustomerIDCriteria=new Criterion("Customer.ID",customerID,CriteriaOperator.Equal);
                query.Add(CustomerIDCriteria);

                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Support> supports = _supportRepository.FindByQuery(query, sort);

                response.data = supports.data.ConvertToSupportViews();
                response.totalCount = supports.totalCount;
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        #endregion

        #endregion

        #region Add

        public GeneralResponse AddSupport(AddSupportRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Support support = new Support();

                support.ID = Guid.NewGuid();
                support.SupportTitle = request.SupportTitle;
                support.SupportComment = request.SupportComment;
                support.Confirmed = request.Confirmed;
                support.CreateDate = PersianDateTime.Now;
                support.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                support.Customer = _customerRepository.FindBy(request.CustomerID);
                support.RowVersion = 1;
                support.Confirmed = true;
                support.CreateBy = (Support.Creator)request.CreateBy;
                support.SupportStatus = _supportStatusRepository.FindAll().Where(x => x.Key == "NoStatus").FirstOrDefault();

                int SupportCount = support.Customer.Supports.Where(x => x.Closed == false).Count();

                if (SupportCount > 0)
                {
                    response.ErrorMessages.Add("تا زمانی که یک پشتیبانی باز وجود دارد امکان افزودن پشتیبانی جدید وجود ندارد");
                    return response;
                }

                _supportRepository.Add(support);
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

        public GeneralResponse EditSupport(EditSupportRequest request, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Support support = new Support();

                support = _supportRepository.FindBy(request.ID);
                support.SupportComment = request.SupportComment;
                support.SupportTitle = request.SupportTitle;
                support.Confirmed = request.Confirmed;
                support.ModifiedDate = PersianDateTime.Now;
                support.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

                #region Row version Check

                if (support.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    support.RowVersion += 1;
                }

                if (support.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in support.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _supportRepository.Save(support);
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

        public GeneralResponse DeleteSupport(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Support support = new Support();
                support = _supportRepository.FindBy(request.ID);

                _supportRepository.Remove(support);
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

        #region Report

        public InstallFormReportView GetInstallReport(Guid SupportID)
        {
            InstallFormReportView response = new InstallFormReportView();

            Query query = new Query();
            Criterion SupportIDCriteria = new Criterion("ID", SupportID, CriteriaOperator.Equal);
            query.Add(SupportIDCriteria);

            Support support = _supportRepository.FindBy(SupportID);

            #region Preparing Data

            response.ADSLPhone = support.Customer.ADSLPhone;
            response.Balance = support.Customer.Balance;
            response.CustomerName = support.Customer.Name;
            response.Alias = support.Customer.Network!=null?support.Customer.Network.Alias:"نامشخص";
            response.Address = support.Customer.Address;
            IEnumerable<ProductSaleDetailView >cus= _saleService.GetUnDeliveredProducts(support.Customer.ID).data;
            if(cus !=null)
            response.HasUnDeliveredProducts = cus.Count() > 0 ? true : false;
            if (support.SupportExpertDispatch != null)
            {
                response.InstallDate = support.SupportExpertDispatch.FirstOrDefault().DispatchDate;
                response.InstallTime = support.SupportExpertDispatch.FirstOrDefault().DispatchTime;
            
                response.InstallerName = support.SupportExpertDispatch.FirstOrDefault().ExpertEmployee.Name;
            }
            response.NetworkName = support.Customer.Network!=null?support.Customer.Network.NetworkName:"نامشخص";
            response.NetworkNote = support.Customer.Network != null ? support.Customer.Network.Note : "";

            #endregion

            return response;

        }

        #endregion
    }
}