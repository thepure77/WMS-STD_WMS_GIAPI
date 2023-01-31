

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_WaveCheckProductLot
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid PlanGoodsIssue_Index { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public Guid Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Lot { get; set; }

    }
}
