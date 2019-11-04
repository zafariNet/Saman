
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net.Cache;
using System.Threading;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;

namespace Services.Implementations
{
    public class QueueLocalPhoneStoreService : IQueueLocalPhoneStoreService
    {
        #region Declar

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IQueueRepository _queueRepository;

        private readonly ILocalPhoneStoreRepository _localPhoneStoreRepository;

        private readonly IQueueLocalPhoneStoreRepository _queueLocalPhoneStoreRepository;

        private readonly ILocalPhoneStoreEmployeeRepository _localPhoneStoreEmployeeRepository;

        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public QueueLocalPhoneStoreService(IEmployeeRepository employeeRepository, IQueueRepository queueRepository,
            ILocalPhoneStoreRepository localPhoneStoreRepository, IQueueLocalPhoneStoreRepository queueLocalPhoneStoreRepository, ILocalPhoneStoreEmployeeRepository localPhoneStoreEmployeeRepository, IUnitOfWork uow)
        {
            _employeeRepository = employeeRepository;
            _queueRepository = queueRepository;
            _localPhoneStoreRepository = localPhoneStoreRepository;
            _queueLocalPhoneStoreRepository = queueLocalPhoneStoreRepository;
            _localPhoneStoreEmployeeRepository = localPhoneStoreEmployeeRepository;
            _uow = uow;
        }

        #endregion

        #region Read All


        public GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>> GetAllQueueLocalPhones(int pageSize,
            int pageNumber,
            IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>> response=new GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "QueueLocalPhoneStore", sort);
                Response<QueueLocalPhoneStore> queueLocalPhoneStores = _queueLocalPhoneStoreRepository.FindAll(query,
                    index, count);

                response.data = queueLocalPhoneStores.data.ConvertToQueueLocalPhoneViews();
                response.totalCount = queueLocalPhoneStores.totalCount;
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

        #region Read By Employee

        public GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>> GetqueueLocalPhoneStoreByEmployee(
            Guid EmployeeID)
        {
            GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>> response=new GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>>();

            try
            {

                IEnumerable<QueueLocalPhoneStore> queueLocalPhoneStores = _queueLocalPhoneStoreRepository.FindAll().Where(x=>x.OwnerEmployee.ID==EmployeeID);

                response.data = queueLocalPhoneStores.ConvertToQueueLocalPhoneViews();
                response.totalCount = queueLocalPhoneStores.Count();

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

        public GeneralResponse AddQueueLocalPhoneStore(IEnumerable<AddQueueLocalPhoneRequest> requests, Guid OwnerEmployeeID,
            Guid EmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    QueueLocalPhoneStore queueLocalPhoneStore=new QueueLocalPhoneStore();
                    queueLocalPhoneStore.ID = Guid.NewGuid();
                    queueLocalPhoneStore.CreateDate = PersianDateTime.Now;
                    queueLocalPhoneStore.CreateEmployee = _employeeRepository.FindBy(EmployeeID);
                    queueLocalPhoneStore.DangerousRing = request.DangerousRing;
                    queueLocalPhoneStore.DangerousSeconds = request.DangerousSeconds;
                    queueLocalPhoneStore.OwnerEmployee =
                        _employeeRepository.FindBy(OwnerEmployeeID);
                    queueLocalPhoneStore.Queue = _queueRepository.FindBy(request.QueueID);
                    queueLocalPhoneStore.RowVersion = 1;
                    queueLocalPhoneStore.SendSmsToOffLineUserOnDangerous = request.SendSmsToOffLineUserOnDangerous;
                    queueLocalPhoneStore.SendSmsToOnLineUserOnDangerous = request.SendSmsToOnLineUserOnDangerous;
                    queueLocalPhoneStore.SmsText = request.Smstext;
                    queueLocalPhoneStore.CanViewQueue = request.CanViewQueue;

                    _queueLocalPhoneStoreRepository.Add(queueLocalPhoneStore);

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

        public GeneralResponse DeleteQueueLocalPhoneStore(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    QueueLocalPhoneStore queueLocalPhoneStore=new QueueLocalPhoneStore();
                    queueLocalPhoneStore = _queueLocalPhoneStoreRepository.FindBy(request.ID);

                    _queueLocalPhoneStoreRepository.Remove(queueLocalPhoneStore);
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

        #region Get Main Info

        public GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreInfoView1>> GetQueueEmployee(string QueueName,int QueueCount)
        {
            GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreInfoView1>> response = new GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreInfoView1>>();

            //IEnumerable<Queue> queues = _queueRepository.FindAll();
            //IEnumerable<QueueLocalPhoneStore> queueLocalPhoneStores = _queueLocalPhoneStoreRepository.FindAll();
            //IList<QueueLocalPhoneStoreInfoView1> list=new List<QueueLocalPhoneStoreInfoView1>();
            //IEnumerable<Employee> employees = _employeeRepository.FindAll();
            //foreach (var employee in employees)
            //{
                QueueLocalPhoneStoreInfoView1 queueLocalPhoneStoreInfoView1=new QueueLocalPhoneStoreInfoView1();
                
            //    IList<SimpleEmployeeView> myList=new List<SimpleEmployeeView>();

            //    SimpleEmployeeView simpleEployee = new SimpleEmployeeView()
            //    {
            //        FirstName = employee.FirstName,
            //        LastName = employee.LastName,
            //        ID = employee.ID
            //    };

            //    queueLocalPhoneStoreInfoView.OwnerEmployee = simpleEployee;



            //    queueLocalPhoneStoreInfoView.Queues =
            //        _queueLocalPhoneStoreRepository.FindAll().Where(x=>x.OwnerEmployee==employee).ConvertToQueueLocalPhoneViews();

            //    list.Add(queueLocalPhoneStoreInfoView);
            //}

            //response.data = list;
            //response.totalCount = list.Count();


            var con = ConfigurationManager.ConnectionStrings["SamanCnn"].ToString();
            IList<QueueLocalPhoneStoreInfoView> QueueLocalPhoneStoreInfoViews=new List<QueueLocalPhoneStoreInfoView>();
            
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string oString =
                    "select t1.EmployeeID ,t1.Mobile,t3.QueueName,t2.DangerousRing,t2.SendSmsToOffLineUserOnDangerous,t2.SendSmsToOnLineUserOnDangerous,t2.SmsText,t2.CanViewQueue from emp.Employee t1 inner join emp.QueueLocalPhoneStore t2 on t1.EmployeeID=t2.OwnerEmployeeID inner join emp.Queue t3 on t2.QueueID=t3.QueueID where t3.QueueName=@QueueName";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                oCmd.Parameters.AddWithValue("@QueueName", QueueName);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        QueueLocalPhoneStoreInfoView queueLocalPhoneStoreInfoView = new QueueLocalPhoneStoreInfoView();
                        queueLocalPhoneStoreInfoView.EmployeeID = Guid.Parse(oReader["EmployeeID"].ToString());
                        queueLocalPhoneStoreInfoView.Mobile = oReader["Mobile"].ToString();
                        queueLocalPhoneStoreInfoView.QueueName = oReader["QueueName"].ToString();
                        queueLocalPhoneStoreInfoView.DangerousRing =int.Parse(oReader["DangerousRing"].ToString());
                        queueLocalPhoneStoreInfoView.SendSmsToOffLineUserOnDangerous = Boolean.Parse(oReader["SendSmsToOffLineUserOnDangerous"].ToString());
                        queueLocalPhoneStoreInfoView.SendSmsToOffLineUserOnDangerous = Boolean.Parse(oReader["SendSmsToOffLineUserOnDangerous"].ToString());
                        queueLocalPhoneStoreInfoView.SmsText = oReader["SmsText"].ToString();
                        queueLocalPhoneStoreInfoView.QueueName = oReader["CanViewQueue"].ToString();
                        QueueLocalPhoneStoreInfoViews.Add(queueLocalPhoneStoreInfoView);

                    }
                    myConnection.Close();
                }
            }
            IList<QueueLocalPhoneStoreInfoView1> list=new List<QueueLocalPhoneStoreInfoView1>();
            foreach (var item in QueueLocalPhoneStoreInfoViews)
            {
                QueueLocalPhoneStoreInfoView1 _item=new QueueLocalPhoneStoreInfoView1();
                _item.EmployeeID = item.EmployeeID;
                if (QueueCount >= item.DangerousRing)
                {

                    
                    //_item.ViewAlarm = true;
                    //item.SendSMS = true;
                    //string smsBody = item.SmsText;

                    //// Threading
                    //SmsData smsData = new SmsData() { body = smsBody, phoneNumber = item.Mobile };
                    //Thread oThread = new Thread(SendSmsVoid);
                    //oThread.Start(smsData);
                }
                else
                {
                    _item.ViewAlarm = false;
                }

                list.Add(_item);
            }
            response.data = list;
            response.totalCount = list.Count();


            return response;
        }

        #endregion

        #region Send SMS

        public void SendSmsVoid(object data)
        {
            ISmsWebService smsWebService = new ISmsWebService();
            SmsData smsData = (SmsData)data;
            smsWebService.SendSms(smsData.body, smsData.phoneNumber);
        }




        #endregion
    }
}
