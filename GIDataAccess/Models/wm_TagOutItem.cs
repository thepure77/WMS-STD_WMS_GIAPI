using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class wm_TagOutItem
    {
        [Key]
        public Guid TagOutItem_Index { get; set; }

        public Guid? TagOut_Index { get; set; }

        [StringLength(50)]
        public string TagOut_No { get; set; }

        public Guid? GoodsIssue_Index { get; set; }

        public Guid? GoodsIssueItem_Index { get; set; }

        public Guid? GoodsIssueItemLocation_Index { get; set; }

        [StringLength(200)]
        public string Carton_No { get; set; }

        public Guid Product_Index { get; set; }

        [Required]
        [StringLength(50)]
        public string Product_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Product_Name { get; set; }

        [StringLength(200)]
        public string Product_SecondName { get; set; }

        [StringLength(200)]
        public string Product_ThirdName { get; set; }

        [StringLength(50)]
        public string Product_Lot { get; set; }

        public Guid ItemStatus_Index { get; set; }

        [StringLength(50)]
        public string ItemStatus_Id { get; set; }

        [StringLength(200)]
        public string ItemStatus_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TotalQty { get; set; }

        public Guid ProductConversion_Index { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductConversion_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductConversion_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Volume { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MFG_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EXP_Date { get; set; }

        [StringLength(200)]
        public string TagOutRef_No1 { get; set; }

        [StringLength(200)]
        public string TagOutRef_No2 { get; set; }

        [StringLength(200)]
        public string TagOutRef_No3 { get; set; }

        [StringLength(200)]
        public string TagOutRef_No4 { get; set; }

        [StringLength(200)]
        public string TagOutRef_No5 { get; set; }

        public int? TagOut_Status { get; set; }

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

        public Guid? Ref_Process_Index { get; set; }

        [StringLength(200)]
        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Cancel_Date { get; set; }

        public Guid? Location_Confirm_Index { get; set; }

        [StringLength(50)]
        public string Location_Confirm_Id { get; set; }

        [StringLength(200)]
        public string Location_Confirm_Name { get; set; }

    }
}
