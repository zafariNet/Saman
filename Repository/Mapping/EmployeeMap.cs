#region Usings

using Model.Employees;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;
using Model.Sales;
using System;
using Model.Customers;

#endregion

namespace Repository.Mapping
{
    public class EmployeeMap : ClassMapping<Employee>
    {
        public EmployeeMap()
        {
            Table("Emp.Employee");

            #region Base Properties
            Id(x => x.ID, c => c.Column("EmployeeID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            //ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            #endregion

            #region Employee Properties
            ManyToOne(x => x.ParentEmployee, c => c.Column("ParentEmployeeID"));
            Property(x => x.LastName, c => c.Length(25));
            Property(x => x.FirstName, c => c.Length(20));
            ManyToOne(x => x.Group, c => c.Column("GroupID"));
            //Property(x => x.Permissions);
            Property(x => x.Phone, c => c.Length(20));
            Property(x => x.Mobile, c => c.Length(20));
            Property(x => x.Address);
            Property(x => x.BirthDate, c => c.Length(10));
            Property(x => x.Note);
            Property(x => x.InstallExpert);
            Property(x => x.LoginName, c => c.Length(100));
            Property(x => x.EncryptedPassword, c => { c.Length(500); c.Column("Password"); });
            //Property(x => x.Password, c => { c.Length(500); });
            Property(x => x.HireDate, c => c.Length(10));
            Property(x => x.Discontinued);
            Property(x=>x.Picture);
            #endregion

            #region Bags
            //Bag<Customer>(x => x.OwnCustomers,
            //    cp => { },
            //    cr => cr.OneToMany(x => x.Class(typeof(Customer))));

            //Bags
            //Bag(x => x.OwnCustomers,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Cus.Customer");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("EmployeeID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Customer))));
            //Bag(x => x.QueryEmployees,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Cus.QueryEmployee");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("EmployeeID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(QueryEmployee))));

            Bag(x => x.ChildEmployees,
            collectionMapping =>
            {

                collectionMapping.Table("Emp.Employee");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("ParentEmployeeID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Employee))));

            //Bag(x => x.Fiscals,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Fiscal.Fiscal");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("EmployeeID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Fiscals.Fiscal))));

            Bag(x => x.Permissions,
            collectionMapping =>
            {

                collectionMapping.Table("Emp.Permit");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
                collectionMapping.Key(k => k.Column("EmployeeID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Employees.Permit))));

            //Bag(x => x.MoneyAccountEmployees,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Fiscal.MoneyAccountEmployee");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("EmployeeID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Fiscals.MoneyAccountEmployee))));

            //Bag(x => x.Sales,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Sale.Sale");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("EmployeeID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Sales.Sale))));

            //Bag(x => x.PersenceSupports,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Sup.PersenceSupport");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("EmployeeID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Support.PersenceSupport))));

            Bag(x => x.Stores,
            collectionMapping =>
            {

                collectionMapping.Table("Store.Store");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("OwnerEmployeeID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Store.Store))));

            //// نماهایی که این مشتری می تواند ببیند
            Bag(x => x.QueriesThisEmployeeCanSee,
            collectionMapping =>
            {

                collectionMapping.Table("Cus.QueryEmployee");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("EmployeeID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Customers.QueryEmployee))));

            //Bag(x => x.Tasks,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Task.Task");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("EmployeeID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Tasks.Task))));


            #endregion
        }
    }
}