using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_Taskitem_with_Truckload_PICKQTY
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid TaskItem_Index { get; set; }

        public Guid Task_Index { get; set; }

        public string Task_No { get; set; }

        public string TruckLoad_No { get; set; }

        [StringLength(50)]
        public string LineNum { get; set; }

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

        public Guid? Location_Index { get; set; }

        [StringLength(50)]
        public string Location_Id { get; set; }

        [StringLength(200)]
        public string Location_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalQty { get; set; }

        public Guid? ProductConversion_Index { get; set; }

        [StringLength(50)]
        public string ProductConversion_Id { get; set; }

        [StringLength(200)]
        public string ProductConversion_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MFG_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EXP_Date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitWeight { get; set; }

        public Guid? UnitWeight_Index { get; set; }

        [StringLength(200)]
        public string UnitWeight_Id { get; set; }

        [StringLength(200)]
        public string UnitWeight_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitWeightRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }

        public Guid? Weight_Index { get; set; }

        [StringLength(200)]
        public string Weight_Id { get; set; }

        [StringLength(200)]
        public string Weight_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WeightRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitNetWeight { get; set; }

        public Guid? UnitNetWeight_Index { get; set; }

        [StringLength(200)]
        public string UnitNetWeight_Id { get; set; }

        [StringLength(200)]
        public string UnitNetWeight_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitNetWeightRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NetWeight { get; set; }

        public Guid? NetWeight_Index { get; set; }

        [StringLength(200)]
        public string NetWeight_Id { get; set; }

        [StringLength(200)]
        public string NetWeight_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NetWeightRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitGrsWeight { get; set; }

        public Guid? UnitGrsWeight_Index { get; set; }

        [StringLength(200)]
        public string UnitGrsWeight_Id { get; set; }

        [StringLength(200)]
        public string UnitGrsWeight_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitGrsWeightRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GrsWeight { get; set; }

        public Guid? GrsWeight_Index { get; set; }

        [StringLength(200)]
        public string GrsWeight_Id { get; set; }

        [StringLength(200)]
        public string GrsWeight_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GrsWeightRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitWidth { get; set; }

        public Guid? UnitWidth_Index { get; set; }

        [StringLength(200)]
        public string UnitWidth_Id { get; set; }

        [StringLength(200)]
        public string UnitWidth_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitWidthRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Width { get; set; }

        public Guid? Width_Index { get; set; }

        [StringLength(200)]
        public string Width_Id { get; set; }

        [StringLength(200)]
        public string Width_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WidthRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitLength { get; set; }

        public Guid? UnitLength_Index { get; set; }

        [StringLength(200)]
        public string UnitLength_Id { get; set; }

        [StringLength(200)]
        public string UnitLength_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitLengthRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Length { get; set; }

        public Guid? Length_Index { get; set; }

        [StringLength(200)]
        public string Length_Id { get; set; }

        [StringLength(200)]
        public string Length_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LengthRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitHeight { get; set; }

        public Guid? UnitHeight_Index { get; set; }

        [StringLength(200)]
        public string UnitHeight_Id { get; set; }

        [StringLength(200)]
        public string UnitHeight_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitHeightRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Height { get; set; }

        public Guid? Height_Index { get; set; }

        [StringLength(200)]
        public string Height_Id { get; set; }

        [StringLength(200)]
        public string Height_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HeightRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitVolume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Volume { get; set; }

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
        public string DocumentRef_No1 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No2 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No3 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No4 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No5 { get; set; }

        public int? Document_Status { get; set; }

        [StringLength(200)]
        public string UDF_1 { get; set; }

        [StringLength(200)]
        public string UDF_2 { get; set; }

        [StringLength(200)]
        public string UDF_3 { get; set; }

        [StringLength(200)]
        public string UDF_4 { get; set; }

        [StringLength(200)]
        public string UDF_5 { get; set; }

        public Guid? Ref_Process_Index { get; set; }

        [StringLength(200)]
        public string Ref_Document_No { get; set; }

        [StringLength(200)]
        public string Ref_Document_LineNum { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        public Guid? ReasonCode_Index { get; set; }

        [StringLength(50)]
        public string ReasonCode_Id { get; set; }

        [StringLength(200)]
        public string ReasonCode_Name { get; set; }

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

        public Guid? TagOutPick_Index { get; set; }

        [StringLength(50)]
        public string TagOutPick_No { get; set; }
        
        public decimal? splitQty { get; set; }
        public Guid? PlanGoodsIssue_Index { get; set; }

        public Guid? PlanGoodsIssueItem_Index { get; set; }

        [StringLength(200)]
        public string PlanGoodsIssue_No { get; set; }

        public Guid? Pick_ProductConversion_Index { get; set; }

        [StringLength(50)]
        public string Pick_ProductConversion_Id { get; set; }

        [StringLength(200)]
        public string Pick_ProductConversion_Name { get; set; }

        [StringLength(200)]
        public string ProductConversionBarcode { get; set; }

        public Guid? TagOut_Index { get; set; }

        [StringLength(200)]
        public string TagOut_No { get; set; }

        [StringLength(400)]
        public string ImageUri { get; set; }

        public Guid? BinBalance_Index { get; set; }

        public Guid? BinBalance_Index_New { get; set; }

        [StringLength(200)]
        public string Invoice_No { get; set; }
        [StringLength(200)]
        public string Invoice_No_Out { get; set; }

        [StringLength(200)]
        public string Declaration_No { get; set; }
        [StringLength(200)]
        public string Declaration_No_Out { get; set; }
        [StringLength(200)]
        public string HS_Code { get; set; }

        [StringLength(200)]
        public string Conutry_of_Origin { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Tax1 { get; set; }

        public Guid? Tax1_Currency_Index { get; set; }

        [StringLength(200)]
        public string Tax1_Currency_Id { get; set; }

        [StringLength(200)]
        public string Tax1_Currency_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Tax2 { get; set; }

        public Guid? Tax2_Currency_Index { get; set; }

        [StringLength(200)]
        public string Tax2_Currency_Id { get; set; }

        [StringLength(200)]
        public string Tax2_Currency_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Tax3 { get; set; }

        public Guid? Tax3_Currency_Index { get; set; }

        [StringLength(200)]
        public string Tax3_Currency_Id { get; set; }

        [StringLength(200)]
        public string Tax3_Currency_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Tax4 { get; set; }

        public Guid? Tax4_Currency_Index { get; set; }

        [StringLength(200)]
        public string Tax4_Currency_Id { get; set; }

        [StringLength(200)]
        public string Tax4_Currency_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Tax5 { get; set; }

        public Guid? Tax5_Currency_Index { get; set; }

        [StringLength(200)]
        public string Tax5_Currency_Id { get; set; }

        [StringLength(200)]
        public string Tax5_Currency_Name { get; set; }
        public string ERP_Location { get; set; }


        //Picking
        public decimal? Picking_Qty { get; set; }
        public decimal? Picking_Ratio { get; set; }
        public decimal? Picking_TotalQty { get; set; }
        public string Picking_By { get; set; }
        public DateTime? Picking_Date { get; set; }
        public int? Picking_Status { get; set; }
        public Guid? Picking_Location_Index { get; set; }
        public string Picking_Location_Id { get; set; }
        public string Picking_Location_Name { get; set; }

        //PickingLabel
        public decimal? PickingLabel_Qty { get; set; }
        public decimal? PickingLabel_Ratio { get; set; }
        public decimal? PickingLabel_TotalQty { get; set; }
        public string PickingLabeling_By { get; set; }
        public DateTime? PickingLabeling_Date { get; set; }
        public int? PickingLabeling_Status { get; set; }
        public Guid? PickingLabeling_Location_Index { get; set; }
        public string PickingLabeling_Location_Id { get; set; }
        public string PickingLabeling_Location_Name { get; set; }

        //PickingPickQty
        public decimal? PickingPickQty_Qty { get; set; }
        public decimal? PickingPickQty_Ratio { get; set; }
        public decimal? PickingPickQty_TotalQty { get; set; }
        public string PickingPickQty_By { get; set; }
        public int? PickingPickQty_Status { get; set; }
        public DateTime? PickingPickQty_Date { get; set; }
        public Guid? PickingPickQty_Location_Index { get; set; }
        public string PickingPickQty_Location_Id { get; set; }
        public string PickingPickQty_Location_Name { get; set; }

        //PickingToDock
        public decimal? PickingToDock_Qty { get; set; }
        public decimal? PickingToDock_Ratio { get; set; }
        public decimal? PickingToDock_TotalQty { get; set; }
        public string PickingToDock_By { get; set; }
        public int? PickingToDock_Status { get; set; }
        public DateTime? PickingToDock_Date { get; set; }
        public Guid? PickingToDock_Location_Index { get; set; }
        
        public string PickingToDock_Location_Id { get; set; }
        public string PickingToDock_Location_Name { get; set; }

        //PickingToStg
        public string PickingToStg_By { get; set; }
        public Guid? Dock_Index { get; set; }
        public string Dock_Name { get; set; }
        public string Dock_Id { get; set; }
        public DateTime? PickingToStg_Date { get; set; }
        public  int? PickingToStg_Status { get; set; }
        public  int? Document_StatusTracking { get; set; }

        public Guid? TruckLoad_Index { get; set; }
    }
}
