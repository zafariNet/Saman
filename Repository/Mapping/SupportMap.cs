using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Model.Support;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SupportMap:ClassMapping<Support>
    {
        public SupportMap()
        {
            Table("Sup.Support");

            // Base Properties
            Id(x => x.ID, c => c.Column("SupportID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            
            ManyToOne(x=>x.Customer,c=>c.Column("CustomerID"));
            ManyToOne(x => x.SupportStatus, c => c.Column("SupportStatusID"));
            

            //OneToOne(x => x.SupporExpertDispatch, x => x.PropertyReference(typeof(SupportExpertDispatch).GetProperty("Support")));
            //OneToOne(x => x.SupportPhoneInstallation, x => x.PropertyReference(typeof(SupportPhoneInstallation).GetProperty("Support")));
            //////OneToOne(x => x.SupportInstallationDelais, x => x.PropertyReference(typeof(SupportInstallationDelay).GetProperty("Support")));
            //OneToOne(x => x.SupportDeliverService, x => x.PropertyReference(typeof(SupportDeliverService).GetProperty("Support")));
            //OneToOne(x => x.SupportTicketWaitingResponse, x => x.PropertyReference(typeof(SupportTicketWaitingResponse).GetProperty("Support")));
            //OneToOne(x => x.SupportQc, x => x.PropertyReference(typeof(SupportQc).GetProperty("Support")));
            //OneToOne(x => x.SupportQcProblem, x => x.PropertyReference(typeof(SupportQcProblem).GetProperty("Support")));
            //OneToOne(x => x.SupportTicketWaiting, x => x.PropertyReference(typeof(SupportTicketWaiting).GetProperty("Support")));

            Bag(x => x.SupportExpertDispatch,
                collectionMapping =>
                {
                    collectionMapping.Table("Sup.SupportExpertDispatch");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("SupportID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof (SupportExpertDispatch))));


            Bag(x => x.SupportDeliverService,
                collectionMapping =>
                {
                    collectionMapping.Table("Sup.SupportDeliverService");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("SupportID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof (SupportDeliverService))));


            Bag(x => x.SupportInstallationDelay,
                collectionMapping =>
                {
                    collectionMapping.Table("Sup.SupportInstallationDelay");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("SupportID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof (SupportInstallationDelay))));


            Bag(x => x.SupportPhoneInstallation,
                collectionMapping =>
                {
                    collectionMapping.Table("Sup.SupportPhoneInstallation");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("SupportID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof (SupportPhoneInstallation))));


            Bag(x => x.SupportQc,
                collectionMapping =>
                {
                    collectionMapping.Table("Sup.SupportQc");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("SupportID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof(SupportQc))));


            Bag(x => x.SupportTicketWaiting,
                collectionMapping =>
                {
                    collectionMapping.Table("Sup.SupportTicketWaiting");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("SupportID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof(SupportTicketWaiting))));



            Bag(x => x.SupportTicketWaitingResponse,
                collectionMapping =>
                {
                    collectionMapping.Table("Sup.SupportTicketWaitingResponse");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("SupportID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof(SupportTicketWaitingResponse))));



            Property(x=>x.Confirmed);
            Property(x=>x.SupportTitle);
            Property(x=>x.SupportComment);
            Property(x=>x.Closed);
            Property(x=>x.CreateBy);



        }
    }
}
