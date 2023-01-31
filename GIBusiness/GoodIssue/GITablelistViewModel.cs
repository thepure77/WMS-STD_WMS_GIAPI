using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class GITablelistViewModel : Pagination
    {
 
        public Guid? GoodsIssueIndex { get; set; }

        public string PickTicketNo { get; set; }

        public Guid? TagOutPickIndex { get; set; }

        public string GoodsIssueNo { get; set; }

        public string GoodsIssueDate { get; set; }

        public string GoodsIssueDateTo { get; set; }


        public Guid? DocumentTypeIndex { get; set; }

        public string DocumentTypeId { get; set; }

        public string DocumentTypeName { get; set; }

        public int? DocumentStatus { get; set; }

        public Guid? OwnerIndex { get; set; }

        public string OwnerId { get; set; }

        public string OwnerName { get; set; }

        public Guid? ShipToIndex { get; set; }

        public string ShipToId { get; set; }

        public string ShipToName { get; set; }

        public Guid? RefDocumentIndex { get; set; }

        public string RefDocumentNo { get; set; }

        public Guid? RoundIndex { get; set; }

        public string RoundId { get; set; }

        public string RoundName { get; set; }

        public Guid? RouteIndex { get; set; }

        public string RouteId { get; set; }

        public string RouteName { get; set; }

        public string CreateDate { get; set; }

        public string CreateBy { get; set; }

        public string UpdateDate { get; set; }

        public string UpdateBy { get; set; }

        public Guid? ProductIndex { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public Guid? WarehouseIndex { get; set; }

        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public Guid? WarehouseIndexTo { get; set; }

        public string WarehouseIdTo { get; set; }

        public string WarehouseNameTo { get; set; }

        public Guid? ZoneIndex { get; set; }

        public string ZoneId { get; set; }

        public string ZoneName { get; set; }
        public Guid PlanGoodsIssueIndex { get; set; }
        public string PlanGoodsIssueNo { get; set; }

        public string ProcessStatusName { get; set; }

        public string ColumnName { get; set; }

        public string Orderby { get; set; }
        public int? PickingStatus { get; set; }
        public int? runWave_Status { get; set; }

        public int? runWave_StatusPickManual { get; set; }

        public class actionResultGIViewModel
        {
            public IList<GITablelistViewModel> items { get; set; }
            public Pagination pagination { get; set; }
        }
    }
}
