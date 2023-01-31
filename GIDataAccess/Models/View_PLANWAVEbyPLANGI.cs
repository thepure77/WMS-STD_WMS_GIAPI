using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{
    public partial class View_PLANWAVEbyPLANGI
    {

        [Column(Order = 0)]
        public Guid PlanGoodsIssue_Index { get; set; }

        [StringLength(50)]
        public string Owner_Id { get; set; }

        [StringLength(50)]
        public string Owner_Name { get; set; }


        [Column(Order = 1)]
        public Guid Owner_Index { get; set; }


        [Column(Order = 2)]
        public Guid SoldTo_Index { get; set; }

        [StringLength(50)]
        public string SoldTo_Id { get; set; }

        [StringLength(200)]
        public string SoldTo_Name { get; set; }

        [StringLength(200)]
        public string SoldTo_Address { get; set; }


        [Column(Order = 3)]
        public Guid ShipTo_Index { get; set; }

        [StringLength(50)]
        public string ShipTo_Id { get; set; }

        [StringLength(200)]
        public string ShipTo_Name { get; set; }

        [StringLength(200)]
        public string ShipTo_Address { get; set; }

        public Guid? DocumentType_Index { get; set; }

        [StringLength(50)]
        public string DocumentType_Id { get; set; }

        [StringLength(200)]
        public string DocumentType_Name { get; set; }


        [Column(Order = 4)]
        [StringLength(50)]
        public string PlanGoodsIssue_No { get; set; }


        [Column(Order = 5, TypeName = "smalldatetime")]
        public DateTime PlanGoodsIssue_Date { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PlanGoodsIssue_Due_Date { get; set; }

        [Key]
        [Column(Order = 6)]
        public Guid PlanGoodsIssueItem_Index { get; set; }

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

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [StringLength(200)]
        public string ItemStatus_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Ratio { get; set; }

        public Guid? ProductConversion_Index { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalQty { get; set; }

        [StringLength(50)]
        public string ProductConversion_Id { get; set; }

        [StringLength(200)]
        public string ProductConversion_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MFG_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EXP_Date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitWeight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitWidth { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitLength { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitHeight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Volume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitVolume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitPrice { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Price { get; set; }

        public int? DocumentPriority_Status { get; set; }

        public int? Document_Status { get; set; }

        public int? ItemDocument_Status { get; set; }

        [StringLength(200)]
        public string DocumentRef_No1 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No2 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No3 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No4 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No5 { get; set; }

        //[StringLength(200)]
        //public string Document_Remark { get; set; }

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

        [Column(TypeName = "numeric")]
        public decimal? GITotalQty { get; set; }
    }
}
