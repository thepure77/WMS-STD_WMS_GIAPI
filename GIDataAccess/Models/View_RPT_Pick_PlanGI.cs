using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_RPT_Pick_PlanGI
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? PlanGoodsIssue_Index { get; set; }
        public string LineNum { get; set; }
    }
}
