using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Base.Validations
{
    /// <summary>
    /// this exception will throw when a value object is invalid.
    /// </summary>
    public class ValueObjectIsInvalidException : Exception
    {
        public ValueObjectIsInvalidException(string message)
            : base(message)
        { }
    }
}
