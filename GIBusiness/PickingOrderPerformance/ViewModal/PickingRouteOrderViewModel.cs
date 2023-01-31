using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.PickingOrderPerformance
{
    public class PickingRouteOrderViewModel
    {
        public string Order { get; set; }

        public string OverallStatus { get; set; }

        public List<PickingRouteOrderDetailViewModel> PickingRouteOrderDetailViewModel { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public string roundName { get; set; }

        public string udf_2 { get; set; }
        public string documentType_Id { get; set; }
    }
}
