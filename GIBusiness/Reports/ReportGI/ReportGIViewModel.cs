using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.Reports
{
    public class ReportGIViewModel
    {
        //header
        public string GoodsIssue_No { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public string ShipTo_AddressName { get; set; }
        public string GoodsIssue_Date { get; set; }
        public string Document_Date { get; set; }
        public string Warehouse_Name { get; set; }
        public string Warehouse_Name_To { get; set; }
        public string Sloc_Name { get; set; }
        public string Sloc_Name_To { get; set; }
        public string DocumentRef_No5 { get; set; }
        public string DocumentRef_No1 { get; set; }

        //detail    
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal Qty { get; set; }
        public string ProductConversion_Name { get; set; }

        //footer

        public bool isDemand_Payment { get; set; }
        public string demand_Payment_Name { get; set; }
        public string demand_Payment_pos_Name { get; set; }

        public bool isPayment { get; set; }
        public string payment_Name { get; set; }
        public string payment_pos_Name { get; set; }

        public bool isRecipent { get; set; }
        public string recipent_Name { get; set; }
        public string recipent_pos_Name { get; set; }

        public bool isRecorder { get; set; }
        public string recorder_Name { get; set; }
        public string recorder_pos_Name { get; set; }
    }
}
