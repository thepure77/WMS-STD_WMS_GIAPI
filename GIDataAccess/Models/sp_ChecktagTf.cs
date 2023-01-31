using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class sp_ChecktagTf
    {
        [Key]
        public string Tag_No { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public decimal Qty { get; set; }

        public decimal TotalQty { get; set; }

        public string ProductConversion_Name { get; set; }

        public string Location_Name { get; set; }

        public string Location_Name_To { get; set; }

        public string ItemStatus_Name { get; set; }

        public string ItemStatus_Name_To { get; set; }

        public string ERP_Location { get; set; }

        public string ERP_Location_To { get; set; }

        public int Document_Status { get; set; }

        public int Item_Document_Status { get; set; }

        public string GoodsTransfer_No { get; set; }

        public string DocumentType_Name { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Create_By { get; set; }
    }
}
