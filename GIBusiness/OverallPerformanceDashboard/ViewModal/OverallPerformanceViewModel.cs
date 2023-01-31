using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.OverallPerformanceDashboard
{
    public class OverallPerformanceViewModel
    {
        public string OverallDate { get; set; }

        public List<OverallStatusViewModel> OverallStatusViewModel { get; set; }
        
        public List<OrderByRoundViewModel> OrderByRoundViewModel { get; set; }

        public List<PickingByRoundViewModel> PickingByRoundViewModel { get; set; }

        public List<PickingByZoneViewModel> PickingByZoneViewModel { get; set; }

        public List<OrderByRouteViewModel> OrderByRouteViewModel { get; set; } 
    }

    public class OverallStatusViewModel
    {
        public string StatusName { get; set; }

        public int Qty { get; set; }
    }
}
