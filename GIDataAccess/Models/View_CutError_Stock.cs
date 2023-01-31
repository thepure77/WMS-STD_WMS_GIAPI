using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{

    public partial class View_CutError_Stock
    {
        [Key]

        public Guid BinBalance_Index { get; set; }

        public string Tag_No { get; set; }

        public string Location_Name { get; set; }

        public string Product_Id { get; set; }

        public decimal? BinBalance_QtyBegin { get; set; }

        public decimal? BinBalance_QtyBal { get; set; }

        public decimal? BinBalance_QtyReserve { get; set; }

        public string ERP_Location { get; set; }

        public DateTime? Create_Date { get; set; }
    }
}
