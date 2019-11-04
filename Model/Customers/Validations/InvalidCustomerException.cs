using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Customers.Validations
{
    /// <summary>
    /// در صورتی که موجودیت مشتری نامعتبر بود این استثنا رخ می دهد
    /// </summary>
    public class InvalidCustomerException : Exception
    {
        public InvalidCustomerException(string message)
            : base(message)
        {
        }
    }
}
