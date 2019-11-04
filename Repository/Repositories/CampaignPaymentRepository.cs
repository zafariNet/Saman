using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Sales;
using Model.Sales.Interfaces;
using NHibernate.Transform;


namespace Repository.Repositories
{
    public class CampaignPaymentRepository:Repository<CampaignPayment>,ICampaignPaymentRepository
    {
        public CampaignPaymentRepository(IUnitOfWork uow):base(uow)
        {
            
        }
        public IList<SuctionModeCost> Report(string RegisterStartdate, string RegisterEndDate, string PaymentStartDate, string PaymentEndDate,int WhichReport,bool IsRanje,string InputSupportStartDate,string InputSupportEndDate,bool HasFiscal,IEnumerable<Guid?> SuctionModeDetailsIDs,IEnumerable<Guid> SuctionModeIDs,IEnumerable<Guid?> CenterIDs)
        {
            string query="";

            if (string.IsNullOrEmpty(RegisterStartdate))
                RegisterStartdate = "1111/01/01";
            if (string.IsNullOrEmpty(RegisterEndDate))
                RegisterEndDate = "9999/12/30";
            if (string.IsNullOrEmpty(PaymentStartDate))
                PaymentStartDate = "1111/01/01";
            if (string.IsNullOrEmpty(PaymentEndDate))
                PaymentEndDate = "9999/12/30";
            if (string.IsNullOrEmpty(InputSupportStartDate))
                InputSupportStartDate = "1111/01/01";
            if (string.IsNullOrEmpty(InputSupportEndDate))
                InputSupportEndDate = "9999/12/30";

            if (WhichReport == 4)
            {
                query =
                    string.Format(@"
                      select t5.CenterName,t5.CustomerPerCenter,t5.SuctionModeDetailID as 'SuctionModeDetailID',t5.SuctionModeDetailName,SUM(cp1.Amount) as Amount ,t5.SuctionModeName from 
(select cusmod.SuctionModeName,t3.SuctionModeDetailName,t3.SuctionModeDetailID,t4.CenterName,t4.[CustomerPerCenter] from cus.suctionmodedetail t3 inner join 
cus.SuctionMode cusmod on t3.SuctionModeID=cusmod.SuctionModeID inner join

	(select t1.CenterName,t2.CustomerPerCenter,SuctionModedetailID from Cus.Center t1 inner join
		(select cus1.SuctionModedetailID,cus1.CenterID,COUNT(cus1.CenterID) as 'CustomerPerCenter' from cus.Customer cus1");

                if (IsRanje)
                    query += " inner join sup.Support sup1 on sup1.CustomerID=cus1.CustomerID ";
                if (HasFiscal)
                    query += " inner join Fiscal.Fiscal fis on cus1.CustomerID=fis.CustomerID  ";
                query += " where ";

                string suctionModeDetailIDs = " cus1.SuctionModedetailID in(";
                if (SuctionModeDetailsIDs != null)
                {
                    foreach (var item in SuctionModeDetailsIDs)
                        suctionModeDetailIDs += string.Format("'{0}',", item);
                    suctionModeDetailIDs = suctionModeDetailIDs.Remove(suctionModeDetailIDs.Length - 1, 1);
                    query += suctionModeDetailIDs + ") and ";

                }
                string suctionModeIDs = " cus1.SuctionModeID in(";
                if (SuctionModeIDs != null)
                {
                    foreach (var item in SuctionModeIDs)
                        suctionModeIDs += string.Format("'{0}',", item);
                    suctionModeIDs = suctionModeDetailIDs.Remove(suctionModeIDs.Length - 1, 1);
                    query += suctionModeIDs + ") and ";

                }

                string centerIDs = " cus1.CenterID in(";
                if (CenterIDs != null)
                {
                    foreach (var item in CenterIDs)
                        centerIDs += string.Format("'{0}',", item);
                    centerIDs = centerIDs.Remove(centerIDs.Length - 1, 1);
                    query += centerIDs + ") and ";

                }


                query += string.Format(@" EXISTS  (
			
				select SuctionModeDetalID,sum(Amount) as AM from sales.CampaignPayment CP where LEFT(PaymentDate,10) between '{2}' and '{3}' and cus1.SuctionModedetailID=cp.SuctionModeDetalID group by SuctionModeDetalID )
		and cus1.CreateDate between '{0}' and '{1}' group by SuctionModedetailID,cus1.CenterID) t2
	on t1.CenterID=t2.CenterID) t4
on t3.SuctionModeDetailID=t4.SuctionModedetailID)  t5 inner join sales.CampaignPayment CP1 on t5.SuctionModeDetailID=cp1.SuctionModeDetalID

where LEFT(cp1.PaymentDate,10) between '{2}' and '{3}'
group by t5.CenterName,t5.CustomerPerCenter,t5.SuctionModeDetailID,t5.SuctionModeDetailName,t5.SuctionModeName", RegisterStartdate,
                        RegisterEndDate, PaymentStartDate, PaymentEndDate);
            }










            if (WhichReport == 3)
            {
                query =
                    string.Format(
                        @"select cp1.SuctionModeDetalID as 'SuctionModeDetailID',t5.CenterName,t5.[CustomerPerCenter],t5.SuctionModeDetailName,sum(Amount) as Amount ,t5.SuctionModeName from sales.CampaignPayment CP1 inner join
(select cusmod.SuctionModeName,t3.SuctionModeDetailName,t3.SuctionModeDetailID,t4.CenterName,t4.[CustomerPerCenter] from cus.suctionmodedetail t3 inner join 
cus.SuctionMode cusmod on t3.SuctionModeID=cusmod.SuctionModeID inner join

	(select t1.CenterName,t2.[CustomerPerCenter],SuctionModedetailID from Cus.Center t1 inner join
		(select SuctionModedetailID,CenterID,COUNT(CenterID) as 'CustomerPerCenter' from cus.Customer cus1 ");
                if (IsRanje)
                    query += "inner join sup.Support sup1 on sup1.CustomerID=cus1.CustomerID ";
                if (HasFiscal)
                    query += "inner join Fiscal.Fiscal fis on cus1.CustomerID=fis.CustomerID  ";
                query += " where ";

                string suctionModeDetailIDs = "cus1.SuctionModedetailID in(";
                if (SuctionModeDetailsIDs != null)
                {
                    foreach (var item in SuctionModeDetailsIDs)
                        suctionModeDetailIDs += string.Format("'{0}',", item);
                    //suctionModeDetailIDs = suctionModeDetailIDs.Remove(suctionModeDetailIDs.Length-1, 1);
                    suctionModeDetailIDs = suctionModeDetailIDs.Remove(suctionModeDetailIDs.Length - 1, 1);
                    query += suctionModeDetailIDs + ") and ";

                }
                string suctionModeIDs = " cus1.SuctionModeID in(";
                if (SuctionModeIDs != null)
                {
                    foreach (var item in SuctionModeIDs)
                        suctionModeIDs += string.Format("'{0}',", item);
                    //suctionModeIDs = suctionModeDetailIDs.Remove(suctionModeIDs.Length-1, 1);
                    suctionModeIDs = suctionModeIDs.Remove(suctionModeIDs.Length - 1, 1);
                    query += suctionModeIDs + ") and ";

                }

                string centerIDs = " cus1.CenterID in(";
                if (CenterIDs != null)
                {
                    foreach (var item in CenterIDs)
                        centerIDs += string.Format("'{0}',", item);
                    //centerIDs = centerIDs.Remove(centerIDs.Length-1, 1);
                    centerIDs = centerIDs.Remove(centerIDs.Length - 1, 1);
                    query += centerIDs + ") and ";

                }

                query +=string.Format( @"
		    EXISTS  (
				select SuctionModeDetalID,sum(Amount) as AM from sales.CampaignPayment CP where LEFT(PaymentDate,10) between '{2}' and '{3}' and cus1.SuctionModedetailID=cp.SuctionModeDetalID group by SuctionModeDetalID )
		and cus1.CreateDate between '{0}' and '{1}' ",RegisterStartdate,RegisterEndDate,PaymentStartDate,PaymentEndDate);
                if (IsRanje)
                    query += string.Format(" and sup1.CreateDate between '{0}' and '{1}'",InputSupportStartDate,InputSupportEndDate);
                query+=string.Format(@" group by SuctionModedetailID,CenterID) t2
	on t1.CenterID=t2.CenterID) t4
on t3.SuctionModeDetailID=t4.SuctionModedetailID) t5
on cp1.SuctionModeDetalID=t5.SuctionModeDetailID
where LEFT(cp1.PaymentDate,10) between '{0}' and '{1}'
 Group by CP1.SuctionModeDetalID,t5.CenterName,t5.[CustomerPerCenter],t5.SuctionModeDetailName,t5.SuctionModeName

", PaymentStartDate, PaymentEndDate);
                ;
            }
            var result =
                SessionFactory.GetCurrentSession()
                    .CreateSQLQuery(query)
                    .SetResultTransformer(Transformers.AliasToBean<SuctionModeCost>()).List<SuctionModeCost>();
            return result;

        }
    }
}
