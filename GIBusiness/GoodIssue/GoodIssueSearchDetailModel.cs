using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class GoodIssueSearchDetailModel
    {
        public Guid GoodsIssueIndex { get; set; }

        public Guid OwnerIndex { get; set; }

        public string OwnerId { get; set; }

        public string OwnerName { get; set; }

        public string ProductIndex { get; set; }

        public Guid? DocumentTypeIndex { get; set; }

        public string DocumentTypeId { get; set; }

        public string DocumentTypeName { get; set; }

        public string RefDocumentIndex { get; set; }

        public string GoodsIssueNo { get; set; }

        public string GoodsIssueDate { get; set; }

        public string GoodsIssueDateTo { get; set; }

        public string DocumentRefNo1 { get; set; }

        public string DocumentRefNo2 { get; set; }
   
        public string DocumentRefNo3 { get; set; }
  
        public string DocumentRemark { get; set; }

        public int? DocumentStatus { get; set; }

        public string UDF1 { get; set; }
     
        public string UDF2 { get; set; }

        public string UDF3 { get; set; }

        public int? PickingStatus { get; set; }

        public string CancelBy { get; set; }

        public string CancelDate { get; set; }

        public Guid RouteIndex { get; set; }

        public Guid SubRouteIndex { get; set; }

        public string RouteId { get; set; }

        public string RouteName { get; set; }

        public Guid RoundIndex { get; set; }

        public string RoundId { get; set; }

        public string RoundName { get; set; }

        public Guid? WarehouseIndex { get; set; }

        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }
        public Guid? WarehouseIndexTo { get; set; }


    }
}
