using GIDataAccess.Models;
using MasterDataBusiness.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class PickbinbalanceViewModel
    {
        public string binbalance_Index { get; set; }
        public string goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public string goodsReceive_date { get; set; }
        public string tag_Index { get; set; }
        public string tag_No { get; set; }
        public string product_Lot { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public decimal? qty { get; set; }
        public decimal? weight { get; set; }
        public string productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public decimal? productConversion_Ratio { get; set; }
        public string status_Index { get; set; }
        public string status_Id { get; set; }
        public string status_Name { get; set; }
        public string location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public decimal? pick { get; set; }

        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }

        public string goodsIssueItemLocation_Index { get; set; }

        public string create_By { get; set; }
        public string Process_Index { get; set; }
        public string binCardReserve_Index { get; set; }

        public string binCard_Index { get; set; }
        public string ref_Document_Index { get; set; }
        public string ref_Document_No { get; set; }
        public string ref_Document_LineNum { get; set; }
        public string ref_DocumentItem_Index { get; set; }
        public string wave_Index { get; set; }
        public ProductConversionViewModelDoc unit { get; set; }
        //public im_GoodsIssueItemLocation GIIL { get; set; }

        public string goodsReceive_Date { get; set; }
        public string goodsReceive_MFG_Date { get; set; }
        public string goodsReceive_EXP_Date { get; set; }
        public string warehouse_Index { get; set; }
        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }
        public string zone_Index { get; set; }
        public string zone_Id { get; set; }
        public string zone_Name { get; set; }
        public decimal? unitWeight { get; set; }
    }

    public class actionResultPickbinbalanceViewModel : Result
    {
        public PickbinbalanceViewModel items { get; set; }
        public ResultPickbinbalanceViewModel resultBinbalance { get; set; }
    }
    public class ResultPickbinbalanceViewModel
    {
        public string binbalance_Index { get; set; }
        public string goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public string tag_Index { get; set; }
        public string tag_No { get; set; }
        public string product_Lot { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public decimal? qty { get; set; }
        public decimal? weight { get; set; }
        public string productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public decimal? productConversion_Ratio { get; set; }
        public string status_Index { get; set; }
        public string status_Id { get; set; }
        public string status_Name { get; set; }
        public string location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public decimal? pick { get; set; }

        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }

        public string goodsIssueItemLocation_Index { get; set; }

        public string create_By { get; set; }

        public string process_Index { get; set; }

        public string binCardReserve_Index { get; set; }

        public string binCard_Index { get; set; }

        public im_GoodsIssueItemLocation GIIL { get; set; }
    }
    public class ListPickbinbalanceViewModel
    {
        public List<PickbinbalanceViewModel> items { get; set; }
    }
}
