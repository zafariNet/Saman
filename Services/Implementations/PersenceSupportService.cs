#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Support.Interfaces;
using Model.Employees.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Model.Support;
using Services.ViewModels.Support;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Infrastructure.Querying;
using Infrastructure.Domain;

#endregion

namespace Services.Implementations
{
    public class PersenceSupportService : IPersenceSupportService
    {
        #region Declares
        
        private readonly IPersenceSupportRepository _persenceSupportRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _uow;
        
        #endregion

        #region Ctor
        
        public PersenceSupportService(IPersenceSupportRepository persenceSupportRepository, IUnitOfWork uow)
        {
            _persenceSupportRepository = persenceSupportRepository;
            _uow = uow;
        }

        public PersenceSupportService(IPersenceSupportRepository persenceSupportRepository, ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository, IUnitOfWork uow)
            : this(persenceSupportRepository, uow)
        {
            this._customerRepository = customerRepository;
            this._employeeRepository = employeeRepository;
        }
        
        #endregion

        #region Old Methods
        
        public GeneralResponse AddPersenceSupport(AddPersenceSupportRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                PersenceSupport persenceSupport = new PersenceSupport();
                persenceSupport.ID = Guid.NewGuid();
                persenceSupport.CreateDate = PersianDateTime.Now;
                persenceSupport.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                persenceSupport.ConnectedToInternet = request.ConnectedToInternet;
                persenceSupport.Customer = this._customerRepository.FindBy(request.CustomerID);
                persenceSupport.PlanDate = request.PlanDate;
                persenceSupport.PlanNote = request.PlanNote;
                persenceSupport.PlanTimeFrom = request.PlanTimeFrom;
                persenceSupport.PlanTimeTo = request.PlanTimeTo;
                persenceSupport.Problem = request.Problem;
                persenceSupport.SupportType = request.SupportType;
                persenceSupport.RowVersion = 1;
                persenceSupport.Installer = _employeeRepository.FindBy(request.InstallerID);

                #region Validation

                if (persenceSupport.GetBrokenRules().Count() > 0)
                {
                    foreach (BusinessRule businessRule in persenceSupport.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _persenceSupportRepository.Add(persenceSupport);
                _uow.Commit();

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GeneralResponse DeliverPersenceSupport(Guid PersenceSupportID, Guid InstallerID, bool Delivered, string DeliverDate, string DeliverNote, string DeliverTime, long ReceivedCost, int RowVersion)
        {
            GeneralResponse response = new GeneralResponse();
            PersenceSupport persenceSupport = new PersenceSupport();
            persenceSupport = _persenceSupportRepository.FindBy(PersenceSupportID);

            if (persenceSupport != null)
            {
                try
                {
                    // اگر قبلاً تحویل شده بود انجام نشود
                    if (persenceSupport.Delivered)
                    {
                        response.ErrorMessages.Add("DeliveredAndCanNotBeDeliveredAgain");
                        return response;
                    }

                    persenceSupport.DeliverDate = DeliverDate;
                    persenceSupport.Delivered = Delivered;
                    persenceSupport.DeliverNote = DeliverNote;
                    persenceSupport.DeliverTime = DeliverTime;
                    persenceSupport.Installer = this._employeeRepository.FindBy(InstallerID);
                    persenceSupport.ReceivedCost = ReceivedCost;

                    #region RowVersion Check

                    if (persenceSupport.RowVersion != RowVersion)
                    {

                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        persenceSupport.RowVersion += 1;
                        response.rowVersion = persenceSupport.RowVersion;
                    }

                    #endregion

                    #region Validation

                    if (persenceSupport.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in persenceSupport.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _persenceSupportRepository.Save(persenceSupport);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }

        public GeneralResponse EditPersenceSupport(EditPersenceSupportRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            PersenceSupport persenceSupport = new PersenceSupport();
            persenceSupport = _persenceSupportRepository.FindBy(request.ID);

            if (persenceSupport != null)
            {
                try
                {
                    // اگر تحویل شده بود قابل ویرایش نباشد
                    if (persenceSupport.Delivered)
                    {
                        response.ErrorMessages.Add("DeliveredAndCanNotBeEdited");
                        return response;
                    }

                    persenceSupport.ModifiedDate = PersianDateTime.Now;
                    persenceSupport.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    persenceSupport.PlanDate = request.PlanDate;
                    persenceSupport.PlanNote = request.PlanNote;
                    persenceSupport.PlanTimeFrom = request.PlanTimeFrom;
                    persenceSupport.PlanTimeTo = request.PlanTimeTo;
                    persenceSupport.Problem = request.Problem;
                    persenceSupport.SupportType = request.SupportType;
                    persenceSupport.Installer = _employeeRepository.FindBy(request.InstallerID);

                    #region RowVersion Check

                    if (persenceSupport.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        persenceSupport.RowVersion += 1;
                        response.rowVersion = persenceSupport.RowVersion;
                    }

                    #endregion

                    #region Validation

                    if (persenceSupport.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in persenceSupport.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _persenceSupportRepository.Save(persenceSupport);
                    _uow.Commit();
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

        public GeneralResponse DeletePersenceSupport(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            PersenceSupport persenceSupport = new PersenceSupport();
            persenceSupport = _persenceSupportRepository.FindBy(request.ID);

            if (persenceSupport != null)
            {
                try
                {
                    // اگر تحویل شده بود قابل حذف نباشد
                    if (persenceSupport.Delivered)
                    {
                        response.ErrorMessages.Add("DeliveredAndCanNotBeDeleted");
                        return response;
                    }

                    _persenceSupportRepository.Remove(persenceSupport);
                    _uow.Commit();

                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }

            return response;
        }

        public GetPersenceSupportResponse GetPersenceSupport(GetRequest request)
        {
            GetPersenceSupportResponse response = new GetPersenceSupportResponse();

            try
            {
                PersenceSupport persenceSupport = new PersenceSupport();
                PersenceSupportView persenceSupportView = persenceSupport.ConvertToPersenceSupportView();

                persenceSupport = _persenceSupportRepository.FindBy(request.ID);
                if (persenceSupport != null)
                    persenceSupportView = persenceSupport.ConvertToPersenceSupportView();

                response.PersenceSupportView = persenceSupportView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetPersenceSupportsResponse GetPersenceSupports()
        {
            GetPersenceSupportsResponse response = new GetPersenceSupportsResponse();

            try
            {
                IEnumerable<PersenceSupportView> persenceSupports = _persenceSupportRepository.FindAll()
                    .ConvertToPersenceSupportViews();

                response.PersenceSupportViews = persenceSupports;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        
        #endregion

        #region Get All

        public GetGeneralResponse<IEnumerable<PersenceSupportView>> GetPersenceSupports(Guid customerID, int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<PersenceSupportView>> response = new GetGeneralResponse<IEnumerable<PersenceSupportView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Customer.ID", customerID, CriteriaOperator.Equal);

                query.Add(criteria);

                Response<PersenceSupport> persenceSupports = _persenceSupportRepository.FindBy(query, index, count);

                response.data = persenceSupports.data.ConvertToPersenceSupportViews();
                response.totalCount = persenceSupports.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }


            return response;
        }
        /// <summary>
        /// فیتر کردن پشتیبانی های حضوری
        /// </summary>
        /// <param name="CreateEmployee"></param>
        /// <param name="CustomerID"></param>
        /// <param name="SupportType"></param>
        /// <param name="CreateDate"></param>
        /// <param name="PlanDate"></param>
        /// <param name="EndPlanDate"></param>
        /// <param name="Installer"></param>
        /// <param name="Deliverd"></param>
        /// <param name="DeliverDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <returns></returns>
        public GetGeneralResponse<IEnumerable<PersenceSupportView>> GetCustomizedPersenceSupports(Guid? CustomerID, Guid? CreateEmployeeID, int? SupportType, 
            string StartCreateDate, string EndCreateDate, string StartPlanDate, string EndPlanDate, Guid? Installer, bool? Deliverd, string StartDeliverDate, 
            string EndDeliverDate, int PageSize, int PageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<PersenceSupportView>> response = new GetGeneralResponse<IEnumerable<PersenceSupportView>>();

            try
            {
                int index = (PageNumber - 1) * PageSize;
                int count = PageSize;

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                if (CreateEmployeeID != Guid.Empty)
                {
                    Criterion criteriaCreateEmployeeID = new Criterion("CreateEmployee.ID", CreateEmployeeID, CriteriaOperator.Equal);
                    query.Add(criteriaCreateEmployeeID);
                }

                if (CustomerID != Guid.Empty)
                {
                    Criterion criteriaCustomerID = new Criterion("Customer.ID", CustomerID, CriteriaOperator.Equal);
                    query.Add(criteriaCustomerID);
                }

                if (SupportType != 0)
                {
                    Criterion criteria = new Criterion("SupportType", (Int16)SupportType, CriteriaOperator.Equal);
                    query.Add(criteria);
                }
                if (StartCreateDate != null)
                {
                    Criterion criteriaStartCreateDate = new Criterion("CreateDate", StartCreateDate, CriteriaOperator.GreaterThanOrEqual);
                    query.Add(criteriaStartCreateDate);
                }
                if (EndCreateDate != null)
                {
                    Criterion criteriaEndCreateDate = new Criterion("CreateDate", EndCreateDate, CriteriaOperator.LesserThanOrEqual);
                    query.Add(criteriaEndCreateDate);
                }

                if (StartPlanDate != null)
                {
                    Criterion criteriaStartPlanDate = new Criterion("PlanDate", StartPlanDate, CriteriaOperator.GreaterThanOrEqual);
                    query.Add(criteriaStartPlanDate);
                }

                if (EndPlanDate != null)
                {
                    Criterion criteriaEndPlanDate = new Criterion("PlanDate", EndPlanDate, CriteriaOperator.LesserThanOrEqual);
                    query.Add(criteriaEndPlanDate);
                }
                if (Installer != Guid.Empty)
                {
                    Criterion criteriaInstaller = new Criterion("Installer.ID", Installer, CriteriaOperator.Equal);
                    query.Add(criteriaInstaller);
                }

                if (Deliverd != null)
                {
                    Criterion criteriaDeliverd = new Criterion("Deliverd", Convert.ToBoolean(Deliverd), CriteriaOperator.Equal);
                    query.Add(criteriaDeliverd);
                }
                if (StartDeliverDate != null)
                {
                    Criterion criteriaStartDeliverDate = new Criterion("DeliverDate", StartDeliverDate, CriteriaOperator.GreaterThanOrEqual);
                    query.Add(criteriaStartDeliverDate);
                }
                if (EndDeliverDate != null)
                {
                    Criterion criteriaEndDeliverDate = new Criterion("DeliverDate", EndDeliverDate, CriteriaOperator.LesserThanOrEqual);
                    query.Add(criteriaEndDeliverDate);
                }
                Response<PersenceSupport> persenceSupports = _persenceSupportRepository.FindBy(query, index, count,sort);

                response.data = persenceSupports.data.ConvertToPersenceSupportViews();
                response.totalCount = persenceSupports.data.Count();

                return response;

            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        #endregion

    }
}
