using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.FiscalCatalogService
{
    public class AddMoneyAccountRequestOld
    {
        public string AccountName { get; set; }
        public bool Receipt { get; set; }
        public bool Pay { get; set; }
        public bool IsBankAccount { get; set; }
        public string BAccountNumber { get; set; }
        public string BAccountInfo { get; set; }
        //public IEnumerable<Employee> AccountEmployees { get; set; }
        public bool Discontinued { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
    public class AddMoneyAccountRequest
    {
        public string AccountName { get; set; }
        public bool Receipt { get; set; }
        public bool Pay { get; set; }
        public bool IsBankAccount { get; set; }
        public string BAccountNumber { get; set; }
        public string BAccountInfo { get; set; }
        public bool Discontinued { get; set; }
        public bool HasUniqueSerialNumber { get; set; }
        public bool Has9Digits { get; set; }
    }
}
