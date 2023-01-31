using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.OverallPerformanceDashboard
{
    public class PickingByZoneViewModel
    {
        public string Zone { get; set; }

        public string PickQty { get; set; }

        public string Fulfilled { get; set; }

        public string UnFulfilled { get; set; }

        public string Remain { get; set; }
    }
}
