using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class __dashboard
    {
        [Key]
        public int RowIndex { get; set; }

        public Guid? Zone_Index { get; set; }

        [StringLength(1000)]
        public string Zone_Name { get; set; }

        [StringLength(1000)]
        public string Doc_ZONE { get; set; }

        public Guid? Route_Index { get; set; }

        [StringLength(1000)]
        public string Route_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DocDate { get; set; }

        public Guid? Round_Index { get; set; }

        [StringLength(1000)]
        public string Round_Name { get; set; }

        [StringLength(1000)]
        public string PlanGoodsIssue_No { get; set; }

        public Guid? PlanGoodsIssue_Index { get; set; }

        [StringLength(1000)]
        public string Pick_Status { get; set; }

        [StringLength(1000)]
        public string Pick_StatusCode { get; set; }

        [StringLength(1000)]
        public string Overall_Status { get; set; }

        [StringLength(1000)]
        public string Overall_StatusCode { get; set; }

        [StringLength(1000)]
        public string UDF_3 { get; set; }

        [StringLength(1000)]
        public string UDF_4 { get; set; }

        [StringLength(1000)]
        public string DocumentType_Id { get; set; }

        public int? Seq { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
