using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.Reports
{
    public class ReportPickingTagViewModel
    {
        public Guid? task_Index { get; set; }
        public Guid? taskItem_Index { get; set; }
        public Guid? goodsIssue_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string location_Id { get; set; }
        public string totalQty { get; set; }
        public string productConversion_Name { get; set; }
        public string goodsIssue_No { get; set; }
        public string shipTo_Address { get; set; }
        public string document_Date { get; set; }
        public string planGoodsIssue_No { get; set; }
        public string planGoodsIssue_No_Barcode { get; set; }
        public string task_No { get; set; }
        public string task_No_Barcode { get; set; }
        public string print_Date { get; set; }
        public string ratio { get; set; }
        public string productConversion_Ratio { get; set; }
    }

    public class ListReportPickingTagViewModel
    {
        public List<ReportPickingTagViewModel> ListReport { get; set; }
    }
}
