using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_Taskitem_with_Truckload_PICKQTY_GETPLAN
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid TaskItem_Index { get; set; }

        public Guid Task_Index { get; set; }

        [StringLength(200)]
        public string PlanGoodsIssue_No { get; set; }
        
        public decimal? Picking_Qty { get; set; }
        
        public Guid? TruckLoad_Index { get; set; }
    }
}
