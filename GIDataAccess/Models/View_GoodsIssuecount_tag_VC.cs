using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_GoodsIssuecount_tag_VC
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

        public string Location_Name { get; set; }
        public decimal? ProductConversion_Weight { get; set; }
     
        public decimal? ProductConversion_Length { get; set; }
        public decimal? ProductConversion_Height { get; set; }
        public decimal? ProductConversion_Volume { get; set; }
        public decimal? Vol_Per_Unit { get; set; }

        public Guid Product_Index { get; set; }
        public string Product_Name { get; set; }
        public string Product_Lot { get; set; }

        public Guid ProductConversion_Index { get; set; }
        public string ProductConversion_Id { get; set; }

        public string ProductConversion_Name { get; set; }

        public Guid ItemStatus_Index { get; set; }
        public string ItemStatus_Id { get; set; }

        public string ItemStatus_Name { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EXP_Date { get; set; }

        public string MaxToteM { get; set; }
        public string MaxToteL { get; set; }

        public string Chute_No { get; set; }

        
    }
}
