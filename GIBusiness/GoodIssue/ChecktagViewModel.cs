using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class ChecktagViewModel
    {
        public string Tag_No { get; set; }
        public string GoodsIssue_No { get; set; }
        public string Location_Name { get; set; }
        public string GITotalQty { get; set; }

        public string Document_Status { get; set; }
        public string Item_Document_Status { get; set; }
        public string GoodsTransfer_No { get; set; }
        public string DocumentType_Name { get; set; }
        public string Create_Date { get; set; }
        public string Create_By { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal Qty { get; set; }
        public decimal TotalQty { get; set; }
        public string ProductConversion_Name { get; set; }
        public string Location_Name_To { get; set; }
        public string ItemStatus_Name { get; set; }
        public string ItemStatus_Name_To { get; set; }
        public string ERP_Location { get; set; }
        public string ERP_Location_To { get; set; }
        
        public string BinBalance_Index { get; set; }
        public decimal BinBalance_QtyBal { get; set; }
        public decimal BinBalance_QtyReserve { get; set; }

        public string goodsIssue_No { get; set; }

        public string Detail { get; set; }
        public Guid Binbalance_index { get; set; }
        public decimal GI_TotalQty { get; set; }
        public decimal diff { get; set; }
        public int? fixWave_Count { get; set; }

        public string GoodsIssue_Index { get; set; }
        public string GoodsIssueItemLocation_Index { get; set; }
    }
}
