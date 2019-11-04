using Model.Customers;
using Model.Support;
using NHibernate.Mapping.ByCode.Conformist;
using Model.Sales;

namespace Repository.Mapping
{
    public class CustomerMap : ClassMapping<Customer>
    {
        public CustomerMap()
        {
            Lazy(true);

            Table("Cus.Customer");

            // Base Properties
            Id(x => x.ID, c => c.Column("CustomerID"));
            Property(x => x.CreateDate);
            Property(x => x.ModifiedDate);
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);



            // Customer Properties
            ManyToOne(x => x.Center, c => c.Column("CenterID"));
            ManyToOne(x => x.Agency, c => c.Column("AgencyID"));
            ManyToOne(x => x.Network, c => c.Column("NetworkID"));
            ManyToOne(x => x.SuctionMode, c => c.Column("SuctionModeID"));
            ManyToOne(x => x.SuctionModeDetail, c => c.Column("SuctionModeDetailID"));
            ManyToOne(x => x.FollowStatus, c => c.Column("FollowStatusID"));
            ManyToOne(x => x.BuyPossibility, c => c.Column("BuyPossibilityID"));
            ManyToOne(x => x.DocumentStatus, c => c.Column("DocumentStatusID"));
            
            ///<summary>
            ///شروع تغییرات توسط محمد ظفری
            ///</summary>
            //ManyToOne(x => x.City, c => c.Column("CityID"));
            //ManyToOne(x=>x.Province , c=>c.Column("ProvinceID"));
            ///<summary>
            ///پایان تغییرات توسط محمد ظفری
            ///</summary>
            Property(x => x.LastName);
            Property(x => x.FirstName);
            Property(x => x.Gender);
            Property(x => x.Balance);
            Property(x => x.BirthDate);
            Property(x => x.Job);
            Property(x => x.Phone);
            Property(x => x.Mobile1);
            Property(x => x.Mobile2);
            Property(x => x.SLastName);
            Property(x => x.SFirstName);
            Property(x => x.ADSLPhone);
            Property(x => x.LegalType);
            Property(x => x.Email);
            Property(x => x.Address);
            Property(x => x.Note);
            Property(x => x.Locked);
            ManyToOne(x => x.LockEmployee, c => c.Column("LockEmployeeID"));
            Property(x => x.LockNote);
            Property(x => x.SentToPap);
            Property(x => x.Discontinued);
            Property(x => x.CanDeliverCost);
            Property(x => x.LevelEntryDate);
            ManyToOne(x => x.Level, c => c.Column("LevelID"));
            ManyToOne(x=>x.SupportStatus,c=>c.Column("SupportStatusID"));
            Property(x=>x.AddedToSite);
            
            //Bags  

            //Bag(x => x.CustomerLevels,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.CustomerLevel");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("CustomerID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(CustomerLevel))));

            //Bag(x => x.Documents,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.Document");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("CustomerID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Document))));

            //Bag(x => x.Emails,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.Email");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("CustomerID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Email))));

            //Bag(x => x.Notes,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.Note");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("CustomerID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Note))));

            //Bag(x => x.Smss,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.Sms");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("CustomerID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Sms))));

            Bag(x => x.Fiscals,
            collectionMapping =>
            {
                collectionMapping.Table("Fiscal.Fiscal");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("CustomerID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Fiscals.Fiscal))));

            Bag(x => x.Sales,
            collectionMapping =>
            {
                collectionMapping.Table("Sales.Sale");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
                collectionMapping.Inverse(true);
                collectionMapping.Key(k => k.Column("CustomerID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Sale))));


            Bag(x => x.Supports,
            collectionMapping =>
            {
                collectionMapping.Table("Sales.Sale");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
                collectionMapping.Inverse(true);
                collectionMapping.Key(k => k.Column("CustomerID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Support))));


            //Bag(x => x.PersenceSupports,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Sup.PersenceSupport");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("CustomerID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Support.PersenceSupport))));

            //Bag(x => x.Problems,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Sup.Problem");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("CustomerID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Support.Problem))));


            //Bag(x => x.Tasks,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Task.Task");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("CustomerID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Tasks.Task))));

        }
    }
}