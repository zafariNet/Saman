using Infrastructure.Domain;
using Infrastructure.UnitOfWork;
using Microsoft.Win32;
using Model.Employees;
using Model.Employees.Interfaces;
using NHibernate.Id;
using Services.Interfaces;
using Services.Messaging;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Mapping;
using Infrastructure.Querying;
using Infrastructure.Persian;
using Services.Messaging.EmployeeCatalogService;
using Model.Customers.Interfaces;
using System.IO;

namespace Services.Implementations
{
    public class ToDoService:IToDoService
    {
        #region Declares

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IToDoRepository _toDoRepository;
        private readonly IToDoResultRepository _toDoResultRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISimpleEmployeeRepository _simpleEmployeeRepository;
        private readonly IGroupService _groupservice;
        #endregion

        #region Ctor

        public ToDoService(IEmployeeRepository employeeRepository, IToDoRepository toDoRepository, IToDoResultRepository toDoResultRepository,
            IGroupRepository groupRepository, ICustomerRepository customerRepository, IUnitOfWork uow, IGroupService groupservice, ISimpleEmployeeRepository simpleEmployeeRepository)
        {
            _employeeRepository = employeeRepository;
            _toDoRepository = toDoRepository;
            _toDoResultRepository = toDoResultRepository;
            _groupRepository = groupRepository;
            _customerRepository = customerRepository;
            _groupservice = groupservice;
            _simpleEmployeeRepository = simpleEmployeeRepository;
            _uow = uow;
        }

        #endregion

        #region ToDo Methods

        #region Read All

        public GetGeneralResponse<IEnumerable<ToDoView>> GetAllToDos(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<ToDoView>> response = new GetGeneralResponse<IEnumerable<ToDoView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<ToDo> toDos = _toDoRepository.FindAll(index, count);

                response.data = toDos.data.ConvertToToDoViews();
                response.totalCount = toDos.totalCount;


            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Read By Creator Employee

        public GetGeneralResponse<IEnumerable<ToDoView>> GetCreatorEmployeeToDos(Guid CreateEmployee, bool? PrimaryClosed, bool? SecondaryClosed, int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<ToDoView>> response = new GetGeneralResponse<IEnumerable<ToDoView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                #region Criterias

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criteriaEmployeeID = new Criterion("CreateEmployee.ID", CreateEmployee, CriteriaOperator.Equal);
                query.Add(criteriaEmployeeID);

                if (SecondaryClosed != null)
                {
                    Criterion criteriaSecondaryClosed = new Criterion("SecondaryClosed", SecondaryClosed, CriteriaOperator.Equal);
                    query.Add(criteriaSecondaryClosed);
                }

                if (PrimaryClosed != null)
                {
                    Criterion criteriaPrimaryClosed = new Criterion("PrimaryClosed", PrimaryClosed, CriteriaOperator.Equal);
                    query.Add(criteriaPrimaryClosed);
                }


                #endregion

                Response<ToDoResult> toDoResults = _toDoResultRepository.FindBy(query, -1, -1);
                if (count != -1)
                    response.data = toDoResults.data.Select(w => w.ToDo).Distinct().ConvertToToDoViews().Skip(index).Take(count);
                else
                    response.data = toDoResults.data.Select(w => w.ToDo).Distinct().ConvertToToDoViews();
                response.totalCount = response.data.Count();

                foreach (ToDoView toDoResult in response.data)
                {
                    toDoResult.Attachment = toDoResult.Attachment.Replace(@"\", "/").Substring(toDoResult.Attachment.IndexOf("data")); ;
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

        #region Read By Refered Employee

        public GetGeneralResponse<IEnumerable<ToDoView>> GetReferedEmployeeToDos(Guid CreateEmployeeID, Guid ReferedEmployeeID, int? Close,int? TaskStatusID,
            string StartDateRange,string EndDateRange,Guid CustomerID, int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<ToDoView>> response = new GetGeneralResponse<IEnumerable<ToDoView>>();

            #region Temp Code

            //IQueryable<ToDoResult> toDoViews = _toDoResultRepository.FindAll().AsQueryable();

            //if (CreateEmployeeID != Guid.Empty)
            //{
            //    toDoViews = toDoViews.Where(x => x.CreateEmployee.ID == CreateEmployeeID || x.ReferedEmployee.ID == CreateEmployeeID).AsQueryable();
            //}

            //if (ReferedEmployeeID != Guid.Empty)
            //{
            //    toDoViews = toDoViews.Where(x => x.ReferedEmployee.ID == ReferedEmployeeID).AsQueryable();
            //}



            //if (StartDateRange != "")
            //{
            //    toDoViews = toDoViews.Where(x => string.Compare(x.ToDo.StartDate.Substring(0, 10), StartDateRange) >= 0);
            //}
            //if (EndDateRange != "")
            //{
            //    toDoViews = toDoViews.Where(x => string.Compare(x.ToDo.StartDate.Substring(0, 10), EndDateRange) <= 0);
            //}
            //switch (Close)
            //{
            //    case 3:
            //        {
            //            toDoViews = toDoViews.Where(x => x.ToDo.PrimaryClosed == true);
            //            break;
            //        }
            //    case 2:
            //        {
            //            toDoViews = toDoViews.Where(x => x.ToDo.PrimaryClosed == false).Where(x => x.SecondaryClosed == true);
            //            break;
            //        }
            //    case 1:
            //        {
            //            toDoViews = toDoViews.Where(x => x.ToDo.PrimaryClosed == false).Where(x => x.SecondaryClosed == false);
            //            break;
            //        }
            //}
            //if (CustomerID != Guid.Empty)
            //{
            //    toDoViews = toDoViews.Where(x => x.ToDo.Customer.ID == CustomerID);
            //}
            //var tt = toDoViews.Distinct().ConvertToToDoResultViews();


            //response.data = toDoViews.Select(x => x.ToDo).Distinct().ConvertToToDoViews();

            #region Temp
            //try
            //{
                

                //    int index = (pageNumber - 1) * pageSize;
                //    int count = pageSize;

                //    Infrastructure.Querying.Query query = new Query();
                //    switch (Close)
                //    {
                //        case null:
                //            {
                //                break;
                //            }
                //        //case 1:
                //        //    {
                //        //        Criterion criteriaSecondaryClosed = new Criterion("ToDo.PrimaryClosed", false, CriteriaOperator.Equal);
                //        //        query.Add(criteriaSecondaryClosed);
                //        //        break;
                //        //    }
                //        case 2:
                //            {
                //                Criterion criteriaSecondaryClosed = new Criterion("SecondaryClosed", true, CriteriaOperator.Equal);
                //                query.Add(criteriaSecondaryClosed);
                //                break;
                //            }

                //    }


                //    Response<ToDoResult> toDoResults = _toDoResultRepository.FindBy(query, -1, -1);

                //    IEnumerable<ToDoResult> result = from r in toDoResults.data where r.ReferedEmployee.ID == ReferedEmployeeID || r.CreateEmployee.ID == ReferedEmployeeID select r;

                //    if (count != -1)
                //    {
                //        if (CustomerID != null)
                //            response.data = result.Select(w => w.ToDo).Distinct().ConvertToToDoViews().Skip(index).Take(count).Where(x => x.CustomerID == CustomerID);
                //        else 
                //            response.data = result.Select(w => w.ToDo).Distinct().ConvertToToDoViews().Skip(index).Take(count);

                //    }



                //    else
                //    {
                //        if (CustomerID != null)
                //        {
                //            response.data = result.Select(w => w.ToDo).Distinct().ConvertToToDoViews().Where(x => x.CustomerID == CustomerID);
                //            response.totalCount = response.data.Count();
                //        }
                //        else
                //        {
                //            response.data = toDoResults.data.Select(w => w.ToDo).Distinct().ConvertToToDoViews();
                //            response.totalCount = response.data.Count();
                //            if (CreateEmployeeID != null)
                //            {
                //                response.data = response.data.Where(x => x.CreateEmployeeID == CreateEmployeeID);
                //            }
                //        }
                //    }
                //    if (StartDateRange != "")
                //    {
                //        response.data = response.data.Where(w => string.Compare(w.StartDate.Substring(0, 10), StartDateRange) >= 0);
                //    }
                //    if (EndDateRange != "")
                //    {
                //        response.data = response.data.Where(w => string.Compare(w.StartDate.Substring(0, 10), StartDateRange) <= 0);
                //    }

                //    switch (Close)
                //    {
                //        case null:
                //            {
                //                break;
                //            }
                //        case 1:
                //            {
                //                response.data = response.data.Where(x => x.PrimaryClosed == false);
                //                break;
                //            }
                //        case 2:
                //        {

                //            response.data = response.data.Where(x => x.PrimaryClosed == false);

                //                break;
                //            }
                //        case 3:
                //            {
                //                response.data = response.data.Where(x => x.PrimaryClosed == true);

                //                break;
                //            }

                //    }


                //    // اصلاح آدرس عکسها و پاس دادن آدرس جدید

                //    foreach (ToDoView toDo in response.data)
                //    {
                //        //if (ReferedEmployeeID != null)
                //        //{
                //        //    toDo.ToDoResults =
                //        //        toDo.ToDoResults.Where(x => x.ReferedEmployeeID == ReferedEmployeeID);
                //        //}

                //        if (toDo.CreateEmployeeID == ReferedEmployeeID)
                //        {
                //            toDo.IsMine = true;
                //        }
                //        if(toDo.Attachment!=null)
                //        toDo.Attachment = toDo.Attachment.Replace(@"\", "/").Substring(toDo.Attachment.IndexOf("data")); ;
                //    }

                //    int counter=0;
                //    IList<ToDoView> finalData=new List<ToDoView>();
                //    foreach (ToDoView toDo in response.data)
                //    {
                //        if(toDo.ToDoResults.Count()>0)
                //            finalData.Add(toDo);

                //    }
                //    response.data = finalData;
                //}

                //catch (Exception ex)
                //{
                //    response.ErrorMessages.Add(ex.Message);
                //    if (ex.InnerException != null)
                //        response.ErrorMessages.Add(ex.InnerException.Message);
                //}



            //}
            #endregion

            #endregion

            #region preparing Filters

            IList<FilterData> filters = new List<FilterData>();

            if (ReferedEmployeeID == Guid.Empty)
                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "list",
                        comparison = "eq",
                        value = new[] { CreateEmployeeID.ToString() }
                    },
                    field = "ToDo.CreateEmployee.ID"

                });

            if (ReferedEmployeeID != Guid.Empty)
                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "list",
                        comparison = "eq",
                        value = new[] { CreateEmployeeID.ToString() }
                    },
                    field = "ReferedEmployee.ID"

                });

            if (Close == 1)
            {
                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "boolean",
                        comparison = "eq",
                        value = new[] { bool.FalseString }
                    },
                    field = "SecondaryClosed"

                });

                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "boolean",
                        comparison = "eq",
                        value = new[] { bool.FalseString }
                    },
                    field = "ToDo.PrimaryClosed"

                });
            }

            if (Close == 2)
            {
                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "boolean",
                        comparison = "eq",
                        value = new[] { bool.TrueString }
                    },
                    field = "SecondaryClosed"

                });

                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "boolean",
                        comparison = "eq",
                        value = new[] { bool.FalseString }
                    },
                    field = "ToDo.PrimaryClosed"

                });
            }

            if (Close == 3)
            {

                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        type = "boolean",
                        comparison = "eq",
                        value = new[] { bool.TrueString }
                    },
                    field = "ToDo.PrimaryClosed"

                });
            }

            #region Payment Date

            if (StartDateRange != null && EndDateRange == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "ToDo.StartDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "date",
                        value = new[] { StartDateRange }
                    }
                };
                filters.Add(Filter);
            }

            if (EndDateRange != null && StartDateRange == null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "ToDo.StartDate",
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "date",
                        value = new[] { EndDateRange }
                    }
                };
                filters.Add(Filter);
            }

            if (StartDateRange != null && EndDateRange != null)
            {
                FilterData Filter = new FilterData()
                {
                    field = "ToDo.StartDate",
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "date",
                        value = new[] { StartDateRange }
                    }
                };
                filters.Add(Filter);
            }
            #endregion

            string query = FilterUtilityService.GenerateFilterHQLQuery(filters, "ToDoResult", null);

            //if (StartDateRange != null && EndDateRange != null)
            //{
            //    response.data =
            //        _toDoResultRepository.FindAll(query)
            //            .data.Select(x => x.ToDo)
            //            .Distinct()
            //            .Where(x => string.Compare(x.EndDate.Substring(0, 10), EndDateRange) >= 0)
            //            .ConvertToToDoViews();
            //}

            IEnumerable<ToDoResult> res = _toDoResultRepository.FindAll(query).data;

            response.data = _toDoResultRepository.FindAll(query).data.Select(x=>x.ToDo).Distinct().ConvertToToDoViews();
            
            

            #endregion

            return response;
        }


        #endregion

        #region Read One

        public GetGeneralResponse<ToDoView> GetToDo(Guid ToDoID)
        {
            GetGeneralResponse<ToDoView> response = new GetGeneralResponse<ToDoView>();
            try
            {
                ToDo toDo = _toDoRepository.FindBy(ToDoID);
                response.data = toDo.ConvertToToDoView();
                response.totalCount = 1;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region primary Close

        public GeneralResponse PrimaryClose(Guid ToDoID, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                ToDo toDo = _toDoRepository.FindBy(ToDoID);
                if (toDo.CreateEmployee.ID != ModifiedEmployeeID)
                {
                    response.ErrorMessages.Add("شما ایجاد کننده وظیفه نمیباشید. لذا قادر به بستن نهایی این وظیفه نیستید");
                }
                foreach (ToDoResult todoResult in toDo.ToDoResults)
                {
                    if (todoResult.SecondaryClosed != true)
                    {
                        todoResult.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                        todoResult.ModifiedDate = PersianDateTime.Now;
                        todoResult.SecondaryClosed = true;
                    }
                }
                toDo.ModifiedDate = PersianDateTime.Now;
                toDo.PrimaryClosed = true;
                toDo.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

                _toDoRepository.Save(toDo);
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

        #region Insert
        public GeneralResponse AddTodo(AddToDoRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                ToDo toDo = new ToDo();
                toDo.ID = Guid.NewGuid();
                toDo.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                toDo.CreateDate = PersianDateTime.Now;
                toDo.Customer = _customerRepository.FindBy(request.CustomerID);
                if(request.EndDate !=null)
                toDo.EndDate = request.EndDate;
                else
                {
                    toDo.EndDate = request.StartDate;
                }
                toDo.EndTime = request.EndTime;
                toDo.StartDate = request.StartDate;
                toDo.StartTime = request.StartTime;
                toDo.PrimaryClosed = false;
                toDo.PriorityType = PriorityType.High;
                toDo.ToDoTitle = request.ToDoTitle;
                toDo.ToDoDescription = request.ToDoDescription;
                toDo.RowVersion = 1;
                // اگر لیست کارمندان آمد یعنی وظیفه مربوط به کارمندان است
                if (request.EmployeeIDs != null)
                {
                    IList<ToDoResult> toDoResults = new List<ToDoResult>();
                    foreach (Guid iD in request.EmployeeIDs)
                    {
                        Employee employee = _employeeRepository.FindBy(iD);

                        ToDoResult toDoResult = new ToDoResult();
                        toDoResult.ID = Guid.NewGuid();
                        toDoResult.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                        toDoResult.CreateDate = PersianDateTime.Now;
                        toDoResult.ToDo = toDo;
                        toDoResult.Remindable = request.Remindable;
                        toDoResult.RemindeTime = request.RemindeTime;
                        toDoResult.ReferedEmployee = employee;
                        toDoResult.RowVersion = 1;
                        toDoResult.ToDoResultDescription = "";
                        toDoResults.Add(toDoResult);
                    }
                    toDo.ToDoResults = toDoResults;
                    toDo.IsGrouped = false;
                }
                //اگر لیست گروه ها آمد به معنی اینکه این وظیفه گروهی است
                if (request.GroupIDs != null)
                {
                    IList<ToDoResult> toDoResults = new List<ToDoResult>();
                    foreach (Guid iD in request.GroupIDs)
                    {
                        
                        IEnumerable<Employee> employees = _employeeRepository.FindAll().Where(x => x.Group.ID == iD);
                        foreach (Employee employee in employees)
                        {

                            ToDoResult toDoResult = new ToDoResult();
                            toDoResult.ID = Guid.NewGuid();
                            toDoResult.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                            toDoResult.CreateDate = PersianDateTime.Now;
                            toDoResult.ToDo = toDo;
                            toDoResult.Remindable = request.Remindable;
                            toDoResult.RemindeTime = request.RemindeTime;
                            toDoResult.ReferedEmployee = employee;
                            toDoResult.RowVersion = 1;
                            toDoResult.ToDoResultDescription = "";
                            toDoResults.Add(toDoResult);

                        }
                    }
                    toDo.ToDoResults = toDoResults;
                    toDo.IsGrouped = false;
                }
                if (request.GroupIDs == null && request.EmployeeIDs == null)
                {
                    IList<ToDoResult> toDoResults = new List<ToDoResult>();

                    ToDoResult toDoResult = new ToDoResult();
                    toDoResult.ID = Guid.NewGuid();
                    toDoResult.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    toDoResult.CreateDate = PersianDateTime.Now;
                    toDoResult.ToDo = toDo;
                    toDoResult.Remindable = request.Remindable;
                    toDoResult.RemindeTime = request.RemindeTime;
                    toDoResult.ReferedEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    toDoResult.RowVersion = 1;
                    toDoResult.ToDoResultDescription = "";
                    toDoResults.Add(toDoResult);
                    toDo.ToDoResults = toDoResults;
                    toDo.IsGrouped = false;
                }
                _toDoRepository.Add(toDo);
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

        #endregion

        #region Update



        #endregion

        #region Add Attachment To ToDo

        public GeneralResponse AddToDoAttachment(AddToDoAttachmentRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {

                ToDo toDo = _toDoRepository.FindBy(request.ToDoID);

                #region Rename The file

                // extract the extention
                var fileExtention = Path.GetExtension(request.Attachment);
                // Get directory
                var directory = Path.GetDirectoryName(request.Attachment);
                // create filename
                string fileName = directory + "\\ToDo_" + toDo.ID + fileExtention;
                // Rename file
                //File.Move(request.Attachment, fileName);

                #endregion

                toDo.AttachmentType = fileExtention;
                toDo.Attachment = fileName;
                toDo.ModifiedDate = PersianDateTime.Now;
                toDo.ModifiedEmployee = _employeeRepository.FindBy(CreateEmployeeID);

                _toDoRepository.Save(toDo);
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

        #region Delete ToDo attached File

        public GeneralResponse DeleteToDoAttachment(Guid ToDoID,Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            
            ToDo toDo = _toDoRepository.FindBy(ToDoID);
            string path = toDo.Attachment;
            try
            {
                
                toDo.Attachment = null;
                toDo.AttachmentType = null;
                toDo.ModifiedDate = PersianDateTime.Now;
                toDo.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

                _toDoRepository.Save(toDo);
                _uow.Commit();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add(ex.InnerException.Message);
                }
            }

            if (response.hasError != true)
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    if (ex.InnerException != null)
                    {
                        response.ErrorMessages.Add(ex.InnerException.Message);
                    }
                }
            }

            return response;
        }

        #endregion

        #region Get ToDo results
        public GetGeneralResponse<IEnumerable<ToDoResultView>> GetToDoResults(Guid EmployeeID,Guid ToDoID)
        {
            GetGeneralResponse<IEnumerable<ToDoResultView>> response = new GetGeneralResponse<IEnumerable<ToDoResultView>>();
            try
            {

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criteriaEmployeeID = new Criterion("ToDo.ID", ToDoID, CriteriaOperator.Equal);
                query.Add(criteriaEmployeeID);

                Response<ToDoResult> toDoResults = _toDoResultRepository.FindBy(query, -1, -1);

                IList<ToDoResult> _todoResults = new List<ToDoResult>();

                foreach (var item in toDoResults.data)
                {
                    if (item.ReferedEmployee.ID == EmployeeID || item.CreateEmployee.ID == EmployeeID)
                        _todoResults.Add(item);
                }
                foreach (var todoResult in _todoResults)
                {
                    if(todoResult.Attachment !=null)
                    todoResult.Attachment = todoResult.Attachment.Replace(@"\", "/").Substring(todoResult.Attachment.IndexOf("data"));
                }
                toDoResults.data = _todoResults;
                response.data = toDoResults.data.ConvertToToDoResultViews();
                response.totalCount = toDoResults.totalCount;
            }
            catch (Exception ex)
            { 
            }
            return response;
        }

        public GetGeneralResponse<IEnumerable<ToDoResultView>> GetToDoResults(IEnumerable<Guid> ToDoResultsID)
        {
            GetGeneralResponse<IEnumerable<ToDoResultView>> response = new GetGeneralResponse<IEnumerable<ToDoResultView>>();
            try
            {
                IList<ToDoResult> _todoResults = new List<ToDoResult>();

                foreach (Guid Id in ToDoResultsID)
                {


                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                    Criterion criteriaEmployeeID = new Criterion("ID", ToDoResultsID, CriteriaOperator.Equal);
                    query.Add(criteriaEmployeeID);

                    ToDoResult toDoResults = _toDoResultRepository.FindBy(Id);
                    _todoResults.Add(toDoResults);
                    
                    foreach (var todoResult in _todoResults)
                    {
                        if (todoResult.Attachment != null)
                            todoResult.Attachment =
                                todoResult.Attachment.Replace(@"\", "/")
                                    .Substring(todoResult.Attachment.IndexOf("data"));
                    }
                    response.data = _todoResults.ConvertToToDoResultViews();
                    response.totalCount = _todoResults.Count();
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException.Message!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }
            return response;
        }

        #endregion

        public bool isFileExist(string path)
        {
            return File.Exists(path);
        }

        public GetGeneralResponse<IEnumerable<SimpleEmployeeView>> GetAllChildren(Guid managerID)
        {
            GetGeneralResponse<IEnumerable<SimpleEmployeeView>> response = new GetGeneralResponse<IEnumerable<SimpleEmployeeView>>();
            SimpleEmployee parent = _simpleEmployeeRepository.FindBy(managerID);
            List<SimpleEmployee> children = new List<SimpleEmployee>();
            PopulateChildren(parent, children);

            SimpleEmployee manager = new SimpleEmployee();
            manager = _simpleEmployeeRepository.FindBy(managerID);
            children.Add(manager);
            response.data = children.ConvertToSimpleEmployeeViews();

            return response;
        }

        private void PopulateChildren(SimpleEmployee parent, List<SimpleEmployee> children)
        {
            List<SimpleEmployee> myChildren = parent.ChildEmployees.ToList();
            if (myChildren.Any())
            {
                children.AddRange(myChildren);
                foreach (SimpleEmployee child in myChildren)
                {
                    PopulateChildren(child, children);
                }
            }
        }

        #endregion

        #region ToDoResult Methods

        public ToDoResultView GetToDoResult(Guid ToDoResultID)
        {
            //Response<ToDoResult> toDoResultView = new Response<ToDoResult>();

            ToDoResultView response = _toDoResultRepository.FindBy(ToDoResultID).ConvertToToDoResultView();

            return response;

        }

        #region Add Attachment To ToDo Result

        public GeneralResponse AddToDoResultAttachment(AddToDoResultAttachmentRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {

                ToDoResult toDoResult = _toDoResultRepository.FindBy(request.ToDoResultID);

                #region Rename The file

                // extract the extention
                var fileExtention = Path.GetExtension(request.Attachment);
                // Get directory
                var directory = Path.GetDirectoryName(request.Attachment);
                // create filename
                string fileName = directory + "\\ToDoResult_" + request.ToDoResultID + fileExtention;
                // Rename file
                //File.Move(request.Attachment, fileName);

                #endregion

                toDoResult.AttachmentType = fileExtention;
                toDoResult.Attachment = fileName;
                toDoResult.ModifiedDate = PersianDateTime.Now;
                toDoResult.ModifiedEmployee = _employeeRepository.FindBy(CreateEmployeeID);

                _toDoResultRepository.Save(toDoResult);
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

        #region Secondary Close

        public GeneralResponse SecondaryClose(Guid ToDoResultID, string ToDoResultDescription, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                ToDoResult toDoResul = _toDoResultRepository.FindBy(ToDoResultID);
                toDoResul.ModifiedDate = PersianDateTime.Now;
                toDoResul.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                toDoResul.ToDoResultDescription = ToDoResultDescription;
                toDoResul.SecondaryClosed = true;
                _toDoResultRepository.Save(toDoResul);

                
                // د صورتی که ایجاد کننده با ارجاع دهنده برابر بود و وظیفه فقط برای ایجاد کننده درج شده باشد بستن اصلی نیز انجام شود
                
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteriaToDoID = new Criterion("ToDo.ID", toDoResul.ToDo.ID,CriteriaOperator.Equal);
                query.Add(criteriaToDoID);
                Response<ToDoResult> toDoResults = _toDoResultRepository.FindBy(query, -1, -1);
                if (toDoResults.data.Count() == 1)
                {
                    ToDo toDo = _toDoRepository.FindBy(toDoResul.ToDo.ID);
                    toDo.PrimaryClosed = true;
                    toDo.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    toDo.ModifiedDate = PersianDateTime.Now;
                    _toDoRepository.Save(toDo);
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

        #region Edit ToDo Result

        public GeneralResponse EditToDoResult(EditToDoResultRequest request, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                ToDoResult toDoResult = new ToDoResult();
                toDoResult = _toDoResultRepository.FindBy(request.ID);
                toDoResult.ModifiedDate = PersianDateTime.Now;
                toDoResult.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                toDoResult.SecondaryClosed = request.SecondaryClosed;
                toDoResult.Remindable = request.Remindable;
                toDoResult.ToDoResultDescription = request.ToDoResultDescription;
                toDoResult.RemindeTime = request.RemindeTime;

                _toDoResultRepository.Save(toDoResult);
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

        #region Delete ToD Result Attachment

        public GeneralResponse DeleteToDoResultAttachment(Guid ToDoID, Guid ModifiedEmployeeID)
        {

            GeneralResponse response = new GeneralResponse();

            ToDoResult toDoResult = _toDoResultRepository.FindBy(ToDoID);
            string path = toDoResult.Attachment;
            try
            {

                toDoResult.Attachment = null;
                toDoResult.AttachmentType = null;
                toDoResult.ModifiedDate = PersianDateTime.Now;
                toDoResult.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

                _toDoResultRepository.Save(toDoResult);
                _uow.Commit();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add(ex.InnerException.Message);
                }
            }

            if (response.hasError != true)
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    if (ex.InnerException != null)
                    {
                        response.ErrorMessages.Add(ex.InnerException.Message);
                    }
                }
            }

            return response;
        }

        #endregion

        #endregion
    }
}
