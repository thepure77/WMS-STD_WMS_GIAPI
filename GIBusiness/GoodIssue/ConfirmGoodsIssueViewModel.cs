using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class ConfirmGoodsIssueViewModel
    {
 
        public Guid? goodsIssueIndex { get; set; }
        public string goodsIssueNo { get; set; }

        public string pickTicketNo { get; set; }

        public string createBy { get; set; }


    }
}
