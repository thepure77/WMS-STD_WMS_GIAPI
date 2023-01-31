using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{

    public partial class CheckFOC
    {
        [Key]
        public long? RowIndex { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public string Round_Id { get; set; }

        public DateTime? PlanGoodsIssue_Due_Date { get; set; }
    }
}
