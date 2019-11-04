using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Employees.Validations
{
    /// <summary>
    /// در صورتی که کارمند نامعتبر باشد این استثنا رخ می دهد
    /// </summary>
    public class InvalidEmployeeException : Exception
    {
        public InvalidEmployeeException(string message)
            : base(message)
        {
        }
    }
}
