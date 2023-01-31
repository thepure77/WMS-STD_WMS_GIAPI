using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.PickingOrderPerformance
{
    public class PickingRouteViewModel
    {
        public string Route { get; set; }

        public int RouteOrderQty { get; set; }

        public List<PickingRouteOrderViewModel> PickingRouteOrderViewModel { get; set; }

        public List<PickingRouteOrderViewModel> PickingRouteOrderViewModel2 { get; set; }

        public string UDF_3 { get; set; }

        public string Seq { get; set; }
        public string RouteTime { get; set; }
    }
}
