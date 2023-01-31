using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class sp_ChecktagGi
    {
        [Key]
        public string Tag_No { get; set; }

        public string GoodsIssue_No { get; set; }

        public string Location_Name { get; set; }

        public decimal GITotalQty { get; set; }
    }
}
