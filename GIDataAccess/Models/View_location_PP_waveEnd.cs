using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_location_PP_waveEnd
    {
        public string PalletSuggestion { get; set; }

        [Key]
        public string Location_ID { get; set; }

        public string PalletID { get; set; }

        public string ProductID { get; set; }

        public string Product_Name { get; set; }

        public decimal? QtySaleUnit { get; set; }

        public string UOM { get; set; }

        public decimal? WMS_QTYBal { get; set; }

        public decimal? WMS_QtyReserve { get; set; }

        public DateTime? Update_Date { get; set; }

        public string Update_By { get; set; }

        public string ERP_Location { get; set; }
    }
}
