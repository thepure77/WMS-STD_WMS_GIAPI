using BomBusiness;
using GIDataAccess.Models;
using MasterDataBusiness.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class RunWaveViewModel
    {
        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }
        public string goodsIssue_Due_Date { get; set; }
        public string goodsIssue_Time { get; set; }
        public string owner_Index { get; set; }
        public string owner_Name { get; set; }
        public string owner_Id { get; set; }
        public string documentRef_No1 { get; set; }
        public string document_Remark { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string create_by { get; set; }
    }
    public class actionResultRunWaveViewModelViewModel : Result
    {
        public RunWaveViewModel items { get; set; }
        public List<plangoodsissueitemViewModel> pgii { get; set; }
    }
    public class RunWaveFilterViewModel
    {
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }
        public string goodsIssue_Due_Date { get; set; }
        public string goodsIssue_Time { get; set; }
        public string owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string documentRef_No1 { get; set; }
        public string document_Remark { get; set; }
        public string create_by { get; set; }
        public string wave_Index { get; set; }
        public string wave_Id { get; set; }
        public string wave_Name { get; set; }
        public List<plangoodsissueitemViewModel> listGoodsIssueItemViewModel { get; set; }
        public List<BomItemViewModel> listGoodsIssueItemBomViewModel { get; set; }
    }

    public class plangoodsissueitemViewModel
    {
        public Guid planGoodsIssueItem_Index { get; set; }
        public Guid planGoodsIssue_Index { get; set; }
        public string lineNum { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }
        public string product_Lot { get; set; }
        public Guid? itemStatus_Index { get; set; }
        public string itemStatus_Id { get; set; }
        public string itemStatus_Name { get; set; }
        public decimal? qty { get; set; }
        public decimal? qty_base { get; set; }
        public decimal? ratio { get; set; }
        public decimal? totalQty { get; set; }
        public Guid? productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public string productConversion_Base { get; set; }
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
        public string documentItem_Remark { get; set; }
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
        public decimal? qtyBackOrder { get; set; }
        public int? backOrderStatus { get; set; }
        public string planGoodsIssue_Size { get; set; }
        public decimal? qtyQA { get; set; }
        public int? isQA { get; set; }
        public decimal? qty_Inner_Pack { get; set; }
        public decimal? qty_Sup_Pack { get; set; }
        public string imageUri { get; set; }
        public string zoneCode { get; set; }
        public string batch_Id { get; set; }
        public string qa_By { get; set; }
        public DateTime? qa_Date { get; set; }
        public int? runWave_Status { get; set; }
        public string planGoodsIssue_No { get; set; }
        public decimal? countQty { get; set; }
        public decimal? qtyPlan { get; set; }
        public bool isDelete { get; set; }
        public string warehouse_Index_To { get; set; }
        public string warehouse_Id_To { get; set; }
        public string warehouse_Name_To { get; set; }
        public string ref_Document_No { get; set; }
        public string product_Id_RefNo2 { get; set; }
        public string erp_Location { get; set; }
    }

    public class WaveRuleViewModel
    {
        public string waveRule_Index { get; set; }

        public string waveRule_Id { get; set; }

        public int? waveRule_Seq { get; set; }

        public string wave_Index { get; set; }

        public string wave_Id { get; set; }

        public string wave_Name { get; set; }

        public string rule_Index { get; set; }

        public string rule_Id { get; set; }

        public string rule_Name { get; set; }

        public int? isActive { get; set; }

        public int? isDelete { get; set; }

        public int? isSystem { get; set; }

        public int? status_Id { get; set; }

        public string create_By { get; set; }

        public string create_Date { get; set; }

        public string update_By { get; set; }

        public string update_Date { get; set; }

        public string cancel_By { get; set; }

        public string cancel_Date { get; set; }
        public string process_Index { get; set; }
    }

    public class WaveTemplateViewModel
    {
        public string wave_Index { get; set; }
        public string wave_Id { get; set; }

        public string wave_Name { get; set; }

        public string waveRule_Id { get; set; }

        public int? waveRule_Seq { get; set; }

        public string waveRule_Index { get; set; }


        public string process_Index { get; set; }

        public string process_Id { get; set; }

        public string process_Name { get; set; }


        public string rule_Index { get; set; }

        public string rule_Id { get; set; }

        public string rule_Name { get; set; }

        public int? rule_Seq { get; set; }


        public string ruleConditionField_Index { get; set; }

        public string ruleConditionField_Name { get; set; }


        public string ruleConditionOperation_Index { get; set; }

        public string ruleConditionOperationType { get; set; }

        public string ruleConditionOperation { get; set; }

        public string ruleCondition_Index { get; set; }

        public string ruleCondition_Param { get; set; }

        public int? ruleCondition_Seq { get; set; }

        public int? isSearch { get; set; }

        public int? isSort { get; set; }

        public int? isSource { get; set; }

        public int? isDestination { get; set; }

        public long? rowIndex { get; set; }
    }
    public class getViewBinbalanceViewModel
    {
        public Guid Owner_Index { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Lot { get; set; }
        public Guid? ItemStatus_Index { get; set; }
        public DateTime? MFG_Date { get; set; }
        public DateTime? EXP_Date { get; set; }
        public string UDF_1 { get; set; }
        public string UDF_2 { get; set; }
        public string UDF_3 { get; set; }
        public string UDF_4 { get; set; }
        public string UDF_5 { get; set; }
        public bool isUseAttribute { get; set; }
        public string isuse { get; set; }
        public bool isActive { get; set; }
        public decimal? qtyPreTag { get; set; }
        public string ERP_Location { get; set; }
        
    }

    public class View_WaveBinBalanceViewModel
    {
        public string binBalance_Index { get; set; }
        public string owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public string goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public string goodsReceive_Date { get; set; }
        public string goodsReceiveItem_Index { get; set; }
        public string goodsReceiveItemLocation_Index { get; set; }
        public string tagItem_Index { get; set; }
        public string tag_Index { get; set; }
        public string tag_No { get; set; }
        public string product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }
        public string product_Lot { get; set; }
        public string itemStatus_Index { get; set; }
        public string itemStatus_Id { get; set; }
        public string itemStatus_Name { get; set; }
        public string goodsReceive_MFG_Date { get; set; }
        public string goodsReceive_EXP_Date { get; set; }
        public string goodsReceive_ProductConversion_Index { get; set; }
        public string goodsReceive_ProductConversion_Id { get; set; }
        public string goodsReceive_ProductConversion_Name { get; set; }
        public decimal? binBalance_Ratio { get; set; }

        public decimal? binBalance_QtyBegin { get; set; }

        public decimal? binBalance_WeightBegin { get; set; }

        public Guid? binBalance_WeightBegin_Index { get; set; }

        public string binBalance_WeightBegin_Id { get; set; }

        public string binBalance_WeightBegin_Name { get; set; }

        public decimal? binBalance_WeightBeginRatio { get; set; }

        public decimal? binBalance_NetWeightBegin { get; set; }

        public Guid? binBalance_NetWeightBegin_Index { get; set; }

        public string binBalance_NetWeightBegin_Id { get; set; }

        public string binBalance_NetWeightBegin_Name { get; set; }


        public decimal? binBalance_NetWeightBeginRatio { get; set; }


        public decimal? binBalance_GrsWeightBegin { get; set; }

        public Guid? binBalance_GrsWeightBegin_Index { get; set; }


        public string binBalance_GrsWeightBegin_Id { get; set; }


        public string binBalance_GrsWeightBegin_Name { get; set; }


        public decimal? binBalance_GrsWeightBeginRatio { get; set; }


        public decimal? binBalance_WidthBegin { get; set; }

        public Guid? binBalance_WidthBegin_Index { get; set; }


        public string binBalance_WidthBegin_Id { get; set; }


        public string binBalance_WidthBegin_Name { get; set; }


        public decimal? binBalance_WidthBeginRatio { get; set; }


        public decimal? binBalance_LengthBegin { get; set; }

        public Guid? binBalance_LengthBegin_Index { get; set; }


        public string binBalance_LengthBegin_Id { get; set; }


        public string binBalance_LengthBegin_Name { get; set; }


        public decimal? binBalance_LengthBeginRatio { get; set; }


        public decimal? binBalance_HeightBegin { get; set; }

        public Guid? binBalance_HeightBegin_Index { get; set; }


        public string binBalance_HeightBegin_Id { get; set; }


        public string binBalance_HeightBegin_Name { get; set; }


        public decimal? binBalance_HeightBeginRatio { get; set; }


        public decimal? binBalance_UnitVolumeBegin { get; set; }


        public decimal? binBalance_VolumeBegin { get; set; }


        public decimal? binBalance_QtyBal { get; set; }


        public decimal? binBalance_WeightBal { get; set; }

        public Guid? binBalance_UnitWeightBal_Index { get; set; }


        public string binBalance_UnitWeightBal_Id { get; set; }


        public string binBalance_UnitWeightBal_Name { get; set; }


        public decimal? binBalance_UnitWeightBalRatio { get; set; }


        public decimal? binBalance_UnitWeightBal { get; set; }

        public Guid? binBalance_WeightBal_Index { get; set; }


        public string binBalance_WeightBal_Id { get; set; }


        public string binBalance_WeightBal_Name { get; set; }


        public decimal? binBalance_WeightBalRatio { get; set; }


        public decimal? binBalance_UnitNetWeightBal { get; set; }

        public Guid? binBalance_UnitNetWeightBal_Index { get; set; }


        public string binBalance_UnitNetWeightBal_Id { get; set; }


        public string binBalance_UnitNetWeightBal_Name { get; set; }


        public decimal? binBalance_UnitNetWeightBalRatio { get; set; }


        public decimal? binBalance_NetWeightBal { get; set; }

        public Guid? binBalance_NetWeightBal_Index { get; set; }


        public string binBalance_NetWeightBal_Id { get; set; }


        public string binBalance_NetWeightBal_Name { get; set; }


        public decimal? binBalance_NetWeightBalRatio { get; set; }


        public decimal? binBalance_UnitGrsWeightBal { get; set; }

        public Guid? binBalance_UnitGrsWeightBal_Index { get; set; }


        public string binBalance_UnitGrsWeightBal_Id { get; set; }


        public string binBalance_UnitGrsWeightBal_Name { get; set; }


        public decimal? binBalance_UnitGrsWeightBalRatio { get; set; }


        public decimal? binBalance_GrsWeightBal { get; set; }

        public Guid? binBalance_GrsWeightBal_Index { get; set; }


        public string binBalance_GrsWeightBal_Id { get; set; }


        public string binBalance_GrsWeightBal_Name { get; set; }


        public decimal? binBalance_GrsWeightBalRatio { get; set; }


        public decimal? binBalance_UnitWidthBal { get; set; }

        public Guid? binBalance_UnitWidthBal_Index { get; set; }


        public string binBalance_UnitWidthBal_Id { get; set; }


        public string binBalance_UnitWidthBal_Name { get; set; }


        public decimal? binBalance_UnitWidthBalRatio { get; set; }


        public decimal? binBalance_WidthBal { get; set; }

        public Guid? binBalance_WidthBal_Index { get; set; }


        public string binBalance_WidthBal_Id { get; set; }


        public string binBalance_WidthBal_Name { get; set; }


        public decimal? binBalance_WidthBalRatio { get; set; }


        public decimal? binBalance_UnitLengthBal { get; set; }

        public Guid? binBalance_UnitLengthBal_Index { get; set; }


        public string binBalance_UnitLengthBal_Id { get; set; }


        public string binBalance_UnitLengthBal_Name { get; set; }


        public decimal? binBalance_UnitLengthBalRatio { get; set; }


        public decimal? binBalance_LengthBal { get; set; }

        public Guid? binBalance_LengthBal_Index { get; set; }


        public string binBalance_LengthBal_Id { get; set; }


        public string binBalance_LengthBal_Name { get; set; }


        public decimal? binBalance_LengthBalRatio { get; set; }


        public decimal? binBalance_UnitHeightBal { get; set; }

        public Guid? binBalance_UnitHeightBal_Index { get; set; }


        public string binBalance_UnitHeightBal_Id { get; set; }


        public string binBalance_UnitHeightBal_Name { get; set; }


        public decimal? binBalance_UnitHeightBalRatio { get; set; }


        public decimal? binBalance_HeightBal { get; set; }

        public Guid? binBalance_HeightBal_Index { get; set; }


        public string binBalance_HeightBal_Id { get; set; }


        public string binBalance_HeightBal_Name { get; set; }


        public decimal? binBalance_HeightBalRatio { get; set; }


        public decimal? binBalance_UnitVolumeBal { get; set; }


        public decimal? binBalance_VolumeBal { get; set; }


        public decimal? binBalance_QtyReserve { get; set; }


        public decimal? binBalance_WeightReserve { get; set; }

        public Guid? binBalance_WeightReserve_Index { get; set; }


        public string binBalance_WeightReserve_Id { get; set; }


        public string binBalance_WeightReserve_Name { get; set; }


        public decimal? binBalance_WeightReserveRatio { get; set; }


        public decimal? binBalance_NetWeightReserve { get; set; }

        public Guid? binBalance_NetWeightReserve_Index { get; set; }


        public string binBalance_NetWeightReserve_Id { get; set; }


        public string binBalance_NetWeightReserve_Name { get; set; }


        public decimal? binBalance_NetWeightReserveRatio { get; set; }


        public decimal? binBalance_GrsWeightReserve { get; set; }

        public Guid? binBalance_GrsWeightReserve_Index { get; set; }


        public string binBalance_GrsWeightReserve_Id { get; set; }


        public string binBalance_GrsWeightReserve_Name { get; set; }


        public decimal? binBalance_GrsWeightReserveRatio { get; set; }


        public decimal? binBalance_WidthReserve { get; set; }

        public Guid? binBalance_WidthReserve_Index { get; set; }


        public string binBalance_WidthReserve_Id { get; set; }


        public string binBalance_WidthReserve_Name { get; set; }


        public decimal? binBalance_WidthReserveRatio { get; set; }


        public decimal? binBalance_LengthReserve { get; set; }

        public Guid? binBalance_LengthReserve_Index { get; set; }


        public string binBalance_LengthReserve_Id { get; set; }


        public string binBalance_LengthReserve_Name { get; set; }


        public decimal? binBalance_LengthReserveRatio { get; set; }

        public decimal? binBalance_HeightReserve { get; set; }

        public Guid? binBalance_HeightReserve_Index { get; set; }

        public string binBalance_HeightReserve_Id { get; set; }
        public string binBalance_HeightReserve_Name { get; set; }

        public decimal? binBalance_HeightReserveRatio { get; set; }

        public decimal? binBalance_UnitVolumeReserve { get; set; }

        public decimal? binBalance_VolumeReserve { get; set; }
        public string productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }

        public decimal? unitPrice { get; set; }

        public Guid? unitPrice_Index { get; set; }

        public string unitPrice_Id { get; set; }

        public string unitPrice_Name { get; set; }

        public decimal? price { get; set; }

        public Guid? price_Index { get; set; }

        public string price_Id { get; set; }

        public string price_Name { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string update_By { get; set; }
        public string update_Date { get; set; }
        public string cancel_By { get; set; }
        public string cancel_Date { get; set; }
        public string isUse { get; set; }
        public int? binBalance_Status { get; set; }
        public int? picking_Seq { get; set; }
        public int? ageRemain { get; set; }
        public int? productShelfLife_D { get; set; }

        public string invoice_No { get; set; }
        public string declaration_No { get; set; }
        public string hs_Code { get; set; }
        public string conutry_of_Origin { get; set; }
        public decimal? tax1 { get; set; }
        public Guid? tax1_Currency_Index { get; set; }
        public string tax1_Currency_Id { get; set; }
        public string tax1_Currency_Name { get; set; }
        public decimal? tax2 { get; set; }
        public Guid? tax2_Currency_Index { get; set; }
        public string tax2_Currency_Id { get; set; }
        public string tax2_Currency_Name { get; set; }
        public decimal? tax3 { get; set; }
        public Guid? tax3_Currency_Index { get; set; }
        public string tax3_Currency_Id { get; set; }
        public string tax3_Currency_Name { get; set; }
        public decimal? tax4 { get; set; }
        public Guid? tax4_Currency_Index { get; set; }
        public string tax4_Currency_Id { get; set; }
        public string tax4_Currency_Name { get; set; }
        public decimal? tax5 { get; set; }
        public Guid? tax5_Currency_Index { get; set; }
        public string tax5_Currency_Id { get; set; }
        public string tax5_Currency_Name { get; set; }
        public string erp_Location { get; set; }

        public int? location_Bay { get; set; }

    }

    public class RunWaveFilterV2ViewModel
    {

        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }
        public string goodsIssue_Due_Date { get; set; }
        public string goodsIssue_Time { get; set; }
        public string owner_Index { get; set; }
        public string owner_Name { get; set; }
        public string owner_Id { get; set; }
        public string documentRef_No1 { get; set; }
        public string documentRef_No2 { get; set; }
        public string documentRef_No3 { get; set; }
        public string documentRef_No4 { get; set; }
        public string documentRef_No5 { get; set; }
        public string document_Remark { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string warehouse_Index { get; set; }
        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }
        public string document_Date { get; set; }
        public string create_by { get; set; }
        public int? documentPriority_Status { get; set; }
        public string wave_Index { get; set; }
        public string wave_Id { get; set; }
        public string wave_Name { get; set; }
        public string groupwave_id { get; set; }

        public List<plangoodsissueitemViewModel> listGoodsIssueItemViewModel { get; set; }
        public List<BomItemViewModel> listGoodsIssueItemBomViewModel { get; set; }
    }

    public class actionResultRunWaveV2ViewModelViewModel : Result
    {
        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }
        public string goodsIssue_Due_Date { get; set; }
        public string goodsIssue_Time { get; set; }
        public string owner_Index { get; set; }
        public string owner_Name { get; set; }
        public string owner_Id { get; set; }
        public string documentRef_No1 { get; set; }
        public string document_Remark { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string warehouse_Index { get; set; }
        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }
        public string document_Date { get; set; }
        public string create_by { get; set; }
        public int? documentPriority_Status { get; set; }
        public string wave_Index { get; set; }
        public string wave_Id { get; set; }
        public string wave_Name { get; set; }
        public RunWaveViewModel items { get; set; }
        public List<plangoodsissueitemViewModel> pgii { get; set; }
        public List<BomItemViewModel> bi { get; set; }
    }

    public class ResultRunWave : Result
    {
        public ResultRunWave()
        {
            CheckWaveDipmodel = new List<CheckWaveDipmodel>();
            CheckWaveDipbyWavemodel = new List<CheckWaveDipbyWavemodel>();
            fixwavemodel = new List<fixwavemodel>();
        }
        public string tap { get; set; }
        public bool ready { get; set; }
        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }
        public string goodsIssue_Due_Date { get; set; }
        public string goodsIssue_Time { get; set; }
        public string owner_Index { get; set; }
        public string owner_Name { get; set; }
        public string owner_Id { get; set; }
        public string documentRef_No1 { get; set; }
        public string document_Remark { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string warehouse_Index { get; set; }
        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }
        public string document_Date { get; set; }
        public string create_by { get; set; }
        public int? documentPriority_Status { get; set; }
        public string wave_Index { get; set; }
        public string wave_Id { get; set; }
        public string wave_Name { get; set; }

        public string Detail { get; set; }
        public Guid Binbalance_index { get; set; }
        public string Tag_No { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal BinBalance_QtyBal { get; set; }
        public decimal BinBalance_QtyReserve { get; set; }
        public decimal GI_TotalQty { get; set; }
        public decimal diff { get; set; }
        public int? fixWave_Count { get; set; }

        public List<CheckWaveDipmodel> CheckWaveDipmodel { get; set; }
        public List<CheckWaveDipbyWavemodel> CheckWaveDipbyWavemodel { get; set; }
        public List<fixwavemodel> fixwavemodel { get; set; }
    }

    public class ResultCheckWaveWCS : Result
    {
        public ResultCheckWaveWCS()
        {
            CheckWaveWCSmodel = new List<ResultCheckWaveWCS>();
            CheckWaveWCS_respomodel = new List<ResultCheckWaveWCS>();
            Checklocation_PPmodel = new List<ResultChecklocation_PP>();
        }

        public string header { get; set; }

        public string Remaining { get; set; }

        public string descip { get; set; }


        public List<ResultCheckWaveWCS> CheckWaveWCSmodel { get; set; }
        public List<ResultCheckWaveWCS> CheckWaveWCS_respomodel { get; set; }
        public List<ResultChecklocation_PP> Checklocation_PPmodel { get; set; }

    }

    public class CheckWaveDipmodel
    {
        public string TruckLoad_No { get; set; }
        public string Appointment_Id { get; set; }
        public string Dock_Name { get; set; }
        public DateTime? Appointment_Date { get; set; }
        public string Appointment_Time { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public string ShipTo_Id { get; set; }
        public string ShipTo_Name { get; set; }
        public string BranchCode { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? Order_Qty { get; set; }
        public string Order_Unit { get; set; }
        public int Flag_ClaimReturn { get; set; }
    }

    public class Responcheckbudget
    {
        public string planGoodsIssue_no { get; set; }
        public string io { get; set; }
        public string checkAvaliable { get; set; }
        public decimal? total { get; set; }
        public string msgx { get; set; }
    }

    public class CheckWaveDipbyWavemodel
    {
        public long? RowIndex { get; set; }
        public string TruckLoad_No { get; set; }
        public string Appointment_Id { get; set; }
        public DateTime? Appointment_Date { get; set; }
        public string Appointment_Time { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public string Order_Seq { get; set; }
        public string LineNum { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? BU_Order_TotalQty { get; set; }
        public decimal? BU_GI_TotalQty { get; set; }
        public decimal? SU_Order_TotalQty { get; set; }
        public decimal? SU_GI_TotalQty { get; set; }
        public string SU_Unit { get; set; }
        public string ERP_Location { get; set; }
        public string Product_Lot { get; set; }
        public decimal? SU_Diff { get; set; }
        public string GoodsIssue_No { get; set; }
        public string Document_Remark { get; set; }
        public string DocumentRef_No3 { get; set; }
        public string Flag_Export { get; set; }
        public int Flag_ClaimReturn { get; set; }

    }

    public class ResultChecklocation_PP
    {
        public string PalletSuggestion { get; set; }

        public string Location_ID { get; set; }

        public string PalletID { get; set; }

        public string ProductID { get; set; }

        public string Product_Name { get; set; }

        public decimal? QtySaleUnit { get; set; }

        public string UOM { get; set; }

        public string ERP_Location { get; set; }

        public decimal? WMS_QTYBal { get; set; }

        public decimal? WMS_QtyReserve { get; set; }

    }

    public class fixwavemodel
    {
        public string Detail { get; set; }
        public Guid Binbalance_index { get; set; }
        public string Tag_No { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal BinBalance_QtyBal { get; set; }
        public decimal BinBalance_QtyReserve { get; set; }
        public decimal GI_TotalQty { get; set; }
        public decimal diff { get; set; }
        public string GoodsIssue_No { get; set; }
    }

    public class Checktaggi : Result
    {
        public Checktaggi()
        {
            checktaggimodel = new List<checktaggimodel>();
            checktagtfmodel = new List<checktagtfmodel>();
            fixwavemodel = new List<fixwavemodel>();
            checktagbygimodel = new List<checktagbygimodel>();
        }
        public string Tag_No { get; set; }
        public string GoodsIssue_No { get; set; }
        public string Location_Name { get; set; }
        public decimal GITotalQty { get; set; }

        public int Document_Status { get; set; }
        public int Item_Document_Status { get; set; }
        public string GoodsTransfer_No { get; set; }
        public string DocumentType_Name { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Create_By { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal Qty { get; set; }
        public decimal TotalQty { get; set; }
        public string ProductConversion_Name { get; set; }
        public string Location_Name_To { get; set; }
        public string ItemStatus_Name { get; set; }
        public string ItemStatus_Name_To { get; set; }
        public string ERP_Location { get; set; }
        public string ERP_Location_To { get; set; }

        public string Detail { get; set; }
        public Guid Binbalance_index { get; set; }
        public decimal BinBalance_QtyBal { get; set; }
        public decimal BinBalance_QtyReserve { get; set; }
        public decimal GI_TotalQty { get; set; }
        public decimal diff { get; set; }

        public Guid GoodsIssue_Index { get; set; }
        public Guid GoodsIssueItemLocation_Index { get; set; }

        public List<checktaggimodel> checktaggimodel { get; set; }
        public List<checktagtfmodel> checktagtfmodel { get; set; }
        public List<fixwavemodel> fixwavemodel { get; set; }
        public List<checktagbygimodel> checktagbygimodel { get; set; }
    }

    public class checktaggimodel
    {
        public string Tag_No { get; set; }
        public string GoodsIssue_No { get; set; }
        public string Location_Name { get; set; }
        public decimal GITotalQty { get; set; }
    }

    public class checktagtfmodel
    {
        public int Document_Status { get; set; }
        public int Item_Document_Status { get; set; }
        public string GoodsTransfer_No { get; set; }
        public string DocumentType_Name { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Create_By { get; set; }
        public string Tag_No { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal Qty { get; set; }
        public decimal TotalQty { get; set; }
        public string ProductConversion_Name { get; set; }
        public string Location_Name { get; set; }
        public string Location_Name_To { get; set; }
        public string ItemStatus_Name { get; set; }
        public string ItemStatus_Name_To { get; set; }
        public string ERP_Location { get; set; }
        public string ERP_Location_To { get; set; }
    }

    public class checktagbygimodel
    {
        public string Tag_No { get; set; }
        public string GoodsIssue_No { get; set; }
        public Guid GoodsIssue_Index { get; set; }
        public Guid GoodsIssueItemLocation_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal TotalQty { get; set; }
        public string ProductConversion_Name { get; set; }
    }
}

