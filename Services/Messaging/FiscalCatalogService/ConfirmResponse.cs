using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging
{
    public class ConfirmResponse
    {
        public bool success { get; set; }

        // ID of newly added entity
        public Guid ID { get; set; }

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