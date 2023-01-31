using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{

    public partial class AssignJobViewModel
    {
        public string Template { get; set; }

        public string Create_By { get; set; }

        public List<listGoodsIssueViewModel> listGoodsIssueViewModel { get; set; }

    }

    public class View_AssignJobViewModel
    {

        public Guid? location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public Guid goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public Guid goodsIssueItem_Index { get; set; }
        public Guid goodsIssueItemLocation_Index { get; set; }
        public Guid? warehouse_Index { get; set; }
        public Guid? zone_Index { get; set; }
        public Guid? planGoodsIssue_Index { get; set; }
        public Guid? route_Index { get; set; }
        public Guid? product_Index { get; set; }

        public string product_Id { get; set; }

        public DateTime? goodsIssue_Date { get; set; }
        public decimal qty { get; set; }

        public decimal totalQty { get; set; }

        public List<View_AssignJobViewModel> ResultItem { get; set; }


    }
}
