using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.TrackingLoading
{
    public class ReturnToWBViewModel
    {
        
        public Guid? truckLoad_Index { get; set; }
        public string planGI { get; set; }
        public Guid? planGoodsIssue_Index { get; set; }
        public string shipment_no { get; set; }
        public string location { get; set; }
        public string location_index { get; set; }
        public string location_id { get; set; }
        public string location_name { get; set; }
        public string userName { get; set; }
        
    }
}
