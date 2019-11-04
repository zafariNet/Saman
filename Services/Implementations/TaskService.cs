using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;
using System.IO;

namespace Services.Implementations
{
    public class TaskService:ITaskService
    {
        #region Declair

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IGroupRepository _groupRepository;

        #endregion

        #region Ctor

        public TaskService(IEmployeeRepository employeeRepository, IUnitOfWork uow,
            ICustomerRepository customerRepository, ITaskRepository taskRepository,IGroupRepository groupRepository)
        {
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _uow = uow;
            _taskRepository = taskRepository;
            _groupRepository = groupRepository;
        }

        #endregion

        #region Add

        public GeneralResponse AddTask(AddTaskRequest request, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Task task = new Task();
                task.ID = Guid.NewGuid();
                task.CreateDate = PersianDateTime.Now;
                task.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                if (request.CustomerID != null)
                    task.Customer =
                        _customerRepository.FindBy(request.CustomerID == null ? Guid.Empty : (Guid) request.CustomerID);
                task.EndDate = request.EndDate;
                task.IsMaster = true;
                task.ToDo = null;
                task.PrimaryClosed = false;
                task.PrimaryClosedDate = null;
                task.SecondaryClosed = false;
                task.SecondaryClosedDate = null;
                task.Reminder = request.Reminder;
                task.RemindTime = request.RemindTime;
                task.SendSms = request.SendSms;
                task.StartDate = request.StartDate;
                task.EndDate = request.EndDate;
                task.RemindTime = request.RemindTime;
                task.StartTime = request.StartTime;
                task.EndTime = request.EndTime;
                task.ToDoDescription = request.ToDoDescription;
                task.ToDoTitle = request.ToDoTitle;
                _taskRepository.Add(task);
                _uow.Commit();
                if (request.EmployeeIDs != null)
                {
                    foreach (var Id in request.EmployeeIDs)
                    {
                        Task childTask = new Task();

                        childTask.ID = Guid.NewGuid();
                        childTask.CreateDate = PersianDateTime.Now;
                        childTask.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                        if (request.CustomerID != null)
                            childTask.Customer =
                                _customerRepository.FindBy(request.CustomerID == null
                                    ? Guid.Empty
                                    : (Guid) request.CustomerID);
                        childTask.EndDate = request.EndDate;
                        childTask.IsMaster = false;
                        childTask.ToDo = null;
                        childTask.PrimaryClosed = false;
                        childTask.PrimaryClosedDate = null;
                        childTask.SecondaryClosed = false;
                        childTask.SecondaryClosedDate = null;
                        childTask.Reminder = request.Reminder;
                        childTask.RemindTime = request.RemindTime;
                        childTask.SendSms = request.SendSms;
                        childTask.StartDate = request.StartDate;
                        childTask.EndDate = request.EndDate;
                        childTask.RemindTime = request.RemindTime;
                        childTask.StartTime = request.StartTime;
                        childTask.EndTime = request.EndTime;
                        childTask.ToDoDescription = request.ToDoDescription;
                        childTask.ToDoTitle = request.ToDoTitle;
                        childTask.ToDo = task;
                        childTask.ReferedEmployee = _employeeRepository.FindBy(Id == null ? Guid.Empty : (Guid) Id);
                        _taskRepository.Add(childTask);
                    }
                    
                }

                if (request.GroupIDs != null && request.EmployeeIDs==null)
                {
                    foreach (var Id in request.GroupIDs)
                    {
                        Group group = _groupRepository.FindBy(Id == null ? Guid.Empty : (Guid) Id);
                        foreach (var item in group.Employees.Where(x=>!x.Discontinued))
                        {


                            Task childTask = new Task();

                            childTask.ID = Guid.NewGuid();
                            childTask.CreateDate = PersianDateTime.Now;
                            childTask.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                            if (request.CustomerID != null)
                                childTask.Customer =
                                    _customerRepository.FindBy(request.CustomerID == null
                                        ? Guid.Empty
                                        : (Guid) request.CustomerID);
                            childTask.EndDate = request.EndDate;
                            childTask.IsMaster = false;
                            childTask.ToDo = null;
                            childTask.PrimaryClosed = false;
                            childTask.PrimaryClosedDate = null;
                            childTask.SecondaryClosed = false;
                            childTask.SecondaryClosedDate = null;
                            childTask.Reminder = request.Reminder;
                            childTask.RemindTime = request.RemindTime;
                            childTask.SendSms = request.SendSms;
                            childTask.StartDate = request.StartDate;
                            childTask.EndDate = request.EndDate;
                            childTask.RemindTime = request.RemindTime;
                            childTask.StartTime = request.StartTime;
                            childTask.EndTime = request.EndTime;
                            childTask.ToDoDescription = request.ToDoDescription;
                            childTask.ToDoTitle = request.ToDoTitle;
                            childTask.ToDo = task;
                            childTask.ReferedEmployee = item;
                            _taskRepository.Add(childTask);
                        }
                    }

                }

                if (request.GroupIDs == null && request.EmployeeIDs == null)
                {
                    var item = _employeeRepository.FindBy(CreateEmployeeID);

                    Task childTask = new Task();
                    childTask.ID = Guid.NewGuid();
                    childTask.CreateDate = PersianDateTime.Now;
                    childTask.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    if (request.CustomerID != null)
                        childTask.Customer =
                            _customerRepository.FindBy(request.CustomerID == null
                                ? Guid.Empty
                                : (Guid)request.CustomerID);
                    childTask.EndDate = request.EndDate;
                    childTask.IsMaster = false;
                    childTask.ToDo = null;
                    childTask.PrimaryClosed = false;
                    childTask.PrimaryClosedDate = null;
                    childTask.SecondaryClosed = false;
                    childTask.SecondaryClosedDate = null;
                    childTask.Reminder = request.Reminder;
                    childTask.RemindTime = request.RemindTime;
                    childTask.SendSms = request.SendSms;
                    childTask.StartDate = request.StartDate;
                    childTask.EndDate = request.EndDate;
                    childTask.RemindTime = request.RemindTime;
                    childTask.StartTime = request.StartTime;
                    childTask.EndTime = request.EndTime;
                    childTask.ToDoDescription = request.ToDoDescription;
                    childTask.ToDoTitle = request.ToDoTitle;
                    childTask.ToDo = task;
                    childTask.ReferedEmployee = item;
                    _taskRepository.Add(childTask);
                }

                // Validate

                if (task.GetBrokenRules().Any())
                {
                    foreach (var item in task.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(item.Rule);
                    }
                    return response;
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

        #region New

        public GetGeneralResponse<IEnumerable<TaskOwnView>> GetTasks(Guid employeeId, string TaskType,
            string Status, string StartDate, string EndDate, Guid CurrentEmployeeId)
        {
            var response = new GetGeneralResponse<IEnumerable<TaskOwnView>>();

            try
            {
                #region between

                IList<FilterData> filters=new List<FilterData>();

                if (TaskType == "Creator")
                {
                    if(Status=="close")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.TrueString }
                            },
                            field = "PrimaryClosed"
                        });
                    if (Status == "open")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            },
                            field = "PrimaryClosed"
                        });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.TrueString }
                        },
                        field = "IsMaster"
                    });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "string",
                            value = new[] { employeeId.ToString()}
                        },
                        field = "CreateEmployee.ID"
                    });
                }
                if (TaskType == "Refered")
                {
                    if (Status == "close")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.TrueString }
                            },
                            field = "SecondaryClosed"
                        });
                    if (Status == "open")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            },
                            field = "SecondaryClosed"
                        });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.FalseString }
                        },
                        field = "IsMaster"
                    });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "string",
                            value = new[] { employeeId.ToString() }
                        },
                        field = "ReferedEmployee.ID"
                    });
                }

                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { StartDate}
                    },
                    field = "StartDate"
                });

                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { EndDate }
                    },
                    field = "EndDate"
                });

                string startQuery = FilterUtilityService.GenerateFilterHQLQuery(filters, "Task", null);
                var between = _taskRepository.FindAll(startQuery);

                #endregion

                #region past


                filters.Clear();

                if (TaskType == "Creator")
                {
                    if (Status == "close")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.TrueString }
                            },
                            field = "PrimaryClosed"
                        });
                    if (Status == "open")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            },
                            field = "PrimaryClosed"
                        });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.TrueString }
                        },
                        field = "IsMaster"
                    });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "string",
                            value = new[] { employeeId.ToString() }
                        },
                        field = "CreateEmployee.ID"
                    });
                }
                if (TaskType == "Refered")
                {
                    if (Status == "close")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.TrueString }
                            },
                            field = "SecondaryClosed"
                        });
                    if (Status == "open")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            },
                            field = "SecondaryClosed"
                        });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.FalseString }
                        },
                        field = "IsMaster"
                    });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "string",
                            value = new[] { employeeId.ToString() }
                        },
                        field = "ReferedEmployee.ID"
                    });
                }


                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { StartDate }
                    },
                    field = "StartDate"
                });
                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { EndDate }
                    },
                    field = "EndDate"
                });


                string startQueryU = FilterUtilityService.GenerateFilterHQLQuery(filters, "Task", null);
                var past = _taskRepository.FindAll(startQueryU);

                #endregion

                #region future


                filters.Clear();

                if (TaskType == "Creator")
                {
                    if (Status == "close")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.TrueString }
                            },
                            field = "PrimaryClosed"
                        });
                    if (Status == "open")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            },
                            field = "PrimaryClosed"
                        });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.TrueString }
                        },
                        field = "IsMaster"
                    });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "string",
                            value = new[] { employeeId.ToString() }
                        },
                        field = "CreateEmployee.ID"
                    });
                }
                if (TaskType == "Refered")
                {
                    if (Status == "close")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.TrueString }
                            },
                            field = "SecondaryClosed"
                        });
                    if (Status == "open")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            },
                            field = "SecondaryClosed"
                        });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.FalseString }
                        },
                        field = "IsMaster"
                    });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "string",
                            value = new[] { employeeId.ToString() }
                        },
                        field = "ReferedEmployee.ID"
                    });
                }


                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { StartDate }
                    },
                    field = "StartDate"
                });
                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { EndDate }
                    },
                    field = "EndDate"
                });


                string endQueryU = FilterUtilityService.GenerateFilterHQLQuery(filters, "Task", null);
                var future = _taskRepository.FindAll(endQueryU);


                #endregion

                #region past & future



                filters.Clear();

                if (TaskType == "Creator")
                {
                    if (Status == "close")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.TrueString }
                            },
                            field = "PrimaryClosed"
                        });
                    if (Status == "open")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            },
                            field = "PrimaryClosed"
                        });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.TrueString }
                        },
                        field = "IsMaster"
                    });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "string",
                            value = new[] { employeeId.ToString() }
                        },
                        field = "CreateEmployee.ID"
                    });
                }
                if (TaskType == "Refered")
                {
                    if (Status == "close")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.TrueString }
                            },
                            field = "SecondaryClosed"
                        });
                    if (Status == "open")
                        filters.Add(new FilterData
                        {
                            data = new data()
                            {
                                comparison = "eq",
                                type = "boolean",
                                value = new[] { bool.FalseString }
                            },
                            field = "SecondaryClosed"
                        });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { bool.FalseString }
                        },
                        field = "IsMaster"
                    });
                    filters.Add(new FilterData
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "string",
                            value = new[] { employeeId.ToString() }
                        },
                        field = "ReferedEmployee.ID"
                    });
                }


                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { StartDate }
                    },
                    field = "StartDate"
                });
                filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { EndDate }
                    },
                    field = "EndDate"
                });


                string domainQuery = FilterUtilityService.GenerateFilterHQLQuery(filters, "Task", null);
                var pastfuture = _taskRepository.FindAll(domainQuery);

                #endregion

                #region prepairing Data

                IList<Task> list = new List<Task>();
                foreach (var item in between.data)
                {
                    list.Add(item);
                }
                foreach (var item in past.data)
                {
                    list.Add(item);
                }
                foreach (var item in future.data)
                {
                    list.Add(item);
                }
                foreach (var item in pastfuture.data)
                {
                    list.Add(item);
                }

                var DistinctList = list.Distinct();
                var finalList = new List<Task>();
                var distinctList = DistinctList as Task[] ?? DistinctList.ToArray();
                if(TaskType=="Refered")
                foreach (var task in distinctList)
                {
                    finalList.Add(task.ToDo);
                }
                else
                {
                    foreach (var task in distinctList)
                    {
                        finalList.Add(task);
                    }
                }

                #endregion


                foreach (var item in finalList.ToList())
                {
                    if (item.CreateEmployee.ID == CurrentEmployeeId)
                        item.CanEditMaster = true;
                    // اصلاح آدرس عکسها و پاس دادن آدرس جدید
                    if (!string.IsNullOrEmpty(item.PrimaryFile))
                        item.PrimaryFile = item.PrimaryFile.Replace(@"\", "/").Substring(item.PrimaryFile.IndexOf("data")); ;
                    IList<Task> refered=new List<Task>();
                    if (item.ToDoResults != null)
                    {

                        foreach (var _item in item.ToDoResults)
                        {
                            if (!string.IsNullOrEmpty(_item.SecondaryFile))
                                _item.SecondaryFile = _item.SecondaryFile.Replace(@"\", "/").Substring(_item.SecondaryFile.IndexOf("data")); ;
                            if (_item.ReferedEmployee.ID == CurrentEmployeeId && !item.Completed)
                                _item.CanEditDetail = true;
                            if (_item.CreateEmployee.ID == CurrentEmployeeId ||
                                _item.ReferedEmployee.ID == CurrentEmployeeId)
                            {
                                refered.Add(_item);
                            }
                        }
                        
                    }
                    item.ToDoResults = refered;
                }

                response.data = finalList.ConvertToTaskOwnViews();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }
            return response;
        }

        #endregion

        #region Read by Employee

        public GetGeneralResponse<IEnumerable<TaskOwnView>> GetEmployeeTasks(Guid EmployeeID, int pageSize, int pageNumber,IList<Sort> sort )
        {
            GetGeneralResponse<IEnumerable<TaskOwnView>> response=new GetGeneralResponse<IEnumerable<TaskOwnView>>();

            try
            {
                #region Get Creator

                IList<FilterData> Creatorfilter=new List<FilterData>();

                Creatorfilter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eqOr",
                        type = "string",
                        value = new[] { EmployeeID.ToString()}
                    },
                    field = "CreateEmployee.ID"

                });
                
                Creatorfilter.Add(new FilterData()
                {
                    
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { Boolean.TrueString }
                    },
                    field = "IsMaster"

                });


                string creatorquery = FilterUtilityService.GenerateFilterHQLQuery(Creatorfilter, "Task", sort);

                IList<Task> FinalcreatorTasks = new List<Task>();

                Response<Task> creatortask = _taskRepository.FindAll(creatorquery);
                foreach (var item in creatortask.data)
                {
                    FinalcreatorTasks.Add(item);
                }

                #endregion

                #region Gete Refered

                IList<FilterData> referfilter = new List<FilterData>();

                referfilter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eqOr",
                        type = "string",
                        value = new[] { EmployeeID.ToString() }
                    },
                    field = "ReferedEmployee.ID"

                });

                Creatorfilter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { Boolean.FalseString }
                    },
                    field = "IsMaster"

                });

                string referedrquery = FilterUtilityService.GenerateFilterHQLQuery(referfilter, "Task", null);


                Response<Task> referedtask = _taskRepository.FindAll(referedrquery);

                IList<Task> FinalReferedTasks =new List<Task>();
                foreach (var item in referedtask.data)

                {
                    var temp = item.ToDo.ToDoResults.Where(x => x.ReferedEmployee.ID == EmployeeID || x.ToDo.CreateEmployee.ID == EmployeeID);
                    item.ToDo.ToDoResults = temp;
                    FinalReferedTasks.Add(item.ToDo);
                }

                #endregion

                IList<Task> FinalTasks=new List<Task>();

                foreach (var item in FinalReferedTasks)
                {
                    
                    FinalTasks.Add(item);
                }
                foreach (var item in FinalcreatorTasks)
                {

                    if(!FinalTasks.Contains(item))
                    FinalTasks.Add(item);
                }

                foreach (var item in FinalTasks)
                {
                    if (item.CreateEmployee.ID == EmployeeID)
                        item.CanEditMaster = true;
                    // اصلاح آدرس عکسها و پاس دادن آدرس جدید
                    if(!string.IsNullOrEmpty(item.PrimaryFile))
                        item.PrimaryFile = item.PrimaryFile.Replace(@"\", "/").Substring(item.PrimaryFile.IndexOf("data")); ;
                    
                    if (item.ToDoResults != null)
                    {
                        
                        foreach (var _item in item.ToDoResults)
                        {
                            if(!string.IsNullOrEmpty(_item.SecondaryFile))
                            _item.SecondaryFile = _item.SecondaryFile.Replace(@"\", "/").Substring(_item.SecondaryFile.IndexOf("data")); ;
                            if (_item.ReferedEmployee.ID == EmployeeID && !item.Completed)
                                _item.CanEditDetail = true;
                        }
                    }
                }


                response.data = FinalTasks.ConvertToTaskOwnViews();
                response.totalCount = FinalTasks.Count();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<TaskOwnView>>  GetFirstPageTasks(string taskType,Guid employeeId,int pageSize,int pageNumber)
        {
            var response = new GetGeneralResponse<IEnumerable<TaskOwnView>> ();
            int index = (pageNumber - 1) * pageSize;
            int count = pageSize;
            if (taskType == "Creator")
            {
                IList<FilterData> filter=new List<FilterData>();
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { PersianDateTime.Now.Substring(0,10)}
                    },
                    field = "StartDate"
                });
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] {PersianDateTime.Now.Substring(0, 10)}
                    },
                    field = "EndDate"
                });
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "string",
                        value = new[] { employeeId.ToString() }
                    },
                    field = "CreateEmployee.ID"
                });
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "boolean",
                        value = new[] { bool.TrueString }
                    },
                    field = "IsMaster"
                });
                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Task", null);

                var tasks = _taskRepository.FindAll(query,index,count);

                response.data = tasks.data.ConvertToTaskOwnViews();
                response.totalCount = tasks.totalCount;

            }
            if (taskType == "Refered")
            {
                IList<FilterData> filter = new List<FilterData>();
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "gteq",
                        type = "dateOnly",
                        value = new[] { PersianDateTime.Now.Substring(0, 10) }
                    },
                    field = "EndDate"
                });
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "lteq",
                        type = "dateOnly",
                        value = new[] { PersianDateTime.Now.Substring(0, 10) }
                    },
                    field = "EndDate"
                });
                filter.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "string",
                        value = new[] { employeeId.ToString() }
                    },
                    field = "ReferedEmployee.ID"
                });

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Task", null);

                var tasks = _taskRepository.FindAll(query,index,count);

                IList<Task> FinalReferedTasks = new List<Task>();

                foreach (var item in tasks.data)
                {
                    var temp = item.ToDo.ToDoResults.Where(x => x.ReferedEmployee.ID == employeeId );
                    item.ToDo.ToDoResults = temp;
                    FinalReferedTasks.Add(item.ToDo);
                }


                response.data = FinalReferedTasks.ConvertToTaskOwnViews();
                response.totalCount = tasks.totalCount;

            }

            return response;
        }

        #endregion

        #region Close Secondary

        public GeneralResponse CloseSecondary(Guid EmployeeID, Guid TaskID, string SecondaryDescription)

        {
            GeneralResponse response=new GeneralResponse();
            try
            {
               
                Task task = _taskRepository.FindBy(TaskID);
                if (!task.IsMaster && task.ReferedEmployee.ID == EmployeeID)
                {
                    task.SecondaryClosed = true;
                    task.SecondaryClosedDate = PersianDateTime.Now;
                    task.ToDoResultDescription = SecondaryDescription;
                    _taskRepository.Save(task);
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

        #region Close Primary

        public GeneralResponse ClosePrimary(Guid EmployeeID, Guid TaskID)

        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                Task task = _taskRepository.FindBy(TaskID);
                if (task.CreateEmployee.ID == EmployeeID && task.IsMaster)
                {
                    task.PrimaryClosed = true;
                    task.PrimaryClosedDate = PersianDateTime.Now;
                    if (task.ToDoResults.Any())
                        foreach (var item in task.ToDoResults)
                        {
                            if (!item.SecondaryClosed)
                            {
                                item.SecondaryClosed = true;
                                item.SecondaryClosedDate = PersianDateTime.Now;
                                item.ToDoResultDescription = "بسته شده توسط ایجاد کننده";
                            }
                        }
                    _taskRepository.Save(task);
                    _uow.Commit();

                }
                else
                {
                    response.ErrorMessages.Add("شما مجاز به بستن وظیفه نیستید.");
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

        #region Can Attach To Secondary

        public bool CanAttachSecondary(Guid TaskID, Guid EmployeeID)
        {
            bool response=false;

            try
            {
                Task task= _taskRepository.FindBy(TaskID);
                if (task.ReferedEmployee.ID == EmployeeID && task.SecondaryFile==null)
                    response = true;
                else
                {
                    response = false;
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return response;
        }

        #endregion

        #region Can Attach To Primary

        public bool CanAttachPrimary(Guid EmployeeID, Guid TaskID)
        {
            bool response;

            try
            {
                Task task = _taskRepository.FindBy(TaskID);
                if (task.CreateEmployee.ID == EmployeeID && task.PrimaryFile == null)
                    response = true;
                else
                {
                    response = false;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return response;
        }

        #endregion

        #region Attach File To Secondary
        public GeneralResponse  AttachFileToSecondary(Guid TaskID, Guid EmployeeID,string path,string fileName)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Task task = _taskRepository.FindBy(TaskID);
                if (task.ReferedEmployee.ID == EmployeeID)
                {
                    var fileExtention = Path.GetExtension(fileName);
                    // Get directory
                    var directory = Path.GetDirectoryName(path);

                    string fileName1 = directory + "/" + fileName;

                    task.SecondaryFile = fileName1;
                    _taskRepository.Save(task);
                    _uow.Commit();
                }
                else
                {
                    response.ErrorMessages.Add("شما مجاز به افزودن فایل به این وظیفه نیستید");
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

        #region Attach File To primary

        public GeneralResponse AttachFileToPrimary(Guid TaskID, Guid EmployeeID,string path, string fileName)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Task task = _taskRepository.FindBy(TaskID);
                if (task.CreateEmployee.ID == EmployeeID)
                {
                    var fileExtention = Path.GetExtension(fileName);
                    // Get directory
                    var directory = Path.GetDirectoryName(path);

                    string fileName1 = directory + "/" + fileName;
                    task.PrimaryFile = fileName1;
                    _taskRepository.Save(task);
                    _uow.Commit();
                }
                else
                {
                    response.ErrorMessages.Add("شما مجاز به افزودن فایل به این وظیفه نیستید");
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

        #region Search Tasks


        public GetGeneralResponse<IEnumerable<TaskOwnView>> GetTasks(IEnumerable<Guid?> CreateEmployeeIDs, IEnumerable<Guid?> ReferedEmployeeIDs,
            int? Status, int? Type, IList<Sort> sort, Guid EmployeeID, string StartDate, string EndDate,Guid CurrentEmployeeId)
        {
            GetGeneralResponse<IEnumerable<TaskOwnView>> response = new GetGeneralResponse<IEnumerable<TaskOwnView>>();
            try
            {

                IList<FilterData> Filters = new List<FilterData>();
                 IList<Task> result=new List<Task>();
                if (CreateEmployeeIDs != null && ReferedEmployeeIDs != null)
                {

                    #region Creator

                    IList<FilterData> CreatorFilters = new List<FilterData>();

                    IList<string> Ids = new List<string>();
                    foreach (var item in CreateEmployeeIDs)
                    {
                        Ids.Add(item.ToString());
                    }
                    FilterData Filter = new FilterData();
                    Filter.field = "CreateEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    CreatorFilters.Add(Filter);

                    CreatorFilters.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { Boolean.TrueString }
                        },
                        field = "IsMaster"

                    });

                    if (StartDate != null && EndDate == null)
                    {
                        FilterData Filter2 = new FilterData()
                        {
                            field = "EndDate",
                            data = new data()
                            {
                                comparison = "gteq",
                                type = "date",
                                value = new[] { StartDate }
                            }
                        };
                        CreatorFilters.Add(Filter2);
                    }

                    if (EndDate != null && StartDate == null)
                    {
                        FilterData Filter3 = new FilterData()
                        {
                            field = "EndDate",
                            data = new data()
                            {
                                comparison = "lteq",
                                type = "date",
                                value = new[] { EndDate }
                            }
                        };
                        CreatorFilters.Add(Filter3);
                    }

                    if (StartDate != null && EndDate != null)
                    {
                        FilterData Filter4 = new FilterData()
                        {
                            field = "EndDate",
                            data = new data()
                            {
                                comparison = "gteq",
                                type = "dateBetween",
                                value = new[] { StartDate,EndDate }
                            }
                        };
                        CreatorFilters.Add(Filter4);
                    }

                    string creatorquery = FilterUtilityService.GenerateFilterHQLQuery(CreatorFilters, "Task", sort);

                    Response<Task> creatortasks = _taskRepository.FindAll(creatorquery);
                    IList<Task> FinalcreatorTasks = new List<Task>();

                    foreach (var item in creatortasks.data)
                    {
                        FinalcreatorTasks.Add(item);
                    }

                    #endregion


                    #region Refered


                    IList<FilterData> referedFilters = new List<FilterData>();

                    IList<string> gIds = new List<string>();
                    foreach (var item in ReferedEmployeeIDs)
                    {
                        gIds.Add(item.ToString());
                    }
                    FilterData Filter1 = new FilterData();
                    Filter.field = "ReferedEmployee.ID";
                    Filter.data = new data()
                    {
                        type = "list",
                        value = Ids.ToArray()
                    };
                    referedFilters.Add(Filter1);

                    referedFilters.Add(new FilterData()
                    {
                        data = new data()
                        {
                            comparison = "eq",
                            type = "boolean",
                            value = new[] { Boolean.FalseString }
                        },
                        field = "IsMaster"

                    });

                    if (StartDate != null && EndDate == null)
                    {
                        FilterData Filter2 = new FilterData()
                        {
                            field = "EndDate",
                            data = new data()
                            {
                                comparison = "gteq",
                                type = "date",
                                value = new[] { StartDate }
                            }
                        };
                        CreatorFilters.Add(Filter2);
                    }

                    if (EndDate != null && StartDate == null)
                    {
                        FilterData Filter3 = new FilterData()
                        {
                            field = "EndDate",
                            data = new data()
                            {
                                comparison = "lteq",
                                type = "date",
                                value = new[] { EndDate }
                            }
                        };
                        CreatorFilters.Add(Filter3);
                    }

                    if (StartDate != null && EndDate != null)
                    {
                        FilterData Filter4 = new FilterData()
                        {
                            field = "EndDate",
                            data = new data()
                            {
                                comparison = "gteq",
                                type = "dateBetween",
                                value = new[] { StartDate, EndDate }
                            }
                        };
                        CreatorFilters.Add(Filter4);
                    }

                    string referedquery = FilterUtilityService.GenerateFilterHQLQuery(CreatorFilters, "Task", sort);

                    Response<Task> referedtasks = _taskRepository.FindAll(referedquery);

                    IList<Task> FinalReferedTasks = new List<Task>();
                    foreach (var item in referedtasks.data)
                    {
                        FinalReferedTasks.Add(item.ToDo);
                    }

                    #endregion


                    IList<Task> FinalTasks = new List<Task>();

                    foreach (var item in FinalReferedTasks)
                    {
                        FinalTasks.Add(item);
                    }
                    foreach (var item in FinalcreatorTasks)
                    {
                        if (!FinalTasks.Contains(item))
                            FinalTasks.Add(item);
                    }

                    foreach (var item in FinalTasks)
                    {
                        if (item.ToDoResults != null)
                        {
                            foreach (var _item in item.ToDoResults)
                            {
                                if (_item.ReferedEmployee.ID == EmployeeID)
                                    _item.CanEditDetail = true;
                            }
                        }
                    }


                   
                    
                    //باز اصلی - باز فرعی
                    if (Type == 1)
                    {
                        var temp = FinalTasks.Where(x => x.PrimaryClosed != false);
                        foreach (var item in temp)
                        {
                            if (item.ToDoResults.Any(x => x.SecondaryClosed == true)) 
                                result.Add(item);
                        }
                    }

                    //باز اصلی - بسته فرعی
                    if (Type == 2)
                    {
                        var temp = FinalTasks.Where(x => x.PrimaryClosed != false);
                        foreach (var item in temp)
                        {
                            if (item.ToDoResults.Any(x => x.Completed == true))
                                result.Add(item);
                        }
                    }
                    // بسته اصلی
                    if (Type == 3)
                    {
                        var temp = FinalTasks.Where(x => x.PrimaryClosed == false);
                        foreach (var item in temp)
                        {
                                result.Add(item);
                        }
                    }

                    if (Type == 4)
                    {
                        var temp = FinalTasks;
                        foreach (var item in temp)
                        {
                            result.Add(item);
                        }
                    }

                    // اگر معوق بود
                    if (Status == 1)
                    {
                        var temp = FinalTasks.Where(x => x.EndDate.CompareTo(PersianDateTime.Now) < 0);
                        foreach (var item in temp)
                        {
                            result.Add(item);
                        }
                    }

                    ///در حال انجام
                    if (Status == 1)
                    {
                        var temp = FinalTasks.Where(x => x.EndDate.CompareTo(PersianDateTime.Now) == 0);
                        foreach (var item in temp)
                        {
                            result.Add(item);
                        }
                    }

                    //آینده
                    if (Status == 2)
                    {
                        var temp = FinalTasks.Where(x => x.EndDate.CompareTo(PersianDateTime.Now) > 0);
                        foreach (var item in temp)
                        {
                            result.Add(item);
                        }
                    }
                }

                response.data = result.ConvertToTaskOwnViews();
                response.totalCount = result.Count();
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

        #region Delete Attachment

        public GeneralResponse DeleteAttachment(Guid TaskID)
        {
            var response = new GeneralResponse();
            try
            {
                bool isPrimary=false;
                Task task = _taskRepository.FindBy(TaskID);
                if (string.IsNullOrEmpty(task.PrimaryFile))
                    isPrimary = true;
                if (string.IsNullOrEmpty(task.SecondaryFile))
                    isPrimary = false;
                task.SecondaryFile = null;
                task.PrimaryFile = null;

                _taskRepository.Save(task);
                _uow.Commit();
                if(isPrimary)
                response.ObjectAdded = task.PrimaryFile;
                else
                {
                    response.ObjectAdded = task.SecondaryFile;
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
    }
}
