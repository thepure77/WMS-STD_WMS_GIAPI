using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_TaskLabeling
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
        public int? PickingLabeling_Status { get; set; }
        public Guid? PlanGoodsIssue_Index { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public string Is_Picklabeling { get; set; }
        public string location_new { get; set; }
    }
}
