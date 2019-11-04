using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging
{
    public class MoveResponse
    {
        public bool success { get; set; }

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
