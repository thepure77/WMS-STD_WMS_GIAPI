using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public partial class ScanPicksearchViewModel
    {
        public string task_Index { get; set; }
        public string task_No { get; set; }
        public string taskItem_Index { get; set; }
        public string product_Index { get; set; }
        public string product_Id { get; set; }
        public string productConversionBarcode { get; set; }
        public string tagOut_Index { get; set; }
        public string tagOut_No { get; set; }
        public string tagNo { get; set; }
        public string userName { get; set; }
        public string confirmLocation { get; set; }
        public string confirm_location_Index { get; set; }
        public string location_Index { get; set; }
        public string confirm_location_Id { get; set; }
        public string location_Id { get; set; }
        public string confirm_location_Name { get; set; }
        public string location_Name { get; set; }
        public string process_Index { get; set; }
        public string goodsissue_Index { get; set; }
        public string goodsissue_No { get; set; }
        public string truckload_no { get; set; }
        public bool isChkUpdate { get; set; }
        public string Dock_Name { get; set; }
        public string Dock_Id { get; set; }
        public Guid? Dock_Index { get; set; }
        public Guid? TruckLoad_Index { get; set; }
    }
    public partial class actionResultScanPicksearchViewModel : Result
    {
        public bool IsSerial { get; set; }
        public List<taskViewModel> items { get; set; }
        public List<PickDockViewModel> itemsDock { get; set; }

        public List<taskItemViewModel> itemsDetail { get; set; }
    }
    public partial class taskItemViewModel
    {
        public string taskItem_Index { get; set; }

        public string task_Index { get; set; }

        public string task_No { get; set; }

        public string lineNum { get; set; }

        public Guid? tagItem_Index { get; set; }
        public Guid? TruckLoad_Index { get; set; }

        public Guid? tag_Index { get; set; }

        public string tag_No { get; set; }

        public string Dock_Name { get; set; }
        public string Dock_Id { get; set; }

        public Guid? Dock_Index { get; set; }
        public Guid? product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string product_SecondName { get; set; }

        public string product_ThirdName { get; set; }

        public string product_Lot { get; set; }

        public Guid? itemStatus_Index { get; set; }

        public string itemStatus_Id { get; set; }

        public string itemStatus_Name { get; set; }

        public Guid? location_Index { get; set; }

        public string location_Id { get; set; }

        public string location_Name { get; set; }

        public decimal? qty { get; set; }

        public decimal? ratio { get; set; }

        public decimal? totalQty { get; set; }

        public Guid? productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public DateTime? mfg_Date { get; set; }

        public DateTime? exp_Date { get; set; }

        public decimal? unitWeight { get; set; }

        public decimal? weight { get; set; }

        public decimal? unitWidth { get; set; }

        public decimal? unitLength { get; set; }

        public decimal? unitHeight { get; set; }

        public decimal? unitVolume { get; set; }
        public decimal? volume { get; set; }
        public decimal? unitPrice { get; set; }
        public decimal? price { get; set; }
        public string documentRef_No1 { get; set; }
        public string documentRef_No2 { get; set; }
        public string documentRef_No3 { get; set; }
        public string documentRef_No4 { get; set; }
        public string documentRef_No5 { get; set; }
        public int? document_Status { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public Guid? ref_Process_Index { get; set; }
        public string ref_Document_No { get; set; }
        public string ref_Document_LineNum { get; set; }
        public Guid? ref_Document_Index { get; set; }
        public Guid? ref_DocumentItem_Index { get; set; }
        public Guid? reasonCode_Index { get; set; }
        public string reasonCode_Id { get; set; }
        public string reasonCode_Name { get; set; }
        public string create_By { get; set; }
        public DateTime? create_Date { get; set; }
        public string update_By { get; set; }
        public DateTime? update_Date { get; set; }
        public string cancel_By { get; set; }
        public DateTime? cancel_Date { get; set; }
        public Guid? tagOutPick_Index { get; set; }
        public string tagOutPick_No { get; set; }
        public decimal? picking_Qty { get; set; }
        public decimal? picking_Ratio { get; set; }
        public decimal? picking_TotalQty { get; set; }
        public string picking_By { get; set; }
        public DateTime? picking_Date { get; set; }
        public int? picking_Status { get; set; }
        public decimal? splitQty { get; set; }
        public Guid? planGoodsIssue_Index { get; set; }
        public Guid? planGoodsIssueItem_Index { get; set; }
        public string planGoodsIssue_No { get; set; }
        public Guid? pick_ProductConversion_Index { get; set; }
        public string pick_ProductConversion_Id { get; set; }
        public string pick_ProductConversion_Name { get; set; }
        public decimal? pick_ProductConversion_Ratio { get; set; }
        public string productConversionBarcode { get; set; }
        public Guid? tagOut_Index { get; set; }
        public string tagOut_No { get; set; }
        public string imageUri { get; set; }
        public bool IsSerial { get; set; }

        public List<plangoodsissue> plangoodsissue { get; set; }
        public List<group_product> group_product { get; set; }

    }
    public class plangoodsissue
    {
        public string planGoodsIssue_No { get; set; }
        public int qty { get; set; }
    }

    public class group_product
    {
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string ProductConversion_Name { get; set; }
        public int qty { get; set; }
    }

    public class printer_model
    {
        public Guid Printer_Index { get; set; }
        public string Printer_Id { get; set; }
        public string Printer_Name { get; set; }
    }

    public class taskViewModel
    {
        public string task_Index { get; set; }
        public string task_No { get; set; }
        public string process_Index { get; set; }
        public Guid? taskGroup_Index { get; set; }
        public string taskGroup_Id { get; set; }
        public string taskGroup_Name { get; set; }
        public int? documentPriority_Status { get; set; }
        public string documentRef_No1 { get; set; }
        public string documentRef_No2 { get; set; }
        public string documentRef_No3 { get; set; }
        public string documentRef_No4 { get; set; }
        public string documentRef_No5 { get; set; }
        public int? document_Status { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public string create_By { get; set; }
        public DateTime? create_Date { get; set; }
        public string update_By { get; set; }
        public DateTime? update_Date { get; set; }
        public string cancel_By { get; set; }
        public DateTime? cancel_Date { get; set; }
        public string doTask_By { get; set; }
        public DateTime? doTask_Date { get; set; }
        public string userAssign { get; set; }
        public string plangoodsissue_Index { get; set; }
        public string plangoodsissue_No { get; set; }
        public string goodsissue_Index { get; set; }
        public string goodsissue_No { get; set; }
        //public string DocumentRef_No1 { get; set; }
        public string truckload_no { get; set; }
        public string Dock_name { get; set; }
    }

    public class PickDockViewModel
    {
        public Guid TruckLoad_Index { get; set; }

        public string TruckLoad_No { get; set; }

        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public string Dock_Name { get; set; }
    }

    public class View_TaskInsertBinCardViewModel
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
        public DateTime? mfg_Date { get; set; }
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

        public Guid? itemStatus_Index_To { get; set; }
        public string itemStatus_Id_To { get; set; }
        public string itemStatus_Name_To { get; set; }

        public string userName { get; set; }
        public bool isTransfer { get; set; }
        public bool isScanSplit { get; set; }
        public bool isScanPick { get; set; }
        public bool isScanToDock { get; set; }
    }
}
