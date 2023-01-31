using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.PlanGoodIssue
{
   public class GoodIssueCancelViewModelItem
    {
        [Key]
        public Guid GoodsIssueItemIndex { get; set; }

        public Guid GoodsIssueIndex { get; set; }

        public Guid ProductIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        [StringLength(200)]
        public string ProductSecondName { get; set; }

        [StringLength(200)]
        public string ProductThirdName { get; set; }

        [StringLength(50)]
        public string ProductLot { get; set; }

        public Guid? ItemStatusIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemStatusId { get; set; }

        [Required]
        [StringLength(200)]
        public string ItemStatusName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? QtyPlan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TotalQty { get; set; }

        public Guid ProductConversionIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductConversionId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductConversionName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MFGDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EXPDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitWeight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UnitWidth { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UnitLength { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UnitHeight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UnitVolume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Volume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string DocumentRefNo1 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo2 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo3 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo4 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo5 { get; set; }

        public int? DocumentStatus { get; set; }

        [StringLength(200)]
        public string UDF1 { get; set; }

        [StringLength(200)]
        public string UDF2 { get; set; }

        [StringLength(200)]
        public string UDF3 { get; set; }

        [StringLength(200)]
        public string UDF4 { get; set; }

        [StringLength(200)]
        public string UDF5 { get; set; }

        public Guid? RefProcessIndex { get; set; }

        public string RefDocumentNo { get; set; }

        public string RefDocumentLineNum { get; set; }

        public Guid? RefDocumentIndex { get; set; }

        public Guid? RefDocumentItemIndex { get; set; }

        public Guid GoodsReceiveItemIndex { get; set; }

        [StringLength(200)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        [StringLength(200)]
        public string UpdateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        [StringLength(200)]
        public string CancelBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CancelDate { get; set; }

        public int? CancelStatus { get; set; }


    }
}
