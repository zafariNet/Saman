using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Caching;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Customers.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Model.Sales;
using Model.Sales.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.SaleCatalogService;
using Services.ViewModels.Sales;

namespace Services.Implementations
{
    public class CourierService:ICourierService
    {

        #region Declare

        private readonly ICustomerRepository _customerRepository;

        private readonly ICourierEmployeeRepository _courierEmployeeRepository;

        private readonly ICourierRepository _courierRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ISaleRepository _saleRepository;

        private readonly IBonusComissionRepository _bonusComissionRepository;

        private readonly IProductSaleDetailRepository _productSaleDetailRepository;

        private readonly ICreditSaleDetailRepository _creditSaleDetailRepository;

        private readonly IUncreditSaleDetailRepository _uncreditSaleDetailRepository;

        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public CourierService(ICustomerRepository customerRepository, ICourierEmployeeRepository courierEmployeeRepository,
            ICourierRepository courierRepository, IEmployeeRepository employeeRepository, ISaleRepository saleRepository, IUnitOfWork uow,
            IBonusComissionRepository bonusComissionRepository, IProductSaleDetailRepository productSaleDetailRepository, ICreditSaleDetailRepository creditSaleDetailRepository, IUncreditSaleDetailRepository uncreditSaleDetailRepository)
        {
            _customerRepository = customerRepository;
            _courierEmployeeRepository = courierEmployeeRepository;
            _courierRepository = courierRepository;
            _employeeRepository = employeeRepository;
            _saleRepository = saleRepository;
            _bonusComissionRepository = bonusComissionRepository;
            _productSaleDetailRepository = productSaleDetailRepository;
            _creditSaleDetailRepository = creditSaleDetailRepository;
            _uncreditSaleDetailRepository = uncreditSaleDetailRepository;
            _uow = uow;
        }

        #endregion

        #region Read

        #region All

        public GetGeneralResponse<IEnumerable<CourierView>> GetAllCouriers(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<CourierView>> response=new GetGeneralResponse<IEnumerable<CourierView>>();

            try
            {
                int index = (pageNumber - 1)*pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Courier", sort);
                Response<Courier> couriers = _courierRepository.FindAll(query, index, count);


                foreach (var item in couriers.data)
                {
                    long Bonus = 0;
                    if (item.Sale.UncreditSaleDetails.Any())
                        Bonus += item.Sale.UncreditSaleDetails.Sum(x => x.Bonus);
                    if (item.Sale.CreditSaleDetails.Any())
                        Bonus += item.Sale.CreditSaleDetails.Sum(x => x.Bonus);
                    if (item.Sale.ProductSaleDetails.Any())
                        Bonus += item.Sale.ProductSaleDetails.Sum(x => x.Bonus);
                    item.Bonus = Bonus;
                }
                response.data = couriers.data.ConvertToCourierViews();
                response.totalCount = couriers.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }


        public GetGeneralResponse<IEnumerable<CourierView>> GetAllCouriersByEmployee(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort,Guid EmployeeID)
        {
            GetGeneralResponse<IEnumerable<CourierView>> response = new GetGeneralResponse<IEnumerable<CourierView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                IList <FilterData > _filter = new List<FilterData>();
                if(filter!=null)
                foreach (var filterData in filter)
                {
                    _filter.Add(filterData);
                }

                #region Add Customer Filter

                _filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "list",
                        value = new[] { EmployeeID .ToString()}
                    },
                    field = "CreateEmployee.ID"
                    
                });

                #endregion

                string query = FilterUtilityService.GenerateFilterHQLQuery(_filter, "Courier", sort);
                Response<Courier> couriers = _courierRepository.FindAll(query, index, count);

                foreach (var item in couriers.data)
                {
                    long Bonus=0;
                    if (item.Sale.UncreditSaleDetails.Any())
                        Bonus += item.Sale.UncreditSaleDetails.Sum(x=>x.Bonus);
                    if(item.Sale.CreditSaleDetails.Any())
                        Bonus += item.Sale.CreditSaleDetails.Sum(x => x.Bonus);
                    if (item.Sale.ProductSaleDetails.Any())
                        Bonus += item.Sale.ProductSaleDetails.Sum(x => x.Bonus);
                    item.Bonus = Bonus;
                }

                response.data = couriers.data.ConvertToCourierViews();
                response.totalCount = couriers.totalCount;
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

        #region One

        public GetGeneralResponse<IEnumerable<CourierView>> GetCstomerCouriers(int pageSize, int pageNumber, Guid CustomerID)
        {
            GetGeneralResponse<IEnumerable<CourierView>> response = new GetGeneralResponse<IEnumerable<CourierView>>();

            try
            {
                Query query=new Query();
                Criterion CustomerIDCretieria = new Criterion("Sale.Customer.ID", CustomerID, CriteriaOperator.Equal);
                query.Add(CustomerIDCretieria);
                
                int index = (pageNumber - 1)*pageSize;
                int count = pageSize;
                IQueryable<Courier> couriersQueryable = _courierRepository.Queryable().Where(x=>x.Sale.Customer.ID==CustomerID);
                IEnumerable<Courier> couriers = couriersQueryable.Skip(index).Take(count).ToList();

                response.data = couriers.ConvertToCourierViews();
                response.totalCount = couriers.Count();
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

        #endregion

        #region Edit

        public GeneralResponse DoCourierAction(Guid CourierID, int CourierStatuse, string ExpertComment,Guid CourierEmployeeID,Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                
                Courier courier = new Courier();
                courier = _courierRepository.FindBy(CourierID);
                if (courier.CourierStatuse == Courier.CourierStatuses.Confirmed)
                {
                    response.ErrorMessages.Add("این اعزام پیک تایید شده است.  لذا دیگر قادر به تغییر وضعیت آن نیستید");
                    return response;
                }

                courier.ExpertComment = ExpertComment;
                courier.CourierStatuse = (Courier.CourierStatuses) CourierStatuse;
                courier.ModifiedDate = PersianDateTime.Now;
                courier.ModifiedEmployee = _employeeRepository.FindBy((ModifiedEmployeeID));
                courier.CourierEmployee = _courierEmployeeRepository.FindBy(CourierEmployeeID);

                #region Add Courier To Bonus An Comissions

                if ((Courier.CourierStatuses) CourierStatuse == Courier.CourierStatuses.Confirmed)
                {
                    //برای خدمات غیر اعتباری
                    foreach (var item in courier.Sale.UncreditSaleDetails.Where(x=>x.Rollbacked==false))
                    {
                        Query query = new Query();
                        Criterion UncreditSaleDetailID = new Criterion("ID", item.ID,
                            CriteriaOperator.Equal);
                        query.Add(UncreditSaleDetailID);
                        UncreditSaleDetail unCreditSaleDetail = _uncreditSaleDetailRepository.FindBy(query).FirstOrDefault();
                        if (unCreditSaleDetail != null)
                        {
                            unCreditSaleDetail.BonusDate = PersianDateTime.Now;

                            _uncreditSaleDetailRepository.Save(unCreditSaleDetail);
                        }
                    }

                    //برای خدمات  اعتباری
                    foreach (var item in courier.Sale.CreditSaleDetails.Where(x=>x.Rollbacked==false))
                    {
                        Query query = new Query();
                        Criterion UncreditSaleDetailID = new Criterion("ID", item.ID,
                            CriteriaOperator.Equal);
                        query.Add(UncreditSaleDetailID);
                        CreditSaleDetail creditSaleDetail = _creditSaleDetailRepository.FindBy(query).FirstOrDefault();
                        if (creditSaleDetail != null)
                        {
                            creditSaleDetail.BonusDate = PersianDateTime.Now;
                            _creditSaleDetailRepository.Save(creditSaleDetail);
                        }
                    }

                    //برای کالاها
                    foreach (var item in courier.Sale.ProductSaleDetails.Where(x => x.Rollbacked == false))
                    {
                        Query query = new Query();
                        Criterion UncreditSaleDetailID = new Criterion("ID", item.ID,
                            CriteriaOperator.Equal);
                        query.Add(UncreditSaleDetailID);
                        ProductSaleDetail productsaleDetail = _productSaleDetailRepository.FindBy(query).FirstOrDefault();
                        if (productsaleDetail != null)
                        {
                            productsaleDetail.BonusDate = PersianDateTime.Now;
                            _productSaleDetailRepository.Save(productsaleDetail);
                        }
                    }
                }

                #endregion

                _courierRepository.Save(courier);

                _uow.Commit();

            }
            catch (Exception ex )
            {

                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GeneralResponse EditCourier(EditCourierRequest request, Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();
            
            try
            {
                Courier courier=new Courier();
                courier = _courierRepository.FindBy(request.ID);
                if (courier.CourierStatuse == Courier.CourierStatuses.Confirmed ||
                    courier.CourierStatuse == Courier.CourierStatuses.NotConfirmed)
                {
                    response.ErrorMessages.Add(
                        "این اعزام پیک قبلا تایید و یا رد شده است. لذا ویرایش آن امکان پذیر نمیباشد");
                }

                courier.ModifiedDate = PersianDateTime.Now;
                courier.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                courier.CourierStatuse = Courier.CourierStatuses.Pending;
                courier.Amount = request.Amount;
                courier.BuildingUnits = request.BuildingUnits;
                courier.CourierCost = request.CourierCost;
                courier.CourierEmployee = null;
               
                courier.CourierType = (Courier.CourierTypes) request.CourierType;
                courier.DeliverDate = request.DeliverDate;
                courier.DeliverTime = request.DeliverTime;
                courier.SaleComment = request.SaleComment;

                #region Row Version Check

                if (courier.RowVersion != request.RowVersion)
                {

                    response.ErrorMessages.Add("EditConcurrencyKey");
                    return response;
                }
                else
                {
                    courier.RowVersion += 1;
                }

                if (courier.GetBrokenRules().Count() > 0)
                {

                    foreach (BusinessRule businessRule in courier.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }


                #endregion

                _courierRepository.Save(courier);
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

        #region Add

        public GeneralResponse AddCourier(AddCourierRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Courier courier = new Courier();
                courier.ID = Guid.NewGuid();
                courier.CreateDate = PersianDateTime.Now;
                courier.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                courier.Amount = request.Amount;
                courier.BuildingUnits = request.BuildingUnits;
                courier.CourierCost = request.CourierCost;
                courier.CourierEmployee = null;
                courier.CourierStatuse = Courier.CourierStatuses.Pending;
                courier.CourierType = (Courier.CourierTypes)request.CourierType;
                courier.DeliverDate = request.DeliverDate;
                courier.DeliverTime = request.DeliverTime;
                courier.SaleComment = request.SaleComment;
                courier.Sale = _saleRepository.FindBy(request.SaleID);
                courier.RowVersion = 1;

                Sale sale = _saleRepository.FindBy(request.SaleID);

                #region Getting Sale Details

                
                //Query query = new Query();

                //Criterion SaleIDCriteria = new Criterion("Sale.ID", sale.ID, CriteriaOperator.Equal);
                //query.Add(SaleIDCriteria);

                //ProductSaleDetail productSaleDetail = _productSaleDetailRepository.FindBy(query).FirstOrDefault();
                //CreditSaleDetail creditSaleDetail = _creditSaleDetailRepository.FindBy(query).FirstOrDefault();
                //UncreditSaleDetail uncreditSaleDetail = _uncreditSaleDetailRepository.FindBy(query).FirstOrDefault();

                //long  bonus = 0;
                //if (creditSaleDetail != null)
                //{

                //    Query query1 = new Query();

                //    Criterion SaleIDCriteria1 = new Criterion("CreditSaleDetail.ID", creditSaleDetail.ID, CriteriaOperator.Equal);
                //    query1.Add(SaleIDCriteria1);
                //    BonusComission temp= _bonusComissionRepository.FindBy(query1).FirstOrDefault();
                //    bonus += temp.Bonus;
                //}
                //if (productSaleDetail != null)
                //{
                //    Query query1 = new Query();

                //    Criterion SaleIDCriteria1 = new Criterion("ProductSaleDetail.ID", productSaleDetail.ID, CriteriaOperator.Equal);
                //    query1.Add(SaleIDCriteria1);
                //    BonusComission temp = _bonusComissionRepository.FindBy(query1).FirstOrDefault();
                //    bonus += temp.Bonus;
                //}
                //if (uncreditSaleDetail != null)
                //{
                //    Query query1 = new Query();
                //    Criterion SaleIDCriteria1 = new Criterion("UnCreditSaleDetail.ID", uncreditSaleDetail.ID, CriteriaOperator.Equal);
                //    query1.Add(SaleIDCriteria1);
                //    BonusComission temp = _bonusComissionRepository.FindBy(query1).FirstOrDefault();
                //    bonus += temp.Bonus;
                //}

                #endregion

                //courier.Bonus = bonus;
                courier.Sale.Couriers = courier;
                courier.Sale.HasCourier = true;
                _courierRepository.Add(courier);

                
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

        public GeneralResponse DeleteCourier(DeleteRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                Courier courier=new Courier();

                courier = _courierRepository.FindBy(request.ID);
                if (courier.CourierStatuse == Courier.CourierStatuses.Confirmed || courier.CourierStatuse == Courier.CourierStatuses.NotConfirmed)
                {
                    response.ErrorMessages.Add("این اعزام پیک قبلا تایید و یا رد شده است. لذا حذف آن امکان پذیر نمیباشد");
                    return response;
                }
                _courierRepository.Remove(courier);
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

    }
}
