using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_Task_moveSTG
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid Task_Index { get; set; }
        public string Task_No { get; set; }

        public string Ref_Document_No { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public string UserAssign { get; set; }
        public string Create_By { get; set; }
        public DateTime Create_Date { get; set; }
        public string Update_By { get; set; }
        public int? Document_Status { get; set; }
        //public Guid? PlanGoodsIssue_Index { get; set; }
        //public string PlanGoodsIssue_No { get; set; }

        public int? Document_StatusScanPick { get; set; }
        public int? Document_StatusLabeling { get; set; }
        public int? Document_StatusPickQty { get; set; }
        public int? Document_StatusDocktoStg { get; set; }
        public int? Document_StatusMovetoStgOG { get; set; }
        public int? Document_StatusTracking { get; set; }
        public string DocumentRef_No1 { get; set; }
        public string Tag_No { get; set; }
        
    }
}
