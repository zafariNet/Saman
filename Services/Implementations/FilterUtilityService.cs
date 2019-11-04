using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using Infrastructure.Querying;
using Services.Messaging.SaleCatalogService;
namespace Services.Implementations
{
    public static class FilterUtilityService
    {
        #region With Criterion
        public static IList<Criterion> GetCriterion(IList<FilterData> Filters)
        {

            IList<Criterion> CritationList = new List<Criterion>();
            CriteriaOperator _operator = CriteriaOperator.Contains;
            foreach (FilterData filter in Filters)
            {


                switch (filter.data.comparison)
                {
                    case "eq":
                        {
                            _operator = CriteriaOperator.Equal;
                            break;
                        }
                    case "lt":
                        {
                            _operator = CriteriaOperator.LesserThan;
                            break;
                        }
                    case "gt":
                        {
                            _operator = CriteriaOperator.GreaterThan;
                            break;
                        }
                    case null:
                        {
                            _operator = CriteriaOperator.Contains;
                            break;
                        }
                }
                switch (filter.field)
                {
                    case "Name":
                        {
                            filter.field = "LastName";
                            break;
                        }
                    case "CenterName":
                        {
                            filter.field = "Center.CenterName";
                            break;
                        }
                }
                CritationList.Add(new Criterion(filter.field, filter.data.value, _operator));

            }
            return CritationList;
        }

        #endregion

        #region Create HQL Query

        #region Create with Sort And Filter

        public static string GenerateFilterHQLQuery(IList<FilterData> Filters,string EntityModel,IList<Sort> sort)
        {

            #region Preparing Filters


            bool hasAnd = false;
            bool firstStep = true;
            bool and = true;
            string query = "From " + EntityModel + " ";
            if (Filters != null && Filters.Count()>0)
            {
                int mainCounter = Filters.Count();
                int maintemp = 0;
                query += " m where ";
                foreach (FilterData filter in Filters)
                {

                    if (filter.data.type == "list")
                    {
                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + " in(";
                        int counter = filter.data.value.Count();
                        int temp = 0;
                        foreach (string _valu in filter.data.value)
                        {
                            temp++;
                            if (temp == counter)
                                query += "'" + _valu + "')";
                            else
                                query += "'" + _valu + "',";
                        }
                    }

                    if (filter.data.type == "numeric")
                    {
                        foreach (string _value in filter.data.value)
                        {
                            switch (filter.data.comparison)
                            {
                                case "eq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "=" + _value + "";
                                        break;
                                    }
                                case "lt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<" + _value;
                                        break;
                                    }
                                case "gt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + ">" + _value;
                                        break;
                                    }
                                case "gteq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "=>'" + _value + "'";
                                        break;
                                    }
                                case "lteq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "=<'" + _value + "'";
                                        break;
                                    }
                                case "Noteq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + "";
                                        break;
                                    }
                                case null:
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "Like '%" + _value + "%'";
                                        break;
                                    }
                            }
                        }
                    }

                    if (filter.data.type == "string")
                    {
                        foreach (string _value in filter.data.value)
                        {
                            query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                        }


                    }

                    if (filter.data.type == "date")
                    {
                        foreach (string _value in filter.data.value)
                        {
                            switch (filter.data.comparison)
                            {

                                case "eq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                                        break;
                                    }
                                case "lt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<'" + _value + " 23:59:59'";
                                        break;
                                    }
                                case "gt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + ">'" + _value + " 00:00:00'";
                                        break;
                                    }
                                case "gteq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + ">='" + _value + " 00:00:00'";
                                        break;
                                    }
                                case "lteq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<='" + _value + " 23:59:59'";
                                        break;
                                    }
                                case null:
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                                        break;
                                    }
                            }
                        }

                    }

                    if (filter.data.type == "dateOnly")
                    {
                        foreach (string _value in filter.data.value)
                        {
                            switch (filter.data.comparison)
                            {

                                case "eq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                                        break;
                                    }
                                case "lt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<'" + _value + "'";
                                        break;
                                    }
                                case "gt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + ">'" + _value + "'";
                                        break;
                                    }
                                case "gteq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + ">='" + _value + "'";
                                        break;
                                    }
                                case "lteq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<='" + _value + "'";
                                        break;
                                    }
                                case null:
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                                        break;
                                    }
                            }
                        }

                    }

                    if (filter.data.type == "dateBetween")
                    {
                        query += " m." + TranslateViwModelToModel(filter.field, EntityModel) + " Between '" + filter.data.value[0] + " 00:00:00' and '" +filter.data.value[1] + " 23:59:59'";

                       
                    }

                    if (filter.data.type == "dateOnlyBetween")
                    {
                        query += " m." + TranslateViwModelToModel(filter.field, EntityModel) + " Between '" + filter.data.value[0] + "' and '" + filter.data.value[1] + "'";

                    }

                    if (filter.data.type == "boolean")
                    {
                        foreach (string _value in filter.data.value)
                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "=" + _value + "";

                    }

                    if (filter.data.type == "CheckNull")
                    {
                        foreach (string _value in filter.data.value)
                        {


                            if (filter.data.comparison == "eqAnd")
                            {
                                
                            }

                            if (filter.data.comparison == "eqOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += " m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + "  or ";
                                    and = false;
                                }
                                else
                                {
                                    query += "  m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + "";
                                    and = true;
                                }
                            }
                            else
                                query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + "";
                        }
                    }


                    if (filter.data.type == "CheckOr")
                    {
                        
                        foreach (string _value in filter.data.value)
                        {

                            if (filter.data.comparison == "eqgtAndBegin")
                            {

                                    query += "( m." + TranslateViwModelToModel(filter.field, EntityModel) + ">'" + _value + "' or ";
                                    and = false ;
                        
                            }

                            if (filter.data.comparison == "eqgtAndEnd")
                            {


                                    query += " m." + TranslateViwModelToModel(filter.field, EntityModel) + ">'" + _value + "' or ";
                                    and = false;
                                
                            }
                            else if (filter.data.comparison == "eqOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += "( m." + TranslateViwModelToModel(filter.field, EntityModel) + "='" + _value + "'  or ";
                                    and = false;
                                }
                                else
                                {
                                    query += "(  m." + TranslateViwModelToModel(filter.field, EntityModel) + "='" + _value + "'";
                                    and = true;
                                }
                            }

                            else if (filter.data.comparison == "eqgtOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += "( m." + TranslateViwModelToModel(filter.field, EntityModel) + ">'" + _value + "'  or ";
                                    and = false;
                                }
                                else
                                {
                                    query += "(  m." + TranslateViwModelToModel(filter.field, EntityModel) + ">'" + _value + "'";
                                    and = true;
                                }
                            }
                            
                        }
                    }

                    if (filter.data.type == "CheckOrEnd")
                    {
                        foreach (string _value in filter.data.value)
                        {




                            if (filter.data.comparison == "eqOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += " m." + TranslateViwModelToModel(filter.field, EntityModel) + "='" + _value + "') ";
                                    and = false;
                                }
                                else
                                {
                                    query += "  m." + TranslateViwModelToModel(filter.field, EntityModel) + "='" + _value + "')";
                                    and = true;
                                }
                            }
                            else
                                query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "='" + _value + "')";
                        }
                    }

                    if (filter.data.type == "CheckOrNull")
                    {
                        foreach (string _value in filter.data.value)
                        {




                            if (filter.data.comparison == "eqOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += "( m." + TranslateViwModelToModel(filter.field, EntityModel) + "=" + _value + "  or ";
                                    and = false;
                                }
                                else
                                {
                                    query += "(  m." + TranslateViwModelToModel(filter.field, EntityModel) + "=" + _value + "";
                                    and = true;
                                }
                            }
                            else if (filter.data.comparison == "eqgtOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += "( m." + TranslateViwModelToModel(filter.field, EntityModel) + ">" + _value + "  or ";
                                    and = false;
                                }
                                else
                                {
                                    query += "(  m." + TranslateViwModelToModel(filter.field, EntityModel) + "> " + _value + "";
                                    and = true;
                                }
                            }

                            else if (filter.data.comparison == "NoteqOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += "( m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + "  or ";
                                    and = false;
                                }
                                else
                                {
                                    query += "(  m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + "";
                                    and = true;
                                }
                            }
                            else
                                query += "(m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + "";
                        }


                    }

                    if (filter.data.type == "CheckOrNullEnd")
                    {
                        foreach (string _value in filter.data.value)
                        {




                            if (filter.data.comparison == "eqOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += " m." + TranslateViwModelToModel(filter.field, EntityModel) + "=" + _value + ") ";
                                    and = false;
                                }
                                else
                                {
                                    query += "  m." + TranslateViwModelToModel(filter.field, EntityModel) + "=" + _value + ")";
                                    and = true;
                                }
                            }

                            if (filter.data.comparison == "eqgtOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += " m." + TranslateViwModelToModel(filter.field, EntityModel) + ">" + _value + ") ";
                                    and = false;
                                }
                                else
                                {
                                    query += "  m." + TranslateViwModelToModel(filter.field, EntityModel) + ">" + _value + ")";
                                    and = true;
                                }
                            }
                            else if (filter.data.comparison == "NoteqOr")
                            {
                                if (hasAnd)
                                {
                                    query = query.Remove(query.Length - 4, 4);
                                    query += " Or ";
                                }
                                if (firstStep)
                                {
                                    query += " m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + ") ";
                                    and = false;
                                }
                                else
                                {
                                    query += "  m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + ")";
                                    and = true;
                                }
                            }
                            else
                                query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<>" + _value + ")";
                        }
                    }

                    if (filter.data.type == "sum")
                    {
                        query += "sum(m." + TranslateViwModelToModel(filter.field, EntityModel) + ")=" + Convert.ToInt32(filter.data.value[0]);
                        
                    }

                    if (filter.data.type == "size")
                    {
                        query += " Size(m." + TranslateViwModelToModel(filter.field, EntityModel) + ")>" + Convert.ToInt32(filter.data.value[0]);
                    }

                    maintemp++;
                    if (maintemp < mainCounter)
                    {
                        if (and)
                        {
                            query += " and ";
                            hasAnd = true;
                        }
                        firstStep = false;

                    }
                }
            }

            #endregion

            #region Preparing Sort

            if (sort != null && sort.Count()>0)
            {
                int sortCounter = sort.Count();
                int sorttemp = 0;

                query += " Order By ";
                foreach (Sort _sort in sort)
                {
                    query += TranslateViwModelToModelForSort(_sort.SortColumn, EntityModel) + " ";
                    if (_sort.Asc == true)
                        query += "asc";
                    else
                        query += "desc";
                    sorttemp++;
                    if (sorttemp < sortCounter)
                    {
                        query += ", ";
                    }

                }
            }

            #endregion

            return query;
        }

        #endregion

        #region Create Filter and Sort for lasyLoading

        public static string GenerateFilterAndSortForlasyLoading(IList<FilterData> Filters, string EntityModel, IList<Sort> sort)
        {
            string query = string.Empty;
            #region Preparing Filters
            if (Filters != null)
            {
                int mainCounter = Filters.Count();
                int maintemp = 0;
                
                foreach (FilterData filter in Filters)
                {

                    if (filter.data.type == "list")
                    {
                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + " in(";
                        int counter = filter.data.value.Count();
                        int temp = 0;
                        foreach (string _valu in filter.data.value)
                        {
                            temp++;
                            if (temp == counter)
                                query += "'" + _valu + "')";
                            else
                                query += "'" + _valu + "',";
                        }

                    }

                    if (filter.data.type == "numeric")
                    {
                        foreach (string _value in filter.data.value)
                        {
                            switch (filter.data.comparison)
                            {
                                case "eq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "=" + _value + "";
                                        break;
                                    }
                                case "lt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<" + _value;
                                        break;
                                    }
                                case "gt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + ">" + _value;
                                        break;
                                    }
                                case null:
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "Like '%" + _value + "%'";
                                        break;
                                    }
                            }
                        }
                    }

                    if (filter.data.type == "string")
                        foreach (string _value in filter.data.value)
                        {
                            query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                        }


                    if (filter.data.type == "date")
                    {
                        foreach (string _value in filter.data.value)
                        {
                            switch (filter.data.comparison)
                            {
                                case "eq":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "='" + _value + "'";
                                        break;
                                    }
                                case "lt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + "<'" + _value + "'";
                                        break;
                                    }
                                case "gt":
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + ">'" + _value + "'";
                                        break;
                                    }
                                case null:
                                    {
                                        query += "m." + TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                                        break;
                                    }
                            }
                        }
                    }
                    maintemp++;
                    if (maintemp < mainCounter)
                        query += " and ";
                }
            }
            #endregion

            #region Preparing Sort
            
            if (sort != null)
            {
                int sortCounter = sort.Count();
                int sorttemp = 0;

                query += " Order By ";
                foreach (Sort _sort in sort)
                {
                    query += " m." + TranslateViwModelToModel(_sort.SortColumn, EntityModel) + " ";
                    if (_sort.Asc == true)
                        query += "asc";
                    else
                        query += "desc";
                    sorttemp++;
                    if (sorttemp < sortCounter)
                    {
                        query += ", ";
                    }

                }
            }

            #endregion

            return query;
        }
        #endregion

        #region Create With Filter Only

        public static string GenerateFilterHQLQueryForCustomer(IList<FilterData> Filters, string EntityModel)
        {

            #region Preparing Filters

            int mainCounter = Filters.Count();
            int maintemp = 0;
            string query= string.Empty;
            foreach (FilterData filter in Filters)
            {

                if (filter.data.type == "list")
                {
                    query +=  TranslateViwModelToModel(filter.field, EntityModel) + " in(";
                    int counter = filter.data.value.Count();
                    int temp = 0;
                    foreach (string _valu in filter.data.value)
                    {
                        temp++;
                        if (temp == counter)
                            query += "'" + _valu + "')";
                        else
                            query += "'" + _valu + "',";
                    }

                }

                if (filter.data.type == "numeric")
                {
                    foreach (string _value in filter.data.value)
                    {
                        switch (filter.data.comparison)
                        {
                            case "eq":
                                {
                                    query +=TranslateViwModelToModel(filter.field, EntityModel) + "=" + _value + "";
                                    break;
                                }
                            case "lt":
                                {
                                    query += TranslateViwModelToModel(filter.field, EntityModel) + "<" + _value;
                                    break;
                                }
                            case "gt":
                                {
                                    query +=TranslateViwModelToModel(filter.field, EntityModel) + ">" + _value;
                                    break;
                                }
                            case null:
                                {
                                    query +=TranslateViwModelToModel(filter.field, EntityModel) + "Like '%" + _value + "%'";
                                    break;
                                }
                        }
                    }
                }

                if (filter.data.type == "string")
                    foreach (string _value in filter.data.value)
                    {
                        query += TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                    }


                if (filter.data.type == "date")
                {
                    foreach (string _value in filter.data.value)
                    {
                        switch (filter.data.comparison)
                        {
                            case "eq":
                                {
                                    query +=TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                                    break;
                                }
                            case "lt":
                                {
                                    query +=TranslateViwModelToModel(filter.field, EntityModel) + "<'" + _value + "'";
                                    break;
                                }
                            case "gt":
                                {
                                    query += TranslateViwModelToModel(filter.field, EntityModel) + ">'" + _value + "'";
                                    break;
                                }
                            case null:
                                {
                                    query += TranslateViwModelToModel(filter.field, EntityModel) + " Like '%" + _value + "%'";
                                    break;
                                }
                        }
                    }
                }
                maintemp++;
                if (maintemp < mainCounter)
                    query += " and ";
            }

            #endregion

            return query;
        }


        #endregion

        #region Create LasyLoading HQL 

        public static string CreateHQL(IList<FilterData> filter,IList<Sort> sort,string Entity,string[] Fields)
        {
            string query = string.Empty;

            query = "select ";
            int Counter = Fields.Count();
            int temp = 0;
            foreach (string field in Fields)
            {
                query += " m." + field;
                temp++;
                if (temp < Counter)
                    query += " , ";
                else
                    query += "";
            }
            query += " FROM " + Entity + " m ";

            query += FilterUtilityService.GenerateFilterAndSortForlasyLoading(filter,Entity,sort);
            



            return query;
        }

        #endregion

        #endregion

        #region Translate ViewModel properties to Model Properties

        private static string TranslateViwModelToModel(string filed,string EntityModel)
        {
            if (EntityModel == "LeadTitleTemplate")
            {
                switch (filed)
                {
                    case "GroupName":
                        {
                            return "Group.ID";
                        }
                }
            }
            if (EntityModel == "Negotiation")
            {
                switch (filed)
                {
                    case "ADSLPhone":
                    {
                        return "Customer.ADSLPhone";
                    }

                    case "CustomerName":
                    {
                        return "Customer.LastName";
                    }

                    case "LeadResulTitle":
                    {
                        return "LeadResultTemplate.ID";
                    }
                    case "LeadTitleTemplateTitle":
                    {
                        return "LeadTitleTemplate.ID";
                    }
                        
                }

            }
            if (EntityModel == "LeadResultTemplate")
            {
                switch (filed)
                {
                    case "GroupName":
                        {
                            return "Group.ID";
                        }
                }
            }
            if (EntityModel == "Notification")
            {
                switch (filed)
                {
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                }
            }
            if (EntityModel == "Courier")
            {
                switch (filed)
                {
                    case "SuctionModeName":
                        {
                            return "Sale.Customer.SuctionMode.SuctionModeName";
                        }
                }
            }

            if (EntityModel == "CampaignPayment")
            {
                switch (filed)
                {
                    case "SuctionModeDetailName":
                        {
                            return "SuctionModeDetail.ID";
                        }
                }
            }

            if (EntityModel == "SupportExpertDispatch")
            {
                switch (filed)
                {
                    case "Address":
                        {
                            return "Support.Customer.Address";
                        }
                    case "CenterName":
                        {
                            return "Support.Customer.Center.ID";
                        }

                    case "Balance":
                        {
                            return "Support.Customer.Balance";
                        }

                    case "NetworkName":
                        {
                            return "Support.Customer.Network.ID";
                        }
                    case "ExpertEmployeeName":
                        {
                            return "ExpertEmployee.ID";
                        }

                }
            }
            if (EntityModel == "CreditService")
            {
                switch (filed)
                {
                    case "NetworkName":
                        {
                            return "Network.ID";
                        }
                }
            }
            if (EntityModel == "ProductLog")
            {
                switch (filed)
                {
                    case "ProductName":
                        {
                            return "Product.ID";
                        }
                    case "StoreName":
                        {
                            return "Store.ID";
                        }
                }
            }
            if (EntityModel == "Center")
            {
                switch (filed)
                {
                    case "CenterName":
                        {
                            return "CenterName";
                        }
                    case "StatusFa":
                        {
                            return "Status";
                        }
                }
            }
            if (EntityModel == "MoneyAccount")
            {
                switch (filed)
                {
                    case "IsBankAccountToString":
                        {
                            return "IsBankAccount";
                        }

                }
            }
            if (EntityModel == "ProductSaleDetail")
            {
                switch (filed)
                {
                    case"StoreName":
                    {
                        return "DeliverStore.ID";
                        break;
                    }
                    case"CreateDate":
                    {
                        return "CreateDate";
                        break;
                    }
                    case"ProductName":
                    {
                        return "ProductPrice.Product.ID";
                    }
                }
                
            }
            if (EntityModel == "Fiscal")
            {
                switch (filed)
                {
                    case "ConfirmTxt":
                        {
                            return "Confirm";
                        }
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                    case "DocumentTypeTxt":
                        {
                            return "DocumentType";
                        }
                    case "ConfirmEmployeeName":
                        {
                            return "ConfirmEmployee.ID";
                        }
                    case "Balance":
                        {
                            return "Customer.Balance";
                        }
                }
                
            }
            if (EntityModel == "Customer")
            {
                switch (filed)
                {
                    case "CenterName":
                        {
                            return "Center.ID";
                        }
                    case "LevelTitle":
                        {
                            return "Level.ID";
                        }
                    case "AgencyName":
                        {
                            return "Agency.ID";
                        }
                    case "NetworkName":
                        {
                            return "Network.ID";
                        }
                    case "SuctionModeName":
                        {
                            return "SuctionMode.ID";
                        }
                    case "SuctionModeDetailName":
                        {
                            return "SuctionModeDetail.ID";
                        }
                    case "FollowStatusName":
                        {
                            return "FollowStatus.ID";
                        }
                    case "BuyPossibilityName":
                        {
                            return "BuyPossibility.ID";
                        }
                    case "DocumentStatusName":
                        {
                            return "DocumentStatus.ID";
                        }
                    case "Name":
                        {
                            return "LastName";
                        }
                    case "LevelTypeTitle":
                        {
                            return "Level.LevelType.ID";
                        }
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                }
            }
            if (EntityModel == "Fiscal")
            {
                switch (filed)
                {
                    case "ADSLPhone":
                        {
                            return "Customer.ADSLPhone";
                        }
                    case "CustomerName":
                        {
                            return "Customer.LastName";
                        }
                    case "MoneyAccountName":
                        {
                            return "MoneyAccount.ID";
                        }
                    case "DocumenttypeTest":
                            {
                                return "DocumentType";
                            }
                    case "CreateEmployeeName":
                            {
                                return "CreateEmployee.ID";
                            }
                    case "Balance":
                            {
                                return "Customer.Balance";
                            }
                }
            }
            if (EntityModel == "BonusComission")
            {
                switch (filed)
                {
                    case "CreateEmploye":
                        {
                            return "CreateEmployee.ID";
                        }
                }
            }

            if (EntityModel == "Sale")
            {
                switch (filed)
                {
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                    case "Name":
                        {
                            return "Customer.LastName";
                        }
                    case "ADSLPhone":
                        {
                            return "Customer.ADSLPhone";
                        }
                    case "CenterName":
                        {
                            return "Customer.Center.ID";
                        }
                    case "LevelTypeTitle":
                        {
                            return "Customer.Level.LevelType.ID";
                        }
                    case "LevelTitle":
                        {
                            return "Customer.Level.ID";
                        }
                    case "Balance":
                        {
                            return "Customer.Balance";
                        }
                    case "CanDeliverCost":
                        {
                            return "Customer.CanDeliverCost";
                        }
                    case "NetworkName":
                        {
                            return "Customer.Network.ID";
                        }
                    case "LevelEntryDate":
                        {
                            return "Customer.LevelEntryDate";
                        }
                    case "AgencyName":
                        {
                            return "Customer.Agency.ID";
                        }
                    case "BuyPossibilityName":
                        {
                            return "Customer.BuyPossibility.ID";
                        }
                    case "SuctionModeName":
                        {
                            return "Customer.SuctionMode.ID";
                        }
                    case "DocumentStatusName":
                        {
                            return "Customer.DocumentStatus.ID";
                        }
                    case "BirthDate":
                        {
                            return "Customer.BirthDate";
                        }
                    case "Job":
                        {
                            return "Cstomer.Job";
                        }


                    case "CustomerName":
                        {
                            return "Customer.ID";
                        }

                    case "Closed":
                        {
                            return "Closed";
                        }

                }

            }
            if (EntityModel == "UncreditSaleDetail" || EntityModel=="ProductSaleDetail" || EntityModel=="CreditSaleDetail")
            {
                switch (filed)
                {
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                    case "DeliverEmployeeName":
                        {
                            return "DeliverEmployee.ID";
                        }
                    case "SaleEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                    case "RollBackEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                    case "Products":
                        {
                            return "ProductPrice.Product.ID";
                        }
                    case "UncreditServices":
                        {
                            return "UncreditService.ID";
                        }
                    case "CreditService":
                        {
                            return "CreditService.ID";
                        }
                    case "SaleDate":
                        {
                            return "CreateDate";
                        }
                    case "RollbackDate":
                        {
                            return "CreateDate";
                        }
                    case "Networks":
                        {
                            return "CreditService.Network.ID";
                        }
                    case "ProductPrices":
                        {
                            return "ProductPrice.ID";
                        }
                    case "DeliverDate":
                        {
                            return "DeliverDate";
                        }
                    case "Delivered":
                        {
                            return "Delivered";
                        }
                    case "Rollbacked": 
                        {
                            return "Rollbacked";
                        }
                    case "Confirmed":
                        {
                            return "Sale.Closed";
                        }
                    case "CenterName":
                    {
                        return "Sale.Customer.Center.ID";
                    }
                }


            }
            if (EntityModel == "Support")
            {
                switch (filed)
                {
                    case "LevelTitle":
                    {
                        return "Customer.Level.ID";
                    }
                    case "SupportStatusName":
                        {
                            return "SupportStatus.ID";
                            break;
                        }
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.ID";
                            break;
                        }
                    case "ModifiedEmployeeName":
                        {
                            return "ModifiedEmployee.LastName";
                            break;
                        }
                    case "CustomerName":
                        {
                            return "Customer.LastName";
                            break;
                        }
                }
            }

            if (EntityModel == "Courier")
            {
                switch (filed)
                {
                    case "Address":
                        {
                            return "Sale.Customer.Address";
                        }
                    case "CenterName":
                        {
                            return "Sale.Customer.Center.ID";
                        }
                    case "SaleEmployeeName":
                        {
                            return "Sale.CreateEmployee.ID";
                        }
                    case "ADSLPhone":
                        {
                            return "Sale.Customer.ADSLPhone";
                        }

                }
            }

            if (EntityModel == "Employee")
            {
                switch (filed)
                {
                    case "GroupName":
                        {
                            return "Group.ID";
                        }
                }
            }

            return filed;
        }

        private static string TranslateViwModelToModelForSort(string filed, string EntityModel)
        {
            if (EntityModel == "LeadTitleTemplate")
            {
                switch (filed)
                {
                    case "GroupName":
                        {
                            return "Group.GroupName";
                        }
                }
            }
            if (EntityModel == "Negotiation")
            {
                switch (filed)
                {
                    case "ADSLPhone":
                        {
                            return "Customer.ADSLPhone";
                        }

                    case "CustomerName":
                        {
                            return "Customer.LastName";
                        }

                    case "LeadResulTitle":
                        {
                            return "LeadResultTemplate.LeadResulTitle";
                        }
                    case "LeadTitleTemplateTitle":
                        {
                            return "LeadTitleTemplate.Title";
                        }

                        
                }
            }
            if (EntityModel == "LeadResultTemplate")
            {
                switch (filed)
                {
                    case "GroupName":
                        {
                            return "Group.GroupName";
                        }
                }
            }
            if (EntityModel == "Notification")
            {
                switch (filed)
                {
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.LastName";
                        }
                }
            }

            if (EntityModel == "Courier")
            {
                switch (filed)
                {
                    case "SuctionModeName":
                        {
                            return "Sale.Customer.SuctionMode.SuctionModeName";
                        }
                }
            }

            if (EntityModel == "Employee")
            {
                switch (filed)
                {
                    case "GroupName":
                        {
                            return "Group.GroupName";
                        }
                }
            }

            if (EntityModel == "CampaignPayment")
            {
                switch (filed)
                {
                    case "SuctionModeDetailName":
                        {
                            return "SuctionModeDetail.SuctionModeDetailName";
                        }
                }
            }

            if (EntityModel == "SupportExpertDispatch")
            {
                switch (filed)
                {
                    case "Address":
                        {
                            return "Support.Customer.Address";
                        }
                    case "CenterName":
                        {
                            return "Support.Customer.Center.CenterName";
                        }

                    case "Balance":
                        {
                            return "Support.Customer.Balance";
                        }

                    case "NetworkName":
                        {
                            return "Support.Customer.Network.NetworkName";
                        }
                    case "ExpertEmployeeName":
                        {
                            return "ExpertEmployee.LastName";
                        }
                        
                }
            }

            if (EntityModel == "Courier")
            {
                switch (filed)
                {
                    case "Address":
                        {
                            return "Sale.Customer.Address";
                        }
                    case "CenterName":
                        {
                            return "Sale.Customer.Center.CenterName";
                        }
                    case "SaleEmployeeName":
                        {
                            return "Sale.CreateEmployee.LastName";
                        }
                    case "ADSLPhone":
                        {
                            return "Sale.Customer.ADSLPhone";
                        }
                        
                        
                        
                }
            }
            if (EntityModel == "CreditService")
            {
                switch (filed)
                {
                    case "NetworkName":
                        {
                            return "Network.ID";
                        }
                }
            }

            if (EntityModel == "Support")
            {
                switch (filed)
                {
                    case "SupportStatusName":
                        {
                            return "SupportStatus.SupportStatusName";
                            break;
                        }
                    case "LevelTitle":
                    {
                        return "Customer.Level.LevelTitle";
                    }
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.LastName";
                            break;
                        }
                    case "ModifiedEmployeeName":
                        {
                            return "ModifiedEmployee.LastName";
                            break;
                        }
                    case "CustomerName":
                        {
                            return "Customer.LastName";
                            break;
                        }
                    case "ExpertEmployeeName":
                    {
                        return "SupporExpertDispatch.ExpertEmployee.LastName";
                    }
                }
            }

            if (EntityModel == "ProductLog")
            {
                switch (filed)
                {
                    case "ProductName":
                        {
                            return "Product.ID";
                        }
                    case "StoreName":
                        {
                            return "Store.ID";
                        }
                }
            }
            if (EntityModel == "Center")
            {
                switch (filed)
                {
                    case "CenterName":
                        {
                            return "CenterName";
                        }
                    case "StatusFa":
                        {
                            return "Status";
                        }

                }
            }
            if (EntityModel == "MoneyAccount")
            {
                switch (filed)
                {
                    case "IsBankAccountToString":
                        {
                            return "IsBankAccount";
                        }

                }
            }
            if (EntityModel == "ProductSaleDetail")
            {
                switch (filed)
                {
                    case "StoreName":
                        {
                            return "DeliverStore.ID";
                            break;
                        }
                    case "CreateDate":
                        {
                            return "CreateDate";
                            break;
                        }
                    case "ProductName":
                        {
                            return "ProductPrice.Product.ID";
                        }
                }

            }
            if (EntityModel == "Fiscal")
            {
                switch (filed)
                {
                    case "ConfirmTxt":
                        {
                            return "Confirm";
                        }
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                    case "DocumentTypeTxt":
                        {
                            return "DocumentType";
                        }
                    case "ConfirmEmployeeName":
                        {
                            return "ConfirmEmployee.ID";
                        }
                }

            }
            if (EntityModel == "Customer")
            {
                switch (filed)
                {
                    case "CenterName":
                        {
                            return "Center.CenterName";
                        }
                    case "LevelTitle":
                        {
                            return "Level.LevelTitle";
                        }
                    case "AgencyName":
                        {
                            return "Agency.AgencyName";
                        }
                    case "NetworkName":
                        {
                            return "Network.ID";
                        }
                    case "SuctionModeName":
                        {
                            return "SuctionMode.SuctionModeName";
                        }
                    case "SuctionModeDetailName":
                        {
                            return "SuctionModeDetail.SuctionModeDetailName";
                        }
                    case "FollowStatusName":
                        {
                            return "FollowStatus.FollowStatusName";
                        }
                    case "BuyPossibilityName":
                        {
                            return "BuyPossibility.BuyPossibilityName";
                        }
                    case "DocumentStatusName":
                        {
                            return "DocumentStatus.DocumentStatusName";
                        }
                    case "Name":
                        {
                            return "LastName";
                        }
                    case "LevelTypeTitle":
                        {
                            return "Level.LevelType.LevelTypeTitle";
                        }
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.LastName";
                        }
                }
            }
            if (EntityModel == "Fiscal")
            {
                switch (filed)
                {
                    case "ADSLPhone":
                        {
                            return "Customer.ADSLPhone";
                        }
                    case "CustomerName":
                        {
                            return "Customer.LastName";
                        }
                    case "MoneyAccountName":
                        {
                            return "MoneyAccount.AccountName";
                        }
                    case "DocumenttypeTest":
                        {
                            return "DocumentType";
                        }
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                }
            
            }
            if (EntityModel == "BonusComission")
            {
                switch (filed)
                {
                    case "CreateEmploye":
                        {
                            return "CreateEmployee.LastName";
                        }
                }
            }
            if (EntityModel == "Sale")
            {
                switch (filed)
                {
                    case "CreateEmployeeName":
                    {
                        return "CreateEmployee.LastName";
                    }
                    case "Name" :
                    {
                        return "Customer.LastName";
                    }
                    case "ADSLPhone":
                    {
                        return "Customer.ADSLPhone";
                    }
                    case "CenterName":
                    {
                        return "Customer.Center.CenterName";
                    }
                    case "LevelTypeTitle":
                    {
                        return "Customer.Level.LevelType.Title";
                    }
                    case "LevelTitle":
                    {
                        return "Customer.Level.LevelTitle";
                    }
                    case "Balance":
                    {
                        return "Customer.Balance";
                    }
                    case "CanDeliverCost":
                    {
                        return "Customer.CanDeliverCost";
                    }
                    case "NetworkName":
                    {
                        return "Customer.Network.NetworkName";
                    }
                    case "LevelEntryDate":
                    {
                        return "Customer.LevelEntryDate";
                    }
                    case "AgencyName":
                    {
                        return "Customer.Agency.AgencyName";
                    }
                    case "BuyPossibilityName":
                    {
                        return "Customer.BuyPossibility.BuyPossibilityName";
                    }
                    case "SuctionModeName":
                    {
                        return "Customer.SuctionMode.SuctionModeName";
                    }
                    case "DocumentStatusName":
                    {
                        return "Customer.DocumentStatus.DocumentStatusName";
                    }
                    case "BirthDate":
                    {
                        return "Customer.BirthDate";
                    }
                    case "Job":
                    {
                        return "Cstomer.Job";
                    }


                    case "CustomerName":
                        {
                            return "Customer.LastName";
                        }

                    case "Closed":
                        {
                            return "Closed";
                        }


                }

            }
            if (EntityModel == "UncreditSaleDetail" || EntityModel == "ProductSaleDetail" || EntityModel == "CreditSaleDetail")
            {
                switch (filed)
                {
                    case "CreateEmployeeName":
                        {
                            return "CreateEmployee.LastName";
                        }
                    case "DeliverEmployeeName":
                        {
                            return "DeliverEmployee.ID";
                        }
                    case "SaleEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                    case "RollBackEmployeeName":
                        {
                            return "CreateEmployee.ID";
                        }
                    case "Products":
                        {
                            return "ProductPrice.Product.ID";
                        }
                    case "UncreditServices":
                        {
                            return "UncreditService.ID";
                        }
                    case "CreditService":
                        {
                            return "CreditService.ID";
                        }
                    case "SaleDate":
                        {
                            return "CreateDate";
                        }
                    case "RollbackDate":
                        {
                            return "CreateDate";
                        }
                    case "Networks":
                        {
                            return "CreditService.Network.ID";
                        }
                    case "ProductPrices":
                        {
                            return "ProductPrice.ID";
                        }
                    case "DeliverDate":
                        {
                            return "DeliverDate";
                        }
                    case "Delivered":
                        {
                            return "Delivered";
                        }
                    case "Rollbacked":
                        {
                            return "Rollbacked";
                        }
                    case "Confirmed":
                        {
                            return "Sale.Closed";
                        }
                    case "CenterName":
                        {
                            return "Sale.Customer.Center.ID";
                        }
                }
            }
            return filed;
        }

        #endregion

        
    }
}
