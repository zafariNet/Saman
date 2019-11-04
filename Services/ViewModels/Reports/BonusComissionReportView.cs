using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Reports
{

    public class BonusComissionDetail
    {
        public BonusComissionDetail()
        {
            
        }
        /// <summary>
        /// کارشناس جاری
        /// </summary>
        public long CurrentEmployeeBC { get; set; }

        public long SumBC { get; set; }

        public long BestEmployeeBC { get; set; }

        public string CurrentEmployeeBCName { get; set; }

        public string BestEmployeeBCName { get; set; }
    }

    public class BonusComissionReport
    {
        public BonusComissionReport()
        {
            Today=new BonusComissionDetail();
            YesterDay=new BonusComissionDetail();
            CurrentMounth=new BonusComissionDetail();
            LastMounth=new BonusComissionDetail();
        }
        /// <summary>
        /// امتیازات امروز
        /// </summary>
        public BonusComissionDetail Today { get; set; }

        /// <summary>
        /// امتیازات دیروز
        /// </summary>
        public BonusComissionDetail YesterDay { get; set; }

        /// <summary>
        /// هفته جاری
        /// </summary>
        public BonusComissionDetail CurrentWeek { get; set; }

        /// <summary>
        /// ماه جاری
        /// </summary>
        public BonusComissionDetail CurrentMounth { get; set; }

        /// <summary>
        /// ماه قبل
        /// </summary>
        public BonusComissionDetail LastMounth { get; set; }

    }

    public class BonusComissionReportView : BaseView
    {
        public BonusComissionReportView()
        {
            Bonus=new BonusComissionReport();
            Comission=new BonusComissionReport();
        }
        public BonusComissionReport Bonus { get; set; }

        public BonusComissionReport Comission { get; set; }
    }
}
