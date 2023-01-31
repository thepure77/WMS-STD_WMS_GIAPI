using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{

    public partial class wm_BinCardReserve
    {
        [Key]
        public Guid BinCardReserve_Index { get; set; }

        public Guid? BinBalance_Index { get; set; }

        public Guid? Process_Index { get; set; }

        public Guid? GoodsReceive_Index { get; set; }

        [StringLength(50)]
        public string GoodsReceive_No { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? GoodsReceive_Date { get; set; }

        public Guid? GoodsReceiveItem_Index { get; set; }

        public Guid? TagItem_Index { get; set; }

        public Guid? Tag_Index { get; set; }

        [StringLength(50)]
        public string Tag_No { get; set; }

        public Guid? Product_Index { get; set; }

        [StringLength(50)]
        public string Product_Id { get; set; }

        [StringLength(200)]
        public string Product_Name { get; set; }

        [StringLength(200)]
        public string Product_SecondName { get; set; }

        [StringLength(200)]
        public string Product_ThirdName { get; set; }

        [StringLength(50)]
        public string Product_Lot { get; set; }

        public Guid? ItemStatus_Index { get; set; }

        [StringLength(50)]
        public string ItemStatus_Id { get; set; }

        [StringLength(200)]
        public string ItemStatus_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MFG_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EXP_Date { get; set; }

        public Guid? ProductConversion_Index { get; set; }

        [StringLength(50)]
        public string ProductConversion_Id { get; set; }

        [StringLength(200)]
        public string ProductConversion_Name { get; set; }

        public Guid? Owner_Index { get; set; }

        [StringLength(50)]
        public string Owner_Id { get; set; }

        [StringLength(50)]
        public string Owner_Name { get; set; }

        public Guid? Location_Index { get; set; }

        [StringLength(50)]
        public string Location_Id { get; set; }

        [StringLength(200)]
        public string Location_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_QtyBal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitWeightBal { get; set; }

        public Guid? BinCardReserve_UnitWeightBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitWeightBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitWeightBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitWeightBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_WeightBal { get; set; }

        public Guid? BinCardReserve_WeightBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_WeightBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_WeightBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_WeightBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitNetWeightBal { get; set; }

        public Guid? BinCardReserve_UnitNetWeightBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitNetWeightBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitNetWeightBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitNetWeightBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_NetWeightBal { get; set; }

        public Guid? BinCardReserve_NetWeightBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_NetWeightBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_NetWeightBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_NetWeightBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitGrsWeightBal { get; set; }

        public Guid? BinCardReserve_UnitGrsWeightBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitGrsWeightBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitGrsWeightBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitGrsWeightBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_GrsWeightBal { get; set; }

        public Guid? BinCardReserve_GrsWeightBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_GrsWeightBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_GrsWeightBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_GrsWeightBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitWidthBal { get; set; }

        public Guid? BinCardReserve_UnitWidthBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitWidthBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitWidthBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitWidthBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_WidthBal { get; set; }

        public Guid? BinCardReserve_WidthBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_WidthBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_WidthBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_WidthBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitLengthBal { get; set; }

        public Guid? BinCardReserve_UnitLengthBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitLengthBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitLengthBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitLengthBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_LengthBal { get; set; }

        public Guid? BinCardReserve_LengthBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_LengthBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_LengthBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_LengthBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitHeightBal { get; set; }

        public Guid? BinCardReserve_UnitHeightBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitHeightBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_UnitHeightBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitHeightBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_HeightBal { get; set; }

        public Guid? BinCardReserve_HeightBal_Index { get; set; }

        [StringLength(200)]
        public string BinCardReserve_HeightBal_Id { get; set; }

        [StringLength(200)]
        public string BinCardReserve_HeightBal_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_HeightBalRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_UnitVolumeBal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BinCardReserve_VolumeBal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitPrice { get; set; }

        public Guid? UnitPrice_Index { get; set; }

        [StringLength(200)]
        public string UnitPrice_Id { get; set; }

        [StringLength(200)]
        public string UnitPrice_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Price { get; set; }

        public Guid? Price_Index { get; set; }

        [StringLength(200)]
        public string Price_Id { get; set; }

        [StringLength(200)]
        public string Price_Name { get; set; }

        [StringLength(200)]
        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        [StringLength(200)]
        public string Ref_Wave_Index { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Cancel_Date { get; set; }

        public int? BinCardReserve_Status { get; set; }
        public string ERP_Location { get; set; }
    }
}
