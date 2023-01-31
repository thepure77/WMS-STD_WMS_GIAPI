using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class sp_Fixwave
    {
        public string Detail { get; set; }

        [Key]
        public Guid Binbalance_index { get; set; }

        public string Tag_No { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public decimal BinBalance_QtyBal { get; set; }

        public decimal BinBalance_QtyReserve { get; set; }

        public decimal GI_TotalQty { get; set; }

        public decimal diff { get; set; }

        public string GoodsIssue_No { get; set; }
        
    }
}
