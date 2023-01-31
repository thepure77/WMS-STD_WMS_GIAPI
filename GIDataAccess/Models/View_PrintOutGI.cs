using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_PrintOutGI
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? GoodsIssue_Index { get; set; }
        public string GoodsIssue_No { get; set; }
        public DateTime? GoodsIssue_Date { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? Qty { get; set; }
        public string ProductConversion_Name { get; set; }
        public string Tag_No { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public Guid? Ref_DocumentItem_Index { get; set; }
    }
}
