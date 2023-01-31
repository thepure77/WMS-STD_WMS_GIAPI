using AspNetCore.Reporting;
using BomBusiness;
using Business.Library;
using Comone.Utils;
using DataAccess;
using GIBusiness.AutoNumber;
using GIBusiness.Reports;
using GIDataAccess.Models;
using GRBusiness.ConfigModel;
using MasterBusiness.GoodsIssue;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using PlanGIBusiness.Libs;
using PlanGIBusiness.PlanGoodIssue;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using static GIBusiness.GoodIssue.TaskfilterViewModel;

namespace GIBusiness.GoodIssue
{
    public class AssignService
    {
        private GIDbContext db;

        public AssignService()
        {
            db = new GIDbContext();
        }
        public AssignService(GIDbContext db)
        {
            this.db = db;
        }

        #region CreateDataTable
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));

            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        #endregion

        #region assign
        public String assign(AssignJobViewModel data)
        {


            String State = "Start";
            String msglog = "";
            var olog = new logtxt();


            try
            {

                var GoodsIssueLocation = db.IM_GoodsIssueItemLocation.Where(c => data.listGoodsIssueViewModel.Select(s => s.goodsIssue_Index).Contains(c.GoodsIssue_Index)).ToList();

                var GoodsIssue = db.IM_GoodsIssue.Where(c => data.listGoodsIssueViewModel.Select(s => s.goodsIssue_Index).Contains(c.GoodsIssue_Index)).ToList();

                #region 1 : 1

                if (data.Template == "1")
                {

                    var ViewJoinWarehouse = (from GIL in GoodsIssueLocation
                                             join GI in GoodsIssue on GIL.GoodsIssue_Index equals GI.GoodsIssue_Index

                                             select new View_AssignJobViewModel
                                             {
                                                 goodsIssue_Index = GI.GoodsIssue_Index,
                                                 goodsIssue_No = GI.GoodsIssue_No,
                                                 goodsIssueItemLocation_Index = GIL.GoodsIssueItemLocation_Index,
                                                 goodsIssue_Date = GI.GoodsIssue_Date,
                                                 qty = GIL.Qty,
                                                 totalQty = GIL.TotalQty,

                                                 location_Index =  GIL.Location_Index,
                                                 location_Id = GIL.Location_Id,
                                                 location_Name = GIL.Location_Name,

                                             }).AsQueryable();



                    var ResultGroup = ViewJoinWarehouse.GroupBy(c => new { c.goodsIssue_Index , c.goodsIssue_Date, c.location_Index, c.location_Id, c.location_Name })
                     .Select(group => new
                     {
                         GI = group.Key.goodsIssue_Index,
                         GID = group.Key.goodsIssue_Date,

                         LOCI = group.Key.location_Index,
                         LOCID = group.Key.location_Id,
                         LOCN = group.Key.location_Name,

                         ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                     }).ToList();

                    foreach (var item in ResultGroup)
                    {
                       //    this.CreateTask(item.GI, item.GID, item.ResultItem, data.Create_By, data.Template);
                        this.CreateTaskByLocation(item.GI, item.GID, item.ResultItem, data.Create_By, data.Template, item.LOCI.ToString(), item.LOCN.ToString());

                    }

                }

                #endregion

                #region Warehouse

                if (data.Template == "2")
                {


                    var listDataLocation = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("getLocationMaster"), new { }.sJson());

                    var ViewJoinWarehouse = (from GIL in GoodsIssueLocation
                                             join Lo in listDataLocation on GIL.Location_Index equals Lo.location_Index
                                             join GI in GoodsIssue on GIL.GoodsIssue_Index equals GI.GoodsIssue_Index

                                             select new View_AssignJobViewModel
                                             {
                                                 goodsIssue_Index = GI.GoodsIssue_Index,
                                                 goodsIssue_No = GI.GoodsIssue_No,
                                                 goodsIssueItemLocation_Index = GIL.GoodsIssueItemLocation_Index,
                                                 warehouse_Index = Lo.warehouse_Index,
                                                 location_Index = Lo.location_Index,
                                                 goodsIssue_Date = GI.GoodsIssue_Date,
                                                 qty = GIL.Qty,
                                                 totalQty = GIL.TotalQty,

                                             }).AsQueryable();



                    var ResultGroup = ViewJoinWarehouse.GroupBy(c => new { c.warehouse_Index, c.goodsIssue_Date })
                     .Select(group => new
                     {
                         WH = group.Key.warehouse_Index,
                         GID = group.Key.goodsIssue_Date,
                         ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                     }).ToList();


                    foreach (var item in ResultGroup)
                    {
                        this.CreateTask(item.WH, item.GID, item.ResultItem, data.Create_By, data.Template);
                    }

                }

                #endregion

                #region Zone

                if (data.Template == "3")
                {
                    var listDataZonelocation = utils.SendDataApi<List<ZoneLocationViewModel>>(new AppSettingConfig().GetUrl("getZoneLocationMaster"), new { }.sJson());

                    var ViewJoine = (from GIL in GoodsIssueLocation
                                     join Zo in listDataZonelocation on GIL.Location_Index equals Zo.location_Index
                                     join GI in GoodsIssue on GIL.GoodsIssue_Index equals GI.GoodsIssue_Index

                                     select new View_AssignJobViewModel
                                     {
                                         goodsIssue_Index = GI.GoodsIssue_Index,
                                         goodsIssue_No = GI.GoodsIssue_No,
                                         goodsIssueItemLocation_Index = GIL.GoodsIssueItemLocation_Index,
                                         zone_Index = Zo.zone_Index,
                                         location_Index = Zo.location_Index,
                                         goodsIssue_Date = GI.GoodsIssue_Date,
                                         qty = GIL.Qty,
                                         totalQty = GIL.TotalQty,
                                     }).AsQueryable();



                    var ResultGroup = ViewJoine.GroupBy(c => new { c.zone_Index, c.goodsIssue_Date })
                     .Select(group => new
                     {
                         Zo = group.Key.zone_Index,
                         GID = group.Key.goodsIssue_Date,
                         ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                     }).ToList();


                    foreach (var item in ResultGroup)
                    {
                        this.CreateTask(item.Zo, item.GID, item.ResultItem, data.Create_By, data.Template);
                    }

                }

                #endregion

                #region Sales Order

                if (data.Template == "4")
                {
                    var listDataplanGoodsIssue = new List<PlanGoodIssueViewModel>();

                    var resultPlanGoodsIssue = new DocumentViewModel();

                    resultPlanGoodsIssue.listDocumentViewModel = new List<DocumentViewModel>();


                    foreach (var item in data.listGoodsIssueViewModel)
                    {
                        var Plan = new DocumentViewModel();

                        Plan.document_No = item.planGoodsIssue_No;

                        resultPlanGoodsIssue.listDocumentViewModel.Add(Plan);

                    }


                    listDataplanGoodsIssue = utils.SendDataApi<List<PlanGoodIssueViewModel>>(new AppSettingConfig().GetUrl("FindPlanGi"), resultPlanGoodsIssue.sJson());

                    var ViewJoin = (from GIL in GoodsIssueLocation
                                    join PGI in listDataplanGoodsIssue on GIL.Ref_Document_Index equals PGI.planGoodsIssue_Index
                                    join GI in GoodsIssue on GIL.GoodsIssue_Index equals GI.GoodsIssue_Index

                                    select new View_AssignJobViewModel
                                    {
                                        goodsIssue_Index = GI.GoodsIssue_Index,
                                        goodsIssue_No = GI.GoodsIssue_No,
                                        goodsIssueItemLocation_Index = GIL.GoodsIssueItemLocation_Index,
                                        planGoodsIssue_Index = PGI.planGoodsIssue_Index,
                                        goodsIssue_Date = GI.GoodsIssue_Date,
                                        qty = GIL.Qty,
                                        totalQty = GIL.TotalQty,

                                    }).AsQueryable();


                    var ResultGroup = ViewJoin.GroupBy(c => new { c.planGoodsIssue_Index, c.goodsIssue_Date })
                        .Select(group => new
                        {
                            pgi = group.Key.planGoodsIssue_Index,
                            GID = group.Key.goodsIssue_Date,
                            ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                        }).ToList();


                    foreach (var item in ResultGroup)
                    {
                        this.CreateTask(item.pgi, item.GID, item.ResultItem, data.Create_By, data.Template);
                    }


                }


                #endregion

                #region Route

                if (data.Template == "5")
                {
                    var listDataplanGoodsIssue = new List<PlanGoodIssueViewModel>();

                    var planGoodsIssue = new List<DocumentViewModel>();

                    var resultPlanGoodsIssue = new DocumentViewModel();

                    resultPlanGoodsIssue.listDocumentViewModel = new List<DocumentViewModel>();


                    foreach (var item in data.listGoodsIssueViewModel)
                    {
                        var Plan = new DocumentViewModel();

                        Plan.document_No = item.planGoodsIssue_No;

                        resultPlanGoodsIssue.listDocumentViewModel.Add(Plan);

                    }


                    listDataplanGoodsIssue = utils.SendDataApi<List<PlanGoodIssueViewModel>>(new AppSettingConfig().GetUrl("FindPlanGi"), resultPlanGoodsIssue.sJson());

                    var ViewJoin = (from GIL in GoodsIssueLocation
                                    join PGI in listDataplanGoodsIssue on GIL.Ref_Document_Index equals PGI.planGoodsIssue_Index
                                    join GI in GoodsIssue on GIL.GoodsIssue_Index equals GI.GoodsIssue_Index

                                    select new View_AssignJobViewModel
                                    {
                                        goodsIssue_Index = GI.GoodsIssue_Index,
                                        goodsIssue_No = GI.GoodsIssue_No,
                                        goodsIssueItemLocation_Index = GIL.GoodsIssueItemLocation_Index,
                                        route_Index = PGI.route_Index,
                                        goodsIssue_Date = GI.GoodsIssue_Date,
                                        qty = GIL.Qty,
                                        totalQty = GIL.TotalQty,

                                    }).AsQueryable();


                    var ResultGroup = ViewJoin.GroupBy(c => new { c.route_Index, c.goodsIssue_Date })
                        .Select(group => new
                        {
                            Ro = group.Key.route_Index,
                            GID = group.Key.goodsIssue_Date,
                            ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                        }).ToList();

                    foreach (var item in ResultGroup)
                    {
                        this.CreateTask(item.Ro, item.GID, item.ResultItem, data.Create_By, data.Template);
                    }


                }


                #endregion

                #region Location

                if (data.Template == "6")
                {
                    var listDataZonelocation = utils.SendDataApi<List<ZoneLocationViewModel>>(new AppSettingConfig().GetUrl("getZoneLocationMaster"), new { }.sJson());


                    var ViewJoine = (from GIL in GoodsIssueLocation
                                     join Lo in listDataZonelocation on GIL.Location_Index equals Lo.location_Index
                                     join GI in GoodsIssue on GIL.GoodsIssue_Index equals GI.GoodsIssue_Index

                                     select new View_AssignJobViewModel
                                     {
                                         goodsIssue_Index = GI.GoodsIssue_Index,
                                         goodsIssue_No = GI.GoodsIssue_No,
                                         goodsIssueItemLocation_Index = GIL.GoodsIssueItemLocation_Index,
                                         location_Index = Lo.location_Index,
                                         goodsIssue_Date = GI.GoodsIssue_Date,
                                         qty = GIL.Qty,
                                         totalQty = GIL.TotalQty,
                                     }).AsQueryable();



                    var ResultGroup = ViewJoine.GroupBy(c => new { c.location_Index, c.goodsIssue_Date })
                     .Select(group => new
                     {
                         Lo = group.Key.location_Index,
                         GID = group.Key.goodsIssue_Date,
                         ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                     }).ToList();


                    foreach (var item in ResultGroup)
                    {
                        this.CreateTask(item.Lo, item.GID, item.ResultItem, data.Create_By, data.Template);
                    }

                }


                #endregion

                #region Location && Sales Order
                if (data.Template == "7")
                {
                    var GoodsIssueLocationvalidate = db.IM_GoodsIssueItemLocation.Where(c => data.listGoodsIssueViewModel.Select(s => s.goodsIssue_Index).Contains(c.GoodsIssue_Index) && c.Ref_Document_Index == null).ToList();
                    if (GoodsIssueLocationvalidate.Count > 0)
                    {
                        return msglog = "ไม่สามารถมอบหมายงานได้เนื่องจากไม่มีเลขที่ใบแจ้งสั่งซื้อสินค้า";
                    }


                    var listDataplanGoodsIssue = new List<PlanGoodIssueViewModel>();

                    var resultPlanGoodsIssue = new DocumentViewModel();

                    resultPlanGoodsIssue.listDocumentViewModel = new List<DocumentViewModel>();


                    foreach (var item in data.listGoodsIssueViewModel)
                    {

                        var listPGI_No = item.planGoodsIssue_No.Split(',');

                        foreach (var p in listPGI_No)
                        {
                            var Plan = new DocumentViewModel();
                            Plan.document_No = p.Trim();
                            resultPlanGoodsIssue.listDocumentViewModel.Add(Plan);
                        }
                    }


                    listDataplanGoodsIssue = utils.SendDataApi<List<PlanGoodIssueViewModel>>(new AppSettingConfig().GetUrl("FindPlanGi"), resultPlanGoodsIssue.sJson());

                    var listDataBom = utils.SendDataApi<List<BomViewModel>>(new AppSettingConfig().GetUrl("FilterBom"), resultPlanGoodsIssue.sJson());

                    //var listDataZonelocation = utils.SendDataApi<List<ZoneLocationViewModel>>(new AppSettingConfig().GetUrl("getZoneLocationMaster"), new { }.sJson());
                    if (listDataplanGoodsIssue.Count() == 0 && listDataBom.Count() == 0)
                    {
                        return msglog = "ไม่สามารถมอบหมายงานได้";
                    }

                    var ViewJoine = new List<View_AssignJobViewModel>();
                    if (listDataplanGoodsIssue.Count() > 0)
                    {
                        ViewJoine = (from GIL in GoodsIssueLocation
                                     join PGI in listDataplanGoodsIssue on GIL.Ref_Document_Index equals PGI.planGoodsIssue_Index
                                     join GI in GoodsIssue on GIL.GoodsIssue_Index equals GI.GoodsIssue_Index
                                     //join Lo in listDataZonelocation on GIL.Location_Index equals Lo.location_Index

                                     select new View_AssignJobViewModel
                                     {
                                         goodsIssue_Index = GI.GoodsIssue_Index,
                                         goodsIssue_No = GI.GoodsIssue_No,
                                         goodsIssueItemLocation_Index = GIL.GoodsIssueItemLocation_Index,
                                         location_Index = GIL.Location_Index,
                                         goodsIssue_Date = GI.GoodsIssue_Date,
                                         planGoodsIssue_Index = PGI.planGoodsIssue_Index,
                                         product_Index = GIL.Product_Index,
                                         qty = GIL.Qty,
                                         totalQty = GIL.TotalQty,
                                     }).ToList();
                    }
                    if (listDataBom.Count() > 0)
                    {
                        ViewJoine = (from GIL in GoodsIssueLocation
                                     join PGI in listDataBom on GIL.Ref_Document_Index equals PGI.BOM_Index
                                     join GI in GoodsIssue on GIL.GoodsIssue_Index equals GI.GoodsIssue_Index
                                     //join Lo in listDataZonelocation on GIL.Location_Index equals Lo.location_Index

                                     select new View_AssignJobViewModel
                                     {
                                         goodsIssue_Index = GI.GoodsIssue_Index,
                                         goodsIssue_No = GI.GoodsIssue_No,
                                         goodsIssueItemLocation_Index = GIL.GoodsIssueItemLocation_Index,
                                         location_Index = GIL.Location_Index,
                                         goodsIssue_Date = GI.GoodsIssue_Date,
                                         planGoodsIssue_Index = PGI.BOM_Index,
                                         product_Index = GIL.Product_Index,
                                         qty = GIL.Qty,
                                         totalQty = GIL.TotalQty,
                                     }).ToList();
                    }



                    var ResultGroup = ViewJoine.GroupBy(c => new { c.location_Index, c.planGoodsIssue_Index, c.product_Index, c.goodsIssue_Date, })
                     .Select(group => new
                     {
                         Lo = group.Key.location_Index,
                         GID = group.Key.goodsIssue_Date,
                         //pgi = group.Key.planGoodsIssue_Index,
                         //pro = group.Key.product_Index,
                         ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                     }).ToList();


                    foreach (var item in ResultGroup)
                    {
                        this.CreateTask(item.Lo, item.GID, item.ResultItem, data.Create_By, data.Template);
                    }
                }
                #endregion




                #region Update Status GI 

                foreach (var ResultGoodsIssue in GoodsIssue)
                {
                    var FindGoodsIssue = db.IM_GoodsIssue.Find(ResultGoodsIssue.GoodsIssue_Index);
                    FindGoodsIssue.Document_Status = 2;
                }

                #endregion


                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveTask", msglog);
                    transaction.Rollback();

                    return exy.ToString();

                }

                return "true";

            }

            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion

        #region CreateTask
        public String CreateTask(Guid? Index, DateTime? GID, List<View_AssignJobViewModel> ResultItem, String Create_By, String Tempalate)
        {
            decimal GILQty = 0;
            decimal CountQty = 0;
            decimal QtyBreak = 50000;
            String TaskIndex = "";
            String TaskNo = "";

            try
            {

                if (Tempalate != "1")
                {
                    foreach (var itemResult in ResultItem)
                    {

                        GILQty = itemResult.qty;


                        for (int i = (int)Math.Ceiling(GILQty); i > 0;)
                        {
                            if (CountQty == 0)
                            {
                                #region Create Task Header


                                #region GenDoc
                                var Gen = new List<GenDocumentTypeViewModel>();
                                var filterModel = new GenDocumentTypeViewModel();
                                filterModel.process_Index = new Guid("065A991E-77BD-4D28-83A7-0060ED68DE26");
                                filterModel.documentType_Index = new Guid("2F1985B9-E9E8-4059-9320-E07B4FB66E9D");
                                Gen = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                                ///////////////////
                                var genDoc = new AutoNumberService();
                                //string DocNo = "";
                                DateTime DocumentDate = DateTime.Now;
                                TaskNo = genDoc.genAutoDocmentNumber(Gen, DocumentDate);
                                #endregion

                                var result = new im_Task();

                                result.Task_Index = Guid.NewGuid();
                                result.Task_No = TaskNo;
                                result.Document_Status = 0;
                                result.Create_By = Create_By;
                                result.Create_Date = DateTime.Now;

                                db.IM_Task.Add(result);

                                TaskIndex = result.Task_Index.ToString();
                                CountQty = QtyBreak;

                                #endregion
                            }

                            #region Create TaskItem

                            var resultItem = new im_TaskItem();

                            var FindGIL = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssueItemLocation_Index == itemResult.goodsIssueItemLocation_Index).FirstOrDefault();


                            if (GILQty >= CountQty)
                            {

                                resultItem.TaskItem_Index = Guid.NewGuid();
                                resultItem.Task_Index = new Guid(TaskIndex);
                                resultItem.Task_No = TaskNo;
                                resultItem.Tag_Index = FindGIL.Tag_Index;
                                resultItem.TagItem_Index = FindGIL.TagItem_Index;
                                resultItem.Tag_No = FindGIL.Tag_No;
                                resultItem.Product_Index = FindGIL.Product_Index;
                                resultItem.Product_Id = FindGIL.Product_Id;
                                resultItem.Product_Name = FindGIL.Product_Name;
                                resultItem.Product_SecondName = FindGIL.Product_SecondName;
                                resultItem.Product_ThirdName = FindGIL.Product_ThirdName;
                                resultItem.Product_Lot = FindGIL.Product_Lot;
                                resultItem.ItemStatus_Index = FindGIL.ItemStatus_Index;
                                resultItem.ItemStatus_Id = FindGIL.ItemStatus_Id;
                                resultItem.ItemStatus_Name = FindGIL.ItemStatus_Name;
                                resultItem.Location_Index = FindGIL.Location_Index;
                                resultItem.Location_Id = FindGIL.Location_Id;
                                resultItem.Location_Name = FindGIL.Location_Name;
                                resultItem.Qty = CountQty;
                                resultItem.Ratio = FindGIL.Ratio;
                                resultItem.TotalQty = (resultItem.Qty * resultItem.Ratio);
                                resultItem.ProductConversion_Index = FindGIL.ProductConversion_Index;
                                resultItem.ProductConversion_Id = FindGIL.ProductConversion_Id;
                                resultItem.ProductConversion_Name = FindGIL.ProductConversion_Name;
                                resultItem.MFG_Date = FindGIL.MFG_Date;
                                resultItem.EXP_Date = FindGIL.EXP_Date;

                                resultItem.UnitWeight = FindGIL.UnitWeight;
                                resultItem.UnitWeight_Index = FindGIL.UnitWeight_Index;
                                resultItem.UnitWeight_Id = FindGIL.UnitWeight_Id;
                                resultItem.UnitWeight_Name = FindGIL.UnitWeight_Name;
                                resultItem.UnitWeightRatio = FindGIL.UnitWeightRatio;

                                resultItem.Weight = (resultItem.Qty ?? 0) * (FindGIL.UnitWeight ?? 0);
                                resultItem.Weight_Index = FindGIL.Weight_Index;
                                resultItem.Weight_Id = FindGIL.Weight_Id;
                                resultItem.Weight_Name = FindGIL.Weight_Name;
                                resultItem.WeightRatio = FindGIL.WeightRatio;

                                resultItem.UnitNetWeight = FindGIL.UnitNetWeight;
                                resultItem.UnitNetWeight_Index = FindGIL.UnitNetWeight_Index;
                                resultItem.UnitNetWeight_Id = FindGIL.UnitNetWeight_Id;
                                resultItem.UnitNetWeight_Name = FindGIL.UnitNetWeight_Name;
                                resultItem.UnitNetWeightRatio = FindGIL.UnitNetWeightRatio;

                                resultItem.NetWeight = (resultItem.Qty ?? 0) * (FindGIL.UnitNetWeight ?? 0);
                                resultItem.NetWeight_Index = FindGIL.NetWeight_Index;
                                resultItem.NetWeight_Id = FindGIL.NetWeight_Id;
                                resultItem.NetWeight_Name = FindGIL.NetWeight_Name;
                                resultItem.NetWeightRatio = FindGIL.NetWeightRatio;

                                resultItem.UnitGrsWeight = FindGIL.UnitGrsWeight;
                                resultItem.UnitGrsWeight_Index = FindGIL.UnitGrsWeight_Index;
                                resultItem.UnitGrsWeight_Id = FindGIL.UnitGrsWeight_Id;
                                resultItem.UnitGrsWeight_Name = FindGIL.UnitGrsWeight_Name;
                                resultItem.UnitGrsWeightRatio = FindGIL.UnitGrsWeightRatio;

                                resultItem.GrsWeight = (resultItem.Qty ?? 0) * (FindGIL.UnitGrsWeight ?? 0);
                                resultItem.GrsWeight_Index = FindGIL.GrsWeight_Index;
                                resultItem.GrsWeight_Id = FindGIL.GrsWeight_Id;
                                resultItem.GrsWeight_Name = FindGIL.GrsWeight_Name;
                                resultItem.GrsWeightRatio = FindGIL.GrsWeightRatio;

                                resultItem.UnitWidth = FindGIL.UnitWidth;
                                resultItem.UnitWidth_Index = FindGIL.UnitWidth_Index;
                                resultItem.UnitWidth_Id = FindGIL.UnitWidth_Id;
                                resultItem.UnitWidth_Name = FindGIL.UnitWidth_Name;
                                resultItem.UnitWidthRatio = FindGIL.UnitWidthRatio;

                                resultItem.Width = (resultItem.Qty ?? 0) * FindGIL.UnitWidth;
                                resultItem.Width_Index = FindGIL.Width_Index;
                                resultItem.Width_Id = FindGIL.Width_Id;
                                resultItem.Width_Name = FindGIL.Width_Name;
                                resultItem.WidthRatio = FindGIL.WidthRatio;

                                resultItem.UnitLength = FindGIL.UnitLength;
                                resultItem.UnitLength_Index = FindGIL.UnitLength_Index;
                                resultItem.UnitLength_Id = FindGIL.UnitLength_Id;
                                resultItem.UnitLength_Name = FindGIL.UnitLength_Name;
                                resultItem.UnitLengthRatio = FindGIL.UnitLengthRatio;

                                resultItem.Length = (resultItem.Qty ?? 0) * FindGIL.UnitLength;
                                resultItem.Length_Index = FindGIL.Length_Index;
                                resultItem.Length_Id = FindGIL.Length_Id;
                                resultItem.Length_Name = FindGIL.Length_Name;
                                resultItem.LengthRatio = FindGIL.LengthRatio;

                                resultItem.UnitHeight = FindGIL.UnitHeight;
                                resultItem.UnitHeight_Index = FindGIL.UnitHeight_Index;
                                resultItem.UnitHeight_Id = FindGIL.UnitHeight_Id;
                                resultItem.UnitHeight_Name = FindGIL.UnitHeight_Name;
                                resultItem.UnitHeightRatio = FindGIL.UnitHeightRatio;

                                resultItem.Height = (resultItem.Qty ?? 0) * FindGIL.UnitHeight;
                                resultItem.Height_Index = FindGIL.Height_Index;
                                resultItem.Height_Id = FindGIL.Height_Id;
                                resultItem.Height_Name = FindGIL.Height_Name;
                                resultItem.HeightRatio = FindGIL.HeightRatio;

                                resultItem.UnitVolume = FindGIL.UnitVolume;
                                resultItem.Volume = (resultItem.Qty ?? 0) * FindGIL.UnitVolume;

                                resultItem.UnitPrice = FindGIL.UnitPrice;
                                resultItem.UnitPrice_Index = FindGIL.UnitPrice_Index;
                                resultItem.UnitPrice_Id = FindGIL.UnitPrice_Id;
                                resultItem.UnitPrice_Name = FindGIL.UnitPrice_Name;
                                resultItem.Price = (resultItem.Qty ?? 0) * FindGIL.UnitPrice;
                                resultItem.Price_Index = FindGIL.Price_Index;
                                resultItem.Price_Id = FindGIL.Price_Id;
                                resultItem.Price_Name = FindGIL.Price_Name;

                                resultItem.DocumentRef_No1 = FindGIL.DocumentRef_No1;
                                resultItem.DocumentRef_No2 = FindGIL.DocumentRef_No2;
                                resultItem.DocumentRef_No3 = FindGIL.DocumentRef_No3;
                                resultItem.DocumentRef_No4 = FindGIL.DocumentRef_No4;
                                resultItem.DocumentRef_No5 = FindGIL.DocumentRef_No5;
                                resultItem.Document_Status = 0;
                                resultItem.UDF_1 = FindGIL.UDF_1;
                                resultItem.UDF_2 = FindGIL.UDF_2;
                                resultItem.UDF_3 = FindGIL.UDF_3;
                                resultItem.UDF_4 = FindGIL.UDF_2;
                                resultItem.UDF_5 = FindGIL.UDF_5;
                                resultItem.Ref_Process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                                resultItem.Ref_Document_Index = FindGIL.GoodsIssue_Index;
                                resultItem.Ref_Document_No = itemResult.goodsIssue_No;
                                resultItem.Ref_Document_LineNum = FindGIL.LineNum;
                                resultItem.Ref_DocumentItem_Index = FindGIL.GoodsIssueItemLocation_Index;
                                resultItem.PlanGoodsIssue_Index = FindGIL.Ref_Document_Index;
                                resultItem.PlanGoodsIssueItem_Index = FindGIL.Ref_DocumentItem_Index;
                                resultItem.PlanGoodsIssue_No = FindGIL.Ref_Document_No;
                                resultItem.Create_By = Create_By;
                                resultItem.Create_Date = DateTime.Now;
                                resultItem.BinBalance_Index = FindGIL.BinBalance_Index;

                                resultItem.Invoice_No = FindGIL.Invoice_No;
                                resultItem.Invoice_No_Out = FindGIL.Invoice_No_Out;
                                resultItem.Declaration_No = FindGIL.Declaration_No;
                                resultItem.Declaration_No_Out = FindGIL.Declaration_No_Out;
                                resultItem.HS_Code = FindGIL.HS_Code;
                                resultItem.Conutry_of_Origin = FindGIL.Conutry_of_Origin;
                                resultItem.Tax1 = FindGIL.Tax1;
                                resultItem.Tax1_Currency_Index = FindGIL.Tax1_Currency_Index;
                                resultItem.Tax1_Currency_Id = FindGIL.Tax1_Currency_Id;
                                resultItem.Tax1_Currency_Name = FindGIL.Tax1_Currency_Name;
                                resultItem.Tax2 = FindGIL.Tax2;
                                resultItem.Tax2_Currency_Index = FindGIL.Tax2_Currency_Index;
                                resultItem.Tax2_Currency_Id = FindGIL.Tax2_Currency_Id;
                                resultItem.Tax2_Currency_Name = FindGIL.Tax2_Currency_Name;
                                resultItem.Tax3 = FindGIL.Tax3;
                                resultItem.Tax3_Currency_Index = FindGIL.Tax3_Currency_Index;
                                resultItem.Tax3_Currency_Id = FindGIL.Tax3_Currency_Id;
                                resultItem.Tax3_Currency_Name = FindGIL.Tax3_Currency_Name;
                                resultItem.Tax4 = FindGIL.Tax4;
                                resultItem.Tax4_Currency_Index = FindGIL.Tax4_Currency_Index;
                                resultItem.Tax4_Currency_Id = FindGIL.Tax4_Currency_Id;
                                resultItem.Tax4_Currency_Name = FindGIL.Tax4_Currency_Name;
                                resultItem.Tax5 = FindGIL.Tax5;
                                resultItem.Tax5_Currency_Index = FindGIL.Tax5_Currency_Index;
                                resultItem.Tax5_Currency_Id = FindGIL.Tax5_Currency_Id;
                                resultItem.Tax5_Currency_Name = FindGIL.Tax5_Currency_Name;

                                resultItem.ERP_Location = FindGIL.ERP_Location;

                                db.IM_TaskItem.Add(resultItem);

                                GILQty = GILQty - CountQty;
                                CountQty = 0;
                                i = (int)Math.Ceiling(GILQty);


                            }

                            else if (GILQty < QtyBreak)
                            {
                                resultItem.TaskItem_Index = Guid.NewGuid();
                                resultItem.Task_Index = new Guid(TaskIndex);
                                resultItem.Task_No = TaskNo;
                                resultItem.Tag_Index = FindGIL.Tag_Index;
                                resultItem.TagItem_Index = FindGIL.TagItem_Index;
                                resultItem.Tag_No = FindGIL.Tag_No;
                                resultItem.Product_Index = FindGIL.Product_Index;
                                resultItem.Product_Id = FindGIL.Product_Id;
                                resultItem.Product_Name = FindGIL.Product_Name;
                                resultItem.Product_SecondName = FindGIL.Product_SecondName;
                                resultItem.Product_ThirdName = FindGIL.Product_ThirdName;
                                resultItem.Product_Lot = FindGIL.Product_Lot;
                                resultItem.ItemStatus_Index = FindGIL.ItemStatus_Index;
                                resultItem.ItemStatus_Id = FindGIL.ItemStatus_Id;
                                resultItem.ItemStatus_Name = FindGIL.ItemStatus_Name;
                                resultItem.Location_Index = FindGIL.Location_Index;
                                resultItem.Location_Id = FindGIL.Location_Id;
                                resultItem.Location_Name = FindGIL.Location_Name;
                                resultItem.Qty = GILQty;
                                resultItem.Ratio = FindGIL.Ratio;
                                resultItem.TotalQty = (resultItem.Qty * resultItem.Ratio);
                                resultItem.ProductConversion_Index = FindGIL.ProductConversion_Index;
                                resultItem.ProductConversion_Id = FindGIL.ProductConversion_Id;
                                resultItem.ProductConversion_Name = FindGIL.ProductConversion_Name;
                                resultItem.MFG_Date = FindGIL.MFG_Date;
                                resultItem.EXP_Date = FindGIL.EXP_Date;

                                resultItem.UnitWeight = FindGIL.UnitWeight;
                                resultItem.UnitWeight_Index = FindGIL.UnitWeight_Index;
                                resultItem.UnitWeight_Id = FindGIL.UnitWeight_Id;
                                resultItem.UnitWeight_Name = FindGIL.UnitWeight_Name;
                                resultItem.UnitWeightRatio = FindGIL.UnitWeightRatio;

                                resultItem.Weight = (resultItem.Qty ?? 0) * (FindGIL.UnitWeight ?? 0);
                                resultItem.Weight_Index = FindGIL.Weight_Index;
                                resultItem.Weight_Id = FindGIL.Weight_Id;
                                resultItem.Weight_Name = FindGIL.Weight_Name;
                                resultItem.WeightRatio = FindGIL.WeightRatio;

                                resultItem.UnitNetWeight = FindGIL.UnitNetWeight;
                                resultItem.UnitNetWeight_Index = FindGIL.UnitNetWeight_Index;
                                resultItem.UnitNetWeight_Id = FindGIL.UnitNetWeight_Id;
                                resultItem.UnitNetWeight_Name = FindGIL.UnitNetWeight_Name;
                                resultItem.UnitNetWeightRatio = FindGIL.UnitNetWeightRatio;

                                resultItem.NetWeight = (resultItem.Qty ?? 0) * (FindGIL.UnitNetWeight ?? 0);
                                resultItem.NetWeight_Index = FindGIL.NetWeight_Index;
                                resultItem.NetWeight_Id = FindGIL.NetWeight_Id;
                                resultItem.NetWeight_Name = FindGIL.NetWeight_Name;
                                resultItem.NetWeightRatio = FindGIL.NetWeightRatio;

                                resultItem.UnitGrsWeight = FindGIL.UnitGrsWeight;
                                resultItem.UnitGrsWeight_Index = FindGIL.UnitGrsWeight_Index;
                                resultItem.UnitGrsWeight_Id = FindGIL.UnitGrsWeight_Id;
                                resultItem.UnitGrsWeight_Name = FindGIL.UnitGrsWeight_Name;
                                resultItem.UnitGrsWeightRatio = FindGIL.UnitGrsWeightRatio;

                                resultItem.GrsWeight = (resultItem.Qty ?? 0) * (FindGIL.UnitGrsWeight ?? 0);
                                resultItem.GrsWeight_Index = FindGIL.GrsWeight_Index;
                                resultItem.GrsWeight_Id = FindGIL.GrsWeight_Id;
                                resultItem.GrsWeight_Name = FindGIL.GrsWeight_Name;
                                resultItem.GrsWeightRatio = FindGIL.GrsWeightRatio;

                                resultItem.UnitWidth = FindGIL.UnitWidth;
                                resultItem.UnitWidth_Index = FindGIL.UnitWidth_Index;
                                resultItem.UnitWidth_Id = FindGIL.UnitWidth_Id;
                                resultItem.UnitWidth_Name = FindGIL.UnitWidth_Name;
                                resultItem.UnitWidthRatio = FindGIL.UnitWidthRatio;

                                resultItem.Width = (resultItem.Qty ?? 0) * FindGIL.UnitWidth;
                                resultItem.Width_Index = FindGIL.Width_Index;
                                resultItem.Width_Id = FindGIL.Width_Id;
                                resultItem.Width_Name = FindGIL.Width_Name;
                                resultItem.WidthRatio = FindGIL.WidthRatio;

                                resultItem.UnitLength = FindGIL.UnitLength;
                                resultItem.UnitLength_Index = FindGIL.UnitLength_Index;
                                resultItem.UnitLength_Id = FindGIL.UnitLength_Id;
                                resultItem.UnitLength_Name = FindGIL.UnitLength_Name;
                                resultItem.UnitLengthRatio = FindGIL.UnitLengthRatio;

                                resultItem.Length = (resultItem.Qty ?? 0) * FindGIL.UnitLength;
                                resultItem.Length_Index = FindGIL.Length_Index;
                                resultItem.Length_Id = FindGIL.Length_Id;
                                resultItem.Length_Name = FindGIL.Length_Name;
                                resultItem.LengthRatio = FindGIL.LengthRatio;

                                resultItem.UnitHeight = FindGIL.UnitHeight;
                                resultItem.UnitHeight_Index = FindGIL.UnitHeight_Index;
                                resultItem.UnitHeight_Id = FindGIL.UnitHeight_Id;
                                resultItem.UnitHeight_Name = FindGIL.UnitHeight_Name;
                                resultItem.UnitHeightRatio = FindGIL.UnitHeightRatio;

                                resultItem.Height = (resultItem.Qty ?? 0) * FindGIL.UnitHeight;
                                resultItem.Height_Index = FindGIL.Height_Index;
                                resultItem.Height_Id = FindGIL.Height_Id;
                                resultItem.Height_Name = FindGIL.Height_Name;
                                resultItem.HeightRatio = FindGIL.HeightRatio;

                                resultItem.UnitVolume = FindGIL.UnitVolume;
                                resultItem.Volume = (resultItem.Qty ?? 0) * FindGIL.UnitVolume;

                                resultItem.UnitPrice = FindGIL.UnitPrice;
                                resultItem.UnitPrice_Index = FindGIL.UnitPrice_Index;
                                resultItem.UnitPrice_Id = FindGIL.UnitPrice_Id;
                                resultItem.UnitPrice_Name = FindGIL.UnitPrice_Name;
                                resultItem.Price = (resultItem.Qty ?? 0) * FindGIL.UnitPrice;
                                resultItem.Price_Index = FindGIL.Price_Index;
                                resultItem.Price_Id = FindGIL.Price_Id;
                                resultItem.Price_Name = FindGIL.Price_Name;

                                resultItem.DocumentRef_No1 = FindGIL.DocumentRef_No1;
                                resultItem.DocumentRef_No2 = FindGIL.DocumentRef_No2;
                                resultItem.DocumentRef_No3 = FindGIL.DocumentRef_No3;
                                resultItem.DocumentRef_No4 = FindGIL.DocumentRef_No4;
                                resultItem.DocumentRef_No5 = FindGIL.DocumentRef_No5;
                                resultItem.Document_Status = 0;
                                resultItem.UDF_1 = FindGIL.UDF_1;
                                resultItem.UDF_2 = FindGIL.UDF_2;
                                resultItem.UDF_3 = FindGIL.UDF_3;
                                resultItem.UDF_4 = FindGIL.UDF_2;
                                resultItem.UDF_5 = FindGIL.UDF_5;
                                resultItem.Ref_Process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                                resultItem.Ref_Document_Index = FindGIL.GoodsIssue_Index;
                                resultItem.Ref_Document_No = itemResult.goodsIssue_No;
                                resultItem.Ref_Document_LineNum = FindGIL.LineNum;
                                resultItem.Ref_DocumentItem_Index = FindGIL.GoodsIssueItemLocation_Index;
                                resultItem.PlanGoodsIssue_Index = FindGIL.Ref_Document_Index;
                                resultItem.PlanGoodsIssueItem_Index = FindGIL.Ref_DocumentItem_Index;
                                resultItem.PlanGoodsIssue_No = FindGIL.Ref_Document_No;
                                resultItem.Create_By = Create_By;
                                resultItem.Create_Date = DateTime.Now;
                                resultItem.BinBalance_Index = FindGIL.BinBalance_Index;

                                resultItem.Invoice_No = FindGIL.Invoice_No;
                                resultItem.Invoice_No_Out = FindGIL.Invoice_No_Out;
                                resultItem.Declaration_No = FindGIL.Declaration_No;
                                resultItem.Declaration_No_Out = FindGIL.Declaration_No_Out;
                                resultItem.HS_Code = FindGIL.HS_Code;
                                resultItem.Conutry_of_Origin = FindGIL.Conutry_of_Origin;
                                resultItem.Tax1 = FindGIL.Tax1;
                                resultItem.Tax1_Currency_Index = FindGIL.Tax1_Currency_Index;
                                resultItem.Tax1_Currency_Id = FindGIL.Tax1_Currency_Id;
                                resultItem.Tax1_Currency_Name = FindGIL.Tax1_Currency_Name;
                                resultItem.Tax2 = FindGIL.Tax2;
                                resultItem.Tax2_Currency_Index = FindGIL.Tax2_Currency_Index;
                                resultItem.Tax2_Currency_Id = FindGIL.Tax2_Currency_Id;
                                resultItem.Tax2_Currency_Name = FindGIL.Tax2_Currency_Name;
                                resultItem.Tax3 = FindGIL.Tax3;
                                resultItem.Tax3_Currency_Index = FindGIL.Tax3_Currency_Index;
                                resultItem.Tax3_Currency_Id = FindGIL.Tax3_Currency_Id;
                                resultItem.Tax3_Currency_Name = FindGIL.Tax3_Currency_Name;
                                resultItem.Tax4 = FindGIL.Tax4;
                                resultItem.Tax4_Currency_Index = FindGIL.Tax4_Currency_Index;
                                resultItem.Tax4_Currency_Id = FindGIL.Tax4_Currency_Id;
                                resultItem.Tax4_Currency_Name = FindGIL.Tax4_Currency_Name;
                                resultItem.Tax5 = FindGIL.Tax5;
                                resultItem.Tax5_Currency_Index = FindGIL.Tax5_Currency_Index;
                                resultItem.Tax5_Currency_Id = FindGIL.Tax5_Currency_Id;
                                resultItem.Tax5_Currency_Name = FindGIL.Tax5_Currency_Name;
                                resultItem.ERP_Location = FindGIL.ERP_Location;

                                db.IM_TaskItem.Add(resultItem);

                                CountQty = CountQty - GILQty;
                                GILQty = 0;
                                i = (int)Math.Ceiling(GILQty);

                            }

                            #endregion
                        }

                    }

                }


                else
                {
                    #region Create Task Header



                    var result = new im_Task();


                    var Gen = new List<GenDocumentTypeViewModel>();

                    var filterModel = new GenDocumentTypeViewModel();


                    filterModel.process_Index = new Guid("065A991E-77BD-4D28-83A7-0060ED68DE26");
                    filterModel.documentType_Index = new Guid("2F1985B9-E9E8-4059-9320-E07B4FB66E9D");
                    //GetConfig
                    Gen = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                    //DataTable resultDocumentType = CreateDataTable(Gen);

                    //var DocumentType = new SqlParameter("DocumentType", SqlDbType.Structured);
                    //DocumentType.TypeName = "[dbo].[ms_DocumentTypeData]";
                    //DocumentType.Value = resultDocumentType;

                    //var DocumentType_Index = new SqlParameter("@DocumentType_Index", filterModel.documentType_Index.ToString());
                    //var DocDate = new SqlParameter("@DocDate", GID);
                    //var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                    //resultParameter.Size = 2000; // some meaningfull value
                    //resultParameter.Direction = ParameterDirection.Output;
                    //db.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate, @DocumentType, @txtReturn OUTPUT", DocumentType_Index, DocDate, DocumentType, resultParameter);
                    //TaskNo = resultParameter.Value.ToString();

                    var genDoc = new AutoNumberService();
                    //string DocNo = "";
                    DateTime DocumentDate = DateTime.Now;
                    TaskNo = genDoc.genAutoDocmentNumber(Gen, DocumentDate);


                    result.Task_Index = Guid.NewGuid();
                    result.Task_No = TaskNo;
                    result.Document_Status = 0;
                    result.Create_By = Create_By;
                    result.Create_Date = DateTime.Now;

                    db.IM_Task.Add(result);

                    #endregion

                    #region Create TaskItem

                    var FindGIL = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssue_Index == Index).ToList();

                    var TaskItem = new List<im_TaskItem>();


                    foreach (var listGIL in FindGIL)
                    {
                        var resultItem = new im_TaskItem();

                        resultItem.TaskItem_Index = Guid.NewGuid();
                        resultItem.Task_Index = result.Task_Index;
                        resultItem.Task_No = TaskNo;
                        resultItem.Tag_Index = listGIL.Tag_Index;
                        resultItem.TagItem_Index = listGIL.TagItem_Index;
                        resultItem.Tag_No = listGIL.Tag_No;
                        resultItem.Product_Index = listGIL.Product_Index;
                        resultItem.Product_Id = listGIL.Product_Id;
                        resultItem.Product_Name = listGIL.Product_Name;
                        resultItem.Product_SecondName = listGIL.Product_SecondName;
                        resultItem.Product_ThirdName = listGIL.Product_ThirdName;
                        resultItem.Product_Lot = listGIL.Product_Lot;
                        resultItem.ItemStatus_Index = listGIL.ItemStatus_Index;
                        resultItem.ItemStatus_Id = listGIL.ItemStatus_Id;
                        resultItem.ItemStatus_Name = listGIL.ItemStatus_Name;
                        resultItem.Location_Index = listGIL.Location_Index;
                        resultItem.Location_Id = listGIL.Location_Id;
                        resultItem.Location_Name = listGIL.Location_Name;
                        resultItem.Qty = listGIL.Qty;
                        resultItem.Ratio = listGIL.Ratio;
                        resultItem.TotalQty = listGIL.TotalQty;
                        resultItem.ProductConversion_Index = listGIL.ProductConversion_Index;
                        resultItem.ProductConversion_Id = listGIL.ProductConversion_Id;
                        resultItem.ProductConversion_Name = listGIL.ProductConversion_Name;
                        resultItem.MFG_Date = listGIL.MFG_Date;
                        resultItem.EXP_Date = listGIL.EXP_Date;

                        resultItem.UnitWeight = listGIL.UnitWeight;
                        resultItem.UnitWeight_Index = listGIL.UnitWeight_Index;
                        resultItem.UnitWeight_Id = listGIL.UnitWeight_Id;
                        resultItem.UnitWeight_Name = listGIL.UnitWeight_Name;
                        resultItem.UnitWeightRatio = listGIL.UnitWeightRatio;

                        resultItem.Weight = listGIL.Weight;
                        resultItem.Weight_Index = listGIL.Weight_Index;
                        resultItem.Weight_Id = listGIL.Weight_Id;
                        resultItem.Weight_Name = listGIL.Weight_Name;
                        resultItem.WeightRatio = listGIL.WeightRatio;

                        resultItem.UnitNetWeight = listGIL.UnitNetWeight;
                        resultItem.UnitNetWeight_Index = listGIL.UnitNetWeight_Index;
                        resultItem.UnitNetWeight_Id = listGIL.UnitNetWeight_Id;
                        resultItem.UnitNetWeight_Name = listGIL.UnitNetWeight_Name;
                        resultItem.UnitNetWeightRatio = listGIL.UnitNetWeightRatio;

                        resultItem.NetWeight = listGIL.NetWeight;
                        resultItem.NetWeight_Index = listGIL.NetWeight_Index;
                        resultItem.NetWeight_Id = listGIL.NetWeight_Id;
                        resultItem.NetWeight_Name = listGIL.NetWeight_Name;
                        resultItem.NetWeightRatio = listGIL.NetWeightRatio;

                        resultItem.UnitGrsWeight = listGIL.UnitGrsWeight;
                        resultItem.UnitGrsWeight_Index = listGIL.UnitGrsWeight_Index;
                        resultItem.UnitGrsWeight_Id = listGIL.UnitGrsWeight_Id;
                        resultItem.UnitGrsWeight_Name = listGIL.UnitGrsWeight_Name;
                        resultItem.UnitGrsWeightRatio = listGIL.UnitGrsWeightRatio;

                        resultItem.GrsWeight = listGIL.GrsWeight;
                        resultItem.GrsWeight_Index = listGIL.GrsWeight_Index;
                        resultItem.GrsWeight_Id = listGIL.GrsWeight_Id;
                        resultItem.GrsWeight_Name = listGIL.GrsWeight_Name;
                        resultItem.GrsWeightRatio = listGIL.GrsWeightRatio;

                        resultItem.UnitWidth = listGIL.UnitWidth;
                        resultItem.UnitWidth_Index = listGIL.UnitWidth_Index;
                        resultItem.UnitWidth_Id = listGIL.UnitWidth_Id;
                        resultItem.UnitWidth_Name = listGIL.UnitWidth_Name;
                        resultItem.UnitWidthRatio = listGIL.UnitWidthRatio;

                        resultItem.Width = listGIL.Width;
                        resultItem.Width_Index = listGIL.Width_Index;
                        resultItem.Width_Id = listGIL.Width_Id;
                        resultItem.Width_Name = listGIL.Width_Name;
                        resultItem.WidthRatio = listGIL.WidthRatio;

                        resultItem.UnitLength = listGIL.UnitLength;
                        resultItem.UnitLength_Index = listGIL.UnitLength_Index;
                        resultItem.UnitLength_Id = listGIL.UnitLength_Id;
                        resultItem.UnitLength_Name = listGIL.UnitLength_Name;
                        resultItem.UnitLengthRatio = listGIL.UnitLengthRatio;

                        resultItem.Length = listGIL.Length;
                        resultItem.Length_Index = listGIL.Length_Index;
                        resultItem.Length_Id = listGIL.Length_Id;
                        resultItem.Length_Name = listGIL.Length_Name;
                        resultItem.LengthRatio = listGIL.LengthRatio;

                        resultItem.UnitHeight = listGIL.UnitHeight;
                        resultItem.UnitHeight_Index = listGIL.UnitHeight_Index;
                        resultItem.UnitHeight_Id = listGIL.UnitHeight_Id;
                        resultItem.UnitHeight_Name = listGIL.UnitHeight_Name;
                        resultItem.UnitHeightRatio = listGIL.UnitHeightRatio;

                        resultItem.Height = listGIL.Height;
                        resultItem.Height_Index = listGIL.Height_Index;
                        resultItem.Height_Id = listGIL.Height_Id;
                        resultItem.Height_Name = listGIL.Height_Name;
                        resultItem.HeightRatio = listGIL.HeightRatio;

                        resultItem.UnitVolume = listGIL.UnitVolume;
                        resultItem.Volume = listGIL.Volume;

                        resultItem.UnitPrice = listGIL.UnitPrice;
                        resultItem.UnitPrice_Index = listGIL.UnitPrice_Index;
                        resultItem.UnitPrice_Id = listGIL.UnitPrice_Id;
                        resultItem.UnitPrice_Name = listGIL.UnitPrice_Name;
                        resultItem.Price = listGIL.Price;
                        resultItem.Price_Index = listGIL.Price_Index;
                        resultItem.Price_Id = listGIL.Price_Id;
                        resultItem.Price_Name = listGIL.Price_Name;

                        resultItem.DocumentRef_No1 = listGIL.DocumentRef_No1;
                        resultItem.DocumentRef_No2 = listGIL.DocumentRef_No2;
                        resultItem.DocumentRef_No3 = listGIL.DocumentRef_No3;
                        resultItem.DocumentRef_No4 = listGIL.DocumentRef_No4;
                        resultItem.DocumentRef_No5 = listGIL.DocumentRef_No5;
                        resultItem.Document_Status = 0;
                        resultItem.UDF_1 = listGIL.UDF_1;
                        resultItem.UDF_2 = listGIL.UDF_2;
                        resultItem.UDF_3 = listGIL.UDF_3;
                        resultItem.UDF_4 = listGIL.UDF_2;
                        resultItem.UDF_5 = listGIL.UDF_5;
                        resultItem.Ref_Process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                        resultItem.Ref_Document_Index = listGIL.GoodsIssue_Index;
                        resultItem.Ref_Document_No = listGIL.GoodsIssue_No;
                        resultItem.Ref_Document_LineNum = listGIL.LineNum;
                        resultItem.Ref_DocumentItem_Index = listGIL.GoodsIssueItemLocation_Index;
                        resultItem.PlanGoodsIssue_Index = listGIL.Ref_Document_Index;
                        resultItem.PlanGoodsIssueItem_Index = listGIL.Ref_DocumentItem_Index;
                        resultItem.PlanGoodsIssue_No = listGIL.Ref_Document_No;
                        resultItem.Create_By = Create_By;
                        resultItem.Create_Date = DateTime.Now;
                        resultItem.BinBalance_Index = listGIL.BinBalance_Index;

                        resultItem.Invoice_No = listGIL.Invoice_No;
                        resultItem.Invoice_No_Out = listGIL.Invoice_No_Out;
                        resultItem.Declaration_No = listGIL.Declaration_No;
                        resultItem.Declaration_No_Out = listGIL.Declaration_No_Out;
                        resultItem.HS_Code = listGIL.HS_Code;
                        resultItem.Conutry_of_Origin = listGIL.Conutry_of_Origin;
                        resultItem.Tax1 = listGIL.Tax1;
                        resultItem.Tax1_Currency_Index = listGIL.Tax1_Currency_Index;
                        resultItem.Tax1_Currency_Id = listGIL.Tax1_Currency_Id;
                        resultItem.Tax1_Currency_Name = listGIL.Tax1_Currency_Name;
                        resultItem.Tax2 = listGIL.Tax2;
                        resultItem.Tax2_Currency_Index = listGIL.Tax2_Currency_Index;
                        resultItem.Tax2_Currency_Id = listGIL.Tax2_Currency_Id;
                        resultItem.Tax2_Currency_Name = listGIL.Tax2_Currency_Name;
                        resultItem.Tax3 = listGIL.Tax3;
                        resultItem.Tax3_Currency_Index = listGIL.Tax3_Currency_Index;
                        resultItem.Tax3_Currency_Id = listGIL.Tax3_Currency_Id;
                        resultItem.Tax3_Currency_Name = listGIL.Tax3_Currency_Name;
                        resultItem.Tax4 = listGIL.Tax4;
                        resultItem.Tax4_Currency_Index = listGIL.Tax4_Currency_Index;
                        resultItem.Tax4_Currency_Id = listGIL.Tax4_Currency_Id;
                        resultItem.Tax4_Currency_Name = listGIL.Tax4_Currency_Name;
                        resultItem.Tax5 = listGIL.Tax5;
                        resultItem.Tax5_Currency_Index = listGIL.Tax5_Currency_Index;
                        resultItem.Tax5_Currency_Id = listGIL.Tax5_Currency_Id;
                        resultItem.Tax5_Currency_Name = listGIL.Tax5_Currency_Name;
                        resultItem.ERP_Location = listGIL.ERP_Location;

                        db.IM_TaskItem.Add(resultItem);

                    }

                    #endregion

                }


                return "success";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public String CreateTaskByLocation(Guid? Index, DateTime? GID, List<View_AssignJobViewModel> ResultItem, String Create_By, String Tempalate, String pLocation_Index, String pLocation_Name)
        {
            decimal GILQty = 0;
            decimal CountQty = 0;
            decimal QtyBreak = 50000;
            String TaskIndex = "";
            String TaskNo = "";

            try
            {

                if (Tempalate != "1")
                {
                    foreach (var itemResult in ResultItem)
                    {

                        GILQty = itemResult.qty;


                        for (int i = (int)Math.Ceiling(GILQty); i > 0;)
                        {
                            if (CountQty == 0)
                            {
                                #region Create Task Header


                                #region GenDoc
                                var Gen = new List<GenDocumentTypeViewModel>();
                                var filterModel = new GenDocumentTypeViewModel();
                                filterModel.process_Index = new Guid("065A991E-77BD-4D28-83A7-0060ED68DE26");
                                filterModel.documentType_Index = new Guid("2F1985B9-E9E8-4059-9320-E07B4FB66E9D");
                                Gen = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                                ///////////////////
                                var genDoc = new AutoNumberService();
                                //string DocNo = "";
                                DateTime DocumentDate = DateTime.Now;
                                TaskNo = genDoc.genAutoDocmentNumber(Gen, DocumentDate);
                                #endregion

                                var result = new im_Task();

                                result.Task_Index = Guid.NewGuid();
                                result.Task_No = TaskNo;
                                result.Document_Status = 1;
                                result.Create_By = Create_By;
                                result.Create_Date = DateTime.Now;

                                db.IM_Task.Add(result);

                                TaskIndex = result.Task_Index.ToString();
                                CountQty = QtyBreak;

                                #endregion
                            }

                            #region Create TaskItem

                            var resultItem = new im_TaskItem();

                            var FindGIL = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssueItemLocation_Index == itemResult.goodsIssueItemLocation_Index).FirstOrDefault();


                            if (GILQty >= CountQty)
                            {

                                resultItem.TaskItem_Index = Guid.NewGuid();
                                resultItem.Task_Index = new Guid(TaskIndex);
                                resultItem.Task_No = TaskNo;
                                resultItem.Tag_Index = FindGIL.Tag_Index;
                                resultItem.TagItem_Index = FindGIL.TagItem_Index;
                                resultItem.Tag_No = FindGIL.Tag_No;
                                resultItem.Product_Index = FindGIL.Product_Index;
                                resultItem.Product_Id = FindGIL.Product_Id;
                                resultItem.Product_Name = FindGIL.Product_Name;
                                resultItem.Product_SecondName = FindGIL.Product_SecondName;
                                resultItem.Product_ThirdName = FindGIL.Product_ThirdName;
                                resultItem.Product_Lot = FindGIL.Product_Lot;
                                resultItem.ItemStatus_Index = FindGIL.ItemStatus_Index;
                                resultItem.ItemStatus_Id = FindGIL.ItemStatus_Id;
                                resultItem.ItemStatus_Name = FindGIL.ItemStatus_Name;
                                resultItem.Location_Index = FindGIL.Location_Index;
                                resultItem.Location_Id = FindGIL.Location_Id;
                                resultItem.Location_Name = FindGIL.Location_Name;
                                resultItem.Qty = CountQty;
                                resultItem.Ratio = FindGIL.Ratio;
                                resultItem.TotalQty = (resultItem.Qty * resultItem.Ratio);
                                resultItem.ProductConversion_Index = FindGIL.ProductConversion_Index;
                                resultItem.ProductConversion_Id = FindGIL.ProductConversion_Id;
                                resultItem.ProductConversion_Name = FindGIL.ProductConversion_Name;
                                resultItem.MFG_Date = FindGIL.MFG_Date;
                                resultItem.EXP_Date = FindGIL.EXP_Date;

                                resultItem.UnitWeight = FindGIL.UnitWeight;
                                resultItem.UnitWeight_Index = FindGIL.UnitWeight_Index;
                                resultItem.UnitWeight_Id = FindGIL.UnitWeight_Id;
                                resultItem.UnitWeight_Name = FindGIL.UnitWeight_Name;
                                resultItem.UnitWeightRatio = FindGIL.UnitWeightRatio;

                                resultItem.Weight = (resultItem.Qty ?? 0) * (FindGIL.UnitWeight ?? 0);
                                resultItem.Weight_Index = FindGIL.Weight_Index;
                                resultItem.Weight_Id = FindGIL.Weight_Id;
                                resultItem.Weight_Name = FindGIL.Weight_Name;
                                resultItem.WeightRatio = FindGIL.WeightRatio;

                                resultItem.UnitNetWeight = FindGIL.UnitNetWeight;
                                resultItem.UnitNetWeight_Index = FindGIL.UnitNetWeight_Index;
                                resultItem.UnitNetWeight_Id = FindGIL.UnitNetWeight_Id;
                                resultItem.UnitNetWeight_Name = FindGIL.UnitNetWeight_Name;
                                resultItem.UnitNetWeightRatio = FindGIL.UnitNetWeightRatio;

                                resultItem.NetWeight = (resultItem.Qty ?? 0) * (FindGIL.UnitNetWeight ?? 0);
                                resultItem.NetWeight_Index = FindGIL.NetWeight_Index;
                                resultItem.NetWeight_Id = FindGIL.NetWeight_Id;
                                resultItem.NetWeight_Name = FindGIL.NetWeight_Name;
                                resultItem.NetWeightRatio = FindGIL.NetWeightRatio;

                                resultItem.UnitGrsWeight = FindGIL.UnitGrsWeight;
                                resultItem.UnitGrsWeight_Index = FindGIL.UnitGrsWeight_Index;
                                resultItem.UnitGrsWeight_Id = FindGIL.UnitGrsWeight_Id;
                                resultItem.UnitGrsWeight_Name = FindGIL.UnitGrsWeight_Name;
                                resultItem.UnitGrsWeightRatio = FindGIL.UnitGrsWeightRatio;

                                resultItem.GrsWeight = (resultItem.Qty ?? 0) * (FindGIL.UnitGrsWeight ?? 0);
                                resultItem.GrsWeight_Index = FindGIL.GrsWeight_Index;
                                resultItem.GrsWeight_Id = FindGIL.GrsWeight_Id;
                                resultItem.GrsWeight_Name = FindGIL.GrsWeight_Name;
                                resultItem.GrsWeightRatio = FindGIL.GrsWeightRatio;

                                resultItem.UnitWidth = FindGIL.UnitWidth;
                                resultItem.UnitWidth_Index = FindGIL.UnitWidth_Index;
                                resultItem.UnitWidth_Id = FindGIL.UnitWidth_Id;
                                resultItem.UnitWidth_Name = FindGIL.UnitWidth_Name;
                                resultItem.UnitWidthRatio = FindGIL.UnitWidthRatio;

                                resultItem.Width = (resultItem.Qty ?? 0) * FindGIL.UnitWidth;
                                resultItem.Width_Index = FindGIL.Width_Index;
                                resultItem.Width_Id = FindGIL.Width_Id;
                                resultItem.Width_Name = FindGIL.Width_Name;
                                resultItem.WidthRatio = FindGIL.WidthRatio;

                                resultItem.UnitLength = FindGIL.UnitLength;
                                resultItem.UnitLength_Index = FindGIL.UnitLength_Index;
                                resultItem.UnitLength_Id = FindGIL.UnitLength_Id;
                                resultItem.UnitLength_Name = FindGIL.UnitLength_Name;
                                resultItem.UnitLengthRatio = FindGIL.UnitLengthRatio;

                                resultItem.Length = (resultItem.Qty ?? 0) * FindGIL.UnitLength;
                                resultItem.Length_Index = FindGIL.Length_Index;
                                resultItem.Length_Id = FindGIL.Length_Id;
                                resultItem.Length_Name = FindGIL.Length_Name;
                                resultItem.LengthRatio = FindGIL.LengthRatio;

                                resultItem.UnitHeight = FindGIL.UnitHeight;
                                resultItem.UnitHeight_Index = FindGIL.UnitHeight_Index;
                                resultItem.UnitHeight_Id = FindGIL.UnitHeight_Id;
                                resultItem.UnitHeight_Name = FindGIL.UnitHeight_Name;
                                resultItem.UnitHeightRatio = FindGIL.UnitHeightRatio;

                                resultItem.Height = (resultItem.Qty ?? 0) * FindGIL.UnitHeight;
                                resultItem.Height_Index = FindGIL.Height_Index;
                                resultItem.Height_Id = FindGIL.Height_Id;
                                resultItem.Height_Name = FindGIL.Height_Name;
                                resultItem.HeightRatio = FindGIL.HeightRatio;

                                resultItem.UnitVolume = FindGIL.UnitVolume;
                                resultItem.Volume = (resultItem.Qty ?? 0) * FindGIL.UnitVolume;

                                resultItem.UnitPrice = FindGIL.UnitPrice;
                                resultItem.UnitPrice_Index = FindGIL.UnitPrice_Index;
                                resultItem.UnitPrice_Id = FindGIL.UnitPrice_Id;
                                resultItem.UnitPrice_Name = FindGIL.UnitPrice_Name;
                                resultItem.Price = (resultItem.Qty ?? 0) * FindGIL.UnitPrice;
                                resultItem.Price_Index = FindGIL.Price_Index;
                                resultItem.Price_Id = FindGIL.Price_Id;
                                resultItem.Price_Name = FindGIL.Price_Name;

                                resultItem.DocumentRef_No1 = FindGIL.DocumentRef_No1;
                                resultItem.DocumentRef_No2 = FindGIL.DocumentRef_No2;
                                resultItem.DocumentRef_No3 = FindGIL.DocumentRef_No3;
                                resultItem.DocumentRef_No4 = FindGIL.DocumentRef_No4;
                                resultItem.DocumentRef_No5 = FindGIL.DocumentRef_No5;
                                resultItem.Document_Status = 0;
                                resultItem.UDF_1 = FindGIL.UDF_1;
                                resultItem.UDF_2 = FindGIL.UDF_2;
                                resultItem.UDF_3 = FindGIL.UDF_3;
                                resultItem.UDF_4 = FindGIL.UDF_2;
                                resultItem.UDF_5 = FindGIL.UDF_5;
                                resultItem.Ref_Process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                                resultItem.Ref_Document_Index = FindGIL.GoodsIssue_Index;
                                resultItem.Ref_Document_No = itemResult.goodsIssue_No;
                                resultItem.Ref_Document_LineNum = FindGIL.LineNum;
                                resultItem.Ref_DocumentItem_Index = FindGIL.GoodsIssueItemLocation_Index;
                                resultItem.PlanGoodsIssue_Index = FindGIL.Ref_Document_Index;
                                resultItem.PlanGoodsIssueItem_Index = FindGIL.Ref_DocumentItem_Index;
                                resultItem.PlanGoodsIssue_No = FindGIL.Ref_Document_No;
                                resultItem.Create_By = Create_By;
                                resultItem.Create_Date = DateTime.Now;
                                resultItem.BinBalance_Index = FindGIL.BinBalance_Index;

                                resultItem.Invoice_No = FindGIL.Invoice_No;
                                resultItem.Invoice_No_Out = FindGIL.Invoice_No_Out;
                                resultItem.Declaration_No = FindGIL.Declaration_No;
                                resultItem.Declaration_No_Out = FindGIL.Declaration_No_Out;
                                resultItem.HS_Code = FindGIL.HS_Code;
                                resultItem.Conutry_of_Origin = FindGIL.Conutry_of_Origin;
                                resultItem.Tax1 = FindGIL.Tax1;
                                resultItem.Tax1_Currency_Index = FindGIL.Tax1_Currency_Index;
                                resultItem.Tax1_Currency_Id = FindGIL.Tax1_Currency_Id;
                                resultItem.Tax1_Currency_Name = FindGIL.Tax1_Currency_Name;
                                resultItem.Tax2 = FindGIL.Tax2;
                                resultItem.Tax2_Currency_Index = FindGIL.Tax2_Currency_Index;
                                resultItem.Tax2_Currency_Id = FindGIL.Tax2_Currency_Id;
                                resultItem.Tax2_Currency_Name = FindGIL.Tax2_Currency_Name;
                                resultItem.Tax3 = FindGIL.Tax3;
                                resultItem.Tax3_Currency_Index = FindGIL.Tax3_Currency_Index;
                                resultItem.Tax3_Currency_Id = FindGIL.Tax3_Currency_Id;
                                resultItem.Tax3_Currency_Name = FindGIL.Tax3_Currency_Name;
                                resultItem.Tax4 = FindGIL.Tax4;
                                resultItem.Tax4_Currency_Index = FindGIL.Tax4_Currency_Index;
                                resultItem.Tax4_Currency_Id = FindGIL.Tax4_Currency_Id;
                                resultItem.Tax4_Currency_Name = FindGIL.Tax4_Currency_Name;
                                resultItem.Tax5 = FindGIL.Tax5;
                                resultItem.Tax5_Currency_Index = FindGIL.Tax5_Currency_Index;
                                resultItem.Tax5_Currency_Id = FindGIL.Tax5_Currency_Id;
                                resultItem.Tax5_Currency_Name = FindGIL.Tax5_Currency_Name;


                                db.IM_TaskItem.Add(resultItem);

                                GILQty = GILQty - CountQty;
                                CountQty = 0;
                                i = (int)Math.Ceiling(GILQty);


                            }

                            else if (GILQty < QtyBreak)
                            {
                                resultItem.TaskItem_Index = Guid.NewGuid();
                                resultItem.Task_Index = new Guid(TaskIndex);
                                resultItem.Task_No = TaskNo;
                                resultItem.Tag_Index = FindGIL.Tag_Index;
                                resultItem.TagItem_Index = FindGIL.TagItem_Index;
                                resultItem.Tag_No = FindGIL.Tag_No;
                                resultItem.Product_Index = FindGIL.Product_Index;
                                resultItem.Product_Id = FindGIL.Product_Id;
                                resultItem.Product_Name = FindGIL.Product_Name;
                                resultItem.Product_SecondName = FindGIL.Product_SecondName;
                                resultItem.Product_ThirdName = FindGIL.Product_ThirdName;
                                resultItem.Product_Lot = FindGIL.Product_Lot;
                                resultItem.ItemStatus_Index = FindGIL.ItemStatus_Index;
                                resultItem.ItemStatus_Id = FindGIL.ItemStatus_Id;
                                resultItem.ItemStatus_Name = FindGIL.ItemStatus_Name;
                                resultItem.Location_Index = FindGIL.Location_Index;
                                resultItem.Location_Id = FindGIL.Location_Id;
                                resultItem.Location_Name = FindGIL.Location_Name;
                                resultItem.Qty = GILQty;
                                resultItem.Ratio = FindGIL.Ratio;
                                resultItem.TotalQty = (resultItem.Qty * resultItem.Ratio);
                                resultItem.ProductConversion_Index = FindGIL.ProductConversion_Index;
                                resultItem.ProductConversion_Id = FindGIL.ProductConversion_Id;
                                resultItem.ProductConversion_Name = FindGIL.ProductConversion_Name;
                                resultItem.MFG_Date = FindGIL.MFG_Date;
                                resultItem.EXP_Date = FindGIL.EXP_Date;

                                resultItem.UnitWeight = FindGIL.UnitWeight;
                                resultItem.UnitWeight_Index = FindGIL.UnitWeight_Index;
                                resultItem.UnitWeight_Id = FindGIL.UnitWeight_Id;
                                resultItem.UnitWeight_Name = FindGIL.UnitWeight_Name;
                                resultItem.UnitWeightRatio = FindGIL.UnitWeightRatio;

                                resultItem.Weight = (resultItem.Qty ?? 0) * (FindGIL.UnitWeight ?? 0);
                                resultItem.Weight_Index = FindGIL.Weight_Index;
                                resultItem.Weight_Id = FindGIL.Weight_Id;
                                resultItem.Weight_Name = FindGIL.Weight_Name;
                                resultItem.WeightRatio = FindGIL.WeightRatio;

                                resultItem.UnitNetWeight = FindGIL.UnitNetWeight;
                                resultItem.UnitNetWeight_Index = FindGIL.UnitNetWeight_Index;
                                resultItem.UnitNetWeight_Id = FindGIL.UnitNetWeight_Id;
                                resultItem.UnitNetWeight_Name = FindGIL.UnitNetWeight_Name;
                                resultItem.UnitNetWeightRatio = FindGIL.UnitNetWeightRatio;

                                resultItem.NetWeight = (resultItem.Qty ?? 0) * (FindGIL.UnitNetWeight ?? 0);
                                resultItem.NetWeight_Index = FindGIL.NetWeight_Index;
                                resultItem.NetWeight_Id = FindGIL.NetWeight_Id;
                                resultItem.NetWeight_Name = FindGIL.NetWeight_Name;
                                resultItem.NetWeightRatio = FindGIL.NetWeightRatio;

                                resultItem.UnitGrsWeight = FindGIL.UnitGrsWeight;
                                resultItem.UnitGrsWeight_Index = FindGIL.UnitGrsWeight_Index;
                                resultItem.UnitGrsWeight_Id = FindGIL.UnitGrsWeight_Id;
                                resultItem.UnitGrsWeight_Name = FindGIL.UnitGrsWeight_Name;
                                resultItem.UnitGrsWeightRatio = FindGIL.UnitGrsWeightRatio;

                                resultItem.GrsWeight = (resultItem.Qty ?? 0) * (FindGIL.UnitGrsWeight ?? 0);
                                resultItem.GrsWeight_Index = FindGIL.GrsWeight_Index;
                                resultItem.GrsWeight_Id = FindGIL.GrsWeight_Id;
                                resultItem.GrsWeight_Name = FindGIL.GrsWeight_Name;
                                resultItem.GrsWeightRatio = FindGIL.GrsWeightRatio;

                                resultItem.UnitWidth = FindGIL.UnitWidth;
                                resultItem.UnitWidth_Index = FindGIL.UnitWidth_Index;
                                resultItem.UnitWidth_Id = FindGIL.UnitWidth_Id;
                                resultItem.UnitWidth_Name = FindGIL.UnitWidth_Name;
                                resultItem.UnitWidthRatio = FindGIL.UnitWidthRatio;

                                resultItem.Width = (resultItem.Qty ?? 0) * FindGIL.UnitWidth;
                                resultItem.Width_Index = FindGIL.Width_Index;
                                resultItem.Width_Id = FindGIL.Width_Id;
                                resultItem.Width_Name = FindGIL.Width_Name;
                                resultItem.WidthRatio = FindGIL.WidthRatio;

                                resultItem.UnitLength = FindGIL.UnitLength;
                                resultItem.UnitLength_Index = FindGIL.UnitLength_Index;
                                resultItem.UnitLength_Id = FindGIL.UnitLength_Id;
                                resultItem.UnitLength_Name = FindGIL.UnitLength_Name;
                                resultItem.UnitLengthRatio = FindGIL.UnitLengthRatio;

                                resultItem.Length = (resultItem.Qty ?? 0) * FindGIL.UnitLength;
                                resultItem.Length_Index = FindGIL.Length_Index;
                                resultItem.Length_Id = FindGIL.Length_Id;
                                resultItem.Length_Name = FindGIL.Length_Name;
                                resultItem.LengthRatio = FindGIL.LengthRatio;

                                resultItem.UnitHeight = FindGIL.UnitHeight;
                                resultItem.UnitHeight_Index = FindGIL.UnitHeight_Index;
                                resultItem.UnitHeight_Id = FindGIL.UnitHeight_Id;
                                resultItem.UnitHeight_Name = FindGIL.UnitHeight_Name;
                                resultItem.UnitHeightRatio = FindGIL.UnitHeightRatio;

                                resultItem.Height = (resultItem.Qty ?? 0) * FindGIL.UnitHeight;
                                resultItem.Height_Index = FindGIL.Height_Index;
                                resultItem.Height_Id = FindGIL.Height_Id;
                                resultItem.Height_Name = FindGIL.Height_Name;
                                resultItem.HeightRatio = FindGIL.HeightRatio;

                                resultItem.UnitVolume = FindGIL.UnitVolume;
                                resultItem.Volume = (resultItem.Qty ?? 0) * FindGIL.UnitVolume;

                                resultItem.UnitPrice = FindGIL.UnitPrice;
                                resultItem.UnitPrice_Index = FindGIL.UnitPrice_Index;
                                resultItem.UnitPrice_Id = FindGIL.UnitPrice_Id;
                                resultItem.UnitPrice_Name = FindGIL.UnitPrice_Name;
                                resultItem.Price = (resultItem.Qty ?? 0) * FindGIL.UnitPrice;
                                resultItem.Price_Index = FindGIL.Price_Index;
                                resultItem.Price_Id = FindGIL.Price_Id;
                                resultItem.Price_Name = FindGIL.Price_Name;

                                resultItem.DocumentRef_No1 = FindGIL.DocumentRef_No1;
                                resultItem.DocumentRef_No2 = FindGIL.DocumentRef_No2;
                                resultItem.DocumentRef_No3 = FindGIL.DocumentRef_No3;
                                resultItem.DocumentRef_No4 = FindGIL.DocumentRef_No4;
                                resultItem.DocumentRef_No5 = FindGIL.DocumentRef_No5;
                                resultItem.Document_Status = 0;
                                resultItem.UDF_1 = FindGIL.UDF_1;
                                resultItem.UDF_2 = FindGIL.UDF_2;
                                resultItem.UDF_3 = FindGIL.UDF_3;
                                resultItem.UDF_4 = FindGIL.UDF_2;
                                resultItem.UDF_5 = FindGIL.UDF_5;
                                resultItem.Ref_Process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                                resultItem.Ref_Document_Index = FindGIL.GoodsIssue_Index;
                                resultItem.Ref_Document_No = itemResult.goodsIssue_No;
                                resultItem.Ref_Document_LineNum = FindGIL.LineNum;
                                resultItem.Ref_DocumentItem_Index = FindGIL.GoodsIssueItemLocation_Index;
                                resultItem.PlanGoodsIssue_Index = FindGIL.Ref_Document_Index;
                                resultItem.PlanGoodsIssueItem_Index = FindGIL.Ref_DocumentItem_Index;
                                resultItem.PlanGoodsIssue_No = FindGIL.Ref_Document_No;
                                resultItem.Create_By = Create_By;
                                resultItem.Create_Date = DateTime.Now;
                                resultItem.BinBalance_Index = FindGIL.BinBalance_Index;

                                resultItem.Invoice_No = FindGIL.Invoice_No;
                                resultItem.Invoice_No_Out = FindGIL.Invoice_No_Out;
                                resultItem.Declaration_No = FindGIL.Declaration_No;
                                resultItem.Declaration_No_Out = FindGIL.Declaration_No_Out;
                                resultItem.HS_Code = FindGIL.HS_Code;
                                resultItem.Conutry_of_Origin = FindGIL.Conutry_of_Origin;
                                resultItem.Tax1 = FindGIL.Tax1;
                                resultItem.Tax1_Currency_Index = FindGIL.Tax1_Currency_Index;
                                resultItem.Tax1_Currency_Id = FindGIL.Tax1_Currency_Id;
                                resultItem.Tax1_Currency_Name = FindGIL.Tax1_Currency_Name;
                                resultItem.Tax2 = FindGIL.Tax2;
                                resultItem.Tax2_Currency_Index = FindGIL.Tax2_Currency_Index;
                                resultItem.Tax2_Currency_Id = FindGIL.Tax2_Currency_Id;
                                resultItem.Tax2_Currency_Name = FindGIL.Tax2_Currency_Name;
                                resultItem.Tax3 = FindGIL.Tax3;
                                resultItem.Tax3_Currency_Index = FindGIL.Tax3_Currency_Index;
                                resultItem.Tax3_Currency_Id = FindGIL.Tax3_Currency_Id;
                                resultItem.Tax3_Currency_Name = FindGIL.Tax3_Currency_Name;
                                resultItem.Tax4 = FindGIL.Tax4;
                                resultItem.Tax4_Currency_Index = FindGIL.Tax4_Currency_Index;
                                resultItem.Tax4_Currency_Id = FindGIL.Tax4_Currency_Id;
                                resultItem.Tax4_Currency_Name = FindGIL.Tax4_Currency_Name;
                                resultItem.Tax5 = FindGIL.Tax5;
                                resultItem.Tax5_Currency_Index = FindGIL.Tax5_Currency_Index;
                                resultItem.Tax5_Currency_Id = FindGIL.Tax5_Currency_Id;
                                resultItem.Tax5_Currency_Name = FindGIL.Tax5_Currency_Name;

                                db.IM_TaskItem.Add(resultItem);

                                CountQty = CountQty - GILQty;
                                GILQty = 0;
                                i = (int)Math.Ceiling(GILQty);

                            }

                            #endregion
                        }

                    }

                }


                else
                {
                    #region Create Task Header



                    var result = new im_Task();


                    var Gen = new List<GenDocumentTypeViewModel>();

                    var filterModel = new GenDocumentTypeViewModel();


                    filterModel.process_Index = new Guid("065A991E-77BD-4D28-83A7-0060ED68DE26");
                    filterModel.documentType_Index = new Guid("2F1985B9-E9E8-4059-9320-E07B4FB66E9D");
                    //GetConfig
                    Gen = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                    var genDoc = new AutoNumberService();
                    //string DocNo = "";
                    DateTime DocumentDate = DateTime.Now;
                    TaskNo = genDoc.genAutoDocmentNumber(Gen, DocumentDate);


                    result.Task_Index = Guid.NewGuid();
                    result.Task_No = TaskNo;
                    result.DocumentRef_No1 = pLocation_Name;
                    result.Document_Status = 1;
                    result.Create_By = Create_By;
                    result.Create_Date = DateTime.Now;

                    db.IM_Task.Add(result);

                    #endregion

                    #region Create TaskItem

                    var FindGIL = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssue_Index == Index && c.Document_Status == 0 && c.Location_Index == Guid.Parse(pLocation_Index)).ToList();

                    var TaskItem = new List<im_TaskItem>();


                    foreach (var listGIL in FindGIL)
                    {
                        var resultItem = new im_TaskItem();

                        resultItem.TaskItem_Index = Guid.NewGuid();
                        resultItem.Task_Index = result.Task_Index;
                        resultItem.Task_No = TaskNo;
                        resultItem.Tag_Index = listGIL.Tag_Index;
                        resultItem.TagItem_Index = listGIL.TagItem_Index;
                        resultItem.Tag_No = listGIL.Tag_No;
                        resultItem.Product_Index = listGIL.Product_Index;
                        resultItem.Product_Id = listGIL.Product_Id;
                        resultItem.Product_Name = listGIL.Product_Name;
                        resultItem.Product_SecondName = listGIL.Product_SecondName;
                        resultItem.Product_ThirdName = listGIL.Product_ThirdName;
                        resultItem.Product_Lot = listGIL.Product_Lot;
                        resultItem.ItemStatus_Index = listGIL.ItemStatus_Index;
                        resultItem.ItemStatus_Id = listGIL.ItemStatus_Id;
                        resultItem.ItemStatus_Name = listGIL.ItemStatus_Name;
                        resultItem.Location_Index = listGIL.Location_Index;
                        resultItem.Location_Id = listGIL.Location_Id;
                        resultItem.Location_Name = listGIL.Location_Name;
                        resultItem.Qty = listGIL.Qty;
                        resultItem.Ratio = listGIL.Ratio;
                        resultItem.TotalQty = listGIL.TotalQty;
                        resultItem.ProductConversion_Index = listGIL.ProductConversion_Index;
                        resultItem.ProductConversion_Id = listGIL.ProductConversion_Id;
                        resultItem.ProductConversion_Name = listGIL.ProductConversion_Name;
                        resultItem.MFG_Date = listGIL.MFG_Date;
                        resultItem.EXP_Date = listGIL.EXP_Date;

                        resultItem.UnitWeight = listGIL.UnitWeight;
                        resultItem.UnitWeight_Index = listGIL.UnitWeight_Index;
                        resultItem.UnitWeight_Id = listGIL.UnitWeight_Id;
                        resultItem.UnitWeight_Name = listGIL.UnitWeight_Name;
                        resultItem.UnitWeightRatio = listGIL.UnitWeightRatio;

                        resultItem.Weight = listGIL.Weight;
                        resultItem.Weight_Index = listGIL.Weight_Index;
                        resultItem.Weight_Id = listGIL.Weight_Id;
                        resultItem.Weight_Name = listGIL.Weight_Name;
                        resultItem.WeightRatio = listGIL.WeightRatio;

                        resultItem.UnitNetWeight = listGIL.UnitNetWeight;
                        resultItem.UnitNetWeight_Index = listGIL.UnitNetWeight_Index;
                        resultItem.UnitNetWeight_Id = listGIL.UnitNetWeight_Id;
                        resultItem.UnitNetWeight_Name = listGIL.UnitNetWeight_Name;
                        resultItem.UnitNetWeightRatio = listGIL.UnitNetWeightRatio;

                        resultItem.NetWeight = listGIL.NetWeight;
                        resultItem.NetWeight_Index = listGIL.NetWeight_Index;
                        resultItem.NetWeight_Id = listGIL.NetWeight_Id;
                        resultItem.NetWeight_Name = listGIL.NetWeight_Name;
                        resultItem.NetWeightRatio = listGIL.NetWeightRatio;

                        resultItem.UnitGrsWeight = listGIL.UnitGrsWeight;
                        resultItem.UnitGrsWeight_Index = listGIL.UnitGrsWeight_Index;
                        resultItem.UnitGrsWeight_Id = listGIL.UnitGrsWeight_Id;
                        resultItem.UnitGrsWeight_Name = listGIL.UnitGrsWeight_Name;
                        resultItem.UnitGrsWeightRatio = listGIL.UnitGrsWeightRatio;

                        resultItem.GrsWeight = listGIL.GrsWeight;
                        resultItem.GrsWeight_Index = listGIL.GrsWeight_Index;
                        resultItem.GrsWeight_Id = listGIL.GrsWeight_Id;
                        resultItem.GrsWeight_Name = listGIL.GrsWeight_Name;
                        resultItem.GrsWeightRatio = listGIL.GrsWeightRatio;

                        resultItem.UnitWidth = listGIL.UnitWidth;
                        resultItem.UnitWidth_Index = listGIL.UnitWidth_Index;
                        resultItem.UnitWidth_Id = listGIL.UnitWidth_Id;
                        resultItem.UnitWidth_Name = listGIL.UnitWidth_Name;
                        resultItem.UnitWidthRatio = listGIL.UnitWidthRatio;

                        resultItem.Width = listGIL.Width;
                        resultItem.Width_Index = listGIL.Width_Index;
                        resultItem.Width_Id = listGIL.Width_Id;
                        resultItem.Width_Name = listGIL.Width_Name;
                        resultItem.WidthRatio = listGIL.WidthRatio;

                        resultItem.UnitLength = listGIL.UnitLength;
                        resultItem.UnitLength_Index = listGIL.UnitLength_Index;
                        resultItem.UnitLength_Id = listGIL.UnitLength_Id;
                        resultItem.UnitLength_Name = listGIL.UnitLength_Name;
                        resultItem.UnitLengthRatio = listGIL.UnitLengthRatio;

                        resultItem.Length = listGIL.Length;
                        resultItem.Length_Index = listGIL.Length_Index;
                        resultItem.Length_Id = listGIL.Length_Id;
                        resultItem.Length_Name = listGIL.Length_Name;
                        resultItem.LengthRatio = listGIL.LengthRatio;

                        resultItem.UnitHeight = listGIL.UnitHeight;
                        resultItem.UnitHeight_Index = listGIL.UnitHeight_Index;
                        resultItem.UnitHeight_Id = listGIL.UnitHeight_Id;
                        resultItem.UnitHeight_Name = listGIL.UnitHeight_Name;
                        resultItem.UnitHeightRatio = listGIL.UnitHeightRatio;

                        resultItem.Height = listGIL.Height;
                        resultItem.Height_Index = listGIL.Height_Index;
                        resultItem.Height_Id = listGIL.Height_Id;
                        resultItem.Height_Name = listGIL.Height_Name;
                        resultItem.HeightRatio = listGIL.HeightRatio;

                        resultItem.UnitVolume = listGIL.UnitVolume;
                        resultItem.Volume = listGIL.Volume;

                        resultItem.UnitPrice = listGIL.UnitPrice;
                        resultItem.UnitPrice_Index = listGIL.UnitPrice_Index;
                        resultItem.UnitPrice_Id = listGIL.UnitPrice_Id;
                        resultItem.UnitPrice_Name = listGIL.UnitPrice_Name;
                        resultItem.Price = listGIL.Price;
                        resultItem.Price_Index = listGIL.Price_Index;
                        resultItem.Price_Id = listGIL.Price_Id;
                        resultItem.Price_Name = listGIL.Price_Name;

                        resultItem.DocumentRef_No1 = listGIL.DocumentRef_No1;
                        resultItem.DocumentRef_No2 = listGIL.DocumentRef_No2;
                        resultItem.DocumentRef_No3 = listGIL.DocumentRef_No3;
                        resultItem.DocumentRef_No4 = listGIL.DocumentRef_No4;
                        resultItem.DocumentRef_No5 = listGIL.DocumentRef_No5;
                        resultItem.Document_Status = 0;
                        resultItem.UDF_1 = listGIL.UDF_1;
                        resultItem.UDF_2 = listGIL.UDF_2;
                        resultItem.UDF_3 = listGIL.UDF_3;
                        resultItem.UDF_4 = listGIL.UDF_2;
                        resultItem.UDF_5 = listGIL.UDF_5;
                        resultItem.Ref_Process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                        resultItem.Ref_Document_Index = listGIL.GoodsIssue_Index;
                        resultItem.Ref_Document_No = listGIL.GoodsIssue_No;
                        resultItem.Ref_Document_LineNum = listGIL.LineNum;
                        resultItem.Ref_DocumentItem_Index = listGIL.GoodsIssueItemLocation_Index;
                        resultItem.PlanGoodsIssue_Index = listGIL.Ref_Document_Index;
                        resultItem.PlanGoodsIssueItem_Index = listGIL.Ref_DocumentItem_Index;
                        resultItem.PlanGoodsIssue_No = listGIL.Ref_Document_No;
                        resultItem.Create_By = Create_By;
                        resultItem.Create_Date = DateTime.Now;
                        resultItem.BinBalance_Index = listGIL.BinBalance_Index;

                        resultItem.Invoice_No = listGIL.Invoice_No;
                        resultItem.Invoice_No_Out = listGIL.Invoice_No_Out;
                        resultItem.Declaration_No = listGIL.Declaration_No;
                        resultItem.Declaration_No_Out = listGIL.Declaration_No_Out;
                        resultItem.HS_Code = listGIL.HS_Code;
                        resultItem.Conutry_of_Origin = listGIL.Conutry_of_Origin;
                        resultItem.Tax1 = listGIL.Tax1;
                        resultItem.Tax1_Currency_Index = listGIL.Tax1_Currency_Index;
                        resultItem.Tax1_Currency_Id = listGIL.Tax1_Currency_Id;
                        resultItem.Tax1_Currency_Name = listGIL.Tax1_Currency_Name;
                        resultItem.Tax2 = listGIL.Tax2;
                        resultItem.Tax2_Currency_Index = listGIL.Tax2_Currency_Index;
                        resultItem.Tax2_Currency_Id = listGIL.Tax2_Currency_Id;
                        resultItem.Tax2_Currency_Name = listGIL.Tax2_Currency_Name;
                        resultItem.Tax3 = listGIL.Tax3;
                        resultItem.Tax3_Currency_Index = listGIL.Tax3_Currency_Index;
                        resultItem.Tax3_Currency_Id = listGIL.Tax3_Currency_Id;
                        resultItem.Tax3_Currency_Name = listGIL.Tax3_Currency_Name;
                        resultItem.Tax4 = listGIL.Tax4;
                        resultItem.Tax4_Currency_Index = listGIL.Tax4_Currency_Index;
                        resultItem.Tax4_Currency_Id = listGIL.Tax4_Currency_Id;
                        resultItem.Tax4_Currency_Name = listGIL.Tax4_Currency_Name;
                        resultItem.Tax5 = listGIL.Tax5;
                        resultItem.Tax5_Currency_Index = listGIL.Tax5_Currency_Index;
                        resultItem.Tax5_Currency_Id = listGIL.Tax5_Currency_Id;
                        resultItem.Tax5_Currency_Name = listGIL.Tax5_Currency_Name;

                        db.IM_TaskItem.Add(resultItem);

                    }

                    #endregion

                }


                return "success";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region taskfilter
        public List<TaskfilterViewModel> taskfilter(TaskfilterViewModel model)
        {
            try
            {
                var query = db.View_Task.AsQueryable();

                var result = new List<TaskfilterViewModel>();


                if (model.listTaskViewModel.Count != 0)
                {

                    query = query.Where(c => model.listTaskViewModel.Select(s => s.goodsIssue_No).Contains(c.Ref_Document_No));

                    var queryresult = query.ToList();

                    foreach (var itemResult in queryresult)
                    {

                        var resultItem = new TaskfilterViewModel();

                        resultItem.task_Index = itemResult.Task_Index;
                        resultItem.goodsIssue_No = itemResult.Ref_Document_No;
                        resultItem.task_No = itemResult.Task_No;
                        resultItem.userAssign = itemResult.UserAssign;
                        resultItem.create_By = itemResult.Create_By;
                        resultItem.create_Date = itemResult.Create_Date.toString();
                        resultItem.create_Time = itemResult.Create_Date.ToString("HH:mm");
                        resultItem.assign_By = itemResult.Update_By;

                        result.Add(resultItem);

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.goodsIssue_No))
                    {
                        query = query.Where(c => c.Ref_Document_No.Contains(model.goodsIssue_No));
                    }
                    else
                    {
                        return result;
                    }

                    var queryresult = query.ToList();


                    foreach (var item in queryresult)
                    {
                        var resultItem = new TaskfilterViewModel();

                        resultItem.task_Index = item.Task_Index;
                        resultItem.goodsIssue_No = item.Ref_Document_No;
                        resultItem.task_No = item.Task_No;
                        resultItem.userAssign = item.UserAssign;
                        resultItem.create_By = item.Create_By;
                        resultItem.create_Date = item.Create_Date.toString();
                        resultItem.create_Time = item.Create_Date.ToString("HH:mm");
                        resultItem.assign_By = item.Update_By;


                        result.Add(resultItem);

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

        #region confirmTask
        public String confirmTask(TaskfilterViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                foreach (var item in data.listTaskViewModel)
                {
                    var Task = db.IM_Task.Find(item.task_Index);

                    if (Task != null)
                    {
                        if (Task.Document_Status == 0)
                        {
                            if (item.document_Status == "1")
                            {
                                Task.Document_Status = 1;
                                Task.Update_By = item.update_By;
                                Task.Update_Date = DateTime.Now;
                                Task.UserAssign = item.userAssign;
                                Task.Assign_By = item.update_By;
                            }
                            else if (item.document_Status == "2")
                            {
                                Task.Document_Status = 0;
                                Task.Update_By = item.update_By;
                                Task.Update_Date = DateTime.Now;
                            }
                        }

                    }

                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("confirmTask", msglog);
                    transaction.Rollback();
                    throw exy;

                }

                return "Done";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region taskPopup
        public List<GoodIssueViewModel> taskPopup(GoodIssueViewModel model)
        {
            try
            {
                var query = db.IM_GoodsIssue.AsQueryable();



                if (!string.IsNullOrEmpty(model.goodsIssue_No))
                {
                    query = query.Where(c => c.GoodsIssue_No.Contains(model.goodsIssue_No));
                }

                var Item = query.ToList();

                var result = new List<GoodIssueViewModel>();


                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");

                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());


                foreach (var item in Item)
                {
                    String Statue = "";
                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();


                    var resultItem = new GoodIssueViewModel();

                    resultItem.goodsIssue_Index = item.GoodsIssue_Index;
                    resultItem.goodsIssue_No = item.GoodsIssue_No;
                    resultItem.goodsIssue_Date = item.GoodsIssue_Date.toString();
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.document_Remark = item.Document_Remark;
                    resultItem.processStatus_Name = ProcessStatusName?.processStatus_Name;

                    result.Add(resultItem);

                }

                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region  autoGoodIssueNo
        public List<ItemListViewModel> autoGoodIssueNo(ItemListViewModel data)
        {
            try
            {
                var query = db.View_Task.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.Ref_Document_No.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.Ref_Document_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        name = item.Ref_Document_No
                    };
                    items.Add(resultItem);

                }



                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DropdownUser
        public List<UserViewModel> dropdownUser(UserViewModel data)
        {
            try
            {
                var result = new List<UserViewModel>();

                var filterModel = new ProcessStatusViewModel();

                //GetConfig
                result = utils.SendDataApi<List<UserViewModel>>(new AppSettingConfig().GetUrl("dropdownUser"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ReportPickingTag
        public string printReportPickingTag(ReportPickingTagViewModel data, string rootPath = "")
        {


            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var queryGI = db.View_RPT_Picking_Tag_GI.AsQueryable();
                var queryPlanGI = db.View_RPT_Picking_Tag_PlanGI.AsQueryable();
                var queryTask = db.View_RPT_Picking_Tag_TaskItem.AsQueryable();

                var result = new List<ReportPickingTagViewModel>();




                var queryRPT_GI = queryGI.ToList();
                var queryRPT_PlanGI = queryPlanGI.ToList();
                var queryRPT_Task = queryTask.ToList();

                queryRPT_Task = queryRPT_Task.Where(q => q.Task_Index == data.task_Index).ToList();

                var queryGI_Task = (from Task in queryRPT_Task
                                    join GI in queryRPT_GI on Task.Ref_DocumentItem_Index equals GI.GoodsIssueItemLocation_Index into ps
                                    from r in ps
                                    select new
                                    {
                                        task = Task,
                                        gi = r
                                    }).ToList();

                var queryGI_PlanGI = (from GI in queryGI_Task
                                      join PGI in queryRPT_PlanGI on GI.gi.Ref_Document_Index equals PGI.PlanGoodsIssue_Index into ps
                                      from r in ps
                                      select new
                                      {
                                          gi = GI,
                                          plangi = r
                                      }).ToList();

                var query = queryGI_PlanGI.OrderBy(o => o.plangi.PlanGoodsIssue_No).ThenBy(o => o.gi.gi.Location_Id).ToList();
                if (query.Count == 0)
                {
                    var resultItem = new ReportPickingTagViewModel();
                    result.Add(resultItem);
                }
                else
                {
                    foreach (var item in query)
                    {
                        string date = item.gi.gi.Document_Date.toString();
                        string Document_date = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                        System.Globalization.CultureInfo.InvariantCulture).ToString("ddd dd/MM/yyyy", culture);


                        var resultItem = new ReportPickingTagViewModel();
                        if (item.gi.gi == null)
                        {
                            resultItem.product_Id = null;
                            resultItem.product_Name = null;
                            resultItem.location_Id = null;
                            resultItem.goodsIssue_No = null;
                            resultItem.totalQty = null;
                        }
                        else
                        {
                            resultItem.product_Id = item.gi.gi.Product_Id;
                            resultItem.product_Name = item.gi.gi.Product_Name;
                            resultItem.location_Id = item.gi.gi.Location_Id;
                            //resultItem.task_No_Barcode = new NetBarcode.Barcode(item.gi.gi.Location_Id, NetBarcode.Type.Code128B).GetBase64Image();
                            decimal decimalTotalQty = item.gi.gi.TotalQty ?? 0;
                            resultItem.goodsIssue_No = item.gi.gi.GoodsIssue_No;
                            resultItem.productConversion_Name = item.gi.gi.ProductConversion_Name;
                            resultItem.totalQty = "(Total = " + (decimalTotalQty).ToString("#") + " " + item.gi.gi.ProductConversion_Name + ")";
                        }
                        if (item.plangi == null)
                        {
                            resultItem.shipTo_Address = null;
                            resultItem.ratio = null;
                            resultItem.planGoodsIssue_No = null;
                            resultItem.productConversion_Ratio = null;
                        }
                        else
                        {
                            resultItem.shipTo_Address = item.plangi.ShipTo_Address;
                            decimal decimalRatio = (item.gi.gi.TotalQty ?? 1) / (item.plangi.Ratio ?? 1);
                            resultItem.ratio = decimalRatio.ToString("#") + " " + item.plangi.ProductConversion_Name;
                            resultItem.planGoodsIssue_No = item.plangi.PlanGoodsIssue_No;
                            resultItem.task_No = item.gi.task.Task_No;
                            resultItem.planGoodsIssue_No_Barcode = new NetBarcode.Barcode(item.plangi.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                            resultItem.productConversion_Ratio = "1 " + item.plangi.ProductConversion_Name + " = " + (item.plangi.Ratio ?? 1).ToString("#") + " " + item.gi.gi.ProductConversion_Name; // ((item.gi.gi.TotalQty) / (item.plangi.Ratio)).ToString() + " " + item.gi.gi.ProductConversion_Name;
                        }

                        resultItem.document_Date = Document_date;
                        resultItem.task_No_Barcode = new NetBarcode.Barcode(item.gi.task.Task_No, NetBarcode.Type.Code128B).GetBase64Image();
                        resultItem.print_Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", culture);





                        result.Add(resultItem);
                    }
                    result.ToList();
                }
                rootPath = rootPath.Replace("\\GIAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportPickingTag");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region ReportPick
        public string printReportPick(ReportPickViewModel data, string rootPath = "")
        {

            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var queryGI = db.View_RPT_Pick_GI.AsQueryable();
                var queryTaskItem = db.View_RPT_Pick_TaskItem.AsQueryable();
                var result = new List<ReportPickViewModel>();

                var queryRPT_GI = queryGI.ToList();
                var queryRPT_TaskItem = queryTaskItem.ToList();



                queryRPT_TaskItem = queryRPT_TaskItem.Where(q => q.Task_Index == data.task_Index).ToList();


                var queryGI_TaskItem = (from TaskItem in queryRPT_TaskItem
                                        join GI in queryRPT_GI on TaskItem.Ref_DocumentItem_Index equals GI.GoodsIssueItemLocation_Index into ps
                                        from r in ps
                                        select new
                                        {
                                            taskitem = TaskItem,
                                            gi = r
                                        }).ToList();

                var ProductConversionBarcodeModel = new ProductConversionBarcodeViewModel();
                var resultProductConversionBarcode = new List<ProductConversionBarcodeViewModel>();
                resultProductConversionBarcode = utils.SendDataApi<List<ProductConversionBarcodeViewModel>>(new AppSettingConfig().GetUrl("ProductBarcode"), ProductConversionBarcodeModel.sJson());

                var query = queryGI_TaskItem.ToList();

                if (query.Count == 0)
                {
                    var resultItem = new ReportPickViewModel();
                    result.Add(resultItem);
                }
                else
                {
                    foreach (var item in query)
                    {

                        var pgi = db.IM_PlanGoodsIssue.Find(item.gi.Ref_Document_Index);
                        string date = item.gi.GoodsIssue_Date.toString();
                        string GoodsIssue_date = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                        System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                        var resultItem = new ReportPickViewModel();
                        if (item.gi == null)
                        {
                            resultItem.goodsIssue_No = null;
                            resultItem.goodsIssue_Date = null;
                            resultItem.document_Remark = null;
                            resultItem.owner_Name = null;
                        }
                        else
                        {
                            resultItem.goodsIssue_No = item.gi.GoodsIssue_No;
                            resultItem.goodsIssue_Date = GoodsIssue_date;
                            resultItem.document_Remark = item.gi.Document_Remark;
                            resultItem.owner_Name = item.gi.Owner_Name;
                        }


                        resultItem.task_No = item.taskitem.Task_No;
                        resultItem.assign_By = item.taskitem.Assign_By;
                        var pBarcode = resultProductConversionBarcode.Find(c => c.product_Id == item.taskitem.Product_Id && c.ref_No1 == "1");

                        if (pBarcode == null)
                        {
                            resultItem.productConversionBarcode = null;
                        }
                        else
                        {
                            var productbarcode = pBarcode.productConversionBarcode;
                            resultItem.productConversionBarcode = productbarcode;
                        }
                        resultItem.product_Id = pgi.ShipTo_Id.ToUpper() == "D2" ? utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), new { product_Index = item.taskitem.Product_Index }.sJson()).FirstOrDefault().Ref_No2 : item.taskitem.Product_Id;
                        resultItem.product_Name = item.taskitem.Product_Name;
                        resultItem.qty = item.taskitem.Qty;
                        resultItem.productConversion_Name = item.taskitem.ProductConversion_Name;
                        resultItem.location_Name = item.taskitem.Location_Name;
                        resultItem.owner_Name = item.gi.Owner_Name;



                        result.Add(resultItem);
                    }
                    result.ToList();
                }
                rootPath = rootPath.Replace("\\GIAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportPick"); ;
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region printListReportPickingTag
        public string printListReportPickingTag(ListReportPickingTagViewModel data, string rootPath = "")
        {


            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                //var resultProductConversion = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("ProductconversionFilter"), new { }.sJson());

                var result = new List<ReportPickingTagViewModel>();

                foreach (var items in data.ListReport)
                {

                    //var queryGI_PlanGI = (from TI in db.IM_TaskItem
                    //                      join PGI in db.View_RPT_Picking_Tag_PlanGI on TI.PlanGoodsIssue_Index equals PGI.PlanGoodsIssue_Index into ps
                    //                      from r in ps
                    //                      where TI.Task_Index == items.task_Index
                    //                      select new
                    //                      {
                    //                          ti = TI,
                    //                          plangi = r
                    //                      }).ToList();

                    var query = db.IM_TaskItem.Where(c => c.Task_Index == items.task_Index && c.Document_Status != -1).GroupBy(g => new
                    {
                        g.PlanGoodsIssue_Index,
                        g.PlanGoodsIssueItem_Index,
                        g.Ref_Document_Index,
                        g.Product_Index,
                        g.Product_Id,
                        g.Product_Name,
                        g.ProductConversion_Name,
                        g.Location_Id,
                        g.Task_No,
                        g.Ref_Document_No,
                        g.PlanGoodsIssue_No,
                    }).Select(s => new
                    {
                        s.Key.PlanGoodsIssue_Index,
                        s.Key.PlanGoodsIssueItem_Index,
                        s.Key.Ref_Document_Index,
                        s.Key.Product_Index,
                        s.Key.Product_Id,
                        s.Key.Product_Name,
                        s.Key.ProductConversion_Name,
                        s.Key.Location_Id,
                        s.Key.Task_No,
                        s.Key.Ref_Document_No,
                        s.Key.PlanGoodsIssue_No,
                        TotalQty = s.Sum(gs=> gs.TotalQty)
                    }
                    
                    ).ToList();
                    if (query.Count == 0)
                    {
                        var resultItem = new ReportPickingTagViewModel();
                        result.Add(resultItem);
                    }
                    else
                    {
                        foreach (var item in query)
                        {
                            var pgi = db.IM_PlanGoodsIssue.Find(item.PlanGoodsIssue_Index);
                            var pgii = db.IM_PlanGoodsIssueItem.Find(item.PlanGoodsIssueItem_Index);
                            Guid gi_index = (Guid)item.Ref_Document_Index;
                            string date = db.IM_GoodsIssue.FirstOrDefault(c => c.GoodsIssue_Index == gi_index).Document_Date.toString();
                            string Document_date = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                            System.Globalization.CultureInfo.InvariantCulture).ToString("ddd dd/MM/yyyy", culture);

                            var resultItem = new ReportPickingTagViewModel();

                            var QtyMod = resultItem.totalQty;

                            var resultPrdCon = new List<ProductConversionViewModelDoc>();

                            var filterModel = new ProductConversionViewModelDoc();

                            filterModel.product_Index = item.Product_Index.GetValueOrDefault();

                            //GetConfig
                            resultPrdCon = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), filterModel.sJson());
                            var RatioCAR = resultPrdCon.Where(c => c.productConversion_Name.ToString().ToUpper() == "CAR").FirstOrDefault();
                            if (RatioCAR == null)
                            {
                                RatioCAR = resultPrdCon.Where(c => c.productConversion_Name.ToString().ToUpper() == "EA").FirstOrDefault();
                            }
                            var QtyCAR = Math.Floor((item.TotalQty ?? 0) / (RatioCAR.productconversion_Ratio ?? 1));
                            var QtyBase = (item.TotalQty ?? 0) % RatioCAR.productconversion_Ratio;
                            string txtQtyCar = QtyCAR.ToString("#0") + ' ' + RatioCAR.productConversion_Name;
                            string txtQtyBase = (QtyBase ?? 0).ToString("#0") + ' ' + item.ProductConversion_Name;
                            string txtTotalQty = "( " + (item.TotalQty ?? 0).ToString("#0") + ' ' + item.ProductConversion_Name + " )";

                            resultItem.product_Id = pgi.ShipTo_Id.ToUpper() == "D2" ? utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), new { product_Index = item.Product_Index }.sJson()).FirstOrDefault().Ref_No2 : item.Product_Id;
                            resultItem.product_Name = item.Product_Name;
                            resultItem.location_Id = item.Location_Id;
                            //resultItem.task_No_Barcode = new NetBarcode.Barcode(item.gi.gi.Location_Id, NetBarcode.Type.Code128B).GetBase64Image();
                            decimal decimalTotalQty = item.TotalQty ?? 0;
                            resultItem.goodsIssue_No = item.Ref_Document_No;
                            resultItem.productConversion_Name = item.ProductConversion_Name;
                            resultItem.totalQty = txtTotalQty; //"(Total = " + (decimalTotalQty).ToString("#0.00") + " " + item.ProductConversion_Name + ")";


                            resultItem.shipTo_Address = pgi?.ShipTo_Name;
                            decimal decimalRatio = (item.TotalQty ?? 1) / (pgii?.Ratio ?? 1);
                            resultItem.ratio = txtQtyCar + Environment.NewLine + txtQtyBase;  //  decimalRatio.ToString("#0.00") + " " + pgii?.ProductConversion_Name;
                            resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                            resultItem.task_No = item.Task_No;
                            resultItem.planGoodsIssue_No_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                            //resultItem.productConversion_Ratio = "1 " + item.plangi?.ProductConversion_Name + " = " + (item.plangi?.Ratio ?? 1).ToString("#") + " " + item.ti.ProductConversion_Name; // ((item.gi.gi.TotalQty) / (item.plangi.Ratio)).ToString() + " " + item.gi.gi.ProductConversion_Name;
                            //resultItem.productConversion_Ratio = resultProductConversion.FirstOrDefault(c => c.product_Id == item.Product_Id).ref_No3;
                            resultItem.productConversion_Ratio = RatioCAR.ref_No3;

                            resultItem.document_Date = Document_date;
                            resultItem.task_No_Barcode = new NetBarcode.Barcode(item.Task_No, NetBarcode.Type.Code128B).GetBase64Image();
                            resultItem.print_Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", culture);


                            result.Add(resultItem);
                        }
                    }
                }

                var DataSet =  result.OrderBy(ol => ol.planGoodsIssue_No).ThenBy(ol => ol.location_Id).ToList();
                rootPath = rootPath.Replace("\\GIAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportPickingTagV2");

                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", DataSet);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region deleteTask
        public actionResultTask deleteTask(TaskfilterViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String msg = "";

            var actionResult = new actionResultTask();

            try
            {

                var task = db.View_Task.Where(c => c.Ref_Document_No == model.goodsIssue_No).ToList();

                foreach (var item in task)
                {
                    var taskItem = db.IM_TaskItem.Where(c => c.Task_Index == item.Task_Index && (c.Picking_Status == 1 || c.Picking_Status == 2)).ToList();

                    if (taskItem.Count > 0)
                    {
                        actionResult.msg = "ใบเบิกสินค้า " + model.goodsIssue_No + " มีการหยิบสินค้าแล้วไม่สามารถลบได้";
                    }
                    else
                    {

                        #region updateGI

                        var updateGI = db.IM_GoodsIssue.Find(item.Ref_Document_Index);

                        updateGI.Document_Status = 1;
                        updateGI.Update_By = model.userAssign;
                        updateGI.Update_Date = DateTime.Now;

                        #endregion

                        #region updateTask

                        var updateTask = db.IM_Task.Find(item.Task_Index);

                        updateTask.Document_Status = -1;
                        updateTask.Update_By = model.userAssign;
                        updateTask.Update_Date = DateTime.Now;

                        #endregion

                        #region updateTaskItem

                        var queryTaskItem = db.IM_TaskItem.Where(c => c.Task_Index == item.Task_Index).ToList();

                        foreach (var items in queryTaskItem)
                        {

                            var updateTaskItem = db.IM_TaskItem.Find(items.TaskItem_Index);

                            updateTaskItem.Document_Status = -1;
                            updateTaskItem.Update_By = model.userAssign;
                            updateTaskItem.Update_Date = DateTime.Now;
                        }

                        #endregion

                        actionResult.msg = "Delete Success";


                    }


                }
                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("deleteTask", msglog);
                    transaction.Rollback();
                    throw exy;

                }

                return actionResult;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
