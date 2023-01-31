using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class sp_UpdateBinBalance
    {
        [Key]
        public string BinBalance_Index { get; set; }

        public decimal BinBalance_QtyBal { get; set; }

        public decimal BinBalance_QtyReserve { get; set; }
    }
}
