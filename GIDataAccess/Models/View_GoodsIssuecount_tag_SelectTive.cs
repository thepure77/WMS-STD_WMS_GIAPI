using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_GoodsIssuecount_tag_SelectTive
    {
        [Key]
        public long? RowIndex { get; set; }
        public string GoodsIssue_No { get; set; }
        public string Product_Id { get; set; }
        public string Branch_Name { get; set; }
        public string Branch_Code { get; set; }
        public string Chute_ID { get; set; }
        public decimal TotalQty { get; set; }
        public decimal SALE_Ratio { get; set; }
        public decimal? QTYCTN { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public Guid? Ref_DocumentItem_Index { get; set; }
        public string Ref_Document_No { get; set; }
        public Guid GoodsIssue_Index { get; set; }
        public Guid GoodsIssueItemLocation_Index { get; set; }
        public string Running { get; set; }
        public string ShipTo_Id { get; set; }
        public Guid? ShipTo_Index { get; set; }
        public string Ref_Process_Index { get; set; }
        public string TagOutType { get; set; }
        public string LocationType { get; set; }

    }
}
