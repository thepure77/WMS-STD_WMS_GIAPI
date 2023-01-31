using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDataBusiness.ViewModels
{
    public  class ProductBOMItemViewModel
    {
        public Guid productBOMItem_Index { get; set; }
        public Guid productBOM_Index { get; set; }
        public string productBOM_No { get; set; }
        public Guid owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public Guid product_Index { get; set; }
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
        public Guid productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public DateTime? mFG_Date { get; set; }
        public DateTime? eXP_Date { get; set; }
        public decimal? unitWeight { get; set; }
        public Guid? unitWeight_Index { get; set; }
        public string unitWeight_Id { get; set; }
        public string unitWeight_Name { get; set; }
        public decimal? unitWeightRatio { get; set; }
        public decimal? weight { get; set; }
        public Guid? weight_Index { get; set; }
        public string weight_Id { get; set; }
        public string weight_Name { get; set; }
        public decimal? weightRatio { get; set; }
        public decimal? unitNetWeight { get; set; }
        public Guid? unitNetWeight_Index { get; set; }
        public string unitNetWeight_Id { get; set; }
        public string unitNetWeight_Name { get; set; }
        public decimal? unitNetWeightRatio { get; set; }
        public decimal? netWeight { get; set; }
        public Guid? netWeight_Index { get; set; }
        public string netWeight_Id { get; set; }
        public string netWeight_Name { get; set; }
        public decimal? netWeightRatio { get; set; }
        public decimal? unitGrsWeight { get; set; }
        public Guid? unitGrsWeight_Index { get; set; }
        public string unitGrsWeight_Id { get; set; }
        public string unitGrsWeight_Name { get; set; }
        public decimal? unitGrsWeightRatio { get; set; }
        public decimal? grsWeight { get; set; }
        public Guid? grsWeight_Index { get; set; }
        public string grsWeight_Id { get; set; }
        public string grsWeight_Name { get; set; }
        public decimal? grsWeightRatio { get; set; }
        public decimal? unitWidth { get; set; }
        public decimal? width { get; set; }
        public decimal? unitLength { get; set; }
        public decimal? length { get; set; }
        public decimal? unitHeight { get; set; }
        public decimal? height { get; set; }
        public decimal? unitVolume { get; set; }
        public decimal? volume { get; set; }
        public decimal? volumeRatio { get; set; }
        public Guid? volume_Index { get; set; }
        public string volume_Id { get; set; }
        public string volume_Name { get; set; }
        public decimal? unitPrice { get; set; }
        public Guid? unitPrice_Index { get; set; }
        public string unitPrice_Id { get; set; }
        public string unitPrice_Name { get; set; }
        public decimal? price { get; set; }
        public Guid? price_Index { get; set; }
        public string price_Id { get; set; }
        public string price_Name { get; set; }
        public string remark { get; set; }
        public int? document_Status { get; set; }
        public string ref_No1 { get; set; }
        public string ref_No2 { get; set; }
        public string ref_No3 { get; set; }
        public string ref_No4 { get; set; }
        public string ref_No5 { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public int isActive { get; set; }
        public int isDelete { get; set; }
        public int isSystem { get; set; }
        public int status_Id { get; set; }
        public string create_By { get; set; }
        public DateTime create_Date { get; set; }
        public string update_By { get; set; }
        public DateTime? update_Date { get; set; }
        public string cancel_By { get; set; }
        public DateTime? cancel_Date { get; set; }
        public string productBOM_Size { get; set; }
        public string imageUri { get; set; }

    }
}
