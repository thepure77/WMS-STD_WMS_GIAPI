using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{


    public class GenTaskViewModel
    {

        [Key]
        [Column(Order = 0)]
        public Guid GoodsIssueIndex { get; set; }


        [Column(Order = 1)]
        public Guid GoodsIssueItemIndex { get; set; }


        [Column(Order = 2)]
        public Guid GoodsIssueItemLocationIndex { get; set; }


        [Column(Order = 3)]
        public Guid OwnerIndex { get; set; }


        [Column(Order = 4)]
        [StringLength(50)]
        public string OwnerId { get; set; }


        [Column(Order = 5)]
        [StringLength(50)]
        public string OwnerName { get; set; }

        public Guid? DocumentTypeIndex { get; set; }


        [Column(Order = 6)]
        [StringLength(50)]
        public string DocumentTypeId { get; set; }


        [Column(Order = 7)]
        [StringLength(200)]
        public string DocumentTypeName { get; set; }


        [Column(Order = 8)]
        [StringLength(50)]
        public string GoodsIssueNo { get; set; }


        [Column(Order = 9, TypeName = "smalldatetime")]
        public string GoodsIssueDate { get; set; }

        public Guid? TagItemIndex { get; set; }

        public Guid? TagIndex { get; set; }

        [StringLength(50)]
        public string TagNo { get; set; }


        [Column(Order = 10)]
        public Guid ProductIndex { get; set; }


        [Column(Order = 11)]
        [StringLength(50)]
        public string ProductId { get; set; }


        [Column(Order = 12)]
        [StringLength(200)]
        public string ProductName { get; set; }

        [StringLength(200)]
        public string ProductSecondName { get; set; }

        [StringLength(200)]
        public string ProductThirdName { get; set; }

        [StringLength(50)]
        public string ProductLot { get; set; }

        public Guid? ItemStatusIndex { get; set; }


        [Column(Order = 13)]
        [StringLength(50)]
        public string ItemStatusId { get; set; }


        [Column(Order = 14)]
        [StringLength(200)]
        public string ItemStatusName { get; set; }

        public Guid? LocationIndex { get; set; }

        [StringLength(50)]
        public string LocationId { get; set; }

        [StringLength(200)]
        public string LocationName { get; set; }


        [Column(Order = 15, TypeName = "numeric")]
        public decimal Qty { get; set; }


        [Column(Order = 16, TypeName = "numeric")]
        public decimal Ratio { get; set; }


        [Column(Order = 17, TypeName = "numeric")]
        public decimal TotalQty { get; set; }


        [Column(Order = 18)]
        public Guid ProductConversionIndex { get; set; }


        [Column(Order = 19)]
        [StringLength(50)]
        public string ProductConversionId { get; set; }


        [Column(Order = 20)]
        [StringLength(200)]
        public string ProductConversionName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MFGDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EXPDate { get; set; }

        [StringLength(200)]
        public string RefDocumentNo { get; set; }

        [StringLength(200)]
        public string RefDocumentLineNum { get; set; }

        public Guid? RefDocumentIndex { get; set; }

        public Guid? RefDocumentItemIndex { get; set; }


        [Column(Order = 21)]
        public Guid ProductSubTypeIndex { get; set; }


        [Column(Order = 22)]
        [StringLength(50)]
        public string ProductSubTypeId { get; set; }


        [Column(Order = 23)]
        [StringLength(200)]
        public string ProductSubTypeName { get; set; }


        [Column(Order = 24)]
        public Guid ProductTypeIndex { get; set; }


        [Column(Order = 25)]
        [StringLength(50)]
        public string ProductTypeId { get; set; }


        [Column(Order = 26)]
        [StringLength(200)]
        public string ProductTypeName { get; set; }


        [Column(Order = 27)]
        public Guid? ZoneIndex { get; set; }

        [StringLength(50)]
        public string ZoneId { get; set; }

        [StringLength(200)]
        public string ZoneName { get; set; }

        public Guid? WorkAreaIndex { get; set; }

        [StringLength(50)]
        public string WorkAreaId { get; set; }

        [StringLength(200)]
        public string WorkAreaName { get; set; }

        public int? IsPack { get; set; }
        public Guid? WarehouseIndex { get; set; }

        [StringLength(50)]
        public string WarehouseId { get; set; }

        [StringLength(200)]
        public string WarehouseName { get; set; }

        public List<GenTaskListViewModel> GenTask { get; set;}

}

    

    public class GenTaskListViewModel
    {
        public List<GenTaskViewModel> GenTaskDetailViewModel { get; set; }
    }
}
