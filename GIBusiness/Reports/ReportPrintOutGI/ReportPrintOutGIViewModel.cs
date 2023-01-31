using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.Reports
{
    public class ReportPrintOutGIViewModel
    {
        public Guid? owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string owner_TaxID { get; set; }

        public Guid? shipTo_Index { get; set; }
        public string shipTo_Id { get; set; }
        public string shipTo_Name { get; set; }
        public string shipTo_TaxID { get; set; }

        public Guid? goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }

        public Guid? planGoodsIssue_Index { get; set; }
        public string planGoodsIssue_No { get; set; }
        public string planGoodsIssue_Date { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Name { get; set; }
        public string date_Print { get; set; }
        public string lineNum { get; set; }
        public string tag_No { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public decimal? qty { get; set; }
        public string productConversion_Name { get; set; }
        public Guid? warehouse_Index { get; set; }
        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }
        public string goodsIssue_Barcode { get; set; }
        public bool checkQuery { get; set; }
        public string checkBomPO { get; set; }
        public string checkBomDate { get; set; }
    }


}
