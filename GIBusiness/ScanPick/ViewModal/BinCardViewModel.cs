using System;
using System.Collections.Generic;

namespace GIBusiness.GoodIssue
{
    public class BinCardViewModel
    {
        public Guid? taskitem_Index { get; set; }
        public Guid? task_Index { get; set; }
        public string task_No { get; set; }
        public Guid? ref_Document_Index { get; set; }
        public Guid? ref_DocumentItem_Index { get; set; }
        public string ref_Document_No { get; set; }
        public Guid? tagOutItem_Index { get; set; }
        public Guid? tagOut_Index { get; set; }
        public string tagOut_No { get; set; }
        public DateTime? goodsIssue_Date { get; set; }
        public Guid? documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public Guid? tagItem_Index { get; set; }
        public Guid? tag_Index { get; set; }
        public string tag_No { get; set; }
        public Guid? tag_Index_To { get; set; }
        public string tag_No_To { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }
        public string product_Lot { get; set; }
        public string product_Lot_To { get; set; }
        public Guid? itemStatus_Index { get; set; }
        public string itemStatus_Id { get; set; }
        public string itemStatus_Name { get; set; }
        public Guid? productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public Guid owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public Guid? location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public DateTime? exp_Date { get; set; }
        public DateTime? exp_Date_To { get; set; }
        public DateTime? mfg_Date { get; set; }
        public DateTime? mfg_Date_To { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public decimal? picking_Qty { get; set; }
        public decimal? picking_Ratio { get; set; }
        public decimal? picking_TotalQty { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
        public Guid? binBalance_Index { get; set; }
        public int? gIILStatus { get; set; }
        public int? gLStatus { get; set; }
        public int? taskItemStatus { get; set; }
        public int? pickStatus { get; set; }
        public Guid? process_Index { get; set; }
        public Guid? location_Index_To { get; set; }
        public string location_Id_To { get; set; }
        public string location_Name_To { get; set; }
        public string userName { get; set; }

        public Guid? itemStatus_Index_To { get; set; }
        public string itemStatus_Id_To { get; set; }
        public string itemStatus_Name_To { get; set; }

        public bool isTransfer { get; set; }
        public bool isScanPick { get; set; }
        public bool isScanToDock { get; set; }
        public bool isScanSplit { get; set; }

        public DateTime? goodsReceive_Date { get; set; }

        public string erp_Location { get; set; }
        public string erp_Location_To { get; set; }

        public Guid? goodsReceiveItem_Index { get; set; }
        public Guid? goodsReceiveItemLocation_Index { get; set; }
    }
    public class ListBinCardViewModel
    {
        public List<BinCardGRViewModel> items { get; set; }
    }
    public class BinCardGRViewModel
    {
        public Guid? process_Index { get; set; }
        public Guid? documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public Guid? goodsreceive_Index { get; set; }
        public Guid? goodsreceiveItem_Index { get; set; }
        public Guid? goodsreceiveItemLocation_Index { get; set; }
        public string bincard_No { get; set; }
        public DateTime? binCard_Date { get; set; }
        public Guid? tagitem_Index { get; set; }
        public Guid? tag_index { get; set; }
        public string tag_no { get; set; }
        public Guid? tag_index_To { get; set; }
        public string tag_no_To { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }
        public Guid? product_Index_To { get; set; }
        public string product_Id_To { get; set; }
        public string product_Name_To { get; set; }
        public string product_SecondName_To { get; set; }
        public string product_ThirdName_To { get; set; }
        public string product_Lot { get; set; }
        public string product_Lot_To { get; set; }
        public Guid? itemstatus_Index { get; set; }
        public string itemstatus_Id { get; set; }
        public string itemstatus_Name { get; set; }
        public Guid? itemstatus_Index_To { get; set; }
        public string itemstatus_Id_To { get; set; }
        public string itemstatus_Name_To { get; set; }
        public Guid? productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public Guid? owner_index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public Guid? owner_index_To { get; set; }
        public string owner_Id_To { get; set; }
        public string owner_Name_To { get; set; }
        public Guid? location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public Guid? location_Index_To { get; set; }
        public string location_Id_To { get; set; }
        public string location_Name_To { get; set; }
        public DateTime? goodsReceive_EXP_Date { get; set; }
        public DateTime? goodsReceive_EXP_Date_To { get; set; }
        public decimal? bincard_QtyIn { get; set; }
        public decimal? bincard_QtyOut { get; set; }
        public decimal? bincard_QtySign { get; set; }
        public decimal? bincard_WeightIn { get; set; }
        public decimal? bincard_WeightOut { get; set; }
        public decimal? bincard_WeightSign { get; set; }
        public decimal? bincard_VolumeIn { get; set; }
        public decimal? bincard_VolumeOut { get; set; }
        public decimal? bincard_VolumeSign { get; set; }
        public string ref_document_No { get; set; }
        public Guid? ref_document_Index { get; set; }
        public Guid? ref_documentItem_Index { get; set; }
        public string tagoutItem_Index { get; set; }
        public Guid? tagout_Index { get; set; }
        public string tagout_No { get; set; }
        public Guid? tagout_Index_To { get; set; }
        public string tagout_No_To { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public string create_By { get; set; }
        public DateTime? create_Date { get; set; }
        public Guid? binbalance_Index { get; set; }


        public decimal? totalQty { get; set; }
        public decimal? weight { get; set; }
        public decimal? volume { get; set; }
        public bool isCheckBinCard { get; set; }

   

    }
}

