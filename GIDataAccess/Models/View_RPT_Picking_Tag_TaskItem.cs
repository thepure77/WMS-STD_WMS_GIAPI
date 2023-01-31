using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_RPT_Picking_Tag_TaskItem
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? Ref_DocumentItem_Index { get; set; }
        public Guid? Task_Index { get; set; }
        public string Task_No { get; set; }
    }
}
