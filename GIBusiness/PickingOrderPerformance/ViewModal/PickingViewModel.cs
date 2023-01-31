using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.PickingOrderPerformance
{
    public class PickingViewModel
    {
        public string PickDate { get; set; }

        public string PickTime { get; set; }

        public int OrderQty { get; set; }

        public List<PickingRouteViewModel> PickingRouteViewModel { get; set; }
    }
}
