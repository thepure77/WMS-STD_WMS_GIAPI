using AspNetCore.Reporting;
using Business.Library;
using Comone.Utils;
using DataAccess;
using GIBusiness.GoodIssue;
using GIBusiness.Reports;
using GIBusiness.TrackingLoading;
using GIDataAccess.Models;
using MasterBusiness.GoodsIssue;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using PlanGIBusiness.Libs;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace GIBusiness.TagOut
{
    public class ReturnToWBService
    {
        private GIDbContext db;

        public ReturnToWBService()
        {
            db = new GIDbContext();
        }
        public ReturnToWBService(GIDbContext db)
        {
            this.db = db;
        }

        #region scanorder
        public Result scanorder(ReturnToWBViewModel model)
        {
            try
            {
                var result = new Result();
                var scan_do = db.im_TruckLoadItem.FirstOrDefault(c => c.PlanGoodsIssue_No == model.planGI && c.Document_Status == 2);
                if (scan_do == null)
                {
                    result.resultIsUse = false;
                    result.resultMsg = "ไม่พบ Order ที่ทำการแสกน";
                }
                else {
                    result.resultIsUse = true;
                    result.resultMsg = scan_do.PlanGoodsIssue_Index.ToString();
                }
                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Scan Location
        public List<locationViewModel> ScanLocaton(ReturnToWBViewModel model)
        {
            try
            {
                var objectLocation = new { location_Name = model.location};
                var location = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), objectLocation.sJson());

                return location;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region saveorderlocation
        public Result saveorderlocation(ReturnToWBViewModel model)
        {
            try
            {
                var result = new Result();
                var taskitemLoad = db.View_TaskInsertBinCardWithLoad.Where(c => c.PlanGoodsIssue_Index == model.planGoodsIssue_Index).ToList();
                var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());
                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = model.location_name } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;
                var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                if (CheckTag.Count() == 0)
                {
                    var tag2 = new { tag_no = model.location_name };
                    var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
                    CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                }
                foreach (var item in taskitemLoad)
                {
                    var View_TaskInsertBinCard = new View_TaskInsertBinCardViewModel
                    {
                        taskitem_Index = item.Taskitem_Index,
                        task_Index = item.Task_Index,
                        task_No = item.Task_No,
                        ref_Document_Index = item.Ref_Document_Index,
                        ref_DocumentItem_Index = item.Ref_DocumentItem_Index,
                        ref_Document_No = item.Ref_Document_No,
                        tagOutItem_Index = item.TagOutItem_Index,
                        tagOut_Index = item.TagOut_Index,
                        tagOut_No = item.TagOut_No,
                        goodsIssue_Date = item.GoodsIssue_Date,
                        documentType_Index = item.DocumentType_Index,
                        documentType_Id = item.DocumentType_Id,
                        documentType_Name = item.DocumentType_Name,
                        tagItem_Index = item.TagItem_Index,
                        tag_Index = item.Tag_Index,
                        tag_No = item.Tag_No,
                        tag_Index_To = CheckTag.FirstOrDefault()?.tag_Index,
                        tag_No_To = model.location_id,
                        product_Index = item.Product_Index,
                        product_Id = item.Product_Id,
                        product_Name = item.Product_Name,
                        product_SecondName = item.Product_SecondName,
                        product_ThirdName = item.Product_ThirdName,
                        product_Lot = item.Product_Lot,
                        itemStatus_Index = item.ItemStatus_Index,
                        itemStatus_Id = item.ItemStatus_Id,
                        itemStatus_Name = item.ItemStatus_Name,
                        itemStatus_Index_To = item.ItemStatus_Index,
                        itemStatus_Id_To = item.ItemStatus_Id,
                        itemStatus_Name_To = item.ItemStatus_Name,
                        productConversion_Index = item.ProductConversion_Index,
                        productConversion_Id = item.ProductConversion_Id,
                        productConversion_Name = item.ProductConversion_Name,
                        owner_Index = item.Owner_Index,
                        owner_Id = item.Owner_Id,
                        owner_Name = item.Owner_Name,
                        location_Index = item.Location_Index,
                        location_Id = item.Location_Id,
                        location_Name = item.Location_Name,
                        exp_Date = item.EXP_Date,
                        mfg_Date = item.MFG_Date,
                        udf_1 = item.UDF_1,
                        udf_2 = item.UDF_2,
                        udf_3 = item.UDF_3,
                        udf_4 = item.UDF_4,
                        udf_5 = item.UDF_5,
                        picking_Qty = item.Picking_Qty,
                        picking_Ratio = item.Picking_Ratio,
                        picking_TotalQty = item.Picking_TotalQty,
                        binBalance_Index = item.BinBalance_Index_New,
                        process_Index = new Guid(configLocation),
                        location_Index_To = Guid.Parse(model.location_index),
                        location_Id_To = model.location_id,
                        location_Name_To = model.location_name,
                        Volume = item.Volume,
                        Weight = item.Weight,
                        userName = model.userName,
                        isScanPick = true,
                        isScanToDock = true

                    };
                    var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardBy_Scanpick"), View_TaskInsertBinCard.sJson());
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var bin_index = new { binbalance_Index = Bincard };
                        var findBin = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), bin_index.sJson());

                        var taskitem = db.IM_TaskItem.Find(item.Taskitem_Index);
                        taskitem.PickingToDock_Status = 2;
                        taskitem.UDF_5 = model.location_name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        taskitem.Tag_Index = findBin.tag_Index;
                        taskitem.Tag_No = findBin.tag_No;
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            //msglog = State + " ex Rollback " + exy.Message.ToString();
                            //olog.logging("ScanConfirmLocatonPick", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Scan Confirm Location ScanPick To Dock
        public bool ScanConfirmLocatonPickingtoDock(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());

                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = model.confirm_location_Name } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;
                var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                if (CheckTag.Count() == 0)
                {
                    var tag2 = new { tag_no = model.confirm_location_Name };
                    var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
                    CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                }


                var task_Index = new Guid(model.task_Index);
                var data = db.View_TaskInsertBinCard_V2.Where(c => c.Task_Index == task_Index && c.PickingLabeling_Status == 2 && c.PickingToDock_Status == 1).ToList();
                foreach (var d in data)
                {
                    var View_TaskInsertBinCard = new View_TaskInsertBinCardViewModel
                    {
                        taskitem_Index = d.Taskitem_Index,
                        task_Index = d.Task_Index,
                        task_No = d.Task_No,
                        ref_Document_Index = d.Ref_Document_Index,
                        ref_DocumentItem_Index = d.Ref_DocumentItem_Index,
                        ref_Document_No = d.Ref_Document_No,
                        tagOutItem_Index = d.TagOutItem_Index,
                        tagOut_Index = d.TagOut_Index,
                        tagOut_No = d.TagOut_No,
                        goodsIssue_Date = d.GoodsIssue_Date,
                        documentType_Index = d.DocumentType_Index,
                        documentType_Id = d.DocumentType_Id,
                        documentType_Name = d.DocumentType_Name,
                        tagItem_Index = d.TagItem_Index,
                        tag_Index = d.Tag_Index,
                        tag_No = d.Tag_No,
                        tag_Index_To = CheckTag.FirstOrDefault()?.tag_Index,
                        tag_No_To = model.confirm_location_Id,
                        product_Index = d.Product_Index,
                        product_Id = d.Product_Id,
                        product_Name = d.Product_Name,
                        product_SecondName = d.Product_SecondName,
                        product_ThirdName = d.Product_ThirdName,
                        product_Lot = d.Product_Lot,
                        itemStatus_Index = d.ItemStatus_Index,
                        itemStatus_Id = d.ItemStatus_Id,
                        itemStatus_Name = d.ItemStatus_Name,
                        itemStatus_Index_To = d.ItemStatus_Index,
                        itemStatus_Id_To = d.ItemStatus_Id,
                        itemStatus_Name_To = d.ItemStatus_Name,
                        productConversion_Index = d.ProductConversion_Index,
                        productConversion_Id = d.ProductConversion_Id,
                        productConversion_Name = d.ProductConversion_Name,
                        owner_Index = d.Owner_Index,
                        owner_Id = d.Owner_Id,
                        owner_Name = d.Owner_Name,
                        location_Index = d.Location_Index,
                        location_Id = d.Location_Id,
                        location_Name = d.Location_Name,
                        exp_Date = d.EXP_Date,
                        mfg_Date = d.MFG_Date,
                        udf_1 = d.UDF_1,
                        udf_2 = d.UDF_2,
                        udf_3 = d.UDF_3,
                        udf_4 = d.UDF_4,
                        udf_5 = d.UDF_5,
                        picking_Qty = d.Picking_Qty,
                        picking_Ratio = d.Picking_Ratio,
                        picking_TotalQty = d.Picking_TotalQty,
                        binBalance_Index = d.BinBalance_Index_New,
                        process_Index = new Guid(configLocation),
                        location_Index_To = new Guid(model.confirm_location_Index),
                        location_Id_To = model.confirm_location_Id,
                        location_Name_To = model.confirm_location_Name,
                        Volume = d.Volume,
                        Weight = d.Weight,
                        userName = model.userName,
                        isScanPick = true,
                        isScanToDock = true

                    };
                    var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardBy_Scanpick"), View_TaskInsertBinCard.sJson());
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var bin_index = new { binbalance_Index = Bincard };
                        var findBin = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), bin_index.sJson());

                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);
                        taskitem.PickingToDock_Status = 2;
                        taskitem.UDF_5 = model.confirm_location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        //taskitem.DocumentRef_No4 = taskitem.Tag_No;
                        taskitem.Tag_Index = findBin.tag_Index;
                        taskitem.Tag_No = findBin.tag_No;

                        //var sqlCMD_Bin = "Update WMSDB_AMZ_Binbalance_V3..wm_BinBalance set " +
                        //   "BinBalance_QtyReserve = " + View_TaskInsertBinCard.picking_Qty + " " +
                        //   "where Binbalance_Index = '" + Bincard + "'";


                        //var transactionX_binnew = db.Database.BeginTransaction();
                        //try
                        //{
                        //    db.Database.ExecuteSqlCommand(sqlCMD_Bin);
                        //    db.SaveChanges();
                        //    transactionX_binnew.Commit();
                        //}
                        //catch (Exception exy)
                        //{
                        //    msglog = State + " ex Rollback " + exy.Message.ToString();
                        //    olog.logging("ScanConfirmLocatonPick", msglog);
                        //    transactionX_binnew.Rollback();
                        //    throw exy;
                        //}
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("ScanConfirmLocatonPick", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                var chkTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == task_Index && (c.PickingToDock_Status == 1 || (c.PickingToDock_Status != null ? c.PickingToDock_Status : 0) == 0)).Count();
                if (chkTaskItem == 0)
                {
                    var task = db.IM_Task.Find(task_Index);

                    var taskitem = db.IM_TaskItem.Where(c => c.Task_Index == task.Task_Index && c.PickingToDock_Status == 2).GroupBy(g => g.Ref_Document_Index);
                    var GI = db.IM_GoodsIssue.Where(c => taskitem.Select(s => s.Key).Contains(c.GoodsIssue_Index)).ToList();
                    foreach (var gi in GI)
                    {
                        gi.Document_Status = 3;
                    }


                    task.Document_StatusDocktoStg = 2;
                    task.Update_Date = DateTime.Now;
                    task.Update_By = model.userName;
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("ScanConfirmLocatonPick", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirmLocatonPick", msglog);
                throw new Exception(ex.Message);
            }
        }
        #endregion


    }
}
