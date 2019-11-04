    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Customers
{
    public enum NetworkCenterStatus
    {
        NotDefined = -1, // مشخص نشده
        Support = 1, // تحت پوش
        NotSupport = 2, // عدم پوشش
        AdameEmkan = 3 // عدم امکان موقت
    }
}
