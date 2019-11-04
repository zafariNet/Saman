using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels;

namespace Services.Messaging
{
    public class GetMainMenusResponse
    {
        public IEnumerable<MainMenuView> data { get; set; }

        public int TotalCount { get; set; }

        public bool success { get; set; }
    }
}
