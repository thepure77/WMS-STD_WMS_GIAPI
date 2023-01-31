using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_RPT_Picking_Tag_GI
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? GoodsIssue_Index { get; set; }
        public Guid? GoodsIssueItemLocation_Index { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public Guid? Location_Index { get; set; }
        public string Location_Id { get; set; }
        public string Location_Name { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public decimal? TotalQty { get; set; }
        public string GoodsIssue_No { get; set; }
        public DateTime? Document_Date { get; set; }
        public string ProductConversion_Name { get; set; }
    }
}
