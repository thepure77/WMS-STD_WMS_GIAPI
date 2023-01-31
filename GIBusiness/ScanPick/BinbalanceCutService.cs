using Business.Library;
using Comone.Utils;
using DataAccess;
using GIDataAccess.Models;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GIBusiness.GoodIssue
{
    public class BinbalanceCutService
    {
        private GIDbContext db;
        private MasterDbContext dbMaster;
        private BinbalanceDbContext dbBinbalance;

        public BinbalanceCutService()
        {
            db = new GIDbContext();
            dbBinbalance = new BinbalanceDbContext();
        }
        public BinbalanceCutService(GIDbContext db, MasterDbContext dbMaster , BinbalanceDbContext dbBinbalance)
        {
            this.db = db;
            this.dbMaster = dbMaster;
            this.dbBinbalance = dbBinbalance;
        }

        public string InsertBinCardBy_Scanpick(BinCardViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(360);
                string guid = "";
                var BinData = new wm_BinBalance();
                var BinBalance = new wm_BinBalance();
                bool IsInsert = false;

                if (!string.IsNullOrEmpty(model.binBalance_Index.ToString()))
                {
                    BinData = dbBinbalance.wm_BinBalance.Find(model.binBalance_Index);
                }
                else
                {
                    BinData = dbBinbalance.wm_BinBalance.FirstOrDefault(c => c.BinBalance_Index == dbBinbalance.wm_BinCardReserve.FirstOrDefault(w => w.Ref_DocumentItem_Index == model.ref_DocumentItem_Index && w.Ref_Document_Index == model.ref_Document_Index).BinBalance_Index);
                }

                guid = BinData.BinBalance_Index.ToString();
                var listBinCard = new List<wm_BinCard>();

                //out
                {
                    var BinCard = new wm_BinCard();
                    BinCard.BinCard_Index = Guid.NewGuid();
                    BinCard.Process_Index = model.process_Index;
                    BinCard.DocumentType_Index = model.documentType_Index;
                    BinCard.DocumentType_Id = model.documentType_Id;
                    BinCard.DocumentType_Name = model.documentType_Name;
                    BinCard.GoodsReceive_Index = BinData.GoodsReceive_Index;
                    BinCard.GoodsReceiveItem_Index = BinData.GoodsReceiveItem_Index;
                    BinCard.GoodsReceiveItemLocation_Index = BinData.GoodsReceiveItemLocation_Index;
                    BinCard.BinCard_No = model.ref_Document_No;
                    BinCard.BinCard_Date = model.goodsIssue_Date;
                    BinCard.TagItem_Index = model.tagItem_Index;
                    BinCard.Tag_Index = BinData.Tag_Index;
                    BinCard.Tag_No = BinData.Tag_No;
                    BinCard.Tag_Index_To = model.tag_Index_To;
                    BinCard.Tag_No_To = model.tag_No_To;
                    BinCard.Product_Index = model.product_Index;
                    BinCard.Product_Id = model.product_Id;
                    BinCard.Product_Name = model.product_Name;
                    BinCard.Product_SecondName = model.product_SecondName;
                    BinCard.Product_ThirdName = model.product_ThirdName;
                    BinCard.Product_Index_To = model.product_Index; 
                    BinCard.Product_Id_To = model.product_Id;
                    BinCard.Product_Name_To = model.product_Name;
                    BinCard.Product_SecondName_To = model.product_SecondName;
                    BinCard.Product_ThirdName_To = model.product_ThirdName;
                    BinCard.Product_Lot = model.product_Lot;
                    BinCard.Product_Lot_To = model.product_Lot;
                    BinCard.ItemStatus_Index = model.itemStatus_Index;
                    BinCard.ItemStatus_Id = model.itemStatus_Id;
                    BinCard.ItemStatus_Name = model.itemStatus_Name;
                    BinCard.ItemStatus_Index_To = model.itemStatus_Index_To;
                    BinCard.ItemStatus_Id_To = model.itemStatus_Id_To;
                    BinCard.ItemStatus_Name_To = model.itemStatus_Name_To;

                    BinCard.ProductConversion_Index = model.productConversion_Index;
                    BinCard.ProductConversion_Id = model.productConversion_Id;
                    BinCard.ProductConversion_Name = model.productConversion_Name;

                    BinCard.Owner_Index = model.owner_Index;
                    BinCard.Owner_Id = model.owner_Id;
                    BinCard.Owner_Name = model.owner_Name;

                    BinCard.Owner_Index_To = model.owner_Index;
                    BinCard.Owner_Id_To = model.owner_Id;
                    BinCard.Owner_Name_To = model.owner_Name;

                    BinCard.Location_Index = model.location_Index;
                    BinCard.Location_Id = model.location_Id;
                    BinCard.Location_Name = model.location_Name;

                    BinCard.Location_Index_To = model.location_Index_To;
                    BinCard.Location_Id_To = model.location_Id_To;
                    BinCard.Location_Name_To = model.location_Name_To;


                    BinCard.GoodsReceive_EXP_Date = model.exp_Date;
                    BinCard.GoodsReceive_EXP_Date_To = model.exp_Date;

                    BinCard.BinCard_QtyIn = 0;
                    BinCard.BinCard_QtyOut = model.picking_TotalQty;
                    BinCard.BinCard_QtySign = model.picking_TotalQty * -1;

                    //Out
                    BinCard.BinCard_UnitWeightOut = BinData.BinBalance_UnitWeightBal;
                    BinCard.BinCard_UnitWeightOut_Index = BinData.BinBalance_UnitWeightBal_Index;
                    BinCard.BinCard_UnitWeightOut_Id = BinData.BinBalance_UnitWeightBal_Id;
                    BinCard.BinCard_UnitWeightOut_Name = BinData.BinBalance_UnitWeightBal_Name;
                    BinCard.BinCard_UnitWeightOutRatio = BinData.BinBalance_UnitWeightBalRatio;

                    BinCard.BinCard_WeightOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0);
                    BinCard.BinCard_WeightOut_Index = BinData.BinBalance_WeightBal_Index;
                    BinCard.BinCard_WeightOut_Id = BinData.BinBalance_WeightBal_Id;
                    BinCard.BinCard_WeightOut_Name = BinData.BinBalance_WeightBal_Name;
                    BinCard.BinCard_WeightOutRatio = BinData.BinBalance_WeightBalRatio;

                    BinCard.BinCard_UnitNetWeightOut = BinData.BinBalance_UnitNetWeightBal;
                    BinCard.BinCard_UnitNetWeightOut_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                    BinCard.BinCard_UnitNetWeightOut_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                    BinCard.BinCard_UnitNetWeightOut_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                    BinCard.BinCard_UnitNetWeightOutRatio = BinData.BinBalance_UnitNetWeightBalRatio;

                    BinCard.BinCard_NetWeightOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0);
                    BinCard.BinCard_NetWeightOut_Index = BinData.BinBalance_NetWeightBal_Index;
                    BinCard.BinCard_NetWeightOut_Id = BinData.BinBalance_NetWeightBal_Id;
                    BinCard.BinCard_NetWeightOut_Name = BinData.BinBalance_NetWeightBal_Name;
                    BinCard.BinCard_NetWeightOutRatio = BinData.BinBalance_NetWeightBalRatio;

                    BinCard.BinCard_UnitGrsWeightOut = BinData.BinBalance_UnitGrsWeightBal;
                    BinCard.BinCard_UnitGrsWeightOut_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                    BinCard.BinCard_UnitGrsWeightOut_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                    BinCard.BinCard_UnitGrsWeightOut_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                    BinCard.BinCard_UnitGrsWeightOutRatio = BinData.BinBalance_UnitGrsWeightBalRatio;

                    BinCard.BinCard_GrsWeightOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0);
                    BinCard.BinCard_GrsWeightOut_Index = BinData.BinBalance_GrsWeightBal_Index;
                    BinCard.BinCard_GrsWeightOut_Id = BinData.BinBalance_GrsWeightBal_Id;
                    BinCard.BinCard_GrsWeightOut_Name = BinData.BinBalance_GrsWeightBal_Name;
                    BinCard.BinCard_GrsWeightOutRatio = BinData.BinBalance_GrsWeightBalRatio;

                    BinCard.BinCard_UnitWidthOut = (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_UnitWidthOut_Index = BinData.BinBalance_UnitWidthBal_Index;
                    BinCard.BinCard_UnitWidthOut_Id = BinData.BinBalance_UnitWidthBal_Id;
                    BinCard.BinCard_UnitWidthOut_Name = BinData.BinBalance_UnitWidthBal_Name;
                    BinCard.BinCard_UnitWidthOutRatio = BinData.BinBalance_UnitWidthBalRatio;

                    BinCard.BinCard_WidthOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_WidthOut_Index = BinData.BinBalance_WidthBal_Index;
                    BinCard.BinCard_WidthOut_Id = BinData.BinBalance_WidthBal_Id;
                    BinCard.BinCard_WidthOut_Name = BinData.BinBalance_WidthBal_Name;
                    BinCard.BinCard_WidthOutRatio = BinData.BinBalance_WidthBalRatio;

                    BinCard.BinCard_UnitLengthOut = (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_UnitLengthOut_Index = BinData.BinBalance_UnitLengthBal_Index;
                    BinCard.BinCard_UnitLengthOut_Id = BinData.BinBalance_UnitLengthBal_Id;
                    BinCard.BinCard_UnitLengthOut_Name = BinData.BinBalance_UnitLengthBal_Name;
                    BinCard.BinCard_UnitLengthOutRatio = BinData.BinBalance_UnitLengthBalRatio;

                    BinCard.BinCard_LengthOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_LengthOut_Index = BinData.BinBalance_LengthBal_Index;
                    BinCard.BinCard_LengthOut_Id = BinData.BinBalance_LengthBal_Id;
                    BinCard.BinCard_LengthOut_Name = BinData.BinBalance_LengthBal_Name;
                    BinCard.BinCard_LengthOutRatio = BinData.BinBalance_LengthBalRatio;

                    BinCard.BinCard_UnitHeightOut = (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_UnitHeightOut_Index = BinData.BinBalance_UnitHeightBal_Index;
                    BinCard.BinCard_UnitHeightOut_Id = BinData.BinBalance_UnitHeightBal_Id;
                    BinCard.BinCard_UnitHeightOut_Name = BinData.BinBalance_UnitHeightBal_Name;
                    BinCard.BinCard_UnitHeightOutRatio = BinData.BinBalance_UnitHeightBalRatio;

                    BinCard.BinCard_HeightOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_HeightOut_Index = BinData.BinBalance_HeightBal_Index;
                    BinCard.BinCard_HeightOut_Id = BinData.BinBalance_HeightBal_Id;
                    BinCard.BinCard_HeightOut_Name = BinData.BinBalance_HeightBal_Name;
                    BinCard.BinCard_HeightOutRatio = BinData.BinBalance_HeightBalRatio;

                    BinCard.BinCard_UnitVolumeOut = (BinData.BinBalance_UnitVolumeBal ?? 0);
                    BinCard.BinCard_VolumeOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0);

                    BinCard.BinCard_UnitPriceOut = BinData.UnitPrice;
                    BinCard.BinCard_UnitPriceOut_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_UnitPriceOut_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_UnitPriceOut_Name = BinData.UnitPrice_Name;
                    BinCard.BinCard_PriceOut = (model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0);
                    BinCard.BinCard_PriceOut_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_PriceOut_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_PriceOut_Name = BinData.UnitPrice_Name;

                    //Sign
                    BinCard.BinCard_UnitWeightSign = BinData.BinBalance_UnitWeightBal;
                    BinCard.BinCard_UnitWeightSign_Index = BinData.BinBalance_UnitWeightBal_Index;
                    BinCard.BinCard_UnitWeightSign_Id = BinData.BinBalance_UnitWeightBal_Id;
                    BinCard.BinCard_UnitWeightSign_Name = BinData.BinBalance_UnitWeightBal_Name;
                    BinCard.BinCard_UnitWeightSignRatio = BinData.BinBalance_UnitWeightBalRatio;

                    BinCard.BinCard_WeightSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0)) * -1);
                    BinCard.BinCard_WeightSign_Index = BinData.BinBalance_WeightBal_Index;
                    BinCard.BinCard_WeightSign_Id = BinData.BinBalance_WeightBal_Id;
                    BinCard.BinCard_WeightSign_Name = BinData.BinBalance_WeightBal_Name;
                    BinCard.BinCard_WeightSignRatio = BinData.BinBalance_WeightBalRatio;

                    BinCard.BinCard_UnitNetWeightSign = BinData.BinBalance_UnitNetWeightBal;
                    BinCard.BinCard_UnitNetWeightSign_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                    BinCard.BinCard_UnitNetWeightSign_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                    BinCard.BinCard_UnitNetWeightSign_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                    BinCard.BinCard_UnitNetWeightSignRatio = BinData.BinBalance_UnitNetWeightBalRatio;

                    BinCard.BinCard_NetWeightSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0)) * -1);
                    BinCard.BinCard_NetWeightSign_Index = BinData.BinBalance_NetWeightBal_Index;
                    BinCard.BinCard_NetWeightSign_Id = BinData.BinBalance_NetWeightBal_Id;
                    BinCard.BinCard_NetWeightSign_Name = BinData.BinBalance_NetWeightBal_Name;
                    BinCard.BinCard_NetWeightSignRatio = BinData.BinBalance_NetWeightBalRatio;

                    BinCard.BinCard_UnitGrsWeightSign = BinData.BinBalance_UnitGrsWeightBal;
                    BinCard.BinCard_UnitGrsWeightSign_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                    BinCard.BinCard_UnitGrsWeightSign_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                    BinCard.BinCard_UnitGrsWeightSign_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                    BinCard.BinCard_UnitGrsWeightSignRatio = BinData.BinBalance_UnitGrsWeightBalRatio;

                    BinCard.BinCard_GrsWeightSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0)) * -1);
                    BinCard.BinCard_GrsWeightSign_Index = BinData.BinBalance_GrsWeightBal_Index;
                    BinCard.BinCard_GrsWeightSign_Id = BinData.BinBalance_GrsWeightBal_Id;
                    BinCard.BinCard_GrsWeightSign_Name = BinData.BinBalance_GrsWeightBal_Name;
                    BinCard.BinCard_GrsWeightSignRatio = BinData.BinBalance_GrsWeightBalRatio;

                    BinCard.BinCard_UnitWidthSign = (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_UnitWidthSign_Index = BinData.BinBalance_UnitWidthBal_Index;
                    BinCard.BinCard_UnitWidthSign_Id = BinData.BinBalance_UnitWidthBal_Id;
                    BinCard.BinCard_UnitWidthSign_Name = BinData.BinBalance_UnitWidthBal_Name;
                    BinCard.BinCard_UnitWidthSignRatio = BinData.BinBalance_UnitWidthBalRatio;

                    BinCard.BinCard_WidthSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0)) * -1);
                    BinCard.BinCard_WidthSign_Index = BinData.BinBalance_WidthBal_Index;
                    BinCard.BinCard_WidthSign_Id = BinData.BinBalance_WidthBal_Id;
                    BinCard.BinCard_WidthSign_Name = BinData.BinBalance_WidthBal_Name;
                    BinCard.BinCard_WidthSignRatio = BinData.BinBalance_WidthBalRatio;

                    BinCard.BinCard_UnitLengthSign = (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_UnitLengthSign_Index = BinData.BinBalance_UnitLengthBal_Index;
                    BinCard.BinCard_UnitLengthSign_Id = BinData.BinBalance_UnitLengthBal_Id;
                    BinCard.BinCard_UnitLengthSign_Name = BinData.BinBalance_UnitLengthBal_Name;
                    BinCard.BinCard_UnitLengthSignRatio = BinData.BinBalance_UnitLengthBalRatio;

                    BinCard.BinCard_LengthSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0)) * -1);
                    BinCard.BinCard_LengthSign_Index = BinData.BinBalance_LengthBal_Index;
                    BinCard.BinCard_LengthSign_Id = BinData.BinBalance_LengthBal_Id;
                    BinCard.BinCard_LengthSign_Name = BinData.BinBalance_LengthBal_Name;
                    BinCard.BinCard_LengthSignRatio = BinData.BinBalance_LengthBalRatio;

                    BinCard.BinCard_UnitHeightSign = (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_UnitHeightSign_Index = BinData.BinBalance_UnitHeightBal_Index;
                    BinCard.BinCard_UnitHeightSign_Id = BinData.BinBalance_UnitHeightBal_Id;
                    BinCard.BinCard_UnitHeightSign_Name = BinData.BinBalance_UnitHeightBal_Name;
                    BinCard.BinCard_UnitHeightSignRatio = BinData.BinBalance_UnitHeightBalRatio;

                    BinCard.BinCard_HeightSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0)) * -1);
                    BinCard.BinCard_HeightSign_Index = BinData.BinBalance_HeightBal_Index;
                    BinCard.BinCard_HeightSign_Id = BinData.BinBalance_HeightBal_Id;
                    BinCard.BinCard_HeightSign_Name = BinData.BinBalance_HeightBal_Name;
                    BinCard.BinCard_HeightSignRatio = BinData.BinBalance_HeightBalRatio;

                    BinCard.BinCard_UnitVolumeSign = (BinData.BinBalance_UnitVolumeBal ?? 0);
                    BinCard.BinCard_VolumeSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0)) * -1);

                    BinCard.BinCard_UnitPriceSign = BinData.UnitPrice;
                    BinCard.BinCard_UnitPriceSign_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_UnitPriceSign_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_UnitPriceSign_Name = BinData.UnitPrice_Name;
                    BinCard.BinCard_PriceSign = (((model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0)) * -1);
                    BinCard.BinCard_PriceSign_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_PriceSign_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_PriceSign_Name = BinData.UnitPrice_Name;
                    BinCard.Ref_Document_No = model.ref_Document_No;
                    BinCard.Ref_Document_Index = model.ref_Document_Index; //tem.Ref_Document_Index;
                    BinCard.Ref_DocumentItem_Index = model.ref_DocumentItem_Index;
                    BinCard.TagOutItem_Index = model?.tagOutItem_Index.ToString();
                    BinCard.TagOut_Index = model?.tagOut_Index;
                    BinCard.TagOut_No = model.tagOut_No;
                    BinCard.Create_By = model.userName;
                    BinCard.Create_Date = DateTime.Now;
                    BinCard.BinCard_Date = DateTime.Now;
                    BinCard.BinBalance_Index = model.binBalance_Index;
                    listBinCard.Add(BinCard);

                }

                //in
                {
                    var BinCard = new wm_BinCard();
                    BinCard.BinCard_Index = Guid.NewGuid();
                    BinCard.Process_Index = model.process_Index;
                    BinCard.DocumentType_Index = model.documentType_Index;
                    BinCard.DocumentType_Id = model.documentType_Id;
                    BinCard.DocumentType_Name = model.documentType_Name;
                    BinCard.GoodsReceive_Index = BinData.GoodsReceive_Index;
                    BinCard.GoodsReceiveItem_Index = BinData.GoodsReceiveItem_Index;
                    BinCard.GoodsReceiveItemLocation_Index = BinData.GoodsReceiveItemLocation_Index;
                    BinCard.BinCard_No = model.ref_Document_No;
                    BinCard.BinCard_Date = model.goodsIssue_Date;
                    BinCard.TagItem_Index = model.tagItem_Index;
                    BinCard.Tag_Index = model.tag_Index_To;
                    BinCard.Tag_No = model.tag_No_To;
                    BinCard.Tag_Index_To = model.tag_Index_To;
                    BinCard.Tag_No_To = model.tag_No_To;
                    BinCard.Product_Index = model.product_Index;
                    BinCard.Product_Id = model.product_Id;
                    BinCard.Product_Name = model.product_Name;
                    BinCard.Product_SecondName = model.product_SecondName;
                    BinCard.Product_ThirdName = model.product_ThirdName;
                    BinCard.Product_Index_To = model.product_Index; //BinCardReserveItem.Product_Index_To;
                    BinCard.Product_Id_To = model.product_Id;
                    BinCard.Product_Name_To = model.product_Name;
                    BinCard.Product_SecondName_To = model.product_SecondName;
                    BinCard.Product_ThirdName_To = model.product_ThirdName;
                    BinCard.Product_Lot = model.product_Lot;
                    BinCard.Product_Lot_To = model.product_Lot;
                    BinCard.ItemStatus_Index = model.itemStatus_Index_To;
                    BinCard.ItemStatus_Id = model.itemStatus_Id_To;
                    BinCard.ItemStatus_Name = model.itemStatus_Name_To;


                    BinCard.ItemStatus_Index_To = model.itemStatus_Index_To;
                    BinCard.ItemStatus_Id_To = model.itemStatus_Id_To;
                    BinCard.ItemStatus_Name_To = model.itemStatus_Name_To;


                    BinCard.ProductConversion_Index = model.productConversion_Index;
                    BinCard.ProductConversion_Id = model.productConversion_Id;
                    BinCard.ProductConversion_Name = model.productConversion_Name;

                    BinCard.Owner_Index = model.owner_Index;
                    BinCard.Owner_Id = model.owner_Id;
                    BinCard.Owner_Name = model.owner_Name;

                    BinCard.Owner_Index_To = model.owner_Index;
                    BinCard.Owner_Id_To = model.owner_Id;
                    BinCard.Owner_Name_To = model.owner_Name;

                    BinCard.Location_Index = model.location_Index_To;//BinCardReserveItem.Location_Index;
                    BinCard.Location_Id = model.location_Id_To; //BinCardReserveItem.Location_Id;
                    BinCard.Location_Name = model.location_Name_To;//BinCardReserveItem.Location_Name;

                    BinCard.Location_Index_To = model.location_Index_To;
                    BinCard.Location_Id_To = model.location_Id_To;
                    BinCard.Location_Name_To = model.location_Name_To;


                    BinCard.GoodsReceive_EXP_Date = model.exp_Date;
                    BinCard.GoodsReceive_EXP_Date_To = model.exp_Date;
                    BinCard.BinCard_QtyIn = model.picking_TotalQty;
                    BinCard.BinCard_QtyOut = 0;
                    BinCard.BinCard_QtySign = model.picking_TotalQty;


                    //In
                    BinCard.BinCard_UnitWeightIn = BinData.BinBalance_UnitWeightBal;
                    BinCard.BinCard_UnitWeightIn_Index = BinData.BinBalance_UnitWeightBal_Index;
                    BinCard.BinCard_UnitWeightIn_Id = BinData.BinBalance_UnitWeightBal_Id;
                    BinCard.BinCard_UnitWeightIn_Name = BinData.BinBalance_UnitWeightBal_Name;
                    BinCard.BinCard_UnitWeightInRatio = BinData.BinBalance_UnitWeightBalRatio;

                    BinCard.BinCard_WeightIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0);
                    BinCard.BinCard_WeightIn_Index = BinData.BinBalance_WeightBal_Index;
                    BinCard.BinCard_WeightIn_Id = BinData.BinBalance_WeightBal_Id;
                    BinCard.BinCard_WeightIn_Name = BinData.BinBalance_WeightBal_Name;
                    BinCard.BinCard_WeightInRatio = BinData.BinBalance_WeightBalRatio;

                    BinCard.BinCard_UnitNetWeightIn = BinData.BinBalance_UnitNetWeightBal;
                    BinCard.BinCard_UnitNetWeightIn_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                    BinCard.BinCard_UnitNetWeightIn_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                    BinCard.BinCard_UnitNetWeightIn_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                    BinCard.BinCard_UnitNetWeightInRatio = BinData.BinBalance_UnitNetWeightBalRatio;

                    BinCard.BinCard_NetWeightIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0);
                    BinCard.BinCard_NetWeightIn_Index = BinData.BinBalance_NetWeightBal_Index;
                    BinCard.BinCard_NetWeightIn_Id = BinData.BinBalance_NetWeightBal_Id;
                    BinCard.BinCard_NetWeightIn_Name = BinData.BinBalance_NetWeightBal_Name;
                    BinCard.BinCard_NetWeightInRatio = BinData.BinBalance_NetWeightBalRatio;

                    BinCard.BinCard_UnitGrsWeightIn = BinData.BinBalance_UnitGrsWeightBal;
                    BinCard.BinCard_UnitGrsWeightIn_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                    BinCard.BinCard_UnitGrsWeightIn_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                    BinCard.BinCard_UnitGrsWeightIn_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                    BinCard.BinCard_UnitGrsWeightInRatio = BinData.BinBalance_UnitGrsWeightBalRatio;

                    BinCard.BinCard_GrsWeightIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0);
                    BinCard.BinCard_GrsWeightIn_Index = BinData.BinBalance_GrsWeightBal_Index;
                    BinCard.BinCard_GrsWeightIn_Id = BinData.BinBalance_GrsWeightBal_Id;
                    BinCard.BinCard_GrsWeightIn_Name = BinData.BinBalance_GrsWeightBal_Name;
                    BinCard.BinCard_GrsWeightInRatio = BinData.BinBalance_GrsWeightBalRatio;

                    BinCard.BinCard_UnitWidthIn = (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_UnitWidthIn_Index = BinData.BinBalance_UnitWidthBal_Index;
                    BinCard.BinCard_UnitWidthIn_Id = BinData.BinBalance_UnitWidthBal_Id;
                    BinCard.BinCard_UnitWidthIn_Name = BinData.BinBalance_UnitWidthBal_Name;
                    BinCard.BinCard_UnitWidthInRatio = BinData.BinBalance_UnitWidthBalRatio;

                    BinCard.BinCard_WidthIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_WidthIn_Index = BinData.BinBalance_WidthBal_Index;
                    BinCard.BinCard_WidthIn_Id = BinData.BinBalance_WidthBal_Id;
                    BinCard.BinCard_WidthIn_Name = BinData.BinBalance_WidthBal_Name;
                    BinCard.BinCard_WidthInRatio = BinData.BinBalance_WidthBalRatio;

                    BinCard.BinCard_UnitLengthIn = (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_UnitLengthIn_Index = BinData.BinBalance_UnitLengthBal_Index;
                    BinCard.BinCard_UnitLengthIn_Id = BinData.BinBalance_UnitLengthBal_Id;
                    BinCard.BinCard_UnitLengthIn_Name = BinData.BinBalance_UnitLengthBal_Name;
                    BinCard.BinCard_UnitLengthInRatio = BinData.BinBalance_UnitLengthBalRatio;

                    BinCard.BinCard_LengthIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_LengthIn_Index = BinData.BinBalance_LengthBal_Index;
                    BinCard.BinCard_LengthIn_Id = BinData.BinBalance_LengthBal_Id;
                    BinCard.BinCard_LengthIn_Name = BinData.BinBalance_LengthBal_Name;
                    BinCard.BinCard_LengthInRatio = BinData.BinBalance_LengthBalRatio;

                    BinCard.BinCard_UnitHeightIn = (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_UnitHeightIn_Index = BinData.BinBalance_UnitHeightBal_Index;
                    BinCard.BinCard_UnitHeightIn_Id = BinData.BinBalance_UnitHeightBal_Id;
                    BinCard.BinCard_UnitHeightIn_Name = BinData.BinBalance_UnitHeightBal_Name;
                    BinCard.BinCard_UnitHeightInRatio = BinData.BinBalance_UnitHeightBalRatio;

                    BinCard.BinCard_HeightIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_HeightIn_Index = BinData.BinBalance_HeightBal_Index;
                    BinCard.BinCard_HeightIn_Id = BinData.BinBalance_HeightBal_Id;
                    BinCard.BinCard_HeightIn_Name = BinData.BinBalance_HeightBal_Name;
                    BinCard.BinCard_HeightInRatio = BinData.BinBalance_HeightBalRatio;

                    BinCard.BinCard_UnitVolumeIn = (BinData.BinBalance_UnitVolumeBal ?? 0);
                    BinCard.BinCard_VolumeIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0);

                    BinCard.BinCard_UnitPriceIn = BinData.UnitPrice;
                    BinCard.BinCard_UnitPriceIn_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_UnitPriceIn_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_UnitPriceIn_Name = BinData.UnitPrice_Name;
                    BinCard.BinCard_PriceIn = (model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0);
                    BinCard.BinCard_PriceIn_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_PriceIn_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_PriceIn_Name = BinData.UnitPrice_Name;

                    //Sign
                    BinCard.BinCard_UnitWeightSign = BinData.BinBalance_UnitWeightBal;
                    BinCard.BinCard_UnitWeightSign_Index = BinData.BinBalance_UnitWeightBal_Index;
                    BinCard.BinCard_UnitWeightSign_Id = BinData.BinBalance_UnitWeightBal_Id;
                    BinCard.BinCard_UnitWeightSign_Name = BinData.BinBalance_UnitWeightBal_Name;
                    BinCard.BinCard_UnitWeightSignRatio = BinData.BinBalance_UnitWeightBalRatio;

                    BinCard.BinCard_WeightSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0));
                    BinCard.BinCard_WeightSign_Index = BinData.BinBalance_WeightBal_Index;
                    BinCard.BinCard_WeightSign_Id = BinData.BinBalance_WeightBal_Id;
                    BinCard.BinCard_WeightSign_Name = BinData.BinBalance_WeightBal_Name;
                    BinCard.BinCard_WeightSignRatio = BinData.BinBalance_WeightBalRatio;

                    BinCard.BinCard_UnitNetWeightSign = BinData.BinBalance_UnitNetWeightBal;
                    BinCard.BinCard_UnitNetWeightSign_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                    BinCard.BinCard_UnitNetWeightSign_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                    BinCard.BinCard_UnitNetWeightSign_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                    BinCard.BinCard_UnitNetWeightSignRatio = BinData.BinBalance_UnitNetWeightBalRatio;

                    BinCard.BinCard_NetWeightSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0));
                    BinCard.BinCard_NetWeightSign_Index = BinData.BinBalance_NetWeightBal_Index;
                    BinCard.BinCard_NetWeightSign_Id = BinData.BinBalance_NetWeightBal_Id;
                    BinCard.BinCard_NetWeightSign_Name = BinData.BinBalance_NetWeightBal_Name;
                    BinCard.BinCard_NetWeightSignRatio = BinData.BinBalance_NetWeightBalRatio;

                    BinCard.BinCard_UnitGrsWeightSign = BinData.BinBalance_UnitGrsWeightBal;
                    BinCard.BinCard_UnitGrsWeightSign_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                    BinCard.BinCard_UnitGrsWeightSign_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                    BinCard.BinCard_UnitGrsWeightSign_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                    BinCard.BinCard_UnitGrsWeightSignRatio = BinData.BinBalance_UnitGrsWeightBalRatio;

                    BinCard.BinCard_GrsWeightSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0));
                    BinCard.BinCard_GrsWeightSign_Index = BinData.BinBalance_GrsWeightBal_Index;
                    BinCard.BinCard_GrsWeightSign_Id = BinData.BinBalance_GrsWeightBal_Id;
                    BinCard.BinCard_GrsWeightSign_Name = BinData.BinBalance_GrsWeightBal_Name;
                    BinCard.BinCard_GrsWeightSignRatio = BinData.BinBalance_GrsWeightBalRatio;

                    BinCard.BinCard_UnitWidthSign = (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_UnitWidthSign_Index = BinData.BinBalance_UnitWidthBal_Index;
                    BinCard.BinCard_UnitWidthSign_Id = BinData.BinBalance_UnitWidthBal_Id;
                    BinCard.BinCard_UnitWidthSign_Name = BinData.BinBalance_UnitWidthBal_Name;
                    BinCard.BinCard_UnitWidthSignRatio = BinData.BinBalance_UnitWidthBalRatio;

                    BinCard.BinCard_WidthSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0));
                    BinCard.BinCard_WidthSign_Index = BinData.BinBalance_WidthBal_Index;
                    BinCard.BinCard_WidthSign_Id = BinData.BinBalance_WidthBal_Id;
                    BinCard.BinCard_WidthSign_Name = BinData.BinBalance_WidthBal_Name;
                    BinCard.BinCard_WidthSignRatio = BinData.BinBalance_WidthBalRatio;

                    BinCard.BinCard_UnitLengthSign = (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_UnitLengthSign_Index = BinData.BinBalance_UnitLengthBal_Index;
                    BinCard.BinCard_UnitLengthSign_Id = BinData.BinBalance_UnitLengthBal_Id;
                    BinCard.BinCard_UnitLengthSign_Name = BinData.BinBalance_UnitLengthBal_Name;
                    BinCard.BinCard_UnitLengthSignRatio = BinData.BinBalance_UnitLengthBalRatio;

                    BinCard.BinCard_LengthSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0));
                    BinCard.BinCard_LengthSign_Index = BinData.BinBalance_LengthBal_Index;
                    BinCard.BinCard_LengthSign_Id = BinData.BinBalance_LengthBal_Id;
                    BinCard.BinCard_LengthSign_Name = BinData.BinBalance_LengthBal_Name;
                    BinCard.BinCard_LengthSignRatio = BinData.BinBalance_LengthBalRatio;

                    BinCard.BinCard_UnitHeightSign = (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_UnitHeightSign_Index = BinData.BinBalance_UnitHeightBal_Index;
                    BinCard.BinCard_UnitHeightSign_Id = BinData.BinBalance_UnitHeightBal_Id;
                    BinCard.BinCard_UnitHeightSign_Name = BinData.BinBalance_UnitHeightBal_Name;
                    BinCard.BinCard_UnitHeightSignRatio = BinData.BinBalance_UnitHeightBalRatio;

                    BinCard.BinCard_HeightSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0));
                    BinCard.BinCard_HeightSign_Index = BinData.BinBalance_HeightBal_Index;
                    BinCard.BinCard_HeightSign_Id = BinData.BinBalance_HeightBal_Id;
                    BinCard.BinCard_HeightSign_Name = BinData.BinBalance_HeightBal_Name;
                    BinCard.BinCard_HeightSignRatio = BinData.BinBalance_HeightBalRatio;

                    BinCard.BinCard_UnitVolumeSign = (BinData.BinBalance_UnitVolumeBal ?? 0);
                    BinCard.BinCard_VolumeSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0));

                    BinCard.BinCard_UnitPriceSign = BinData.UnitPrice;
                    BinCard.BinCard_UnitPriceSign_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_UnitPriceSign_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_UnitPriceSign_Name = BinData.UnitPrice_Name;
                    BinCard.BinCard_PriceSign = ((model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0));
                    BinCard.BinCard_PriceSign_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_PriceSign_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_PriceSign_Name = BinData.UnitPrice_Name;

                    BinCard.Ref_Document_No = model.ref_Document_No;
                    BinCard.Ref_Document_Index = model.ref_Document_Index;
                    BinCard.Ref_DocumentItem_Index = model.ref_DocumentItem_Index;
                    BinCard.TagOutItem_Index = model?.tagOutItem_Index.ToString();
                    BinCard.TagOut_Index = model?.tagOut_Index;
                    BinCard.TagOut_No = model.tagOut_No;
                    BinCard.Create_By = model.userName;
                    BinCard.Create_Date = DateTime.Now;
                    BinCard.BinCard_Date = DateTime.Now;
                    BinCard.BinBalance_Index = model.binBalance_Index;

                    listBinCard.Add(BinCard);


                    if (model.isScanPick)
                    {
                        BinData.Location_Index = new Guid(model?.location_Index_To.ToString());
                        BinData.Location_Id = model.location_Id_To;
                        BinData.Location_Name = model.location_Name_To;
                        BinData.Update_By = model.userName;
                        BinData.Update_Date = DateTime.Now;

                        if (model.isScanToDock)
                        {
                            BinData.Tag_No = model.tag_No_To;
                            BinData.Tag_Index = new Guid(model?.tag_Index_To.ToString());
                        }

                    }
                    else
                    {
                        if (BinData.BinBalance_QtyBal == model.picking_TotalQty && BinData.BinBalance_QtyBal == BinData.BinBalance_QtyReserve)
                        {
                            if (model.isTransfer)
                            {
                                BinData.BinBalance_QtyReserve = (BinData.BinBalance_QtyReserve - model.picking_TotalQty);
                                BinData.BinBalance_WeightReserve = (BinData.BinBalance_WeightReserve - model.Weight);
                                BinData.BinBalance_VolumeReserve = (BinData.BinBalance_VolumeReserve - model.Volume);

                                BinData.Location_Index = new Guid(model?.location_Index_To.ToString());
                                BinData.Location_Id = model.location_Id_To;
                                BinData.Location_Name = model.location_Name_To;
                                BinData.Tag_No = model.tag_No_To;
                                BinData.Tag_Index = new Guid(model?.tag_Index_To.ToString());
                                BinData.ItemStatus_Index = new Guid(model?.itemStatus_Index_To.ToString());
                                BinData.ItemStatus_Id = model.itemStatus_Id_To;
                                BinData.ItemStatus_Name = model.itemStatus_Name_To;
                            }
                            else
                            {
                                BinData.Location_Index = new Guid(model?.location_Index_To.ToString());
                                BinData.Location_Id = model.location_Id_To;
                                BinData.Location_Name = model.location_Name_To;
                            }
                            BinData.Update_By = model.userName;
                            BinData.Update_Date = DateTime.Now;
                        }
                        else
                        {
                            IsInsert = true;
                            var tagitem = new
                            {
                                tag_Index = model.tag_Index_To
                                ,
                                goodsReceive_Index = BinData.GoodsReceive_Index
                                ,
                                goodsReceiveItem_Index = BinData.GoodsReceiveItem_Index
                                ,
                                process_Index = model.process_Index
                                ,
                                product_Index = BinData.Product_Index
                                ,
                                product_Id = BinData.Product_Id
                                ,
                                product_Name = BinData.Product_Name
                                ,
                                product_SecondName = BinData.Product_SecondName
                                ,
                                product_ThirdName = BinData.Product_ThirdName
                                ,
                                product_Lot = BinData.Product_Lot
                                ,
                                itemStatus_Index = model.itemStatus_Index_To
                                ,
                                itemStatus_Id = model.itemStatus_Id_To
                                ,
                                itemStatus_Name = model.itemStatus_Name_To
                                ,
                                qty = model.picking_Qty
                                ,
                                productConversion_Ratio = model.picking_Ratio
                                ,
                                totalQty = model.picking_TotalQty
                                ,
                                productConversion_Index = model.productConversion_Index
                                ,
                                productConversion_Id = model.productConversion_Id
                                ,
                                productConversion_Name = model.productConversion_Name
                                ,
                                weight = model.Weight
                                ,
                                volume = model.Volume
                                ,
                                mfg_Date = model.mfg_Date
                                ,
                                exp_Date = model.exp_Date
                                ,
                                create_By = model.userName
                                ,
                                suggest_Location_Index = new Guid(model?.location_Index_To.ToString())
                                ,
                                suggest_Location_Id = model.location_Id_To
                                ,
                                suggest_Location_Name = model.location_Name_To
                            };
                            var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagItemsV2"), tagitem.sJson());


                            ////--------------------Bin Balance --------------------

                            BinBalance.BinBalance_Index = Guid.NewGuid();

                            guid = BinBalance.BinBalance_Index.ToString();

                            BinBalance.Owner_Index = BinData.Owner_Index;
                            BinBalance.Owner_Id = BinData.Owner_Id;
                            BinBalance.Owner_Name = BinData.Owner_Name;

                            BinBalance.Location_Index = new Guid(model?.location_Index_To.ToString());
                            BinBalance.Location_Id = model.location_Id_To;
                            BinBalance.Location_Name = model.location_Name_To;

                            BinBalance.GoodsReceive_Index = BinData.GoodsReceive_Index;
                            BinBalance.GoodsReceive_No = BinData.GoodsReceive_No;
                            BinBalance.GoodsReceive_Date = BinData.GoodsReceive_Date;
                            BinBalance.GoodsReceiveItem_Index = BinData.GoodsReceiveItem_Index;
                            BinBalance.GoodsReceiveItemLocation_Index = BinData.GoodsReceiveItemLocation_Index;
                            BinBalance.TagItem_Index = BinData.TagItem_Index;
                            BinBalance.Tag_Index = new Guid(model.tag_Index_To.ToString());
                            BinBalance.Tag_No = model.tag_No_To;
                            BinBalance.Product_Index = BinData.Product_Index;
                            BinBalance.Product_Id = BinData.Product_Id;
                            BinBalance.Product_Name = BinData.Product_Name;
                            BinBalance.Product_SecondName = BinData.Product_SecondName;
                            BinBalance.Product_ThirdName = BinData.Product_ThirdName;
                            BinBalance.Product_Lot = BinData.Product_Lot;
                            BinBalance.ItemStatus_Index = BinData.ItemStatus_Index;
                            BinBalance.ItemStatus_Id = BinData.ItemStatus_Id;
                            BinBalance.ItemStatus_Name = BinData.ItemStatus_Name;
                            BinBalance.GoodsReceive_MFG_Date = BinData.GoodsReceive_MFG_Date;
                            BinBalance.GoodsReceive_EXP_Date = BinData.GoodsReceive_EXP_Date;
                            BinBalance.GoodsReceive_ProductConversion_Index = BinData.GoodsReceive_ProductConversion_Index;
                            BinBalance.GoodsReceive_ProductConversion_Id = BinData.GoodsReceive_ProductConversion_Id;
                            BinBalance.GoodsReceive_ProductConversion_Name = BinData.GoodsReceive_ProductConversion_Name;
                            BinBalance.BinBalance_Ratio = BinData.BinBalance_Ratio;
                            BinBalance.BinBalance_QtyBegin = model.picking_TotalQty;
                            BinBalance.BinBalance_WeightBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0);
                            BinBalance.BinBalance_WeightBegin_Index = BinData.BinBalance_WeightBegin_Index;
                            BinBalance.BinBalance_WeightBegin_Id = BinData.BinBalance_WeightBegin_Id;
                            BinBalance.BinBalance_WeightBegin_Name = BinData.BinBalance_WeightBegin_Name;
                            BinBalance.BinBalance_WeightBeginRatio = BinData.BinBalance_WeightBeginRatio;
                            BinBalance.BinBalance_NetWeightBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0);
                            BinBalance.BinBalance_NetWeightBegin_Index = BinData.BinBalance_NetWeightBegin_Index;
                            BinBalance.BinBalance_NetWeightBegin_Id = BinData.BinBalance_NetWeightBegin_Id;
                            BinBalance.BinBalance_NetWeightBegin_Name = BinData.BinBalance_NetWeightBegin_Name;
                            BinBalance.BinBalance_NetWeightBeginRatio = BinData.BinBalance_NetWeightBeginRatio;
                            BinBalance.BinBalance_GrsWeightBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0);
                            BinBalance.BinBalance_GrsWeightBegin_Index = BinData.BinBalance_GrsWeightBegin_Index;
                            BinBalance.BinBalance_GrsWeightBegin_Id = BinData.BinBalance_GrsWeightBegin_Id;
                            BinBalance.BinBalance_GrsWeightBegin_Name = BinData.BinBalance_GrsWeightBegin_Name;
                            BinBalance.BinBalance_GrsWeightBeginRatio = BinData.BinBalance_GrsWeightBeginRatio;
                            BinBalance.BinBalance_WidthBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0);
                            BinBalance.BinBalance_WidthBegin_Index = BinData.BinBalance_WidthBegin_Index;
                            BinBalance.BinBalance_WidthBegin_Id = BinData.BinBalance_WidthBegin_Id;
                            BinBalance.BinBalance_WidthBegin_Name = BinData.BinBalance_WidthBegin_Name;
                            BinBalance.BinBalance_WidthBeginRatio = BinData.BinBalance_WidthBeginRatio;
                            BinBalance.BinBalance_LengthBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0);
                            BinBalance.BinBalance_LengthBegin_Index = BinData.BinBalance_LengthBegin_Index;
                            BinBalance.BinBalance_LengthBegin_Id = BinData.BinBalance_LengthBegin_Id;
                            BinBalance.BinBalance_LengthBegin_Name = BinData.BinBalance_LengthBegin_Name;
                            BinBalance.BinBalance_LengthBeginRatio = BinData.BinBalance_LengthBeginRatio;
                            BinBalance.BinBalance_HeightBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0);
                            BinBalance.BinBalance_HeightBegin_Index = BinData.BinBalance_HeightBegin_Index;
                            BinBalance.BinBalance_HeightBegin_Id = BinData.BinBalance_HeightBegin_Id;
                            BinBalance.BinBalance_HeightBegin_Name = BinData.BinBalance_HeightBegin_Name;
                            BinBalance.BinBalance_HeightBeginRatio = BinData.BinBalance_HeightBeginRatio;
                            BinBalance.BinBalance_UnitVolumeBegin = BinData.BinBalance_UnitVolumeBegin;
                            BinBalance.BinBalance_VolumeBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0);
                            BinBalance.BinBalance_QtyBal = model.picking_TotalQty;
                            BinBalance.BinBalance_UnitWeightBal = BinData.BinBalance_UnitWeightBal;
                            BinBalance.BinBalance_UnitWeightBal_Index = BinData.BinBalance_UnitWeightBal_Index;
                            BinBalance.BinBalance_UnitWeightBal_Id = BinData.BinBalance_UnitWeightBal_Id;
                            BinBalance.BinBalance_UnitWeightBal_Name = BinData.BinBalance_UnitWeightBal_Name;
                            BinBalance.BinBalance_UnitWeightBalRatio = BinData.BinBalance_UnitWeightBalRatio;
                            BinBalance.BinBalance_WeightBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0);
                            BinBalance.BinBalance_WeightBal_Index = BinData.BinBalance_WeightBal_Index;
                            BinBalance.BinBalance_WeightBal_Id = BinData.BinBalance_WeightBal_Id;
                            BinBalance.BinBalance_WeightBal_Name = BinData.BinBalance_WeightBal_Name;
                            BinBalance.BinBalance_WeightBalRatio = BinData.BinBalance_WeightBalRatio;
                            BinBalance.BinBalance_UnitNetWeightBal = BinData.BinBalance_UnitNetWeightBal;
                            BinBalance.BinBalance_UnitNetWeightBal_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                            BinBalance.BinBalance_UnitNetWeightBal_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                            BinBalance.BinBalance_UnitNetWeightBal_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                            BinBalance.BinBalance_UnitNetWeightBalRatio = BinData.BinBalance_UnitNetWeightBalRatio;
                            BinBalance.BinBalance_NetWeightBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0);
                            BinBalance.BinBalance_NetWeightBal_Index = BinData.BinBalance_NetWeightBal_Index;
                            BinBalance.BinBalance_NetWeightBal_Id = BinData.BinBalance_NetWeightBal_Id;
                            BinBalance.BinBalance_NetWeightBal_Name = BinData.BinBalance_NetWeightBal_Name;
                            BinBalance.BinBalance_NetWeightBalRatio = BinData.BinBalance_NetWeightBalRatio;
                            BinBalance.BinBalance_UnitGrsWeightBal = BinData.BinBalance_UnitGrsWeightBal;
                            BinBalance.BinBalance_UnitGrsWeightBal_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                            BinBalance.BinBalance_UnitGrsWeightBal_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                            BinBalance.BinBalance_UnitGrsWeightBal_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                            BinBalance.BinBalance_UnitGrsWeightBalRatio = BinData.BinBalance_UnitGrsWeightBalRatio;
                            BinBalance.BinBalance_GrsWeightBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0);
                            BinBalance.BinBalance_GrsWeightBal_Index = BinData.BinBalance_GrsWeightBal_Index;
                            BinBalance.BinBalance_GrsWeightBal_Id = BinData.BinBalance_GrsWeightBal_Id;
                            BinBalance.BinBalance_GrsWeightBal_Name = BinData.BinBalance_GrsWeightBal_Name;
                            BinBalance.BinBalance_GrsWeightBalRatio = BinData.BinBalance_GrsWeightBalRatio;
                            BinBalance.BinBalance_UnitWidthBal = BinData.BinBalance_UnitWidthBal;
                            BinBalance.BinBalance_UnitWidthBal_Index = BinData.BinBalance_UnitWidthBal_Index;
                            BinBalance.BinBalance_UnitWidthBal_Id = BinData.BinBalance_UnitWidthBal_Id;
                            BinBalance.BinBalance_UnitWidthBal_Name = BinData.BinBalance_UnitWidthBal_Name;
                            BinBalance.BinBalance_UnitWidthBalRatio = BinData.BinBalance_UnitWidthBalRatio;
                            BinBalance.BinBalance_WidthBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0);
                            BinBalance.BinBalance_WidthBal_Index = BinData.BinBalance_WidthBal_Index;
                            BinBalance.BinBalance_WidthBal_Id = BinData.BinBalance_WidthBal_Id;
                            BinBalance.BinBalance_WidthBal_Name = BinData.BinBalance_WidthBal_Name;
                            BinBalance.BinBalance_WidthBalRatio = BinData.BinBalance_WidthBalRatio;
                            BinBalance.BinBalance_UnitLengthBal = BinData.BinBalance_UnitLengthBal;
                            BinBalance.BinBalance_UnitLengthBal_Index = BinData.BinBalance_UnitLengthBal_Index;
                            BinBalance.BinBalance_UnitLengthBal_Id = BinData.BinBalance_UnitLengthBal_Id;
                            BinBalance.BinBalance_UnitLengthBal_Name = BinData.BinBalance_UnitLengthBal_Name;
                            BinBalance.BinBalance_UnitLengthBalRatio = BinData.BinBalance_UnitLengthBalRatio;
                            BinBalance.BinBalance_LengthBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0);
                            BinBalance.BinBalance_LengthBal_Index = BinData.BinBalance_LengthBal_Index;
                            BinBalance.BinBalance_LengthBal_Id = BinData.BinBalance_LengthBal_Id;
                            BinBalance.BinBalance_LengthBal_Name = BinData.BinBalance_LengthBal_Name;
                            BinBalance.BinBalance_LengthBalRatio = BinData.BinBalance_LengthBalRatio;
                            BinBalance.BinBalance_UnitHeightBal = BinData.BinBalance_UnitHeightBal;
                            BinBalance.BinBalance_UnitHeightBal_Index = BinData.BinBalance_UnitHeightBal_Index;
                            BinBalance.BinBalance_UnitHeightBal_Id = BinData.BinBalance_UnitHeightBal_Id;
                            BinBalance.BinBalance_UnitHeightBal_Name = BinData.BinBalance_UnitHeightBal_Name;
                            BinBalance.BinBalance_UnitHeightBalRatio = BinData.BinBalance_UnitHeightBalRatio;
                            BinBalance.BinBalance_HeightBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0);
                            BinBalance.BinBalance_HeightBal_Index = BinData.BinBalance_HeightBal_Index;
                            BinBalance.BinBalance_HeightBal_Id = BinData.BinBalance_HeightBal_Id;
                            BinBalance.BinBalance_HeightBal_Name = BinData.BinBalance_HeightBal_Name;
                            BinBalance.BinBalance_HeightBalRatio = BinData.BinBalance_HeightBalRatio;
                            BinBalance.BinBalance_UnitVolumeBal = BinData.BinBalance_UnitVolumeBal;
                            BinBalance.BinBalance_VolumeBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0);


                            if (model.isTransfer)
                            {
                                BinBalance.BinBalance_QtyReserve = 0;
                                BinBalance.BinBalance_WeightReserve = 0;
                                BinBalance.BinBalance_WeightReserve_Index = BinData.BinBalance_WeightReserve_Index;
                                BinBalance.BinBalance_WeightReserve_Id = BinData.BinBalance_WeightReserve_Id;
                                BinBalance.BinBalance_WeightReserve_Name = BinData.BinBalance_WeightReserve_Name;
                                BinBalance.BinBalance_WeightReserveRatio = BinData.BinBalance_WeightReserveRatio;
                                BinBalance.BinBalance_NetWeightReserve = 0;
                                BinBalance.BinBalance_NetWeightReserve_Index = BinData.BinBalance_NetWeightReserve_Index;
                                BinBalance.BinBalance_NetWeightReserve_Id = BinData.BinBalance_NetWeightReserve_Id;
                                BinBalance.BinBalance_NetWeightReserve_Name = BinData.BinBalance_NetWeightReserve_Name;
                                BinBalance.BinBalance_NetWeightReserveRatio = BinData.BinBalance_NetWeightReserveRatio;
                                BinBalance.BinBalance_GrsWeightReserve = 0;
                                BinBalance.BinBalance_GrsWeightReserve_Index = BinData.BinBalance_GrsWeightReserve_Index;
                                BinBalance.BinBalance_GrsWeightReserve_Id = BinData.BinBalance_GrsWeightReserve_Id;
                                BinBalance.BinBalance_GrsWeightReserve_Name = BinData.BinBalance_GrsWeightReserve_Name;
                                BinBalance.BinBalance_GrsWeightReserveRatio = BinData.BinBalance_GrsWeightReserveRatio;
                                BinBalance.BinBalance_WidthReserve = 0;
                                BinBalance.BinBalance_WidthReserve_Index = BinData.BinBalance_WidthReserve_Index;
                                BinBalance.BinBalance_WidthReserve_Id = BinData.BinBalance_WidthReserve_Id;
                                BinBalance.BinBalance_WidthReserve_Name = BinData.BinBalance_WidthReserve_Name;
                                BinBalance.BinBalance_WidthReserveRatio = BinData.BinBalance_WidthReserveRatio;
                                BinBalance.BinBalance_LengthReserve = 0;
                                BinBalance.BinBalance_LengthReserve_Index = BinData.BinBalance_LengthReserve_Index;
                                BinBalance.BinBalance_LengthReserve_Id = BinData.BinBalance_LengthReserve_Id;
                                BinBalance.BinBalance_LengthReserve_Name = BinData.BinBalance_LengthReserve_Name;
                                BinBalance.BinBalance_LengthReserveRatio = BinData.BinBalance_LengthReserveRatio;
                                BinBalance.BinBalance_HeightReserve = 0;
                                BinBalance.BinBalance_HeightReserve_Index = BinData.BinBalance_HeightReserve_Index;
                                BinBalance.BinBalance_HeightReserve_Id = BinData.BinBalance_HeightReserve_Id;
                                BinBalance.BinBalance_HeightReserve_Name = BinData.BinBalance_HeightReserve_Name;
                                BinBalance.BinBalance_HeightReserveRatio = BinData.BinBalance_HeightReserveRatio;
                                BinBalance.BinBalance_UnitVolumeReserve = 0;
                                BinBalance.BinBalance_VolumeReserve = 0;

                                BinBalance.ItemStatus_Index = model.itemStatus_Index_To == null ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(model.itemStatus_Index_To.ToString());
                                BinBalance.ItemStatus_Id = model.itemStatus_Id_To;
                                BinBalance.ItemStatus_Name = model.itemStatus_Name_To;
                            }
                            else
                            {
                                BinBalance.BinBalance_QtyReserve = model.picking_TotalQty;
                                BinBalance.BinBalance_WeightReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0);
                                BinBalance.BinBalance_WeightReserve_Index = BinData.BinBalance_WeightReserve_Index;
                                BinBalance.BinBalance_WeightReserve_Id = BinData.BinBalance_WeightReserve_Id;
                                BinBalance.BinBalance_WeightReserve_Name = BinData.BinBalance_WeightReserve_Name;
                                BinBalance.BinBalance_WeightReserveRatio = BinData.BinBalance_WeightReserveRatio;
                                BinBalance.BinBalance_NetWeightReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0);
                                BinBalance.BinBalance_NetWeightReserve_Index = BinData.BinBalance_NetWeightReserve_Index;
                                BinBalance.BinBalance_NetWeightReserve_Id = BinData.BinBalance_NetWeightReserve_Id;
                                BinBalance.BinBalance_NetWeightReserve_Name = BinData.BinBalance_NetWeightReserve_Name;
                                BinBalance.BinBalance_NetWeightReserveRatio = BinData.BinBalance_NetWeightReserveRatio;
                                BinBalance.BinBalance_GrsWeightReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0);
                                BinBalance.BinBalance_GrsWeightReserve_Index = BinData.BinBalance_GrsWeightReserve_Index;
                                BinBalance.BinBalance_GrsWeightReserve_Id = BinData.BinBalance_GrsWeightReserve_Id;
                                BinBalance.BinBalance_GrsWeightReserve_Name = BinData.BinBalance_GrsWeightReserve_Name;
                                BinBalance.BinBalance_GrsWeightReserveRatio = BinData.BinBalance_GrsWeightReserveRatio;
                                BinBalance.BinBalance_WidthReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0);
                                BinBalance.BinBalance_WidthReserve_Index = BinData.BinBalance_WidthReserve_Index;
                                BinBalance.BinBalance_WidthReserve_Id = BinData.BinBalance_WidthReserve_Id;
                                BinBalance.BinBalance_WidthReserve_Name = BinData.BinBalance_WidthReserve_Name;
                                BinBalance.BinBalance_WidthReserveRatio = BinData.BinBalance_WidthReserveRatio;
                                BinBalance.BinBalance_LengthReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0);
                                BinBalance.BinBalance_LengthReserve_Index = BinData.BinBalance_LengthReserve_Index;
                                BinBalance.BinBalance_LengthReserve_Id = BinData.BinBalance_LengthReserve_Id;
                                BinBalance.BinBalance_LengthReserve_Name = BinData.BinBalance_LengthReserve_Name;
                                BinBalance.BinBalance_LengthReserveRatio = BinData.BinBalance_LengthReserveRatio;
                                BinBalance.BinBalance_HeightReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0);
                                BinBalance.BinBalance_HeightReserve_Index = BinData.BinBalance_HeightReserve_Index;
                                BinBalance.BinBalance_HeightReserve_Id = BinData.BinBalance_HeightReserve_Id;
                                BinBalance.BinBalance_HeightReserve_Name = BinData.BinBalance_HeightReserve_Name;
                                BinBalance.BinBalance_HeightReserveRatio = BinData.BinBalance_HeightReserveRatio;
                                BinBalance.BinBalance_UnitVolumeReserve = BinData.BinBalance_UnitVolumeReserve;
                                BinBalance.BinBalance_VolumeReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0);

                            }
                            BinBalance.ProductConversion_Index = BinData.ProductConversion_Index;
                            BinBalance.ProductConversion_Id = BinData.ProductConversion_Id;
                            BinBalance.ProductConversion_Name = BinData.ProductConversion_Name;

                            BinBalance.UnitPrice = BinData.UnitPrice;
                            BinBalance.UnitPrice_Index = BinData.UnitPrice_Index;
                            BinBalance.UnitPrice_Id = BinData.UnitPrice_Id;
                            BinBalance.UnitPrice_Name = BinData.UnitPrice_Name;
                            BinBalance.Price = (model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0);
                            BinBalance.Price_Index = BinData.UnitPrice_Index;
                            BinBalance.Price_Id = BinData.UnitPrice_Id;
                            BinBalance.Price_Name = BinData.UnitPrice_Name;

                            BinBalance.UDF_1 = BinData.UDF_1;
                            BinBalance.UDF_2 = BinData.UDF_2;
                            BinBalance.UDF_3 = BinData.UDF_3;
                            BinBalance.UDF_4 = BinData.UDF_4;
                            BinBalance.UDF_5 = BinData.UDF_5;
                            BinBalance.Create_By = model.userName;
                            BinBalance.Create_Date = DateTime.Now;

                            BinBalance.Invoice_No = BinData.Invoice_No;
                            BinBalance.Declaration_No = BinData.Declaration_No;
                            BinBalance.HS_Code = BinData.HS_Code;
                            BinBalance.Conutry_of_Origin = BinData.Conutry_of_Origin;
                            BinBalance.Tax1 = BinData.Tax1;
                            BinBalance.Tax1_Currency_Index = BinData.Tax1_Currency_Index;
                            BinBalance.Tax1_Currency_Id = BinData.Tax1_Currency_Id;
                            BinBalance.Tax1_Currency_Name = BinData.Tax1_Currency_Name;
                            BinBalance.Tax2 = BinData.Tax2;
                            BinBalance.Tax2_Currency_Index = BinData.Tax2_Currency_Index;
                            BinBalance.Tax2_Currency_Id = BinData.Tax2_Currency_Id;
                            BinBalance.Tax2_Currency_Name = BinData.Tax2_Currency_Name;
                            BinBalance.Tax3 = BinData.Tax3;
                            BinBalance.Tax3_Currency_Index = BinData.Tax3_Currency_Index;
                            BinBalance.Tax3_Currency_Id = BinData.Tax3_Currency_Id;
                            BinBalance.Tax3_Currency_Name = BinData.Tax3_Currency_Name;
                            BinBalance.Tax4 = BinData.Tax4;
                            BinBalance.Tax4_Currency_Index = BinData.Tax4_Currency_Index;
                            BinBalance.Tax4_Currency_Id = BinData.Tax4_Currency_Id;
                            BinBalance.Tax4_Currency_Name = BinData.Tax4_Currency_Name;
                            BinBalance.Tax5 = BinData.Tax5;
                            BinBalance.Tax5_Currency_Index = BinData.Tax5_Currency_Index;
                            BinBalance.Tax5_Currency_Id = BinData.Tax5_Currency_Id;
                            BinBalance.Tax5_Currency_Name = BinData.Tax5_Currency_Name;


                            BinData.BinBalance_QtyBal = (BinData.BinBalance_QtyBal - model.picking_TotalQty);
                            BinData.BinBalance_QtyReserve = (BinData.BinBalance_QtyReserve - model.picking_TotalQty);

                            if (BinData.BinBalance_WeightBegin != 0)
                            {
                                var WeightReserve = (model.picking_TotalQty * BinData.BinBalance_UnitWeightBal);
                                BinData.BinBalance_WeightReserve = BinData.BinBalance_WeightReserve - WeightReserve;
                                BinData.BinBalance_WeightBal = BinData.BinBalance_WeightBal - WeightReserve;
                            }

                            if (BinData.BinBalance_NetWeightBegin != 0)
                            {
                                var NetWeightReserve = (model.picking_TotalQty * BinData.BinBalance_UnitNetWeightBal);
                                BinData.BinBalance_NetWeightReserve = BinData.BinBalance_NetWeightReserve - NetWeightReserve;
                                BinData.BinBalance_NetWeightBal = BinData.BinBalance_NetWeightBal - NetWeightReserve;
                            }


                            if (BinData.BinBalance_GrsWeightBegin != 0)
                            {
                                var GrsWeightReserve = (model.picking_TotalQty * BinData.BinBalance_UnitGrsWeightBal);
                                BinData.BinBalance_GrsWeightReserve = BinData.BinBalance_GrsWeightReserve - GrsWeightReserve;
                                BinData.BinBalance_GrsWeightBal = BinData.BinBalance_GrsWeightBal - GrsWeightReserve;
                            }


                            if (BinData.BinBalance_WidthBegin != 0)
                            {
                                var WidthReserve = (model.picking_TotalQty * BinData.BinBalance_UnitWidthBal);
                                BinData.BinBalance_WidthReserve = BinData.BinBalance_WidthReserve - WidthReserve;
                                BinData.BinBalance_WidthBal = BinData.BinBalance_WidthBal - WidthReserve;
                            }


                            if (BinData.BinBalance_LengthBegin != 0)
                            {
                                var LengthReserve = (model.picking_TotalQty * BinData.BinBalance_UnitLengthBal);
                                BinData.BinBalance_LengthReserve = BinData.BinBalance_LengthReserve - LengthReserve;
                                BinData.BinBalance_LengthBal = BinData.BinBalance_LengthBal - LengthReserve;
                            }


                            if (BinData.BinBalance_HeightBegin != 0)
                            {
                                var HeightReserve = (model.picking_TotalQty * BinData.BinBalance_UnitHeightBal);
                                BinData.BinBalance_HeightReserve = BinData.BinBalance_HeightReserve - HeightReserve;
                                BinData.BinBalance_HeightBal = BinData.BinBalance_HeightBal - HeightReserve;
                            }

                            if (BinData.BinBalance_VolumeBegin != 0)
                            {
                                var VolReserve = (model.picking_TotalQty * BinData.BinBalance_UnitVolumeBal);
                                BinData.BinBalance_VolumeReserve = BinData.BinBalance_VolumeReserve - VolReserve;
                                BinData.BinBalance_VolumeBal = BinData.BinBalance_VolumeBal - VolReserve;
                            }

                            if ((BinData.UnitPrice ?? 0) != 0)
                            {
                                var VoltPrice = (model.picking_TotalQty * BinData.UnitPrice);
                                BinData.Price = BinData.Price - VoltPrice;
                            }
                        }
                    }

                }

                var transaction = dbBinbalance.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    dbBinbalance.wm_BinCard.AddRange(listBinCard);
                    if (IsInsert)
                    {
                        dbBinbalance.wm_BinBalance.Add(BinBalance);
                    }
                    dbBinbalance.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    transaction.Rollback();
                    throw exy;
                }

                return guid;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string InsertBinCard(BinCardViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(360);
                olog.logging("InsertBinCard", State + " " + model.sJson());


                string guid = "";
                var BinData = new wm_BinBalance();
                var BinBalance = new wm_BinBalance();
                bool IsInsert = false;

                if (!string.IsNullOrEmpty(model.binBalance_Index.ToString()))
                {
                    State = "if binBalance_Index not null";
                    olog.logging("InsertBinCard", State);

                    BinData = dbBinbalance.wm_BinBalance.Find(model.binBalance_Index);
                }
                else
                {
                    State = "if binBalance_Index IS null";
                    olog.logging("InsertBinCard", State);

                    olog.logging("InsertBinCard", State + " " + model.sJson());

                    BinData = dbBinbalance.wm_BinBalance.FirstOrDefault(c => c.BinBalance_Index == dbBinbalance.wm_BinCardReserve.FirstOrDefault(w => w.Ref_DocumentItem_Index == model.ref_DocumentItem_Index && w.Ref_Document_Index == model.ref_Document_Index).BinBalance_Index);
                }



                guid = BinData.BinBalance_Index.ToString();
                var listBinCard = new List<wm_BinCard>();

                //out
                {
                    var BinCard = new wm_BinCard();
                    BinCard.BinCard_Index = Guid.NewGuid();
                    BinCard.Process_Index = model.process_Index;
                    BinCard.DocumentType_Index = model.documentType_Index;
                    BinCard.DocumentType_Id = model.documentType_Id;
                    BinCard.DocumentType_Name = model.documentType_Name;
                    BinCard.GoodsReceive_Index = BinData.GoodsReceive_Index;
                    BinCard.GoodsReceiveItem_Index = BinData.GoodsReceiveItem_Index;
                    BinCard.GoodsReceiveItemLocation_Index = BinData.GoodsReceiveItemLocation_Index;
                    BinCard.BinCard_No = model.ref_Document_No;
                    BinCard.BinCard_Date = model.goodsIssue_Date;
                    BinCard.TagItem_Index = model.tagItem_Index;
                    BinCard.Tag_Index = BinData.Tag_Index;
                    BinCard.Tag_No = BinData.Tag_No;
                    BinCard.Tag_Index_To = model.tag_Index_To;
                    BinCard.Tag_No_To = model.tag_No_To;
                    BinCard.Product_Index = model.product_Index;
                    BinCard.Product_Id = model.product_Id;
                    BinCard.Product_Name = model.product_Name;
                    BinCard.Product_SecondName = model.product_SecondName;
                    BinCard.Product_ThirdName = model.product_ThirdName;
                    BinCard.Product_Index_To = model.product_Index; //BinCardReserveItem.Product_Index_To;
                    BinCard.Product_Id_To = model.product_Id;
                    BinCard.Product_Name_To = model.product_Name;
                    BinCard.Product_SecondName_To = model.product_SecondName;
                    BinCard.Product_ThirdName_To = model.product_ThirdName;
                    BinCard.Product_Lot = model.product_Lot;
                    BinCard.Product_Lot_To = model.product_Lot;
                    BinCard.ItemStatus_Index = model.itemStatus_Index;
                    BinCard.ItemStatus_Id = model.itemStatus_Id;
                    BinCard.ItemStatus_Name = model.itemStatus_Name;
                    BinCard.ItemStatus_Index_To = model.itemStatus_Index_To;
                    BinCard.ItemStatus_Id_To = model.itemStatus_Id_To;
                    BinCard.ItemStatus_Name_To = model.itemStatus_Name_To;

                    

                    BinCard.ProductConversion_Index = model.productConversion_Index;
                    BinCard.ProductConversion_Id = model.productConversion_Id;
                    BinCard.ProductConversion_Name = model.productConversion_Name;

                    BinCard.Owner_Index = model.owner_Index;
                    BinCard.Owner_Id = model.owner_Id;
                    BinCard.Owner_Name = model.owner_Name;

                    BinCard.Owner_Index_To = model.owner_Index;
                    BinCard.Owner_Id_To = model.owner_Id;
                    BinCard.Owner_Name_To = model.owner_Name;

                    BinCard.Location_Index = model.location_Index;//BinCardReserveItem.Location_Index;
                    BinCard.Location_Id = model.location_Id; //BinCardReserveItem.Location_Id;
                    BinCard.Location_Name = model.location_Name;//BinCardReserveItem.Location_Name;

                    BinCard.Location_Index_To = model.location_Index_To;
                    BinCard.Location_Id_To = model.location_Id_To;
                    BinCard.Location_Name_To = model.location_Name_To;


                    BinCard.GoodsReceive_EXP_Date = model.exp_Date;
                    BinCard.GoodsReceive_EXP_Date_To = model.exp_Date;

                    BinCard.BinCard_QtyIn = 0;
                    BinCard.BinCard_QtyOut = model.picking_TotalQty;
                    BinCard.BinCard_QtySign = model.picking_TotalQty * -1;

                    //Out
                    BinCard.BinCard_UnitWeightOut = BinData.BinBalance_UnitWeightBal;
                    BinCard.BinCard_UnitWeightOut_Index = BinData.BinBalance_UnitWeightBal_Index;
                    BinCard.BinCard_UnitWeightOut_Id = BinData.BinBalance_UnitWeightBal_Id;
                    BinCard.BinCard_UnitWeightOut_Name = BinData.BinBalance_UnitWeightBal_Name;
                    BinCard.BinCard_UnitWeightOutRatio = BinData.BinBalance_UnitWeightBalRatio;

                    BinCard.BinCard_WeightOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0);
                    BinCard.BinCard_WeightOut_Index = BinData.BinBalance_WeightBal_Index;
                    BinCard.BinCard_WeightOut_Id = BinData.BinBalance_WeightBal_Id;
                    BinCard.BinCard_WeightOut_Name = BinData.BinBalance_WeightBal_Name;
                    BinCard.BinCard_WeightOutRatio = BinData.BinBalance_WeightBalRatio;

                    BinCard.BinCard_UnitNetWeightOut = BinData.BinBalance_UnitNetWeightBal;
                    BinCard.BinCard_UnitNetWeightOut_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                    BinCard.BinCard_UnitNetWeightOut_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                    BinCard.BinCard_UnitNetWeightOut_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                    BinCard.BinCard_UnitNetWeightOutRatio = BinData.BinBalance_UnitNetWeightBalRatio;

                    BinCard.BinCard_NetWeightOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0);
                    BinCard.BinCard_NetWeightOut_Index = BinData.BinBalance_NetWeightBal_Index;
                    BinCard.BinCard_NetWeightOut_Id = BinData.BinBalance_NetWeightBal_Id;
                    BinCard.BinCard_NetWeightOut_Name = BinData.BinBalance_NetWeightBal_Name;
                    BinCard.BinCard_NetWeightOutRatio = BinData.BinBalance_NetWeightBalRatio;

                    BinCard.BinCard_UnitGrsWeightOut = BinData.BinBalance_UnitGrsWeightBal;
                    BinCard.BinCard_UnitGrsWeightOut_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                    BinCard.BinCard_UnitGrsWeightOut_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                    BinCard.BinCard_UnitGrsWeightOut_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                    BinCard.BinCard_UnitGrsWeightOutRatio = BinData.BinBalance_UnitGrsWeightBalRatio;

                    BinCard.BinCard_GrsWeightOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0);
                    BinCard.BinCard_GrsWeightOut_Index = BinData.BinBalance_GrsWeightBal_Index;
                    BinCard.BinCard_GrsWeightOut_Id = BinData.BinBalance_GrsWeightBal_Id;
                    BinCard.BinCard_GrsWeightOut_Name = BinData.BinBalance_GrsWeightBal_Name;
                    BinCard.BinCard_GrsWeightOutRatio = BinData.BinBalance_GrsWeightBalRatio;

                    BinCard.BinCard_UnitWidthOut = (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_UnitWidthOut_Index = BinData.BinBalance_UnitWidthBal_Index;
                    BinCard.BinCard_UnitWidthOut_Id = BinData.BinBalance_UnitWidthBal_Id;
                    BinCard.BinCard_UnitWidthOut_Name = BinData.BinBalance_UnitWidthBal_Name;
                    BinCard.BinCard_UnitWidthOutRatio = BinData.BinBalance_UnitWidthBalRatio;

                    BinCard.BinCard_WidthOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_WidthOut_Index = BinData.BinBalance_WidthBal_Index;
                    BinCard.BinCard_WidthOut_Id = BinData.BinBalance_WidthBal_Id;
                    BinCard.BinCard_WidthOut_Name = BinData.BinBalance_WidthBal_Name;
                    BinCard.BinCard_WidthOutRatio = BinData.BinBalance_WidthBalRatio;

                    BinCard.BinCard_UnitLengthOut = (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_UnitLengthOut_Index = BinData.BinBalance_UnitLengthBal_Index;
                    BinCard.BinCard_UnitLengthOut_Id = BinData.BinBalance_UnitLengthBal_Id;
                    BinCard.BinCard_UnitLengthOut_Name = BinData.BinBalance_UnitLengthBal_Name;
                    BinCard.BinCard_UnitLengthOutRatio = BinData.BinBalance_UnitLengthBalRatio;

                    BinCard.BinCard_LengthOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_LengthOut_Index = BinData.BinBalance_LengthBal_Index;
                    BinCard.BinCard_LengthOut_Id = BinData.BinBalance_LengthBal_Id;
                    BinCard.BinCard_LengthOut_Name = BinData.BinBalance_LengthBal_Name;
                    BinCard.BinCard_LengthOutRatio = BinData.BinBalance_LengthBalRatio;

                    BinCard.BinCard_UnitHeightOut = (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_UnitHeightOut_Index = BinData.BinBalance_UnitHeightBal_Index;
                    BinCard.BinCard_UnitHeightOut_Id = BinData.BinBalance_UnitHeightBal_Id;
                    BinCard.BinCard_UnitHeightOut_Name = BinData.BinBalance_UnitHeightBal_Name;
                    BinCard.BinCard_UnitHeightOutRatio = BinData.BinBalance_UnitHeightBalRatio;

                    BinCard.BinCard_HeightOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_HeightOut_Index = BinData.BinBalance_HeightBal_Index;
                    BinCard.BinCard_HeightOut_Id = BinData.BinBalance_HeightBal_Id;
                    BinCard.BinCard_HeightOut_Name = BinData.BinBalance_HeightBal_Name;
                    BinCard.BinCard_HeightOutRatio = BinData.BinBalance_HeightBalRatio;

                    BinCard.BinCard_UnitVolumeOut = (BinData.BinBalance_UnitVolumeBal ?? 0);
                    BinCard.BinCard_VolumeOut = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0);

                    BinCard.BinCard_UnitPriceOut = BinData.UnitPrice;
                    BinCard.BinCard_UnitPriceOut_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_UnitPriceOut_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_UnitPriceOut_Name = BinData.UnitPrice_Name;
                    BinCard.BinCard_PriceOut = (model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0);
                    BinCard.BinCard_PriceOut_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_PriceOut_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_PriceOut_Name = BinData.UnitPrice_Name;

                    //Sign
                    BinCard.BinCard_UnitWeightSign = BinData.BinBalance_UnitWeightBal;
                    BinCard.BinCard_UnitWeightSign_Index = BinData.BinBalance_UnitWeightBal_Index;
                    BinCard.BinCard_UnitWeightSign_Id = BinData.BinBalance_UnitWeightBal_Id;
                    BinCard.BinCard_UnitWeightSign_Name = BinData.BinBalance_UnitWeightBal_Name;
                    BinCard.BinCard_UnitWeightSignRatio = BinData.BinBalance_UnitWeightBalRatio;

                    BinCard.BinCard_WeightSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0)) * -1);
                    BinCard.BinCard_WeightSign_Index = BinData.BinBalance_WeightBal_Index;
                    BinCard.BinCard_WeightSign_Id = BinData.BinBalance_WeightBal_Id;
                    BinCard.BinCard_WeightSign_Name = BinData.BinBalance_WeightBal_Name;
                    BinCard.BinCard_WeightSignRatio = BinData.BinBalance_WeightBalRatio;

                    BinCard.BinCard_UnitNetWeightSign = BinData.BinBalance_UnitNetWeightBal;
                    BinCard.BinCard_UnitNetWeightSign_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                    BinCard.BinCard_UnitNetWeightSign_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                    BinCard.BinCard_UnitNetWeightSign_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                    BinCard.BinCard_UnitNetWeightSignRatio = BinData.BinBalance_UnitNetWeightBalRatio;

                    BinCard.BinCard_NetWeightSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0)) * -1);
                    BinCard.BinCard_NetWeightSign_Index = BinData.BinBalance_NetWeightBal_Index;
                    BinCard.BinCard_NetWeightSign_Id = BinData.BinBalance_NetWeightBal_Id;
                    BinCard.BinCard_NetWeightSign_Name = BinData.BinBalance_NetWeightBal_Name;
                    BinCard.BinCard_NetWeightSignRatio = BinData.BinBalance_NetWeightBalRatio;

                    BinCard.BinCard_UnitGrsWeightSign = BinData.BinBalance_UnitGrsWeightBal;
                    BinCard.BinCard_UnitGrsWeightSign_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                    BinCard.BinCard_UnitGrsWeightSign_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                    BinCard.BinCard_UnitGrsWeightSign_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                    BinCard.BinCard_UnitGrsWeightSignRatio = BinData.BinBalance_UnitGrsWeightBalRatio;

                    BinCard.BinCard_GrsWeightSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0)) * -1);
                    BinCard.BinCard_GrsWeightSign_Index = BinData.BinBalance_GrsWeightBal_Index;
                    BinCard.BinCard_GrsWeightSign_Id = BinData.BinBalance_GrsWeightBal_Id;
                    BinCard.BinCard_GrsWeightSign_Name = BinData.BinBalance_GrsWeightBal_Name;
                    BinCard.BinCard_GrsWeightSignRatio = BinData.BinBalance_GrsWeightBalRatio;

                    BinCard.BinCard_UnitWidthSign = (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_UnitWidthSign_Index = BinData.BinBalance_UnitWidthBal_Index;
                    BinCard.BinCard_UnitWidthSign_Id = BinData.BinBalance_UnitWidthBal_Id;
                    BinCard.BinCard_UnitWidthSign_Name = BinData.BinBalance_UnitWidthBal_Name;
                    BinCard.BinCard_UnitWidthSignRatio = BinData.BinBalance_UnitWidthBalRatio;

                    BinCard.BinCard_WidthSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0)) * -1);
                    BinCard.BinCard_WidthSign_Index = BinData.BinBalance_WidthBal_Index;
                    BinCard.BinCard_WidthSign_Id = BinData.BinBalance_WidthBal_Id;
                    BinCard.BinCard_WidthSign_Name = BinData.BinBalance_WidthBal_Name;
                    BinCard.BinCard_WidthSignRatio = BinData.BinBalance_WidthBalRatio;

                    BinCard.BinCard_UnitLengthSign = (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_UnitLengthSign_Index = BinData.BinBalance_UnitLengthBal_Index;
                    BinCard.BinCard_UnitLengthSign_Id = BinData.BinBalance_UnitLengthBal_Id;
                    BinCard.BinCard_UnitLengthSign_Name = BinData.BinBalance_UnitLengthBal_Name;
                    BinCard.BinCard_UnitLengthSignRatio = BinData.BinBalance_UnitLengthBalRatio;

                    BinCard.BinCard_LengthSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0)) * -1);
                    BinCard.BinCard_LengthSign_Index = BinData.BinBalance_LengthBal_Index;
                    BinCard.BinCard_LengthSign_Id = BinData.BinBalance_LengthBal_Id;
                    BinCard.BinCard_LengthSign_Name = BinData.BinBalance_LengthBal_Name;
                    BinCard.BinCard_LengthSignRatio = BinData.BinBalance_LengthBalRatio;

                    BinCard.BinCard_UnitHeightSign = (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_UnitHeightSign_Index = BinData.BinBalance_UnitHeightBal_Index;
                    BinCard.BinCard_UnitHeightSign_Id = BinData.BinBalance_UnitHeightBal_Id;
                    BinCard.BinCard_UnitHeightSign_Name = BinData.BinBalance_UnitHeightBal_Name;
                    BinCard.BinCard_UnitHeightSignRatio = BinData.BinBalance_UnitHeightBalRatio;

                    BinCard.BinCard_HeightSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0)) * -1);
                    BinCard.BinCard_HeightSign_Index = BinData.BinBalance_HeightBal_Index;
                    BinCard.BinCard_HeightSign_Id = BinData.BinBalance_HeightBal_Id;
                    BinCard.BinCard_HeightSign_Name = BinData.BinBalance_HeightBal_Name;
                    BinCard.BinCard_HeightSignRatio = BinData.BinBalance_HeightBalRatio;

                    BinCard.BinCard_UnitVolumeSign = (BinData.BinBalance_UnitVolumeBal ?? 0);
                    BinCard.BinCard_VolumeSign = (((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0)) * -1);

                    BinCard.BinCard_UnitPriceSign = BinData.UnitPrice;
                    BinCard.BinCard_UnitPriceSign_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_UnitPriceSign_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_UnitPriceSign_Name = BinData.UnitPrice_Name;
                    BinCard.BinCard_PriceSign = (((model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0)) * -1);
                    BinCard.BinCard_PriceSign_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_PriceSign_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_PriceSign_Name = BinData.UnitPrice_Name;
                    BinCard.Ref_Document_No = model.ref_Document_No;
                    BinCard.Ref_Document_Index = model.ref_Document_Index; //tem.Ref_Document_Index;
                    BinCard.Ref_DocumentItem_Index = model.ref_DocumentItem_Index;
                    BinCard.TagOutItem_Index = model?.tagOutItem_Index.ToString();
                    BinCard.TagOut_Index = model?.tagOut_Index;
                    BinCard.TagOut_No = model.tagOut_No;
                    BinCard.Create_By = model.userName;
                    BinCard.Create_Date = DateTime.Now;
                    BinCard.BinCard_Date = DateTime.Now;
                    BinCard.BinBalance_Index = model.binBalance_Index;
                    listBinCard.Add(BinCard);

                }

                //in
                {
                    var BinCard = new wm_BinCard();
                    BinCard.BinCard_Index = Guid.NewGuid();
                    BinCard.Process_Index = model.process_Index;
                    BinCard.DocumentType_Index = model.documentType_Index;
                    BinCard.DocumentType_Id = model.documentType_Id;
                    BinCard.DocumentType_Name = model.documentType_Name;
                    BinCard.GoodsReceive_Index = BinData.GoodsReceive_Index;
                    BinCard.GoodsReceiveItem_Index = BinData.GoodsReceiveItem_Index;
                    BinCard.GoodsReceiveItemLocation_Index = BinData.GoodsReceiveItemLocation_Index;
                    BinCard.BinCard_No = model.ref_Document_No;
                    BinCard.BinCard_Date = model.goodsIssue_Date;
                    BinCard.TagItem_Index = model.tagItem_Index;
                    BinCard.Tag_Index = model.tag_Index_To;
                    BinCard.Tag_No = model.tag_No_To;
                    BinCard.Tag_Index_To = model.tag_Index_To;
                    BinCard.Tag_No_To = model.tag_No_To;
                    BinCard.Product_Index = model.product_Index;
                    BinCard.Product_Id = model.product_Id;
                    BinCard.Product_Name = model.product_Name;
                    BinCard.Product_SecondName = model.product_SecondName;
                    BinCard.Product_ThirdName = model.product_ThirdName;
                    BinCard.Product_Index_To = model.product_Index; //BinCardReserveItem.Product_Index_To;
                    BinCard.Product_Id_To = model.product_Id;
                    BinCard.Product_Name_To = model.product_Name;
                    BinCard.Product_SecondName_To = model.product_SecondName;
                    BinCard.Product_ThirdName_To = model.product_ThirdName;
                    BinCard.Product_Lot = model.product_Lot;
                    BinCard.Product_Lot_To = model.product_Lot;
                    BinCard.ItemStatus_Index = model.itemStatus_Index_To;
                    BinCard.ItemStatus_Id = model.itemStatus_Id_To;
                    BinCard.ItemStatus_Name = model.itemStatus_Name_To;


                    BinCard.ItemStatus_Index_To = model.itemStatus_Index_To;
                    BinCard.ItemStatus_Id_To = model.itemStatus_Id_To;
                    BinCard.ItemStatus_Name_To = model.itemStatus_Name_To;


                    BinCard.ProductConversion_Index = model.productConversion_Index;
                    BinCard.ProductConversion_Id = model.productConversion_Id;
                    BinCard.ProductConversion_Name = model.productConversion_Name;

                    BinCard.Owner_Index = model.owner_Index;
                    BinCard.Owner_Id = model.owner_Id;
                    BinCard.Owner_Name = model.owner_Name;

                    BinCard.Owner_Index_To = model.owner_Index;
                    BinCard.Owner_Id_To = model.owner_Id;
                    BinCard.Owner_Name_To = model.owner_Name;

                    BinCard.Location_Index = model.location_Index_To;//BinCardReserveItem.Location_Index;
                    BinCard.Location_Id = model.location_Id_To; //BinCardReserveItem.Location_Id;
                    BinCard.Location_Name = model.location_Name_To;//BinCardReserveItem.Location_Name;

                    BinCard.Location_Index_To = model.location_Index_To;
                    BinCard.Location_Id_To = model.location_Id_To;
                    BinCard.Location_Name_To = model.location_Name_To;


                    BinCard.GoodsReceive_EXP_Date = model.exp_Date;
                    BinCard.GoodsReceive_EXP_Date_To = model.exp_Date;
                    BinCard.BinCard_QtyIn = model.picking_TotalQty;
                    BinCard.BinCard_QtyOut = 0;
                    BinCard.BinCard_QtySign = model.picking_TotalQty;


                    //In
                    BinCard.BinCard_UnitWeightIn = BinData.BinBalance_UnitWeightBal;
                    BinCard.BinCard_UnitWeightIn_Index = BinData.BinBalance_UnitWeightBal_Index;
                    BinCard.BinCard_UnitWeightIn_Id = BinData.BinBalance_UnitWeightBal_Id;
                    BinCard.BinCard_UnitWeightIn_Name = BinData.BinBalance_UnitWeightBal_Name;
                    BinCard.BinCard_UnitWeightInRatio = BinData.BinBalance_UnitWeightBalRatio;

                    BinCard.BinCard_WeightIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0);
                    BinCard.BinCard_WeightIn_Index = BinData.BinBalance_WeightBal_Index;
                    BinCard.BinCard_WeightIn_Id = BinData.BinBalance_WeightBal_Id;
                    BinCard.BinCard_WeightIn_Name = BinData.BinBalance_WeightBal_Name;
                    BinCard.BinCard_WeightInRatio = BinData.BinBalance_WeightBalRatio;

                    BinCard.BinCard_UnitNetWeightIn = BinData.BinBalance_UnitNetWeightBal;
                    BinCard.BinCard_UnitNetWeightIn_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                    BinCard.BinCard_UnitNetWeightIn_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                    BinCard.BinCard_UnitNetWeightIn_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                    BinCard.BinCard_UnitNetWeightInRatio = BinData.BinBalance_UnitNetWeightBalRatio;

                    BinCard.BinCard_NetWeightIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0);
                    BinCard.BinCard_NetWeightIn_Index = BinData.BinBalance_NetWeightBal_Index;
                    BinCard.BinCard_NetWeightIn_Id = BinData.BinBalance_NetWeightBal_Id;
                    BinCard.BinCard_NetWeightIn_Name = BinData.BinBalance_NetWeightBal_Name;
                    BinCard.BinCard_NetWeightInRatio = BinData.BinBalance_NetWeightBalRatio;

                    BinCard.BinCard_UnitGrsWeightIn = BinData.BinBalance_UnitGrsWeightBal;
                    BinCard.BinCard_UnitGrsWeightIn_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                    BinCard.BinCard_UnitGrsWeightIn_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                    BinCard.BinCard_UnitGrsWeightIn_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                    BinCard.BinCard_UnitGrsWeightInRatio = BinData.BinBalance_UnitGrsWeightBalRatio;

                    BinCard.BinCard_GrsWeightIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0);
                    BinCard.BinCard_GrsWeightIn_Index = BinData.BinBalance_GrsWeightBal_Index;
                    BinCard.BinCard_GrsWeightIn_Id = BinData.BinBalance_GrsWeightBal_Id;
                    BinCard.BinCard_GrsWeightIn_Name = BinData.BinBalance_GrsWeightBal_Name;
                    BinCard.BinCard_GrsWeightInRatio = BinData.BinBalance_GrsWeightBalRatio;

                    BinCard.BinCard_UnitWidthIn = (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_UnitWidthIn_Index = BinData.BinBalance_UnitWidthBal_Index;
                    BinCard.BinCard_UnitWidthIn_Id = BinData.BinBalance_UnitWidthBal_Id;
                    BinCard.BinCard_UnitWidthIn_Name = BinData.BinBalance_UnitWidthBal_Name;
                    BinCard.BinCard_UnitWidthInRatio = BinData.BinBalance_UnitWidthBalRatio;

                    BinCard.BinCard_WidthIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_WidthIn_Index = BinData.BinBalance_WidthBal_Index;
                    BinCard.BinCard_WidthIn_Id = BinData.BinBalance_WidthBal_Id;
                    BinCard.BinCard_WidthIn_Name = BinData.BinBalance_WidthBal_Name;
                    BinCard.BinCard_WidthInRatio = BinData.BinBalance_WidthBalRatio;

                    BinCard.BinCard_UnitLengthIn = (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_UnitLengthIn_Index = BinData.BinBalance_UnitLengthBal_Index;
                    BinCard.BinCard_UnitLengthIn_Id = BinData.BinBalance_UnitLengthBal_Id;
                    BinCard.BinCard_UnitLengthIn_Name = BinData.BinBalance_UnitLengthBal_Name;
                    BinCard.BinCard_UnitLengthInRatio = BinData.BinBalance_UnitLengthBalRatio;

                    BinCard.BinCard_LengthIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_LengthIn_Index = BinData.BinBalance_LengthBal_Index;
                    BinCard.BinCard_LengthIn_Id = BinData.BinBalance_LengthBal_Id;
                    BinCard.BinCard_LengthIn_Name = BinData.BinBalance_LengthBal_Name;
                    BinCard.BinCard_LengthInRatio = BinData.BinBalance_LengthBalRatio;

                    BinCard.BinCard_UnitHeightIn = (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_UnitHeightIn_Index = BinData.BinBalance_UnitHeightBal_Index;
                    BinCard.BinCard_UnitHeightIn_Id = BinData.BinBalance_UnitHeightBal_Id;
                    BinCard.BinCard_UnitHeightIn_Name = BinData.BinBalance_UnitHeightBal_Name;
                    BinCard.BinCard_UnitHeightInRatio = BinData.BinBalance_UnitHeightBalRatio;

                    BinCard.BinCard_HeightIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_HeightIn_Index = BinData.BinBalance_HeightBal_Index;
                    BinCard.BinCard_HeightIn_Id = BinData.BinBalance_HeightBal_Id;
                    BinCard.BinCard_HeightIn_Name = BinData.BinBalance_HeightBal_Name;
                    BinCard.BinCard_HeightInRatio = BinData.BinBalance_HeightBalRatio;

                    BinCard.BinCard_UnitVolumeIn = (BinData.BinBalance_UnitVolumeBal ?? 0);
                    BinCard.BinCard_VolumeIn = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0);

                    BinCard.BinCard_UnitPriceIn = BinData.UnitPrice;
                    BinCard.BinCard_UnitPriceIn_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_UnitPriceIn_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_UnitPriceIn_Name = BinData.UnitPrice_Name;
                    BinCard.BinCard_PriceIn = (model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0);
                    BinCard.BinCard_PriceIn_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_PriceIn_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_PriceIn_Name = BinData.UnitPrice_Name;

                    //Sign
                    BinCard.BinCard_UnitWeightSign = BinData.BinBalance_UnitWeightBal;
                    BinCard.BinCard_UnitWeightSign_Index = BinData.BinBalance_UnitWeightBal_Index;
                    BinCard.BinCard_UnitWeightSign_Id = BinData.BinBalance_UnitWeightBal_Id;
                    BinCard.BinCard_UnitWeightSign_Name = BinData.BinBalance_UnitWeightBal_Name;
                    BinCard.BinCard_UnitWeightSignRatio = BinData.BinBalance_UnitWeightBalRatio;

                    BinCard.BinCard_WeightSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0));
                    BinCard.BinCard_WeightSign_Index = BinData.BinBalance_WeightBal_Index;
                    BinCard.BinCard_WeightSign_Id = BinData.BinBalance_WeightBal_Id;
                    BinCard.BinCard_WeightSign_Name = BinData.BinBalance_WeightBal_Name;
                    BinCard.BinCard_WeightSignRatio = BinData.BinBalance_WeightBalRatio;

                    BinCard.BinCard_UnitNetWeightSign = BinData.BinBalance_UnitNetWeightBal;
                    BinCard.BinCard_UnitNetWeightSign_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                    BinCard.BinCard_UnitNetWeightSign_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                    BinCard.BinCard_UnitNetWeightSign_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                    BinCard.BinCard_UnitNetWeightSignRatio = BinData.BinBalance_UnitNetWeightBalRatio;

                    BinCard.BinCard_NetWeightSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0));
                    BinCard.BinCard_NetWeightSign_Index = BinData.BinBalance_NetWeightBal_Index;
                    BinCard.BinCard_NetWeightSign_Id = BinData.BinBalance_NetWeightBal_Id;
                    BinCard.BinCard_NetWeightSign_Name = BinData.BinBalance_NetWeightBal_Name;
                    BinCard.BinCard_NetWeightSignRatio = BinData.BinBalance_NetWeightBalRatio;

                    BinCard.BinCard_UnitGrsWeightSign = BinData.BinBalance_UnitGrsWeightBal;
                    BinCard.BinCard_UnitGrsWeightSign_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                    BinCard.BinCard_UnitGrsWeightSign_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                    BinCard.BinCard_UnitGrsWeightSign_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                    BinCard.BinCard_UnitGrsWeightSignRatio = BinData.BinBalance_UnitGrsWeightBalRatio;

                    BinCard.BinCard_GrsWeightSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0));
                    BinCard.BinCard_GrsWeightSign_Index = BinData.BinBalance_GrsWeightBal_Index;
                    BinCard.BinCard_GrsWeightSign_Id = BinData.BinBalance_GrsWeightBal_Id;
                    BinCard.BinCard_GrsWeightSign_Name = BinData.BinBalance_GrsWeightBal_Name;
                    BinCard.BinCard_GrsWeightSignRatio = BinData.BinBalance_GrsWeightBalRatio;

                    BinCard.BinCard_UnitWidthSign = (BinData.BinBalance_UnitWidthBal ?? 0);
                    BinCard.BinCard_UnitWidthSign_Index = BinData.BinBalance_UnitWidthBal_Index;
                    BinCard.BinCard_UnitWidthSign_Id = BinData.BinBalance_UnitWidthBal_Id;
                    BinCard.BinCard_UnitWidthSign_Name = BinData.BinBalance_UnitWidthBal_Name;
                    BinCard.BinCard_UnitWidthSignRatio = BinData.BinBalance_UnitWidthBalRatio;

                    BinCard.BinCard_WidthSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0));
                    BinCard.BinCard_WidthSign_Index = BinData.BinBalance_WidthBal_Index;
                    BinCard.BinCard_WidthSign_Id = BinData.BinBalance_WidthBal_Id;
                    BinCard.BinCard_WidthSign_Name = BinData.BinBalance_WidthBal_Name;
                    BinCard.BinCard_WidthSignRatio = BinData.BinBalance_WidthBalRatio;

                    BinCard.BinCard_UnitLengthSign = (BinData.BinBalance_UnitLengthBal ?? 0);
                    BinCard.BinCard_UnitLengthSign_Index = BinData.BinBalance_UnitLengthBal_Index;
                    BinCard.BinCard_UnitLengthSign_Id = BinData.BinBalance_UnitLengthBal_Id;
                    BinCard.BinCard_UnitLengthSign_Name = BinData.BinBalance_UnitLengthBal_Name;
                    BinCard.BinCard_UnitLengthSignRatio = BinData.BinBalance_UnitLengthBalRatio;

                    BinCard.BinCard_LengthSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0));
                    BinCard.BinCard_LengthSign_Index = BinData.BinBalance_LengthBal_Index;
                    BinCard.BinCard_LengthSign_Id = BinData.BinBalance_LengthBal_Id;
                    BinCard.BinCard_LengthSign_Name = BinData.BinBalance_LengthBal_Name;
                    BinCard.BinCard_LengthSignRatio = BinData.BinBalance_LengthBalRatio;

                    BinCard.BinCard_UnitHeightSign = (BinData.BinBalance_UnitHeightBal ?? 0);
                    BinCard.BinCard_UnitHeightSign_Index = BinData.BinBalance_UnitHeightBal_Index;
                    BinCard.BinCard_UnitHeightSign_Id = BinData.BinBalance_UnitHeightBal_Id;
                    BinCard.BinCard_UnitHeightSign_Name = BinData.BinBalance_UnitHeightBal_Name;
                    BinCard.BinCard_UnitHeightSignRatio = BinData.BinBalance_UnitHeightBalRatio;

                    BinCard.BinCard_HeightSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0));
                    BinCard.BinCard_HeightSign_Index = BinData.BinBalance_HeightBal_Index;
                    BinCard.BinCard_HeightSign_Id = BinData.BinBalance_HeightBal_Id;
                    BinCard.BinCard_HeightSign_Name = BinData.BinBalance_HeightBal_Name;
                    BinCard.BinCard_HeightSignRatio = BinData.BinBalance_HeightBalRatio;

                    BinCard.BinCard_UnitVolumeSign = (BinData.BinBalance_UnitVolumeBal ?? 0);
                    BinCard.BinCard_VolumeSign = ((model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0));

                    BinCard.BinCard_UnitPriceSign = BinData.UnitPrice;
                    BinCard.BinCard_UnitPriceSign_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_UnitPriceSign_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_UnitPriceSign_Name = BinData.UnitPrice_Name;
                    BinCard.BinCard_PriceSign = ((model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0));
                    BinCard.BinCard_PriceSign_Index = BinData.UnitPrice_Index;
                    BinCard.BinCard_PriceSign_Id = BinData.UnitPrice_Id;
                    BinCard.BinCard_PriceSign_Name = BinData.UnitPrice_Name;
                    
                    BinCard.Ref_Document_No = model.ref_Document_No;
                    BinCard.Ref_Document_Index = model.ref_Document_Index; //tem.Ref_Document_Index;
                    BinCard.Ref_DocumentItem_Index = model.ref_DocumentItem_Index;
                    BinCard.TagOutItem_Index = model?.tagOutItem_Index.ToString();
                    BinCard.TagOut_Index = model?.tagOut_Index;
                    BinCard.TagOut_No = model.tagOut_No;
                    BinCard.Create_By = model.userName;
                    BinCard.Create_Date = DateTime.Now;
                    BinCard.BinCard_Date = DateTime.Now;
                    BinCard.BinBalance_Index = model.binBalance_Index;

                    listBinCard.Add(BinCard);


                    if (BinData.BinBalance_QtyBal == model.picking_TotalQty && BinData.BinBalance_QtyBal == BinData.BinBalance_QtyReserve)
                    {
                        if (model.isTransfer)
                        {
                            BinData.BinBalance_QtyReserve = (BinData.BinBalance_QtyReserve - model.picking_TotalQty);
                            BinData.BinBalance_WeightReserve = (BinData.BinBalance_WeightReserve - model.Weight);
                            BinData.BinBalance_VolumeReserve = (BinData.BinBalance_VolumeReserve - model.Volume);

                            BinData.Location_Index = new Guid(model?.location_Index_To.ToString());
                            BinData.Location_Id = model.location_Id_To;
                            BinData.Location_Name = model.location_Name_To;
                            BinData.Tag_No = model.tag_No_To;
                            BinData.Tag_Index = new Guid(model?.tag_Index_To.ToString());
                            BinData.ItemStatus_Index = new Guid(model?.itemStatus_Index_To.ToString());
                            BinData.ItemStatus_Id = model.itemStatus_Id_To;
                            BinData.ItemStatus_Name = model.itemStatus_Name_To;

                        }
                        else if (model.isScanSplit)
                        {

                            BinData.Location_Index = new Guid(model?.location_Index_To.ToString());
                            BinData.Location_Id = model.location_Id_To;
                            BinData.Location_Name = model.location_Name_To;
                            BinData.Tag_No = model.tag_No_To;
                            BinData.Tag_Index = new Guid(model?.tag_Index_To.ToString());
                            BinData.ItemStatus_Index = new Guid(model?.itemStatus_Index_To.ToString());
                            BinData.ItemStatus_Id = model.itemStatus_Id_To;
                            BinData.ItemStatus_Name = model.itemStatus_Name_To;

                        }
                        else
                        {
                            BinData.Location_Index = new Guid(model?.location_Index_To.ToString());
                            BinData.Location_Id = model.location_Id_To;
                            BinData.Location_Name = model.location_Name_To;
                        }
                        BinData.Update_By = model.userName;
                        BinData.Update_Date = DateTime.Now;
                    }
                    else
                    {
                        IsInsert = true;
                        var tagitem = new
                        {
                            tag_Index = model.tag_Index_To
                            ,
                            goodsReceive_Index = BinData.GoodsReceive_Index
                            ,
                            goodsReceiveItem_Index = BinData.GoodsReceiveItem_Index
                            ,
                            process_Index = model.process_Index
                            ,
                            product_Index = BinData.Product_Index
                            ,
                            product_Id = BinData.Product_Id
                            ,
                            product_Name = BinData.Product_Name
                            ,
                            product_SecondName = BinData.Product_SecondName
                            ,
                            product_ThirdName = BinData.Product_ThirdName
                            ,
                            product_Lot = BinData.Product_Lot
                            ,
                            itemStatus_Index = model.itemStatus_Index_To
                            ,
                            itemStatus_Id = model.itemStatus_Id_To
                            ,
                            itemStatus_Name = model.itemStatus_Name_To
                            ,
                            qty = model.picking_Qty
                            ,
                            productConversion_Ratio = model.picking_Ratio
                            ,
                            totalQty = model.picking_TotalQty
                            ,
                            productConversion_Index = model.productConversion_Index
                            ,
                            productConversion_Id = model.productConversion_Id
                            ,
                            productConversion_Name = model.productConversion_Name
                            ,
                            weight = model.Weight
                            ,
                            volume = model.Volume
                            ,
                            mfg_Date = model.mfg_Date
                            ,
                            exp_Date = model.exp_Date
                            ,
                            create_By = model.userName
                            ,
                            suggest_Location_Index = new Guid(model?.location_Index_To.ToString())
                            ,
                            suggest_Location_Id = model.location_Id_To
                            ,
                            suggest_Location_Name = model.location_Name_To
                        };
                        var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagItemsV2"), tagitem.sJson());


                        ////--------------------Bin Balance --------------------

                        BinBalance.BinBalance_Index = Guid.NewGuid();

                        guid = BinBalance.BinBalance_Index.ToString();

                        BinBalance.Owner_Index = BinData.Owner_Index;
                        BinBalance.Owner_Id = BinData.Owner_Id;
                        BinBalance.Owner_Name = BinData.Owner_Name;

                        BinBalance.Location_Index = new Guid(model?.location_Index_To.ToString());
                        BinBalance.Location_Id = model.location_Id_To;
                        BinBalance.Location_Name = model.location_Name_To;

                        BinBalance.GoodsReceive_Index = BinData.GoodsReceive_Index;
                        BinBalance.GoodsReceive_No = BinData.GoodsReceive_No;
                        BinBalance.GoodsReceive_Date = BinData.GoodsReceive_Date;
                        BinBalance.GoodsReceiveItem_Index = BinData.GoodsReceiveItem_Index;
                        BinBalance.GoodsReceiveItemLocation_Index = BinData.GoodsReceiveItemLocation_Index;
                        BinBalance.TagItem_Index = BinData.TagItem_Index;
                        BinBalance.Tag_Index = new Guid(model.tag_Index_To.ToString());
                        BinBalance.Tag_No = model.tag_No_To;
                        BinBalance.Product_Index = BinData.Product_Index;
                        BinBalance.Product_Id = BinData.Product_Id;
                        BinBalance.Product_Name = BinData.Product_Name;
                        BinBalance.Product_SecondName = BinData.Product_SecondName;
                        BinBalance.Product_ThirdName = BinData.Product_ThirdName;
                        BinBalance.Product_Lot = BinData.Product_Lot;
                        BinBalance.ItemStatus_Index = BinData.ItemStatus_Index;
                        BinBalance.ItemStatus_Id = BinData.ItemStatus_Id;
                        BinBalance.ItemStatus_Name = BinData.ItemStatus_Name;
                        BinBalance.GoodsReceive_MFG_Date = BinData.GoodsReceive_MFG_Date;
                        BinBalance.GoodsReceive_EXP_Date = BinData.GoodsReceive_EXP_Date;
                        BinBalance.GoodsReceive_ProductConversion_Index = BinData.GoodsReceive_ProductConversion_Index;
                        BinBalance.GoodsReceive_ProductConversion_Id = BinData.GoodsReceive_ProductConversion_Id;
                        BinBalance.GoodsReceive_ProductConversion_Name = BinData.GoodsReceive_ProductConversion_Name;

                        //BinBalance.BinBalance_Ratio = BinData.BinBalance_Ratio;
                        //BinBalance.BinBalance_QtyBegin = model.picking_TotalQty;
                        //BinBalance.BinBalance_WeightBegin = BinCard.BinCard_WeightIn;
                        //BinBalance.BinBalance_VolumeBegin = BinCard.BinCard_VolumeIn;
                        //BinBalance.BinBalance_QtyBal = model.picking_TotalQty;
                        //BinBalance.BinBalance_WeightBal = BinCard.BinCard_WeightIn;
                        //BinBalance.BinBalance_VolumeBal = BinCard.BinCard_VolumeIn;

                        BinBalance.BinBalance_Ratio = BinData.BinBalance_Ratio;
                        BinBalance.BinBalance_QtyBegin = model.picking_TotalQty;
                        BinBalance.BinBalance_WeightBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0); // BinData.BinBalance_WeightBegin;
                        BinBalance.BinBalance_WeightBegin_Index = BinData.BinBalance_WeightBegin_Index;
                        BinBalance.BinBalance_WeightBegin_Id = BinData.BinBalance_WeightBegin_Id;
                        BinBalance.BinBalance_WeightBegin_Name = BinData.BinBalance_WeightBegin_Name;
                        BinBalance.BinBalance_WeightBeginRatio = BinData.BinBalance_WeightBeginRatio;
                        BinBalance.BinBalance_NetWeightBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0); // BinData.BinBalance_NetWeightBegin;
                        BinBalance.BinBalance_NetWeightBegin_Index = BinData.BinBalance_NetWeightBegin_Index;
                        BinBalance.BinBalance_NetWeightBegin_Id = BinData.BinBalance_NetWeightBegin_Id;
                        BinBalance.BinBalance_NetWeightBegin_Name = BinData.BinBalance_NetWeightBegin_Name;
                        BinBalance.BinBalance_NetWeightBeginRatio = BinData.BinBalance_NetWeightBeginRatio;
                        BinBalance.BinBalance_GrsWeightBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0); // BinData.BinBalance_GrsWeightBegin;
                        BinBalance.BinBalance_GrsWeightBegin_Index = BinData.BinBalance_GrsWeightBegin_Index;
                        BinBalance.BinBalance_GrsWeightBegin_Id = BinData.BinBalance_GrsWeightBegin_Id;
                        BinBalance.BinBalance_GrsWeightBegin_Name = BinData.BinBalance_GrsWeightBegin_Name;
                        BinBalance.BinBalance_GrsWeightBeginRatio = BinData.BinBalance_GrsWeightBeginRatio;
                        BinBalance.BinBalance_WidthBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0); // BinData.BinBalance_WidthBegin;
                        BinBalance.BinBalance_WidthBegin_Index = BinData.BinBalance_WidthBegin_Index;
                        BinBalance.BinBalance_WidthBegin_Id = BinData.BinBalance_WidthBegin_Id;
                        BinBalance.BinBalance_WidthBegin_Name = BinData.BinBalance_WidthBegin_Name;
                        BinBalance.BinBalance_WidthBeginRatio = BinData.BinBalance_WidthBeginRatio;
                        BinBalance.BinBalance_LengthBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0); // BinData.BinBalance_LengthBegin;
                        BinBalance.BinBalance_LengthBegin_Index = BinData.BinBalance_LengthBegin_Index;
                        BinBalance.BinBalance_LengthBegin_Id = BinData.BinBalance_LengthBegin_Id;
                        BinBalance.BinBalance_LengthBegin_Name = BinData.BinBalance_LengthBegin_Name;
                        BinBalance.BinBalance_LengthBeginRatio = BinData.BinBalance_LengthBeginRatio;
                        BinBalance.BinBalance_HeightBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0); // BinData.BinBalance_HeightBegin;
                        BinBalance.BinBalance_HeightBegin_Index = BinData.BinBalance_HeightBegin_Index;
                        BinBalance.BinBalance_HeightBegin_Id = BinData.BinBalance_HeightBegin_Id;
                        BinBalance.BinBalance_HeightBegin_Name = BinData.BinBalance_HeightBegin_Name;
                        BinBalance.BinBalance_HeightBeginRatio = BinData.BinBalance_HeightBeginRatio;
                        BinBalance.BinBalance_UnitVolumeBegin = BinData.BinBalance_UnitVolumeBegin;
                        BinBalance.BinBalance_VolumeBegin = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0); // BinData.BinBalance_VolumeBegin;
                        BinBalance.BinBalance_QtyBal = model.picking_TotalQty;
                        BinBalance.BinBalance_UnitWeightBal = BinData.BinBalance_UnitWeightBal;
                        BinBalance.BinBalance_UnitWeightBal_Index = BinData.BinBalance_UnitWeightBal_Index;
                        BinBalance.BinBalance_UnitWeightBal_Id = BinData.BinBalance_UnitWeightBal_Id;
                        BinBalance.BinBalance_UnitWeightBal_Name = BinData.BinBalance_UnitWeightBal_Name;
                        BinBalance.BinBalance_UnitWeightBalRatio = BinData.BinBalance_UnitWeightBalRatio;
                        BinBalance.BinBalance_WeightBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0); // BinData.BinBalance_WeightBal;
                        BinBalance.BinBalance_WeightBal_Index = BinData.BinBalance_WeightBal_Index;
                        BinBalance.BinBalance_WeightBal_Id = BinData.BinBalance_WeightBal_Id;
                        BinBalance.BinBalance_WeightBal_Name = BinData.BinBalance_WeightBal_Name;
                        BinBalance.BinBalance_WeightBalRatio = BinData.BinBalance_WeightBalRatio;
                        BinBalance.BinBalance_UnitNetWeightBal = BinData.BinBalance_UnitNetWeightBal;
                        BinBalance.BinBalance_UnitNetWeightBal_Index = BinData.BinBalance_UnitNetWeightBal_Index;
                        BinBalance.BinBalance_UnitNetWeightBal_Id = BinData.BinBalance_UnitNetWeightBal_Id;
                        BinBalance.BinBalance_UnitNetWeightBal_Name = BinData.BinBalance_UnitNetWeightBal_Name;
                        BinBalance.BinBalance_UnitNetWeightBalRatio = BinData.BinBalance_UnitNetWeightBalRatio;
                        BinBalance.BinBalance_NetWeightBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0); //  BinData.BinBalance_NetWeightBal;
                        BinBalance.BinBalance_NetWeightBal_Index = BinData.BinBalance_NetWeightBal_Index;
                        BinBalance.BinBalance_NetWeightBal_Id = BinData.BinBalance_NetWeightBal_Id;
                        BinBalance.BinBalance_NetWeightBal_Name = BinData.BinBalance_NetWeightBal_Name;
                        BinBalance.BinBalance_NetWeightBalRatio = BinData.BinBalance_NetWeightBalRatio;
                        BinBalance.BinBalance_UnitGrsWeightBal = BinData.BinBalance_UnitGrsWeightBal;
                        BinBalance.BinBalance_UnitGrsWeightBal_Index = BinData.BinBalance_UnitGrsWeightBal_Index;
                        BinBalance.BinBalance_UnitGrsWeightBal_Id = BinData.BinBalance_UnitGrsWeightBal_Id;
                        BinBalance.BinBalance_UnitGrsWeightBal_Name = BinData.BinBalance_UnitGrsWeightBal_Name;
                        BinBalance.BinBalance_UnitGrsWeightBalRatio = BinData.BinBalance_UnitGrsWeightBalRatio;
                        BinBalance.BinBalance_GrsWeightBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0); //  BinData.BinBalance_GrsWeightBal;
                        BinBalance.BinBalance_GrsWeightBal_Index = BinData.BinBalance_GrsWeightBal_Index;
                        BinBalance.BinBalance_GrsWeightBal_Id = BinData.BinBalance_GrsWeightBal_Id;
                        BinBalance.BinBalance_GrsWeightBal_Name = BinData.BinBalance_GrsWeightBal_Name;
                        BinBalance.BinBalance_GrsWeightBalRatio = BinData.BinBalance_GrsWeightBalRatio;
                        BinBalance.BinBalance_UnitWidthBal = BinData.BinBalance_UnitWidthBal;
                        BinBalance.BinBalance_UnitWidthBal_Index = BinData.BinBalance_UnitWidthBal_Index;
                        BinBalance.BinBalance_UnitWidthBal_Id = BinData.BinBalance_UnitWidthBal_Id;
                        BinBalance.BinBalance_UnitWidthBal_Name = BinData.BinBalance_UnitWidthBal_Name;
                        BinBalance.BinBalance_UnitWidthBalRatio = BinData.BinBalance_UnitWidthBalRatio;
                        BinBalance.BinBalance_WidthBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0); //  BinData.BinBalance_WidthBal;
                        BinBalance.BinBalance_WidthBal_Index = BinData.BinBalance_WidthBal_Index;
                        BinBalance.BinBalance_WidthBal_Id = BinData.BinBalance_WidthBal_Id;
                        BinBalance.BinBalance_WidthBal_Name = BinData.BinBalance_WidthBal_Name;
                        BinBalance.BinBalance_WidthBalRatio = BinData.BinBalance_WidthBalRatio;
                        BinBalance.BinBalance_UnitLengthBal = BinData.BinBalance_UnitLengthBal;
                        BinBalance.BinBalance_UnitLengthBal_Index = BinData.BinBalance_UnitLengthBal_Index;
                        BinBalance.BinBalance_UnitLengthBal_Id = BinData.BinBalance_UnitLengthBal_Id;
                        BinBalance.BinBalance_UnitLengthBal_Name = BinData.BinBalance_UnitLengthBal_Name;
                        BinBalance.BinBalance_UnitLengthBalRatio = BinData.BinBalance_UnitLengthBalRatio;
                        BinBalance.BinBalance_LengthBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0); //  BinData.BinBalance_LengthBal;
                        BinBalance.BinBalance_LengthBal_Index = BinData.BinBalance_LengthBal_Index;
                        BinBalance.BinBalance_LengthBal_Id = BinData.BinBalance_LengthBal_Id;
                        BinBalance.BinBalance_LengthBal_Name = BinData.BinBalance_LengthBal_Name;
                        BinBalance.BinBalance_LengthBalRatio = BinData.BinBalance_LengthBalRatio;
                        BinBalance.BinBalance_UnitHeightBal = BinData.BinBalance_UnitHeightBal;
                        BinBalance.BinBalance_UnitHeightBal_Index = BinData.BinBalance_UnitHeightBal_Index;
                        BinBalance.BinBalance_UnitHeightBal_Id = BinData.BinBalance_UnitHeightBal_Id;
                        BinBalance.BinBalance_UnitHeightBal_Name = BinData.BinBalance_UnitHeightBal_Name;
                        BinBalance.BinBalance_UnitHeightBalRatio = BinData.BinBalance_UnitHeightBalRatio;
                        BinBalance.BinBalance_HeightBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0); //  BinData.BinBalance_HeightBal;
                        BinBalance.BinBalance_HeightBal_Index = BinData.BinBalance_HeightBal_Index;
                        BinBalance.BinBalance_HeightBal_Id = BinData.BinBalance_HeightBal_Id;
                        BinBalance.BinBalance_HeightBal_Name = BinData.BinBalance_HeightBal_Name;
                        BinBalance.BinBalance_HeightBalRatio = BinData.BinBalance_HeightBalRatio;
                        BinBalance.BinBalance_UnitVolumeBal = BinData.BinBalance_UnitVolumeBal;
                        BinBalance.BinBalance_VolumeBal = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0); //  BinData.BinBalance_VolumeBal;


                        if (model.isTransfer)
                        {
                            //BinBalance.BinBalance_QtyReserve = 0;
                            //BinBalance.BinBalance_WeightReserve = 0;
                            //BinBalance.BinBalance_VolumeReserve = 0;

                            BinBalance.BinBalance_QtyReserve = 0;
                            BinBalance.BinBalance_WeightReserve = 0; // BinData.BinBalance_WeightReserve;
                            BinBalance.BinBalance_WeightReserve_Index = BinData.BinBalance_WeightReserve_Index;
                            BinBalance.BinBalance_WeightReserve_Id = BinData.BinBalance_WeightReserve_Id;
                            BinBalance.BinBalance_WeightReserve_Name = BinData.BinBalance_WeightReserve_Name;
                            BinBalance.BinBalance_WeightReserveRatio = BinData.BinBalance_WeightReserveRatio;
                            BinBalance.BinBalance_NetWeightReserve = 0; // BinData.BinBalance_NetWeightReserve;
                            BinBalance.BinBalance_NetWeightReserve_Index = BinData.BinBalance_NetWeightReserve_Index;
                            BinBalance.BinBalance_NetWeightReserve_Id = BinData.BinBalance_NetWeightReserve_Id;
                            BinBalance.BinBalance_NetWeightReserve_Name = BinData.BinBalance_NetWeightReserve_Name;
                            BinBalance.BinBalance_NetWeightReserveRatio = BinData.BinBalance_NetWeightReserveRatio;
                            BinBalance.BinBalance_GrsWeightReserve = 0; // BinData.BinBalance_GrsWeightReserve;
                            BinBalance.BinBalance_GrsWeightReserve_Index = BinData.BinBalance_GrsWeightReserve_Index;
                            BinBalance.BinBalance_GrsWeightReserve_Id = BinData.BinBalance_GrsWeightReserve_Id;
                            BinBalance.BinBalance_GrsWeightReserve_Name = BinData.BinBalance_GrsWeightReserve_Name;
                            BinBalance.BinBalance_GrsWeightReserveRatio = BinData.BinBalance_GrsWeightReserveRatio;
                            BinBalance.BinBalance_WidthReserve = 0; // BinData.BinBalance_WidthReserve;
                            BinBalance.BinBalance_WidthReserve_Index = BinData.BinBalance_WidthReserve_Index;
                            BinBalance.BinBalance_WidthReserve_Id = BinData.BinBalance_WidthReserve_Id;
                            BinBalance.BinBalance_WidthReserve_Name = BinData.BinBalance_WidthReserve_Name;
                            BinBalance.BinBalance_WidthReserveRatio = BinData.BinBalance_WidthReserveRatio;
                            BinBalance.BinBalance_LengthReserve = 0; // BinData.BinBalance_LengthReserve;
                            BinBalance.BinBalance_LengthReserve_Index = BinData.BinBalance_LengthReserve_Index;
                            BinBalance.BinBalance_LengthReserve_Id = BinData.BinBalance_LengthReserve_Id;
                            BinBalance.BinBalance_LengthReserve_Name = BinData.BinBalance_LengthReserve_Name;
                            BinBalance.BinBalance_LengthReserveRatio = BinData.BinBalance_LengthReserveRatio;
                            BinBalance.BinBalance_HeightReserve = 0; // BinData.BinBalance_HeightReserve;
                            BinBalance.BinBalance_HeightReserve_Index = BinData.BinBalance_HeightReserve_Index;
                            BinBalance.BinBalance_HeightReserve_Id = BinData.BinBalance_HeightReserve_Id;
                            BinBalance.BinBalance_HeightReserve_Name = BinData.BinBalance_HeightReserve_Name;
                            BinBalance.BinBalance_HeightReserveRatio = BinData.BinBalance_HeightReserveRatio;
                            BinBalance.BinBalance_UnitVolumeReserve = 0;
                            BinBalance.BinBalance_VolumeReserve = 0; // BinData.BinBalance_VolumeReserve;

                            BinBalance.ItemStatus_Index = model.itemStatus_Index_To == null ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(model.itemStatus_Index_To.ToString());//BinData.ItemStatus_Index;
                            BinBalance.ItemStatus_Id = model.itemStatus_Id_To;//BinData.ItemStatus_Id;
                            BinBalance.ItemStatus_Name = model.itemStatus_Name_To;//BinData.ItemStatus_Name;
                        }
                        else
                        {
                            //BinBalance.BinBalance_QtyReserve = model.picking_TotalQty;
                            //BinBalance.BinBalance_WeightReserve = BinData.BinBalance_WeightReserve;
                            //BinBalance.BinBalance_VolumeReserve = BinData.BinBalance_VolumeReserve;

                            BinBalance.BinBalance_QtyReserve = model.picking_TotalQty;
                            BinBalance.BinBalance_WeightReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWeightBal ?? 0); // BinData.BinBalance_WeightReserve;
                            BinBalance.BinBalance_WeightReserve_Index = BinData.BinBalance_WeightReserve_Index;
                            BinBalance.BinBalance_WeightReserve_Id = BinData.BinBalance_WeightReserve_Id;
                            BinBalance.BinBalance_WeightReserve_Name = BinData.BinBalance_WeightReserve_Name;
                            BinBalance.BinBalance_WeightReserveRatio = BinData.BinBalance_WeightReserveRatio;
                            BinBalance.BinBalance_NetWeightReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitNetWeightBal ?? 0); // BinData.BinBalance_NetWeightReserve;
                            BinBalance.BinBalance_NetWeightReserve_Index = BinData.BinBalance_NetWeightReserve_Index;
                            BinBalance.BinBalance_NetWeightReserve_Id = BinData.BinBalance_NetWeightReserve_Id;
                            BinBalance.BinBalance_NetWeightReserve_Name = BinData.BinBalance_NetWeightReserve_Name;
                            BinBalance.BinBalance_NetWeightReserveRatio = BinData.BinBalance_NetWeightReserveRatio;
                            BinBalance.BinBalance_GrsWeightReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitGrsWeightBal ?? 0); // BinData.BinBalance_GrsWeightReserve;
                            BinBalance.BinBalance_GrsWeightReserve_Index = BinData.BinBalance_GrsWeightReserve_Index;
                            BinBalance.BinBalance_GrsWeightReserve_Id = BinData.BinBalance_GrsWeightReserve_Id;
                            BinBalance.BinBalance_GrsWeightReserve_Name = BinData.BinBalance_GrsWeightReserve_Name;
                            BinBalance.BinBalance_GrsWeightReserveRatio = BinData.BinBalance_GrsWeightReserveRatio;
                            BinBalance.BinBalance_WidthReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitWidthBal ?? 0); // BinData.BinBalance_WidthReserve;
                            BinBalance.BinBalance_WidthReserve_Index = BinData.BinBalance_WidthReserve_Index;
                            BinBalance.BinBalance_WidthReserve_Id = BinData.BinBalance_WidthReserve_Id;
                            BinBalance.BinBalance_WidthReserve_Name = BinData.BinBalance_WidthReserve_Name;
                            BinBalance.BinBalance_WidthReserveRatio = BinData.BinBalance_WidthReserveRatio;
                            BinBalance.BinBalance_LengthReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitLengthBal ?? 0); // BinData.BinBalance_LengthReserve;
                            BinBalance.BinBalance_LengthReserve_Index = BinData.BinBalance_LengthReserve_Index;
                            BinBalance.BinBalance_LengthReserve_Id = BinData.BinBalance_LengthReserve_Id;
                            BinBalance.BinBalance_LengthReserve_Name = BinData.BinBalance_LengthReserve_Name;
                            BinBalance.BinBalance_LengthReserveRatio = BinData.BinBalance_LengthReserveRatio;
                            BinBalance.BinBalance_HeightReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitHeightBal ?? 0); // BinData.BinBalance_HeightReserve;
                            BinBalance.BinBalance_HeightReserve_Index = BinData.BinBalance_HeightReserve_Index;
                            BinBalance.BinBalance_HeightReserve_Id = BinData.BinBalance_HeightReserve_Id;
                            BinBalance.BinBalance_HeightReserve_Name = BinData.BinBalance_HeightReserve_Name;
                            BinBalance.BinBalance_HeightReserveRatio = BinData.BinBalance_HeightReserveRatio;
                            BinBalance.BinBalance_UnitVolumeReserve = BinData.BinBalance_UnitVolumeReserve;
                            BinBalance.BinBalance_VolumeReserve = (model.picking_TotalQty ?? 0) * (BinData.BinBalance_UnitVolumeBal ?? 0); // BinData.BinBalance_VolumeReserve;

                        }
                        BinBalance.ProductConversion_Index = BinData.ProductConversion_Index;
                        BinBalance.ProductConversion_Id = BinData.ProductConversion_Id;
                        BinBalance.ProductConversion_Name = BinData.ProductConversion_Name;

                        BinBalance.UnitPrice = BinData.UnitPrice;
                        BinBalance.UnitPrice_Index = BinData.UnitPrice_Index;
                        BinBalance.UnitPrice_Id = BinData.UnitPrice_Id;
                        BinBalance.UnitPrice_Name = BinData.UnitPrice_Name;
                        BinBalance.Price = (model.picking_TotalQty ?? 0) * (BinData.UnitPrice ?? 0);
                        BinBalance.Price_Index = BinData.UnitPrice_Index;
                        BinBalance.Price_Id = BinData.UnitPrice_Id;
                        BinBalance.Price_Name = BinData.UnitPrice_Name;

                        BinBalance.UDF_1 = BinData.UDF_1;
                        BinBalance.UDF_2 = BinData.UDF_2;
                        BinBalance.UDF_3 = BinData.UDF_3;
                        BinBalance.UDF_4 = BinData.UDF_4;
                        BinBalance.UDF_5 = BinData.UDF_5;
                        BinBalance.Create_By = model.userName;
                        BinBalance.Create_Date = DateTime.Now;

                        BinBalance.Invoice_No = BinData.Invoice_No;
                        BinBalance.Declaration_No = BinData.Declaration_No;
                        BinBalance.HS_Code = BinData.HS_Code;
                        BinBalance.Conutry_of_Origin = BinData.Conutry_of_Origin;
                        BinBalance.Tax1 = BinData.Tax1;
                        BinBalance.Tax1_Currency_Index = BinData.Tax1_Currency_Index;
                        BinBalance.Tax1_Currency_Id = BinData.Tax1_Currency_Id;
                        BinBalance.Tax1_Currency_Name = BinData.Tax1_Currency_Name;
                        BinBalance.Tax2 = BinData.Tax2;
                        BinBalance.Tax2_Currency_Index = BinData.Tax2_Currency_Index;
                        BinBalance.Tax2_Currency_Id = BinData.Tax2_Currency_Id;
                        BinBalance.Tax2_Currency_Name = BinData.Tax2_Currency_Name;
                        BinBalance.Tax3 = BinData.Tax3;
                        BinBalance.Tax3_Currency_Index = BinData.Tax3_Currency_Index;
                        BinBalance.Tax3_Currency_Id = BinData.Tax3_Currency_Id;
                        BinBalance.Tax3_Currency_Name = BinData.Tax3_Currency_Name;
                        BinBalance.Tax4 = BinData.Tax4;
                        BinBalance.Tax4_Currency_Index = BinData.Tax4_Currency_Index;
                        BinBalance.Tax4_Currency_Id = BinData.Tax4_Currency_Id;
                        BinBalance.Tax4_Currency_Name = BinData.Tax4_Currency_Name;
                        BinBalance.Tax5 = BinData.Tax5;
                        BinBalance.Tax5_Currency_Index = BinData.Tax5_Currency_Index;
                        BinBalance.Tax5_Currency_Id = BinData.Tax5_Currency_Id;
                        BinBalance.Tax5_Currency_Name = BinData.Tax5_Currency_Name;


                        BinData.BinBalance_QtyBal = (BinData.BinBalance_QtyBal - model.picking_TotalQty);
                        BinData.BinBalance_QtyReserve = (BinData.BinBalance_QtyReserve - model.picking_TotalQty);

                        if (BinData.BinBalance_WeightBegin != 0)
                        {
                            var WeightReserve = (model.picking_TotalQty * BinData.BinBalance_UnitWeightBal);
                            BinData.BinBalance_WeightReserve = BinData.BinBalance_WeightReserve - WeightReserve;
                            BinData.BinBalance_WeightBal = BinData.BinBalance_WeightBal - WeightReserve;
                        }

                        if (BinData.BinBalance_NetWeightBegin != 0)
                        {
                            var NetWeightReserve = (model.picking_TotalQty * BinData.BinBalance_UnitNetWeightBal);
                            BinData.BinBalance_NetWeightReserve = BinData.BinBalance_NetWeightReserve - NetWeightReserve;
                            BinData.BinBalance_NetWeightBal = BinData.BinBalance_NetWeightBal - NetWeightReserve;
                        }


                        if (BinData.BinBalance_GrsWeightBegin != 0)
                        {
                            var GrsWeightReserve = (model.picking_TotalQty * BinData.BinBalance_UnitGrsWeightBal);
                            BinData.BinBalance_GrsWeightReserve = BinData.BinBalance_GrsWeightReserve - GrsWeightReserve;
                            BinData.BinBalance_GrsWeightBal = BinData.BinBalance_GrsWeightBal - GrsWeightReserve;
                        }


                        if (BinData.BinBalance_WidthBegin != 0)
                        {
                            var WidthReserve = (model.picking_TotalQty * BinData.BinBalance_UnitWidthBal);
                            BinData.BinBalance_WidthReserve = BinData.BinBalance_WidthReserve - WidthReserve;
                            BinData.BinBalance_WidthBal = BinData.BinBalance_WidthBal - WidthReserve;
                        }


                        if (BinData.BinBalance_LengthBegin != 0)
                        {
                            var LengthReserve = (model.picking_TotalQty * BinData.BinBalance_UnitLengthBal);
                            BinData.BinBalance_LengthReserve = BinData.BinBalance_LengthReserve - LengthReserve;
                            BinData.BinBalance_LengthBal = BinData.BinBalance_LengthBal - LengthReserve;
                        }


                        if (BinData.BinBalance_HeightBegin != 0)
                        {
                            var HeightReserve = (model.picking_TotalQty * BinData.BinBalance_UnitHeightBal);
                            BinData.BinBalance_HeightReserve = BinData.BinBalance_HeightReserve - HeightReserve;
                            BinData.BinBalance_HeightBal = BinData.BinBalance_HeightBal - HeightReserve;
                        }

                        if (BinData.BinBalance_VolumeBegin != 0)
                        {
                            var VolReserve = (model.picking_TotalQty * BinData.BinBalance_UnitVolumeBal);
                            BinData.BinBalance_VolumeReserve = BinData.BinBalance_VolumeReserve - VolReserve;
                            BinData.BinBalance_VolumeBal = BinData.BinBalance_VolumeBal - VolReserve;
                        }

                        if ((BinData.UnitPrice ?? 0) != 0)
                        {
                            var VoltPrice = (model.picking_TotalQty * BinData.UnitPrice);
                            BinData.Price = BinData.Price - VoltPrice;
                        }
                    }

                }


                State = "s.SaveChanges";
                var transaction = dbBinbalance.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    dbBinbalance.wm_BinCard.AddRange(listBinCard);
                    if (IsInsert)
                    {
                        dbBinbalance.wm_BinBalance.Add(BinBalance);
                    }
                    dbBinbalance.SaveChanges();
                    transaction.Commit();


                    State = "E.SaveChanges";
                    olog.logging("InsertBinCard", State);
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("InsertBinCard", msglog);
                    transaction.Rollback();
                    throw exy;
                }

                return guid;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("InsertBinCard", msglog);
                return "";
            }
        }
    }
}
