using AspNetCore.Reporting;
using Business.Library;
using Comone.Utils;
using DataAccess;
using GIBusiness.GoodIssue;
using GIBusiness.Reports;
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
using static GIBusiness.Result;

namespace GIBusiness.TagOut
{
    public class TagOutitemService
    {
        private GIDbContext db;

        public TagOutitemService()
        {
            db = new GIDbContext();
        }
        public TagOutitemService(GIDbContext db)
        {
            this.db = db;
        }

        #region ScantagOutitem
        public Result ScantagOutitem(TagOutitemViewModel model)
        {
            try
            {
                var result = new Result();
                wm_TagOutItem query = db.WM_TagOutItem.FirstOrDefault(c => c.TagOut_No == model.tagOut_No && c.TagOut_Status == 0);
                if (query == null)
                {
                    result.resultIsUse = false;
                    result.resultMsg = "ไม่พบ tag ที่ค้นหา";
                }
                else {
                    result.resultMsg = query.TagOut_Index.ToString();
                    result.resultIsUse = true;
                }

                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region scanBarcode_no
        public Result scanBarcode_no(TagOutitemViewModel model)
        {
            try
            {
                var result = new Result();
                var resultBarcode = new List<BarcodeViewModel>();

                var filterModel = new { productConversionBarcode = model.product_barcode };

                resultBarcode = utils.SendDataApi<List<BarcodeViewModel>>(new AppSettingConfig().GetUrl("configBarcode"), filterModel.sJson());
                if (resultBarcode.Count() <= 0)
                {
                    result.resultIsUse = false;
                    result.resultMsg = "MSG_Alert_Barcode_Not_Found";
                }
                else {
                    wm_TagOutItem query = db.WM_TagOutItem.FirstOrDefault(c => c.TagOut_No == model.tagOut_No && c.TagOut_Status == 0 && c.Product_Index == resultBarcode[0].product_Index);
                    if (query == null)
                    {
                        result.resultIsUse = false;
                        result.resultMsg = "BarCode not match";
                    }
                    else {
                        result.resultMsg = query.TagOutItem_Index.ToString();
                        result.resultIsUse = true;
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

        #region confirmPicktoLight

        #region OLD
        //public Result confirmPicktoLight(TagOutitemViewModel model)
        //{
        //    try
        //    {
        //        db.Database.SetCommandTimeout(360);
        //        var result = new Result();
        //        var tagoutitem = db.WM_TagOutItem.Where(c => c.TagOut_Index == model.tagOut_Index && c.TagOut_Status == 0).ToList();

        //        if (tagoutitem.Count > 0)
        //        {
        //            var tote = new { toteID = model.tagOut_No };
        //            var resultScanPick = utils.SendDataApi<TagOutitemViewModel>(new AppSettingConfig().GetUrl("picking"), tote.sJson());
        //            if (resultScanPick.status == "10")
        //            {
        //                foreach (var item in tagoutitem)
        //                {
        //                    item.TagOut_Status = 1;
        //                    item.Update_By = model.create_By;
        //                    item.Update_Date = DateTime.Now;

        //                    var transaction = db.Database.BeginTransaction();
        //                    try
        //                    {
        //                        db.SaveChanges();
        //                        transaction.Commit();
        //                        result.resultIsUse = true;
        //                        result.resultMsg = item.TagOut_Index.ToString();
        //                    }

        //                    catch (Exception exy)
        //                    {
        //                        result.resultIsUse = false;
        //                        result.resultMsg = "ไม่สามารถบันทึกได้";
        //                        transaction.Rollback();
        //                        throw exy;
        //                    }
                            
        //                }

        //                var get_plan = db.WM_TagOutItem.Where(c => c.TagOutRef_No4 == tagoutitem[0].TagOutRef_No4 && !string.IsNullOrEmpty(c.TagOutRef_No2) && c.TagOut_Status == 0).ToList();
        //                if (get_plan.Count > 0)
        //                {
        //                    return result;
        //                }
        //                else
        //                {
        //                    List<Guid?> get_cuting = db.WM_TagOutItem.Where(c => c.TagOutRef_No4 == tagoutitem[0].TagOutRef_No4 && !string.IsNullOrEmpty(c.TagOutRef_No2) && c.TagOut_Status == 1)
        //                        .GroupBy(c => c.GoodsIssueItemLocation_Index).Select(c => c.Key).ToList();

        //                    var get_taskitem = db.View_TaskInsertBinCard.Where(c => get_cuting.Contains(c.GoodsIssueItemLocation_Index)).ToList();
        //                    if (get_taskitem.Count > 0)
        //                    {
        //                        var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = "WH7TOASRS" } };
        //                        var tag = new DocumentViewModel();
        //                        tag.listDocumentViewModel = listTag;
        //                        var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
        //                        if (CheckTag.Count() == 0)
        //                        {
        //                            var tag2 = new { tag_no = "WH7TOASRS" };
        //                            var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
        //                            CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
        //                        }

        //                        var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());
        //                        foreach (var d in get_taskitem)
        //                        {
        //                            var View_TaskInsertBinCard = new BinCardViewModel
        //                            {
        //                                taskitem_Index = d.Taskitem_Index,
        //                                task_Index = d.Task_Index,
        //                                task_No = d.Task_No,
        //                                ref_Document_Index = d.Ref_Document_Index,
        //                                ref_DocumentItem_Index = d.Ref_DocumentItem_Index,
        //                                ref_Document_No = d.Ref_Document_No,
        //                                tagOutItem_Index = d.TagOutItem_Index,
        //                                tagOut_Index = d.TagOut_Index,
        //                                tagOut_No = d.TagOut_No,
        //                                goodsIssue_Date = d.GoodsIssue_Date,
        //                                documentType_Index = d.DocumentType_Index,
        //                                documentType_Id = d.DocumentType_Id,
        //                                documentType_Name = d.DocumentType_Name,
        //                                tagItem_Index = d.TagItem_Index,
        //                                tag_Index = d.Tag_Index,
        //                                tag_No = d.Tag_No,
        //                                tag_Index_To = CheckTag.FirstOrDefault()?.tag_Index,
        //                                tag_No_To = CheckTag.FirstOrDefault()?.tag_No,
        //                                product_Index = d.Product_Index,
        //                                product_Id = d.Product_Id,
        //                                product_Name = d.Product_Name,
        //                                product_SecondName = d.Product_SecondName,
        //                                product_ThirdName = d.Product_ThirdName,
        //                                product_Lot = d.Product_Lot,
        //                                itemStatus_Index = d.ItemStatus_Index,
        //                                itemStatus_Id = d.ItemStatus_Id,
        //                                itemStatus_Name = d.ItemStatus_Name,
        //                                itemStatus_Index_To = d.ItemStatus_Index,
        //                                itemStatus_Id_To = d.ItemStatus_Id,
        //                                itemStatus_Name_To = d.ItemStatus_Name,
        //                                productConversion_Index = d.ProductConversion_Index,
        //                                productConversion_Id = d.ProductConversion_Id,
        //                                productConversion_Name = d.ProductConversion_Name,
        //                                owner_Index = d.Owner_Index,
        //                                owner_Id = d.Owner_Id,
        //                                owner_Name = d.Owner_Name,
        //                                location_Index = d.Location_Index,
        //                                location_Id = d.Location_Id,
        //                                location_Name = d.Location_Name,
        //                                exp_Date = d.EXP_Date,
        //                                mfg_Date = d.MFG_Date,
        //                                udf_1 = d.UDF_1,
        //                                udf_2 = d.UDF_2,
        //                                udf_3 = d.UDF_3,
        //                                udf_4 = d.UDF_4,
        //                                udf_5 = d.UDF_5,
        //                                picking_Qty = d.Qty,
        //                                picking_Ratio = d.Ratio,
        //                                picking_TotalQty = d.TotalQty,
        //                                binBalance_Index = d.BinBalance_Index,
        //                                process_Index = new Guid(configLocation),
        //                                location_Index_To = Guid.Parse("DF3D2410-FCBD-4C82-9F2E-65EE51A83F51"),
        //                                location_Id_To = "WH7TOASRS",
        //                                location_Name_To = "WH7TOASRS",
        //                                Volume = d.Volume,
        //                                Weight = d.Weight,
        //                                userName = model.create_By
        //                            };
        //                            //var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("insertBincard"), View_TaskInsertBinCard.sJson());
        //                            var BinbalanceCutService = new BinbalanceCutService();
        //                            var Bincard = BinbalanceCutService.InsertBinCard(View_TaskInsertBinCard);
        //                            if (!string.IsNullOrEmpty(Bincard))
        //                            {
        //                                var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);
        //                                taskitem.Picking_Status = 2;
        //                                taskitem.UDF_5 = "WH7TOASRS";
        //                                taskitem.Picking_Qty = d.Qty;
        //                                taskitem.Picking_Ratio = d.Ratio;
        //                                taskitem.Picking_TotalQty = d.TotalQty;
        //                                taskitem.Pick_ProductConversion_Index = d.ProductConversion_Index;
        //                                taskitem.Pick_ProductConversion_Id = d.ProductConversion_Id;
        //                                taskitem.Pick_ProductConversion_Name = d.ProductConversion_Name;
        //                                taskitem.Update_Date = DateTime.Now;
        //                                taskitem.Update_By = model.create_By;
        //                                taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
        //                                var transactionXX = db.Database.BeginTransaction();
        //                                try
        //                                {
        //                                    db.SaveChanges();
        //                                    transactionXX.Commit();
        //                                }

        //                                catch (Exception exy)
        //                                {
                                          
        //                                    transactionXX.Rollback();
        //                                    throw exy;
        //                                }

        //                                var update_s = db.WM_TagOutItem.Where(c => c.GoodsIssueItemLocation_Index == d.GoodsIssueItemLocation_Index && !string.IsNullOrEmpty(c.TagOutRef_No2) && c.TagOut_Status == 1).ToList();
        //                                foreach (var item in update_s)
        //                                {
        //                                    item.TagOut_Status = 2;
        //                                    item.Update_By = model.create_By;
        //                                    item.Update_Date = DateTime.Now;

        //                                    var transaction = db.Database.BeginTransaction();
        //                                    try
        //                                    {
        //                                        db.SaveChanges();
        //                                        transaction.Commit();
        //                                        result.resultIsUse = true;
        //                                        result.resultMsg = model.tagOut_Index.ToString();
        //                                    }

        //                                    catch (Exception exy)
        //                                    {
        //                                        result.resultIsUse = false;
        //                                        result.resultMsg = "ไม่สามารถบันทึกได้";
        //                                        transaction.Rollback();
        //                                        throw exy;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //            else {
        //                result.resultIsUse = false;
        //                result.resultMsg = resultScanPick.message.description;
        //                var list = new List<PTLPickingModel>();
        //                foreach (var item in resultScanPick.message.models)
        //                {
        //                    var set_value = new PTLPickingModel();
        //                    set_value.product_Id = item.productId;
        //                    set_value.product_Name = item.productName;
        //                    set_value.qty = item.quantity;
        //                    set_value.status = "Wait for pick";
        //                    set_value.productConversion_Name = item.productConversionName;
        //                    list.Add(set_value);
        //                }
        //                result.models = list;
        //            }
                    
        //        }
                


        //        return result;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        #endregion

        #region New
        public Result confirmPicktoLight(TagOutitemViewModel model)
        {
            try
            {
                db.Database.SetCommandTimeout(360);
                var result = new Result();
                var tagoutitem = db.WM_TagOutItem.Where(c => c.TagOut_Index == model.tagOut_Index && c.TagOut_Status == 0).ToList();

                if (tagoutitem.Count > 0)
                {
                    var tote = new { toteID = model.tagOut_No };

                    foreach (var item in tagoutitem)
                    {
                        item.TagOut_Status = 1;
                        item.Update_By = model.create_By;
                        item.Update_Date = DateTime.Now;

                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            result.resultIsUse = true;
                            result.resultMsg = item.TagOut_Index.ToString();
                        }

                        catch (Exception exy)
                        {
                            result.resultIsUse = false;
                            result.resultMsg = "ไม่สามารถบันทึกได้";
                            transaction.Rollback();
                            throw exy;
                        }

                    }

                    var get_plan = db.WM_TagOutItem.Where(c => c.TagOutRef_No4 == tagoutitem[0].TagOutRef_No4 && !string.IsNullOrEmpty(c.TagOutRef_No2) && c.TagOut_Status == 0).ToList();
                    if (get_plan.Count > 0)
                    {
                        return result;
                    }
                    else
                    {
                        List<Guid?> get_cuting = db.WM_TagOutItem.Where(c => c.TagOutRef_No4 == tagoutitem[0].TagOutRef_No4 && !string.IsNullOrEmpty(c.TagOutRef_No2) && c.TagOut_Status == 1)
                            .GroupBy(c => c.GoodsIssueItemLocation_Index).Select(c => c.Key).ToList();

                        var taskItems = db.IM_TaskItem.Where(c => get_cuting.Contains(c.Ref_DocumentItem_Index)).ToList();

                        foreach (var item in taskItems)
                        {
                            item.flag_picktolight = 1;
                            item.flag_picktolight_by = model.create_By;
                            item.flag_picktolight_Date = DateTime.Now;
                        }

                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            result.resultIsUse = true;
                        }

                        catch (Exception exy)
                        {
                            result.resultIsUse = false;
                            result.resultMsg = "ไม่สามารถบันทึกได้";
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

        #region task Cut
        public Result confirmPicktoLight_CUT()
        {
            try
            {
                db.Database.SetCommandTimeout(360);
                var result = new Result();

                var get_taskitem = db.View_TaskInsertBinCard_PickTiLight.Where(c => c.flag_picktolight == 1).ToList();

                var task_picktilight = get_taskitem.GroupBy(c => c.Ref_DocumentItem_Index).Select(c => c.Key).ToList();
                var taskItems = db.IM_TaskItem.Where(c => c.flag_picktolight == 1 && task_picktilight.Contains(c.Ref_DocumentItem_Index)).ToList();
                foreach (var item in taskItems)
                {
                    item.flag_picktolight = 2;
                    //item.flag_picktolight_by = model.create_By;
                    item.flag_picktolight_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        result.resultIsUse = true;
                        result.resultMsg = item.TagOut_Index.ToString();
                    }

                    catch (Exception exy)
                    {
                        result.resultIsUse = false;
                        result.resultMsg = "ไม่สามารถบันทึกได้";
                        transaction.Rollback();
                        throw exy;
                    }
                }

                if (get_taskitem.Count > 0)
                {
                    var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = "WH7TOASRS" } };
                    var tag = new DocumentViewModel();
                    tag.listDocumentViewModel = listTag;
                    var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                    if (CheckTag.Count() == 0)
                    {
                        var tag2 = new { tag_no = "WH7TOASRS" };
                        var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
                        CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                    }

                    var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());
                    foreach (var d in get_taskitem)
                    {
                        var View_TaskInsertBinCard = new BinCardViewModel
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
                            tag_No_To = CheckTag.FirstOrDefault()?.tag_No,
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
                            picking_Qty = d.Qty,
                            picking_Ratio = d.Ratio,
                            picking_TotalQty = d.TotalQty,
                            binBalance_Index = d.BinBalance_Index,
                            process_Index = new Guid(configLocation),
                            location_Index_To = Guid.Parse("DF3D2410-FCBD-4C82-9F2E-65EE51A83F51"),
                            location_Id_To = "WH7TOASRS",
                            location_Name_To = "WH7TOASRS",
                            Volume = d.Volume,
                            Weight = d.Weight,
                            userName = "System"
                        };
                        //var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("insertBincard"), View_TaskInsertBinCard.sJson());
                        var BinbalanceCutService = new BinbalanceCutService();
                        var Bincard = BinbalanceCutService.InsertBinCard(View_TaskInsertBinCard);
                        if (!string.IsNullOrEmpty(Bincard))
                        {
                            var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);
                            taskitem.Picking_Status = 2;
                            taskitem.flag_picktolight = 3;
                            taskitem.UDF_5 = "WH7TOASRS";
                            taskitem.Picking_Qty = d.Qty;
                            taskitem.Picking_Ratio = d.Ratio;
                            taskitem.Picking_TotalQty = d.TotalQty;
                            taskitem.Pick_ProductConversion_Index = d.ProductConversion_Index;
                            taskitem.Pick_ProductConversion_Id = d.ProductConversion_Id;
                            taskitem.Pick_ProductConversion_Name = d.ProductConversion_Name;
                            taskitem.Update_Date = DateTime.Now;
                            //taskitem.Update_By = model.create_By;
                            taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                            var transactionXX = db.Database.BeginTransaction();
                            try
                            {
                                db.SaveChanges();
                                transactionXX.Commit();
                            }

                            catch (Exception exy)
                            {

                                transactionXX.Rollback();
                                throw exy;
                            }

                            var update_s = db.WM_TagOutItem.Where(c => c.GoodsIssueItemLocation_Index == d.GoodsIssueItemLocation_Index && !string.IsNullOrEmpty(c.TagOutRef_No2) && c.TagOut_Status == 1).ToList();
                            foreach (var item in update_s)
                            {
                                item.TagOut_Status = 2;
                                //item.Update_By = model.create_By;
                                item.Update_Date = DateTime.Now;

                                var transaction = db.Database.BeginTransaction();
                                try
                                {
                                    db.SaveChanges();
                                    transaction.Commit();
                                    result.resultIsUse = true;
                                    result.resultMsg = "";
                                }

                                catch (Exception exy)
                                {
                                    result.resultIsUse = false;
                                    result.resultMsg = "ไม่สามารถบันทึกได้";
                                    transaction.Rollback();
                                    throw exy;
                                }
                            }
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

        #endregion

        #region DetailScanPicktolight
        public List<TagOutitemViewModel> DetailScanPicktolight(TagOutitemViewModel model)
        {
            try
            {
                var checksorter = false;
                var result = new List<TagOutitemViewModel>();
                var detail = db.WM_TagOutItem.Where(c => c.TagOut_Index == Guid.Parse(model.resultMsg) && c.TagOut_Status != -1 ).ToList();
                var get_group = detail.GroupBy(c => new
                {
                    c.Product_Index,
                    c.Product_Id,
                    c.Product_Name,
                    c.TagOut_Status,
                    c.ProductConversion_Name,
                    c.TagOutRef_No2
                }).Select(c => new
                {
                    c.Key.Product_Index,
                    c.Key.Product_Id,
                    c.Key.Product_Name,
                    c.Key.TagOut_Status,
                    c.Key.ProductConversion_Name,
                    c.Key.TagOutRef_No2,
                    Qty = c.Sum(x => x.Qty)
                }).ToList();

                List<ProductViewModel> productList = new List<ProductViewModel>();
                foreach (var item in get_group)
                {
                    ProductViewModel getproduct = new ProductViewModel();
                    getproduct.product_Index = item.Product_Index;
                    productList.Add(getproduct);
                }

                var list = new {
                    listProductViewModel = productList
                };

                var Product = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), list.sJson());

                foreach (var item in Product)
                {
                    if (!checksorter)
                    {
                        if (!string.IsNullOrEmpty(item.UDF_2))
                        {
                            checksorter = true;
                        }
                    }
                }
                foreach (var item in get_group)
                {
                    var resultitem = new TagOutitemViewModel();
                    if (checksorter)
                    {
                        resultitem.resultCheckSorter = true;
                    }
                    resultitem.product_Id = item.Product_Id;
                    resultitem.product_Name = item.Product_Name;
                    resultitem.status = item.TagOut_Status == 0 ? "Wait for a scan" : "Scaned";
                    resultitem.qty = item.Qty;
                    resultitem.productConversion_Name = item.ProductConversion_Name;
                    resultitem.size = item.TagOutRef_No2;
                    resultitem.total_qty = get_group.Select(c=> c.Qty).Sum();
                    result.Add(resultitem);
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

    }
}
