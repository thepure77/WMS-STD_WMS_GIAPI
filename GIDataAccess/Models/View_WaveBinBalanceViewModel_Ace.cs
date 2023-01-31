using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_WaveBinBalanceViewModel_Ace
    {
        [Key]
        public Guid Row_Index { get; set; }
        public string binBalance_Index { get; set; }
        public string owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public string goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public DateTime? goodsReceive_Date { get; set; }
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
}
