using GIBusiness.PlanGoodIssue;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class GoodDocIssueViewModel
    {
        
        public Guid GoodsIssueIndex { get; set; }

        public Guid OwnerIndex { get; set; }

        
        [StringLength(50)]
        public string OwnerId { get; set; }

        
        [StringLength(50)]
        public string OwnerName { get; set; }

        public Guid? DocumentTypeIndex { get; set; }

        
        [StringLength(50)]
        public string DocumentTypeId { get; set; }

        
        [StringLength(200)]
        public string DocumentTypeName { get; set; }

        
        [StringLength(50)]
        public string GoodsIssueNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string GoodsIssueDate { get; set; }

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

        [StringLength(200)]
        public string DocumentRemark { get; set; }

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

        public int? DocumentPriorityStatus { get; set; }

        public int? PickingStatus { get; set; }

        [StringLength(200)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string CreateDate { get; set; }

        [StringLength(200)]
        public string UpdateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string UpdateDate { get; set; }

        [StringLength(200)]
        public string CancelBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string CancelDate { get; set; }

        public Guid? WarehouseIndex { get; set; }

        [StringLength(50)]
        public string WarehouseId { get; set; }

        [StringLength(200)]
        public string WarehouseName { get; set; }

        public List<GoodIssueViewModelItem> listGoodIssueViewModelItem { get; set; }


    }
}
