using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Fiscals
{
    public class MoneyAccountView : BaseView
    {
        [Display(Name = "حساب")]
        public string AccountName { get; set; }

        [Display(Name = "دریافت")]
        public bool Receipt { get; set; }

        [Display(Name = "پرداخت")]
        public bool Pay { get; set; }

        [Display(Name = "بانکی/نقدی")]
        public bool IsBankAccount { get; set; }

        [Display(Name = "بانکی/نقدی")]
        public string IsBankAccountToString
        {
            get { return IsBankAccount ? "بانکی" : "نقدی"; }
        }
        [Display(Name = "فعال")]
        public bool Discontinued { get; set; }
        public int SortOrder { get; set; }

        [Display(Name = "شماره حساب")]
        public string BAccountNumber { get; set; }

        [Display(Name = "مشخصات")]
        public string BAccountInfo { get; set; }

        /// <summary>
        /// دارای شماره رسید مالی منحصر به فرد
        /// </summary>
        public bool HasUniqueSerialNumber { get; set; }
        public bool Has9Digits { get; set; }

        //IEnumerables
        
        [Display(Name = "کارمندان")]
        public IEnumerable<Employees.EmployeeView> AccountEmployees { get; protected set; }
        
        [Display(Name = "امور مالی")]
        public IEnumerable<Fiscals.FiscalView> Fiscals { get; protected set; }
        
        [Display(Name = "حسابهای کارمندان")]
        public IEnumerable<MoneyAccountEmployeeView> MoneyAccountEmployees { get; protected set; }
        
        [Display(Name = "شبکه ها")]
        public IEnumerable<Store.NetworkView> Networks { get; protected set; }
    }
}
