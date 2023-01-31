using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_RPT_Pick_TaskItem
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? Task_Index { get; set; }
        public Guid? Ref_DocumentItem_Index { get; set; }
        public string Task_No { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? Qty { get; set; }
        public Guid? ProductConversion_Index { get; set; }
        public string ProductConversion_Id{ get; set; }
        public string ProductConversion_Name { get; set; }
        public string Location_Name { get; set; }
        public string Assign_By { get; set; }
    }
}
