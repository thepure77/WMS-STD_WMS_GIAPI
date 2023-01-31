using Business.Library;
using Comone.Utils;
using DataAccess;
using GIDataAccess.Models;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using static GIBusiness.GoodIssue.TaskfilterViewModel;

namespace GIBusiness.GoodIssue
{
    public class ScanPickItemExportService
    {
        private GIDbContext db;
        private MasterDbContext dbMaster;

        public ScanPickItemExportService()
        {
            db = new GIDbContext();
            dbMaster = new MasterDbContext();
        }
        public ScanPickItemExportService(GIDbContext db, MasterDbContext dbMaster)
        {
            this.db = db;
            this.dbMaster = dbMaster;
        }

        #region FilterOld
        public actionResultScanPicksearchViewModel GetDataScanTaskItemOld(ScanPicksearchViewModel model)
        {
            try
            {
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskItemViewModel>();

                var query = db.IM_TaskItem.Where(c => c.Task_Index == new Guid(model.task_Index) && (c.Picking_Status == null ? 0 : c.Picking_Status) == 0).ToList();

                foreach (var q in query)
                {
                    //var taskItem = db.IM_TaskItem.FirstOrDefault(c => c.Task_Index == q.Task_Index);
                    var item = new taskItemViewModel
                    {
                        taskItem_Index = q.TaskItem_Index.ToString(),
                        task_Index = q.Task_Index.ToString(),
                        task_No = q.Task_No,
                        lineNum = q.LineNum,
                        tagItem_Index = q.TagItem_Index,
                        tag_Index = q.Tag_Index,
                        tag_No = q.Tag_No,
                        product_Index = q.Product_Index,
                        product_Id = q.Product_Id,
                        product_Name = q.Product_Name,
                        product_SecondName = q.Product_SecondName,
                        product_ThirdName = q.Product_ThirdName,
                        product_Lot = q.Product_Lot,
                        itemStatus_Index = q.ItemStatus_Index,
                        itemStatus_Id = q.ItemStatus_Id,
                        itemStatus_Name = q.ItemStatus_Name,
                        location_Index = q.Location_Index,
                        location_Id = q.Location_Id,
                        location_Name = q.Location_Name,
                        qty = q.Qty,
                        ratio = q.Ratio,
                        totalQty = q.TotalQty,
                        productConversion_Index = q.ProductConversion_Index,
                        productConversion_Id = q.ProductConversion_Id,
                        productConversion_Name = q.ProductConversion_Name,
                        mfg_Date = q.MFG_Date,
                        exp_Date = q.EXP_Date,
                        unitWeight = q.UnitWeight,
                        weight = q.Weight,
                        unitWidth = q.UnitWidth,
                        unitLength = q.UnitLength,
                        unitHeight = q.UnitHeight,
                        unitVolume = q.UnitVolume,
                        volume = q.Volume,
                        unitPrice = q.UnitPrice,
                        price = q.Price,
                        //documentRef_No1 = q.DocumentRef_No1,
                        //documentRef_No2 = q.DocumentRef_No2,
                        //documentRef_No3 = q.DocumentRef_No3,
                        //documentRef_No4 = q.DocumentRef_No4,
                        //documentRef_No5 = q.DocumentRef_No5,
                        //document_Status = q.Document_Status,
                        //udf_1 = q.UDF_1,
                        //udf_2 = q.UDF_2,
                        //udf_3 = q.UDF_3,
                        //udf_4 = q.UDF_4,
                        //udf_5 = q.UDF_5,
                        ref_Process_Index = q.Ref_Process_Index,
                        ref_Document_No = q.Ref_Document_No,
                        ref_Document_LineNum = q.Ref_Document_LineNum,
                        ref_Document_Index = q.Ref_Document_Index,
                        ref_DocumentItem_Index = q.Ref_DocumentItem_Index,
                        reasonCode_Index = q.ReasonCode_Index,
                        reasonCode_Id = q.ReasonCode_Id,
                        reasonCode_Name = q.ReasonCode_Name,
                        tagOutPick_Index = q.TagOutPick_Index,
                        tagOutPick_No = q.TagOutPick_No,
                        picking_Qty = q.Picking_Qty,
                        picking_Ratio = q.Picking_Ratio,
                        picking_TotalQty = q.Picking_TotalQty,
                        picking_By = q.Picking_By,
                        picking_Date = q.Picking_Date,
                        picking_Status = q.Picking_Status,
                        splitQty = q.splitQty,
                        planGoodsIssue_Index = q.PlanGoodsIssue_Index,
                        planGoodsIssueItem_Index = q.PlanGoodsIssueItem_Index,
                        planGoodsIssue_No = q.PlanGoodsIssue_No,
                        pick_ProductConversion_Index = q.Pick_ProductConversion_Index,
                        pick_ProductConversion_Id = q.Pick_ProductConversion_Id,
                        pick_ProductConversion_Name = q.Pick_ProductConversion_Name,
                        productConversionBarcode = q.ProductConversionBarcode,
                        tagOut_Index = q.TagOut_Index,
                        tagOut_No = q.TagOut_No,
                        imageUri = q.ImageUri
                    };
                    items.Add(item);
                }
                results.itemsDetail = items;
                results.resultIsUse = true;

                return results;

            }
            catch (Exception ex)
            {
                var results = new actionResultScanPicksearchViewModel();

                results.resultMsg = ex.Message;
                results.resultIsUse = true;
                return results;
            }
        }
        #endregion
        
        #region GetDataScanTaskScanPick
        public actionResultScanPicksearchViewModel GetDataScanTaskItem(ScanPicksearchViewModel model)
        {
            try
            {
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskItemViewModel>();

                var query = db.IM_TaskItem.Where(c => c.Task_Index == new Guid(model.task_Index) && (c.Picking_Status == null ? 0 : c.Picking_Status) == 0)
                     .GroupBy(g => new
                     {
                         task_Index = g.Task_Index.ToString(),
                         task_No = g.Task_No,
                         tag_Index = g.Tag_Index,
                         tag_No = g.Tag_No,
                         product_Index = g.Product_Index,
                         product_Id = g.Product_Id,
                         product_Name = g.Product_Name,
                         product_SecondName = g.Product_SecondName,
                         product_ThirdName = g.Product_ThirdName,
                         product_Lot = g.Product_Lot,
                         itemStatus_Index = g.ItemStatus_Index,
                         itemStatus_Id = g.ItemStatus_Id,
                         itemStatus_Name = g.ItemStatus_Name,
                         location_Index = g.Location_Index,
                         location_Id = g.Location_Id,
                         location_Name = g.Location_Name,
                         ratio = g.Ratio,
                         productConversion_Index = g.ProductConversion_Index,
                         productConversion_Id = g.ProductConversion_Id,
                         productConversion_Name = g.ProductConversion_Name,
                         //documentRef_No1 = g.DocumentRef_No1,
                         //documentRef_No2 = g.DocumentRef_No2,
                         //documentRef_No3 = g.DocumentRef_No3,
                         //documentRef_No4 = g.DocumentRef_No4,
                         //documentRef_No5 = g.DocumentRef_No5,
                         //document_Status = g.Document_Status,
                         //udf_1 = g.UDF_1,
                         //udf_2 = g.UDF_2,
                         //udf_3 = g.UDF_3,
                         //udf_4 = g.UDF_4,
                         //udf_5 = g.UDF_5,
                         ref_Document_No = g.Ref_Document_No,
                         ref_Document_Index = g.Ref_Document_Index,
                         //planGoodsIssue_Index = g.PlanGoodsIssue_Index,
                         //PlanGoodsIssueItem_Index = g.PlanGoodsIssueItem_Index,
                         //planGoodsIssue_No = g.PlanGoodsIssue_No,
                         imageUri = g.ImageUri
                     }).Select(s => new
                     {
                         s.Key.task_Index,
                         s.Key.task_No,
                         s.Key.tag_Index,
                         s.Key.tag_No,
                         s.Key.product_Index,
                         s.Key.product_Id,
                         s.Key.product_Name,
                         s.Key.product_SecondName,
                         s.Key.product_ThirdName,
                         s.Key.product_Lot,
                         s.Key.itemStatus_Index,
                         s.Key.itemStatus_Id,
                         s.Key.itemStatus_Name,
                         s.Key.location_Index,
                         s.Key.location_Id,
                         s.Key.location_Name,
                         Qty = s.Sum(sg => (sg.Qty ?? 0)),
                         s.Key.ratio,
                         TotalQty = s.Sum(sg => (sg.TotalQty ?? 0)),
                         s.Key.productConversion_Index,
                         s.Key.productConversion_Id,
                         s.Key.productConversion_Name,
                         //s.Key.documentRef_No1,
                         //s.Key.documentRef_No2,
                         //s.Key.documentRef_No3,
                         //s.Key.documentRef_No4,
                         //s.Key.documentRef_No5,
                         //s.Key.document_Status,
                         //s.Key.udf_1,
                         //s.Key.udf_2,
                         //s.Key.udf_3,
                         //s.Key.udf_4,
                         //s.Key.udf_5,
                         s.Key.ref_Document_No,
                         s.Key.ref_Document_Index,
                         //s.Key.planGoodsIssue_Index,
                         //s.Key.PlanGoodsIssueItem_Index,
                         //s.Key.planGoodsIssue_No,
                         s.Key.imageUri
                     }).ToList();

                foreach (var q in query)
                {
                    var item = new taskItemViewModel
                    {
                        task_Index = q.task_Index.ToString(),
                        task_No = q.task_No,
                        tag_Index = q.tag_Index,
                        tag_No = q.tag_No,
                        product_Index = q.product_Index,
                        product_Id = q.product_Id,
                        product_Name = q.product_Name,
                        product_SecondName = q.product_SecondName,
                        product_ThirdName = q.product_ThirdName,
                        product_Lot = q.product_Lot,
                        itemStatus_Index = q.itemStatus_Index,
                        itemStatus_Id = q.itemStatus_Id,
                        itemStatus_Name = q.itemStatus_Name,
                        location_Index = q.location_Index,
                        location_Id = q.location_Id,
                        location_Name = q.location_Name,
                        qty = q.Qty,
                        ratio = q.ratio,
                        totalQty = q.TotalQty,
                        productConversion_Index = q.productConversion_Index,
                        productConversion_Id = q.productConversion_Id,
                        productConversion_Name = q.productConversion_Name,
                        //documentRef_No1 = q.documentRef_No1,
                        //documentRef_No2 = q.documentRef_No2,
                        //documentRef_No3 = q.documentRef_No3,
                        //documentRef_No4 = q.documentRef_No4,
                        //documentRef_No5 = q.documentRef_No5,
                        //document_Status = q.document_Status,
                        //udf_1 = q.udf_1,
                        //udf_2 = q.udf_2,
                        //udf_3 = q.udf_3,
                        //udf_4 = q.udf_4,
                        //udf_5 = q.udf_5,
                        ref_Document_No = q.ref_Document_No,
                        ref_Document_Index = q.ref_Document_Index,
                        //planGoodsIssue_Index = q.planGoodsIssue_Index,
                        //planGoodsIssueItem_Index = q.PlanGoodsIssueItem_Index,
                        //planGoodsIssue_No = q.planGoodsIssue_No,
                        imageUri = q.imageUri
                    };
                    items.Add(item);
                }
                results.itemsDetail = items;
                results.resultIsUse = true;

                return results;

            }
            catch (Exception ex)
            {
                var results = new actionResultScanPicksearchViewModel();

                results.resultMsg = ex.Message;
                results.resultIsUse = true;
                return results;
            }
        }
        #endregion

        #region GetDataScanTaskLabelingItem
        public actionResultScanPicksearchViewModel GetDataScanTaskLabelingItem(ScanPicksearchViewModel model)
        {
            try
            {
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskItemViewModel>();

                var query = db.View_Taskitem_Labeling.Where(c => c.task_Index == new Guid(model.task_Index)).ToList();

                foreach (var q in query)
                {
                    var item = new taskItemViewModel
                    {
                        task_Index = q.task_Index.ToString(),
                        task_No = q.task_No,
                        tag_Index = q.tag_Index,
                        tag_No = q.tag_No,
                        product_Index = q.product_Index,
                        product_Id = q.product_Id,
                        product_Name = q.product_Name,
                        product_SecondName = q.product_SecondName,
                        product_ThirdName = q.product_ThirdName,
                        product_Lot = q.product_Lot,
                        itemStatus_Index = q.itemStatus_Index,
                        itemStatus_Id = q.itemStatus_Id,
                        itemStatus_Name = q.itemStatus_Name,
                        location_Index = q.location_Index,
                        location_Id = q.location_Id,
                        location_Name = q.location_Name,
                        qty = q.Qty,
                        ratio = q.ratio,
                        totalQty = q.TotalQty,
                        productConversion_Index = q.productConversion_Index,
                        productConversion_Id = q.productConversion_Id,
                        productConversion_Name = q.productConversion_Name,
                        ref_Document_No = q.ref_Document_No,
                        ref_Document_Index = q.ref_Document_Index,
                        imageUri = q.imageUri
                    };
                    items.Add(item);
                }
                results.itemsDetail = items;
                results.resultIsUse = true;

                return results;

            }
            catch (Exception ex)
            {
                var results = new actionResultScanPicksearchViewModel();

                results.resultMsg = ex.Message;
                results.resultIsUse = true;
                return results;
            }
        }
        #endregion

        #region GetDataScanTaskScanPickQtyItem
        public actionResultScanPicksearchViewModel GetDataScanTaskScanPickQtyItem(ScanPicksearchViewModel model)
        {
            try
            {
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskItemViewModel>();

                var query = db.View_Taskitem_with_Truckload_PICKQTY.Where(c => c.Task_Index == new Guid(model.task_Index) && (c.Picking_Status == null ? 0 : c.Picking_Status) == 2 && (c.PickingLabeling_Status == null ? 0 : c.PickingLabeling_Status) == 2 && (c.PickingPickQty_Status == null ? 0 : c.PickingPickQty_Status) == 0)
                     .GroupBy(g => new
                     {
                         task_Index = g.Task_Index.ToString(),
                         task_No = g.Task_No,
                         tag_Index = g.Tag_Index,
                         tag_No = g.Tag_No,
                         product_Index = g.Product_Index,
                         product_Id = g.Product_Id,
                         product_Name = g.Product_Name,
                         product_SecondName = g.Product_SecondName,
                         product_ThirdName = g.Product_ThirdName,
                         product_Lot = g.Product_Lot,
                         itemStatus_Index = g.ItemStatus_Index,
                         itemStatus_Id = g.ItemStatus_Id,
                         itemStatus_Name = g.ItemStatus_Name,
                         location_Index = g.PickingLabeling_Location_Index,
                         location_Id = g.PickingLabeling_Location_Id,
                         location_Name = g.PickingLabeling_Location_Name,
                         ratio = g.Ratio,
                         productConversion_Index = g.ProductConversion_Index,
                         productConversion_Id = g.ProductConversion_Id,
                         productConversion_Name = g.ProductConversion_Name,
                         //documentRef_No1 = g.DocumentRef_No1,
                         //documentRef_No2 = g.DocumentRef_No2,
                         //documentRef_No3 = g.DocumentRef_No3,
                         //documentRef_No4 = g.DocumentRef_No4,
                         //documentRef_No5 = g.DocumentRef_No5,
                         //document_Status = g.Document_Status,
                         //udf_1 = g.UDF_1,
                         //udf_2 = g.UDF_2,
                         //udf_3 = g.UDF_3,
                         //udf_4 = g.UDF_4,
                         //udf_5 = g.UDF_5,
                         ref_Document_No = g.Ref_Document_No,
                         ref_Document_Index = g.Ref_Document_Index,
                         TruckLoad_Index = g.TruckLoad_Index,
                         Dock = g.Dock_Name,
                         Dock_index = g.Dock_Index,
                         //planGoodsIssue_Index = g.PlanGoodsIssue_Index,
                         //PlanGoodsIssueItem_Index = g.PlanGoodsIssueItem_Index,
                         //planGoodsIssue_No = g.PlanGoodsIssue_No,
                         imageUri = g.ImageUri
                     }).Select(s => new
                     {
                         s.Key.task_Index,
                         s.Key.task_No,
                         s.Key.tag_Index,
                         s.Key.tag_No,
                         s.Key.product_Index,
                         s.Key.product_Id,
                         s.Key.product_Name,
                         s.Key.product_SecondName,
                         s.Key.product_ThirdName,
                         s.Key.product_Lot,
                         s.Key.itemStatus_Index,
                         s.Key.itemStatus_Id,
                         s.Key.itemStatus_Name,
                         s.Key.location_Index,
                         s.Key.location_Id,
                         s.Key.location_Name,
                         Qty = s.Sum(sg => (sg.Qty ?? 0)),
                         s.Key.ratio,
                         TotalQty = s.Sum(sg => (sg.TotalQty ?? 0)),
                         s.Key.productConversion_Index,
                         s.Key.productConversion_Id,
                         s.Key.productConversion_Name,
                         //s.Key.documentRef_No1,
                         //s.Key.documentRef_No2,
                         //s.Key.documentRef_No3,
                         //s.Key.documentRef_No4,
                         //s.Key.documentRef_No5,
                         //s.Key.document_Status,
                         //s.Key.udf_1,
                         //s.Key.udf_2,
                         //s.Key.udf_3,
                         //s.Key.udf_4,
                         //s.Key.udf_5,
                         s.Key.ref_Document_No,
                         s.Key.ref_Document_Index,
                         s.Key.TruckLoad_Index,
                         s.Key.Dock,
                         s.Key.Dock_index,
                         //s.Key.planGoodsIssue_Index,
                         //s.Key.PlanGoodsIssueItem_Index,
                         //s.Key.planGoodsIssue_No,
                         s.Key.imageUri
                     }).ToList();

                var getplan = db.View_Taskitem_with_Truckload_PICKQTY_GETPLAN.Where(c => c.Task_Index == Guid.Parse(model.task_Index)).ToList();
                foreach (var q in query)
                {
                    var Serial = dbMaster.MS_Product.FirstOrDefault(c => c.Product_Index == q.product_Index);
                    var item = new taskItemViewModel
                    {
                        task_Index = q.task_Index.ToString(),
                        task_No = q.task_No,
                        tag_Index = q.tag_Index,
                        tag_No = q.tag_No,
                        product_Index = q.product_Index,
                        product_Id = q.product_Id,
                        product_Name = q.product_Name,
                        product_SecondName = q.product_SecondName,
                        product_ThirdName = q.product_ThirdName,
                        product_Lot = q.product_Lot,
                        itemStatus_Index = q.itemStatus_Index,
                        itemStatus_Id = q.itemStatus_Id,
                        itemStatus_Name = q.itemStatus_Name,
                        location_Index = q.location_Index,
                        location_Id = q.location_Id,
                        location_Name = q.location_Name,
                        qty = q.Qty,
                        ratio = q.ratio,
                        totalQty = q.TotalQty,
                        productConversion_Index = q.productConversion_Index,
                        productConversion_Id = q.productConversion_Id,
                        productConversion_Name = q.productConversion_Name,
                        //documentRef_No1 = q.documentRef_No1,
                        //documentRef_No2 = q.documentRef_No2,
                        //documentRef_No3 = q.documentRef_No3,
                        //documentRef_No4 = q.documentRef_No4,
                        //documentRef_No5 = q.documentRef_No5,
                        //document_Status = q.document_Status,
                        //udf_1 = q.udf_1,
                        //udf_2 = q.udf_2,
                        //udf_3 = q.udf_3,
                        //udf_4 = q.udf_4,
                        //udf_5 = q.udf_5,
                        ref_Document_No = q.ref_Document_No,
                        ref_Document_Index = q.ref_Document_Index,
                        TruckLoad_Index = q.TruckLoad_Index,
                        IsSerial = Serial.IsSerial == 1,
                        Dock_Name = q.Dock,
                        Dock_Index = q.Dock_index,
                        //planGoodsIssue_Index = q.planGoodsIssue_Index,
                        //planGoodsIssueItem_Index = q.PlanGoodsIssueItem_Index,
                        //planGoodsIssue_No = q.planGoodsIssue_No,
                        imageUri = q.imageUri
                    };

                    List<plangoodsissue> plan = new List<plangoodsissue>();
                    getplan = getplan.Where(c=> c.TruckLoad_Index == q.TruckLoad_Index).ToList();
                    foreach (var planX in getplan)
                    {
                        plangoodsissue getitem = new plangoodsissue();
                        getitem.planGoodsIssue_No = planX.PlanGoodsIssue_No;
                        if (planX.Picking_Qty != null)
                        {
                            getitem.qty = (int)planX.Picking_Qty;
                        }
                        
                        plan.Add(getitem);
                    }
                    item.plangoodsissue = plan;
                    items.Add(item);
                }
                results.itemsDetail = items;
                results.resultIsUse = true;


                return results;

            }
            catch (Exception ex)
            {
                var results = new actionResultScanPicksearchViewModel();

                results.resultMsg = ex.Message;
                results.resultIsUse = true;
                return results;
            }
        }
        #endregion

        #region GetDataScanTaskLocationLabelingItem

        public actionResultScanPicksearchViewModel GetDataScanTaskLocationLabelingItem(ScanPicksearchViewModel model)
        {
            try
            {
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskItemViewModel>();
                var query = db.View_Taskitem_with_Truckload.Where(c => c.TruckLoad_Index == model.TruckLoad_Index && c.PickingPickQty_Status == 2 && c.PickingToDock_Status == null).ToList();
                var group_query = query.GroupBy(c => new
                                                        {
                                                            c.Tag_No,
                                                            c.Dock_Name,
                                                            c.Dock_Id,
                                                            c.Dock_Index,
                                                            c.TruckLoad_Index,
                                                            c.TruckLoad_No
                                                        }).Select(c=> new {
                                                            c.Key.Tag_No,
                                                            c.Key.Dock_Name,
                                                            c.Key.Dock_Id,
                                                            c.Key.Dock_Index,
                                                            c.Key.TruckLoad_Index,
                                                            c.Key.TruckLoad_No
                                                        }).ToList();

                foreach (var q in group_query)
                {
                    var item = new taskItemViewModel
                    {
                        tag_No = q.Tag_No,
                        TruckLoad_Index = q.TruckLoad_Index,
                        Dock_Name = q.Dock_Name,
                        Dock_Id = q.Dock_Id,
                        Dock_Index = q.Dock_Index
                    };
                    var plan_getproduct = new List<group_product>();
                    var getproduct = query.GroupBy(c => new { c.Product_Id,c.Product_Name }).Select(c => new { c.Key.Product_Id,c.Key.Product_Name,sum = c.Sum(x=> x.Qty) }).ToList();
                    foreach (var planX in getproduct.OrderBy(c=> c.Product_Id))
                    {
                        group_product getitem = new group_product();
                        getitem.Product_Id = planX.Product_Id;
                        getitem.Product_Name = planX.Product_Name;
                        if (planX.sum != null)
                        {
                            getitem.qty = (int)planX.sum;
                        }

                        plan_getproduct.Add(getitem);
                    }
                    item.group_product = plan_getproduct;
                    items.Add(item);
                }
                results.itemsDetail = items;
                results.resultIsUse = true;

                return results;

            }
            catch (Exception ex)
            {
                var results = new actionResultScanPicksearchViewModel();

                results.resultMsg = ex.Message;
                results.resultIsUse = true;
                return results;
            }
        }

        #region OLD
        //public actionResultScanPicksearchViewModel GetDataScanTaskLocationLabelingItem(ScanPicksearchViewModel model)
        //{
        //    try
        //    {
        //        var results = new actionResultScanPicksearchViewModel();

        //        var items = new List<taskItemViewModel>();
        //        var query = db.View_Taskitem_with_Truckload.Where(c => c.Task_Index == new Guid(model.task_Index) && (c.Picking_Status == null ? 0 : c.Picking_Status) == 2 && (c.PickingLabeling_Status == null ? 0 : c.PickingLabeling_Status) == 2 && (c.PickingPickQty_Status == null ? 0 : c.PickingPickQty_Status) == 2 && (c.PickingToDock_Status == null ? 0 : c.PickingToDock_Status) == 0)
        //        .GroupBy(g => new
        //             {
        //                 task_Index = g.Task_Index.ToString(),
        //                 task_No = g.Task_No,
        //                 tag_Index = g.Tag_Index,
        //                 tag_No = g.Tag_No,
        //                 product_Index = g.Product_Index,
        //                 product_Id = g.Product_Id,
        //                 product_Name = g.Product_Name,
        //                 product_SecondName = g.Product_SecondName,
        //                 product_ThirdName = g.Product_ThirdName,
        //                 product_Lot = g.Product_Lot,
        //                 itemStatus_Index = g.ItemStatus_Index,
        //                 itemStatus_Id = g.ItemStatus_Id,
        //                 itemStatus_Name = g.ItemStatus_Name,
        //                 location_Index = g.PickingPickQty_Location_Index,
        //                 location_Id = g.PickingPickQty_Location_Id,
        //                 location_Name = g.PickingPickQty_Location_Name,
        //                 ratio = g.Ratio,
        //                 productConversion_Index = g.ProductConversion_Index,
        //                 productConversion_Id = g.ProductConversion_Id,
        //                 productConversion_Name = g.ProductConversion_Name,
        //                 documentRef_No1 = g.DocumentRef_No1,
        //                 documentRef_No2 = g.DocumentRef_No2,
        //                 documentRef_No3 = g.DocumentRef_No3,
        //                 documentRef_No4 = g.DocumentRef_No4,
        //                 documentRef_No5 = g.DocumentRef_No5,
        //                 document_Status = g.Document_Status,
        //                 udf_1 = g.UDF_1,
        //                 udf_2 = g.UDF_2,
        //                 udf_3 = g.UDF_3,
        //                 udf_4 = g.UDF_4,
        //                 udf_5 = g.UDF_5,
        //                 ref_Document_No = g.Ref_Document_No,
        //                 ref_Document_Index = g.Ref_Document_Index,
        //                 TruckLoad_Index = g.TruckLoad_Index,
        //                 Dock_Name = g.Dock_Name,
        //                 Dock_Index = g.Dock_Index,
        //                 Dock_Id = g.Dock_Id,
        //                 //planGoodsIssue_Index = g.PlanGoodsIssue_Index,
        //                 //PlanGoodsIssueItem_Index = g.PlanGoodsIssueItem_Index,
        //                 //planGoodsIssue_No = g.PlanGoodsIssue_No,
        //                 imageUri = g.ImageUri
        //             }).Select(s => new
        //             {
        //                 s.Key.task_Index,
        //                 s.Key.task_No,
        //                 s.Key.tag_Index,
        //                 s.Key.tag_No,
        //                 s.Key.product_Index,
        //                 s.Key.product_Id,
        //                 s.Key.product_Name,
        //                 s.Key.product_SecondName,
        //                 s.Key.product_ThirdName,
        //                 s.Key.product_Lot,
        //                 s.Key.itemStatus_Index,
        //                 s.Key.itemStatus_Id,
        //                 s.Key.itemStatus_Name,
        //                 s.Key.location_Index,
        //                 s.Key.location_Id,
        //                 s.Key.location_Name,
        //                 Qty = s.Sum(sg => (sg.Qty ?? 0)),
        //                 s.Key.ratio,
        //                 TotalQty = s.Sum(sg => (sg.TotalQty ?? 0)),
        //                 s.Key.productConversion_Index,
        //                 s.Key.productConversion_Id,
        //                 s.Key.productConversion_Name,
        //                 s.Key.documentRef_No1,
        //                 s.Key.documentRef_No2,
        //                 s.Key.documentRef_No3,
        //                 s.Key.documentRef_No4,
        //                 s.Key.documentRef_No5,
        //                 s.Key.document_Status,
        //                 s.Key.udf_1,
        //                 s.Key.udf_2,
        //                 s.Key.udf_3,
        //                 s.Key.udf_4,
        //                 s.Key.udf_5,
        //                 s.Key.ref_Document_No,
        //                 s.Key.ref_Document_Index,
        //                 //s.Key.planGoodsIssue_Index,
        //                 //s.Key.PlanGoodsIssueItem_Index,
        //                 //s.Key.planGoodsIssue_No,
        //                 s.Key.TruckLoad_Index,
        //                 s.Key.Dock_Name,
        //                 s.Key.Dock_Index,
        //                 s.Key.Dock_Id,
        //                 s.Key.imageUri
        //             }).ToList();

        //        foreach (var q in query.OrderBy(c=> c.tag_No))
        //        {
        //            //var taskItem = db.IM_TaskItem.FirstOrDefault(c => c.Task_Index == q.Task_Index);
        //            var item = new taskItemViewModel
        //            {
        //                task_Index = q.task_Index.ToString(),
        //                task_No = q.task_No,
        //                tag_Index = q.tag_Index,
        //                tag_No = q.tag_No,
        //                product_Index = q.product_Index,
        //                product_Id = q.product_Id,
        //                product_Name = q.product_Name,
        //                product_SecondName = q.product_SecondName,
        //                product_ThirdName = q.product_ThirdName,
        //                product_Lot = q.product_Lot,
        //                itemStatus_Index = q.itemStatus_Index,
        //                itemStatus_Id = q.itemStatus_Id,
        //                itemStatus_Name = q.itemStatus_Name,
        //                location_Index = q.location_Index,
        //                location_Id = q.location_Id,
        //                location_Name = q.location_Name,
        //                qty = q.Qty,
        //                ratio = q.ratio,
        //                totalQty = q.TotalQty,
        //                productConversion_Index = q.productConversion_Index,
        //                productConversion_Id = q.productConversion_Id,
        //                productConversion_Name = q.productConversion_Name,
        //                documentRef_No1 = q.documentRef_No1,
        //                documentRef_No2 = q.documentRef_No2,
        //                documentRef_No3 = q.documentRef_No3,
        //                documentRef_No4 = q.documentRef_No4,
        //                documentRef_No5 = q.documentRef_No5,
        //                document_Status = q.document_Status,
        //                udf_1 = q.udf_1,
        //                udf_2 = q.udf_2,
        //                udf_3 = q.udf_3,
        //                udf_4 = q.udf_4,
        //                udf_5 = q.udf_5,
        //                ref_Document_No = q.ref_Document_No,
        //                ref_Document_Index = q.ref_Document_Index,
        //                //planGoodsIssue_Index = q.planGoodsIssue_Index,
        //                //planGoodsIssueItem_Index = q.PlanGoodsIssueItem_Index,
        //                //planGoodsIssue_No = q.planGoodsIssue_No,
        //                TruckLoad_Index = q.TruckLoad_Index,
        //                Dock_Name = q.Dock_Name,
        //                Dock_Id = q.Dock_Id,
        //                Dock_Index = q.Dock_Index,
        //                imageUri = q.imageUri
        //            };
        //            items.Add(item);
        //        }
        //        results.itemsDetail = items;
        //        results.resultIsUse = true;

        //        return results;

        //    }
        //    catch (Exception ex)
        //    {
        //        var results = new actionResultScanPicksearchViewModel();

        //        results.resultMsg = ex.Message;
        //        results.resultIsUse = true;
        //        return results;
        //    }
        //}
        #endregion

        #endregion

        #region GetDataScanTaskMoveToselective
        public actionResultScanPicksearchViewModel GetDataScanTaskMoveToselective(ScanPicksearchViewModel model)
        {
            try
            {
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskItemViewModel>();
                var query = db.View_Taskitem_MoveOnGround.Where(c => c.task_Index == new Guid(model.task_Index)).ToList();

                foreach (var q in query)
                {
                    var item = new taskItemViewModel
                    {
                        task_Index = q.task_Index.ToString(),
                        task_No = q.task_No,
                        tag_Index = q.tag_Index,
                        tag_No = q.tag_No,
                        product_Index = q.product_Index,
                        product_Id = q.product_Id,
                        product_Name = q.product_Name,
                        product_SecondName = q.product_SecondName,
                        product_ThirdName = q.product_ThirdName,
                        product_Lot = q.product_Lot,
                        itemStatus_Index = q.itemStatus_Index,
                        itemStatus_Id = q.itemStatus_Id,
                        itemStatus_Name = q.itemStatus_Name,
                        location_Index = q.location_Index,
                        location_Id = q.location_Id,
                        location_Name = q.location_Name,
                        qty = q.Qty,
                        ratio = q.ratio,
                        totalQty = q.TotalQty,
                        productConversion_Index = q.productConversion_Index,
                        productConversion_Id = q.productConversion_Id,
                        productConversion_Name = q.productConversion_Name,
                        ref_Document_No = q.ref_Document_No,
                        ref_Document_Index = q.ref_Document_Index,
                        imageUri = q.imageUri
                    };
                    items.Add(item);
                }
                results.itemsDetail = items;
                results.resultIsUse = true;

                return results;

            }
            catch (Exception ex)
            {
                var results = new actionResultScanPicksearchViewModel();

                results.resultMsg = ex.Message;
                results.resultIsUse = true;
                return results;
            }
        }
        #endregion

        #region GetDataScanTasStgToDock

        #region NEW
        public actionResultScanPicksearchViewModel GetDataScanTasStgToDock(ScanPicksearchViewModel model)
        {
            try
            {
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskItemViewModel>();
                var query = db.View_Taskitem_with_Truckload.Where(c => c.TruckLoad_Index == model.TruckLoad_Index && c.PickingPickQty_Status == 2 && c.PickingToDock_Status == 2 && c.Document_StatusTracking == 1).ToList();
                var group_query = query.GroupBy(c => new
                {
                    c.Tag_No,
                    c.Dock_Name,
                    c.Dock_Id,
                    c.Dock_Index,
                    c.TruckLoad_Index,
                    c.TruckLoad_No
                }).Select(c => new {
                    c.Key.Tag_No,
                    c.Key.Dock_Name,
                    c.Key.Dock_Id,
                    c.Key.Dock_Index,
                    c.Key.TruckLoad_Index,
                    c.Key.TruckLoad_No
                }).ToList();

                foreach (var q in group_query)
                {
                    var item = new taskItemViewModel
                    {
                        tag_No = q.Tag_No,
                        TruckLoad_Index = q.TruckLoad_Index,
                        Dock_Name = q.Dock_Name,
                        Dock_Id = q.Dock_Id,
                        Dock_Index = q.Dock_Index
                    };
                    var plan_getproduct = new List<group_product>();
                    var getproduct = query.GroupBy(c => new { c.Product_Id, c.Product_Name,c.ProductConversion_Name }).Select(c => new { c.Key.Product_Id, c.Key.Product_Name, sum = c.Sum(x => x.Qty) ,c.Key.ProductConversion_Name }).ToList();
                    foreach (var planX in getproduct.OrderBy(c => c.Product_Id))
                    {
                        group_product getitem = new group_product();
                        getitem.Product_Id = planX.Product_Id;
                        getitem.Product_Name = planX.Product_Name;
                        getitem.ProductConversion_Name = planX.ProductConversion_Name;
                        if (planX.sum != null)
                        {
                            getitem.qty = (int)planX.sum;
                        }

                        plan_getproduct.Add(getitem);
                    }
                    item.group_product = plan_getproduct;
                    items.Add(item);
                }
                results.itemsDetail = items;
                results.resultIsUse = true;

                return results;

            }
            catch (Exception ex)
            {
                var results = new actionResultScanPicksearchViewModel();

                results.resultMsg = ex.Message;
                results.resultIsUse = true;
                return results;
            }
        }
        #endregion

        #region OLD
        //public actionResultScanPicksearchViewModel GetDataScanTasStgToDock(ScanPicksearchViewModel model)
        //{
        //    try
        //    {
        //        var results = new actionResultScanPicksearchViewModel();

        //        var items = new List<taskItemViewModel>();
        //        var query = db.View_Taskitem_with_Truckload.Where(c => c.Task_Index == new Guid(model.task_Index) && c.TruckLoad_No == model.truckload_no && (c.Picking_Status == null ? 0 : c.Picking_Status) == 2 && (c.PickingLabeling_Status == null ? 0 : c.PickingLabeling_Status) == 2 && (c.PickingPickQty_Status == null ? 0 : c.PickingPickQty_Status) == 2 && (c.PickingToDock_Status == null ? 0 : c.PickingToDock_Status) == 2 && (c.Document_StatusTracking == null ? 0 : c.Document_StatusTracking) == 1)
        //        .GroupBy(g => new
        //        {
        //            task_Index = g.Task_Index.ToString(),
        //            task_No = g.Task_No,
        //            tag_Index = g.Tag_Index,
        //            tag_No = g.Tag_No,
        //            product_Index = g.Product_Index,
        //            product_Id = g.Product_Id,
        //            product_Name = g.Product_Name,
        //            product_SecondName = g.Product_SecondName,
        //            product_ThirdName = g.Product_ThirdName,
        //            product_Lot = g.Product_Lot,
        //            itemStatus_Index = g.ItemStatus_Index,
        //            itemStatus_Id = g.ItemStatus_Id,
        //            itemStatus_Name = g.ItemStatus_Name,
        //            location_Index = g.Location_Index,
        //            location_Id = g.Location_Id,
        //            location_Name = g.Location_Name,
        //            ratio = g.Ratio,
        //            productConversion_Index = g.ProductConversion_Index,
        //            productConversion_Id = g.ProductConversion_Id,
        //            productConversion_Name = g.ProductConversion_Name,
        //            documentRef_No1 = g.DocumentRef_No1,
        //            documentRef_No2 = g.DocumentRef_No2,
        //            documentRef_No3 = g.DocumentRef_No3,
        //            documentRef_No4 = g.DocumentRef_No4,
        //            documentRef_No5 = g.DocumentRef_No5,
        //            document_Status = g.Document_Status,
        //            udf_1 = g.UDF_1,
        //            udf_2 = g.UDF_2,
        //            udf_3 = g.UDF_3,
        //            udf_4 = g.UDF_4,
        //            udf_5 = g.UDF_5,
        //            ref_Document_No = g.Ref_Document_No,
        //            ref_Document_Index = g.Ref_Document_Index,
        //            TruckLoad_Index = g.TruckLoad_Index,
        //            //planGoodsIssue_Index = g.PlanGoodsIssue_Index,
        //            //PlanGoodsIssueItem_Index = g.PlanGoodsIssueItem_Index,
        //            //planGoodsIssue_No = g.PlanGoodsIssue_No,
        //            imageUri = g.ImageUri
        //        }).Select(s => new
        //        {
        //            s.Key.task_Index,
        //            s.Key.task_No,
        //            s.Key.tag_Index,
        //            s.Key.tag_No,
        //            s.Key.product_Index,
        //            s.Key.product_Id,
        //            s.Key.product_Name,
        //            s.Key.product_SecondName,
        //            s.Key.product_ThirdName,
        //            s.Key.product_Lot,
        //            s.Key.itemStatus_Index,
        //            s.Key.itemStatus_Id,
        //            s.Key.itemStatus_Name,
        //            s.Key.location_Index,
        //            s.Key.location_Id,
        //            s.Key.location_Name,
        //            Qty = s.Sum(sg => (sg.Qty ?? 0)),
        //            s.Key.ratio,
        //            TotalQty = s.Sum(sg => (sg.TotalQty ?? 0)),
        //            s.Key.productConversion_Index,
        //            s.Key.productConversion_Id,
        //            s.Key.productConversion_Name,
        //            s.Key.documentRef_No1,
        //            s.Key.documentRef_No2,
        //            s.Key.documentRef_No3,
        //            s.Key.documentRef_No4,
        //            s.Key.documentRef_No5,
        //            s.Key.document_Status,
        //            s.Key.udf_1,
        //            s.Key.udf_2,
        //            s.Key.udf_3,
        //            s.Key.udf_4,
        //            s.Key.udf_5,
        //            s.Key.ref_Document_No,
        //            s.Key.ref_Document_Index,
        //            s.Key.TruckLoad_Index,
        //            //s.Key.planGoodsIssue_Index,
        //            //s.Key.PlanGoodsIssueItem_Index,
        //            //s.Key.planGoodsIssue_No,
        //            s.Key.imageUri
        //        }).ToList();

        //        foreach (var q in query)
        //        {
        //            //var taskItem = db.IM_TaskItem.FirstOrDefault(c => c.Task_Index == q.Task_Index);
        //            var item = new taskItemViewModel
        //            {
        //                task_Index = q.task_Index.ToString(),
        //                task_No = q.task_No,
        //                tag_Index = q.tag_Index,
        //                tag_No = q.tag_No,
        //                product_Index = q.product_Index,
        //                product_Id = q.product_Id,
        //                product_Name = q.product_Name,
        //                product_SecondName = q.product_SecondName,
        //                product_ThirdName = q.product_ThirdName,
        //                product_Lot = q.product_Lot,
        //                itemStatus_Index = q.itemStatus_Index,
        //                itemStatus_Id = q.itemStatus_Id,
        //                itemStatus_Name = q.itemStatus_Name,
        //                location_Index = q.location_Index,
        //                location_Id = q.location_Id,
        //                location_Name = q.location_Name,
        //                qty = q.Qty,
        //                ratio = q.ratio,
        //                totalQty = q.TotalQty,
        //                productConversion_Index = q.productConversion_Index,
        //                productConversion_Id = q.productConversion_Id,
        //                productConversion_Name = q.productConversion_Name,
        //                documentRef_No1 = q.documentRef_No1,
        //                documentRef_No2 = q.documentRef_No2,
        //                documentRef_No3 = q.documentRef_No3,
        //                documentRef_No4 = q.documentRef_No4,
        //                documentRef_No5 = q.documentRef_No5,
        //                document_Status = q.document_Status,
        //                udf_1 = q.udf_1,
        //                udf_2 = q.udf_2,
        //                udf_3 = q.udf_3,
        //                udf_4 = q.udf_4,
        //                udf_5 = q.udf_5,
        //                ref_Document_No = q.ref_Document_No,
        //                ref_Document_Index = q.ref_Document_Index,
        //                TruckLoad_Index = q.TruckLoad_Index,
        //                //planGoodsIssue_Index = q.planGoodsIssue_Index,
        //                //planGoodsIssueItem_Index = q.PlanGoodsIssueItem_Index,
        //                //planGoodsIssue_No = q.planGoodsIssue_No,
        //                imageUri = q.imageUri
        //            };
        //            items.Add(item);
        //        }
        //        results.itemsDetail = items;
        //        results.resultIsUse = true;

        //        return results;

        //    }
        //    catch (Exception ex)
        //    {
        //        var results = new actionResultScanPicksearchViewModel();

        //        results.resultMsg = ex.Message;
        //        results.resultIsUse = true;
        //        return results;
        //    }
        //}
        #endregion

        #endregion

    }
}
