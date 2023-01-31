using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_TaskInsertBinCard_PickTiLight
    {
        [Key]
        public Guid Taskitem_Index { get; set; }

        public Guid Task_Index { get; set; }

        [StringLength(50)]
        public string Task_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        [StringLength(200)]
        public string Ref_Document_No { get; set; }
        public Guid? TagOutItem_Index { get; set; }

        public Guid? TagOut_Index { get; set; }

        [StringLength(200)]
        public string TagOut_No { get; set; }

        public DateTime GoodsIssue_Date { get; set; }

        public Guid GoodsIssueItemLocation_Index { get; set; }
        public Guid DocumentType_Index { get; set; }

        [StringLength(50)]
        public string DocumentType_Id { get; set; }

        [StringLength(200)]
        public string DocumentType_Name { get; set; }

        public Guid? TagItem_Index { get; set; }

        public Guid? Tag_Index { get; set; }

        [StringLength(50)]
        public string Tag_No { get; set; }

        public Guid? Product_Index { get; set; }

        [StringLength(50)]
        public string Product_Id { get; set; }

        [StringLength(200)]
        public string Product_Name { get; set; }

        [StringLength(200)]
        public string Product_SecondName { get; set; }

        [StringLength(200)]
        public string Product_ThirdName { get; set; }

        [StringLength(50)]
        public string Product_Lot { get; set; }

        public Guid? ItemStatus_Index { get; set; }

        [StringLength(50)]
        public string ItemStatus_Id { get; set; }

        [StringLength(200)]
        public string ItemStatus_Name { get; set; }

        public Guid? ProductConversion_Index { get; set; }

        [StringLength(50)]
        public string ProductConversion_Id { get; set; }

        [StringLength(200)]
        public string ProductConversion_Name { get; set; }

        public Guid Owner_Index { get; set; }

        [StringLength(50)]
        public string Owner_Id { get; set; }

        [StringLength(50)]
        public string Owner_Name { get; set; }

        public Guid? Location_Index { get; set; }

        [StringLength(50)]
        public string Location_Id { get; set; }

        [StringLength(200)]
        public string Location_Name { get; set; }

        public DateTime? EXP_Date { get; set; }

        public DateTime? MFG_Date { get; set; }

        [StringLength(200)]
        public string UDF_1 { get; set; }

        [StringLength(200)]
        public string UDF_2 { get; set; }

        [StringLength(200)]
        public string UDF_3 { get; set; }

        [StringLength(200)]
        public string UDF_4 { get; set; }

        [StringLength(200)]
        public string UDF_5 { get; set; }

        public decimal? Picking_Qty { get; set; }

        public decimal? Picking_Ratio { get; set; }

        public decimal? Picking_TotalQty { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Ratio { get; set; }

        public decimal? TotalQty { get; set; }

        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }

        public Guid? BinBalance_Index { get; set; }

        public int? GIILStatus { get; set; }

        public int? GLStatus { get; set; }

        public int? TaskItemStatus { get; set; }

        public int? PickStatus { get; set; }

        public int? flag_picktolight { get; set; }

    }
}
