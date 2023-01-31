using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.Reports
{
    public class ReportPickViewModel
    {
        public Guid? task_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }
        public string task_No { get; set; }
        public string assign_By { get; set; }
        public string productConversionBarcode { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public decimal? qty { get; set; }
        public string productConversion_Name { get; set; }
        public string location_Name { get; set; }
        public string owner_Name { get; set; }
        public string document_Remark { get; set; }
        public string lineNum { get; set; }

    }


}
