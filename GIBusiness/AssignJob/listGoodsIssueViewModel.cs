using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{

    public partial class listGoodsIssueViewModel
    {
        public Guid? goodsIssue_Index { get; set; }

        public string goodsIssue_No { get; set; }


        public string goodsIssue_Date { get; set; }
        public string goodsIssue_Date_To { get; set; }

        public string planGoodsIssue_Date { get; set; }

        public string planGoodsIssue_Due_Date { get; set; }
        public string planGoodsIssue_Due_Date_To { get; set; }


        public Guid? owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }

        public int? document_Status { get; set; }


        public string planGoodsIssue_No { get; set; }


        public Guid? documentType_Index { get; set; }


        public string documentType_Id { get; set; }


        public string documentType_Name { get; set; }

        public decimal? weight { get; set; }

        public decimal? qty { get; set; }

        public string create_By { get; set; }
        public string document_Remark { get; set; }


        public string processStatus_Name { get; set; }


    }
}
