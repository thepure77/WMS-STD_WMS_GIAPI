using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_RPT_Picking_Tag_PlanGI
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? PlanGoodsIssue_Index { get; set; }
        public string ShipTo_Address { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public decimal? Ratio { get; set; }
        public string ProductConversion_Name { get; set; }
    }
}
