using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_GoodsIssue
    {
        [Key]
        public long? RowIndex { get; set; }


        public Guid GoodsIssue_Index { get; set; }

        public string GoodsIssue_No { get; set; }


        public DateTime GoodsIssue_Date { get; set; }
        public DateTime? PlanGoodsIssue_Due_Date { get; set; }
        public DateTime? PlanGoodsIssue_Date { get; set; }

        
        public Guid? Owner_Index { get; set; }
        public string Owner_Id { get; set; }
        public string Owner_Name { get; set; }
        public int? Document_Status { get; set; }


        public string PlanGoodsIssue_No { get; set; }


        public Guid DocumentType_Index { get; set; }


        public string DocumentType_Id { get; set; }


        public string DocumentType_Name { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Qty { get; set; }

        public string Create_By { get; set; }
        public string Document_Remark { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Update_By { get; set; }
        public string Cancel_By { get; set; }
        public string DocumentRef_No1 { get; set; }
        public string DocumentRef_No2 { get; set; }
        public int? GI_status { get; set; }
        public int? WCS_status { get; set; }

    }
}
