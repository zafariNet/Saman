using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Messaging;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Services.Mapping;
using Services.Messaging.EmployeeCatalogService;
using Infrastructure.Persian;

namespace Services.Implementations
{
    public class NotificationService:INotificationService
    {

        #region Declare

        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly INotificationRepository _notificationRepository;

        #endregion

        #region Ctor

        public NotificationService(IUnitOfWork uow,IEmployeeRepository employeeRepository,IGroupRepository groupRepository
            ,INotificationRepository notificationRepository)
        {
            _uow = uow;
            _employeeRepository = employeeRepository;
            _groupRepository = groupRepository;
            _notificationRepository = notificationRepository;
        }

        #endregion

        #region Read By Employee

        public GetGeneralResponse<IEnumerable<NotificationView>> NotificationReadByEmployee(Guid ReferedEmployeeID,int pageSize,int pageNumber,IList<FilterData> filter,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<NotificationView>> response = new GetGeneralResponse<IEnumerable<NotificationView>>();

            IList<FilterData> Filter = new List<FilterData>();

            if(filter!=null)
                foreach(var item in filter)
                    Filter.Add(item);

            Filter.Add(new FilterData()
            {
                data = new data()
                {
                    comparison = "eq",
                    type = "string",
                    value = new[] { ReferedEmployeeID.ToString() }
                },
                field = "ReferedEmployee.ID"
            });

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(Filter, "Notification", sort);

                Response<Notification> notifications = _notificationRepository.FindAll(query, index, count);

                response.data = notifications.data.ConvertToNotificationViews();
                response.totalCount = notifications.totalCount;
            }

           catch(Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region get Notification

        public GetGeneralResponse<IEnumerable<NotificationView>> GetNotifications(IList<FilterData> filter, IList<Sort> sort, int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<NotificationView>> response = new GetGeneralResponse<IEnumerable<NotificationView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Notification", sort);

                Response<Notification> notifications = _notificationRepository.FindAll(query);

                response.data = notifications.data.ConvertToNotificationViews();
                response.totalCount = notifications.totalCount;
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

        public GeneralResponse AddNotification(AddNotificationRequest request,Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                if(request.ReferedEmployeeIDs!=null && request.ReferedGroupIDs!=null)
                {
                    foreach(var item in request.ReferedEmployeeIDs)
                    {
                        Guid referedEmployeeID = (Guid)item;
                        
                        Employee employee = _employeeRepository.FindBy(referedEmployeeID);

                        Notification notification = new Notification();
                        notification.ID=Guid.NewGuid();
                        notification.CreateEmployee=_employeeRepository.FindBy(CreateEmployeeID);
                        notification.ReferedEmployee = employee;
                        notification.CreateDate = PersianDateTime.Now;
                        notification.NotificationTitle = request.NotificationTitle;
                        notification.NotificationComment = request.NotificationComment;
                        notification.Visited = false;

                        _notificationRepository.Add(notification);
                    }
                    _uow.Commit();
                }

                if (request.ReferedGroupIDs != null && request.ReferedEmployeeIDs==null)
                {
                    foreach (var item in request.ReferedGroupIDs)
                    {
                        Guid referedGroupID = (Guid)item;


                        Query query = new Query();
                        Criterion ctr = new Criterion("Group.ID", referedGroupID, CriteriaOperator.Equal);
                        query.Add(ctr);


                        IEnumerable<Employee> employees = _employeeRepository.FindBy(query);

                        foreach (var _item in employees)
                        {
                            Notification notification = new Notification();
                            notification.ID = Guid.NewGuid();
                            notification.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                            notification.ReferedEmployee = _item;
                            notification.CreateDate = PersianDateTime.Now;
                            notification.NotificationTitle = request.NotificationTitle;
                            notification.NotificationComment = request.NotificationComment;
                            notification.Visited = false;

                            _notificationRepository.Add(notification);
                        }
           
                    }
                    _uow.Commit();
                }
                if (request.ReferedEmployeeIDs == null && request.ReferedGroupIDs  == null)
                {
                    IEnumerable<Employee> employees = _employeeRepository.FindAll();

                    foreach (var employee in employees.Where(x=>x.Discontinued==false))
                    {
                        Notification notification = new Notification();
                        notification.ID = Guid.NewGuid();
                        notification.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                        notification.ReferedEmployee = employee;
                        notification.CreateDate = PersianDateTime.Now;
                        notification.NotificationTitle = request.NotificationTitle;
                        notification.NotificationComment = request.NotificationComment;
                        notification.Visited = false;

                        _notificationRepository.Add(notification);
                    }
                    _uow.Commit();
                }
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

        #region Get One

        public NotificationView GetNotification(Guid NotificationID)
        {
            NotificationView response = new NotificationView();
            try
            {
                Notification notification = _notificationRepository.FindBy(NotificationID);

                notification.Visited = true;
                notification.VisitedDate = PersianDateTime.Now;

                _notificationRepository.Save(notification);
                _uow.Commit();
                response = notification.ConvertToNotificationView();
            }
            catch(Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #region Get By Creator

        public GetGeneralResponse<IEnumerable<NotificationView>> GetNotificationsByCreator(Guid EmployeeID , int pageSize,int pageNumber)
        {
            GetGeneralResponse<IEnumerable<NotificationView>> response = new GetGeneralResponse<IEnumerable<NotificationView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Query query = new Query();
                Criterion ctr = new Criterion("CreateEmployee.ID", EmployeeID, CriteriaOperator.Equal);
                query.Add(ctr);

                Response<Notification> notifications = _notificationRepository.FindBy(query,index,count);

                response.data = notifications.data.ConvertToNotificationViews();
                response.totalCount = notifications.totalCount;

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
