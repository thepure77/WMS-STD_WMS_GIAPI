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
using System.Reflection;
using static GIBusiness.GoodIssue.TaskfilterViewModel;

namespace GIBusiness.GoodIssue
{
    public class ScanPickServiceExport
    {
        private GIDbContext db;
        private MasterDbContext dbMaster;

        public ScanPickServiceExport()
        {
            db = new GIDbContext();
            dbMaster = new MasterDbContext();
        }
        public ScanPickServiceExport(GIDbContext db, MasterDbContext dbMaster)
        {
            this.db = db;
            this.dbMaster = dbMaster;
        }

        #region GetReasonCode
        public List<ReasonCodeViewModel> GetReasonCode(ReasonCodeFilterViewModel model)
        {
            try
            {
                model.process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                var results = utils.SendDataApi<List<ReasonCodeViewModel>>(new AppSettingConfig().GetUrl("GetReasonCode"), model.sJson());

                return results;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region dropdown_printer
        public List<printer_model> dropdown_printer()
        {
            List<printer_model> printer_Models = new List<printer_model>();
            logtxt olog = new logtxt();
            try
            {
                List<ms_Printer> printers = dbMaster.ms_Printer.Where(c => c.IsActive == 1 && c.IsDelete == 0).OrderBy(c=> c.Printer_Name).ToList();

                foreach (var item in printers)
                {
                    printer_model printer_Model = new printer_model();
                    printer_Model.Printer_Index = item.Printer_Index;
                    printer_Model.Printer_Id = item.Printer_Id;
                    printer_Model.Printer_Name = item.Printer_Name;
                    printer_Models.Add(printer_Model);
                }
                return printer_Models;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Filter

        #region Filter ScanPick
        public actionResultScanPicksearchViewModel FilterTask(ScanPicksearchViewModel model)
        {
            try
            {
                db.Database.SetCommandTimeout(120);
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskViewModel>();

                //var query = db.IM_Task.Where(c => c.Document_Status == 1 && c.UserAssign == model.userName).AsQueryable();
                //var query = db.IM_Task.Where(c => c.Document_Status == 1).AsQueryable();
                var query = db.View_Task_export.Where(c => c.Document_Status == 1 && c.Document_StatusScanPick == null ).AsQueryable();

                if (!string.IsNullOrEmpty(model.task_No))
                {
                    //query = query.Where(c => model.task_No.Contains(c.Task_No));
                    query = query.Where(c => c.Task_No.Contains(model.task_No) || c.Ref_Document_No.Contains(model.task_No) || c.Tag_No == model.task_No);
                }

                foreach (var q in query.OrderByDescending(o => o.Task_No))
                {
                    var taskItem = db.IM_TaskItem.FirstOrDefault(c => c.Task_Index == q.Task_Index);
                    var item = new taskViewModel
                    {
                        task_Index = q.Task_Index.ToString(),
                        task_No = q.Task_No,
                        document_Status = q.Document_Status,
                        //process_Index = q.Process_Index.ToString(),
                        //taskGroup_Index = q.TaskGroup_Index,
                        //taskGroup_Id = q.TaskGroup_Id,
                        //taskGroup_Name = q.TaskGroup_Name,
                        //documentPriority_Status = q.DocumentPriority_Status,
                        documentRef_No1 = q.DocumentRef_No1,
                        //documentRef_No2 = q.DocumentRef_No2,
                        //documentRef_No3 = q.DocumentRef_No3,
                        //documentRef_No4 = q.DocumentRef_No4,
                        //documentRef_No5 = q.DocumentRef_No5,
                        //udf_1 = q.UDF_1,
                        //udf_2 = q.UDF_2,
                        //udf_3 = q.UDF_3,
                        //udf_4 = q.UDF_4,
                        //udf_5 = q.UDF_5,
                        //doTask_By = q.DoTask_By,
                        //doTask_Date = q.DoTask_Date,
                        userAssign = q.UserAssign,
                        plangoodsissue_Index = taskItem?.PlanGoodsIssue_Index.ToString(),
                        plangoodsissue_No = taskItem?.PlanGoodsIssue_No,
                        goodsissue_Index = taskItem?.Ref_Document_Index.ToString(),
                        goodsissue_No = taskItem?.Ref_Document_No
                    };
                    items.Add(item);
                }
                results.items = items;
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

        #region Filter Labeling
        public actionResultScanPicksearchViewModel FilterTaskLabeling(ScanPicksearchViewModel model)
        {
            try
            {
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskViewModel>();
                db.Database.SetCommandTimeout(120);
                //var query = db.IM_Task.Where(c => c.Document_Status == 1 && c.UserAssign == model.userName).AsQueryable();
                //var query = db.IM_Task.Where(c => c.Document_Status == 1).AsQueryable();
                var query = db.View_Task_export.Where(c => c.Document_Status == 2 && c.Document_StatusScanPick == 2 && c.Document_StatusLabeling == null);

                if (!string.IsNullOrEmpty(model.task_No))
                {
                    //query = query.Where(c => model.task_No.Contains(c.Task_No));
                    query = query.Where(c => c.Task_No.Contains(model.task_No) || c.Ref_Document_No.Contains(model.task_No) || c.Tag_No == model.task_No);
                }

                foreach (var q in query.OrderByDescending(o => o.Task_No))
                {
                    var taskItem = db.IM_TaskItem.FirstOrDefault(c => c.Task_Index == q.Task_Index );
                    var item = new taskViewModel
                    {
                        task_Index = q.Task_Index.ToString(),
                        task_No = q.Task_No,
                        //process_Index = q.Process_Index.ToString(),
                        //taskGroup_Index = q.TaskGroup_Index,
                        //taskGroup_Id = q.TaskGroup_Id,
                        //taskGroup_Name = q.TaskGroup_Name,
                        //documentPriority_Status = q.DocumentPriority_Status,
                        documentRef_No1 = q.DocumentRef_No1,
                        //documentRef_No2 = q.DocumentRef_No2,
                        //documentRef_No3 = q.DocumentRef_No3,
                        //documentRef_No4 = q.DocumentRef_No4,
                        //documentRef_No5 = q.DocumentRef_No5,
                        document_Status = q.Document_Status,
                        //udf_1 = q.UDF_1,
                        //udf_2 = q.UDF_2,
                        //udf_3 = q.UDF_3,
                        //udf_4 = q.UDF_4,
                        //udf_5 = q.UDF_5,
                        //doTask_By = q.DoTask_By,
                        //doTask_Date = q.DoTask_Date,
                        userAssign = q.UserAssign,
                        plangoodsissue_Index = taskItem?.PlanGoodsIssue_Index.ToString(),
                        plangoodsissue_No = taskItem?.PlanGoodsIssue_No,
                        goodsissue_Index = taskItem?.Ref_Document_Index.ToString(),
                        goodsissue_No = taskItem?.Ref_Document_No
                    };
                    items.Add(item);
                }
                results.items = items;
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

        #region Filter ScanPick Qty
        public actionResultScanPicksearchViewModel FilterTaskPickQty(ScanPicksearchViewModel model)
        {
            try
            {
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskViewModel>();
                db.Database.SetCommandTimeout(120);
                //var query = db.View_Task.Where(c => c.Document_Status == 2 && c.Is_Picklabeling == "1" && (c.PickingLabeling_Status != null || c.PickingLabeling_Status != 0) && (c.location_new == null || c.location_new == "")).AsQueryable();
                var query = db.View_Task_export.Where(c => c.Document_Status == 2 && c.Document_StatusScanPick == 2 && c.Document_StatusLabeling == 2 && c.Document_StatusPickQty == null );

                if (!string.IsNullOrEmpty(model.task_No))
                {
                    query = query.Where(c => c.Task_No.Contains(model.task_No) || c.Ref_Document_No.Contains(model.task_No) || c.Tag_No == model.task_No);
                }
                var oldtask = "";
                foreach (var q in query.OrderByDescending(o => o.Task_No))
                {
                    if (oldtask == "")
                    {
                        oldtask = q.Task_No;
                    }
                    else {
                        if (oldtask == q.Task_No)
                        {
                            continue;
                        }
                        else {
                            oldtask = q.Task_No;
                        }
                    }
                    var taskItem = db.IM_TaskItem.FirstOrDefault(c => c.Task_Index == q.Task_Index);
                    var item = new taskViewModel
                    {
                        task_Index = q.Task_Index.ToString(),
                        task_No = q.Task_No,
                        documentRef_No1 = q.DocumentRef_No1,
                        document_Status = q.Document_Status,
                        userAssign = q.UserAssign,
                        plangoodsissue_Index = taskItem?.PlanGoodsIssue_Index.ToString(),
                        plangoodsissue_No = taskItem?.PlanGoodsIssue_No,
                        goodsissue_Index = taskItem?.Ref_Document_Index.ToString(),
                        goodsissue_No = taskItem?.Ref_Document_No
                    };
                    items.Add(item);
                }
                results.items = items;
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

        #region Filter ScanPick Dock
        public actionResultScanPicksearchViewModel FilterTaskLocationLabeling(ScanPicksearchViewModel model)
        {
            try
            {
                db.Database.SetCommandTimeout(120);
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<PickDockViewModel>();

                var query = db.View_ScanPickToDock.Where(c => c.PickingToDock_Status == null).ToList();

                if (!string.IsNullOrEmpty(model.task_No))
                {
                    query = query.Where(c => c.TruckLoad_No.Contains(model.task_No)).ToList();
                }

                foreach (var q in query.OrderBy(c=> c.Dock_Name))
                {
                    var item = new PickDockViewModel
                    {
                        TruckLoad_Index = q.TruckLoad_Index,
                        TruckLoad_No = q.TruckLoad_No,
                        Ref_Document_No = q.Ref_Document_No,
                        Ref_Document_Index = q.Ref_Document_Index,
                        Dock_Name = q.Dock_Name
                    };
                    items.Add(item);
                }
                results.itemsDock = items;
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
        //public actionResultScanPicksearchViewModel FilterTaskLocationLabeling(ScanPicksearchViewModel model)
        //{
        //    try
        //    {
        //        var results = new actionResultScanPicksearchViewModel();

        //        var items = new List<taskViewModel>();

        //        var query = db.View_TaskAftersplit.Where(c => c.Document_Status == 2 && c.Document_StatusScanPick == 2 && c.Document_StatusLabeling == 2 && c.Document_StatusPickQty == 2 && c.Document_StatusDocktoStg == null);

        //        if (!string.IsNullOrEmpty(model.task_No))
        //        {
        //            query = query.Where(c => c.Task_No.Contains(model.task_No) || c.Ref_Document_No.Contains(model.task_No));
        //        }

        //        foreach (var q in query.OrderByDescending(o => o.DocumentRef_No1))
        //        {
        //            var taskItem = db.IM_TaskItem.Where(c => c.Task_Index == q.Task_Index && c.PickingToDock_Status == null).OrderBy(c=> c.Tag_No).FirstOrDefault();
        //            var item = new taskViewModel
        //            {
        //                task_Index = q.Task_Index.ToString(),
        //                task_No = q.Task_No,
        //                documentRef_No1 = taskItem.Tag_No,
        //                document_Status = q.Document_Status,
        //                userAssign = q.UserAssign,
        //                plangoodsissue_Index = taskItem?.PlanGoodsIssue_Index.ToString(),
        //                plangoodsissue_No = taskItem?.PlanGoodsIssue_No,
        //                goodsissue_Index = taskItem?.Ref_Document_Index.ToString(),
        //                goodsissue_No = taskItem?.Ref_Document_No
        //            };
        //            items.Add(item);
        //        }
        //        results.items = items;
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

        #region Filter MoveToSelective
        public actionResultScanPicksearchViewModel FilterTaskMoveToSelective(ScanPicksearchViewModel model)
        {
            try
            {
                db.Database.SetCommandTimeout(120);
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<taskViewModel>();
                
                var query = db.View_Task.Where(c => c.Document_Status == 2 && c.Document_StatusScanPick == 2 && c.Document_StatusLabeling == 2 && c.Document_StatusPickQty == 2 && c.Document_StatusMovetoStgOG == null);

                if (!string.IsNullOrEmpty(model.task_No))
                {
                    query = query.Where(c => c.Task_No.Contains(model.task_No) || c.Ref_Document_No.Contains(model.task_No) || c.Tag_No == model.task_No);
                }
                var oldtask = "";
                foreach (var q in query.OrderByDescending(o => o.Task_No))
                {
                    if (oldtask == "")
                    {
                        oldtask = q.Task_No;
                    }
                    else
                    {
                        if (oldtask == q.Task_No)
                        {
                            continue;
                        }
                        else
                        {
                            oldtask = q.Task_No;
                        }
                    }
                    var taskItem = db.IM_TaskItem.FirstOrDefault(c => c.Task_Index == q.Task_Index);
                    if (taskItem.PickingLabeling_Location_Name.Contains("UP"))
                    {
                        continue;
                    }
                    var item = new taskViewModel
                    {
                        task_Index = q.Task_Index.ToString(),
                        task_No = q.Task_No,
                        documentRef_No1 = q.DocumentRef_No1,
                        documentRef_No4 = taskItem.DocumentRef_No4,
                        documentRef_No5 = taskItem ?.PickingLabeling_Location_Name,
                        document_Status = q.Document_Status,
                        userAssign = q.UserAssign,
                        plangoodsissue_Index = taskItem?.PlanGoodsIssue_Index.ToString(),
                        plangoodsissue_No = taskItem?.PlanGoodsIssue_No,
                        goodsissue_Index = taskItem?.Ref_Document_Index.ToString(),
                        goodsissue_No = taskItem?.Ref_Document_No
                    };
                    items.Add(item);
                }
                results.items = items;
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

        #region Filter FilterTaskStgtodock
        public actionResultScanPicksearchViewModel FilterTaskStgtodock(ScanPicksearchViewModel model)
        {
            try
            {
                db.Database.SetCommandTimeout(120);
                var results = new actionResultScanPicksearchViewModel();

                var items = new List<PickDockViewModel>();
                
                var query = db.View_ScanPicKSTG_To_Dock.Where(c => c.Document_StatusTracking == 1).ToList();

                if (!string.IsNullOrEmpty(model.task_No))
                {
                    //query = query.Where(c => model.task_No.Contains(c.Task_No));
                    query = query.Where(c => c.TruckLoad_No.Contains(model.task_No)).ToList();
                }

                foreach (var q in query.OrderBy(c => c.Dock_Name))
                {
                    var item = new PickDockViewModel
                    {
                        TruckLoad_Index = q.TruckLoad_Index,
                        TruckLoad_No = q.TruckLoad_No,
                        Ref_Document_No = q.Ref_Document_No,
                        Ref_Document_Index = q.Ref_Document_Index,
                        Dock_Name = q.Dock_Name
                    };
                    items.Add(item);
                }
                results.itemsDock = items;
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

        #endregion

        #region Scantag
        public actionResultScanPicksearchViewModel Scantag(ScanPicksearchViewModel model)
        {
            try
            {
                var Result = new actionResultScanPicksearchViewModel();

                var tag_no = db.IM_TaskItem.FirstOrDefault(c => c.Tag_No == model.tagNo && c.Task_Index == Guid.Parse(model.task_Index));
                if (tag_no == null)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "สแกนบาร์โค้ดไม่ถูกต้อง : " + model.tagNo;
                    return Result;
                }
                else
                {

                    var list = new List<taskItemViewModel> {
                        new taskItemViewModel{
                            pick_ProductConversion_Index = tag_no.ProductConversion_Index
                            ,pick_ProductConversion_Id = tag_no.ProductConversion_Id
                            ,pick_ProductConversion_Name = tag_no.ProductConversion_Name
                            ,pick_ProductConversion_Ratio = tag_no.Ratio
                        }
                    };
                    Result.resultIsUse = true;
                    Result.itemsDetail = list;
                }

                return Result;
            }
            catch (Exception ex)
            {
                var Result = new actionResultScanPicksearchViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region Scantag_Dock
        public actionResultScanPicksearchViewModel Scantag_Dock(ScanPicksearchViewModel model)
        {
            try
            {
                var Result = new actionResultScanPicksearchViewModel();

                var tag_no = db.View_Taskitem_with_Truckload.FirstOrDefault(c => c.Tag_No == model.tagNo && c.TruckLoad_Index == model.TruckLoad_Index);
                if (tag_no == null)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "สแกนบาร์โค้ดไม่ถูกต้อง : " + model.tagNo;
                    return Result;
                }
                else {
                    Result.resultIsUse = true;
                }

                return Result;
            }
            catch (Exception ex)
            {
                var Result = new actionResultScanPicksearchViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region ScantagMoveToSTG
        public actionResultScanPicksearchViewModel ScantagMoveToSTG(ScanPicksearchViewModel model)
        {
            try
            {
                var Result = new actionResultScanPicksearchViewModel();

                var tag_no = db.IM_TaskItem.FirstOrDefault(c => c.DocumentRef_No4 == model.tagNo && c.Task_Index == Guid.Parse(model.task_Index));
                if (tag_no == null)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "สแกนบาร์โค้ดไม่ถูกต้อง : " + model.tagNo;
                    return Result;
                }
                else
                {

                    var list = new List<taskItemViewModel> {
                        new taskItemViewModel{
                            pick_ProductConversion_Index = tag_no.ProductConversion_Index
                            ,pick_ProductConversion_Id = tag_no.ProductConversion_Id
                            ,pick_ProductConversion_Name = tag_no.ProductConversion_Name
                            ,pick_ProductConversion_Ratio = tag_no.Ratio
                        }
                    };
                    Result.resultIsUse = true;
                    Result.itemsDetail = list;
                }

                return Result;
            }
            catch (Exception ex)
            {
                var Result = new actionResultScanPicksearchViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region GetTagOut
        public actionResultScanPicksearchViewModel GetTagOut(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var Result = new actionResultScanPicksearchViewModel();

                var filterModel = new GenDocumentTypeViewModel();


                filterModel.process_Index = new Guid("8A46C0A7-977B-42E0-A527-2B5684C99800");
                filterModel.documentType_Index = new Guid("8029E6A6-5418-4631-9B76-25B814FDA4AF");
                //GetConfig
                var typeTagOut = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                DataTable resultDocumentType = GoodIssueService.CreateDataTable(typeTagOut);

                var DocumentType = new SqlParameter("DocumentType", SqlDbType.Structured);
                DocumentType.TypeName = "[dbo].[ms_DocumentTypeData]";
                DocumentType.Value = resultDocumentType;

                var DocumentType_Index = new SqlParameter("@DocumentType_Index", filterModel.documentType_Index.ToString());
                var DocDate = new SqlParameter("@DocDate", DateTime.Now);
                var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                resultParameter.Size = 2000; // some meaningfull value
                resultParameter.Direction = ParameterDirection.Output;
                db.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate, @DocumentType, @txtReturn OUTPUT", DocumentType_Index, DocDate, DocumentType, resultParameter);
                var tagOut_No = resultParameter.Value.ToString();

                var tagout = new wm_TagOut
                {
                    TagOut_Index = Guid.NewGuid(),
                    TagOut_No = tagOut_No,
                    TagOutRef_No1 = "",
                    TagOutRef_No2 = "",
                    TagOutRef_No3 = "",
                    TagOutRef_No4 = "",
                    TagOutRef_No5 = "",
                    TagOut_Status = 0,
                    UDF_1 = "",
                    UDF_2 = "",
                    UDF_3 = "",
                    UDF_4 = "",
                    UDF_5 = "",
                    Zone_Index = null,
                    Ref_Process_Index = new Guid("22744590-55D8-4448-88EF-5997C252111F"),
                    Ref_Document_No = null, //model.task_No,
                    Ref_Document_Index = null, //!string.IsNullOrEmpty(model.task_Index) ? new Guid(model.task_Index) : (Guid?)null,
                    Ref_DocumentItem_Index = null, //!string.IsNullOrEmpty(model.taskItem_Index) ? new Guid(model.taskItem_Index) : (Guid?)null,
                    Create_By = model.userName,
                    Create_Date = DateTime.Now,
                    isPrint = null,
                };

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.WM_TagOut.Add(tagout);
                    db.SaveChanges();
                    transaction.Commit();

                    var list = new List<taskItemViewModel> {
                        new taskItemViewModel{
                            tagOut_Index = tagout.TagOut_Index
                            ,tagOut_No = tagout.TagOut_No
                        }
                    };
                    Result.resultIsUse = true;
                    Result.itemsDetail = list;
                }

                catch (Exception exy)
                {
                    msglog = model.sJson() + " / " + State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("GetTagOut", msglog);
                    transaction.Rollback();
                    throw exy;
                }

                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("GetTagOut", msglog);
                var Result = new actionResultScanPicksearchViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region ScanCarton
        public actionResultScanPicksearchViewModel ScanCarton(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var Result = new actionResultScanPicksearchViewModel();
                var list = new List<taskItemViewModel>();


                var carton = db.WM_TagOut.Where(c => c.TagOut_No == model.tagOut_No).ToList();

                if (carton.Count() == 0)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Scan Carton Error : " + model.tagOut_No;
                    return Result;
                }
                else
                {
                    //if (model.isChkUpdate)
                    //{
                    //    var taskItem = db.IM_TaskItem.Where(c => c.Ref_Document_Index == new Guid(model.goodsissue_Index) && c.TagOut_No == model.tagOut_No).ToList();

                    //    foreach (var ti in taskItem)
                    //    {
                    //        ti.UDF_5 = model.location_Name;
                    //    }
                    //    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    //    try
                    //    {
                    //        db.SaveChanges();
                    //        transaction.Commit();
                    //    }

                    //    catch (Exception exy)
                    //    {
                    //        msglog = State + " ex Rollback " + exy.Message.ToString();
                    //        olog.logging("UpdateStatusPGII", msglog);
                    //        transaction.Rollback();
                    //        throw exy;
                    //    }
                    //}


                    foreach (var c in carton)
                    {
                        var l = new taskItemViewModel
                        {
                            tagOut_Index = c.TagOut_Index,
                            tagOut_No = c.TagOut_No
                        };
                        list.Add(l);
                    }
                    Result.resultIsUse = true;
                }

                Result.itemsDetail = list;


                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanCarton", msglog);
                var Result = new actionResultScanPicksearchViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion
        
        #region ScanConfirmAuto
        public String ScanConfirmAuto(string GI_No)
        {
            String State = "Start";
            String msglog = "";
            String msgretrun = "";
            var olog = new logtxt();
            try
            {

                var ModelsCTN = new ScanPicksearchViewModel();
                ModelsCTN.userName = "AUTO";
                var carton = GetTagOut(ModelsCTN);

                if (carton == null)
                {
                    return "";
                }

                if (carton.itemsDetail[0].tagOut_Index == null)
                {
                    return "";
                }


                //ScanPickViewModel
                var itemscan = new ScanPickViewModel();

                var queryView = db.IM_TaskItem.AsQueryable();
                queryView = queryView.Where(c => c.Ref_Document_No == GI_No && c.Picking_Status != 1 && c.Picking_Status != 2);
                var listdataView = queryView.ToList();

                foreach (var item in listdataView)
                {
                    itemscan.taskItem_Index = item.TaskItem_Index.ToString();
                    itemscan.task_Index = item.Task_Index.ToString();
                    itemscan.task_No = item.Task_No;
                    itemscan.lineNum = item.LineNum;
                    itemscan.tagItem_Index = item.TagItem_Index;
                    itemscan.tag_Index = item.Tag_Index;
                    itemscan.tag_No = item.Tag_No;

                    itemscan.product_Index = item.Product_Index;

                    itemscan.product_Id = item.Product_Id;

                    itemscan.product_Name = item.Product_Name;

                    itemscan.product_SecondName = item.Product_SecondName;

                    itemscan.product_ThirdName = item.Product_ThirdName;

                    itemscan.product_Lot = item.Product_Lot;

                    itemscan.itemStatus_Index = item.ItemStatus_Index;

                    itemscan.itemStatus_Id = item.ItemStatus_Id;

                    itemscan.itemStatus_Name = item.ItemStatus_Name;

                    itemscan.location_Index = item.Location_Index;

                    itemscan.location_Id = item.Location_Id;

                    itemscan.location_Name = item.Location_Name;

                    itemscan.qty = item.Qty;

                    itemscan.ratio = item.Ratio;

                    itemscan.totalQty = item.TotalQty;

                    itemscan.productConversion_Index = item.ProductConversion_Index;

                    itemscan.productConversion_Id = item.ProductConversion_Id;

                    itemscan.productConversion_Name = item.ProductConversion_Name;

                    itemscan.mfg_Date = item.MFG_Date;

                    itemscan.exp_Date = item.EXP_Date;

                    itemscan.unitWeight = item.UnitWeight;

                    itemscan.weight = item.Weight;

                    itemscan.unitWidth = item.UnitWidth;

                    itemscan.unitLength = item.UnitLength;

                    itemscan.unitHeight = item.UnitHeight;

                    itemscan.unitVolume = item.UnitVolume;
                    itemscan.volume = item.Volume;
                    itemscan.unitPrice = item.UnitPrice;
                    itemscan.price = item.Price;
                    itemscan.documentRef_No1 = item.DocumentRef_No1;
                    itemscan.documentRef_No2 = item.DocumentRef_No2;
                    itemscan.documentRef_No3 = item.DocumentRef_No3;
                    itemscan.documentRef_No4 = item.DocumentRef_No4;
                    itemscan.documentRef_No5 = item.DocumentRef_No5;
                    itemscan.document_Status = item.Document_Status;
                    itemscan.udf_1 = item.UDF_1;
                    itemscan.udf_2 = item.UDF_2;
                    itemscan.udf_3 = item.UDF_3;
                    itemscan.udf_4 = item.UDF_4;
                    itemscan.udf_5 = item.UDF_5;
                    itemscan.ref_Process_Index = item.Ref_Process_Index;
                    itemscan.ref_Document_No = item.Ref_Document_No;
                    itemscan.ref_Document_LineNum = item.Ref_Document_LineNum;
                    itemscan.ref_Document_Index = item.Ref_Document_Index;
                    itemscan.ref_DocumentItem_Index = item.Ref_DocumentItem_Index;
                    itemscan.reasonCode_Index = item.ReasonCode_Index;
                    itemscan.reasonCode_Id = item.ReasonCode_Id;
                    itemscan.reasonCode_Name = item.ReasonCode_Name;
                    itemscan.create_By = item.Create_By;
                    itemscan.create_Date = item.Create_Date;
                    itemscan.update_By = item.Update_By;
                    itemscan.update_Date = item.Update_Date;
                    itemscan.cancel_By = item.Cancel_By;
                    itemscan.cancel_Date = item.Cancel_Date;
                    itemscan.tagOutPick_Index = item.TagOutPick_Index;
                    itemscan.tagOutPick_No = item.TagOutPick_No;
                    itemscan.picking_Qty = item.Picking_Qty;
                    itemscan.picking_Ratio = item.Picking_Ratio;
                    itemscan.picking_TotalQty = item.Picking_TotalQty;
                    itemscan.picking_By = item.Picking_By;
                    itemscan.picking_Date = item.Picking_Date;
                    itemscan.picking_Status = item.Picking_Status;
                    itemscan.splitQty = item.splitQty;
                    itemscan.planGoodsIssue_Index = item.PlanGoodsIssue_Index;
                    itemscan.planGoodsIssueItem_Index = item.PlanGoodsIssueItem_Index;
                    itemscan.planGoodsIssue_No = item.PlanGoodsIssue_No;





                    itemscan.pick_ProductConversion_Index = item.ProductConversion_Index;
                    itemscan.pick_ProductConversion_Id = item.ProductConversion_Id;
                    itemscan.pick_ProductConversion_Name = item.ProductConversion_Name;
                    itemscan.pick_ProductConversion_Ratio = item.Ratio;
                    itemscan.productConversionBarcode = item.ProductConversionBarcode;
                    itemscan.tagOut_Index = carton.itemsDetail[0].tagOut_Index;
                    itemscan.tagOut_No = carton.itemsDetail[0].tagOut_No;


                    itemscan.confirmlocation_Index = item.Location_Index;
                    itemscan.confirmlocation_Name = item.Location_Name;
                    itemscan.pick_Qty = item.Qty;
                    itemscan.userName = "Auto";


                    msgretrun += GI_No + " " + item.LineNum + " ";

                    var result = ScanConfirm(itemscan);

                    msgretrun += result.resultMsg;
                    msgretrun += Environment.NewLine;

                }





                return msgretrun;
            }
            catch (Exception ex)
            {
                //msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                //olog.logging("ScanConfirm", msglog);
                //var Result = new actionResultScanPickViewModel();
                //Result.resultIsUse = false;
                //Result.resultMsg = ex.Message;
                msgretrun += ex.Message.ToString();
                return msgretrun;
            }
        }
        #endregion
        
        #region Film Cutting

        #region Scan Confirm for Film cutting
        public actionResultScanPickViewModel ScanConfirmforFilmcutting(ScanPickViewModel model)
        {
            logtxt log = new logtxt();
            log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" + DateTime.Now.ToString("yyyy-MM-dd"), "model : " + JsonConvert.SerializeObject(model) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
                var Result = new actionResultScanPickViewModel();
                //----------------------------------------------------------------------------------------------------------- step 1 -----------------------------------------------------------------------------------------------------------
                var ListTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == new Guid(model.task_Index)
                && c.Tag_Index == model.tag_Index
                && c.Product_Id == model.product_Id
                && c.Product_Index == model.product_Index
                ).ToList();
                log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" + DateTime.Now.ToString("yyyy-MM-dd"), "model step 1 : " + JsonConvert.SerializeObject(ListTaskItem) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                if (ListTaskItem.Count() == 0)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }

                decimal? RemainScanQty = ListTaskItem.Sum(s => s.TotalQty) - ListTaskItem.Sum(s => s.Picking_TotalQty);
                decimal? ScanPick = (model.pick_Qty * model.pick_ProductConversion_Ratio);
                if (ScanPick == RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        TaskItem.Picking_Qty = TaskItem.Qty;
                        TaskItem.Picking_Ratio = TaskItem.Ratio;
                        TaskItem.Picking_TotalQty = TaskItem.TotalQty;
                        TaskItem.Picking_By = model.userName;
                        TaskItem.Picking_Date = DateTime.Now;
                        TaskItem.Picking_Status = 1;
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                        
                    }
                }
                else if (ScanPick < RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        TaskItem.Picking_Qty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.Picking_Ratio = model.pick_ProductConversion_Ratio;
                        TaskItem.Picking_TotalQty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.Picking_By = model.userName;
                        TaskItem.Picking_Date = DateTime.Now;
                        TaskItem.Picking_Status = 1;
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                        ScanPick = ScanPick == 0 ? 0 : ScanPick - TaskItem.TotalQty;
                        
                    }
                }
                else
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        TaskItem.Picking_By = model.userName;
                        TaskItem.Picking_Date = DateTime.Now;
                        TaskItem.Picking_Status = 1;
                    }
                }

                //var Product = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), new { model.product_Index }.sJson());

                //----------------------------------------------------------------------------------------------------------- step 2 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" + DateTime.Now.ToString("yyyy-MM-dd"), "model step 2 : Save => " +  DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    Result.resultIsUse = true;
                    log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" + DateTime.Now.ToString("yyyy-MM-dd"), "model step 2 : Save Time => " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                }

                catch (Exception exy)
                {
                    log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" + DateTime.Now.ToString("yyyy-MM-dd"), "model step 2 : Error Time => " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" + DateTime.Now.ToString("yyyy-MM-dd"), "model step 2 : Error MSG => " + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("ScanConfirm", msglog);
                    transaction.Rollback();
                    throw exy;
                }
                //}

                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirm", msglog);
                var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region Scan Confirm for Location Film cutting
        public bool ScanConfirmLocatonFilmcutting(ScanPicksearchViewModel model)
        {
            logtxt log = new logtxt();
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
                var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());
                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = model.location_Name } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;
                
                var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                if (CheckTag.Count() == 0)
                {
                    var tag2 = new { tag_no = model.location_Name };
                    var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
                    CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                }
                var task_Index = new Guid(model.task_Index);
                var data = db.View_TaskInsertBinCard.Where(c => c.Task_Index == task_Index && c.PickStatus == 1).ToList();
                foreach (var d in data)
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
                        picking_Qty = d.Picking_Qty,
                        picking_Ratio = d.Picking_Ratio,
                        picking_TotalQty = d.Picking_TotalQty,
                        binBalance_Index = d.BinBalance_Index,
                        process_Index = new Guid(configLocation),
                        location_Index_To = new Guid(model.location_Index),
                        location_Id_To = model.location_Id,
                        location_Name_To = model.location_Name,
                        Volume = d.Volume,
                        Weight = d.Weight,
                        userName = model.userName
                    };
                    //----------------------------------------------------------------------------------------------------------- step 6 -----------------------------------------------------------------------------------------------------------
                    //var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("insertBincard"), View_TaskInsertBinCard.sJson());
                    var BinbalanceCutService = new BinbalanceCutService();
                    var Bincard = BinbalanceCutService.InsertBinCard(View_TaskInsertBinCard);
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);
                        taskitem.Picking_Status = 2;
                        taskitem.UDF_5 = model.location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" , "step 7 : Save Error exy : " + exy.Message.ToString() +" : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                var chkTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == task_Index && (c.Picking_Status == 1 || (c.Picking_Status != null ? c.Picking_Status : 0) == 0)).Count();
                if (chkTaskItem == 0)
                {
                    var task = db.IM_Task.Find(task_Index);

                    var taskitem = db.IM_TaskItem.Where(c => c.Task_Index == task.Task_Index && c.Picking_Status == 2).GroupBy(g => g.Ref_Document_Index);
                    var GI = db.IM_GoodsIssue.Where(c => taskitem.Select(s => s.Key).Contains(c.GoodsIssue_Index)).ToList();
                    foreach (var gi in GI)
                    {
                        gi.Document_Status = 3;
                    }


                    task.Document_Status = 2;
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
                        log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" , "step 10 : Save Task End Error exy" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirmforFilmcutting", "ScanConfirmforFilmcutting" , "EX" + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion
        
        #region ScanPick

        #region Scan Location ScanPick
        public List<locationViewModel> ScanLocatonScanPick(ScanPicksearchViewModel model)
        {
            try
            {
                var location = new List<locationViewModel>();
                if (!string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    var objectLocation = new { location_Name = model.confirm_location_Id, locationType_Index = "F9EDDAEC-A893-4F63-A700-526C69CC08C0" };
                    location = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), objectLocation.sJson());
                    
                }
                return location;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Scan Confirm ScanPick
        public actionResultScanPickViewModel ScanConfirm(ScanPickViewModel model)
        {
            db.Database.SetCommandTimeout(120);
            String State = "Start";
            String msglog = "";
            logtxt log = new logtxt();
            try
            {
                var Result = new actionResultScanPickViewModel();
                if (string.IsNullOrEmpty(model.confirm_location_Index))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Name))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                var ListTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == new Guid(model.task_Index) && c.Tag_Index == model.tag_Index && c.Product_Id == model.product_Id && c.Product_Index == model.product_Index).ToList();

                if (ListTaskItem.Count() == 0)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }

                decimal? RemainScanQty = ListTaskItem.Sum(s => s.TotalQty) - ListTaskItem.Sum(s => s.Picking_TotalQty);
                decimal? ScanPick = (model.pick_Qty * model.pick_ProductConversion_Ratio);
                if (ScanPick == RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        TaskItem.Picking_Qty = TaskItem.Qty;
                        TaskItem.Picking_Ratio = TaskItem.Ratio;
                        TaskItem.Picking_TotalQty = TaskItem.TotalQty;
                        TaskItem.Picking_By = model.userName;
                        TaskItem.Picking_Date = DateTime.Now;
                        TaskItem.Picking_Status = 1;
                        TaskItem.Picking_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.Picking_Location_Id = model.confirm_location_Id;
                        TaskItem.Picking_Location_Name = model.confirm_location_Name;
                        
                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                    }
                }
                else if (ScanPick < RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        TaskItem.Picking_Qty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.Picking_Ratio = model.pick_ProductConversion_Ratio;
                        TaskItem.Picking_TotalQty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.Picking_By = model.userName;
                        TaskItem.Picking_Date = DateTime.Now;
                        TaskItem.Picking_Status = 1;
                        TaskItem.Picking_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.Picking_Location_Id = model.confirm_location_Id;
                        TaskItem.Picking_Location_Name = model.confirm_location_Name;
                        
                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                        ScanPick = ScanPick == 0 ? 0 : ScanPick - TaskItem.TotalQty;
                    }
                }
                else
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        TaskItem.Picking_By = model.userName;
                        TaskItem.Picking_Date = DateTime.Now;
                        TaskItem.Picking_Status = 1;
                        TaskItem.Picking_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.Picking_Location_Id = model.confirm_location_Id;
                        TaskItem.Picking_Location_Name = model.confirm_location_Name;
                    }
                }
                var transaction = db.Database.BeginTransaction();
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    Result.resultIsUse = true;
                }
                catch (Exception exy)
                {
                    log.DataLogLines("ScanConfirm", "ScanConfirm", "Save Error EXY => "+exy.Message.ToString()+" : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    transaction.Rollback();
                    throw exy;
                }
                if (Result.resultIsUse)
                {
                    ScanPicksearchViewModel new_model = new ScanPicksearchViewModel();
                    new_model.confirm_location_Name = model.confirm_location_Name;
                    new_model.task_Index = model.task_Index;
                    new_model.confirm_location_Index = model.confirm_location_Index;
                    new_model.confirm_location_Id = model.confirm_location_Id;
                    new_model.userName = model.userName;
                    var re = ScanConfirmLocaton(new_model);
                    if (!re)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task Error";
                        return Result;
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirm", "ScanConfirm", "Save Error EX => " + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region Scan Confirm Locaton ScanPick
        public bool ScanConfirmLocaton(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            db.Database.SetCommandTimeout(120);
            logtxt log = new logtxt();
            try
            {
                var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());
                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = model.confirm_location_Name } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                if (CheckTag.Count() == 0)
                {
                    var tag2 = new { tag_no = model.confirm_location_Name };var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                }
                var task_Index = new Guid(model.task_Index);var data = db.View_TaskInsertBinCard.Where(c => c.Task_Index == task_Index && c.PickStatus == 1).ToList();
                foreach (var d in data)
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
                        picking_Qty = d.Picking_Qty,
                        picking_Ratio = d.Picking_Ratio,
                        picking_TotalQty = d.Picking_TotalQty,
                        binBalance_Index = d.BinBalance_Index,
                        process_Index = new Guid(configLocation),
                        location_Index_To = new Guid(model.confirm_location_Index),
                        location_Id_To = model.confirm_location_Id,
                        location_Name_To = model.confirm_location_Name,
                        Volume = d.Volume,
                        Weight = d.Weight,
                        userName = model.userName,
                        isScanPick = true,
                        isScanToDock = false
                    };
                    //----------------------------------------------------------------------------------------------------------- step 6 -----------------------------------------------------------------------------------------------------------
                    //var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardBy_Scanpick"), View_TaskInsertBinCard.sJson());

                    var BinbalanceCutService = new BinbalanceCutService();
                    var Bincard = BinbalanceCutService.InsertBinCardBy_Scanpick(View_TaskInsertBinCard);
                    if (!string.IsNullOrEmpty(Bincard))
                    {


                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);
                        taskitem.Picking_Status = 2;
                        taskitem.Document_Status = 2;
                        taskitem.UDF_5 = model.confirm_location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        taskitem.DocumentRef_No4 = taskitem.Tag_No;
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocaton", "ScanConfirmLocaton", "step 7 : Save Error : " + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("ScanConfirmLocatonPick", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                var chkTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == task_Index && (c.Picking_Status == 1 || (c.Picking_Status != null ? c.Picking_Status : 0) == 0)).Count();
                if (chkTaskItem == 0)
                {
                    var task = db.IM_Task.Find(task_Index);

                    var taskitem = db.IM_TaskItem.Where(c => c.Task_Index == task.Task_Index && c.Picking_Status == 2).GroupBy(g => g.Ref_Document_Index);
                    var GI = db.IM_GoodsIssue.Where(c => taskitem.Select(s => s.Key).Contains(c.GoodsIssue_Index)).ToList();
                    foreach (var gi in GI)
                    {
                        gi.Document_Status = 3;
                    }


                    task.Document_Status = 2;
                    task.DocumentRef_No1 = model.confirm_location_Name;
                    task.Document_StatusScanPick = 2;
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
                        log.DataLogLines("ScanConfirmLocaton", "ScanConfirmLocaton", "step 10 : Save Task End Error" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirmLocaton", "ScanConfirmLocaton", "EX" + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion

        #region ScanPick Labeling

        #region Scan Location ScanPick Labeling
        public List<locationViewModel> ScanLocatonLabeling(ScanPicksearchViewModel model)
        {
            try
            {
                var location = new List<locationViewModel>();
                if (!string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    View_CheckLocation checklocation = dbMaster.View_CheckLocation.FirstOrDefault(c => c.Location_Id == model.confirm_location_Id);
                    if (checklocation == null)
                    {
                        return location;
                    }
                    var objectLocation = new { location_Name = model.confirm_location_Id, locationType_Index = "48F83BB5-7807-4B32-9E3C-74962CEF92E8" };
                    location = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), objectLocation.sJson());
                }
                

                return location;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Scan Confirm ScanPick Labeling
        public actionResultScanPickViewModel ScanConfirmLabeling(ScanPickViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
                var Result = new actionResultScanPickViewModel();
                if (string.IsNullOrEmpty(model.confirm_location_Index))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Name))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                var ListTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == new Guid(model.task_Index)
                && c.Tag_Index == model.tag_Index
                && c.Product_Id == model.product_Id
                && c.Product_Index == model.product_Index
                ).ToList();

                if (ListTaskItem.Count() == 0)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }

                decimal? RemainScanQty = ListTaskItem.Sum(s => s.TotalQty) - ListTaskItem.Sum(s => s.PickingLabel_TotalQty);
                decimal? ScanPick = (model.pick_Qty * model.pick_ProductConversion_Ratio);
                if (ScanPick == RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        
                        //TaskItem.PlanGoodsIssue_Index = model.planGoodsIssue_Index;
                        //TaskItem.PlanGoodsIssueItem_Index = model.planGoodsIssueItem_Index;
                        //TaskItem.PlanGoodsIssue_No = model.planGoodsIssue_No;
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        TaskItem.PickingLabel_Qty = TaskItem.Qty;
                        TaskItem.PickingLabel_Ratio = TaskItem.Ratio;
                        TaskItem.PickingLabel_TotalQty = TaskItem.TotalQty;
                        TaskItem.PickingLabeling_By = model.userName;
                        TaskItem.PickingLabeling_Date = DateTime.Now;
                        TaskItem.PickingLabeling_Status = 1;
                        TaskItem.PickingLabeling_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingLabeling_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingLabeling_Location_Name = model.confirm_location_Name;


                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                    }
                }
                else if (ScanPick < RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        
                        //TaskItem.PlanGoodsIssue_Index = model.planGoodsIssue_Index;
                        //TaskItem.PlanGoodsIssueItem_Index = model.planGoodsIssueItem_Index;
                        //TaskItem.PlanGoodsIssue_No = model.planGoodsIssue_No;
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        TaskItem.PickingLabel_Qty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.PickingLabel_Ratio = model.pick_ProductConversion_Ratio;
                        TaskItem.PickingLabel_TotalQty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.PickingLabeling_By = model.userName;
                        TaskItem.PickingLabeling_Date = DateTime.Now;
                        TaskItem.PickingLabeling_Status = 1;
                        TaskItem.PickingLabeling_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingLabeling_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingLabeling_Location_Name = model.confirm_location_Name;

                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                        ScanPick = ScanPick == 0 ? 0 : ScanPick - TaskItem.TotalQty;

                    }

                }
                else
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        TaskItem.PickingLabeling_By = model.userName;
                        TaskItem.PickingLabeling_Date = DateTime.Now;
                        TaskItem.PickingLabeling_Status = 1;
                        TaskItem.PickingLabeling_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingLabeling_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingLabeling_Location_Name = model.confirm_location_Name;
                    }
                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    Result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("ScanConfirm", msglog);
                    transaction.Rollback();
                    throw exy;
                }
                if (Result.resultIsUse)
                {
                    ScanPicksearchViewModel new_model = new ScanPicksearchViewModel();
                    new_model.confirm_location_Name = model.confirm_location_Name;
                    new_model.task_Index = model.task_Index;
                    new_model.confirm_location_Index = model.confirm_location_Index;
                    new_model.confirm_location_Id = model.confirm_location_Id;
                    new_model.userName = model.userName;
                    var re = ScanConfirmLocatonPickingLabel(new_model);
                    if (!re)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task Error";
                        return Result;
                    }
                }
                //}

                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirm", msglog);
                var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region Scan Confirm Location ScanPick Labeling
        public bool ScanConfirmLocatonPickingLabel(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            logtxt log = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
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
                var data = db.View_TaskInsertBinCard_V2.Where(c => c.Task_Index == task_Index && c.PickingLabeling_Status == 1).ToList();
                foreach (var d in data)
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
                        isScanToDock = false
                    };

                    //var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardBy_Scanpick"), View_TaskInsertBinCard.sJson());
                    var BinbalanceCutService = new BinbalanceCutService();
                    var Bincard = BinbalanceCutService.InsertBinCardBy_Scanpick(View_TaskInsertBinCard);
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var bin_index = new { binbalance_Index = Bincard };
                        var findBin = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), bin_index.sJson());

                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);
                        taskitem.PickingLabeling_Status = 2;
                        taskitem.UDF_5 = model.confirm_location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        taskitem.DocumentRef_No4 = taskitem.Tag_No;
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocatonPickingLabel", "ScanConfirmLocatonPickingLabel", "step 7 : Save Error exy: " + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                var chkTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == task_Index && (c.PickingLabeling_Status == 1 || (c.PickingLabeling_Status != null ? c.PickingLabeling_Status : 0) == 0)).Count();
                if (chkTaskItem == 0)
                {
                    var task = db.IM_Task.Find(task_Index);

                    var taskitem = db.IM_TaskItem.Where(c => c.Task_Index == task.Task_Index && c.PickingLabeling_Status == 2).GroupBy(g => g.Ref_Document_Index);
                    var GI = db.IM_GoodsIssue.Where(c => taskitem.Select(s => s.Key).Contains(c.GoodsIssue_Index)).ToList();
                    foreach (var gi in GI)
                    {
                        gi.Document_Status = 3;
                    }

                    task.DocumentRef_No1 = model.confirm_location_Name;
                    task.Document_StatusLabeling = 2;
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
                        log.DataLogLines("ScanConfirmLocatonPickingLabel", "ScanConfirmLocatonPickingLabel", "step 10 : Save Task End Error exy" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirmLocatonPickingLabel", "ScanConfirmLocatonPickingLabel", "EX" + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion

        #region ScanPick Qty

        #region ScanCheckTagout
        public Result ScanCheckTagout (ScanPickViewModel_Qty model)
        {
            Result result = new Result();
            try
            {
                if (!string.IsNullOrEmpty(model.tagOut_No) && !string.IsNullOrEmpty(model.task_Index))
                {
                    var tagout = db.WM_TagOut.FirstOrDefault(c=> c.TagOut_No == model.tagOut_No);
                    if (tagout != null)
                    {
                        var checkmatch_task = db.IM_TaskItem.Where(c => c.Task_Index == Guid.Parse(model.task_Index) && c.Ref_DocumentItem_Index == tagout.Ref_DocumentItem_Index).ToList();
                        if (checkmatch_task.Count <= 0)
                        {
                            result.resultIsUse = false;
                            result.resultMsg = "Tagout ไม่ถูกต้อง";
                            return result;
                        }
                    }
                }
                else {
                    result.resultIsUse = false;
                    result.resultMsg = "Scan Error";
                    return result;
                }
                
                result.resultIsUse = true;
                return result;
            }
            catch (Exception ex)
            {
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }
        }
        #endregion

        #region Scan Location ScanPick Qty
        public List<locationViewModel> ScanLocatonQty(ScanPicksearchViewModel model)
        {
            try
            {
                var location = new List<locationViewModel>();
                if (!string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    if (model.Dock_Index != null)
                    {
                        var Docklocation = db.View_Get_location_DoclSTG.FirstOrDefault(c => c.Dock_Index == model.Dock_Index && c.Location_Labeling_Id == model.confirm_location_Id);
                        if (Docklocation == null)
                        {
                            return location;
                        }
                    }
                    //View_CheckLocation checklocation = dbMaster.View_CheckLocation.FirstOrDefault(c => c.Location_Id == model.confirm_location_Id);
                    //if (checklocation == null)
                    //{
                    //    return location;
                    //}
                    var objectLocation = new { location_Name = model.confirm_location_Id, locationType_Index = "550b4c74-56cc-4d11-9228-f2656d8fa3f6" };
                    location = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), objectLocation.sJson());
                }


                return location;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Scan Location ScanPick Qty
        public List<locationViewModel> ScanLocatonQtyCool(ScanPicksearchViewModel model)
        {
            try
            {
                var location = new List<locationViewModel>();
                if (!string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    //if (model.Dock_Index != null)
                    //{

                    //    var Docklocation = db.View_Get_location_DoclSTG.FirstOrDefault(c => c.Dock_Index == model.Dock_Index && c.Location_Labeling_Id == model.confirm_location_Id);
                    //    if (Docklocation == null)
                    //    {
                    //        return location;
                    //    }
                    //}
                    View_CheckLocation checklocation = dbMaster.View_CheckLocation.FirstOrDefault(c => c.Location_Id == model.confirm_location_Id);
                    if (checklocation == null)
                    {
                        return location;
                    }
                    var objectLocation = new { location_Name = model.confirm_location_Id, locationType_Index = "0c547af3-c6f6-405e-a9d8-81805cd79735" };
                    location = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), objectLocation.sJson());
                }


                return location;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Scan Location Print tag
        public Result ScanQty_print_tag(ScanPickViewModel_Qty model)
        {
            Result result = new Result();
            logtxt olog = new logtxt();
            try
            {
                olog.DataLogLines("ScanQty_print_tag", "ScanQty_print_tag"+DateTime.Now, "--------------------------------------------------------------------------------");
                olog.DataLogLines("ScanQty_print_tag", "ScanQty_print_tag"+DateTime.Now, "Goodissue No      : " + DateTime.Now +" : "+ model.ref_Document_No);
                olog.DataLogLines("ScanQty_print_tag", "ScanQty_print_tag"+DateTime.Now, "Goodissue Index   : " + DateTime.Now +" : "+ model.ref_Document_Index);
                olog.DataLogLines("ScanQty_print_tag", "ScanQty_print_tag"+DateTime.Now, "tag No            : " + DateTime.Now +" : " + model.tag_No);
                
                List<string> planGoodsIssue_No = model.plangoodsissue.Select(c => c.planGoodsIssue_No).ToList();
                olog.DataLogLines("ScanQty_print_tag", "ScanQty_print_tag" + DateTime.Now, "planGoodsIssue No : " + DateTime.Now + " : " + planGoodsIssue_No);
                List<Guid> goodsIssueItemLocation = db.IM_GoodsIssueItemLocation.Where(c => c.Tag_No == model.tag_No 
                                                                                    && c.GoodsIssue_No == model.ref_Document_No 
                                                                                    && c.GoodsIssue_Index == model.ref_Document_Index
                                                                                    && planGoodsIssue_No.Contains(c.Ref_Document_No)
                                                                                    && c.Document_Status != -1
                                                                                    ).Select(x=> x.GoodsIssueItemLocation_Index).ToList();

                var tagOutItem = db.WM_TagOutItem.Where(c => goodsIssueItemLocation.Contains(c.GoodsIssueItemLocation_Index.GetValueOrDefault()))
                                                                 .GroupBy(c=> new {
                                                                     c.TagOut_No,
                                                                     c.TagOut_Index
                                                                 })
                                                                 .Select(c=> new {
                                                                     c.Key.TagOut_No,
                                                                     c.Key.TagOut_Index
                                                                 }).ToList();

                List<Send_TagWCS> send_TagWCs = new List<Send_TagWCS>();
                foreach (var item in tagOutItem)
                {
                    Send_TagWCS tagWCS = new Send_TagWCS();
                    tagWCS.TagOut_Index = item.TagOut_Index;
                    tagWCS.TagOut_No = item.TagOut_No;
                    tagWCS.printer_id = model.printer.Printer_Id;
                    tagWCS.user_id = model.userName;
                    send_TagWCs.Add(tagWCS);
                }


                var printer_result = utils.SendDataApi<result_WCS>(new AppSettingConfig().GetUrl("Send_printer_WCS"), JsonConvert.SerializeObject(send_TagWCs));

                if (printer_result.status != 10)
                {
                    result.resultIsUse = false;
                    result.resultMsg = printer_result.message.description;
                    return result;
                }

                result.resultIsUse = true;
                return result;
            }
            catch (Exception ex)
            {
                olog.DataLogLines("ScanQty_print_tag", "ScanQty_print_tag" + DateTime.Now, "Error : "+ ex);
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;

            }
        }
        #endregion

        #region Scan Confirm ScanPick Qty
        public actionResultScanPickViewModel ScanConfirmPickQty(ScanPickViewModel_Qty model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
                var Result = new actionResultScanPickViewModel();
                if (string.IsNullOrEmpty(model.confirm_location_Index))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Name))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                var GetTruckload_Task = db.View_Taskitem_with_Truckload.Where(c => c.Task_Index == Guid.Parse(model.task_Index) && c.TruckLoad_Index == model.TruckLoad_Index).Select(c => c.TaskItem_Index).ToList();
                var ListTaskItem = db.IM_TaskItem.Where(c => GetTruckload_Task.Contains(c.TaskItem_Index)
                && c.Tag_Index == model.tag_Index
                && c.Product_Id == model.product_Id
                && c.Product_Index == model.product_Index
                ).ToList();

                //var ListTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == new Guid(model.task_Index)
                //&& c.Tag_Index == model.tag_Index
                //&& c.Product_Id == model.product_Id
                //&& c.Product_Index == model.product_Index
                //).ToList();

                if (ListTaskItem.Count() == 0)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }

                decimal? RemainScanQty = ListTaskItem.Sum(s => s.TotalQty) - ListTaskItem.Sum(s => s.PickingLabel_TotalQty);
                decimal? ScanPick = (model.pick_Qty * model.pick_ProductConversion_Ratio);
                if (ScanPick == RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {

                        //TaskItem.PlanGoodsIssue_Index = model.planGoodsIssue_Index;
                        //TaskItem.PlanGoodsIssueItem_Index = model.planGoodsIssueItem_Index;
                        //TaskItem.PlanGoodsIssue_No = model.planGoodsIssue_No;
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        TaskItem.PickingPickQty_Qty = TaskItem.Qty;
                        TaskItem.PickingPickQty_Ratio = TaskItem.Ratio;
                        TaskItem.PickingPickQty_TotalQty = TaskItem.TotalQty;
                        TaskItem.PickingPickQty_By = model.userName;
                        TaskItem.PickingPickQty_Date = DateTime.Now;
                        TaskItem.PickingPickQty_Status = 1;
                        TaskItem.PickingPickQty_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingPickQty_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingPickQty_Location_Name = model.confirm_location_Name;


                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                    }
                }
                else if (ScanPick < RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {

                        //TaskItem.PlanGoodsIssue_Index = model.planGoodsIssue_Index;
                        //TaskItem.PlanGoodsIssueItem_Index = model.planGoodsIssueItem_Index;
                        //TaskItem.PlanGoodsIssue_No = model.planGoodsIssue_No;
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        TaskItem.PickingPickQty_Qty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.PickingPickQty_Ratio = model.pick_ProductConversion_Ratio;
                        TaskItem.PickingPickQty_TotalQty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.PickingPickQty_By = model.userName;
                        TaskItem.PickingPickQty_Date = DateTime.Now;
                        TaskItem.PickingPickQty_Status = 1;
                        TaskItem.PickingPickQty_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingPickQty_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingPickQty_Location_Name = model.confirm_location_Name;

                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                        ScanPick = ScanPick == 0 ? 0 : ScanPick - TaskItem.TotalQty;

                    }

                }
                else
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        TaskItem.PickingPickQty_By = model.userName;
                        TaskItem.PickingPickQty_Date = DateTime.Now;
                        TaskItem.PickingPickQty_Status = 1;
                        TaskItem.PickingPickQty_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingPickQty_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingPickQty_Location_Name = model.confirm_location_Name;
                    }
                }

                foreach (var item in model.serailnumber)
                {
                    wm_TagOut tagOut = db.WM_TagOut.FirstOrDefault(c => c.TagOut_No == item.tagOut_No);
                    if (tagOut == null)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Tagout not found";
                        return Result;
                    }
                    im_GoodsIssueItemLocation goodsIssueItemLocation = db.IM_GoodsIssueItemLocation.FirstOrDefault(c => c.GoodsIssueItemLocation_Index == tagOut.Ref_DocumentItem_Index);
                    if (tagOut == null)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "GI not found";
                        return Result;
                    }
                    im_TaskItem taskItem = db.IM_TaskItem.FirstOrDefault(c => c.Ref_DocumentItem_Index == tagOut.Ref_DocumentItem_Index);
                    if (tagOut == null)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task not found";
                        return Result;
                    }
                    im_GoodsIssueItemLocationSN goodsIssueItemLocationSN = new im_GoodsIssueItemLocationSN();
                    goodsIssueItemLocationSN.GoodsIssueItemLocationSN_Index     = Guid.NewGuid();
                    goodsIssueItemLocationSN.GoodsIssueItemLocation_Index = goodsIssueItemLocation.GoodsIssueItemLocation_Index;
                    goodsIssueItemLocationSN.GoodsIssue_Index = goodsIssueItemLocation.GoodsIssue_Index;
                    goodsIssueItemLocationSN.LineNum = goodsIssueItemLocation.LineNum;
                    goodsIssueItemLocationSN.TagItem_Index = goodsIssueItemLocation.TagItem_Index;
                    goodsIssueItemLocationSN.Tag_Index = goodsIssueItemLocation.Tag_Index;
                    goodsIssueItemLocationSN.Tag_No = goodsIssueItemLocation.Tag_No;
                    goodsIssueItemLocationSN.Product_Index = goodsIssueItemLocation.Product_Index;
                    goodsIssueItemLocationSN.Product_Id = goodsIssueItemLocation.Product_Id;
                    goodsIssueItemLocationSN.Product_Name = goodsIssueItemLocation.Product_Name;  
                    goodsIssueItemLocationSN.Product_SecondName = goodsIssueItemLocation.Product_SecondName;  
                    goodsIssueItemLocationSN.Product_ThirdName = goodsIssueItemLocation.Product_ThirdName;  
                    goodsIssueItemLocationSN.Product_Lot = goodsIssueItemLocation.Product_Lot;  
                    goodsIssueItemLocationSN.ItemStatus_Index = goodsIssueItemLocation.ItemStatus_Index;  
                    goodsIssueItemLocationSN.ItemStatus_Id = goodsIssueItemLocation.ItemStatus_Id;  
                    goodsIssueItemLocationSN.ItemStatus_Name = goodsIssueItemLocation.ItemStatus_Name;  
                    goodsIssueItemLocationSN.Location_Index = goodsIssueItemLocation.Location_Index;  
                    goodsIssueItemLocationSN.Location_Id = goodsIssueItemLocation.Location_Id;  
                    goodsIssueItemLocationSN.Location_Name = goodsIssueItemLocation.Location_Name;  
                    goodsIssueItemLocationSN.QtyPlan = goodsIssueItemLocation.QtyPlan;  
                    goodsIssueItemLocationSN.Qty = goodsIssueItemLocation.Qty;  
                    goodsIssueItemLocationSN.Ratio = goodsIssueItemLocation.Ratio;  
                    goodsIssueItemLocationSN.TotalQty = goodsIssueItemLocation.TotalQty;  
                    goodsIssueItemLocationSN.ProductConversion_Index = goodsIssueItemLocation.ProductConversion_Index;  
                    goodsIssueItemLocationSN.ProductConversion_Id = goodsIssueItemLocation.ProductConversion_Id;  
                    goodsIssueItemLocationSN.ProductConversion_Name = goodsIssueItemLocation.ProductConversion_Name;  
                    goodsIssueItemLocationSN.MFG_Date = goodsIssueItemLocation.MFG_Date;  
                    goodsIssueItemLocationSN.EXP_Date = goodsIssueItemLocation.EXP_Date;  
                    goodsIssueItemLocationSN.UnitWeight = goodsIssueItemLocation.UnitWeight;  
                    goodsIssueItemLocationSN.UnitWeight_Index = goodsIssueItemLocation.UnitWeight_Index;  
                    goodsIssueItemLocationSN.UnitWeight_Id = goodsIssueItemLocation.UnitWeight_Id;  
                    goodsIssueItemLocationSN.UnitWeight_Name = goodsIssueItemLocation.UnitWeight_Name;  
                    goodsIssueItemLocationSN.UnitWeightRatio = goodsIssueItemLocation.UnitWeightRatio;  
                    goodsIssueItemLocationSN.Weight = goodsIssueItemLocation.Weight;  
                    goodsIssueItemLocationSN.Weight_Index = goodsIssueItemLocation.Weight_Index;  
                    goodsIssueItemLocationSN.Weight_Id = goodsIssueItemLocation.Weight_Id;  
                    goodsIssueItemLocationSN.Weight_Name = goodsIssueItemLocation.Weight_Name;  
                    goodsIssueItemLocationSN.WeightRatio = goodsIssueItemLocation.WeightRatio;  
                    goodsIssueItemLocationSN.UnitNetWeight = goodsIssueItemLocation.UnitNetWeight;  
                    goodsIssueItemLocationSN.UnitNetWeight_Index = goodsIssueItemLocation.UnitNetWeight_Index;  
                    goodsIssueItemLocationSN.UnitNetWeight_Id = goodsIssueItemLocation.UnitNetWeight_Id;  
                    goodsIssueItemLocationSN.UnitNetWeight_Name = goodsIssueItemLocation.UnitNetWeight_Name;  
                    goodsIssueItemLocationSN.UnitNetWeightRatio = goodsIssueItemLocation.UnitNetWeightRatio;  
                    goodsIssueItemLocationSN.NetWeight = goodsIssueItemLocation.NetWeight;  
                    goodsIssueItemLocationSN.NetWeight_Index = goodsIssueItemLocation.NetWeight_Index;  
                    goodsIssueItemLocationSN.NetWeight_Id = goodsIssueItemLocation.NetWeight_Id;  
                    goodsIssueItemLocationSN.NetWeight_Name = goodsIssueItemLocation.NetWeight_Name;  
                    goodsIssueItemLocationSN.NetWeightRatio = goodsIssueItemLocation.NetWeightRatio;  
                    goodsIssueItemLocationSN.UnitGrsWeight = goodsIssueItemLocation.UnitGrsWeight;  
                    goodsIssueItemLocationSN.UnitGrsWeight_Index = goodsIssueItemLocation.UnitGrsWeight_Index;  
                    goodsIssueItemLocationSN.UnitGrsWeight_Id = goodsIssueItemLocation.UnitGrsWeight_Id;  
                    goodsIssueItemLocationSN.UnitGrsWeight_Name = goodsIssueItemLocation.UnitGrsWeight_Name;  
                    goodsIssueItemLocationSN.UnitGrsWeightRatio = goodsIssueItemLocation.UnitGrsWeightRatio;  
                    goodsIssueItemLocationSN.GrsWeight = goodsIssueItemLocation.GrsWeight;  
                    goodsIssueItemLocationSN.GrsWeight_Index = goodsIssueItemLocation.GrsWeight_Index;  
                    goodsIssueItemLocationSN.GrsWeight_Id = goodsIssueItemLocation.GrsWeight_Id;  
                    goodsIssueItemLocationSN.GrsWeight_Name = goodsIssueItemLocation.GrsWeight_Name;  
                    goodsIssueItemLocationSN.GrsWeightRatio = goodsIssueItemLocation.GrsWeightRatio;  
                    goodsIssueItemLocationSN.UnitWidth = goodsIssueItemLocation.UnitWidth;  
                    goodsIssueItemLocationSN.UnitWidth_Index = goodsIssueItemLocation.UnitWidth_Index;  
                    goodsIssueItemLocationSN.UnitWidth_Id = goodsIssueItemLocation.UnitWidth_Id;  
                    goodsIssueItemLocationSN.UnitWidth_Name = goodsIssueItemLocation.UnitWidth_Name;  
                    goodsIssueItemLocationSN.UnitWidthRatio = goodsIssueItemLocation.UnitWidthRatio;  
                    goodsIssueItemLocationSN.Width = goodsIssueItemLocation.Width;  
                    goodsIssueItemLocationSN.Width_Index = goodsIssueItemLocation.Width_Index;  
                    goodsIssueItemLocationSN.Width_Id = goodsIssueItemLocation.Width_Id;  
                    goodsIssueItemLocationSN.Width_Name = goodsIssueItemLocation.Width_Name;  
                    goodsIssueItemLocationSN.WidthRatio = goodsIssueItemLocation.WidthRatio;  
                    goodsIssueItemLocationSN.UnitLength = goodsIssueItemLocation.UnitLength;  
                    goodsIssueItemLocationSN.UnitLength_Index = goodsIssueItemLocation.UnitLength_Index;  
                    goodsIssueItemLocationSN.UnitLength_Id = goodsIssueItemLocation.UnitLength_Id;  
                    goodsIssueItemLocationSN.UnitLength_Name = goodsIssueItemLocation.UnitLength_Name;  
                    goodsIssueItemLocationSN.UnitLengthRatio = goodsIssueItemLocation.UnitLengthRatio;  
                    goodsIssueItemLocationSN.Length = goodsIssueItemLocation.Length;  
                    goodsIssueItemLocationSN.Length_Index = goodsIssueItemLocation.Length_Index;  
                    goodsIssueItemLocationSN.Length_Id = goodsIssueItemLocation.Length_Id;  
                    goodsIssueItemLocationSN.Length_Name = goodsIssueItemLocation.Length_Name;  
                    goodsIssueItemLocationSN.LengthRatio = goodsIssueItemLocation.LengthRatio;  
                    goodsIssueItemLocationSN.UnitHeight = goodsIssueItemLocation.UnitHeight;  
                    goodsIssueItemLocationSN.UnitHeight_Index = goodsIssueItemLocation.UnitHeight_Index;  
                    goodsIssueItemLocationSN.UnitHeight_Id = goodsIssueItemLocation.UnitHeight_Id;  
                    goodsIssueItemLocationSN.UnitHeight_Name = goodsIssueItemLocation.UnitHeight_Name;  
                    goodsIssueItemLocationSN.UnitHeightRatio = goodsIssueItemLocation.UnitHeightRatio;  
                    goodsIssueItemLocationSN.Height = goodsIssueItemLocation.Height;  
                    goodsIssueItemLocationSN.Height_Index = goodsIssueItemLocation.Height_Index;  
                    goodsIssueItemLocationSN.Height_Id = goodsIssueItemLocation.Height_Id;  
                    goodsIssueItemLocationSN.Height_Name = goodsIssueItemLocation.Height_Name;  
                    goodsIssueItemLocationSN.HeightRatio = goodsIssueItemLocation.HeightRatio;  
                    goodsIssueItemLocationSN.UnitVolume = goodsIssueItemLocation.UnitVolume;  
                    goodsIssueItemLocationSN.Volume = goodsIssueItemLocation.Volume;  
                    goodsIssueItemLocationSN.UnitPrice = goodsIssueItemLocation.UnitPrice;  
                    goodsIssueItemLocationSN.UnitPrice_Index = goodsIssueItemLocation.UnitPrice_Index;  
                    goodsIssueItemLocationSN.UnitPrice_Id = goodsIssueItemLocation.UnitPrice_Id;  
                    goodsIssueItemLocationSN.UnitPrice_Name = goodsIssueItemLocation.UnitPrice_Name;  
                    goodsIssueItemLocationSN.Price = goodsIssueItemLocation.Price;  
                    goodsIssueItemLocationSN.Price_Index = goodsIssueItemLocation.Price_Index;  
                    goodsIssueItemLocationSN.Price_Id = goodsIssueItemLocation.Price_Id;  
                    goodsIssueItemLocationSN.Price_Name = goodsIssueItemLocation.Price_Name;  
                    goodsIssueItemLocationSN.DocumentRef_No1 = goodsIssueItemLocation.DocumentRef_No1;  
                    goodsIssueItemLocationSN.DocumentRef_No2 = goodsIssueItemLocation.DocumentRef_No2;  
                    goodsIssueItemLocationSN.DocumentRef_No3 = goodsIssueItemLocation.DocumentRef_No3;  
                    goodsIssueItemLocationSN.DocumentRef_No4 = goodsIssueItemLocation.DocumentRef_No4;  
                    goodsIssueItemLocationSN.DocumentRef_No5 = goodsIssueItemLocation.DocumentRef_No5;  
                    goodsIssueItemLocationSN.Document_Status = goodsIssueItemLocation.Document_Status;  
                    goodsIssueItemLocationSN.UDF_1 = goodsIssueItemLocation.UDF_1;  
                    goodsIssueItemLocationSN.UDF_2 = goodsIssueItemLocation.UDF_2;  
                    goodsIssueItemLocationSN.UDF_3 = goodsIssueItemLocation.UDF_3;  
                    goodsIssueItemLocationSN.UDF_4 = goodsIssueItemLocation.UDF_4;  
                    goodsIssueItemLocationSN.UDF_5 = goodsIssueItemLocation.UDF_5;  
                    goodsIssueItemLocationSN.Ref_Process_Index = goodsIssueItemLocation.Ref_Process_Index;  
                    goodsIssueItemLocationSN.Ref_Document_No = goodsIssueItemLocation.Ref_Document_No;  
                    goodsIssueItemLocationSN.Ref_Document_LineNum = goodsIssueItemLocation.Ref_Document_LineNum;  
                    goodsIssueItemLocationSN.Ref_Document_Index = goodsIssueItemLocation.Ref_Document_Index;  
                    goodsIssueItemLocationSN.Ref_DocumentItem_Index = goodsIssueItemLocation.Ref_DocumentItem_Index;  
                    goodsIssueItemLocationSN.GoodsReceiveItem_Index = goodsIssueItemLocation.GoodsReceiveItem_Index;  
                    goodsIssueItemLocationSN.Create_By = model.userName;  
                    goodsIssueItemLocationSN.Create_Date = DateTime.Now;
                    goodsIssueItemLocationSN.Update_By = null ;  
                    goodsIssueItemLocationSN.Update_Date = null;  
                    goodsIssueItemLocationSN.Cancel_By = null;  
                    goodsIssueItemLocationSN.Cancel_Date = null;  
                    goodsIssueItemLocationSN.Picking_Status = goodsIssueItemLocation.Picking_Status;  
                    goodsIssueItemLocationSN.Picking_By = goodsIssueItemLocation.Picking_By;  
                    goodsIssueItemLocationSN.Picking_Date = goodsIssueItemLocation.Picking_Date;  
                    goodsIssueItemLocationSN.Picking_Ref1 = goodsIssueItemLocation.Picking_Ref1;  
                    goodsIssueItemLocationSN.Picking_Ref2 = goodsIssueItemLocation.Picking_Ref2;  
                    goodsIssueItemLocationSN.Picking_Qty = goodsIssueItemLocation.Picking_Qty;  
                    goodsIssueItemLocationSN.Picking_Ratio = goodsIssueItemLocation.Picking_Ratio;  
                    goodsIssueItemLocationSN.Picking_TotalQty = goodsIssueItemLocation.Picking_TotalQty;  
                    goodsIssueItemLocationSN.Picking_ProductConversion_Index = goodsIssueItemLocation.Picking_ProductConversion_Index;  
                    goodsIssueItemLocationSN.Mashall_Status = goodsIssueItemLocation.Mashall_Status;  
                    goodsIssueItemLocationSN.Mashall_Qty = goodsIssueItemLocation.Mashall_Qty;  
                    goodsIssueItemLocationSN.Cancel_Status = goodsIssueItemLocation.Cancel_Status;  
                    goodsIssueItemLocationSN.GoodsIssue_No = goodsIssueItemLocation.GoodsIssue_No;  
                    goodsIssueItemLocationSN.BinBalance_Index = goodsIssueItemLocation.BinBalance_Index;  
                    goodsIssueItemLocationSN.Invoice_No = goodsIssueItemLocation.Invoice_No;  
                    goodsIssueItemLocationSN.Invoice_No_Out = goodsIssueItemLocation.Invoice_No_Out;  
                    goodsIssueItemLocationSN.Declaration_No = goodsIssueItemLocation.Declaration_No;  
                    goodsIssueItemLocationSN.Declaration_No_Out = goodsIssueItemLocation.Declaration_No_Out;  
                    goodsIssueItemLocationSN.HS_Code = goodsIssueItemLocation.HS_Code;  
                    goodsIssueItemLocationSN.Conutry_of_Origin = goodsIssueItemLocation.Conutry_of_Origin;  
                    goodsIssueItemLocationSN.Tax1 = goodsIssueItemLocation.Tax1;  
                    goodsIssueItemLocationSN.Tax1_Currency_Index = goodsIssueItemLocation.Tax1_Currency_Index;  
                    goodsIssueItemLocationSN.Tax1_Currency_Id = goodsIssueItemLocation.Tax1_Currency_Id;  
                    goodsIssueItemLocationSN.Tax1_Currency_Name = goodsIssueItemLocation.Tax1_Currency_Name;  
                    goodsIssueItemLocationSN.Tax2 = goodsIssueItemLocation.Tax2;  
                    goodsIssueItemLocationSN.Tax2_Currency_Index = goodsIssueItemLocation.Tax2_Currency_Index;  
                    goodsIssueItemLocationSN.Tax2_Currency_Id = goodsIssueItemLocation.Tax2_Currency_Id;  
                    goodsIssueItemLocationSN.Tax2_Currency_Name = goodsIssueItemLocation.Tax2_Currency_Name;  
                    goodsIssueItemLocationSN.Tax3 = goodsIssueItemLocation.Tax3;  
                    goodsIssueItemLocationSN.Tax3_Currency_Index = goodsIssueItemLocation.Tax3_Currency_Index;  
                    goodsIssueItemLocationSN.Tax3_Currency_Id = goodsIssueItemLocation.Tax3_Currency_Id;  
                    goodsIssueItemLocationSN.Tax3_Currency_Name = goodsIssueItemLocation.Tax3_Currency_Name;  
                    goodsIssueItemLocationSN.Tax4 = goodsIssueItemLocation.Tax4;  
                    goodsIssueItemLocationSN.Tax4_Currency_Index = goodsIssueItemLocation.Tax4_Currency_Index;  
                    goodsIssueItemLocationSN.Tax4_Currency_Id = goodsIssueItemLocation.Tax4_Currency_Id;  
                    goodsIssueItemLocationSN.Tax4_Currency_Name = goodsIssueItemLocation.Tax4_Currency_Name;  
                    goodsIssueItemLocationSN.Tax5 = goodsIssueItemLocation.Tax5;  
                    goodsIssueItemLocationSN.Tax5_Currency_Index = goodsIssueItemLocation.Tax5_Currency_Index;  
                    goodsIssueItemLocationSN.Tax5_Currency_Id = goodsIssueItemLocation.Tax5_Currency_Id;  
                    goodsIssueItemLocationSN.Tax5_Currency_Name = goodsIssueItemLocation.Tax5_Currency_Name;
                    goodsIssueItemLocationSN.TaskItem_Index = taskItem.TaskItem_Index;
                    goodsIssueItemLocationSN.Serial = item.insertSerial;
                    db.im_GoodsIssueItemLocationSN.Add(goodsIssueItemLocationSN);

                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    Result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("ScanConfirm", msglog);
                    transaction.Rollback();
                    throw exy;
                }
                if (Result.resultIsUse)
                {
                    ScanPicksearchViewModel new_model = new ScanPicksearchViewModel();
                    new_model.confirm_location_Name = model.confirm_location_Name;
                    new_model.task_Index = model.task_Index;
                    new_model.confirm_location_Index = model.confirm_location_Index;
                    new_model.confirm_location_Id = model.confirm_location_Id;
                    new_model.userName = model.userName;
                    var re = ScanConfirmLocatonPickingQty(new_model);
                    if (!re)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task Error";
                        return Result;
                    }
                }
                //}

                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirm", msglog);
                var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region Scan Confirm Location ScanPick Qty
        public bool ScanConfirmLocatonPickingQty(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            logtxt log = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
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
                var data = db.View_TaskInsertBinCard_V2.Where(c => c.Task_Index == task_Index && c.PickingPickQty_Status == 1).ToList();
                foreach (var d in data)
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
                        isScanSplit = true
                    };
                    //----------------------------------------------------------------------------------------------------------- step 6 -----------------------------------------------------------------------------------------------------------
                    //var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("insertBincard"), View_TaskInsertBinCard.sJson());
                    var BinbalanceCutService = new BinbalanceCutService();
                    var Bincard = BinbalanceCutService.InsertBinCard(View_TaskInsertBinCard);
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var bin_index = new { binbalance_Index = Bincard };
                        var findBin = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), bin_index.sJson());
                        
                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);

                        if (Guid.Parse(Bincard) == taskitem.BinBalance_Index)
                        {
                            var updatetask = db.IM_Task.FirstOrDefault(c => c.Task_Index == taskitem.Task_Index);
                            updatetask.Document_StatusMovetoStgOG = 2;
                            taskitem.PickingToStg_Status = 2;
                            taskitem.PickingToStg_By = model.userName;
                            taskitem.PickingToStg_Date = DateTime.Now;
                        }
                        taskitem.PickingPickQty_Status = 2;
                        taskitem.UDF_5 = model.confirm_location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        taskitem.DocumentRef_No3 = taskitem.Tag_Index.ToString();
                        taskitem.DocumentRef_No4 = taskitem.Tag_No;
                        taskitem.Tag_Index = findBin.tag_Index;
                        taskitem.Tag_No = model.confirm_location_Name;
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocatonPickingQty", "ScanConfirmLocatonPickingQty", "step 7 : Save Error exy : " + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                var chkTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == task_Index && (c.PickingPickQty_Status == 1 || (c.PickingPickQty_Status != null ? c.PickingPickQty_Status : 0) == 0)).Count();
                if (chkTaskItem == 0)
                {
                    var task = db.IM_Task.Find(task_Index);

                    var taskitem = db.IM_TaskItem.Where(c => c.Task_Index == task.Task_Index && c.PickingPickQty_Status == 2).GroupBy(g => g.Ref_Document_Index);
                    var GI = db.IM_GoodsIssue.Where(c => taskitem.Select(s => s.Key).Contains(c.GoodsIssue_Index)).ToList();
                    foreach (var gi in GI)
                    {
                        gi.Document_Status = 3;
                    }

                    task.DocumentRef_No1 = model.confirm_location_Name;
                    task.Document_StatusPickQty = 2;
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
                        log.DataLogLines("ScanConfirmLocatonPickingQty", "ScanConfirmLocatonPickingQty" , "step 10 : Save Task End Error exy" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirmLocatonPickingQty", "ScanConfirmLocatonPickingQty" , "EX" + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Scan Confirm ScanPick Qty cool
        public actionResultScanPickViewModel ScanConfirmPickQtycool(ScanPickViewModel_Qty model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
                var Result = new actionResultScanPickViewModel();
                if (string.IsNullOrEmpty(model.confirm_location_Index))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Name))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                var GetTruckload_Task = db.View_Taskitem_with_Truckload.Where(c => c.Task_Index == Guid.Parse(model.task_Index) && c.TruckLoad_Index == model.TruckLoad_Index).Select(c => c.TaskItem_Index).ToList();
                var ListTaskItem = db.IM_TaskItem.Where(c => GetTruckload_Task.Contains(c.TaskItem_Index)
                && c.Tag_Index == model.tag_Index
                && c.Product_Id == model.product_Id
                && c.Product_Index == model.product_Index
                ).ToList();

                //var ListTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == new Guid(model.task_Index)
                //&& c.Tag_Index == model.tag_Index
                //&& c.Product_Id == model.product_Id
                //&& c.Product_Index == model.product_Index
                //).ToList();

                if (ListTaskItem.Count() == 0)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }

                decimal? RemainScanQty = ListTaskItem.Sum(s => s.TotalQty) - ListTaskItem.Sum(s => s.PickingLabel_TotalQty);
                decimal? ScanPick = (model.pick_Qty * model.pick_ProductConversion_Ratio);
                if (ScanPick == RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {

                        //TaskItem.PlanGoodsIssue_Index = model.planGoodsIssue_Index;
                        //TaskItem.PlanGoodsIssueItem_Index = model.planGoodsIssueItem_Index;
                        //TaskItem.PlanGoodsIssue_No = model.planGoodsIssue_No;
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        TaskItem.PickingPickQty_Qty = TaskItem.Qty;
                        TaskItem.PickingPickQty_Ratio = TaskItem.Ratio;
                        TaskItem.PickingPickQty_TotalQty = TaskItem.TotalQty;
                        TaskItem.PickingPickQty_By = model.userName;
                        TaskItem.PickingPickQty_Date = DateTime.Now;
                        TaskItem.PickingPickQty_Status = 1;
                        TaskItem.PickingPickQty_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingPickQty_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingPickQty_Location_Name = model.confirm_location_Name;


                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                    }
                }
                else if (ScanPick < RemainScanQty && RemainScanQty > 0)
                {
                    foreach (var TaskItem in ListTaskItem)
                    {

                        //TaskItem.PlanGoodsIssue_Index = model.planGoodsIssue_Index;
                        //TaskItem.PlanGoodsIssueItem_Index = model.planGoodsIssueItem_Index;
                        //TaskItem.PlanGoodsIssue_No = model.planGoodsIssue_No;
                        TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                        TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                        TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                        TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                        TaskItem.TagOut_Index = model.tagOut_Index;
                        TaskItem.TagOut_No = model.tagOut_No;

                        TaskItem.PickingPickQty_Qty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.PickingPickQty_Ratio = model.pick_ProductConversion_Ratio;
                        TaskItem.PickingPickQty_TotalQty = ScanPick > TaskItem.TotalQty ? TaskItem.TotalQty : ScanPick;
                        TaskItem.PickingPickQty_By = model.userName;
                        TaskItem.PickingPickQty_Date = DateTime.Now;
                        TaskItem.PickingPickQty_Status = 1;
                        TaskItem.PickingPickQty_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingPickQty_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingPickQty_Location_Name = model.confirm_location_Name;

                        if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                        {
                            TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                            TaskItem.ReasonCode_Id = model.reasonCode_Id;
                            TaskItem.ReasonCode_Name = model.reasonCode_Name;
                        }
                        ScanPick = ScanPick == 0 ? 0 : ScanPick - TaskItem.TotalQty;

                    }

                }
                else
                {
                    foreach (var TaskItem in ListTaskItem)
                    {
                        TaskItem.PickingPickQty_By = model.userName;
                        TaskItem.PickingPickQty_Date = DateTime.Now;
                        TaskItem.PickingPickQty_Status = 1;
                        TaskItem.PickingPickQty_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingPickQty_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingPickQty_Location_Name = model.confirm_location_Name;
                    }
                }

                foreach (var item in model.serailnumber)
                {
                    wm_TagOut tagOut = db.WM_TagOut.FirstOrDefault(c => c.TagOut_No == item.tagOut_No);
                    if (tagOut == null)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Tagout not found";
                        return Result;
                    }
                    im_GoodsIssueItemLocation goodsIssueItemLocation = db.IM_GoodsIssueItemLocation.FirstOrDefault(c => c.GoodsIssueItemLocation_Index == tagOut.Ref_DocumentItem_Index);
                    if (tagOut == null)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "GI not found";
                        return Result;
                    }
                    im_TaskItem taskItem = db.IM_TaskItem.FirstOrDefault(c => c.Ref_DocumentItem_Index == tagOut.Ref_DocumentItem_Index);
                    if (tagOut == null)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task not found";
                        return Result;
                    }
                    im_GoodsIssueItemLocationSN goodsIssueItemLocationSN = new im_GoodsIssueItemLocationSN();
                    goodsIssueItemLocationSN.GoodsIssueItemLocationSN_Index = Guid.NewGuid();
                    goodsIssueItemLocationSN.GoodsIssueItemLocation_Index = goodsIssueItemLocation.GoodsIssueItemLocation_Index;
                    goodsIssueItemLocationSN.GoodsIssue_Index = goodsIssueItemLocation.GoodsIssue_Index;
                    goodsIssueItemLocationSN.LineNum = goodsIssueItemLocation.LineNum;
                    goodsIssueItemLocationSN.TagItem_Index = goodsIssueItemLocation.TagItem_Index;
                    goodsIssueItemLocationSN.Tag_Index = goodsIssueItemLocation.Tag_Index;
                    goodsIssueItemLocationSN.Tag_No = goodsIssueItemLocation.Tag_No;
                    goodsIssueItemLocationSN.Product_Index = goodsIssueItemLocation.Product_Index;
                    goodsIssueItemLocationSN.Product_Id = goodsIssueItemLocation.Product_Id;
                    goodsIssueItemLocationSN.Product_Name = goodsIssueItemLocation.Product_Name;
                    goodsIssueItemLocationSN.Product_SecondName = goodsIssueItemLocation.Product_SecondName;
                    goodsIssueItemLocationSN.Product_ThirdName = goodsIssueItemLocation.Product_ThirdName;
                    goodsIssueItemLocationSN.Product_Lot = goodsIssueItemLocation.Product_Lot;
                    goodsIssueItemLocationSN.ItemStatus_Index = goodsIssueItemLocation.ItemStatus_Index;
                    goodsIssueItemLocationSN.ItemStatus_Id = goodsIssueItemLocation.ItemStatus_Id;
                    goodsIssueItemLocationSN.ItemStatus_Name = goodsIssueItemLocation.ItemStatus_Name;
                    goodsIssueItemLocationSN.Location_Index = goodsIssueItemLocation.Location_Index;
                    goodsIssueItemLocationSN.Location_Id = goodsIssueItemLocation.Location_Id;
                    goodsIssueItemLocationSN.Location_Name = goodsIssueItemLocation.Location_Name;
                    goodsIssueItemLocationSN.QtyPlan = goodsIssueItemLocation.QtyPlan;
                    goodsIssueItemLocationSN.Qty = goodsIssueItemLocation.Qty;
                    goodsIssueItemLocationSN.Ratio = goodsIssueItemLocation.Ratio;
                    goodsIssueItemLocationSN.TotalQty = goodsIssueItemLocation.TotalQty;
                    goodsIssueItemLocationSN.ProductConversion_Index = goodsIssueItemLocation.ProductConversion_Index;
                    goodsIssueItemLocationSN.ProductConversion_Id = goodsIssueItemLocation.ProductConversion_Id;
                    goodsIssueItemLocationSN.ProductConversion_Name = goodsIssueItemLocation.ProductConversion_Name;
                    goodsIssueItemLocationSN.MFG_Date = goodsIssueItemLocation.MFG_Date;
                    goodsIssueItemLocationSN.EXP_Date = goodsIssueItemLocation.EXP_Date;
                    goodsIssueItemLocationSN.UnitWeight = goodsIssueItemLocation.UnitWeight;
                    goodsIssueItemLocationSN.UnitWeight_Index = goodsIssueItemLocation.UnitWeight_Index;
                    goodsIssueItemLocationSN.UnitWeight_Id = goodsIssueItemLocation.UnitWeight_Id;
                    goodsIssueItemLocationSN.UnitWeight_Name = goodsIssueItemLocation.UnitWeight_Name;
                    goodsIssueItemLocationSN.UnitWeightRatio = goodsIssueItemLocation.UnitWeightRatio;
                    goodsIssueItemLocationSN.Weight = goodsIssueItemLocation.Weight;
                    goodsIssueItemLocationSN.Weight_Index = goodsIssueItemLocation.Weight_Index;
                    goodsIssueItemLocationSN.Weight_Id = goodsIssueItemLocation.Weight_Id;
                    goodsIssueItemLocationSN.Weight_Name = goodsIssueItemLocation.Weight_Name;
                    goodsIssueItemLocationSN.WeightRatio = goodsIssueItemLocation.WeightRatio;
                    goodsIssueItemLocationSN.UnitNetWeight = goodsIssueItemLocation.UnitNetWeight;
                    goodsIssueItemLocationSN.UnitNetWeight_Index = goodsIssueItemLocation.UnitNetWeight_Index;
                    goodsIssueItemLocationSN.UnitNetWeight_Id = goodsIssueItemLocation.UnitNetWeight_Id;
                    goodsIssueItemLocationSN.UnitNetWeight_Name = goodsIssueItemLocation.UnitNetWeight_Name;
                    goodsIssueItemLocationSN.UnitNetWeightRatio = goodsIssueItemLocation.UnitNetWeightRatio;
                    goodsIssueItemLocationSN.NetWeight = goodsIssueItemLocation.NetWeight;
                    goodsIssueItemLocationSN.NetWeight_Index = goodsIssueItemLocation.NetWeight_Index;
                    goodsIssueItemLocationSN.NetWeight_Id = goodsIssueItemLocation.NetWeight_Id;
                    goodsIssueItemLocationSN.NetWeight_Name = goodsIssueItemLocation.NetWeight_Name;
                    goodsIssueItemLocationSN.NetWeightRatio = goodsIssueItemLocation.NetWeightRatio;
                    goodsIssueItemLocationSN.UnitGrsWeight = goodsIssueItemLocation.UnitGrsWeight;
                    goodsIssueItemLocationSN.UnitGrsWeight_Index = goodsIssueItemLocation.UnitGrsWeight_Index;
                    goodsIssueItemLocationSN.UnitGrsWeight_Id = goodsIssueItemLocation.UnitGrsWeight_Id;
                    goodsIssueItemLocationSN.UnitGrsWeight_Name = goodsIssueItemLocation.UnitGrsWeight_Name;
                    goodsIssueItemLocationSN.UnitGrsWeightRatio = goodsIssueItemLocation.UnitGrsWeightRatio;
                    goodsIssueItemLocationSN.GrsWeight = goodsIssueItemLocation.GrsWeight;
                    goodsIssueItemLocationSN.GrsWeight_Index = goodsIssueItemLocation.GrsWeight_Index;
                    goodsIssueItemLocationSN.GrsWeight_Id = goodsIssueItemLocation.GrsWeight_Id;
                    goodsIssueItemLocationSN.GrsWeight_Name = goodsIssueItemLocation.GrsWeight_Name;
                    goodsIssueItemLocationSN.GrsWeightRatio = goodsIssueItemLocation.GrsWeightRatio;
                    goodsIssueItemLocationSN.UnitWidth = goodsIssueItemLocation.UnitWidth;
                    goodsIssueItemLocationSN.UnitWidth_Index = goodsIssueItemLocation.UnitWidth_Index;
                    goodsIssueItemLocationSN.UnitWidth_Id = goodsIssueItemLocation.UnitWidth_Id;
                    goodsIssueItemLocationSN.UnitWidth_Name = goodsIssueItemLocation.UnitWidth_Name;
                    goodsIssueItemLocationSN.UnitWidthRatio = goodsIssueItemLocation.UnitWidthRatio;
                    goodsIssueItemLocationSN.Width = goodsIssueItemLocation.Width;
                    goodsIssueItemLocationSN.Width_Index = goodsIssueItemLocation.Width_Index;
                    goodsIssueItemLocationSN.Width_Id = goodsIssueItemLocation.Width_Id;
                    goodsIssueItemLocationSN.Width_Name = goodsIssueItemLocation.Width_Name;
                    goodsIssueItemLocationSN.WidthRatio = goodsIssueItemLocation.WidthRatio;
                    goodsIssueItemLocationSN.UnitLength = goodsIssueItemLocation.UnitLength;
                    goodsIssueItemLocationSN.UnitLength_Index = goodsIssueItemLocation.UnitLength_Index;
                    goodsIssueItemLocationSN.UnitLength_Id = goodsIssueItemLocation.UnitLength_Id;
                    goodsIssueItemLocationSN.UnitLength_Name = goodsIssueItemLocation.UnitLength_Name;
                    goodsIssueItemLocationSN.UnitLengthRatio = goodsIssueItemLocation.UnitLengthRatio;
                    goodsIssueItemLocationSN.Length = goodsIssueItemLocation.Length;
                    goodsIssueItemLocationSN.Length_Index = goodsIssueItemLocation.Length_Index;
                    goodsIssueItemLocationSN.Length_Id = goodsIssueItemLocation.Length_Id;
                    goodsIssueItemLocationSN.Length_Name = goodsIssueItemLocation.Length_Name;
                    goodsIssueItemLocationSN.LengthRatio = goodsIssueItemLocation.LengthRatio;
                    goodsIssueItemLocationSN.UnitHeight = goodsIssueItemLocation.UnitHeight;
                    goodsIssueItemLocationSN.UnitHeight_Index = goodsIssueItemLocation.UnitHeight_Index;
                    goodsIssueItemLocationSN.UnitHeight_Id = goodsIssueItemLocation.UnitHeight_Id;
                    goodsIssueItemLocationSN.UnitHeight_Name = goodsIssueItemLocation.UnitHeight_Name;
                    goodsIssueItemLocationSN.UnitHeightRatio = goodsIssueItemLocation.UnitHeightRatio;
                    goodsIssueItemLocationSN.Height = goodsIssueItemLocation.Height;
                    goodsIssueItemLocationSN.Height_Index = goodsIssueItemLocation.Height_Index;
                    goodsIssueItemLocationSN.Height_Id = goodsIssueItemLocation.Height_Id;
                    goodsIssueItemLocationSN.Height_Name = goodsIssueItemLocation.Height_Name;
                    goodsIssueItemLocationSN.HeightRatio = goodsIssueItemLocation.HeightRatio;
                    goodsIssueItemLocationSN.UnitVolume = goodsIssueItemLocation.UnitVolume;
                    goodsIssueItemLocationSN.Volume = goodsIssueItemLocation.Volume;
                    goodsIssueItemLocationSN.UnitPrice = goodsIssueItemLocation.UnitPrice;
                    goodsIssueItemLocationSN.UnitPrice_Index = goodsIssueItemLocation.UnitPrice_Index;
                    goodsIssueItemLocationSN.UnitPrice_Id = goodsIssueItemLocation.UnitPrice_Id;
                    goodsIssueItemLocationSN.UnitPrice_Name = goodsIssueItemLocation.UnitPrice_Name;
                    goodsIssueItemLocationSN.Price = goodsIssueItemLocation.Price;
                    goodsIssueItemLocationSN.Price_Index = goodsIssueItemLocation.Price_Index;
                    goodsIssueItemLocationSN.Price_Id = goodsIssueItemLocation.Price_Id;
                    goodsIssueItemLocationSN.Price_Name = goodsIssueItemLocation.Price_Name;
                    goodsIssueItemLocationSN.DocumentRef_No1 = goodsIssueItemLocation.DocumentRef_No1;
                    goodsIssueItemLocationSN.DocumentRef_No2 = goodsIssueItemLocation.DocumentRef_No2;
                    goodsIssueItemLocationSN.DocumentRef_No3 = goodsIssueItemLocation.DocumentRef_No3;
                    goodsIssueItemLocationSN.DocumentRef_No4 = goodsIssueItemLocation.DocumentRef_No4;
                    goodsIssueItemLocationSN.DocumentRef_No5 = goodsIssueItemLocation.DocumentRef_No5;
                    goodsIssueItemLocationSN.Document_Status = goodsIssueItemLocation.Document_Status;
                    goodsIssueItemLocationSN.UDF_1 = goodsIssueItemLocation.UDF_1;
                    goodsIssueItemLocationSN.UDF_2 = goodsIssueItemLocation.UDF_2;
                    goodsIssueItemLocationSN.UDF_3 = goodsIssueItemLocation.UDF_3;
                    goodsIssueItemLocationSN.UDF_4 = goodsIssueItemLocation.UDF_4;
                    goodsIssueItemLocationSN.UDF_5 = goodsIssueItemLocation.UDF_5;
                    goodsIssueItemLocationSN.Ref_Process_Index = goodsIssueItemLocation.Ref_Process_Index;
                    goodsIssueItemLocationSN.Ref_Document_No = goodsIssueItemLocation.Ref_Document_No;
                    goodsIssueItemLocationSN.Ref_Document_LineNum = goodsIssueItemLocation.Ref_Document_LineNum;
                    goodsIssueItemLocationSN.Ref_Document_Index = goodsIssueItemLocation.Ref_Document_Index;
                    goodsIssueItemLocationSN.Ref_DocumentItem_Index = goodsIssueItemLocation.Ref_DocumentItem_Index;
                    goodsIssueItemLocationSN.GoodsReceiveItem_Index = goodsIssueItemLocation.GoodsReceiveItem_Index;
                    goodsIssueItemLocationSN.Create_By = model.userName;
                    goodsIssueItemLocationSN.Create_Date = DateTime.Now;
                    goodsIssueItemLocationSN.Update_By = null;
                    goodsIssueItemLocationSN.Update_Date = null;
                    goodsIssueItemLocationSN.Cancel_By = null;
                    goodsIssueItemLocationSN.Cancel_Date = null;
                    goodsIssueItemLocationSN.Picking_Status = goodsIssueItemLocation.Picking_Status;
                    goodsIssueItemLocationSN.Picking_By = goodsIssueItemLocation.Picking_By;
                    goodsIssueItemLocationSN.Picking_Date = goodsIssueItemLocation.Picking_Date;
                    goodsIssueItemLocationSN.Picking_Ref1 = goodsIssueItemLocation.Picking_Ref1;
                    goodsIssueItemLocationSN.Picking_Ref2 = goodsIssueItemLocation.Picking_Ref2;
                    goodsIssueItemLocationSN.Picking_Qty = goodsIssueItemLocation.Picking_Qty;
                    goodsIssueItemLocationSN.Picking_Ratio = goodsIssueItemLocation.Picking_Ratio;
                    goodsIssueItemLocationSN.Picking_TotalQty = goodsIssueItemLocation.Picking_TotalQty;
                    goodsIssueItemLocationSN.Picking_ProductConversion_Index = goodsIssueItemLocation.Picking_ProductConversion_Index;
                    goodsIssueItemLocationSN.Mashall_Status = goodsIssueItemLocation.Mashall_Status;
                    goodsIssueItemLocationSN.Mashall_Qty = goodsIssueItemLocation.Mashall_Qty;
                    goodsIssueItemLocationSN.Cancel_Status = goodsIssueItemLocation.Cancel_Status;
                    goodsIssueItemLocationSN.GoodsIssue_No = goodsIssueItemLocation.GoodsIssue_No;
                    goodsIssueItemLocationSN.BinBalance_Index = goodsIssueItemLocation.BinBalance_Index;
                    goodsIssueItemLocationSN.Invoice_No = goodsIssueItemLocation.Invoice_No;
                    goodsIssueItemLocationSN.Invoice_No_Out = goodsIssueItemLocation.Invoice_No_Out;
                    goodsIssueItemLocationSN.Declaration_No = goodsIssueItemLocation.Declaration_No;
                    goodsIssueItemLocationSN.Declaration_No_Out = goodsIssueItemLocation.Declaration_No_Out;
                    goodsIssueItemLocationSN.HS_Code = goodsIssueItemLocation.HS_Code;
                    goodsIssueItemLocationSN.Conutry_of_Origin = goodsIssueItemLocation.Conutry_of_Origin;
                    goodsIssueItemLocationSN.Tax1 = goodsIssueItemLocation.Tax1;
                    goodsIssueItemLocationSN.Tax1_Currency_Index = goodsIssueItemLocation.Tax1_Currency_Index;
                    goodsIssueItemLocationSN.Tax1_Currency_Id = goodsIssueItemLocation.Tax1_Currency_Id;
                    goodsIssueItemLocationSN.Tax1_Currency_Name = goodsIssueItemLocation.Tax1_Currency_Name;
                    goodsIssueItemLocationSN.Tax2 = goodsIssueItemLocation.Tax2;
                    goodsIssueItemLocationSN.Tax2_Currency_Index = goodsIssueItemLocation.Tax2_Currency_Index;
                    goodsIssueItemLocationSN.Tax2_Currency_Id = goodsIssueItemLocation.Tax2_Currency_Id;
                    goodsIssueItemLocationSN.Tax2_Currency_Name = goodsIssueItemLocation.Tax2_Currency_Name;
                    goodsIssueItemLocationSN.Tax3 = goodsIssueItemLocation.Tax3;
                    goodsIssueItemLocationSN.Tax3_Currency_Index = goodsIssueItemLocation.Tax3_Currency_Index;
                    goodsIssueItemLocationSN.Tax3_Currency_Id = goodsIssueItemLocation.Tax3_Currency_Id;
                    goodsIssueItemLocationSN.Tax3_Currency_Name = goodsIssueItemLocation.Tax3_Currency_Name;
                    goodsIssueItemLocationSN.Tax4 = goodsIssueItemLocation.Tax4;
                    goodsIssueItemLocationSN.Tax4_Currency_Index = goodsIssueItemLocation.Tax4_Currency_Index;
                    goodsIssueItemLocationSN.Tax4_Currency_Id = goodsIssueItemLocation.Tax4_Currency_Id;
                    goodsIssueItemLocationSN.Tax4_Currency_Name = goodsIssueItemLocation.Tax4_Currency_Name;
                    goodsIssueItemLocationSN.Tax5 = goodsIssueItemLocation.Tax5;
                    goodsIssueItemLocationSN.Tax5_Currency_Index = goodsIssueItemLocation.Tax5_Currency_Index;
                    goodsIssueItemLocationSN.Tax5_Currency_Id = goodsIssueItemLocation.Tax5_Currency_Id;
                    goodsIssueItemLocationSN.Tax5_Currency_Name = goodsIssueItemLocation.Tax5_Currency_Name;
                    goodsIssueItemLocationSN.TaskItem_Index = taskItem.TaskItem_Index;
                    goodsIssueItemLocationSN.Serial = item.insertSerial;
                    db.im_GoodsIssueItemLocationSN.Add(goodsIssueItemLocationSN);

                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    Result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("ScanConfirm", msglog);
                    transaction.Rollback();
                    throw exy;
                }
                if (Result.resultIsUse)
                {
                    ScanPicksearchViewModel new_model = new ScanPicksearchViewModel();
                    new_model.confirm_location_Name = model.confirm_location_Name;
                    new_model.task_Index = model.task_Index;
                    new_model.confirm_location_Index = model.confirm_location_Index;
                    new_model.confirm_location_Id = model.confirm_location_Id;
                    new_model.userName = model.userName;
                    var re = ScanConfirmLocatonPickingQtyCool(new_model);
                    if (!re)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task Error";
                        return Result;
                    }
                }
                //}

                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirm", msglog);
                var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region Scan Confirm Location ScanPick Qty cool
        public bool ScanConfirmLocatonPickingQtyCool(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            logtxt log = new logtxt();
            log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + DateTime.Now.ToString("yyyy-MM-dd"), "model : " + JsonConvert.SerializeObject(model) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            try
            {
                db.Database.SetCommandTimeout(120);
                //----------------------------------------------------------------------------------------------------------- step 1 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 1 : Get Config Key " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());

                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = model.confirm_location_Name } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;
                //----------------------------------------------------------------------------------------------------------- step 2 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 2 : CheckTag " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                if (CheckTag.Count() == 0)
                {
                    var tag2 = new { tag_no = model.confirm_location_Name };
                    //----------------------------------------------------------------------------------------------------------- step 3 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 3 : CreateTagHeader data : " + tag2 + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
                    //----------------------------------------------------------------------------------------------------------- step 4 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 4 : CheckTag data : " + tag + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                }


                var task_Index = new Guid(model.task_Index);
                //----------------------------------------------------------------------------------------------------------- step 5 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard " + model.task_Index + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var data = db.View_TaskInsertBinCard_V2.Where(c => c.Task_Index == task_Index && c.PickingPickQty_Status == 1).ToList();
                log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard : " + JsonConvert.SerializeObject(data) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
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
                        isScanSplit = true
                    };
                    //----------------------------------------------------------------------------------------------------------- step 6 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 6 : InsertBinCardBy_Scanpick : " + JsonConvert.SerializeObject(View_TaskInsertBinCard) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("insertBincard"), View_TaskInsertBinCard.sJson());
                    log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 6 : InsertBinCardBy_Scanpick result : " + JsonConvert.SerializeObject(Bincard) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var bin_index = new { binbalance_Index = Bincard };
                        var findBin = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), bin_index.sJson());

                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);

                        //if (Guid.Parse(Bincard) == taskitem.BinBalance_Index)
                        //{
                        //    var updatetask = db.IM_Task.FirstOrDefault(c => c.Task_Index == taskitem.Task_Index);
                        //    updatetask.Document_StatusMovetoStgOG = 2;
                        //    taskitem.PickingToStg_Status = 2;
                        //    taskitem.PickingToStg_By = model.userName;
                        //    taskitem.PickingToStg_Date = DateTime.Now;
                        //}
                        taskitem.PickingPickQty_Status = 2;
                        taskitem.PickingToDock_Status = 2;
                        taskitem.UDF_5 = model.confirm_location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        taskitem.DocumentRef_No3 = taskitem.Tag_Index.ToString();
                        taskitem.DocumentRef_No4 = taskitem.Tag_No;
                        taskitem.Tag_Index = findBin.tag_Index;
                        taskitem.Tag_No = model.confirm_location_Name;
                        //----------------------------------------------------------------------------------------------------------- step 7 -----------------------------------------------------------------------------------------------------------
                        log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save Time : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save Error : " + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("ScanConfirmLocatonPickingQtyCool", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                var chkTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == task_Index && (c.PickingPickQty_Status == 1 || (c.PickingPickQty_Status != null ? c.PickingPickQty_Status : 0) == 0)).Count();
                if (chkTaskItem == 0)
                {
                    var task = db.IM_Task.Find(task_Index);

                    var taskitem = db.IM_TaskItem.Where(c => c.Task_Index == task.Task_Index && c.PickingPickQty_Status == 2).GroupBy(g => g.Ref_Document_Index);
                    var GI = db.IM_GoodsIssue.Where(c => taskitem.Select(s => s.Key).Contains(c.GoodsIssue_Index)).ToList();
                    foreach (var gi in GI)
                    {
                        gi.Document_Status = 3;
                    }

                    task.DocumentRef_No1 = model.confirm_location_Name;
                    task.Document_StatusPickQty = 2;
                    task.Document_StatusDocktoStg = 2;
                    task.Update_Date = DateTime.Now;
                    task.Update_By = model.userName;
                    //----------------------------------------------------------------------------------------------------------- step 10 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Time" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    }

                    catch (Exception exy)
                    {
                        log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Error" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("ScanConfirmLocatonPickingQtyCool", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirmLocatonPickingQtyCool", "ScanConfirmLocatonPickingQtyCool" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "EX" + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirmLocatonPickingQtyCool", msglog);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion

        #region ScanPick To Dock

        #region Scan Location ScanPick To Dock
        public List<locationViewModel> ScanLocatonDock(ScanPicksearchViewModel model)
        {
            try
            {
                List<locationViewModel> location = new List<locationViewModel>();
                
                if (!string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    if (model.Dock_Index != null)
                    {
                        var Docklocation = db.View_Get_location_DoclSTG.FirstOrDefault(c => c.Dock_Index == model.Dock_Index && c.Location_Id == model.confirm_location_Id);
                        if (Docklocation == null)
                        {
                            return location;
                        }
                    }
                    var objectLocation = new { location_Name = model.confirm_location_Id, locationType_Index = "DDE89154-CCFA-4880-B9F7-61C284D2C91A" };
                    var locationX = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), objectLocation.sJson());

                    var objectLocation2 = new { location_Name = model.confirm_location_Id, locationType_Index = "A1F7BFA0-1429-4010-863D-6A0EB01DB61D" };
                    var locationY = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), objectLocation2.sJson());
                    location = locationX.Union(locationY).ToList();
                }
                return location;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Scan Confirm ScanPick To Dock
        public actionResultScanPickViewModel ScanConfirmforPickingLocation(ScanPickViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
                var Result = new actionResultScanPickViewModel();
                if (string.IsNullOrEmpty(model.confirm_location_Index))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Name))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }

                var GetTruckload_Task = db.View_Taskitem_with_Truckload.Where(c => c.TruckLoad_Index == model.TruckLoad_Index && c.Picking_Status ==2 && c.PickingLabeling_Status == 2 && c.PickingPickQty_Status == 2 && c.PickingToDock_Status == null).Select(c => c.TaskItem_Index).ToList();
                var ListTaskItem = db.IM_TaskItem.Where(c => GetTruckload_Task.Contains(c.TaskItem_Index)).ToList();

                if (ListTaskItem.Count() == 0)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                foreach (var item in ListTaskItem)
                {
                    item.PickingToDock_Location_Index = Guid.Parse(model.confirm_location_Index);
                    item.PickingToDock_Location_Id = model.confirm_location_Id;
                    item.PickingToDock_Location_Name = model.confirm_location_Name;

                    item.Location_Index = Guid.Parse(model.confirm_location_Index);
                    item.Location_Id = model.confirm_location_Id;
                    item.Location_Name = model.confirm_location_Name;
                    item.PickingToDock_Location_Index = Guid.Parse(model.confirm_location_Index);
                    item.PickingToDock_Location_Id = model.confirm_location_Id;
                    item.PickingToDock_Location_Name = model.confirm_location_Name;
                    item.PickingToDock_Status = 1;
                    item.PickingToDock_By = model.create_By;
                    item.PickingToDock_Date = DateTime.Now;

                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    Result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("ScanConfirm", msglog);
                    transaction.Rollback();
                    throw exy;
                }
                if (Result.resultIsUse)
                {
                    ScanPicksearchViewModel new_model = new ScanPicksearchViewModel();
                    new_model.confirm_location_Name = model.confirm_location_Name;
                    new_model.task_Index = model.TruckLoad_Index.ToString();
                    new_model.confirm_location_Index = model.confirm_location_Index;
                    new_model.confirm_location_Id = model.confirm_location_Id;
                    new_model.userName = model.userName;
                    var re = ScanConfirmLocatonPickingtoDock(new_model);
                    if (!re)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task Error";
                        return Result;
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirm", msglog);
                var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region Scan Confirm Location ScanPick To Dock
        public bool ScanConfirmLocatonPickingtoDock(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            logtxt log = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
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

                var task_I = db.View_Taskitem_with_Truckload.Where(c => c.TruckLoad_Index == Guid.Parse(model.task_Index) && c.PickingToDock_Status == 1).GroupBy(c=> c.Task_Index).Select(c => c.Key).ToList();

                //var task_Index = new Guid(model.task_Index);
                var data = db.View_TaskInsertBinCard_V2.Where(c => task_I.Contains(c.Task_Index) && c.PickingLabeling_Status == 2 && c.PickingToDock_Status == 1).ToList();
                foreach (var d in data)
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
                    //----------------------------------------------------------------------------------------------------------- step 6 -----------------------------------------------------------------------------------------------------------
                    //var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardBy_Scanpick"), View_TaskInsertBinCard.sJson());
                    var BinbalanceCutService = new BinbalanceCutService();
                    var Bincard = BinbalanceCutService.InsertBinCardBy_Scanpick(View_TaskInsertBinCard);
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
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocatonPickingtoDock", "ScanConfirmLocatonPickingtoDock" , "step 7 : Save Error exy : " + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                    var chkTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == d.Task_Index && (c.PickingToDock_Status == 1 || (c.PickingToDock_Status != null ? c.PickingToDock_Status : 0) == 0)).Count();
                    if (chkTaskItem == 0)
                    {
                        var task = db.IM_Task.Find(d.Task_Index);

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
                            log.DataLogLines("ScanConfirmLocatonPickingtoDock", "ScanConfirmLocatonPickingtoDock" , "step 10 : Save Task End Error exy" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                

                return true;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirmLocatonPickingtoDock", "ScanConfirmLocatonPickingtoDock" , "EX" + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion

        #region ScanPick Move TO Stg

        #region Scan Location MOve To Stg
        public List<locationViewModel> ScanLocatonMoveToSTG(ScanPicksearchViewModel model)
        {
            try
            {
                var location = new List<locationViewModel>();
                if (!string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    var objectLocation = new { location_Name = model.confirm_location_Id, locationType_Index = "BA0142A8-98B7-4E0B-A1CE-6266716F5F67" };
                    location = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), objectLocation.sJson());

                }
                return location;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Scan Confirm Move To STG
        public actionResultScanPickViewModel ScanConfirmforMoveToSTG(ScanPickViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
                var Result = new actionResultScanPickViewModel();
                if (string.IsNullOrEmpty(model.confirm_location_Index))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Name))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                if (!string.IsNullOrEmpty(model.confirm_location_Index))
                {
                    View_CheckLocation checklocation = dbMaster.View_CheckLocation.FirstOrDefault(c => c.Location_Index == Guid.Parse(model.confirm_location_Index));
                    if (checklocation == null)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Location ที่ Scan ถูกจองหรือมีสินค้าอื่นอยู่ กรุณาเลือก Location อื่น";
                        return Result;
                    }
                }

                var ListTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == new Guid(model.task_Index)
                && c.Product_Id == model.product_Id
                && c.Product_Index == model.product_Index
                ).ToList();

                if (ListTaskItem.Count() == 0)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                foreach (var item in ListTaskItem)
                {
                    //item.PickingToDock_Location_Index = Guid.Parse(model.confirm_location_Index);
                    //item.PickingToDock_Location_Id = model.confirm_location_Id;
                    //item.PickingToDock_Location_Name = model.confirm_location_Name;

                    //item.Location_Index = Guid.Parse(model.confirm_location_Index);
                    //item.Location_Id = model.confirm_location_Id;
                    //item.Location_Name = model.confirm_location_Name;
                    //item.PickingToDock_Location_Index = Guid.Parse(model.confirm_location_Index);
                    //item.PickingToDock_Location_Id = model.confirm_location_Id;
                    //item.PickingToDock_Location_Name = model.confirm_location_Name;
                    item.PickingToStg_Status = 1;
                    item.PickingToStg_By = model.create_By;
                    item.PickingToStg_Date = DateTime.Now;

                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    Result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("ScanConfirm", msglog);
                    transaction.Rollback();
                    throw exy;
                }
                if (Result.resultIsUse)
                {
                    ScanPicksearchViewModel new_model = new ScanPicksearchViewModel();
                    new_model.confirm_location_Name = model.confirm_location_Name;
                    new_model.task_Index = model.task_Index;
                    new_model.confirm_location_Index = model.confirm_location_Index;
                    new_model.confirm_location_Id = model.confirm_location_Id;
                    new_model.userName = model.userName;
                    var re = ScanConfirmLocatonMoveToSTG(new_model);
                    if (!re)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task Error";
                        return Result;
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirm", msglog);
                var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region Scan Confirm Location Move To STG
        public bool ScanConfirmLocatonMoveToSTG(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            logtxt log = new logtxt();
            log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + DateTime.Now.ToString("yyyy-MM-dd"), "model : " + JsonConvert.SerializeObject(model) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            try
            {
                db.Database.SetCommandTimeout(120);
                //----------------------------------------------------------------------------------------------------------- step 1 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 1 : Get Config Key " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());

                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = model.confirm_location_Name } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;
                //----------------------------------------------------------------------------------------------------------- step 2 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 2 : CheckTag " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                if (CheckTag.Count() == 0)
                {

                    var tag2 = new { tag_no = model.confirm_location_Name };
                    //----------------------------------------------------------------------------------------------------------- step 3 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 3 : CreateTagHeader data : " + tag2 + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
                    //----------------------------------------------------------------------------------------------------------- step 4 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 4 : CheckTag data : " + tag + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                }


                var task_Index = new Guid(model.task_Index);
                //----------------------------------------------------------------------------------------------------------- step 5 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard " + model.task_Index + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var data = db.View_TaskInsertBinCard_V2.Where(c => c.Task_Index == task_Index && c.PickingLabeling_Status == 2 && c.PickingPickQty_Status == 2 && c.PickingToStg_Status == 1).ToList();
                log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard : " + JsonConvert.SerializeObject(data) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
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
                        tag_Index = Guid.Parse(d.DocumentRef_No3),
                        tag_No = d.DocumentRef_No4,
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
                        binBalance_Index = d.BinBalance_Index,
                        process_Index = new Guid(configLocation),
                        location_Index_To = new Guid(model.confirm_location_Index),
                        location_Id_To = model.confirm_location_Id,
                        location_Name_To = model.confirm_location_Name,
                        Volume = d.Volume,
                        Weight = d.Weight,
                        userName = model.userName,
                        isScanPick = true
                    };
                    //----------------------------------------------------------------------------------------------------------- step 6 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 6 : InsertBinCardBy_Scanpick : " + JsonConvert.SerializeObject(View_TaskInsertBinCard) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardBy_Scanpick"), View_TaskInsertBinCard.sJson());
                    log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 6 : InsertBinCardBy_Scanpick result : " + JsonConvert.SerializeObject(Bincard) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var bin_index = new { binbalance_Index = Bincard };
                        var findBin = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), bin_index.sJson());

                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);
                        taskitem.PickingToStg_Status = 2;
                        taskitem.UDF_5 = model.confirm_location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        //----------------------------------------------------------------------------------------------------------- step 7 -----------------------------------------------------------------------------------------------------------
                        log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save Time : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Error" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("ScanConfirmLocatonPick", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                var chkTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == task_Index && (c.PickingToStg_Status == 1 || (c.PickingToStg_Status != null ? c.PickingToStg_Status : 0) == 0)).Count();
                if (chkTaskItem == 0)
                {
                    var task = db.IM_Task.Find(task_Index);

                    var taskitem = db.IM_TaskItem.Where(c => c.Task_Index == task.Task_Index && c.PickingToStg_Status == 2).GroupBy(g => g.Ref_Document_Index);
                    var GI = db.IM_GoodsIssue.Where(c => taskitem.Select(s => s.Key).Contains(c.GoodsIssue_Index)).ToList();
                    foreach (var gi in GI)
                    {
                        gi.Document_Status = 3;
                    }


                    task.Document_StatusMovetoStgOG = 2;
                    task.Update_Date = DateTime.Now;
                    task.Update_By = model.userName;
                    //----------------------------------------------------------------------------------------------------------- step 10 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Time" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    }

                    catch (Exception exy)
                    {
                        log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Error" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
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
                log.DataLogLines("ScanConfirmLocatonMoveToSTG", "ScanConfirmLocatonMoveToSTG" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "EX" + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirmLocatonPick", msglog);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion
        
        #region stg To Dock

        #region Scan Location Stg To Dock
        public List<locationViewModel> ScanLocationstgtoDock(ScanPicksearchViewModel model)
        {
            try
            {
                List<locationViewModel> location = new List<locationViewModel>();
                if (!string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    var objectLocation = new { location_Name = model.confirm_location_Id, locationType_Index = "64341969-E596-4B8B-8836-395061777490" };
                    location = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), objectLocation.sJson());

                }
                return location;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Scan Confirm Stg To Dock
        public actionResultScanPickViewModel ScanConfirmforStgToDock(ScanPickViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
                var Result = new actionResultScanPickViewModel();
                if (string.IsNullOrEmpty(model.confirm_location_Index))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Id))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                else if (string.IsNullOrEmpty(model.confirm_location_Name))
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }

                //var GetTruckload_Task = db.View_Taskitem_with_Truckload.Where(c => c.Task_Index == Guid.Parse(model.task_Index) && c.TruckLoad_Index == model.TruckLoad_Index).Select(c => c.TaskItem_Index).ToList();
                //var ListTaskItem = db.IM_TaskItem.Where(c => GetTruckload_Task.Contains(c.TaskItem_Index)
                //&& c.Tag_Index == model.tag_Index
                //&& c.Product_Id == model.product_Id
                //&& c.Product_Index == model.product_Index
                //).ToList();

                var GetTruckload_Task = db.View_Taskitem_with_Truckload.Where(c => c.TruckLoad_Index == model.TruckLoad_Index && c.Picking_Status == 2 && c.PickingLabeling_Status == 2 && c.PickingPickQty_Status == 2 && c.PickingToDock_Status == 2 && c.Document_StatusTracking == 1).Select(c => c.TaskItem_Index).ToList();
                var ListTaskItem = db.IM_TaskItem.Where(c => GetTruckload_Task.Contains(c.TaskItem_Index)).ToList();

                //var ListTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == new Guid(model.task_Index)
                //&& c.Tag_Index == model.tag_Index
                //&& c.Product_Id == model.product_Id
                //&& c.Product_Index == model.product_Index
                //).ToList();

                if (ListTaskItem.Count() == 0)
                {
                    Result.resultIsUse = false;
                    Result.resultMsg = "Task Error";
                    return Result;
                }
                foreach (var item in ListTaskItem)
                {
                    item.Location_Index = Guid.Parse(model.confirm_location_Index);
                    item.Location_Id = model.confirm_location_Id;
                    item.Location_Name = model.confirm_location_Name;
                    item.Document_StatusTracking = 3;
                    item.PickingToDock_By = model.create_By;
                    item.PickingToDock_Date = DateTime.Now;

                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    Result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("ScanConfirm", msglog);
                    transaction.Rollback();
                    throw exy;
                }
                if (Result.resultIsUse)
                {
                    ScanPicksearchViewModel new_model = new ScanPicksearchViewModel();
                    new_model.confirm_location_Name = model.confirm_location_Name;
                    new_model.task_Index = model.TruckLoad_Index.ToString();
                    new_model.confirm_location_Index = model.confirm_location_Index;
                    new_model.confirm_location_Id = model.confirm_location_Id;
                    new_model.userName = model.userName;
                    var re = ScanConfirmLocatonstgtoDock(new_model);
                    if (!re)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task Error";
                        return Result;
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirm", msglog);
                var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region Scan Confirm Location stg to Dock
        public bool ScanConfirmLocatonstgtoDock(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            logtxt log = new logtxt();
            log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + DateTime.Now.ToString("yyyy-MM-dd"), "model : " + JsonConvert.SerializeObject(model) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            try
            {
                db.Database.SetCommandTimeout(120);
                //----------------------------------------------------------------------------------------------------------- step 1 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 1 : Get Config Key " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());

                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = model.confirm_location_Name } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;
                //----------------------------------------------------------------------------------------------------------- step 2 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 2 : CheckTag " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                if (CheckTag.Count() == 0)
                {
                    var tag2 = new { tag_no = model.confirm_location_Name };
                    //----------------------------------------------------------------------------------------------------------- step 3 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 3 : CreateTagHeader data : " + tag2 + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
                    //----------------------------------------------------------------------------------------------------------- step 4 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 4 : CheckTag data : " + tag + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                }

                var task_I = db.View_Taskitem_with_Truckload.Where(c => c.TruckLoad_Index == Guid.Parse(model.task_Index) && c.Document_StatusTracking == 3).GroupBy(c => c.Task_Index).Select(c => c.Key).ToList();

                //----------------------------------------------------------------------------------------------------------- step 5 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard " + model.task_Index + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var data = db.View_TaskInsertBinCard_V2.Where(c => task_I.Contains(c.Task_Index) && c.Document_StatusTracking == 3).ToList();
                log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard : " + JsonConvert.SerializeObject(data) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
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
                    //----------------------------------------------------------------------------------------------------------- step 6 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 6 : InsertBinCardBy_Scanpick : " + JsonConvert.SerializeObject(View_TaskInsertBinCard) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardBy_Scanpick"), View_TaskInsertBinCard.sJson());
                    log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 6 : InsertBinCardBy_Scanpick result : " + JsonConvert.SerializeObject(Bincard) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var bin_index = new { binbalance_Index = Bincard };

                        var findBin = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), bin_index.sJson());

                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);
                        taskitem.Document_StatusTracking = 2;
                        taskitem.UDF_5 = model.confirm_location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        //taskitem.DocumentRef_No4 = taskitem.Tag_No;
                        taskitem.Tag_Index = findBin.tag_Index;
                        taskitem.Tag_No = findBin.tag_No;

                        //----------------------------------------------------------------------------------------------------------- step 7 -----------------------------------------------------------------------------------------------------------
                        log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save Time : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Error" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("ScanConfirmLocatonPick", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                    var chkTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == d.Taskitem_Index && (c.Document_StatusTracking == 1 || c.Document_StatusTracking == 3 || (c.Document_StatusTracking != null ? c.Document_StatusTracking : 0) == 0)).Count();
                    if (chkTaskItem == 0)
                    {
                        var task = db.IM_Task.Find(d.Task_Index);

                        var taskitem = db.IM_TaskItem.Where(c => c.Task_Index == task.Task_Index && c.Document_StatusTracking == 2).GroupBy(g => g.Ref_Document_Index);
                        var GI = db.IM_GoodsIssue.Where(c => taskitem.Select(s => s.Key).Contains(c.GoodsIssue_Index)).ToList();
                        foreach (var gi in GI)
                        {
                            gi.Document_Status = 3;
                        }


                        task.Document_StatusTracking = 2;
                        task.Update_Date = DateTime.Now;
                        task.Update_By = model.userName;
                        //----------------------------------------------------------------------------------------------------------- step 10 -----------------------------------------------------------------------------------------------------------
                        log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Time" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Error" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("ScanConfirmLocatonPick", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                

                return true;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirmLocatonstgtoDock", "ScanConfirmLocatonstgtoDock" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "EX" + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirmLocatonPick", msglog);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion
        
        #region Bypass LBL

        #region ScanConfirmPickQty_bypassLBL
        public actionResultScanPickViewModel ScanConfirmPickQty_bypassLBL(ScanPickViewModel_Qty model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(120);
                var Result = new actionResultScanPickViewModel();
                model.confirm_location_Index = "B41DAB2A-1297-475A-BD0D-78373323A8B9";
                model.confirm_location_Id = "53";
                model.confirm_location_Name = "RC-DK-001";


                var ListTaskItem = db.IM_TaskItem.Where(c => c.Ref_Document_Index == Guid.Parse(model.goodsissue_index)).ToList();

                foreach (var TaskItem in ListTaskItem)
                {
                    if (TaskItem.PickingPickQty_Status == null)
                    {
                        TaskItem.Picking_Qty = TaskItem.Qty;
                        TaskItem.Picking_Ratio = TaskItem.Ratio;
                        TaskItem.Picking_TotalQty = TaskItem.TotalQty;
                        TaskItem.Picking_By = model.userName;
                        TaskItem.Picking_Date = DateTime.Now;
                        TaskItem.Picking_Status = 2;
                        TaskItem.Picking_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.Picking_Location_Id = model.confirm_location_Id;
                        TaskItem.Picking_Location_Name = model.confirm_location_Name;

                        TaskItem.PickingLabel_Qty = TaskItem.Qty;
                        TaskItem.PickingLabel_Ratio = TaskItem.Ratio;
                        TaskItem.PickingLabel_TotalQty = TaskItem.TotalQty;
                        TaskItem.PickingLabeling_By = model.userName;
                        TaskItem.PickingLabeling_Date = DateTime.Now;
                        TaskItem.PickingLabeling_Status = 2;
                        TaskItem.PickingLabeling_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingLabeling_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingLabeling_Location_Name = model.confirm_location_Name;

                        TaskItem.PickingToDock_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingToDock_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingToDock_Location_Name = model.confirm_location_Name;
                        TaskItem.PickingToDock_Status = 2;
                        TaskItem.PickingToDock_By = model.create_By;
                        TaskItem.PickingToDock_Date = DateTime.Now;

                        TaskItem.Document_StatusTracking = 2;
                        TaskItem.PickingToDock_By = model.create_By;
                        TaskItem.PickingToDock_Date = DateTime.Now;

                        TaskItem.PickingPickQty_Qty = TaskItem.Qty;
                        TaskItem.PickingPickQty_Ratio = TaskItem.Ratio;
                        TaskItem.PickingPickQty_TotalQty = TaskItem.TotalQty;
                        TaskItem.PickingPickQty_By = model.userName;
                        TaskItem.PickingPickQty_Date = DateTime.Now;
                        TaskItem.PickingPickQty_Status = 1;
                        TaskItem.PickingPickQty_Location_Index = Guid.Parse(model.confirm_location_Index);
                        TaskItem.PickingPickQty_Location_Id = model.confirm_location_Id;
                        TaskItem.PickingPickQty_Location_Name = model.confirm_location_Name;
                    }
                    
                }


                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    Result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("ScanConfirm", msglog);
                    transaction.Rollback();
                    throw exy;
                }
                if (Result.resultIsUse)
                {
                    ScanPicksearchViewModel new_model = new ScanPicksearchViewModel();
                    new_model.confirm_location_Name = model.confirm_location_Name;
                    new_model.task_Index = model.task_Index;
                    new_model.confirm_location_Index = model.confirm_location_Index;
                    new_model.confirm_location_Id = model.confirm_location_Id;
                    new_model.userName = model.userName;
                    new_model.goodsissue_Index = model.goodsissue_index;
                    var re = ScanConfirmLocatonPickingQty_bypassLBL(new_model);
                    if (!re)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task Error";
                        return Result;
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                msglog = model.sJson() + " / " + State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirm", msglog);
                var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region ScanConfirmLocatonPickingQty_bypassLBL
        public bool ScanConfirmLocatonPickingQty_bypassLBL(ScanPicksearchViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            logtxt log = new logtxt();
            log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + DateTime.Now.ToString("yyyy-MM-dd"), "model : " + JsonConvert.SerializeObject(model) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            try
            {
                db.Database.SetCommandTimeout(120);
                //----------------------------------------------------------------------------------------------------------- step 1 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 1 : Get Config Key " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());

                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = model.confirm_location_Name } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;
                //----------------------------------------------------------------------------------------------------------- step 2 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 2 : CheckTag " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                if (CheckTag.Count() == 0)
                {
                    var tag2 = new { tag_no = model.confirm_location_Name };
                    //----------------------------------------------------------------------------------------------------------- step 3 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 3 : CreateTagHeader data : " + tag2 + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
                    //----------------------------------------------------------------------------------------------------------- step 4 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 4 : CheckTag data : " + tag + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                }


                var goodsissue_Index = new Guid(model.goodsissue_Index);
                //----------------------------------------------------------------------------------------------------------- step 5 -----------------------------------------------------------------------------------------------------------
                log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard " + model.task_Index + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var data = db.View_TaskInsertBinCard_wavewithoutrobot.Where(c => c.Ref_Document_Index == goodsissue_Index && c.PickingPickQty_Status == 1).ToList();
                log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 5 : View_TaskInsertBinCard : " + JsonConvert.SerializeObject(data) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
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
                        binBalance_Index = d.BinBalance_Index,
                        process_Index = new Guid(configLocation),
                        location_Index_To = new Guid(model.confirm_location_Index),
                        location_Id_To = model.confirm_location_Id,
                        location_Name_To = model.confirm_location_Name,
                        Volume = d.Volume,
                        Weight = d.Weight,
                        userName = model.userName,
                        isScanSplit = true
                    };
                    //----------------------------------------------------------------------------------------------------------- step 6 -----------------------------------------------------------------------------------------------------------
                    log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 6 : InsertBinCardBy_Scanpick : " + JsonConvert.SerializeObject(View_TaskInsertBinCard) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("insertBincard"), View_TaskInsertBinCard.sJson());
                    log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 6 : InsertBinCardBy_Scanpick result : " + JsonConvert.SerializeObject(Bincard) + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var bin_index = new { binbalance_Index = Bincard };
                        var findBin = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), bin_index.sJson());

                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);

                        if (Guid.Parse(Bincard) == taskitem.BinBalance_Index)
                        {
                            var updatetask = db.IM_Task.FirstOrDefault(c => c.Task_Index == taskitem.Task_Index);
                            updatetask.Document_StatusMovetoStgOG = 2;
                            taskitem.PickingToStg_Status = 2;
                            taskitem.PickingToStg_By = model.userName;
                            taskitem.PickingToStg_Date = DateTime.Now;
                        }
                        taskitem.PickingPickQty_Status = 2;
                        taskitem.UDF_5 = model.confirm_location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        taskitem.DocumentRef_No3 = taskitem.Tag_Index.ToString();
                        taskitem.DocumentRef_No4 = taskitem.Tag_No;
                        taskitem.Tag_Index = findBin.tag_Index;
                        taskitem.Tag_No = model.confirm_location_Name;
                        //----------------------------------------------------------------------------------------------------------- step 7 -----------------------------------------------------------------------------------------------------------
                        log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save Time : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 7 : Save Error : " + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("ScanConfirmLocatonPick", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                var chkTaskItem = db.IM_TaskItem.Where(c => c.Ref_Document_Index == goodsissue_Index && (c.PickingPickQty_Status == 1 || (c.PickingPickQty_Status != null ? c.PickingPickQty_Status : 0) == 0)).ToList();
                if (chkTaskItem.Count == 0)
                {
                    var update_task = db.IM_TaskItem.Where(c => c.Ref_Document_Index == goodsissue_Index).GroupBy(c => c.Task_Index).Select(c => c.Key).ToList();
                    foreach (var item in update_task)
                    {
                        var task = db.IM_Task.Find(item);

                        var taskitem = db.IM_TaskItem.Where(c => c.Task_Index == task.Task_Index && c.PickingPickQty_Status == 2).GroupBy(g => g.Ref_Document_Index);
                        var GI = db.IM_GoodsIssue.Where(c => taskitem.Select(s => s.Key).Contains(c.GoodsIssue_Index)).ToList();
                        foreach (var gi in GI)
                        {
                            gi.Document_Status = 3;
                        }

                        task.DocumentRef_No1 = model.confirm_location_Name;
                        task.Document_StatusScanPick = 2;
                        task.Document_StatusLabeling = 2;
                        task.Document_StatusPickQty = 2;
                        task.Document_StatusDocktoStg = 2;
                        task.Document_StatusMovetoStgOG = 2;
                        task.Document_StatusTracking = 2;
                        task.Update_Date = DateTime.Now;
                        task.Update_By = model.userName;
                        //----------------------------------------------------------------------------------------------------------- step 10 -----------------------------------------------------------------------------------------------------------
                        log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Time" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        }

                        catch (Exception exy)
                        {
                            log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "step 10 : Save Task End Error" + exy.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("ScanConfirmLocatonPick", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                    
                }

                return true;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirmLocatonPickingQty_bypassLBL", "ScanConfirmLocatonPickingQty_bypassLBL" + model.task_Index + DateTime.Now.ToString("yyyy-MM-dd"), "EX" + ex.Message.ToString() + " : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanConfirmLocatonPick", msglog);
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #endregion

        #region ScanConfirmforError

        #region ScanConfirmforError
        public actionResultScanPickViewModel ScanConfirmforError(ScanPickViewModel model)
        {
            logtxt log = new logtxt();

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            var Result = new actionResultScanPickViewModel();
            try
            {
                db.Database.SetCommandTimeout(120);

                var check_stockResult = db.View_CutError_Stock.ToList();

                foreach (var item in check_stockResult)
                {
                    var ListTaskItem = db.IM_TaskItem.Where(c => c.Tag_No == item.Tag_No && c.Picking_Status == null).ToList();

                    if (ListTaskItem.Count() != 0)
                    {
                        foreach (var TaskItem in ListTaskItem)
                        {
                            TaskItem.Picking_Qty = TaskItem.Qty;
                            TaskItem.Picking_Ratio = TaskItem.Ratio;
                            TaskItem.Picking_TotalQty = TaskItem.TotalQty;
                            TaskItem.Picking_By = "System";
                            TaskItem.Picking_Date = DateTime.Now;
                            TaskItem.Picking_Status = 1;
                            TaskItem.Pick_ProductConversion_Index = model.pick_ProductConversion_Index;
                            TaskItem.Pick_ProductConversion_Id = model.pick_ProductConversion_Id;
                            TaskItem.Pick_ProductConversion_Name = model.pick_ProductConversion_Name;
                            TaskItem.ProductConversionBarcode = model.productConversionBarcode;
                            TaskItem.TagOut_Index = model.tagOut_Index;
                            TaskItem.TagOut_No = model.tagOut_No;

                            if (!string.IsNullOrEmpty(model.reasonCode_Index.ToString()))
                            {
                                TaskItem.ReasonCode_Index = new Guid(model.reasonCode_Index.ToString());
                                TaskItem.ReasonCode_Id = model.reasonCode_Id;
                                TaskItem.ReasonCode_Name = model.reasonCode_Name;
                            }

                        }

                        var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            Result.resultIsUse = true;
                        }

                        catch (Exception exy)
                        {
                            transaction.Rollback();
                            throw exy;
                        }
                    }



                    ScanPicksearchViewModel new_model = new ScanPicksearchViewModel();
                    new_model.confirm_location_Name = "WH7TOASRS";
                    new_model.tagNo = item.Tag_No;
                    new_model.confirm_location_Index = "DF3D2410-FCBD-4C82-9F2E-65EE51A83F51";
                    new_model.confirm_location_Id = "WH7TOASRS";
                    new_model.userName = "System";
                    var re = ScanConfirmforErrorConfirm(new_model);
                    if (!re)
                    {
                        Result.resultIsUse = false;
                        Result.resultMsg = "Task Error";
                        return Result;
                    }
                }
                
                

                return Result;
            }
            catch (Exception ex)
            {
                //var Result = new actionResultScanPickViewModel();
                Result.resultIsUse = false;
                Result.resultMsg = ex.Message;
                return Result;
            }
        }
        #endregion

        #region ScanConfirmforErrorConfirm
        public bool ScanConfirmforErrorConfirm(ScanPicksearchViewModel model)
        {
            logtxt log = new logtxt();
          
            String State = "Start";
            String msglog = "";
            try
            {
                db.Database.SetCommandTimeout(120);
                var configLocation = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_Suggest_Location_Staging" }.sJson());
                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = model.location_Name } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;

                log.DataLogLines("ScanConfirmforErrorConfirm", "ScanConfirmforErrorConfirm" + DateTime.Now.ToString("yyyy-MM-dd"), JsonConvert.SerializeObject(configLocation));

                var CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                if (CheckTag.Count() == 0)
                {
                    var tag2 = new { tag_no = model.location_Name };
                    var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CreateTagHeader"), tag2.sJson());
                    CheckTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("CheckTag"), tag.sJson());
                }
                //log.DataLogLines("ScanConfirmforErrorConfirm", "ScanConfirmforErrorConfirm" + DateTime.Now.ToString("yyyy-MM-dd"), JsonConvert.SerializeObject(CheckTag));
                //var taskitem_Index = new Guid(model.taskItem_Index);
                var data = db.View_TaskInsertBinCard_wavewithoutrobot.Where(c => c.Tag_No == model.tagNo && c.PickStatus == 1).ToList();
                foreach (var d in data)
                {
                    log.DataLogLines("ScanConfirmforErrorConfirm", "ScanConfirmforErrorConfirm" + DateTime.Now.ToString("yyyy-MM-dd"), JsonConvert.SerializeObject(d));
                    log.DataLogLines("ScanConfirmforErrorConfirm", "ScanConfirmforErrorConfirm" + DateTime.Now.ToString("yyyy-MM-dd"), "CheckTag :" + CheckTag.FirstOrDefault()?.tag_Index);
                    log.DataLogLines("ScanConfirmforErrorConfirm", "ScanConfirmforErrorConfirm" + DateTime.Now.ToString("yyyy-MM-dd"), "CheckTag :" + CheckTag.FirstOrDefault()?.tag_No);
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
                        picking_Qty = d.Picking_Qty,
                        picking_Ratio = d.Picking_Ratio,
                        picking_TotalQty = d.Picking_TotalQty,
                        binBalance_Index = d.BinBalance_Index,
                        process_Index = new Guid(configLocation),
                        location_Index_To = new Guid(model.confirm_location_Index),
                        location_Id_To = model.confirm_location_Id,
                        location_Name_To = model.confirm_location_Name,
                        Volume = d.Volume,
                        Weight = d.Weight,
                        userName = model.userName
                    };
                    log.DataLogLines("ScanConfirmforErrorConfirm", "ScanConfirmforErrorConfirm" + DateTime.Now.ToString("yyyy-MM-dd"), View_TaskInsertBinCard.sJson());
                    var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("insertBincard"), View_TaskInsertBinCard.sJson());
                    if (!string.IsNullOrEmpty(Bincard))
                    {
                        var taskitem = db.IM_TaskItem.Find(d.Taskitem_Index);
                        taskitem.Picking_Status = 2;
                        taskitem.UDF_5 = model.location_Name;
                        taskitem.Update_Date = DateTime.Now;
                        taskitem.Update_By = model.userName;
                        taskitem.BinBalance_Index_New = Guid.Parse(Bincard);
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }
                              

                return true;
            }
            catch (Exception ex)
            {
                log.DataLogLines("ScanConfirmforErrorConfirm", "ScanConfirmforErrorConfirm" + DateTime.Now.ToString("yyyy-MM-dd"), ex.sJson());
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion
    }
}
