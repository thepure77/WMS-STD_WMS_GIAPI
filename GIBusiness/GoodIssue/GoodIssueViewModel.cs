using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class GoodIssueViewModel
    {
        [Key]
        public Guid goodsIssue_Index { get; set; }

        public Guid owner_Index { get; set; }


        [StringLength(50)]
        public string owner_Id { get; set; }


        [StringLength(50)]
        public string owner_Name { get; set; }

        public Guid? documentType_Index { get; set; }


        [StringLength(50)]
        public string documentType_Id { get; set; }


        [StringLength(200)]
        public string documentType_Name { get; set; }


        [StringLength(50)]
        public string goodsIssue_No { get; set; }

        public string goodsIssue_Date { get; set; }
        [StringLength(50)]
        public string goodsIssue_Time { get; set; }
        public string document_Date { get; set; }

        [StringLength(200)]
        public string documentRef_No1 { get; set; }

        [StringLength(200)]
        public string documentRef_No2 { get; set; }

        [StringLength(200)]
        public string documentRef_No3 { get; set; }

        [StringLength(200)]
        public string documentRef_No4 { get; set; }

        [StringLength(200)]
        public string documentRef_No5 { get; set; }

        [StringLength(200)]
        public string document_Remark { get; set; }

        public int? document_Status { get; set; }

        [StringLength(200)]
        public string udf_1 { get; set; }

        [StringLength(200)]
        public string udf_2 { get; set; }

        [StringLength(200)]
        public string udf_3 { get; set; }

        [StringLength(200)]
        public string udf_4 { get; set; }

        [StringLength(200)]
        public string udf_5 { get; set; }

        public int? documentPriority_Status { get; set; }

        public int? picking_Status { get; set; }

        [StringLength(200)]
        public string create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? create_Date { get; set; }

        [StringLength(200)]
        public string update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? update_Date { get; set; }

        [StringLength(200)]
        public string cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? cancel_Date { get; set; }

        public Guid? warehouse_Index { get; set; }

        [StringLength(50)]
        public string warehouse_Id { get; set; }

        [StringLength(200)]
        public string warehouse_Name { get; set; }
        public int? cancel_Status { get; set; }
        public int? runWave_Status { get; set; }
        public Guid? wave_Index { get; set; }
        public string processStatus_Name { get; set; }

        
    }
}
