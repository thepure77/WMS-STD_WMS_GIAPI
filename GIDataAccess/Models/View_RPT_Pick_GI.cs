using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_RPT_Pick_GI
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? GoodsIssue_Index { get; set; }
        public Guid? GoodsIssueItemLocation_Index { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public string GoodsIssue_No { get; set; }
        public DateTime? GoodsIssue_Date { get; set; }
        public string Document_Remark { get; set; }
        public string Owner_Name { get; set; }

    }
}
