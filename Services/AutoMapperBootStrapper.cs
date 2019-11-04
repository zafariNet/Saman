#region Usings
using System;
using System.Linq;
using AutoMapper;
using Model.Customers;
using Model.Employees;
using Model.Fiscals;
using Model.Leads;
using Model.Sales;
using Model.Store;
using Model.Support;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;
using Services.ViewModels.Fiscals;
using Services.ViewModels.Sales;
using Services.ViewModels.Store;
using Services.ViewModels.Support;
using Model;
using Services.ViewModels;
using Services.ViewModels.Leads;
#endregion

namespace Services
{
    public class AutoMapperBootStrapper
    {
        public static void ConfigureAutoMapper()
        {
            #region Simple Customer

            Mapper.CreateMap<SimpleCustomer, SimpleCustomerView>();
            //    .ForMember(x => x.LevelTitle, c => c.MapFrom(m => m.Level.LevelTitle));

            #endregion

            #region CampaignAgent


            #endregion

            #region Customers

            #region Employees


            #region Simple Employee

            #endregion

            #region Agency

            Mapper.CreateMap<Agency, AgencyView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Province

            Mapper.CreateMap<Province, ProvinceView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region City

            Mapper.CreateMap<City, CityView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Center

            Mapper.CreateMap<Center, CenterView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));
            #endregion

            #region Code

            Mapper.CreateMap<Code, CodeView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.CenterName,
                        c => c.MapFrom(s => s.Center.CenterName));
            #endregion

            #region Condition

            Mapper.CreateMap<Condition, ConditionView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Customer

            Mapper.CreateMap<Customer, CustomerView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.CenterName,c => c.MapFrom(s => s.Center.CenterName))
                .ForMember(m => m.AgencyName,c => c.MapFrom(s => s.Agency.AgencyName))
                .ForMember(m => m.NetworkName,c => c.MapFrom(s => s.Network.NetworkName))
                .ForMember(m => m.SuctionModeName,c => c.MapFrom(s => s.SuctionMode.SuctionModeName))
                .ForMember(m => m.FollowStatusName,c => c.MapFrom(s => s.FollowStatus.FollowStatusName))
                .ForMember(m => m.BuyPossibilityName,c => c.MapFrom(s => s.BuyPossibility.BuyPossibilityName))
                .ForMember(m => m.DocumentStatusName,c => c.MapFrom(s => s.DocumentStatus.DocumentStatusName))
                .ForMember(m => m.SuctionModeDetailName, c => c.MapFrom(s => s.SuctionModeDetail.SuctionModeDetailName))
                //.ForMember(m=>m.LevelTypeTitle,c=>c.MapFrom(s=>s.Level.LevelType.Title))
                .ForMember(m=>m.LevelTitle,c=>c.MapFrom(s=>s.Level.LevelTitle))
                
                 .ForMember(m => m.LockEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.LockEmployee.FirstName, s.LockEmployee.LastName)));
             

            #endregion

            #region Customer

            Mapper.CreateMap<Customer, CustomerView1>();

                // For Base members:
                //.ForMember(m => m.ModifiedEmployeeName,
                //        c => c.MapFrom(s => String.Format("{0} {1}",
                //            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                //.ForMember(m => m.CreateEmployeeName,
                //        c => c.MapFrom(s => String.Format("{0} {1}",
                //            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

                // For Complex members:
                //.ForMember(m => m.CenterName,c => c.MapFrom(s => s.Center.CenterName))
                //.ForMember(m => m.AgencyName,c => c.MapFrom(s => s.Agency.AgencyName))
                //.ForMember(m => m.NetworkName,c => c.MapFrom(s => s.Network.NetworkName))
                //.ForMember(m => m.SuctionModeName, c => c.MapFrom(s => s.SuctionMode.SuctionModeName))
                //.ForMember(m => m.FollowStatusName, c => c.MapFrom(s => s.FollowStatus.FollowStatusName))
                //.ForMember(m => m.BuyPossibilityName,c => c.MapFrom(s => s.BuyPossibility.BuyPossibilityName))
                //.ForMember(m => m.DocumentStatusName,c => c.MapFrom(s => s.DocumentStatus.DocumentStatusName))
                //.ForMember(m => m.SuctionModeDetailName,c => c.MapFrom(s => s.SuctionModeDetail.SuctionModeDetailName))
                //.ForMember(m => m.LockEmployeeName,c => c.MapFrom(s => String.Format("{0} {1}",
                //            s.LockEmployee.FirstName, s.LockEmployee.LastName)));


            #endregion

            #region CustomerLevel

            Mapper.CreateMap<CustomerLevel, CustomerLevelView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.CustomerName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.Customer.FirstName, s.Customer.LastName)))
                .ForMember(m => m.LevelTitle, c => c.MapFrom(s => s.Level.LevelTitle));

            #endregion

            #region Document

            Mapper.CreateMap<Document, DocumentView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.CustomerName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.Customer.FirstName, s.Customer.LastName)));

            #endregion

            #region DocumentStatus

            Mapper.CreateMap<DocumentStatus, DocumentStatusView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Simple Employee
            Mapper.CreateMap<SimpleEmployee, SimpleEmployeeView>()
                            .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                
                // For other complex members:
                .ForMember(m => m.GroupName,
                        c => c.MapFrom(s => s.Group.GroupName))
                .ForMember(m => m.ParentEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ParentEmployee.FirstName, s.ParentEmployee.LastName)));

            #endregion

            #region Email

            Mapper.CreateMap<Email, EmailView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.CustomerName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.Customer.FirstName, s.Customer.LastName)));

            #endregion

            #region LevelLevel

            Mapper.CreateMap<LevelLevel, LevelLevelView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.LevelTitle,
                        c => c.MapFrom(s => s.Level.LevelTitle))
                .ForMember(m => m.RelatedLevelTitle,
                        c => c.MapFrom(s => s.RelatedLevel.LevelTitle))
                        ;

            #endregion

            #region Level

            Mapper.CreateMap<Level, LevelView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.GraphicalPropertiesV, c => c.MapFrom(s => s.GraphicalObjectProperties))
                .ForMember(m => m.LevelOptionsV, c => c.MapFrom(s => s.Options))
                ;

            Mapper.CreateMap<GraphicalProperties, GraphicalPropertiesView>();
            Mapper.CreateMap<LevelOptions, LevelOptionsView>();

            #endregion

            #region LevelCondition

            Mapper.CreateMap<LevelCondition, LevelConditionView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.LevelTitle,
                        c => c.MapFrom(s => s.Level.LevelTitle));

            #endregion

            #region LevelType

            Mapper.CreateMap<LevelType, LevelTypeView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region NetworkCenter

            Mapper.CreateMap<NetworkCenter, NetworkCenterView>()

               // For Base members:
               .ForMember(m => m.ModifiedEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
               .ForMember(m => m.CreateEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                //For Complex members:
               .ForMember(m => m.CenterName,
                       c => c.MapFrom(s => s.Center.CenterName))
               .ForMember(m => m.NetworkName,
                       c => c.MapFrom(s => s.Network.NetworkName))
                .ForMember(m => m.StatusStr, c => c.MapFrom(s => Status((int)s.Status)))
                .ForMember(m => m.StatusInt, c => c.MapFrom(s => (int)s.Status))
                .ForMember(m => m.Status, c => c.MapFrom(s => Status(s.Status)));
            #endregion

            #region Note

            Mapper.CreateMap<Note, NoteView>()

               // For Base members:
               .ForMember(m => m.ModifiedEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
               .ForMember(m => m.CreateEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

               // For Complex members:
               .ForMember(m => m.CustomerName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.Customer.FirstName, s.Customer.LastName)))
               .ForMember(m => m.LevelTitle,
                       c => c.MapFrom(s => s.Level.LevelTitle));

            #endregion

            #region Query

            Mapper.CreateMap<Query, QueryView>()

               // For Base members:
               .ForMember(m => m.ModifiedEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
               .ForMember(m=>m.PreLoad,c=>c.MapFrom(s=>s.PreLoad))
               .ForMember(m => m.CreateEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region QueryEmployee

            Mapper.CreateMap<QueryEmployee, QueryEmployeeView>()

               // For Base members:
                // This is a join table with 2 columns. So it 
                // is not any base member.

               // For Complex members:
               ;

            #endregion

            #region Sms

            Mapper.CreateMap<Sms, SmsView>()

               // For Base members:
               .ForMember(m => m.ModifiedEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
               .ForMember(m => m.CreateEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

               // For Complex members:
               .ForMember(m => m.CustomerName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.Customer.FirstName, s.Customer.LastName)));

            #endregion

            #region SpecialNumber

            Mapper.CreateMap<SpecialNumber, SpecialNumberView>()

               // For Base members:
               .ForMember(m => m.ModifiedEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
               .ForMember(m => m.CreateEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region SuctionMode

            Mapper.CreateMap<SuctionMode, SuctionModeView>()

               // For Base members:
               .ForMember(m => m.ModifiedEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
               .ForMember(m => m.CreateEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region SuctionModeDetail

            Mapper.CreateMap<SuctionModeDetail, SuctionModeDetailview>()
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ParentID, c => c.MapFrom(s => s.SuctionMode.ID));
            

            #endregion

            #region FollowStatus

            Mapper.CreateMap<FollowStatus, FollowStatusView>()

               // For Base members:
               .ForMember(m => m.ModifiedEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
               .ForMember(m => m.CreateEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region BuyPossibility

            Mapper.CreateMap<BuyPossibility, BuyPossibilityView>()

               // For Base members:
               .ForMember(m => m.ModifiedEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
               .ForMember(m => m.CreateEmployeeName,
                       c => c.MapFrom(s => String.Format("{0} {1}",
                           s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #endregion

            #region Level Question

            Mapper.CreateMap<Answer, AnswerView>()
                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(x => x.QuestionID, c => c.MapFrom(s => s.Question.ID))
                .ForMember(x => x.QuestionText, c => c.MapFrom(s => s.Question.QuestionText));

            Mapper.CreateMap<Question, QuestionView>()
                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));


            Mapper.CreateMap<QuestionAnswer, QuestionAnswerView>()
                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(x => x.ADSLPhone, c => c.MapFrom(s => s.Customer.ADSLPhone))
                .ForMember(x => x.CustomerName, c => c.MapFrom(s => s.Customer.Name))
                .ForMember(x => x.CustomerID, c => c.MapFrom(s => s.Customer.ID))
                .ForMember(x => x.QuestionID, c => c.MapFrom(s => s.Question.ID))
                .ForMember(x => x.QuestionText, c => c.MapFrom(s => s.Question.QuestionText))
                .ForMember(x => x.AnswerID, c => c.MapFrom(s => s.Answer.ID))
                .ForMember(x => x.AnswerText, c => c.MapFrom(s => s.Answer.AnswerText));

            #endregion

            #region Employee
            Mapper.CreateMap<Employee, EmployeeView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                
                // For other complex members:
                .ForMember(m => m.GroupName,
                        c => c.MapFrom(s => s.Group.GroupName))
                .ForMember(m => m.ParentEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ParentEmployee.FirstName, s.ParentEmployee.LastName)));

            Mapper.CreateMap<Employee, EmployeeWithChildView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));
            #endregion

            #region Courier Employee

            Mapper.CreateMap<CourierEmployee, CourierEmployeeView>()
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));
            #endregion

            #region  Group
            Mapper.CreateMap<Group, GroupView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.EmployeeCount,
                c => c.MapFrom(s => s.Employees != null ? s.Employees.Count() : 0))
                .ForMember(m => m.ParentGroupName,
                c => c.MapFrom(s => s.ParentGroup.GroupName))
                //.ForMember(m => m.GroupStaffID,
                //c => c.MapFrom(s => s.ParentGroup.GroupStaff))
                .ForMember(m => m.GroupStaffID,
                c => c.MapFrom(s => s.GroupStaff.ID));
            #endregion

            #region  LocalPhone

            Mapper.CreateMap<LocalPhone, LocalPhoneView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.OwnerEmployeeName, c => c.MapFrom(s => s.OwnerEmployee.Name));

            #endregion

            #region  Permission
            Mapper.CreateMap<Permission, PermissionView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));
            #endregion

            #region  Permit
            Mapper.CreateMap<Permit, PermitView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));
            #endregion
  
            

            #endregion
            
            #region Fiscals

            #region Fiscal
            Mapper.CreateMap<Fiscal, FiscalView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                            // For Complex members:
                .ForMember(m => m.CustomerName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.Customer.FirstName, s.Customer.LastName)))
                .ForMember(m => m.MoneyAccountName,
                        c => c.MapFrom(s => s.MoneyAccount.AccountName))
                //.ForMember(m => m.EmployeesWhoCanConfirm,
                //        c => c.MapFrom(s => s.MoneyAccount.EmployeesWhoCanConfirm))
                .ForMember(m => m.ConfirmEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ConfirmEmployee.FirstName, s.ConfirmEmployee.LastName)))
                            .ForMember(x=>x.Balance,c=>c.MapFrom(s=>s.Customer.Balance))
                .ForMember(m => m.ADSLPhone,
                    c => c.MapFrom(s => s.Customer.ADSLPhone));


            Mapper.CreateMap<Fiscal, FiscalRealView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.CustomerName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.Customer.FirstName, s.Customer.LastName)))
                .ForMember(m => m.MoneyAccountName,
                    c => c.MapFrom(s => s.MoneyAccount.AccountName))
                //.ForMember(m => m.EmployeesWhoCanConfirm,
                //        c => c.MapFrom(s => s.MoneyAccount.EmployeesWhoCanConfirm))
                .ForMember(m => m.ConfirmEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ConfirmEmployee.FirstName, s.ConfirmEmployee.LastName)))
                .ForMember(x => x.Balance, c => c.MapFrom(s => s.Customer.Balance))
                .ForMember(m => m.ADSLPhone,
                    c => c.MapFrom(s => s.Customer.ADSLPhone));

            #endregion

            #region MoneyAccount

            Mapper.CreateMap<MoneyAccount, MoneyAccountView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region MoneyAccountEmployee

            Mapper.CreateMap<MoneyAccountEmployee, MoneyAccountEmployeeView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #endregion

            #region Sales

            #region Sale

            Mapper.CreateMap<Sale, SaleView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.Address, c => c.MapFrom(s => s.Customer.Address))
                .ForMember(m => m.Mobile1, c => c.MapFrom(s => s.Customer.Mobile1))
                .ForMember(m => m.CenterName, c => c.MapFrom(s => s.Customer.Center.CenterName))
                .ForMember(m => m.Sname, c => c.MapFrom(s => String.Format("{0} {1}",
                    s.Customer.SFirstName, s.Customer.SLastName)))
                .ForMember(m=>m.LevelTitle,c=>c.MapFrom(s=>s.Customer.Level.LevelTitle))
                .ForMember(m => m.ADSLPhone, c => c.MapFrom(s => s.Customer.ADSLPhone));

            Mapper.CreateMap<Sale, SimpleSaleView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.Address, c => c.MapFrom(s => s.Customer.Address))
                .ForMember(m => m.Mobile1, c => c.MapFrom(s => s.Customer.Mobile1))
                .ForMember(m => m.CenterName, c => c.MapFrom(s => s.Customer.Center.CenterName))
                .ForMember(m => m.Sname, c => c.MapFrom(s => String.Format("{0} {1}",
                    s.Customer.SFirstName, s.Customer.SLastName)))
                .ForMember(m=>m.LevelTitle,c=>c.MapFrom(s=>s.Customer.Level.LevelTitle))
                .ForMember(m => m.ADSLPhone, c => c.MapFrom(s => s.Customer.ADSLPhone));



            #endregion

            #region CreditSaleDetail

            Mapper.CreateMap<CreditSaleDetail, CreditSaleDetailView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.CreditServiceName,
                    c => c.MapFrom(s => s.CreditService.ServiceName))
                .ForMember(m => m.PurchaseUnitPrice,
                    c => c.MapFrom(s => s.CreditService.PurchaseUnitPrice))
                .ForMember(m => m.SaleNumber,
                    c => c.MapFrom(s => s.Sale.SaleNumber));
                //.ForMember(m => m.RollbackedCreditSaleDetailViews,
                //        c => c.MapFrom(s => s.RollbackedCreditSaleDetails));

            #endregion

            #region Courier


            Mapper.CreateMap<Courier, CourierView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ADSLPhone, c => c.MapFrom(s => s.Sale.Customer.ADSLPhone))
                .ForMember(m=>m.CustomerID,c=>c.MapFrom(s=>s.Sale.Customer.ID))
                .ForMember(m=>m.SuctionModeName,c=>c.MapFrom(s=>s.Sale.Customer.SuctionMode.SuctionModeName))
                .ForMember(m => m.Address, c => c.MapFrom(s => s.Sale.Customer.Address))
                .ForMember(m => m.CenterName, c => c.MapFrom(s => s.Sale.Customer.Center.CenterName))
                .ForMember(m=>m.Bonus,c=>c.MapFrom(s=>s.Bonus))
                .ForMember(m => m.SaleEmployeeName,
                    c =>
                        c.MapFrom(
                            s =>
                                String.Format("{0} {1}", s.Sale.CreateEmployee.FirstName, s.Sale.CreateEmployee.LastName)))
                .ForMember(m => m.CustomerName, c => c.MapFrom(s => s.Sale.Customer.Name))
                .ForMember(m => m.CourierEmployeeName,
                    c =>
                        c.MapFrom(s => String.Format("{0} {1}", s.CourierEmployee.FirstName, s.CourierEmployee.LastName)));
                

            #endregion

            #region System Counters

            Mapper.CreateMap<SystemCounters, SystemCountersView>();


            #endregion

            #region ProductSaleDetail

            Mapper.CreateMap<ProductSaleDetail, ProductSaleDetailView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.ProductPriceTitle,
                        c => c.MapFrom(s => s.ProductPrice.ProductPriceTitle))
                                        .ForMember(m => m.DeliverStoreName,
                        c => c.MapFrom(s => s.DeliverStore.StoreName))
                        .ForMember(m => m.DeliverStoreID,
                        c => c.MapFrom(s => s.DeliverStore.ID))
                .ForMember(m => m.SaleNumber,
                        c => c.MapFrom(s => s.Sale.SaleNumber));
                //.ForMember(m => m.RollbackedProductSaleDetailViews,
                //        c => c.MapFrom(s => s.RollbackedProductSaleDetails));
                        

            #endregion

            #region UncreditSaleDetail

            Mapper.CreateMap<UncreditSaleDetail, UncreditSaleDetailView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c =>
                        c.MapFrom(
                            s => String.Format("{0} {1}", s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}", s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                // For Complex members:
                .ForMember(m => m.UncreditServiceName, c => c.MapFrom(s => s.UncreditService.UncreditServiceName))
                //.ForMember(m => m.UnitPrice, c => c.MapFrom(s => s.UnitPrice))
                .ForMember(m => m.SaleNumber, c => c.MapFrom(s => s.Sale.SaleNumber));
                //.ForMember(m => m.RollbackedUncreditSaleDetailViews,
                //        c => c.MapFrom(s => s.RollbackedUncreditSaleDetails))
                
            #endregion

            #endregion

            #region Store

            #region CreditService

            Mapper.CreateMap<CreditService, CreditServiceView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.NetworkName,
                        c => c.MapFrom(s => s.Network.NetworkName));

            #endregion

            #region Network

            Mapper.CreateMap<Network, NetworkView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            Mapper.CreateMap<Network, NetworkSummaryView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region NetworkCredit

            Mapper.CreateMap<NetworkCredit, NetworkCreditView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))

                .ForMember(m => m.FromAccountTitle,
                        c => c.MapFrom(s => s.FromAccount.AccountName))
                .ForMember(m => m.NetworkName,
                        c => c.MapFrom(s => s.Network.NetworkName));

            #endregion

            #region Product

            Mapper.CreateMap<Product, ProductView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m=>m.TodalInStores,c=>c.MapFrom(s=>s.StoreProducts.Sum(x=>x.UnitsInStock)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName))) ;

            #endregion

            #region ProductCategory

            Mapper.CreateMap<ProductCategory, ProductCategoryView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region ProductLog

            Mapper.CreateMap<ProductLog, ProductLogView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m=>m.ParentProduct,c=>c.MapFrom(s=>s.Product.ProductName))
                
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));


            #endregion

            #region ProductPrice

            Mapper.CreateMap<ProductPrice, ProductPriceView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ProductName,
                        c => c.MapFrom(s => s.Product.ProductName))
                .ForMember(m=>m.ProductPriceCode,c=>c.MapFrom(s=>s.ProductPriceCode));

            #endregion

            #region Store

            Mapper.CreateMap<Store, StoreView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                // For Complex members:
                .ForMember(m => m.OwnerEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.OwnerEmployee.FirstName, s.OwnerEmployee.LastName)));

            #endregion

            #region StoreProduct

            Mapper.CreateMap<StoreProduct, StoreProductView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                                            .ForMember(m => m.StoreName,
                        c => c.MapFrom(s =>s.Store.StoreName))
                        .ForMember(m=>m.OwnerEmployeeName,c=>c.MapFrom(s=>s.Store.OwnerEmployee.Name))
                .ForMember(m => m.ProductName,
                        c => c.MapFrom(s => s.Product.ProductName));

            #endregion

            #region UncreditService

            Mapper.CreateMap<UncreditService, UncreditServiceView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #endregion

            #region Support

            #region PersenceSupport

            Mapper.CreateMap<PersenceSupport, PersenceSupportView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Problem

            Mapper.CreateMap<Problem, ProblemView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Support Status

            Mapper.CreateMap<SupportStatus, SupportStatusView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Support Status Relation

            Mapper.CreateMap<SupportStatusRelation, SupportStatusRelationView>()

                // For Base members:
                .ForMember(m => m.SupportStatusName,
                    c => c.MapFrom(s => s.SupportStatus.SupportStatusName))
                    .ForMember(m=>m.Key,c=>c.MapFrom(s=>s.RelatedSupportStatus.Key))
                .ForMember(m => m.RelatedSupportStatusName,
                    c => c.MapFrom(s => s.RelatedSupportStatus.SupportStatusName));

            #endregion

            #region Spport

            Mapper.CreateMap<Support, SupportView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                            .ForMember(m=>m.ADSLPhone,c=>c.MapFrom(s=>s.Customer.ADSLPhone))
                            .ForMember(m=>m.CustomerID,c=>c.MapFrom(s=>s.Customer.ID))
                .ForMember(m=>m.SupportDeliverServices,c=>c.MapFrom(s=>s.SupportDeliverService))
                .ForMember(m=>m.LevelTitle,c=>c.MapFrom(s=>s.Customer.Level.LevelTitle))
                .ForMember(m=>m.CustomerName , c=>c.MapFrom(s=>s.Customer.Name));
            #endregion

            #region Support Expert Dispatch

            Mapper.CreateMap<SupportExpertDispatch, SupportExpertDispatchView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ExpertEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ExpertEmployee.FirstName, s.ExpertEmployee.LastName)));


            Mapper.CreateMap<SupportExpertDispatch, SupportOwnView>()

    // For Base members:
            .ForMember(m => m.ModifiedEmployeeName,
                c => c.MapFrom(s => String.Format("{0} {1}",
                    s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
            .ForMember(m => m.CreateEmployeeName,
                c => c.MapFrom(s => String.Format("{0} {1}",
                    s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
            .ForMember(m=>m.Address,c=>c.MapFrom(s=>s.Support.Customer.Address))
            .ForMember(m=>m.ADSLPhone,c=>c.MapFrom(s=>s.Support.Customer.ADSLPhone))
            .ForMember(m=>m.Balance,c=>c.MapFrom(s=>s.Support.Customer.Balance))
            .ForMember(m=>m.CenterName,c=>c.MapFrom(s=>s.Support.Customer.Center.CenterName))
            .ForMember(m=>m.CustomerName,c=>c.MapFrom(s=>s.Support.Customer.Name))
            .ForMember(m=>m.NetworkName,c=>c.MapFrom(s=>s.Support.Customer.Network.NetworkName))
            .ForMember(m=>m.CustomerID,c=>c.MapFrom(s=>s.Support.Customer.ID))
            .ForMember(m=>m.SupportID,c=>c.MapFrom(s=>s.Support.ID))
            //.ForMember(m=>m.HasNotDeliveredProducts,c=>c.MapFrom(s=>s.Support.Customer.Sales.Where(x=>x.ProductSaleDetails.Where(v=>v.IsRollbackDetail!=true)
            //    .Where(v=>v.Delivered!=true)).Count()>0?true:false))
            .ForMember(m => m.ExpertEmployeeName,
                c => c.MapFrom(s => String.Format("{0} {1}",
                    s.ExpertEmployee.FirstName, s.ExpertEmployee.LastName)));


            #endregion

            #region Support Deliver Service

            Mapper.CreateMap<SupportDeliverService, SupportDeliverServiceView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Support Installation Delay


            Mapper.CreateMap<SupportInstallationDelay, SupportInstallationDelayView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Support Phone Installation

            Mapper.CreateMap<SupportPhoneInstallation, SupportPhoneInstallationView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Support Qc

            Mapper.CreateMap<SupportQc, SupportQcView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ExpertCover, c => c.MapFrom(s => TypeConvert((int) s.ExpertCover)))
                .ForMember(m => m.ExpertBehavior, c => c.MapFrom(s => TypeConvert((int) s.ExpertBehavior)))
                .ForMember(m => m.SaleAndService, c => c.MapFrom(s => TypeConvert((int) s.SaleAndService)));

            #endregion

            #region Support Qc Problem

            Mapper.CreateMap<SupportQcProblem, SupportQcProblemView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.InstallerEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.InstallerEmployee.FirstName, s.InstallerEmployee.LastName)));

            #endregion

            #region Support Ticket Waiting

            Mapper.CreateMap<SupportTicketWaiting, SupportTicketWaitingView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.InstallExpertName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.InstallExpert.FirstName, s.InstallExpert.LastName)));

            #endregion

            #region Support Ticket Waiting Response

            Mapper.CreateMap<SupportTicketWaitingResponse, SupportTicketWaitingResponseView>()

                // For Base members:
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #endregion

            #region General

            Mapper.CreateMap<MainMenu, MainMenuView>();

            #endregion

            #region Message Template
            Mapper.CreateMap<MessageTemplate, MessageTemplateView>()
                            .ForMember(m => m.ModifiedEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CreateEmployeeName,
                        c => c.MapFrom(s => String.Format("{0} {1}",
                            s.CreateEmployee.FirstName, s.CreateEmployee.LastName)));

            #endregion

            #region Notificstion 

            Mapper.CreateMap<Notification, NotificationView>()
                .ForMember(m => m.ModifiedEmployeeName,
            c => c.MapFrom(s => String.Format("{0} {1}",
                s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
    .ForMember(m => m.CreateEmployeeName,
            c => c.MapFrom(s => String.Format("{0} {1}",
                s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(x=>x.ReferedEmployeeName,c=>c.MapFrom(s=>s.ReferedEmployee.Name))
                ;

            #endregion

            #region Bonus and Comission

            Mapper.CreateMap<BonusComission, BonusComissionView>()
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(x => x.CustomerName, c => c.MapFrom(s => s.Customer.Name))
                .ForMember(x => x.CreditSaleDetailName,
                    c => c.MapFrom(s => s.CreditSaleDetail.CreditService.ServiceName))
                .ForMember(x => x.UnCreditSaleDetailName,
                    c => c.MapFrom(s => s.UnCreditSaleDetail.UncreditService.UncreditServiceName))
                .ForMember(m=>m.CreateEmployeeName,c=>c.MapFrom(s=>s.CreateEmployee.Name))
                .ForMember(x => x.ProductSaleDetailName,
                    c => c.MapFrom(s => s.ProductSaleDetail.ProductPrice.ProductPriceTitle));

            #endregion

            #region Campaign Agent

            Mapper.CreateMap<CampaignAgent, CampaignAgentView>()
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)));

            #endregion

            #region Campaign Payment

            Mapper.CreateMap<CampaignPayment, CampaignPaymentView>()
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(x => x.CampaignAgentName, c => c.MapFrom(s => s.CampaignAgent.CampaignAgentName))
                .ForMember(x=>x.SuctionModeName,c=>c.MapFrom(s=>s.SuctionModeDetail.SuctionMode.SuctionModeName))
                .ForMember(x => x.SuctionModeID, c => c.MapFrom(s => s.SuctionModeDetail.SuctionMode.ID))
                .ForMember(x => x.SuctionModeDetailID, c => c.MapFrom(s => s.SuctionModeDetail.ID))
                .ForMember(x => x.SuctionModeDetailName, c => c.MapFrom(s => s.SuctionModeDetail.SuctionModeDetailName));

            #endregion

            #region Queue Employee
            Mapper.CreateMap<QueueLocalPhoneStore, QueueLocalPhoneStoreView>()
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                        .ForMember(m=>m.OwnerEmployeeID,c=>c.MapFrom(s=>s.OwnerEmployee.ID))
                .ForMember(m => m.OwnerEmployeeName, c => c.MapFrom(s => s.OwnerEmployee.Name))
                .ForMember(m => m.PersianName, c => c.MapFrom(s => s.Queue.PersianName))
                .ForMember(m => m.QueueID, c => c.MapFrom(s => s.Queue.ID))
                .ForMember(m => m.QueueName, c => c.MapFrom(s => s.Queue.QueueName));

            #endregion

            #region Local Phone Employee

            Mapper.CreateMap<LocalPhoneStoreEmployee, LocalPhoneStoreEmployeeView>()
                                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                        .ForMember(m => m.LocalPhoneStoreID, c => c.MapFrom(s => s.LocalPhoneStore.ID))
                        .ForMember(m=>m.AsteriskID,c=>c.MapFrom(s=>s.LocalPhoneStore.AsteriskID))
                        .ForMember(m => m.LocalPhoneNumber, c => c.MapFrom(s => s.LocalPhoneStore.LocalPhoneStoreNumber))
                        .ForMember(m => m.OwnerEmployeeName, c => c.MapFrom(s => s.OwnerEmployee.Name));

            #endregion

            #region Queue

            Mapper.CreateMap<Queue, QueueView>();
            

            #endregion

            #region Local Phone

            Mapper.CreateMap<LocalPhoneStore, LocalPhoneStoreView>().ForMember(m => m.LocalPhoneNumber, c => c.MapFrom(s => s.LocalPhoneStoreNumber));

            #endregion

            #region Sms Employee

            Mapper.CreateMap<SmsEmployee, SmsEmployeeView>()
                .ForMember(x => x.OwnerEmployeeName, c => c.MapFrom(s => s.Employee.Name))
                .ForMember(x => x.OwneremployeeID, c => c.MapFrom(s => s.Employee.ID));

            #endregion

            #region Task

            Mapper.CreateMap<Task, TaskOwnView>()
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(x => x.ADSLPhone, c => c.MapFrom(s => s.Customer.ADSLPhone))
                .ForMember(x => x.CustomerName, c => c.MapFrom(s => s.Customer.Name))
                .ForMember(x => x.ReferedEmployeeName, c => c.MapFrom(s => s.ReferedEmployee.Name));
            //.ForMember(x => x.DetailTasks,
            //    c =>
            //        c.MapFrom(
            //            s =>
            //                s.CanEditDetail));

            #endregion

            #region Lead

            Mapper.CreateMap<LeadTitleTemplate, LeadTitleTemplateView>().ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                        .ForMember(m=>m.GroupName,c=>c.MapFrom(s=>s.Group.GroupName))
                        .ForMember(m=>m.GroupID,c=>c.MapFrom(s=>s.Group.ID));


            Mapper.CreateMap<LeadResultTemplate, LeadResultTemplateView>().ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                        .ForMember(m => m.GroupName, c => c.MapFrom(s => s.Group.GroupName))
                        .ForMember(m => m.GroupID, c => c.MapFrom(s => s.Group.ID));


            Mapper.CreateMap<Negotiation, NegotiationView>().ForMember(m => m.CreateEmployeeName,
                c => c.MapFrom(s => String.Format("{0} {1}",
                    s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(x => x.ReferedEmployeeID, c => c.MapFrom(s => s.ReferedEmployee.ID))
                .ForMember(x => x.ReferedEmployeeName, c => c.MapFrom(s => s.ReferedEmployee.Name))
                .ForMember(x => x.CustomerID, c => c.MapFrom(s => s.Customer.ID))
                .ForMember(x => x.CustomerName, c => c.MapFrom(s => String.Format("{0} {1}",
                    s.Customer.FirstName, s.Customer.LastName)))
                    .ForMember(x=>x.ADSLPhone,c=>c.MapFrom(s=>s.Customer.ADSLPhone))
                .ForMember(x => x.LeadResulTitle, c => c.MapFrom(s => s.LeadTitleTemplate.Title))
                .ForMember(x => x.LeadTitleTemplateID, c => c.MapFrom(s => s.LeadTitleTemplate.ID))
                .ForMember(x => x.LeadResulTitle, c => c.MapFrom(s => s.LeadResultTemplate.LeadResulTitle))
                .ForMember(x=>x.NegotiationResultDescription,c=>c.MapFrom(s=>s.LeadResultTemplate.Description))
                .ForMember(x => x.LeadResultTemplateID, c => c.MapFrom(s => s.LeadResultTemplate.ID));

            Mapper.CreateMap<CallLog, CallLogView>().ForMember(m => m.CreateEmployeeName,
                c => c.MapFrom(s => String.Format("{0} {1}",
                    s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(m => m.CustomerContactTemplateTitle, c => c.MapFrom(s => s.CustomerContactTemplate.Title))
                .ForMember(m => m.CustomerContactTemplateDescription,
                    c => c.MapFrom(s => s.CustomerContactTemplate.Description))
                .ForMember(m => m.CustomerID, c => c.MapFrom(s => s.Customer.ID))
                .ForMember(m => m.CustomerName, c => c.MapFrom(s => s.Customer.Name));

            #endregion

            #region Customer Contact

            Mapper.CreateMap<CustomerContactTemplate, CustomerContactTemplateView>()
                .ForMember(m => m.CreateEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.CreateEmployee.FirstName, s.CreateEmployee.LastName)))
                .ForMember(m => m.ModifiedEmployeeName,
                    c => c.MapFrom(s => String.Format("{0} {1}",
                        s.ModifiedEmployee.FirstName, s.ModifiedEmployee.LastName)))
                .ForMember(x => x.GroupName, c => c.MapFrom(s => s.Group.GroupName))
                .ForMember(x => x.GroupID, c => c.MapFrom(s => s.Group.ID));


            #endregion

            #region Network Center Priority

            Mapper.CreateMap<NetworkCenterPriority, NetworkCenterPriorityView>()
                .ForMember(m => m.NetworkID, c => c.MapFrom(s => s.Network.ID))
                .ForMember(m => m.NetworkName, c => c.MapFrom(s => s.Network.NetworkName))
                .ForMember(m => m.CenterID, c => c.MapFrom(s => s.Center.ID))
                .ForMember(m => m.CenterName, c => c.MapFrom(s => s.Center.CenterName));

            #endregion
            
        }

        #region Status Converter

        public static string Status(int status)
        {
            switch (status)
            {
                case 1: return "تحت پوشش";
                case 2: return "عدم پوشش";
                case 3: return "عدم امکان موقت";
                default: return "مشخص نشده";
            }
        }

        public static Status Status(NetworkCenterStatus status)
        {
            Status result = new Status();
            result.Value = (int)status;
            result.Text = Status((int)status);

            return result;
        }

        #endregion

        #region Type Converter

        public static string TypeConvert(int type)
        {
            switch (type)
            {
                case 1:
                {
                    return "عالی";
                    break;
                }

                case 2:
                {
                    return "خوب";
                    break;
                }
                case 3:
                {
                    return "متوسط";
                    break;
                }
                case 4:
                {
                    return "ضعیف";
                    break;
                }
            }
            return "نامشحص";
        }

        #endregion




        public static string ConvertToUrl(string path)
        {
            return path.Replace(@"\", "/").Substring(path.IndexOf("data"));
        }


    }
    
}