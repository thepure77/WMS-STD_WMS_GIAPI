﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BomBusiness
{

    public partial class BomItemViewModel
    {
        public Guid bomItem_Index { get; set; }
        public Guid bom_Index { get; set; }
        public string bom_No { get; set; }
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
        public decimal? ratio { get; set; }
        public decimal? totalQty { get; set; }
        public Guid? productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public string mfg_Date { get; set; }
        public string exp_Date { get; set; }
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
        public decimal? totalQtyBackOrder { get; set; }
        public int? backOrderStatus { get; set; }
        public string imageUri { get; set; }
        public decimal? unitWeight { get; set; }
        public decimal? weight { get; set; }
        public decimal? netWeight { get; set; }
        public Guid? weight_Index { get; set; }
        public string weight_Id { get; set; }
        public string weight_Name { get; set; }
        public decimal? weightRatio { get; set; }
        public decimal? unitGrsWeight { get; set; }
        public decimal? grsWeight { get; set; }
        public Guid? grsWeight_Index { get; set; }
        public string grsWeight_Id { get; set; }
        public string grsWeight_Name { get; set; }
        public decimal? grsWeightRatio { get; set; }
        public decimal? unitWidth { get; set; }
        public decimal? width { get; set; }
        public Guid? width_Index { get; set; }
        public string width_Id { get; set; }
        public string width_Name { get; set; }
        public decimal? widthRatio { get; set; }
        public decimal? unitLength { get; set; }
        public decimal? length { get; set; }
        public Guid? length_Index { get; set; }
        public string length_Id { get; set; }
        public string length_Name { get; set; }
        public decimal? lengthRatio { get; set; }
        public decimal? unitHeight { get; set; }
        public decimal? height { get; set; }
        public Guid? height_Index { get; set; }
        public string height_Id { get; set; }
        public string height_Name { get; set; }
        public decimal? heightRatio { get; set; }
        public decimal? unitVolume { get; set; }
        public decimal? volume { get; set; }
        public int? runWave_Status { get; set; }
        public string create_By { get; set; }
        public DateTime? create_Date { get; set; }
        public string update_By { get; set; }
        public DateTime? update_Date { get; set; }
        public string cancel_By { get; set; }
        public DateTime? cancel_Date { get; set; }



        public decimal? qtyPlan { get; set; }
        public bool isDelete { get; set; }
        public string ref_DocumentItem_Index { get; set; }
        public string ref_Document_No { get; set; }
    }
}
