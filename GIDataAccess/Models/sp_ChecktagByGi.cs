using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class sp_ChecktagByGi 
    {
        [Key]
        public string Tag_No { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public decimal TotalQty { get; set; }

        public string ProductConversion_Name { get; set; }
        
        public string GoodsIssue_No { get; set; }

        public Guid GoodsIssue_Index { get; set; }

        public Guid GoodsIssueItemLocation_Index { get; set; }
    }
}
