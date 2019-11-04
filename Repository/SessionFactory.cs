#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Lead;
using Model.Leads;
using NHibernate;
using NHibernate.Cfg;
using System.Web;
using Repository.SessionStorage;
using NHibernate.Dialect;
using NHibernate.Driver;
using System.Data;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using Repository.Mapping;
using Model.Employees;
using NHibernate.Tool.hbm2ddl;
using Model.Sales;
using Model.Customers;
using Model.Store;
using Model.Fiscals;
using Model.Support;

using Model;
#endregion

namespace Repository
{
    public class SessionFactory
    {
        static ISessionFactory _sessionFactory;

        private static void Init()
        {
            Configuration config = new Configuration();
            config.SessionFactoryName("BuiltIt");
            
            config.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2008DialectBugFix>();
                db.Driver<SqlClientDriver>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.IsolationLevel = IsolationLevel.ReadCommitted;
                db.ConnectionString = System.Configuration
                            .ConfigurationManager
                            .ConnectionStrings["SamanCnn"].ToString();
                db.Timeout = 30;

                db.LogFormattedSql = true;
                db.LogSqlInConsole = true;
                db.AutoCommentSql = true;
            });

            HbmMapping mapping = GetMappings();
            config.AddDeserializedMapping(mapping, "Saman");

            SchemaMetadataUpdater.QuoteTableAndColumns(config);

            _sessionFactory = config.BuildSessionFactory();
        }

        protected static HbmMapping GetMappings()
        {
            #region Model Mapper



            ModelMapper mapper = new ModelMapper();

            #region Spport

            mapper.AddMapping<SupportMap>();
            mapper.AddMapping<SupportDeliverServiceMap>();
            mapper.AddMapping<SupportExpertDispatchMap>();
            mapper.AddMapping<SupportInstallationDelayMap>();
            mapper.AddMapping<SupportPhoneInstallationMap>();
            mapper.AddMapping<SupportQcMap>();
            mapper.AddMapping<SupportQcProblemMap>();
            mapper.AddMapping<SupportTicketWaitingMap>();
            mapper.AddMapping<SupportTicketWaitingResponseMap>();

            #endregion

            #region Lead

            mapper.AddMapping<LeadTitleTemplateMap>();
            mapper.AddMapping<LeadResultTemplateMap>();
            mapper.AddMapping<NegotiationMap>();

            #endregion

            mapper.AddMapping<QueueMap>();
            mapper.AddMapping<NotificationMap>();
            mapper.AddMapping<CampaignPaymentMap>();
            mapper.AddMapping<CampaignAgentMap>();
            mapper.AddMapping<BonusComissionMap>();
            mapper.AddMapping<AgencyMap>();
            mapper.AddMapping<MessageTemplateMap>();
            mapper.AddMapping<CenterMap>();
            mapper.AddMapping<CodeMap>();
            mapper.AddMapping<ConditionMap>();
            mapper.AddMapping<CreditSaleDetailMap>();
            mapper.AddMapping<CreditServiceMap>();
            mapper.AddMapping<CustomerLevelMap>();
            mapper.AddMapping<CustomerMap>();
            mapper.AddMapping<DocumentMap>();
            mapper.AddMapping<DocumentStatusMap>();
            mapper.AddMapping<NetworkCenterPriorityMap>();
            mapper.AddMapping<CallLogMap>();

            // Added By zafai
            mapper.AddMapping<SuctionModeDetailMap>();
            // Added By Zafari
            mapper.AddMapping<ToDoMap>();
            // Added By Zafari
            mapper.AddMapping<ToDoResultMap>();
            mapper.AddMapping<EmailMap>();
            mapper.AddMapping<EmployeeMap>();
            mapper.AddMapping<FiscalMap>();
            mapper.AddMapping<GroupMap>();
            mapper.AddMapping<LocalPhoneMap>();
            mapper.AddMapping<LevelConditionMap>();
            mapper.AddMapping<LevelLevelMap>();
            mapper.AddMapping<LevelMap>();
            mapper.AddMapping<LevelTypeMap>();
            mapper.AddMapping<MoneyAccountEmployeeMap>();
            mapper.AddMapping<MoneyAccountMap>();
            mapper.AddMapping<NetworkCenterMap>();
            mapper.AddMapping<NetworkCreditMap>();
            mapper.AddMapping<NetworkMap>();
            mapper.AddMapping<NoteMap>();
            mapper.AddMapping<PermissionMap>();
            mapper.AddMapping<PersenceSupportMap>();
            mapper.AddMapping<ProblemMap>();
            mapper.AddMapping<ProductCategoryMap>();
            mapper.AddMapping<ProductLogMap>();
            mapper.AddMapping<ProductMap>();
            mapper.AddMapping<ProductPriceMap>();
            mapper.AddMapping<ProductSaleDetailMap>();
            mapper.AddMapping<QueryEmployeeMap>();
            mapper.AddMapping<QueryMap>();
            mapper.AddMapping<SaleMap>();
            mapper.AddMapping<SmsMap>();
            mapper.AddMapping<SpecialNumberMap>();
            mapper.AddMapping<StoreMap>();
            mapper.AddMapping<StoreProductMap>();
            mapper.AddMapping<SuctionModeMap>();
            mapper.AddMapping<FollowStatusMap>();
            mapper.AddMapping<BuyPossibilityMap>();
            mapper.AddMapping<MainMenuMap>();
            mapper.AddMapping<UncreditSaleDetailMap>();
            mapper.AddMapping<UncreditServiceMap>();
            mapper.AddMapping<PermitMap>();
            mapper.AddMapping<SimpleEmployeeMap>();
            mapper.AddMapping<SystemCountersMap>();
            mapper.AddMapping<QueueLocalPhoneStoreMap>();
            mapper.AddMapping<LocalPhoneStoreEmployeeMap>();
            mapper.AddMapping<ProvinceMap>();
            mapper.AddMapping<LocalPhoneStoreMap>();
            mapper.AddMapping<CityMap>();

            mapper.AddMapping<SupportStatusMap>();
            mapper.AddMapping<SupportStatusRelationMap>();

            mapper.AddMapping<CourierMap>();
            mapper.AddMapping<CourierEmployeeMap>();
            //mapper.AddMapping<SimpleCustomerMap>();
            mapper.AddMapping<SmsEmployeeMap>();
            mapper.AddMapping<TaskMap>();
            mapper.AddMapping<AnswerMap>();
            mapper.AddMapping<QuestionMap>();
            mapper.AddMapping<QuestionAnswerMap>();
            mapper.AddMapping<CustomerContactTemplateMap>();
            #endregion

            #region HbmMapping

            HbmMapping mapping = mapper.CompileMappingFor(new[] 
            {
                //typeof(SimpleCustomer),

                #region Support

                typeof(Support),
                typeof(SupportExpertDispatch),
                typeof(SupportDeliverService),
                typeof(SupportInstallationDelay),
                typeof(SupportPhoneInstallation),
                typeof(SupportQc),
                typeof(SupportQcProblem),
                typeof(SupportTicketWaiting),
                typeof(SupportTicketWaitingResponse),
                
                #endregion

                typeof(Answer),
                typeof(Question),
                typeof(QuestionAnswer),
                typeof(CustomerContactTemplate),
                typeof(CallLog),


                #region Lead

                typeof(LeadTitleTemplate),
                typeof(LeadResultTemplate),
                typeof(Negotiation),

                #endregion

                typeof(NetworkCenterPriority),
                typeof(Notification),
                typeof(SmsEmployee),
                typeof(Queue),
                typeof(QueueLocalPhoneStore),
                typeof(LocalPhoneStoreEmployee),
                typeof(LocalPhoneStore),
                typeof(CampaignPayment),
                typeof(CampaignAgent),
                typeof(BonusComission),
                typeof(Courier),
                typeof(CourierEmployee),
                typeof(SupportStatus),
                typeof(SupportStatusRelation),
                typeof(SimpleEmployee),
                typeof(MessageTemplate),
                typeof(Agency),
                typeof(Center),
                typeof(Code),
                typeof(Condition),
                typeof(CreditSaleDetail),
                typeof(CreditService),
                typeof(CustomerLevel),
                typeof(SystemCounters),
                // Added By Zafari
                typeof(Task),
                // Added By Zafari

                // added By Zafari
                typeof(SuctionModeDetail),
                // Added By zafari
                typeof(ToDo),
                // Added By zafari
                typeof(ToDoResult),
                //Added By Zafari
                
                typeof(Customer),
                typeof(Document),
                typeof(DocumentStatus),
                typeof(Email),
                typeof(Employee),
                typeof(Fiscal),
                typeof(Group),
                typeof(LocalPhone),
                typeof(LevelCondition),
                typeof(LevelLevel),
                typeof(Level),
                typeof(LevelType),
                typeof(MoneyAccountEmployee),
                typeof(MoneyAccount),
                typeof(NetworkCenter),
                typeof(NetworkCredit),
                typeof(Network),
                typeof(Note),
                typeof(Permission),
                typeof(PersenceSupport),
                typeof(Problem),
                typeof(ProductCategory),
                typeof(ProductLog),
                typeof(Product),
                typeof(ProductPrice),
                typeof(ProductSaleDetail),
                typeof(QueryEmployee),
                typeof(Query),
                typeof(Sale),
                typeof(Sms),
                typeof(SpecialNumber),
                typeof(Store),
                typeof(StoreProduct),
                typeof(SuctionMode),
                typeof(FollowStatus),
                typeof(BuyPossibility),
                typeof(MainMenu),
                typeof(UncreditSaleDetail),
                typeof(UncreditService),
                typeof(Permit),
                ///<summary>
                ///ایجاد شده توسط محمد ظفری
                ///</summary>
                typeof(Province),
                ///<summary>
                ///ایجاد شده توسط محمد ظفری
                ///</summary>
                typeof(City)
            });

            #endregion

            return mapping;
        }

        private static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
                Init();

            return _sessionFactory;
        }

        private static ISession GetNewSession()
        {
            return GetSessionFactory().OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            ISessionStorageContainer sessionStorageContainer = SessionStorageFactory.GetStorageContainer();

            ISession currentSession = sessionStorageContainer.GetCurrentSession();

            if (currentSession == null)
            {
                currentSession = GetNewSession();
                sessionStorageContainer.Store(currentSession);
            }

            return currentSession;
        }
    }
}
