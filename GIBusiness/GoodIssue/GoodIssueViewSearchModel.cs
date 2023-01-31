using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class GoodIssueViewSearchModel
    {

        public Guid? GoodsIssueIndex { get; set; }
        public string GoodsIssueNo { get; set; }
        public Guid? RefDocumentIndex { get; set; }
        public string RefDocumentName { get; set; }
        public Guid? OwnerIndex { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public Guid? ShipToIndex { get; set; }
        public string GoodsIssueDate { get; set; }
        public string GoodsIssueDateTo { get; set; }
        public string PickTicketNo { get; set; }
        public Guid? ProductIndex { get; set; }
        public Guid? DocumentTypeIndex { get; set; }
        public string DocumentTypeName { get; set; }
        public int? DocumentStatus { get; set; }
        public Guid? RoundIndex { get; set; }
        public Guid? RouteIndex { get; set; }
        public Guid? WarehouseIndexTo { get; set; }

        public Guid? WarehouseIndex { get; set; }
        public Guid? ZoneIndex { get; set; }
        public string WaveBy { get; set; }



    }
}
