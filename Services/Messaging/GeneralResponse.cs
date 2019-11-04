#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Services.Messaging
{
    /// <summary>
    /// For Json Insert, Update and Delete response
    /// </summary>
    public class GeneralResponse
    {
        public bool success {
            get
            {
                return true;
            }
        }

        public bool hasError
        {
            get
            {
                return ErrorMessages.Count() > 0;
            }
        }

        public int rowVersion { get; set; }

        // ID of newly added entity
        public Guid ID { get; set; }

        public object ObjectAdded { get; set; }

        private IList<string> _ErrorMessages;
        public IList<string> ErrorMessages
        {
            get
            {
                if (_ErrorMessages == null)
                    _ErrorMessages = new List<string>();

                return _ErrorMessages;
            }
        }
    }
}
