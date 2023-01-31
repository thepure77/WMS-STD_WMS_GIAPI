using GIBusiness.PlanGoodIssue;
using GIDataAccess.Models;
using MasterDataBusiness.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class View_GoodsIssueBOM_ViewModel 
    {
        [Key]
        public Guid PlanGoodsIssueItem_Index { get; set; }
        public Guid PlanGoodsIssue_Index { get; set; }
        public Guid Product_Index { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public string SO_Type { get; set; }
        public string ITEM_CAT { get; set; }
        public string LineNum { get; set; }
        public string HIGH_LV_ITEM { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Lot { get; set; }

        public decimal Qty { get; set; }

        public decimal Ratio { get; set; }
        public decimal TotalQty { get; set; }


        public decimal GI_TotalQty { get; set; }
        public decimal BOM_Ratio { get; set; }
        public decimal BomTotalQTY { get; set; }
        public decimal ModBomTotalQty { get; set; }

        public decimal BomRemianTotalQty { get; set; }

        public decimal BomDiffTotalQty { get; set; }



        public string ProductConversion_Id { get; set; }

        public string ProductConversion_Name { get; set; }
        public Guid ProductConversion_Index { get; set; }




    }
}
