#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;

#endregion

namespace Services.Messaging
{
    /// <summary>
    /// For Json Read(s)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GetGeneralResponse<T> : GeneralResponse
    {

        public T data { get; set; }

        public int totalCount { get; set; }
    }
}
