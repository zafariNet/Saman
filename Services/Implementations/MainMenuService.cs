#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Persian;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Customers;
using Model.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;
using Infrastructure.Querying;
using Model;
using Services.ViewModels;
using Infrastructure.Domain;

#endregion

namespace Services.Implementations
{
    public class MainMenuService : IMainMenuService

    {
        #region Declares

        private readonly IMainMenuRepository _mainMenuRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPermitRepository _permitRepository;

        #endregion

        #region Ctor

        public MainMenuService(IMainMenuRepository mainMenuRepository, 
            IEmployeeRepository employeeRepository,
            IPermitRepository permitRepository)
        {
            _mainMenuRepository = mainMenuRepository;
            _employeeRepository = employeeRepository;
            _permitRepository = permitRepository;
        }

        #endregion

        #region Get Some

        public GetMainMenusResponse GetMainMenus(MainMenusGetRequest request)
        {
            GetMainMenusResponse response = new GetMainMenusResponse();
            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();

                Criterion criterion = new Criterion("ParentMenuName", request.ParentMenuName, CriteriaOperator.Equal);
                query.Add(criterion);

                Sort sort = new Sort("SortOrder", true);
                IList<Sort> sortList = new List<Sort>();
                sortList.Add(sort);
                Response<MainMenu> mainMenusResponse =
                    _mainMenuRepository.FindByQuery(query, sortList);

                Infrastructure.Querying.Query query2 = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Employee.ID", request.EmployeeID, CriteriaOperator.Equal);
                query2.Add(criteria);

                // Check permission
                Response<Permit> permits = _permitRepository.FindByQuery(query2);
                List<MainMenu> permitedMainMenu = new List<MainMenu>();
                foreach (var item in mainMenusResponse.data)
                {
                    Permit permit = permits.data.Where(w => w.PermitKey == item.PermissionKey).FirstOrDefault();

                    if ((permit != null && permit.Guaranteed) || item.PermissionKey == null)
                        if(item.Show)
                        permitedMainMenu.Add(item);
                }

               response.data = permitedMainMenu.ConvertToMainMenuViews();
               response.TotalCount = permitedMainMenu.Count();
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
