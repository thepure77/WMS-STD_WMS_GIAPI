using AspNetCore.Reporting;
using BomBusiness;
using Business.Library;
using Comone.Utils;
using DataAccess;
using GIBusiness.AutoNumber;
using GIBusiness.GoodsIssue;
using GIBusiness.PlanGoodIssue;
using GIBusiness.Reports;
using GIBusiness.TagOut;
using GIDataAccess.Models;
using InterfaceBusiness;
using MasterBusiness.GoodsIssue;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using PlanGIBusiness.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static GIBusiness.GoodIssue.SearchDetailModel;
using static MasterDataBusiness.ViewModels.PopupGIRunWaveViewModel;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace GIBusiness.GoodIssue
{
    public class GoodIssue_ExportService
    {
        private GIDbContext db;
        private GIDbContext dblog;
        private BinbalanceDbContext dbBinbalance;
        private MasterDbContext dbMaster;

        public GoodIssue_ExportService()
        {
            db = new GIDbContext();
            dblog = new GIDbContext();
            dbBinbalance = new BinbalanceDbContext();
            dbMaster = new MasterDbContext();
        }
        public GoodIssue_ExportService(GIDbContext db, GIDbContext dblog , BinbalanceDbContext dbBinbalance , MasterDbContext dbMaster)
        {
            this.db = db;
            this.dblog = dblog;
            this.dbBinbalance = dbBinbalance;
            this.dbMaster = dbMaster;
        }
        
        bool chkdatawave = false;


        #region runwaveandHeader


        #region New log
        public actionResultRunWaveV2ViewModelViewModel runwaveandHeader(RunWaveFilterV2ViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            var process = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
            var PlanGiRunWave = new List<Guid>();
            var ListPlanGi_Index = new List<Guid>();
            var listpgiinotinsert = new List<plangoodsissueitemViewModel>();
            bool CheckRunwavePast = false;
            long IsPA = 1;
            Boolean IsTote = false;
            List<log_Waveprocress> log = new List<log_Waveprocress>();
            olog.logging("runwave", State);
            try
            {
                db.Database.SetCommandTimeout(360);
                var result = new actionResultRunWaveV2ViewModelViewModel();

                #region Get data Product and Location

                var location_onground = dbMaster.ms_Location.Where(c => c.LocationType_Index == Guid.Parse("BA0142A8-98B7-4E0B-A1CE-6266716F5F67")).Select(c=> c.Location_Index).ToList();


                var listDataProduct2 = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), new { }.sJson());
                var listDataLocation2 = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("getLocationMaster"), new { }.sJson());

                #endregion

                #region create and update header

                try
                {
                    Guid gi_index = !string.IsNullOrEmpty(model.goodsIssue_Index) ? new Guid(model.goodsIssue_Index) : new Guid("00000000-0000-0000-0000-000000000000");
                    var gi = db.IM_GoodsIssue.Find(gi_index);
                    if (gi == null)
                    {
                        var filterModel = new GenDocumentTypeViewModel();

                        filterModel.process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                        filterModel.documentType_Index = new Guid(model.documentType_Index);
                        //GetConfig
                        var dropDownDocumentType = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                        var genDoc = new AutoNumberService();
                        DateTime DocumentDate = DateTime.Now;
                        string DocNum = genDoc.genAutoDocmentNumber(dropDownDocumentType, DocumentDate);

                        var newGI = new im_GoodsIssue();
                        newGI.GoodsIssue_Index = Guid.NewGuid();
                        newGI.Owner_Index = !string.IsNullOrEmpty(model.owner_Index) ? new Guid(model.owner_Index) : Guid.Parse("00000000-0000-0000-0000-000000000000");
                        newGI.Owner_Id = !string.IsNullOrEmpty(model.owner_Index) ? model.owner_Id : "";
                        newGI.Owner_Name = !string.IsNullOrEmpty(model.owner_Index) ? model.owner_Name : "";
                        newGI.DocumentType_Index = new Guid(model.documentType_Index);
                        newGI.DocumentType_Id = model.documentType_Id;
                        newGI.DocumentType_Name = model.documentType_Name;
                        newGI.GoodsIssue_No = DocNum;
                        newGI.GoodsIssue_Date = model.goodsIssue_Date.toDateDefault();
                        newGI.GoodsIssue_Time = model.goodsIssue_Time;
                        newGI.DocumentRef_No1 = model.documentRef_No1;
                        newGI.DocumentRef_No2 = model.documentRef_No2;
                        newGI.DocumentRef_No3 = model.documentRef_No3;
                        newGI.DocumentRef_No4 = model.documentRef_No4;
                        newGI.DocumentRef_No5 = model.documentRef_No5;
                        newGI.Document_Remark = model.document_Remark;
                        newGI.DocumentPriority_Status = model.documentPriority_Status;
                        newGI.Document_Date = model.document_Date.toDate();
                        newGI.Warehouse_Index = !string.IsNullOrEmpty(model.warehouse_Index) ? new Guid(model.warehouse_Index) : (Guid?)null;
                        newGI.Warehouse_Id = !string.IsNullOrEmpty(model.warehouse_Index) ? model.warehouse_Id : null;
                        newGI.Warehouse_Name = !string.IsNullOrEmpty(model.warehouse_Index) ? model.warehouse_Name : null;
                        newGI.Document_Status = -2;
                        newGI.Create_By = model.create_by;
                        newGI.Create_Date = DateTime.Now;
                        db.IM_GoodsIssue.Add(newGI);



                        model.goodsIssue_Index = newGI.GoodsIssue_Index.ToString();
                        model.goodsIssue_No = newGI.GoodsIssue_No;

                        var Gi_index = new SqlParameter("@GoodsIssue_Index", model.goodsIssue_Index);
                        var Gi_no = new SqlParameter("@GoodsIssue_No", model.goodsIssue_No);
                        var Step = new SqlParameter("@step", "create and update header");
                        var json = new SqlParameter("@json", "Create");
                        var User = new SqlParameter("@User", model.create_by);
                        var resultx = db.Database.ExecuteSqlCommand("EXEC sp_Insert_Process_wave @GoodsIssue_Index ,@GoodsIssue_No ,@step ,@json ,@User", Gi_index, Gi_no, Step, json, User);
                        
                    }
                    else
                    {
                        if (gi.Document_Status == 0)
                        {
                            gi.Owner_Index = !string.IsNullOrEmpty(model.owner_Index) ? new Guid(model.owner_Index) : Guid.Parse("00000000-0000-0000-0000-000000000000");
                            gi.Owner_Id = !string.IsNullOrEmpty(model.owner_Index) ? model.owner_Id : "";
                            gi.Owner_Name = !string.IsNullOrEmpty(model.owner_Index) ? model.owner_Name : "";
                            gi.DocumentType_Index = new Guid(model.documentType_Index);
                            gi.DocumentType_Id = model.documentType_Id;
                            gi.DocumentType_Name = model.documentType_Name;
                            gi.GoodsIssue_Date = model.goodsIssue_Date.toDateDefault();
                            gi.GoodsIssue_Time = model.goodsIssue_Time;
                            gi.DocumentRef_No1 = model.documentRef_No1;
                            gi.Document_Remark = model.document_Remark;
                            gi.Document_Date = model.document_Date.toDate();
                            gi.Warehouse_Index = !string.IsNullOrEmpty(model.warehouse_Index) ? new Guid(model.warehouse_Index) : (Guid?)null;
                            gi.Warehouse_Id = !string.IsNullOrEmpty(model.warehouse_Index) ? model.warehouse_Id : null;
                            gi.Warehouse_Name = !string.IsNullOrEmpty(model.warehouse_Index) ? model.warehouse_Name : null;
                            gi.Update_By = model.create_by;
                            gi.Update_Date = DateTime.Now;


                        }

                        model.goodsIssue_Index = gi.GoodsIssue_Index.ToString();
                        model.goodsIssue_No = gi.GoodsIssue_No;

                        var Gi_index = new SqlParameter("@GoodsIssue_Index", model.goodsIssue_Index);
                        var Gi_no = new SqlParameter("@GoodsIssue_No", model.goodsIssue_No);
                        var Step = new SqlParameter("@step", "create and update header");
                        var json = new SqlParameter("@json", "Update");
                        var User = new SqlParameter("@User", model.create_by);
                        var resultx = db.Database.ExecuteSqlCommand("EXEC sp_Insert_Process_wave @GoodsIssue_Index ,@GoodsIssue_No ,@step ,@json ,@User", Gi_index, Gi_no, Step, json, User);
                        
                    }

                    var transactionx = db.Database.BeginTransaction();
                    try
                    {

                        db.SaveChanges();
                        transactionx.Commit();


                    }

                    catch (Exception exy)
                    {

                      
                        transactionx.Rollback();

                        var Gi_index = new SqlParameter("@GoodsIssue_Index", model.goodsIssue_Index);
                        var Gi_no = new SqlParameter("@GoodsIssue_No", model.goodsIssue_No);
                        var Step = new SqlParameter("@step", "Error : create and update header EXY");
                        var json = new SqlParameter("@json", exy.Message);
                        var User = new SqlParameter("@User", model.create_by);
                        var resultx = db.Database.ExecuteSqlCommand("EXEC sp_Insert_Process_wave @GoodsIssue_Index ,@GoodsIssue_No ,@step ,@json ,@User", Gi_index, Gi_no, Step, json, User);

                        throw exy;



                    }
                }
                catch (Exception exyz)
                {
                    var Gi_index = new SqlParameter("@GoodsIssue_Index", model.goodsIssue_Index);
                    var Gi_no = new SqlParameter("@GoodsIssue_No", model.goodsIssue_No);
                    var Step = new SqlParameter("@step", "Error : create and update header EXYZ");
                    var json = new SqlParameter("@json", exyz.Message);
                    var User = new SqlParameter("@User", model.create_by);
                    var resultx = db.Database.ExecuteSqlCommand("EXEC sp_Insert_Process_wave @GoodsIssue_Index ,@GoodsIssue_No ,@step ,@json ,@User", Gi_index, Gi_no, Step, json, User);

                    throw exyz;
                }

                #endregion

                #region runwave status 10
                

                if (model.listGoodsIssueItemViewModel.Count > 0)
                {
                    foreach (var item in model.listGoodsIssueItemViewModel)
                    {
                        if (item.planGoodsIssueItem_Index != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        {
                            PlanGiRunWave.Add(item.planGoodsIssueItem_Index);
                            ListPlanGi_Index.Add(item.planGoodsIssue_Index);
                        }
                    }
                }

                using (var db2 = new GIDbContext())
                {
                    db2.Database.SetCommandTimeout(120);
                    var transaction = db2.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {


                        var pgi = db2.IM_PlanGoodsIssueItem.Where(c => PlanGiRunWave.Contains(c.PlanGoodsIssueItem_Index) && c.Document_Status == 0).ToList();
                        foreach (var p in pgi)
                        {
                            p.Document_Status = 1;
                        }
                        var GI = db2.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && (c.RunWave_Status == null || c.RunWave_Status == 0)).ToList();
                        foreach (var g in GI)
                        {
                            g.Wave_Index = new Guid(model.wave_Index);
                            g.RunWave_Status = 10;
                        }
                        db2.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {

                      

                        transaction.Rollback();
                        throw exy;
                    }
                }

                #endregion

                #region planGIResultx

                var planGIResultx = db.View_PLANWAVEV.Where(c => PlanGiRunWave.Contains(c.PlanGoodsIssueItem_Index))
                    .GroupBy(g => new
                    {
                        g.Product_Index,
                        g.Product_Id,
                        g.Product_Name,
                        g.Product_SecondName,
                        g.Product_ThirdName,
                        g.Product_Lot,
                        g.ItemStatus_Index,
                        g.ItemStatus_Id,
                        g.ItemStatus_Name,
                        g.MFG_Date,
                        g.EXP_Date,
                        g.DocumentRef_No1,
                        g.DocumentRef_No2,
                        g.DocumentRef_No3,
                        g.DocumentRef_No4,
                        g.DocumentRef_No5,
                        g.UDF_1,
                        g.UDF_2,
                        g.UDF_3,
                        g.UDF_4,
                        g.UDF_5,
                        g.Owner_Index,
                        g.PlanGoodsIssue_UDF_1,
                        g.PlanGoodsIssue_UDF_2,
                        g.PlanGoodsIssue_UDF_3,
                        g.PlanGoodsIssue_UDF_4,
                        g.PlanGoodsIssue_UDF_5,
                    })
                    .Select(s => new
                    {
                        s.Key.Product_Index,
                        s.Key.Product_Id,
                        s.Key.Product_Name,
                        s.Key.Product_SecondName,
                        s.Key.Product_ThirdName,
                        s.Key.Product_Lot,
                        s.Key.ItemStatus_Index,
                        s.Key.ItemStatus_Id,
                        s.Key.ItemStatus_Name,
                        s.Key.MFG_Date,
                        s.Key.EXP_Date,
                        s.Key.DocumentRef_No1,
                        s.Key.DocumentRef_No2,
                        s.Key.DocumentRef_No3,
                        s.Key.DocumentRef_No4,
                        s.Key.DocumentRef_No5,
                        s.Key.UDF_1,
                        s.Key.UDF_2,
                        s.Key.UDF_3,
                        s.Key.UDF_4,
                        s.Key.UDF_5,
                        total = s.Sum(x => x.TotalQty),
                        GITotalQty = s.Sum(x => x.GITotalQty),
                        QtyWave = s.Sum(x => x.TotalQty) - s.Sum(x => x.GITotalQty),
                        s.Key.Owner_Index,
                        s.Key.PlanGoodsIssue_UDF_1,
                        s.Key.PlanGoodsIssue_UDF_2,
                        s.Key.PlanGoodsIssue_UDF_3,
                        s.Key.PlanGoodsIssue_UDF_4,
                        s.Key.PlanGoodsIssue_UDF_5,
                    }).ToList();

                #endregion

                #region Check  View_PLANWAVEV

                State = "View_PLANWAVEV";
                olog.logging("runwave", State);
                if (planGIResultx.Count == 0)
                {
                    throw new Exception("Plan GI not found..");
                }

                #endregion

                #region Get WaveRule and ViewWaveTemplate

                var jsGetWaveRule = new { process_Index = process, wave_Index = model.wave_Index };
                var getWaveRule = utils.SendDataApi<List<WaveRuleViewModel>>(new AppSettingConfig().GetUrl("getWaveRule"), jsGetWaveRule.sJson());

                if (getWaveRule.Count == 0)
                {
                    throw new Exception("Wave Template not found.");
                }

                var getViewWaveTemplateEX = utils.SendDataApi<List<WaveTemplateViewModel>>(new AppSettingConfig().GetUrl("getViewWaveTemplate"), new { }.sJson());

                #endregion

                State = "getWaveRule";
                olog.logging("runwave", State);
                bool isUseAttribute = false;

                #region for getWaveRule >>>>  A_LOOP

                foreach (var waveRule in getWaveRule.OrderBy(o => o.waveRule_Seq))
                {
                   

                    var jsgetViewWaveTemplate = new { process_Index = process, wave_Index = model.wave_Index, rule_Index = waveRule.rule_Index };
                    var getViewWaveTemplate = getViewWaveTemplateEX.Where(c => c.process_Index == process.ToString() && c.wave_Index == model.wave_Index && c.rule_Index == waveRule.rule_Index);
                    State = "getViewWaveTemplate";
                    olog.logging("runwave", State);
                    var planGIWaveResult = db.View_PLANWAVEbyPLANGIV2.AsQueryable();
                    var check = planGIWaveResult.ToList();
                    planGIWaveResult = planGIWaveResult.Where(c => PlanGiRunWave.Contains(c.PlanGoodsIssueItem_Index));

                    var planGIWaveResult2 = planGIWaveResult.ToList();
                    State = "View_PLANWAVEbyPLANGIV2";
                    olog.logging("runwave", State);

                    #region for planGIResultx >>>> D_LOOP
                    foreach (var item in planGIResultx)
                    {
                        olog.logging("runwave", "planGIResultx : " + item.Product_Id.ToString());

                        var planGIWaveResult3 = planGIWaveResult2.AsQueryable();
                        var strwhere = new getViewBinbalanceViewModel();

                        #region query Plag Gi
                        if (item.Owner_Index.ToString() != "")
                        {
                            strwhere.Owner_Index = item.Owner_Index;
                            planGIWaveResult3 = planGIWaveResult3.Where(c => c.Owner_Index == item.Owner_Index);
                        }
                        if (item.Product_Index.ToString() != "")
                        {
                            strwhere.Product_Index = item.Product_Index;
                            planGIWaveResult3 = planGIWaveResult3.Where(c => c.Product_Index == item.Product_Index);
                        }

                        if (item.ItemStatus_Index.ToString() != "")
                        {
                            strwhere.ItemStatus_Index = item.ItemStatus_Index;
                            planGIWaveResult3 = planGIWaveResult3.Where(c => c.ItemStatus_Index == item.ItemStatus_Index);
                        }

                        if (item.Product_Lot != null)
                        {
                            if (item.Product_Lot.ToString() != "")
                            {
                                strwhere.Product_Lot = item.Product_Lot;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.Product_Lot == item.Product_Lot);
                            }
                        }

                        if (isUseAttribute == true)
                        {
                            strwhere.isUseAttribute = isUseAttribute;
                            if (item.UDF_1 != null)
                            {
                                strwhere.UDF_1 = item.UDF_1;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_1 == item.UDF_1);
                            }

                            if (item.UDF_2 != null)
                            {
                                strwhere.UDF_2 = item.UDF_2;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_2 == item.UDF_2);
                            }

                            if (item.UDF_3 != null)
                            {
                                strwhere.UDF_3 = item.UDF_3;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_3 == item.UDF_3);
                            }

                            if (item.UDF_4 != null)
                            {
                                strwhere.UDF_4 = item.UDF_4;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_4 == item.UDF_4);
                            }

                            if (item.UDF_5 != null)
                            {
                                strwhere.UDF_5 = item.UDF_5;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_5 == item.UDF_5);
                            }
                        }
                        #endregion

                        if (planGIWaveResult3.OrderBy(c => c.LineNum).ToList().Count < 1)
                        {
                            continue;
                        }

                        #region Isuse
                        strwhere.isuse = model.goodsIssue_Index;
                        var Isuse = new SqlParameter("@Isuse", strwhere.isuse);
                        var product_index = new SqlParameter("@product_index", strwhere.Product_Index);
                        var itemstatus_index = new SqlParameter("@itemstatus_index", strwhere.ItemStatus_Index);
                        var owner_index = new SqlParameter("@owner_index", strwhere.Owner_Index);
                        var resultx = db.Database.ExecuteSqlCommand("EXEC sp_Update_isuse @Isuse ,@product_index ,@itemstatus_index ,@owner_index ", Isuse, product_index, itemstatus_index, owner_index);
                        #endregion

                        State = "planGIWaveResult3";
                        olog.logging("runwave", State);
                        strwhere.isuse = model.goodsIssue_Index;
                        strwhere.isActive = true;

                        decimal? QtyPlanGIRemian = 0;

                        #region for planGIWaveResult3 >>>> E_LOOP

                        var planwith_export = planGIWaveResult3.Where(c => !string.IsNullOrEmpty(c.EXPORT_FLAG)).ToList();

                        if (planwith_export.Count > 0)
                        {

                        }
                        var planwith_normal = planGIWaveResult3.Where(c => string.IsNullOrEmpty(c.EXPORT_FLAG)).ToList();

                        #region Exprot

                        foreach (var itemPlanGI in planwith_export.OrderBy(c => c.LineNum))
                        {

                            if (itemPlanGI.ModPlanGI == 0)
                            {
                                IsPA = 0;

                            }
                            else
                            {
                                IsPA = 1;
                            }

                            State = "listDataProduct2.ToList()";
                            olog.logging("runwave", State);
                            var listProducttote = listDataProduct2.Where(c => c.product_Index == itemPlanGI.Product_Index).ToList();
                            if (listProducttote != null)
                            {
                                var checkProduct = listProducttote.Where(c => c.product_Index == itemPlanGI.Product_Index).FirstOrDefault();
                                if (checkProduct == null)
                                {
                                    continue;
                                }

                                if (checkProduct.Ref_No1 == "carton flow rack")
                                {
                                    IsTote = true;

                                }
                                else
                                {
                                    IsTote = false;

                                }

                            }
                            else
                            {
                                IsTote = false;
                            }


                            var ListGoodsIssueItemLocation = new List<im_GoodsIssueItemLocation>();
                            if (itemPlanGI.Product_Id == "1004492")
                            {
                                var lnum = itemPlanGI.LineNum;

                            }

                            QtyPlanGIRemian = itemPlanGI.TotalQty - itemPlanGI.GITotalQty;
                            if (QtyPlanGIRemian <= 0)
                            {
                                break;
                            }

                            #region view_waveBinbalance2
                            State = "getViewBinbalanceapi";
                            olog.logging("runwave", State);
                            State = "View_WaveCheckProductLot";
                            olog.logging("runwave", State);

                            var ListLot = new List<String>();
                            var ListLotNotWave = new List<String>();
                            var listAll_Lot = db.View_WaveCheckProductLot.Where(c => c.Product_Index == itemPlanGI.Product_Index).ToList();

                            var listLot_In_Product = listAll_Lot.Where(c => c.PlanGoodsIssue_Index == itemPlanGI.PlanGoodsIssue_Index && c.Product_Index == itemPlanGI.Product_Index).ToList();

                            if (listLot_In_Product.Count > 0)
                            {
                                foreach (var itemlot in listLot_In_Product)
                                {
                                    ListLot.Add(item.Product_Lot);
                                }
                            }

                            var listLot_NotIN_Wave = listAll_Lot.Where(c => !ListLot.Contains(c.Product_Lot)).ToList();


                            if (listLot_NotIN_Wave.Count > 0)
                            {
                                foreach (var itemlot in listLot_NotIN_Wave)
                                {
                                    ListLotNotWave.Add(itemlot.Product_Lot);
                                }
                            }

                            State = "getView_WaveBinBalance2";
                            olog.logging("runwave", State);
                            var GIDate = model.goodsIssue_Date.toDate();

                            var GoodsIssue_Index = new SqlParameter("@GoodsIssue_Index", strwhere.isuse);
                            var Owner_Index = new SqlParameter("@Owner_Index", strwhere.Owner_Index.ToString());
                            var Product_Index = new SqlParameter("@Product_Index", strwhere.Product_Index.ToString());
                            var Product_Lot = new SqlParameter("@Product_Lot", strwhere.Product_Lot == null ? "" : strwhere.Product_Lot);
                            var ItemStatus_Index = new SqlParameter("@ItemStatus_Index", strwhere.ItemStatus_Index.ToString());
                            List<View_WaveBinBalanceViewModel_Ace> View_WaveBinBalance2 = new List<View_WaveBinBalanceViewModel_Ace>();
                            View_WaveBinBalance2 = db.View_WaveBinBalanceViewModel_Ace.FromSql("EXEC sp_WaveBinBalance_Export @GoodsIssue_Index ,@Owner_Index ,@Product_Index ,@Product_Lot ,@ItemStatus_Index", GoodsIssue_Index, Owner_Index, Product_Index, Product_Lot, ItemStatus_Index).ToList();


                            State = "View_WaveBinBalance2 EXEC";
                            olog.logging("runwave", State);

                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c =>
                                (c.goodsReceive_EXP_Date != null ? c.goodsReceive_EXP_Date.sParse<DateTime>().Subtract(DateTime.Now.AddDays(-1)).Days : 1) > (c.productShelfLife_D ?? 0) &&
                                !(ListLotNotWave.Contains(c.product_Lot)) &&
                                 (c.binBalance_QtyBal) > 0 &&
                                 (c.binBalance_QtyReserve) >= 0 &&
                                c.goodsReceive_Date <= GIDate.Value.Date &&
                                (string.IsNullOrEmpty(itemPlanGI.ERP_Location) ? (c.erp_Location ?? "") == "" : c.erp_Location == itemPlanGI.ERP_Location)
                            ).ToList();

                            State = "View_WaveBinBalance2 EXEC S";




                            #endregion

                            State = "View_WaveBinBalance2";
                            olog.logging("runwave", State);
                            var BinBalanceResult = View_WaveBinBalance2.ToList();

                            State = "View_WaveBinBalance2.ToList";
                            olog.logging("runwave", State);

                            var itemBinSort = new List<View_WaveBinBalanceViewModel_Ace>();


                            if (IsTote == true)
                            {
                                //itemBinSort = BinBalanceResult.Where(c=> location_onground.Contains(Guid.Parse(c.location_Index))).OrderByDescending(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name).ToList();
                                itemBinSort = BinBalanceResult.OrderByDescending(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(c => c.picking_Seq).ThenBy(f => f.location_Name).ToList();

                            }
                            else
                            {
                                itemBinSort = BinBalanceResult.OrderByDescending(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(c => c.picking_Seq).ThenBy(f => f.location_Name).ToList();

                            }

                            //if (IsPA == 1 && IsTote == true)
                            //{
                            //    itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenBy(f => f.location_Bay).ThenByDescending(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name).ToList();

                            //}
                            //else if (IsPA == 0 && IsTote == true)
                            //{
                            //    itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenByDescending(f => f.location_Bay).ThenByDescending(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name).ToList();
                            //}
                            //else
                            //{
                            //    itemBinSort = BinBalanceResult.OrderByDescending(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(c => c.picking_Seq).ThenBy(f => f.location_Name).ToList();

                            //}

                            int icountloop = 0;

                            #region for itemBinSort >>>> H_LOOP

                            foreach (var itemBin in itemBinSort)
                            {
                                icountloop = icountloop + 1;

                                olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [" + icountloop.ToString() + "]  Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);

                                decimal? QtyBal = itemBin.binBalance_QtyBal - itemBin.binBalance_QtyReserve;


                                olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " :  QtyBal " + QtyBal.ToString() + " =  binBalance_QtyBal : " + itemBin.binBalance_QtyBal.ToString() + "  -  binBalance_QtyReserve  : " + itemBin.binBalance_QtyReserve.ToString());


                                if (QtyPlanGIRemian <= 0)
                                {
                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPlanGIRemian <= 0 ]  " + QtyPlanGIRemian.ToString());

                                    break;
                                }
                                if (QtyBal <= 0)
                                {
                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyBal <= 0 ]  " + QtyBal.ToString());
                                    continue;
                                }


                                if (QtyPlanGIRemian >= QtyBal && QtyBal > 0)
                                {
                                    State = "QtyPlanGIRemian >= QtyBal && QtyBal > 0";
                                    olog.logging("runwave", State + " TAG_NO " + itemBin.tag_No + " Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPlanGIRemian >= QtyBal && QtyBal > 0]   QtyPlanGIRemian " + QtyPlanGIRemian.ToString() + "    QtyBal : " + QtyBal.ToString());

                                    var GoodsIssueItemLocation = new im_GoodsIssueItemLocation();
                                    GoodsIssueItemLocation.GoodsIssueItemLocation_Index = Guid.NewGuid();
                                    GoodsIssueItemLocation.GoodsIssue_Index = new Guid(model.goodsIssue_Index);
                                    GoodsIssueItemLocation.TagItem_Index = new Guid(itemBin.tagItem_Index);
                                    GoodsIssueItemLocation.Tag_Index = new Guid(itemBin.tag_Index);
                                    GoodsIssueItemLocation.Tag_No = itemBin.tag_No;
                                    GoodsIssueItemLocation.Product_Index = new Guid(itemBin.product_Index);
                                    GoodsIssueItemLocation.Product_Id = itemBin.product_Id;
                                    GoodsIssueItemLocation.Product_Name = itemBin.product_Name;
                                    GoodsIssueItemLocation.Product_SecondName = itemBin.product_SecondName;
                                    GoodsIssueItemLocation.Product_ThirdName = itemBin.product_ThirdName;
                                    GoodsIssueItemLocation.Product_Lot = itemBin.product_Lot;
                                    GoodsIssueItemLocation.ItemStatus_Index = new Guid(itemBin.itemStatus_Index);
                                    GoodsIssueItemLocation.ItemStatus_Id = itemBin.itemStatus_Id;
                                    GoodsIssueItemLocation.ItemStatus_Name = itemBin.itemStatus_Name;
                                    GoodsIssueItemLocation.Location_Index = new Guid(itemBin.location_Index);
                                    GoodsIssueItemLocation.Location_Id = itemBin.location_Id;
                                    GoodsIssueItemLocation.Location_Name = itemBin.location_Name;
                                    GoodsIssueItemLocation.QtyPlan = (Decimal)itemPlanGI.TotalQtyRemian;
                                    GoodsIssueItemLocation.Qty = (Decimal)QtyBal / (Decimal)itemPlanGI.Ratio;
                                    GoodsIssueItemLocation.Ratio = (Decimal)itemPlanGI.Ratio;
                                    GoodsIssueItemLocation.TotalQty = (Decimal)QtyBal;
                                    GoodsIssueItemLocation.ProductConversion_Index = (Guid)itemPlanGI.ProductConversion_Index;
                                    GoodsIssueItemLocation.ProductConversion_Id = itemPlanGI.ProductConversion_Id;
                                    GoodsIssueItemLocation.ProductConversion_Name = itemPlanGI.ProductConversion_Name;
                                    GoodsIssueItemLocation.MFG_Date = !string.IsNullOrEmpty(itemBin.goodsReceive_MFG_Date) ? itemBin.goodsReceive_MFG_Date.toDate() : null;
                                    GoodsIssueItemLocation.EXP_Date = !string.IsNullOrEmpty(itemBin.goodsReceive_EXP_Date) ? itemBin.goodsReceive_EXP_Date.toDate() : null;

                                    GoodsIssueItemLocation.UnitWeight = itemBin.binBalance_UnitWeightBal;
                                    GoodsIssueItemLocation.UnitWeight_Index = itemBin.binBalance_UnitWeightBal_Index;
                                    GoodsIssueItemLocation.UnitWeight_Id = itemBin.binBalance_UnitWeightBal_Id;
                                    GoodsIssueItemLocation.UnitWeight_Name = itemBin.binBalance_UnitWeightBal_Name;
                                    GoodsIssueItemLocation.UnitWeightRatio = itemBin.binBalance_UnitWeightBalRatio;

                                    GoodsIssueItemLocation.Weight = (itemBin.binBalance_WeightBal ?? 0) - (itemBin.binBalance_WeightReserve ?? 0);
                                    GoodsIssueItemLocation.Weight_Index = itemBin.binBalance_WeightBal_Index;
                                    GoodsIssueItemLocation.Weight_Id = itemBin.binBalance_WeightBal_Id;
                                    GoodsIssueItemLocation.Weight_Name = itemBin.binBalance_WeightBal_Name;
                                    GoodsIssueItemLocation.WeightRatio = itemBin.binBalance_WeightBalRatio;

                                    GoodsIssueItemLocation.UnitNetWeight = itemBin.binBalance_UnitNetWeightBal;
                                    GoodsIssueItemLocation.UnitNetWeight_Index = itemBin.binBalance_UnitNetWeightBal_Index;
                                    GoodsIssueItemLocation.UnitNetWeight_Id = itemBin.binBalance_UnitNetWeightBal_Id;
                                    GoodsIssueItemLocation.UnitNetWeight_Name = itemBin.binBalance_UnitNetWeightBal_Name;
                                    GoodsIssueItemLocation.UnitNetWeightRatio = itemBin.binBalance_UnitNetWeightBalRatio;

                                    GoodsIssueItemLocation.NetWeight = (itemBin.binBalance_NetWeightBal ?? 0) - (itemBin.binBalance_NetWeightReserve ?? 0);
                                    GoodsIssueItemLocation.NetWeight_Index = itemBin.binBalance_NetWeightBal_Index;
                                    GoodsIssueItemLocation.NetWeight_Id = itemBin.binBalance_NetWeightBal_Id;
                                    GoodsIssueItemLocation.NetWeight_Name = itemBin.binBalance_NetWeightBal_Name;
                                    GoodsIssueItemLocation.NetWeightRatio = itemBin.binBalance_NetWeightBalRatio;

                                    GoodsIssueItemLocation.UnitGrsWeight = itemBin.binBalance_UnitGrsWeightBal;
                                    GoodsIssueItemLocation.UnitGrsWeight_Index = itemBin.binBalance_UnitGrsWeightBal_Index;
                                    GoodsIssueItemLocation.UnitGrsWeight_Id = itemBin.binBalance_UnitGrsWeightBal_Id;
                                    GoodsIssueItemLocation.UnitGrsWeight_Name = itemBin.binBalance_UnitGrsWeightBal_Name;
                                    GoodsIssueItemLocation.UnitGrsWeightRatio = itemBin.binBalance_UnitGrsWeightBalRatio;

                                    GoodsIssueItemLocation.GrsWeight = (itemBin.binBalance_GrsWeightBal ?? 0) - (itemBin.binBalance_GrsWeightReserve ?? 0);
                                    GoodsIssueItemLocation.GrsWeight_Index = itemBin.binBalance_GrsWeightBal_Index;
                                    GoodsIssueItemLocation.GrsWeight_Id = itemBin.binBalance_GrsWeightBal_Id;
                                    GoodsIssueItemLocation.GrsWeight_Name = itemBin.binBalance_GrsWeightBal_Name;
                                    GoodsIssueItemLocation.GrsWeightRatio = itemBin.binBalance_GrsWeightBalRatio;

                                    GoodsIssueItemLocation.UnitWidth = (itemBin.binBalance_UnitWidthBal ?? 0);
                                    GoodsIssueItemLocation.UnitWidth_Index = itemBin.binBalance_UnitWidthBal_Index;
                                    GoodsIssueItemLocation.UnitWidth_Id = itemBin.binBalance_UnitWidthBal_Id;
                                    GoodsIssueItemLocation.UnitWidth_Name = itemBin.binBalance_UnitWidthBal_Name;
                                    GoodsIssueItemLocation.UnitWidthRatio = itemBin.binBalance_UnitWidthBalRatio;

                                    GoodsIssueItemLocation.Width = (itemBin.binBalance_WidthBal ?? 0) - (itemBin.binBalance_WidthReserve ?? 0);
                                    GoodsIssueItemLocation.Width_Index = itemBin.binBalance_WidthBal_Index;
                                    GoodsIssueItemLocation.Width_Id = itemBin.binBalance_WidthBal_Id;
                                    GoodsIssueItemLocation.Width_Name = itemBin.binBalance_WidthBal_Name;
                                    GoodsIssueItemLocation.WidthRatio = itemBin.binBalance_WidthBalRatio;

                                    GoodsIssueItemLocation.UnitLength = (itemBin.binBalance_UnitLengthBal ?? 0);
                                    GoodsIssueItemLocation.UnitLength_Index = itemBin.binBalance_UnitLengthBal_Index;
                                    GoodsIssueItemLocation.UnitLength_Id = itemBin.binBalance_UnitLengthBal_Id;
                                    GoodsIssueItemLocation.UnitLength_Name = itemBin.binBalance_UnitLengthBal_Name;
                                    GoodsIssueItemLocation.UnitLengthRatio = itemBin.binBalance_UnitLengthBalRatio;

                                    GoodsIssueItemLocation.Length = (itemBin.binBalance_LengthBal ?? 0) - (itemBin.binBalance_LengthReserve ?? 0);
                                    GoodsIssueItemLocation.Length_Index = itemBin.binBalance_LengthBal_Index;
                                    GoodsIssueItemLocation.Length_Id = itemBin.binBalance_LengthBal_Id;
                                    GoodsIssueItemLocation.Length_Name = itemBin.binBalance_LengthBal_Name;
                                    GoodsIssueItemLocation.LengthRatio = itemBin.binBalance_LengthBalRatio;

                                    GoodsIssueItemLocation.UnitHeight = (itemBin.binBalance_UnitHeightBal ?? 0);
                                    GoodsIssueItemLocation.UnitHeight_Index = itemBin.binBalance_UnitHeightBal_Index;
                                    GoodsIssueItemLocation.UnitHeight_Id = itemBin.binBalance_UnitHeightBal_Id;
                                    GoodsIssueItemLocation.UnitHeight_Name = itemBin.binBalance_UnitHeightBal_Name;
                                    GoodsIssueItemLocation.UnitHeightRatio = itemBin.binBalance_UnitHeightBalRatio;

                                    GoodsIssueItemLocation.Height = (itemBin.binBalance_HeightBal ?? 0) - (itemBin.binBalance_HeightReserve ?? 0);
                                    GoodsIssueItemLocation.Height_Index = itemBin.binBalance_HeightBal_Index;
                                    GoodsIssueItemLocation.Height_Id = itemBin.binBalance_HeightBal_Id;
                                    GoodsIssueItemLocation.Height_Name = itemBin.binBalance_HeightBal_Name;
                                    GoodsIssueItemLocation.HeightRatio = itemBin.binBalance_HeightBalRatio;

                                    GoodsIssueItemLocation.UnitVolume = (itemBin.binBalance_UnitVolumeBal ?? 0);
                                    GoodsIssueItemLocation.Volume = (itemBin.binBalance_VolumeBal ?? 0) - (itemBin.binBalance_VolumeReserve ?? 0);

                                    GoodsIssueItemLocation.UnitPrice = (itemBin.unitPrice ?? 0);
                                    GoodsIssueItemLocation.UnitPrice_Index = itemBin.unitPrice_Index;
                                    GoodsIssueItemLocation.UnitPrice_Id = itemBin.unitPrice_Id;
                                    GoodsIssueItemLocation.UnitPrice_Name = itemBin.unitPrice_Name;
                                    GoodsIssueItemLocation.Price = (itemBin.price ?? 0);
                                    GoodsIssueItemLocation.Price_Index = itemBin.price_Index;
                                    GoodsIssueItemLocation.Price_Id = itemBin.price_Id;
                                    GoodsIssueItemLocation.Price_Name = itemBin.price_Name;


                                    GoodsIssueItemLocation.DocumentRef_No1 = itemPlanGI.DocumentRef_No1;
                                    GoodsIssueItemLocation.DocumentRef_No2 = itemPlanGI.DocumentRef_No2;
                                    GoodsIssueItemLocation.DocumentRef_No3 = itemPlanGI.DocumentRef_No3;
                                    GoodsIssueItemLocation.DocumentRef_No4 = itemPlanGI.DocumentRef_No4;
                                    GoodsIssueItemLocation.DocumentRef_No5 = itemPlanGI.DocumentRef_No5;
                                    GoodsIssueItemLocation.Document_Status = -2;
                                    GoodsIssueItemLocation.UDF_1 = itemPlanGI.UDF_1;
                                    GoodsIssueItemLocation.UDF_2 = itemPlanGI.UDF_2;
                                    GoodsIssueItemLocation.UDF_3 = itemPlanGI.UDF_3;
                                    GoodsIssueItemLocation.UDF_4 = itemPlanGI.UDF_4;
                                    GoodsIssueItemLocation.UDF_5 = itemPlanGI.UDF_5;
                                    GoodsIssueItemLocation.Ref_Process_Index = new Guid("22744590-55D8-4448-88EF-5997C252111F");  // PLAN GI Process
                                    GoodsIssueItemLocation.Ref_Document_No = itemPlanGI.PlanGoodsIssue_No;
                                    GoodsIssueItemLocation.Ref_Document_Index = (Guid)itemPlanGI.PlanGoodsIssue_Index;
                                    GoodsIssueItemLocation.Ref_DocumentItem_Index = itemPlanGI.PlanGoodsIssueItem_Index;
                                    GoodsIssueItemLocation.GoodsReceiveItem_Index = new Guid(itemBin.goodsReceiveItem_Index);
                                    GoodsIssueItemLocation.Create_By = model.create_by;
                                    GoodsIssueItemLocation.Create_Date = DateTime.Now;
                                    GoodsIssueItemLocation.GoodsIssue_No = model.goodsIssue_No;
                                    GoodsIssueItemLocation.BinBalance_Index = new Guid(itemBin.binBalance_Index);


                                    GoodsIssueItemLocation.Invoice_No = itemBin.invoice_No;
                                    GoodsIssueItemLocation.Declaration_No = itemBin.declaration_No;
                                    GoodsIssueItemLocation.HS_Code = itemBin.hs_Code;
                                    GoodsIssueItemLocation.Conutry_of_Origin = itemBin.conutry_of_Origin;
                                    GoodsIssueItemLocation.Tax1 = itemBin.tax1;
                                    GoodsIssueItemLocation.Tax1_Currency_Index = itemBin.tax1_Currency_Index;
                                    GoodsIssueItemLocation.Tax1_Currency_Id = itemBin.tax1_Currency_Id;
                                    GoodsIssueItemLocation.Tax1_Currency_Name = itemBin.tax1_Currency_Name;
                                    GoodsIssueItemLocation.Tax2 = itemBin.tax2;
                                    GoodsIssueItemLocation.Tax2_Currency_Index = itemBin.tax2_Currency_Index;
                                    GoodsIssueItemLocation.Tax2_Currency_Id = itemBin.tax2_Currency_Id;
                                    GoodsIssueItemLocation.Tax2_Currency_Name = itemBin.tax2_Currency_Name;
                                    GoodsIssueItemLocation.Tax3 = itemBin.tax3;
                                    GoodsIssueItemLocation.Tax3_Currency_Index = itemBin.tax3_Currency_Index;
                                    GoodsIssueItemLocation.Tax3_Currency_Id = itemBin.tax3_Currency_Id;
                                    GoodsIssueItemLocation.Tax3_Currency_Name = itemBin.tax3_Currency_Name;
                                    GoodsIssueItemLocation.Tax4 = itemBin.tax4;
                                    GoodsIssueItemLocation.Tax4_Currency_Index = itemBin.tax4_Currency_Index;
                                    GoodsIssueItemLocation.Tax4_Currency_Id = itemBin.tax4_Currency_Id;
                                    GoodsIssueItemLocation.Tax4_Currency_Name = itemBin.tax4_Currency_Name;
                                    GoodsIssueItemLocation.Tax5 = itemBin.tax5;
                                    GoodsIssueItemLocation.Tax5_Currency_Index = itemBin.tax5_Currency_Index;
                                    GoodsIssueItemLocation.Tax5_Currency_Id = itemBin.tax5_Currency_Id;
                                    GoodsIssueItemLocation.Tax5_Currency_Name = itemBin.tax5_Currency_Name;
                                    GoodsIssueItemLocation.ERP_Location = itemBin.erp_Location;
                                    GoodsIssueItemLocation.Export_type =  "X";

                                    chkdatawave = true;
                                    ListGoodsIssueItemLocation.Add(GoodsIssueItemLocation);

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [GIL]   GoodsIssueItemLocation.TotalQty " + GoodsIssueItemLocation.TotalQty.ToString() + "    Ref_Document_No : " + GoodsIssueItemLocation.Ref_Document_No.ToString());

                                    itemPlanGI.GITotalQty = itemPlanGI.GITotalQty + QtyBal;

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPlanGIRemian - QtyBal ]  " + QtyPlanGIRemian.ToString() + "  - " + QtyBal.ToString());

                                    QtyPlanGIRemian = QtyPlanGIRemian - QtyBal;

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [END line QtyPlanGIRemian ]  " + QtyPlanGIRemian.ToString());

                                }
                                else if (QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0)
                                {
                                    State = "QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0";
                                    olog.logging("runwave", State + " TAG_NO " + itemBin.tag_No + " Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0]   QtyPlanGIRemian " + QtyPlanGIRemian.ToString() + "    QtyBal : " + QtyBal.ToString());

                                    var QtyPick = QtyPlanGIRemian;

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPick]   QtyPick " + QtyPick.ToString());

                                    var GoodsIssueItemLocation = new im_GoodsIssueItemLocation();
                                    GoodsIssueItemLocation.GoodsIssueItemLocation_Index = Guid.NewGuid();
                                    GoodsIssueItemLocation.GoodsIssue_Index = new Guid(model.goodsIssue_Index);
                                    GoodsIssueItemLocation.TagItem_Index = new Guid(itemBin.tagItem_Index);
                                    GoodsIssueItemLocation.Tag_Index = new Guid(itemBin.tag_Index);
                                    GoodsIssueItemLocation.Tag_No = itemBin.tag_No;
                                    GoodsIssueItemLocation.Product_Index = new Guid(itemBin.product_Index);
                                    GoodsIssueItemLocation.Product_Id = itemBin.product_Id;
                                    GoodsIssueItemLocation.Product_Name = itemBin.product_Name;
                                    GoodsIssueItemLocation.Product_SecondName = itemBin.product_SecondName;
                                    GoodsIssueItemLocation.Product_ThirdName = itemBin.product_ThirdName;
                                    GoodsIssueItemLocation.Product_Lot = itemBin.product_Lot;
                                    GoodsIssueItemLocation.ItemStatus_Index = new Guid(itemBin.itemStatus_Index);
                                    GoodsIssueItemLocation.ItemStatus_Id = itemBin.itemStatus_Id;
                                    GoodsIssueItemLocation.ItemStatus_Name = itemBin.itemStatus_Name;
                                    GoodsIssueItemLocation.Location_Index = new Guid(itemBin.location_Index);
                                    GoodsIssueItemLocation.Location_Id = itemBin.location_Id;
                                    GoodsIssueItemLocation.Location_Name = itemBin.location_Name;
                                    GoodsIssueItemLocation.QtyPlan = (Decimal)itemPlanGI.TotalQtyRemian;
                                    GoodsIssueItemLocation.Qty = (Decimal)QtyPick / (Decimal)itemPlanGI.Ratio;
                                    GoodsIssueItemLocation.Ratio = (Decimal)itemPlanGI.Ratio;
                                    GoodsIssueItemLocation.TotalQty = (Decimal)QtyPick;
                                    GoodsIssueItemLocation.ProductConversion_Index = (Guid)itemPlanGI.ProductConversion_Index;
                                    GoodsIssueItemLocation.ProductConversion_Id = itemPlanGI.ProductConversion_Id;
                                    GoodsIssueItemLocation.ProductConversion_Name = itemPlanGI.ProductConversion_Name;
                                    GoodsIssueItemLocation.MFG_Date = !string.IsNullOrEmpty(itemBin.goodsReceive_MFG_Date) ? itemBin.goodsReceive_MFG_Date.toDate() : null;
                                    GoodsIssueItemLocation.EXP_Date = !string.IsNullOrEmpty(itemBin.goodsReceive_EXP_Date) ? itemBin.goodsReceive_EXP_Date.toDate() : null;

                                    if (itemBin.binBalance_WeightBegin == 0)
                                    {
                                        GoodsIssueItemLocation.Weight = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitWeight = itemBin.binBalance_UnitWeightBal;
                                        GoodsIssueItemLocation.UnitWeight_Index = itemBin.binBalance_UnitWeightBal_Index;
                                        GoodsIssueItemLocation.UnitWeight_Id = itemBin.binBalance_UnitWeightBal_Id;
                                        GoodsIssueItemLocation.UnitWeight_Name = itemBin.binBalance_UnitWeightBal_Name;
                                        GoodsIssueItemLocation.UnitWeightRatio = itemBin.binBalance_UnitWeightBalRatio;

                                        GoodsIssueItemLocation.Weight = (Decimal)(QtyPick * (itemBin.binBalance_UnitWeightBal ?? 0));
                                        GoodsIssueItemLocation.Weight_Index = itemBin.binBalance_UnitWeightBal_Index;
                                        GoodsIssueItemLocation.Weight_Id = itemBin.binBalance_UnitWeightBal_Id;
                                        GoodsIssueItemLocation.Weight_Name = itemBin.binBalance_UnitWeightBal_Name;
                                        GoodsIssueItemLocation.WeightRatio = itemBin.binBalance_UnitWeightBalRatio;
                                    }

                                    if (itemBin.binBalance_NetWeightBegin == 0)
                                    {
                                        GoodsIssueItemLocation.NetWeight = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitNetWeight = itemBin.binBalance_UnitNetWeightBal;
                                        GoodsIssueItemLocation.UnitNetWeight_Index = itemBin.binBalance_UnitNetWeightBal_Index;
                                        GoodsIssueItemLocation.UnitNetWeight_Id = itemBin.binBalance_UnitNetWeightBal_Id;
                                        GoodsIssueItemLocation.UnitNetWeight_Name = itemBin.binBalance_UnitNetWeightBal_Name;
                                        GoodsIssueItemLocation.UnitNetWeightRatio = itemBin.binBalance_UnitNetWeightBalRatio;

                                        GoodsIssueItemLocation.NetWeight = (Decimal)(QtyPick * (itemBin.binBalance_UnitNetWeightBal ?? 0));
                                        GoodsIssueItemLocation.NetWeight_Index = itemBin.binBalance_UnitNetWeightBal_Index;
                                        GoodsIssueItemLocation.NetWeight_Id = itemBin.binBalance_UnitNetWeightBal_Id;
                                        GoodsIssueItemLocation.NetWeight_Name = itemBin.binBalance_UnitNetWeightBal_Name;
                                        GoodsIssueItemLocation.NetWeightRatio = itemBin.binBalance_UnitNetWeightBalRatio;
                                    }

                                    if (itemBin.binBalance_GrsWeightBegin == 0)
                                    {
                                        GoodsIssueItemLocation.GrsWeight = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitGrsWeight = itemBin.binBalance_UnitGrsWeightBal;
                                        GoodsIssueItemLocation.UnitGrsWeight_Index = itemBin.binBalance_UnitGrsWeightBal_Index;
                                        GoodsIssueItemLocation.UnitGrsWeight_Id = itemBin.binBalance_UnitGrsWeightBal_Id;
                                        GoodsIssueItemLocation.UnitGrsWeight_Name = itemBin.binBalance_UnitGrsWeightBal_Name;
                                        GoodsIssueItemLocation.UnitGrsWeightRatio = itemBin.binBalance_UnitGrsWeightBalRatio;

                                        GoodsIssueItemLocation.GrsWeight = (Decimal)(QtyPick * (itemBin.binBalance_UnitGrsWeightBal ?? 0));
                                        GoodsIssueItemLocation.GrsWeight_Index = itemBin.binBalance_UnitGrsWeightBal_Index;
                                        GoodsIssueItemLocation.GrsWeight_Id = itemBin.binBalance_UnitGrsWeightBal_Id;
                                        GoodsIssueItemLocation.GrsWeight_Name = itemBin.binBalance_UnitGrsWeightBal_Name;
                                        GoodsIssueItemLocation.GrsWeightRatio = itemBin.binBalance_UnitGrsWeightBalRatio;
                                    }

                                    if (itemBin.binBalance_WidthBegin == 0)
                                    {
                                        GoodsIssueItemLocation.Width = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitWidth = (itemBin.binBalance_UnitWidthBal ?? 0);
                                        GoodsIssueItemLocation.UnitWidth_Index = itemBin.binBalance_UnitWidthBal_Index;
                                        GoodsIssueItemLocation.UnitWidth_Id = itemBin.binBalance_UnitWidthBal_Id;
                                        GoodsIssueItemLocation.UnitWidth_Name = itemBin.binBalance_UnitWidthBal_Name;
                                        GoodsIssueItemLocation.UnitWidthRatio = itemBin.binBalance_UnitWidthBalRatio;

                                        GoodsIssueItemLocation.Width = (Decimal)(QtyPick * (itemBin.binBalance_UnitWidthBal ?? 0));
                                        GoodsIssueItemLocation.Width_Index = itemBin.binBalance_UnitWidthBal_Index;
                                        GoodsIssueItemLocation.Width_Id = itemBin.binBalance_UnitWidthBal_Id;
                                        GoodsIssueItemLocation.Width_Name = itemBin.binBalance_UnitWidthBal_Name;
                                        GoodsIssueItemLocation.WidthRatio = itemBin.binBalance_UnitWidthBalRatio;
                                    }

                                    if (itemBin.binBalance_LengthBegin == 0)
                                    {
                                        GoodsIssueItemLocation.Length = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitLength = (itemBin.binBalance_UnitLengthBal ?? 0);
                                        GoodsIssueItemLocation.UnitLength_Index = itemBin.binBalance_UnitLengthBal_Index;
                                        GoodsIssueItemLocation.UnitLength_Id = itemBin.binBalance_UnitLengthBal_Id;
                                        GoodsIssueItemLocation.UnitLength_Name = itemBin.binBalance_UnitLengthBal_Name;
                                        GoodsIssueItemLocation.UnitLengthRatio = itemBin.binBalance_UnitLengthBalRatio;

                                        GoodsIssueItemLocation.Length = (Decimal)(QtyPick * (itemBin.binBalance_UnitWidthBal ?? 0));
                                        GoodsIssueItemLocation.Length_Index = itemBin.binBalance_UnitLengthBal_Index;
                                        GoodsIssueItemLocation.Length_Id = itemBin.binBalance_UnitLengthBal_Id;
                                        GoodsIssueItemLocation.Length_Name = itemBin.binBalance_UnitLengthBal_Name;
                                        GoodsIssueItemLocation.LengthRatio = itemBin.binBalance_UnitLengthBalRatio;
                                    }

                                    if (itemBin.binBalance_HeightBegin == 0)
                                    {
                                        GoodsIssueItemLocation.Height = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitHeight = (itemBin.binBalance_UnitHeightBal ?? 0);
                                        GoodsIssueItemLocation.UnitHeight_Index = itemBin.binBalance_UnitHeightBal_Index;
                                        GoodsIssueItemLocation.UnitHeight_Id = itemBin.binBalance_UnitHeightBal_Id;
                                        GoodsIssueItemLocation.UnitHeight_Name = itemBin.binBalance_UnitHeightBal_Name;
                                        GoodsIssueItemLocation.UnitHeightRatio = itemBin.binBalance_UnitHeightBalRatio;

                                        GoodsIssueItemLocation.Height = (Decimal)(QtyPick * (itemBin.binBalance_UnitHeightBal ?? 0));
                                        GoodsIssueItemLocation.Height_Index = itemBin.binBalance_UnitHeightBal_Index;
                                        GoodsIssueItemLocation.Height_Id = itemBin.binBalance_UnitHeightBal_Id;
                                        GoodsIssueItemLocation.Height_Name = itemBin.binBalance_UnitHeightBal_Name;
                                        GoodsIssueItemLocation.HeightRatio = itemBin.binBalance_UnitHeightBalRatio;
                                    }

                                    if (itemBin.binBalance_UnitVolumeBal == 0)
                                    {
                                        GoodsIssueItemLocation.UnitVolume = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitVolume = (itemBin.binBalance_UnitVolumeBal ?? 0);
                                        GoodsIssueItemLocation.Volume = (Decimal)(QtyPick * (itemBin.binBalance_UnitVolumeBal ?? 0));
                                    }

                                    GoodsIssueItemLocation.UnitPrice = (itemBin.unitPrice ?? 0);
                                    GoodsIssueItemLocation.UnitPrice_Index = itemBin.unitPrice_Index;
                                    GoodsIssueItemLocation.UnitPrice_Id = itemBin.unitPrice_Id;
                                    GoodsIssueItemLocation.UnitPrice_Name = itemBin.unitPrice_Name;
                                    GoodsIssueItemLocation.Price = (Decimal)(QtyPick * (itemBin.unitPrice ?? 0));
                                    GoodsIssueItemLocation.Price_Index = itemBin.unitPrice_Index;
                                    GoodsIssueItemLocation.Price_Id = itemBin.unitPrice_Id;
                                    GoodsIssueItemLocation.Price_Name = itemBin.unitPrice_Name;



                                    GoodsIssueItemLocation.DocumentRef_No1 = itemPlanGI.DocumentRef_No1;
                                    GoodsIssueItemLocation.DocumentRef_No2 = itemPlanGI.DocumentRef_No2;
                                    GoodsIssueItemLocation.DocumentRef_No3 = itemPlanGI.DocumentRef_No3;
                                    GoodsIssueItemLocation.DocumentRef_No4 = itemPlanGI.DocumentRef_No4;
                                    GoodsIssueItemLocation.DocumentRef_No5 = itemPlanGI.DocumentRef_No5;
                                    GoodsIssueItemLocation.Document_Status = -2;
                                    GoodsIssueItemLocation.UDF_1 = itemPlanGI.UDF_1;
                                    GoodsIssueItemLocation.UDF_2 = itemPlanGI.UDF_2;
                                    GoodsIssueItemLocation.UDF_3 = itemPlanGI.UDF_3;
                                    GoodsIssueItemLocation.UDF_4 = itemPlanGI.UDF_4;
                                    GoodsIssueItemLocation.UDF_5 = itemPlanGI.UDF_5;
                                    GoodsIssueItemLocation.Ref_Process_Index = new Guid("22744590-55D8-4448-88EF-5997C252111F");  // PLAN GI Process
                                    GoodsIssueItemLocation.Ref_Document_No = itemPlanGI.PlanGoodsIssue_No;
                                    GoodsIssueItemLocation.Ref_Document_Index = (Guid)itemPlanGI.PlanGoodsIssue_Index;
                                    GoodsIssueItemLocation.Ref_DocumentItem_Index = itemPlanGI.PlanGoodsIssueItem_Index;
                                    GoodsIssueItemLocation.GoodsReceiveItem_Index = new Guid(itemBin.goodsReceiveItem_Index);
                                    GoodsIssueItemLocation.Create_By = model.create_by;
                                    GoodsIssueItemLocation.Create_Date = DateTime.Now;
                                    GoodsIssueItemLocation.GoodsIssue_No = model.goodsIssue_No;
                                    GoodsIssueItemLocation.BinBalance_Index = new Guid(itemBin.binBalance_Index);

                                    GoodsIssueItemLocation.Invoice_No = itemBin.invoice_No;
                                    GoodsIssueItemLocation.Declaration_No = itemBin.declaration_No;
                                    GoodsIssueItemLocation.HS_Code = itemBin.hs_Code;
                                    GoodsIssueItemLocation.Conutry_of_Origin = itemBin.conutry_of_Origin;
                                    GoodsIssueItemLocation.Tax1 = itemBin.tax1;
                                    GoodsIssueItemLocation.Tax1_Currency_Index = itemBin.tax1_Currency_Index;
                                    GoodsIssueItemLocation.Tax1_Currency_Id = itemBin.tax1_Currency_Id;
                                    GoodsIssueItemLocation.Tax1_Currency_Name = itemBin.tax1_Currency_Name;
                                    GoodsIssueItemLocation.Tax2 = itemBin.tax2;
                                    GoodsIssueItemLocation.Tax2_Currency_Index = itemBin.tax2_Currency_Index;
                                    GoodsIssueItemLocation.Tax2_Currency_Id = itemBin.tax2_Currency_Id;
                                    GoodsIssueItemLocation.Tax2_Currency_Name = itemBin.tax2_Currency_Name;
                                    GoodsIssueItemLocation.Tax3 = itemBin.tax3;
                                    GoodsIssueItemLocation.Tax3_Currency_Index = itemBin.tax3_Currency_Index;
                                    GoodsIssueItemLocation.Tax3_Currency_Id = itemBin.tax3_Currency_Id;
                                    GoodsIssueItemLocation.Tax3_Currency_Name = itemBin.tax3_Currency_Name;
                                    GoodsIssueItemLocation.Tax4 = itemBin.tax4;
                                    GoodsIssueItemLocation.Tax4_Currency_Index = itemBin.tax4_Currency_Index;
                                    GoodsIssueItemLocation.Tax4_Currency_Id = itemBin.tax4_Currency_Id;
                                    GoodsIssueItemLocation.Tax4_Currency_Name = itemBin.tax4_Currency_Name;
                                    GoodsIssueItemLocation.Tax5 = itemBin.tax5;
                                    GoodsIssueItemLocation.Tax5_Currency_Index = itemBin.tax5_Currency_Index;
                                    GoodsIssueItemLocation.Tax5_Currency_Id = itemBin.tax5_Currency_Id;
                                    GoodsIssueItemLocation.Tax5_Currency_Name = itemBin.tax5_Currency_Name;
                                    GoodsIssueItemLocation.ERP_Location = itemBin.erp_Location;
                                    GoodsIssueItemLocation.Export_type = "X";

                                    chkdatawave = true;
                                    ListGoodsIssueItemLocation.Add(GoodsIssueItemLocation);

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [GIL]   GoodsIssueItemLocation.TotalQty " + GoodsIssueItemLocation.TotalQty.ToString() + "    Ref_Document_No : " + GoodsIssueItemLocation.Ref_Document_No.ToString());

                                    itemPlanGI.GITotalQty = itemPlanGI.GITotalQty + QtyPick;

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [ QtyPlanGIRemian - QtyPick ]  " + QtyPlanGIRemian.ToString() + "  - " + QtyPick.ToString());

                                    QtyPlanGIRemian = QtyPlanGIRemian - QtyPick;

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [END line QtyPlanGIRemian ]  " + QtyPlanGIRemian.ToString());

                                }
                            }

                            #endregion

                            olog.logging("GIStock", " -------------- ");

                            #region inset GIL and runwave status 30

                            using (var db4 = new GIDbContext())
                            {
                                db4.Database.SetCommandTimeout(120);
                                var transaction = db4.Database.BeginTransaction(IsolationLevel.Serializable);
                                try
                                {
                                    var GI = db4.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 20).ToList();
                                    foreach (var g in GI)
                                    {
                                        g.RunWave_Status = 30;
                                    }

                                    db4.IM_GoodsIssueItemLocation.AddRange(ListGoodsIssueItemLocation);
                                    db4.SaveChanges();
                                    transaction.Commit();
                                }
                                catch (Exception exy)
                                {
                                    msglog = State + " ex Rollback " + exy.Message.ToString();
                                    olog.logging("runwave", msglog);
                                    transaction.Rollback();
                                    throw exy;
                                }
                            }

                            #endregion

                            State = "inset GIL and runwave status 30";
                            olog.logging("runwave", State);

                            #region insert bincardreserve and runwave status 40

                            using (var Contact = new GIDbContext())
                            {
                                Contact.Database.SetCommandTimeout(360);

                                State = "Contact.IM_GoodsIssueItemLocation";
                                olog.logging("runwave", State);

                                var GIL = Contact.IM_GoodsIssueItemLocation.Where(c => c.Ref_Document_Index == itemPlanGI.PlanGoodsIssue_Index && c.Ref_DocumentItem_Index == itemPlanGI.PlanGoodsIssueItem_Index && c.Document_Status != -1).ToList();

                                #region for GIL >>>>  I_LOOP

                                foreach (var g in GIL)
                                {

                                    State = "insertBinCardReserve";

                                    olog.logging("runwave", State + " GIIL_Index" + g.GoodsIssueItemLocation_Index.ToString());

                                    var insertBinCardReserve = new PickbinbalanceFromGIViewModel();

                                    insertBinCardReserve.ref_Document_Index = g.GoodsIssue_Index.ToString();
                                    insertBinCardReserve.ref_DocumentItem_Index = g.GoodsIssueItemLocation_Index.ToString();
                                    insertBinCardReserve.goodsIssue_No = model.goodsIssue_No;
                                    insertBinCardReserve.process_Index = "22744590-55D8-4448-88EF-5997C252111F";
                                    insertBinCardReserve.create_By = model.create_by;
                                    insertBinCardReserve.pick = g.TotalQty;
                                    insertBinCardReserve.binbalance_Index = g.BinBalance_Index.ToString();
                                    insertBinCardReserve.wave_Index = "7efa474a-8ff4-439e-a5d7-8c844cb53b56";

                                    State = "insetBinRe";
                                    olog.logging("runwave", State);
                                    var insetBinRe = getinsertBinCardReserve(insertBinCardReserve);
                                    //var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("insertBinCardReserve"), insertBinCardReserve.sJson());
                                    if (insetBinRe.resultIsUse)
                                    {
                                        State = "resultIsUse";
                                        olog.logging("runwave", State);

                                        var transaction = Contact.Database.BeginTransaction(IsolationLevel.Serializable);
                                        try
                                        {
                                            var GI = Contact.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 30).ToList();
                                            foreach (var gg in GI)
                                            {
                                                gg.RunWave_Status = 40;
                                            }
                                            Contact.SaveChanges();
                                            transaction.Commit();
                                        }
                                        catch (Exception exy)
                                        {
                                            msglog = State + " ex Rollback " + exy.Message.ToString();
                                            olog.logging("runwave", msglog);
                                            transaction.Rollback();
                                            throw exy;
                                        }
                                    }
                                    else
                                    {
                                        State = "resultIsUse else";
                                        olog.logging("runwave", State);

                                        var transaction = Contact.Database.BeginTransaction(IsolationLevel.Serializable);
                                        try
                                        {

                                            //var GoodsIssueItemLocation_Index = new SqlParameter("@GoodsIssueItemLocation_Index", g.GoodsIssueItemLocation_Index);
                                            //var resultPickingplan = db.Database.ExecuteSqlCommand("EXEC sp_DeleteGIIL_Error @GoodsIssueItemLocation_Index", GoodsIssueItemLocation_Index);

                                            var GI = Contact.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && (c.RunWave_Status == 30 || c.RunWave_Status == 40)).ToList();
                                            foreach (var gg in GI)
                                            {
                                                gg.RunWave_Status = 30;
                                            }
                                            Contact.SaveChanges();
                                            transaction.Commit();
                                        }
                                        catch (Exception exy)
                                        {
                                            msglog = State + " ex Rollback " + exy.Message.ToString();
                                            olog.logging("runwave", msglog);
                                            transaction.Rollback();
                                            throw exy;
                                        }
                                        msglog = State + " ex Rollback " + "Insert BinCardReserve Error";
                                        olog.logging("runwave", msglog);
                                        result.resultMsg = "Insert BinCardReserve Error";
                                        result.resultIsUse = false;
                                        //return result;

                                    }
                                }

                                #endregion

                            }
                            #endregion
                            State = "insert bincardreserve and runwave status 40";
                            olog.logging("runwave", State);
                        }

                        #endregion

                        #region normal

                        foreach (var itemPlanGI in planwith_normal.OrderBy(c => c.LineNum))
                        {
                          
                            if (itemPlanGI.ModPlanGI == 0)
                            {
                                IsPA = 0;

                            }
                            else
                            {
                                IsPA = 1;
                            }

                            State = "listDataProduct2.ToList()";
                            olog.logging("runwave", State);
                            var listProducttote = listDataProduct2.Where(c => c.product_Index == itemPlanGI.Product_Index).ToList();
                            if (listProducttote != null)
                            {
                                var checkProduct = listProducttote.Where(c => c.product_Index == itemPlanGI.Product_Index).FirstOrDefault();
                                if (checkProduct == null)
                                {
                                    continue;
                                }

                                if (checkProduct.Ref_No1 == "carton flow rack")
                                {
                                    IsTote = true;

                                }
                                else
                                {
                                    IsTote = false;

                                }

                            }
                            else
                            {
                                IsTote = false;
                            }


                            var ListGoodsIssueItemLocation = new List<im_GoodsIssueItemLocation>();
                            if (itemPlanGI.Product_Id == "1004492")
                            {
                                var lnum = itemPlanGI.LineNum;

                            }

                            QtyPlanGIRemian = itemPlanGI.TotalQty - itemPlanGI.GITotalQty;
                            if (QtyPlanGIRemian <= 0)
                            {
                                break;
                            }

                            #region view_waveBinbalance2
                            State = "getViewBinbalanceapi";
                            olog.logging("runwave", State);
                            State = "View_WaveCheckProductLot";
                            olog.logging("runwave", State);

                            var ListLot = new List<String>();
                            var ListLotNotWave = new List<String>();
                            var listAll_Lot = db.View_WaveCheckProductLot.Where(c => c.Product_Index == itemPlanGI.Product_Index).ToList();

                            var listLot_In_Product = listAll_Lot.Where(c => c.PlanGoodsIssue_Index == itemPlanGI.PlanGoodsIssue_Index && c.Product_Index == itemPlanGI.Product_Index).ToList();

                            if (listLot_In_Product.Count > 0)
                            {
                                foreach (var itemlot in listLot_In_Product)
                                {
                                    ListLot.Add(item.Product_Lot);
                                }
                            }

                            var listLot_NotIN_Wave = listAll_Lot.Where(c => !ListLot.Contains(c.Product_Lot)).ToList();


                            if (listLot_NotIN_Wave.Count > 0)
                            {
                                foreach (var itemlot in listLot_NotIN_Wave)
                                {
                                    ListLotNotWave.Add(itemlot.Product_Lot);
                                }
                            }

                            State = "getView_WaveBinBalance2";
                            olog.logging("runwave", State);
                            var GIDate = model.goodsIssue_Date.toDate();

                            var GoodsIssue_Index = new SqlParameter("@GoodsIssue_Index", strwhere.isuse);
                            var Owner_Index = new SqlParameter("@Owner_Index", strwhere.Owner_Index.ToString());
                            var Product_Index = new SqlParameter("@Product_Index", strwhere.Product_Index.ToString());
                            var Product_Lot = new SqlParameter("@Product_Lot", strwhere.Product_Lot == null ? "" : strwhere.Product_Lot);
                            var ItemStatus_Index = new SqlParameter("@ItemStatus_Index", strwhere.ItemStatus_Index.ToString());
                            List<View_WaveBinBalanceViewModel_Ace> View_WaveBinBalance2 = new List<View_WaveBinBalanceViewModel_Ace>();
                            View_WaveBinBalance2 = db.View_WaveBinBalanceViewModel_Ace.FromSql("EXEC sp_WaveBinBalance @GoodsIssue_Index ,@Owner_Index ,@Product_Index ,@Product_Lot ,@ItemStatus_Index", GoodsIssue_Index, Owner_Index, Product_Index, Product_Lot, ItemStatus_Index).ToList();


                            State = "View_WaveBinBalance2 EXEC";
                            olog.logging("runwave", State);

                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c =>
                                (c.goodsReceive_EXP_Date != null ? c.goodsReceive_EXP_Date.sParse<DateTime>().Subtract(DateTime.Now.AddDays(-1)).Days : 1) > (c.productShelfLife_D ?? 0) &&
                                !(ListLotNotWave.Contains(c.product_Lot)) &&
                                 (c.binBalance_QtyBal) > 0 &&
                                 (c.binBalance_QtyReserve) >= 0 &&
                                c.goodsReceive_Date <= GIDate.Value.Date &&
                                (string.IsNullOrEmpty(itemPlanGI.ERP_Location) ? (c.erp_Location ?? "") == "" : c.erp_Location == itemPlanGI.ERP_Location)
                            ).ToList();

                            State = "View_WaveBinBalance2 EXEC S";




                            #endregion
                            
                            State = "View_WaveBinBalance2";
                            olog.logging("runwave", State);
                            var BinBalanceResult = View_WaveBinBalance2.ToList();

                            State = "View_WaveBinBalance2.ToList";
                            olog.logging("runwave", State);

                            var itemBinSort = new List<View_WaveBinBalanceViewModel_Ace>();


                            if (IsPA == 1 && IsTote == true)
                            {
                                itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenBy(f => f.location_Bay).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name).ToList();

                            }
                            else if (IsPA == 0 && IsTote == true)
                            {
                                itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenByDescending(f => f.location_Bay).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name).ToList();
                            }
                            else
                            {
                                itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name).ToList();

                            }

                            int icountloop = 0;

                            #region for itemBinSort >>>> H_LOOP

                            foreach (var itemBin in itemBinSort)
                            {
                                icountloop = icountloop + 1;

                                olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [" + icountloop.ToString() + "]  Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);

                                decimal? QtyBal = itemBin.binBalance_QtyBal - itemBin.binBalance_QtyReserve;


                                olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " :  QtyBal " + QtyBal.ToString() + " =  binBalance_QtyBal : " + itemBin.binBalance_QtyBal.ToString() + "  -  binBalance_QtyReserve  : " + itemBin.binBalance_QtyReserve.ToString());


                                if (QtyPlanGIRemian <= 0)
                                {
                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPlanGIRemian <= 0 ]  " + QtyPlanGIRemian.ToString());

                                    break;
                                }
                                if (QtyBal <= 0)
                                {
                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyBal <= 0 ]  " + QtyBal.ToString());
                                    continue;
                                }


                                if (QtyPlanGIRemian >= QtyBal && QtyBal > 0)
                                {
                                    State = "QtyPlanGIRemian >= QtyBal && QtyBal > 0";
                                    olog.logging("runwave", State + " TAG_NO " + itemBin.tag_No + " Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);
                                    
                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPlanGIRemian >= QtyBal && QtyBal > 0]   QtyPlanGIRemian " + QtyPlanGIRemian.ToString() + "    QtyBal : " + QtyBal.ToString());
                                    
                                    var GoodsIssueItemLocation = new im_GoodsIssueItemLocation();
                                    GoodsIssueItemLocation.GoodsIssueItemLocation_Index = Guid.NewGuid();
                                    GoodsIssueItemLocation.GoodsIssue_Index = new Guid(model.goodsIssue_Index);
                                    GoodsIssueItemLocation.TagItem_Index = new Guid(itemBin.tagItem_Index);
                                    GoodsIssueItemLocation.Tag_Index = new Guid(itemBin.tag_Index);
                                    GoodsIssueItemLocation.Tag_No = itemBin.tag_No;
                                    GoodsIssueItemLocation.Product_Index = new Guid(itemBin.product_Index);
                                    GoodsIssueItemLocation.Product_Id = itemBin.product_Id;
                                    GoodsIssueItemLocation.Product_Name = itemBin.product_Name;
                                    GoodsIssueItemLocation.Product_SecondName = itemBin.product_SecondName;
                                    GoodsIssueItemLocation.Product_ThirdName = itemBin.product_ThirdName;
                                    GoodsIssueItemLocation.Product_Lot = itemBin.product_Lot;
                                    GoodsIssueItemLocation.ItemStatus_Index = new Guid(itemBin.itemStatus_Index);
                                    GoodsIssueItemLocation.ItemStatus_Id = itemBin.itemStatus_Id;
                                    GoodsIssueItemLocation.ItemStatus_Name = itemBin.itemStatus_Name;
                                    GoodsIssueItemLocation.Location_Index = new Guid(itemBin.location_Index);
                                    GoodsIssueItemLocation.Location_Id = itemBin.location_Id;
                                    GoodsIssueItemLocation.Location_Name = itemBin.location_Name;
                                    GoodsIssueItemLocation.QtyPlan = (Decimal)itemPlanGI.TotalQtyRemian;
                                    GoodsIssueItemLocation.Qty = (Decimal)QtyBal / (Decimal)itemPlanGI.Ratio;
                                    GoodsIssueItemLocation.Ratio = (Decimal)itemPlanGI.Ratio;
                                    GoodsIssueItemLocation.TotalQty = (Decimal)QtyBal;
                                    GoodsIssueItemLocation.ProductConversion_Index = (Guid)itemPlanGI.ProductConversion_Index;
                                    GoodsIssueItemLocation.ProductConversion_Id = itemPlanGI.ProductConversion_Id;
                                    GoodsIssueItemLocation.ProductConversion_Name = itemPlanGI.ProductConversion_Name;
                                    GoodsIssueItemLocation.MFG_Date = !string.IsNullOrEmpty(itemBin.goodsReceive_MFG_Date) ? itemBin.goodsReceive_MFG_Date.toDate() : null;
                                    GoodsIssueItemLocation.EXP_Date = !string.IsNullOrEmpty(itemBin.goodsReceive_EXP_Date) ? itemBin.goodsReceive_EXP_Date.toDate() : null;

                                    GoodsIssueItemLocation.UnitWeight = itemBin.binBalance_UnitWeightBal;
                                    GoodsIssueItemLocation.UnitWeight_Index = itemBin.binBalance_UnitWeightBal_Index;
                                    GoodsIssueItemLocation.UnitWeight_Id = itemBin.binBalance_UnitWeightBal_Id;
                                    GoodsIssueItemLocation.UnitWeight_Name = itemBin.binBalance_UnitWeightBal_Name;
                                    GoodsIssueItemLocation.UnitWeightRatio = itemBin.binBalance_UnitWeightBalRatio;

                                    GoodsIssueItemLocation.Weight = (itemBin.binBalance_WeightBal ?? 0) - (itemBin.binBalance_WeightReserve ?? 0);
                                    GoodsIssueItemLocation.Weight_Index = itemBin.binBalance_WeightBal_Index;
                                    GoodsIssueItemLocation.Weight_Id = itemBin.binBalance_WeightBal_Id;
                                    GoodsIssueItemLocation.Weight_Name = itemBin.binBalance_WeightBal_Name;
                                    GoodsIssueItemLocation.WeightRatio = itemBin.binBalance_WeightBalRatio;

                                    GoodsIssueItemLocation.UnitNetWeight = itemBin.binBalance_UnitNetWeightBal;
                                    GoodsIssueItemLocation.UnitNetWeight_Index = itemBin.binBalance_UnitNetWeightBal_Index;
                                    GoodsIssueItemLocation.UnitNetWeight_Id = itemBin.binBalance_UnitNetWeightBal_Id;
                                    GoodsIssueItemLocation.UnitNetWeight_Name = itemBin.binBalance_UnitNetWeightBal_Name;
                                    GoodsIssueItemLocation.UnitNetWeightRatio = itemBin.binBalance_UnitNetWeightBalRatio;

                                    GoodsIssueItemLocation.NetWeight = (itemBin.binBalance_NetWeightBal ?? 0) - (itemBin.binBalance_NetWeightReserve ?? 0);
                                    GoodsIssueItemLocation.NetWeight_Index = itemBin.binBalance_NetWeightBal_Index;
                                    GoodsIssueItemLocation.NetWeight_Id = itemBin.binBalance_NetWeightBal_Id;
                                    GoodsIssueItemLocation.NetWeight_Name = itemBin.binBalance_NetWeightBal_Name;
                                    GoodsIssueItemLocation.NetWeightRatio = itemBin.binBalance_NetWeightBalRatio;

                                    GoodsIssueItemLocation.UnitGrsWeight = itemBin.binBalance_UnitGrsWeightBal;
                                    GoodsIssueItemLocation.UnitGrsWeight_Index = itemBin.binBalance_UnitGrsWeightBal_Index;
                                    GoodsIssueItemLocation.UnitGrsWeight_Id = itemBin.binBalance_UnitGrsWeightBal_Id;
                                    GoodsIssueItemLocation.UnitGrsWeight_Name = itemBin.binBalance_UnitGrsWeightBal_Name;
                                    GoodsIssueItemLocation.UnitGrsWeightRatio = itemBin.binBalance_UnitGrsWeightBalRatio;

                                    GoodsIssueItemLocation.GrsWeight = (itemBin.binBalance_GrsWeightBal ?? 0) - (itemBin.binBalance_GrsWeightReserve ?? 0);
                                    GoodsIssueItemLocation.GrsWeight_Index = itemBin.binBalance_GrsWeightBal_Index;
                                    GoodsIssueItemLocation.GrsWeight_Id = itemBin.binBalance_GrsWeightBal_Id;
                                    GoodsIssueItemLocation.GrsWeight_Name = itemBin.binBalance_GrsWeightBal_Name;
                                    GoodsIssueItemLocation.GrsWeightRatio = itemBin.binBalance_GrsWeightBalRatio;

                                    GoodsIssueItemLocation.UnitWidth = (itemBin.binBalance_UnitWidthBal ?? 0);
                                    GoodsIssueItemLocation.UnitWidth_Index = itemBin.binBalance_UnitWidthBal_Index;
                                    GoodsIssueItemLocation.UnitWidth_Id = itemBin.binBalance_UnitWidthBal_Id;
                                    GoodsIssueItemLocation.UnitWidth_Name = itemBin.binBalance_UnitWidthBal_Name;
                                    GoodsIssueItemLocation.UnitWidthRatio = itemBin.binBalance_UnitWidthBalRatio;

                                    GoodsIssueItemLocation.Width = (itemBin.binBalance_WidthBal ?? 0) - (itemBin.binBalance_WidthReserve ?? 0);
                                    GoodsIssueItemLocation.Width_Index = itemBin.binBalance_WidthBal_Index;
                                    GoodsIssueItemLocation.Width_Id = itemBin.binBalance_WidthBal_Id;
                                    GoodsIssueItemLocation.Width_Name = itemBin.binBalance_WidthBal_Name;
                                    GoodsIssueItemLocation.WidthRatio = itemBin.binBalance_WidthBalRatio;

                                    GoodsIssueItemLocation.UnitLength = (itemBin.binBalance_UnitLengthBal ?? 0);
                                    GoodsIssueItemLocation.UnitLength_Index = itemBin.binBalance_UnitLengthBal_Index;
                                    GoodsIssueItemLocation.UnitLength_Id = itemBin.binBalance_UnitLengthBal_Id;
                                    GoodsIssueItemLocation.UnitLength_Name = itemBin.binBalance_UnitLengthBal_Name;
                                    GoodsIssueItemLocation.UnitLengthRatio = itemBin.binBalance_UnitLengthBalRatio;

                                    GoodsIssueItemLocation.Length = (itemBin.binBalance_LengthBal ?? 0) - (itemBin.binBalance_LengthReserve ?? 0);
                                    GoodsIssueItemLocation.Length_Index = itemBin.binBalance_LengthBal_Index;
                                    GoodsIssueItemLocation.Length_Id = itemBin.binBalance_LengthBal_Id;
                                    GoodsIssueItemLocation.Length_Name = itemBin.binBalance_LengthBal_Name;
                                    GoodsIssueItemLocation.LengthRatio = itemBin.binBalance_LengthBalRatio;

                                    GoodsIssueItemLocation.UnitHeight = (itemBin.binBalance_UnitHeightBal ?? 0);
                                    GoodsIssueItemLocation.UnitHeight_Index = itemBin.binBalance_UnitHeightBal_Index;
                                    GoodsIssueItemLocation.UnitHeight_Id = itemBin.binBalance_UnitHeightBal_Id;
                                    GoodsIssueItemLocation.UnitHeight_Name = itemBin.binBalance_UnitHeightBal_Name;
                                    GoodsIssueItemLocation.UnitHeightRatio = itemBin.binBalance_UnitHeightBalRatio;

                                    GoodsIssueItemLocation.Height = (itemBin.binBalance_HeightBal ?? 0) - (itemBin.binBalance_HeightReserve ?? 0);
                                    GoodsIssueItemLocation.Height_Index = itemBin.binBalance_HeightBal_Index;
                                    GoodsIssueItemLocation.Height_Id = itemBin.binBalance_HeightBal_Id;
                                    GoodsIssueItemLocation.Height_Name = itemBin.binBalance_HeightBal_Name;
                                    GoodsIssueItemLocation.HeightRatio = itemBin.binBalance_HeightBalRatio;

                                    GoodsIssueItemLocation.UnitVolume = (itemBin.binBalance_UnitVolumeBal ?? 0);
                                    GoodsIssueItemLocation.Volume = (itemBin.binBalance_VolumeBal ?? 0) - (itemBin.binBalance_VolumeReserve ?? 0);

                                    GoodsIssueItemLocation.UnitPrice = (itemBin.unitPrice ?? 0);
                                    GoodsIssueItemLocation.UnitPrice_Index = itemBin.unitPrice_Index;
                                    GoodsIssueItemLocation.UnitPrice_Id = itemBin.unitPrice_Id;
                                    GoodsIssueItemLocation.UnitPrice_Name = itemBin.unitPrice_Name;
                                    GoodsIssueItemLocation.Price = (itemBin.price ?? 0);
                                    GoodsIssueItemLocation.Price_Index = itemBin.price_Index;
                                    GoodsIssueItemLocation.Price_Id = itemBin.price_Id;
                                    GoodsIssueItemLocation.Price_Name = itemBin.price_Name;


                                    GoodsIssueItemLocation.DocumentRef_No1 = itemPlanGI.DocumentRef_No1;
                                    GoodsIssueItemLocation.DocumentRef_No2 = itemPlanGI.DocumentRef_No2;
                                    GoodsIssueItemLocation.DocumentRef_No3 = itemPlanGI.DocumentRef_No3;
                                    GoodsIssueItemLocation.DocumentRef_No4 = itemPlanGI.DocumentRef_No4;
                                    GoodsIssueItemLocation.DocumentRef_No5 = itemPlanGI.DocumentRef_No5;
                                    GoodsIssueItemLocation.Document_Status = -2;
                                    GoodsIssueItemLocation.UDF_1 = itemPlanGI.UDF_1;
                                    GoodsIssueItemLocation.UDF_2 = itemPlanGI.UDF_2;
                                    GoodsIssueItemLocation.UDF_3 = itemPlanGI.UDF_3;
                                    GoodsIssueItemLocation.UDF_4 = itemPlanGI.UDF_4;
                                    GoodsIssueItemLocation.UDF_5 = itemPlanGI.UDF_5;
                                    GoodsIssueItemLocation.Ref_Process_Index = new Guid("22744590-55D8-4448-88EF-5997C252111F");  // PLAN GI Process
                                    GoodsIssueItemLocation.Ref_Document_No = itemPlanGI.PlanGoodsIssue_No;
                                    GoodsIssueItemLocation.Ref_Document_Index = (Guid)itemPlanGI.PlanGoodsIssue_Index;
                                    GoodsIssueItemLocation.Ref_DocumentItem_Index = itemPlanGI.PlanGoodsIssueItem_Index;
                                    GoodsIssueItemLocation.GoodsReceiveItem_Index = new Guid(itemBin.goodsReceiveItem_Index);
                                    GoodsIssueItemLocation.Create_By = model.create_by;
                                    GoodsIssueItemLocation.Create_Date = DateTime.Now;
                                    GoodsIssueItemLocation.GoodsIssue_No = model.goodsIssue_No;
                                    GoodsIssueItemLocation.BinBalance_Index = new Guid(itemBin.binBalance_Index);


                                    GoodsIssueItemLocation.Invoice_No = itemBin.invoice_No;
                                    GoodsIssueItemLocation.Declaration_No = itemBin.declaration_No;
                                    GoodsIssueItemLocation.HS_Code = itemBin.hs_Code;
                                    GoodsIssueItemLocation.Conutry_of_Origin = itemBin.conutry_of_Origin;
                                    GoodsIssueItemLocation.Tax1 = itemBin.tax1;
                                    GoodsIssueItemLocation.Tax1_Currency_Index = itemBin.tax1_Currency_Index;
                                    GoodsIssueItemLocation.Tax1_Currency_Id = itemBin.tax1_Currency_Id;
                                    GoodsIssueItemLocation.Tax1_Currency_Name = itemBin.tax1_Currency_Name;
                                    GoodsIssueItemLocation.Tax2 = itemBin.tax2;
                                    GoodsIssueItemLocation.Tax2_Currency_Index = itemBin.tax2_Currency_Index;
                                    GoodsIssueItemLocation.Tax2_Currency_Id = itemBin.tax2_Currency_Id;
                                    GoodsIssueItemLocation.Tax2_Currency_Name = itemBin.tax2_Currency_Name;
                                    GoodsIssueItemLocation.Tax3 = itemBin.tax3;
                                    GoodsIssueItemLocation.Tax3_Currency_Index = itemBin.tax3_Currency_Index;
                                    GoodsIssueItemLocation.Tax3_Currency_Id = itemBin.tax3_Currency_Id;
                                    GoodsIssueItemLocation.Tax3_Currency_Name = itemBin.tax3_Currency_Name;
                                    GoodsIssueItemLocation.Tax4 = itemBin.tax4;
                                    GoodsIssueItemLocation.Tax4_Currency_Index = itemBin.tax4_Currency_Index;
                                    GoodsIssueItemLocation.Tax4_Currency_Id = itemBin.tax4_Currency_Id;
                                    GoodsIssueItemLocation.Tax4_Currency_Name = itemBin.tax4_Currency_Name;
                                    GoodsIssueItemLocation.Tax5 = itemBin.tax5;
                                    GoodsIssueItemLocation.Tax5_Currency_Index = itemBin.tax5_Currency_Index;
                                    GoodsIssueItemLocation.Tax5_Currency_Id = itemBin.tax5_Currency_Id;
                                    GoodsIssueItemLocation.Tax5_Currency_Name = itemBin.tax5_Currency_Name;
                                    GoodsIssueItemLocation.ERP_Location = itemBin.erp_Location;
                                    
                                    chkdatawave = true;
                                    ListGoodsIssueItemLocation.Add(GoodsIssueItemLocation);

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [GIL]   GoodsIssueItemLocation.TotalQty " + GoodsIssueItemLocation.TotalQty.ToString() + "    Ref_Document_No : " + GoodsIssueItemLocation.Ref_Document_No.ToString());
                                    
                                    itemPlanGI.GITotalQty = itemPlanGI.GITotalQty + QtyBal;

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPlanGIRemian - QtyBal ]  " + QtyPlanGIRemian.ToString() + "  - " + QtyBal.ToString());

                                    QtyPlanGIRemian = QtyPlanGIRemian - QtyBal;

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [END line QtyPlanGIRemian ]  " + QtyPlanGIRemian.ToString());
                                    
                                }
                                else if (QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0)
                                {
                                    State = "QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0";
                                    olog.logging("runwave", State + " TAG_NO " + itemBin.tag_No + " Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);
                                    
                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0]   QtyPlanGIRemian " + QtyPlanGIRemian.ToString() + "    QtyBal : " + QtyBal.ToString());
                                    
                                    var QtyPick = QtyPlanGIRemian;

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [QtyPick]   QtyPick " + QtyPick.ToString());
                                    
                                    var GoodsIssueItemLocation = new im_GoodsIssueItemLocation();
                                    GoodsIssueItemLocation.GoodsIssueItemLocation_Index = Guid.NewGuid();
                                    GoodsIssueItemLocation.GoodsIssue_Index = new Guid(model.goodsIssue_Index);
                                    GoodsIssueItemLocation.TagItem_Index = new Guid(itemBin.tagItem_Index);
                                    GoodsIssueItemLocation.Tag_Index = new Guid(itemBin.tag_Index);
                                    GoodsIssueItemLocation.Tag_No = itemBin.tag_No;
                                    GoodsIssueItemLocation.Product_Index = new Guid(itemBin.product_Index);
                                    GoodsIssueItemLocation.Product_Id = itemBin.product_Id;
                                    GoodsIssueItemLocation.Product_Name = itemBin.product_Name;
                                    GoodsIssueItemLocation.Product_SecondName = itemBin.product_SecondName;
                                    GoodsIssueItemLocation.Product_ThirdName = itemBin.product_ThirdName;
                                    GoodsIssueItemLocation.Product_Lot = itemBin.product_Lot;
                                    GoodsIssueItemLocation.ItemStatus_Index = new Guid(itemBin.itemStatus_Index);
                                    GoodsIssueItemLocation.ItemStatus_Id = itemBin.itemStatus_Id;
                                    GoodsIssueItemLocation.ItemStatus_Name = itemBin.itemStatus_Name;
                                    GoodsIssueItemLocation.Location_Index = new Guid(itemBin.location_Index);
                                    GoodsIssueItemLocation.Location_Id = itemBin.location_Id;
                                    GoodsIssueItemLocation.Location_Name = itemBin.location_Name;
                                    GoodsIssueItemLocation.QtyPlan = (Decimal)itemPlanGI.TotalQtyRemian;
                                    GoodsIssueItemLocation.Qty = (Decimal)QtyPick / (Decimal)itemPlanGI.Ratio;
                                    GoodsIssueItemLocation.Ratio = (Decimal)itemPlanGI.Ratio;
                                    GoodsIssueItemLocation.TotalQty = (Decimal)QtyPick;
                                    GoodsIssueItemLocation.ProductConversion_Index = (Guid)itemPlanGI.ProductConversion_Index;
                                    GoodsIssueItemLocation.ProductConversion_Id = itemPlanGI.ProductConversion_Id;
                                    GoodsIssueItemLocation.ProductConversion_Name = itemPlanGI.ProductConversion_Name;
                                    GoodsIssueItemLocation.MFG_Date = !string.IsNullOrEmpty(itemBin.goodsReceive_MFG_Date) ? itemBin.goodsReceive_MFG_Date.toDate() : null;
                                    GoodsIssueItemLocation.EXP_Date = !string.IsNullOrEmpty(itemBin.goodsReceive_EXP_Date) ? itemBin.goodsReceive_EXP_Date.toDate() : null;

                                    if (itemBin.binBalance_WeightBegin == 0)
                                    {
                                        GoodsIssueItemLocation.Weight = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitWeight = itemBin.binBalance_UnitWeightBal;
                                        GoodsIssueItemLocation.UnitWeight_Index = itemBin.binBalance_UnitWeightBal_Index;
                                        GoodsIssueItemLocation.UnitWeight_Id = itemBin.binBalance_UnitWeightBal_Id;
                                        GoodsIssueItemLocation.UnitWeight_Name = itemBin.binBalance_UnitWeightBal_Name;
                                        GoodsIssueItemLocation.UnitWeightRatio = itemBin.binBalance_UnitWeightBalRatio;

                                        GoodsIssueItemLocation.Weight = (Decimal)(QtyPick * (itemBin.binBalance_UnitWeightBal ?? 0));
                                        GoodsIssueItemLocation.Weight_Index = itemBin.binBalance_UnitWeightBal_Index;
                                        GoodsIssueItemLocation.Weight_Id = itemBin.binBalance_UnitWeightBal_Id;
                                        GoodsIssueItemLocation.Weight_Name = itemBin.binBalance_UnitWeightBal_Name;
                                        GoodsIssueItemLocation.WeightRatio = itemBin.binBalance_UnitWeightBalRatio;
                                    }

                                    if (itemBin.binBalance_NetWeightBegin == 0)
                                    {
                                        GoodsIssueItemLocation.NetWeight = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitNetWeight = itemBin.binBalance_UnitNetWeightBal;
                                        GoodsIssueItemLocation.UnitNetWeight_Index = itemBin.binBalance_UnitNetWeightBal_Index;
                                        GoodsIssueItemLocation.UnitNetWeight_Id = itemBin.binBalance_UnitNetWeightBal_Id;
                                        GoodsIssueItemLocation.UnitNetWeight_Name = itemBin.binBalance_UnitNetWeightBal_Name;
                                        GoodsIssueItemLocation.UnitNetWeightRatio = itemBin.binBalance_UnitNetWeightBalRatio;

                                        GoodsIssueItemLocation.NetWeight = (Decimal)(QtyPick * (itemBin.binBalance_UnitNetWeightBal ?? 0));
                                        GoodsIssueItemLocation.NetWeight_Index = itemBin.binBalance_UnitNetWeightBal_Index;
                                        GoodsIssueItemLocation.NetWeight_Id = itemBin.binBalance_UnitNetWeightBal_Id;
                                        GoodsIssueItemLocation.NetWeight_Name = itemBin.binBalance_UnitNetWeightBal_Name;
                                        GoodsIssueItemLocation.NetWeightRatio = itemBin.binBalance_UnitNetWeightBalRatio;
                                    }

                                    if (itemBin.binBalance_GrsWeightBegin == 0)
                                    {
                                        GoodsIssueItemLocation.GrsWeight = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitGrsWeight = itemBin.binBalance_UnitGrsWeightBal;
                                        GoodsIssueItemLocation.UnitGrsWeight_Index = itemBin.binBalance_UnitGrsWeightBal_Index;
                                        GoodsIssueItemLocation.UnitGrsWeight_Id = itemBin.binBalance_UnitGrsWeightBal_Id;
                                        GoodsIssueItemLocation.UnitGrsWeight_Name = itemBin.binBalance_UnitGrsWeightBal_Name;
                                        GoodsIssueItemLocation.UnitGrsWeightRatio = itemBin.binBalance_UnitGrsWeightBalRatio;

                                        GoodsIssueItemLocation.GrsWeight = (Decimal)(QtyPick * (itemBin.binBalance_UnitGrsWeightBal ?? 0));
                                        GoodsIssueItemLocation.GrsWeight_Index = itemBin.binBalance_UnitGrsWeightBal_Index;
                                        GoodsIssueItemLocation.GrsWeight_Id = itemBin.binBalance_UnitGrsWeightBal_Id;
                                        GoodsIssueItemLocation.GrsWeight_Name = itemBin.binBalance_UnitGrsWeightBal_Name;
                                        GoodsIssueItemLocation.GrsWeightRatio = itemBin.binBalance_UnitGrsWeightBalRatio;
                                    }

                                    if (itemBin.binBalance_WidthBegin == 0)
                                    {
                                        GoodsIssueItemLocation.Width = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitWidth = (itemBin.binBalance_UnitWidthBal ?? 0);
                                        GoodsIssueItemLocation.UnitWidth_Index = itemBin.binBalance_UnitWidthBal_Index;
                                        GoodsIssueItemLocation.UnitWidth_Id = itemBin.binBalance_UnitWidthBal_Id;
                                        GoodsIssueItemLocation.UnitWidth_Name = itemBin.binBalance_UnitWidthBal_Name;
                                        GoodsIssueItemLocation.UnitWidthRatio = itemBin.binBalance_UnitWidthBalRatio;

                                        GoodsIssueItemLocation.Width = (Decimal)(QtyPick * (itemBin.binBalance_UnitWidthBal ?? 0));
                                        GoodsIssueItemLocation.Width_Index = itemBin.binBalance_UnitWidthBal_Index;
                                        GoodsIssueItemLocation.Width_Id = itemBin.binBalance_UnitWidthBal_Id;
                                        GoodsIssueItemLocation.Width_Name = itemBin.binBalance_UnitWidthBal_Name;
                                        GoodsIssueItemLocation.WidthRatio = itemBin.binBalance_UnitWidthBalRatio;
                                    }

                                    if (itemBin.binBalance_LengthBegin == 0)
                                    {
                                        GoodsIssueItemLocation.Length = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitLength = (itemBin.binBalance_UnitLengthBal ?? 0);
                                        GoodsIssueItemLocation.UnitLength_Index = itemBin.binBalance_UnitLengthBal_Index;
                                        GoodsIssueItemLocation.UnitLength_Id = itemBin.binBalance_UnitLengthBal_Id;
                                        GoodsIssueItemLocation.UnitLength_Name = itemBin.binBalance_UnitLengthBal_Name;
                                        GoodsIssueItemLocation.UnitLengthRatio = itemBin.binBalance_UnitLengthBalRatio;

                                        GoodsIssueItemLocation.Length = (Decimal)(QtyPick * (itemBin.binBalance_UnitWidthBal ?? 0));
                                        GoodsIssueItemLocation.Length_Index = itemBin.binBalance_UnitLengthBal_Index;
                                        GoodsIssueItemLocation.Length_Id = itemBin.binBalance_UnitLengthBal_Id;
                                        GoodsIssueItemLocation.Length_Name = itemBin.binBalance_UnitLengthBal_Name;
                                        GoodsIssueItemLocation.LengthRatio = itemBin.binBalance_UnitLengthBalRatio;
                                    }

                                    if (itemBin.binBalance_HeightBegin == 0)
                                    {
                                        GoodsIssueItemLocation.Height = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitHeight = (itemBin.binBalance_UnitHeightBal ?? 0);
                                        GoodsIssueItemLocation.UnitHeight_Index = itemBin.binBalance_UnitHeightBal_Index;
                                        GoodsIssueItemLocation.UnitHeight_Id = itemBin.binBalance_UnitHeightBal_Id;
                                        GoodsIssueItemLocation.UnitHeight_Name = itemBin.binBalance_UnitHeightBal_Name;
                                        GoodsIssueItemLocation.UnitHeightRatio = itemBin.binBalance_UnitHeightBalRatio;

                                        GoodsIssueItemLocation.Height = (Decimal)(QtyPick * (itemBin.binBalance_UnitHeightBal ?? 0));
                                        GoodsIssueItemLocation.Height_Index = itemBin.binBalance_UnitHeightBal_Index;
                                        GoodsIssueItemLocation.Height_Id = itemBin.binBalance_UnitHeightBal_Id;
                                        GoodsIssueItemLocation.Height_Name = itemBin.binBalance_UnitHeightBal_Name;
                                        GoodsIssueItemLocation.HeightRatio = itemBin.binBalance_UnitHeightBalRatio;
                                    }

                                    if (itemBin.binBalance_UnitVolumeBal == 0)
                                    {
                                        GoodsIssueItemLocation.UnitVolume = 0;
                                    }
                                    else
                                    {
                                        GoodsIssueItemLocation.UnitVolume = (itemBin.binBalance_UnitVolumeBal ?? 0);
                                        GoodsIssueItemLocation.Volume = (Decimal)(QtyPick * (itemBin.binBalance_UnitVolumeBal ?? 0));
                                    }

                                    GoodsIssueItemLocation.UnitPrice = (itemBin.unitPrice ?? 0);
                                    GoodsIssueItemLocation.UnitPrice_Index = itemBin.unitPrice_Index;
                                    GoodsIssueItemLocation.UnitPrice_Id = itemBin.unitPrice_Id;
                                    GoodsIssueItemLocation.UnitPrice_Name = itemBin.unitPrice_Name;
                                    GoodsIssueItemLocation.Price = (Decimal)(QtyPick * (itemBin.unitPrice ?? 0));
                                    GoodsIssueItemLocation.Price_Index = itemBin.unitPrice_Index;
                                    GoodsIssueItemLocation.Price_Id = itemBin.unitPrice_Id;
                                    GoodsIssueItemLocation.Price_Name = itemBin.unitPrice_Name;



                                    GoodsIssueItemLocation.DocumentRef_No1 = itemPlanGI.DocumentRef_No1;
                                    GoodsIssueItemLocation.DocumentRef_No2 = itemPlanGI.DocumentRef_No2;
                                    GoodsIssueItemLocation.DocumentRef_No3 = itemPlanGI.DocumentRef_No3;
                                    GoodsIssueItemLocation.DocumentRef_No4 = itemPlanGI.DocumentRef_No4;
                                    GoodsIssueItemLocation.DocumentRef_No5 = itemPlanGI.DocumentRef_No5;
                                    GoodsIssueItemLocation.Document_Status = -2;
                                    GoodsIssueItemLocation.UDF_1 = itemPlanGI.UDF_1;
                                    GoodsIssueItemLocation.UDF_2 = itemPlanGI.UDF_2;
                                    GoodsIssueItemLocation.UDF_3 = itemPlanGI.UDF_3;
                                    GoodsIssueItemLocation.UDF_4 = itemPlanGI.UDF_4;
                                    GoodsIssueItemLocation.UDF_5 = itemPlanGI.UDF_5;
                                    GoodsIssueItemLocation.Ref_Process_Index = new Guid("22744590-55D8-4448-88EF-5997C252111F");  // PLAN GI Process
                                    GoodsIssueItemLocation.Ref_Document_No = itemPlanGI.PlanGoodsIssue_No;
                                    GoodsIssueItemLocation.Ref_Document_Index = (Guid)itemPlanGI.PlanGoodsIssue_Index;
                                    GoodsIssueItemLocation.Ref_DocumentItem_Index = itemPlanGI.PlanGoodsIssueItem_Index;
                                    GoodsIssueItemLocation.GoodsReceiveItem_Index = new Guid(itemBin.goodsReceiveItem_Index);
                                    GoodsIssueItemLocation.Create_By = model.create_by;
                                    GoodsIssueItemLocation.Create_Date = DateTime.Now;
                                    GoodsIssueItemLocation.GoodsIssue_No = model.goodsIssue_No;
                                    GoodsIssueItemLocation.BinBalance_Index = new Guid(itemBin.binBalance_Index);

                                    GoodsIssueItemLocation.Invoice_No = itemBin.invoice_No;
                                    GoodsIssueItemLocation.Declaration_No = itemBin.declaration_No;
                                    GoodsIssueItemLocation.HS_Code = itemBin.hs_Code;
                                    GoodsIssueItemLocation.Conutry_of_Origin = itemBin.conutry_of_Origin;
                                    GoodsIssueItemLocation.Tax1 = itemBin.tax1;
                                    GoodsIssueItemLocation.Tax1_Currency_Index = itemBin.tax1_Currency_Index;
                                    GoodsIssueItemLocation.Tax1_Currency_Id = itemBin.tax1_Currency_Id;
                                    GoodsIssueItemLocation.Tax1_Currency_Name = itemBin.tax1_Currency_Name;
                                    GoodsIssueItemLocation.Tax2 = itemBin.tax2;
                                    GoodsIssueItemLocation.Tax2_Currency_Index = itemBin.tax2_Currency_Index;
                                    GoodsIssueItemLocation.Tax2_Currency_Id = itemBin.tax2_Currency_Id;
                                    GoodsIssueItemLocation.Tax2_Currency_Name = itemBin.tax2_Currency_Name;
                                    GoodsIssueItemLocation.Tax3 = itemBin.tax3;
                                    GoodsIssueItemLocation.Tax3_Currency_Index = itemBin.tax3_Currency_Index;
                                    GoodsIssueItemLocation.Tax3_Currency_Id = itemBin.tax3_Currency_Id;
                                    GoodsIssueItemLocation.Tax3_Currency_Name = itemBin.tax3_Currency_Name;
                                    GoodsIssueItemLocation.Tax4 = itemBin.tax4;
                                    GoodsIssueItemLocation.Tax4_Currency_Index = itemBin.tax4_Currency_Index;
                                    GoodsIssueItemLocation.Tax4_Currency_Id = itemBin.tax4_Currency_Id;
                                    GoodsIssueItemLocation.Tax4_Currency_Name = itemBin.tax4_Currency_Name;
                                    GoodsIssueItemLocation.Tax5 = itemBin.tax5;
                                    GoodsIssueItemLocation.Tax5_Currency_Index = itemBin.tax5_Currency_Index;
                                    GoodsIssueItemLocation.Tax5_Currency_Id = itemBin.tax5_Currency_Id;
                                    GoodsIssueItemLocation.Tax5_Currency_Name = itemBin.tax5_Currency_Name;
                                    GoodsIssueItemLocation.ERP_Location = itemBin.erp_Location;

                                    chkdatawave = true;
                                    ListGoodsIssueItemLocation.Add(GoodsIssueItemLocation);
                                    
                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [GIL]   GoodsIssueItemLocation.TotalQty " + GoodsIssueItemLocation.TotalQty.ToString() + "    Ref_Document_No : " + GoodsIssueItemLocation.Ref_Document_No.ToString());
                                    
                                    itemPlanGI.GITotalQty = itemPlanGI.GITotalQty + QtyPick;
                                    
                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [ QtyPlanGIRemian - QtyPick ]  " + QtyPlanGIRemian.ToString() + "  - " + QtyPick.ToString());
                                    
                                    QtyPlanGIRemian = QtyPlanGIRemian - QtyPick;

                                    olog.logging("GIStock", " TAG_NO " + itemBin.tag_No + " : [END line QtyPlanGIRemian ]  " + QtyPlanGIRemian.ToString());
                                    
                                }
                            }

                            #endregion

                            olog.logging("GIStock", " -------------- ");

                            #region inset GIL and runwave status 30

                            using (var db4 = new GIDbContext())
                            {
                                db4.Database.SetCommandTimeout(120);
                                var transaction = db4.Database.BeginTransaction(IsolationLevel.Serializable);
                                try
                                {
                                    var GI = db4.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 20).ToList();
                                    foreach (var g in GI)
                                    {
                                        g.RunWave_Status = 30;
                                    }

                                    db4.IM_GoodsIssueItemLocation.AddRange(ListGoodsIssueItemLocation);
                                    db4.SaveChanges();
                                    transaction.Commit();
                                }
                                catch (Exception exy)
                                {
                                    msglog = State + " ex Rollback " + exy.Message.ToString();
                                    olog.logging("runwave", msglog);
                                    transaction.Rollback();
                                    throw exy;
                                }
                            }

                            #endregion

                            State = "inset GIL and runwave status 30";
                            olog.logging("runwave", State);

                            #region insert bincardreserve and runwave status 40

                            using (var Contact = new GIDbContext())
                            {
                                Contact.Database.SetCommandTimeout(360);

                                State = "Contact.IM_GoodsIssueItemLocation";
                                olog.logging("runwave", State);

                                var GIL = Contact.IM_GoodsIssueItemLocation.Where(c => c.Ref_Document_Index == itemPlanGI.PlanGoodsIssue_Index && c.Ref_DocumentItem_Index == itemPlanGI.PlanGoodsIssueItem_Index && c.Document_Status != -1).ToList();

                                #region for GIL >>>>  I_LOOP

                                foreach (var g in GIL)
                                {
                                  
                                    State = "insertBinCardReserve";

                                    olog.logging("runwave", State + " GIIL_Index" + g.GoodsIssueItemLocation_Index.ToString());

                                    var insertBinCardReserve = new PickbinbalanceFromGIViewModel();

                                    insertBinCardReserve.ref_Document_Index = g.GoodsIssue_Index.ToString();
                                    insertBinCardReserve.ref_DocumentItem_Index = g.GoodsIssueItemLocation_Index.ToString();
                                    insertBinCardReserve.goodsIssue_No = model.goodsIssue_No;
                                    insertBinCardReserve.process_Index = "22744590-55D8-4448-88EF-5997C252111F";
                                    insertBinCardReserve.create_By = model.create_by;
                                    insertBinCardReserve.pick = g.TotalQty;
                                    insertBinCardReserve.binbalance_Index = g.BinBalance_Index.ToString();
                                    insertBinCardReserve.wave_Index = "7efa474a-8ff4-439e-a5d7-8c844cb53b56";

                                    State = "insetBinRe";
                                    olog.logging("runwave", State);
                                    var insetBinRe = getinsertBinCardReserve(insertBinCardReserve);
                                    //var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("insertBinCardReserve"), insertBinCardReserve.sJson());
                                    if (insetBinRe.resultIsUse)
                                    {
                                        State = "resultIsUse";
                                        olog.logging("runwave", State);

                                        var transaction = Contact.Database.BeginTransaction(IsolationLevel.Serializable);
                                        try
                                        {
                                            var GI = Contact.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 30).ToList();
                                            foreach (var gg in GI)
                                            {
                                                gg.RunWave_Status = 40;
                                            }
                                            Contact.SaveChanges();
                                            transaction.Commit();
                                        }
                                        catch (Exception exy)
                                        {
                                            msglog = State + " ex Rollback " + exy.Message.ToString();
                                            olog.logging("runwave", msglog);
                                            transaction.Rollback();
                                            throw exy;
                                        }
                                    }
                                    else
                                    {
                                        State = "resultIsUse else";
                                        olog.logging("runwave", State);
                                        
                                        var transaction = Contact.Database.BeginTransaction(IsolationLevel.Serializable);
                                        try
                                        {

                                            //var GoodsIssueItemLocation_Index = new SqlParameter("@GoodsIssueItemLocation_Index", g.GoodsIssueItemLocation_Index);
                                            //var resultPickingplan = db.Database.ExecuteSqlCommand("EXEC sp_DeleteGIIL_Error @GoodsIssueItemLocation_Index", GoodsIssueItemLocation_Index);

                                            var GI = Contact.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && (c.RunWave_Status == 30 || c.RunWave_Status == 40)).ToList();
                                            foreach (var gg in GI)
                                            {
                                                gg.RunWave_Status = 30;
                                            }
                                            Contact.SaveChanges();
                                            transaction.Commit();
                                        }
                                        catch (Exception exy)
                                        {
                                            msglog = State + " ex Rollback " + exy.Message.ToString();
                                            olog.logging("runwave", msglog);
                                            transaction.Rollback();
                                            throw exy;
                                        }
                                        msglog = State + " ex Rollback " + "Insert BinCardReserve Error";
                                        olog.logging("runwave", msglog);
                                        result.resultMsg = "Insert BinCardReserve Error";
                                        result.resultIsUse = false;
                                        //return result;

                                    }
                                }

                                #endregion

                            }
                            #endregion
                            State = "insert bincardreserve and runwave status 40";
                            olog.logging("runwave", State);
                        }

                        #endregion

                        

                        #endregion

                        #region update isuse = '' and runwave 50

                        //strwhere.isActive = true;

                        //log_Waveprocress logsaveT = new log_Waveprocress();
                        //logsaveT.Waveprocress_Index = Guid.NewGuid();
                        //logsaveT.GoodsIssue_Index = Guid.Parse(model.goodsIssue_Index);
                        //logsaveT.GoodsIssue_No = model.goodsIssue_No;
                        //logsaveT.DocumentRef_No1 = "update isuse = '' and runwave 50";
                        //logsaveT.DocumentRef_No2 = "Update";
                        //logsaveT.Json = "";
                        //logsaveT.Create_By = model.create_by;
                        //logsaveT.Create_Date = DateTime.Now;
                        //dblog.log_Waveprocress.Add(logsaveT);

                        //var resultXX = db.Database.ExecuteSqlCommand("EXEC sp_Delete_isuse @Isuse ,@product_index ,@itemstatus_index ,@owner_index ", Isuse, product_index, itemstatus_index, owner_index);

                        #region update isuse = '' and runwave 50
                        strwhere.isActive = true;
                        State = "updateIsuseViewBinbalance ";
                        var updateIsuseViewBinbalance = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateIsuseViewBinbalance"), strwhere.sJson());
                        //PickbinbalanceFromGIViewModel data = new PickbinbalanceFromGIViewModel();
                        //data = strwhere;
                        //var updateIsuseViewBinbalance = insertBinCardReserve(data);
                        if (updateIsuseViewBinbalance)
                        {
                            using (var db5 = new GIDbContext())
                            {
                                db5.Database.SetCommandTimeout(120);
                                var transaction = db5.Database.BeginTransaction(IsolationLevel.Serializable);
                                try
                                {
                                    var GI = db5.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 40).ToList();
                                    foreach (var g in GI)
                                    {
                                        g.RunWave_Status = 50;
                                        g.GI_status = 1;
                                        g.TaskGI_status = 0;
                                        g.TagOut_status = 0;
                                        g.WCS_status = 0;
                                    }
                                    db5.SaveChanges();
                                    transaction.Commit();


                                   
                                }
                                catch (Exception exy)
                                {
                                    msglog = State + " ex Rollback " + exy.Message.ToString();
                                    olog.logging("runwave", msglog);
                                    transaction.Rollback();

                                    throw exy;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Update IsUse By Error");
                        }
                        #endregion

                        #endregion

                        State = "region update isuse = '' and runwave 50";
                        olog.logging("runwave", State);
                    }

                    #endregion

                }

                List<string> planGI = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.Document_Status == -2).GroupBy(c=>c.Ref_Document_No).Select(c=> c.Key).ToList();
                foreach (var item in planGI)
                {
                    var des = "กำลังจัด";
                    try
                    {

                        var resmodel = new
                        {
                            referenceNo = item,
                            status = 102,
                            statusAfter = 103,
                            statusBefore = 101,
                            statusDesc = des,
                            statusDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        SaveLogRequest(item, JsonConvert.SerializeObject(resmodel), des, 1, des, Guid.NewGuid());
                        var result_api = utils.SendDataApi<DemoCallbackResponseViewModel>(new AppSettingConfig().GetUrl("TMS_status"), JsonConvert.SerializeObject(resmodel));
                        SaveLogResponse(item, JsonConvert.SerializeObject(result_api), resmodel.statusDesc, 2, resmodel.statusDesc, Guid.NewGuid());
                    }
                    catch (Exception ex)
                    {
                        SaveLogResponse(item, JsonConvert.SerializeObject(ex.Message), des, -1, des, Guid.NewGuid());
                    }
                }

                #endregion

                result.goodsIssue_Index = model.goodsIssue_Index;
                result.goodsIssue_No = model.goodsIssue_No;

                result.resultIsUse = true;

                #region ------------------------------------------------

                //------------------------------------------------

                State = "Update GI Status = 2 : " + model.goodsIssue_No.ToString();
                olog.logging("runwave", State);

                string cmd = "";

                cmd = "  Update  im_GoodsIssue set   " +
                         "  GI_status = 2      " +
                         "  ,Complete_prepareWave = GETDATE()      " +
                         "  where GoodsIssue_Index = '" + result.goodsIssue_Index + "'  " +
                         "   and GI_status = 1 " +
                         "   and Document_Status = -2 ";

                try
                {
                    olog.logging("runwave", State + " " + cmd.ToString());
                    var GIRes = db.Database.ExecuteSqlCommand(cmd);

                }
                catch (Exception exTrans)
                {
                    msglog = State + " exTrans   GI_status = 2   " + exTrans.Message.ToString();
                    olog.logging("runwave", msglog);
                    olog.logging("runwave", "exTrans inner - " + exTrans.InnerException.Message.ToString());

                    throw exTrans;
                }


                //-----------------------------------------------

                #endregion

                State = "end for";
                olog.logging("runwave", State);

                #region Check retrun status PGII >>>> J_LOOP

                foreach (var itemPGII in model.listGoodsIssueItemViewModel)
                {
                    //J_LOOP++;
                    //log_Waveprocress logsaveR = new log_Waveprocress();
                    //logsaveR.Waveprocress_Index = Guid.NewGuid();
                    //logsaveR.GoodsIssue_Index = Guid.Parse(model.goodsIssue_Index);
                    //logsaveR.GoodsIssue_No = model.goodsIssue_No;
                    //logsaveR.DocumentRef_No1 = "listGoodsIssueItemViewModel J_LOOP";
                    //logsaveR.DocumentRef_No2 = "J : " + J_LOOP;
                    //logsaveR.Json = "";
                    //logsaveR.Create_By = model.create_by;
                    //logsaveR.Create_Date = DateTime.Now;
                    //dblog.log_Waveprocress.Add(logsaveR);


                    var chkGIL2 = db.IM_GoodsIssueItemLocation.Where(c => c.Ref_Document_Index == itemPGII.planGoodsIssue_Index && c.Ref_DocumentItem_Index == itemPGII.planGoodsIssueItem_Index && c.Document_Status != -1).ToList();
                    var sumqty = chkGIL2.Sum(s => s.TotalQty);
                    if (chkGIL2.Count == 0 || sumqty != itemPGII.totalQty)
                    {

                        var pgii = model.listGoodsIssueItemViewModel.Where(c => c.planGoodsIssueItem_Index == itemPGII.planGoodsIssueItem_Index).ToList();

                        #region for pgii >>>> K_LOOP

                        foreach (var resultpgii in pgii)
                        {
                            //K_LOOP++;
                            //log_Waveprocress logsaveS = new log_Waveprocress();
                            //logsaveS.Waveprocress_Index = Guid.NewGuid();
                            //logsaveS.GoodsIssue_Index = Guid.Parse(model.goodsIssue_Index);
                            //logsaveS.GoodsIssue_No = model.goodsIssue_No;
                            //logsaveS.DocumentRef_No1 = "RuleSourceList K_LOOP";
                            //logsaveS.DocumentRef_No2 = "J : " + J_LOOP + " __ K: " + K_LOOP;
                            //logsaveS.Json = "";
                            //logsaveS.Create_By = model.create_by;
                            //logsaveS.Create_Date = DateTime.Now;
                            //dblog.log_Waveprocress.Add(logsaveS);



                            resultpgii.qtyPlan = (resultpgii.totalQty - sumqty);
                            resultpgii.totalQty = (resultpgii.totalQty - sumqty);
                            listpgiinotinsert.Add(resultpgii);
                            CheckRunwavePast = true;
                            using (var updatepginotProduct = new GIDbContext())
                            {
                                var transactionresultpgii = updatepginotProduct.Database.BeginTransaction(IsolationLevel.Serializable);
                                try
                                {

                                    var updateresultpgii = updatepginotProduct.IM_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssueItem_Index == resultpgii.planGoodsIssueItem_Index && c.Document_Status == 1).ToList();
                                    foreach (var p in updateresultpgii)
                                    {
                                        p.Document_Status = 0;
                                    }
                                    updatepginotProduct.SaveChanges();
                                    transactionresultpgii.Commit();
                                }

                                catch (Exception exy)
                                {
                                    msglog = State + " ex Rollback " + exy.Message.ToString();
                                    olog.logging("UpdateUserAssign", msglog);
                                    transactionresultpgii.Rollback();
                                    throw exy;
                                }
                            }
                        }

                        #endregion

                    }
                }
                #endregion

               State = "Check retrun status PGII";
                olog.logging("runwave", State);

                #region update PI status 3 and runwave status 60
                using (var db5 = new GIDbContext())
                {
                    db5.Database.SetCommandTimeout(120);
                    var listPGI = new List<Guid>();
                    if (model.listGoodsIssueItemViewModel.Count > 0)
                    {
                        foreach (var item in model.listGoodsIssueItemViewModel)
                        {
                            if (item.planGoodsIssue_Index != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                            {
                                listPGI.Add(item.planGoodsIssue_Index);
                            }
                        }
                    }
                    foreach (var item in listPGI)
                    {
                        var pgii = db5.IM_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssue_Index == item && c.Document_Status == 0).Count();
                        if (pgii == 0)
                        {
                            var transaction = db5.Database.BeginTransaction(IsolationLevel.Serializable);
                            try
                            {
                                var pgi = db5.IM_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_Index == item).ToList();
                                foreach (var p in pgi)
                                {
                                    p.Document_Status = 3;

                                    var des = "กำลังจัด";
                                    try
                                    {

                                        var resmodel = new
                                        {
                                            referenceNo = p.PlanGoodsIssue_No,
                                            status = 102,
                                            statusAfter = 103,
                                            statusBefore = 101,
                                            statusDesc = des,
                                            statusDateTime = DateTime.Now
                                        };
                                        SaveLogRequest(p.PlanGoodsIssue_No, JsonConvert.SerializeObject(resmodel), des, 1, des, Guid.NewGuid());
                                        var result_api = utils.SendDataApi<DemoCallbackResponseViewModel>(new AppSettingConfig().GetUrl("TMS_status"), JsonConvert.SerializeObject(resmodel));
                                        SaveLogResponse(p.PlanGoodsIssue_No, JsonConvert.SerializeObject(result_api), resmodel.statusDesc, 2, resmodel.statusDesc, Guid.NewGuid());
                                    }
                                    catch (Exception ex)
                                    {
                                        SaveLogResponse(p.PlanGoodsIssue_No, JsonConvert.SerializeObject(ex.Message), des, -1, des, Guid.NewGuid());
                                    }
                                }
                                

                                db5.SaveChanges();
                                transaction.Commit();
                            }

                            catch (Exception exy)
                            {
                                msglog = State + " ex Rollback " + exy.Message.ToString();
                                olog.logging("runwave", msglog);
                                transaction.Rollback();
                                throw exy;
                            }
                        }
                    }
                }
                #endregion

                State = "update PI status 3 and runwave status 60";
                olog.logging("runwave", State);

                result.resultMsg = CheckRunwavePast ? chkdatawave ? "หยิบสินค้าได้บางส่วน" : "สินค้าไม่เพียงพอ" : "หยิบสินค้าสำเร็จ";
                result.pgii = listpgiinotinsert;
                return result;

            }
            catch (Exception ex)
            {
                #region update isuse = '' and runwave 50
                var strwhere = new getViewBinbalanceViewModel();
                strwhere.isuse = model.goodsIssue_Index;
                strwhere.isActive = true;
                var updateIsuseViewBinbalance = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateIsuseViewBinbalance"), strwhere.sJson());

                #endregion

                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("runwave", msglog);
                olog.logging("runwave", "ex inner - " + ex.InnerException.Message.ToString());

                var result = new actionResultRunWaveV2ViewModelViewModel();
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }
        }


        #endregion


        public string SaveLogRequest(string orderno, string json, string interfacename, int status, string txt, Guid logindex)
        {
            try
            {
                log_api_request l = new log_api_request();
                l.log_id = logindex;
                l.log_date = DateTime.Now;
                l.log_requestbody = json;
                l.log_absoluteuri = "";
                l.status = status;
                l.Interface_Name = interfacename;
                l.Status_Text = txt;
                l.File_Name = orderno;
                dblog.log_api_request.Add(l);
                dblog.SaveChanges();
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string SaveLogResponse(string orderno, string json, string interfacename, int status, string txt, Guid logindex)
        {
            try
            {
                log_api_reponse l = new log_api_reponse();
                l.log_id = logindex;
                l.log_date = DateTime.Now;
                l.log_reponsebody = json;
                l.log_absoluteuri = "";
                l.status = status;
                l.Interface_Name = interfacename;
                l.Status_Text = txt;
                l.File_Name = orderno;
                dblog.log_api_reponse.Add(l);

                //var d = db.log_api_request.Where(c => c.log_id == logindex).FirstOrDefault();
                //d.status = status;

                dblog.SaveChanges();
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        #region insertBinCardReserve new
        public actionResultPickbinbalanceFromGIViewModel getinsertBinCardReserve(PickbinbalanceFromGIViewModel model)
        {
            String State = "Start " + model.sJson();
            String msglog = "";
            var xx = "";
            var olog = new logtxt();
            try
            {
                olog.logging("insertBinCardReserve", State);


                var result = new actionResultPickbinbalanceFromGIViewModel();
                var BinCardReserve_Index = Guid.NewGuid();

                olog.logging("insertBinCardReserve", " ref_DocumentItem_Index  - " + model.ref_DocumentItem_Index.ToString());
                db.Database.SetCommandTimeout(360);
                var itemBinReserve = dbBinbalance.wm_BinCardReserve.Where(c => c.Ref_Document_Index == new Guid(model.ref_Document_Index) && c.Ref_DocumentItem_Index == new Guid(model.ref_DocumentItem_Index)).ToList();
                if (itemBinReserve.Count == 0)
                {

                    State = "itemBinReserve.Count == 0";
                    olog.logging("insertBinCardReserve", State + " - " + model.ref_DocumentItem_Index.ToString());

                    var itemBin = dbBinbalance.wm_BinBalance.Find(Guid.Parse(model.binbalance_Index));
                    var BinCardReserve = new wm_BinCardReserve();

                    BinCardReserve.BinCardReserve_Index = BinCardReserve_Index;
                    BinCardReserve.BinBalance_Index = itemBin.BinBalance_Index;
                    BinCardReserve.Process_Index = new Guid(model.process_Index);
                    BinCardReserve.GoodsReceive_Index = itemBin.GoodsReceive_Index;
                    BinCardReserve.GoodsReceive_No = itemBin.GoodsReceive_No;
                    BinCardReserve.GoodsReceive_Date = itemBin.GoodsReceive_Date;
                    BinCardReserve.GoodsReceiveItem_Index = itemBin.GoodsReceiveItem_Index;
                    BinCardReserve.TagItem_Index = itemBin.TagItem_Index;
                    BinCardReserve.Tag_Index = itemBin.Tag_Index;
                    BinCardReserve.Tag_No = itemBin.Tag_No;
                    BinCardReserve.Product_Index = itemBin.Product_Index;
                    BinCardReserve.Product_Id = itemBin.Product_Id;
                    BinCardReserve.Product_Name = itemBin.Product_Name;
                    BinCardReserve.Product_SecondName = itemBin.Product_SecondName;
                    BinCardReserve.Product_ThirdName = itemBin.Product_ThirdName;
                    BinCardReserve.Product_Lot = itemBin.Product_Lot;
                    BinCardReserve.ItemStatus_Index = itemBin.ItemStatus_Index;
                    BinCardReserve.ItemStatus_Id = itemBin.ItemStatus_Id;
                    BinCardReserve.ItemStatus_Name = itemBin.ItemStatus_Name;
                    BinCardReserve.MFG_Date = itemBin.GoodsReceive_MFG_Date;
                    BinCardReserve.EXP_Date = itemBin.GoodsReceive_EXP_Date;
                    BinCardReserve.ProductConversion_Index = itemBin.ProductConversion_Index;
                    BinCardReserve.ProductConversion_Id = itemBin.ProductConversion_Id;
                    BinCardReserve.ProductConversion_Name = itemBin.ProductConversion_Name;
                    BinCardReserve.Owner_Index = itemBin.Owner_Index;
                    BinCardReserve.Owner_Id = itemBin.Owner_Id;
                    BinCardReserve.Owner_Name = itemBin.Owner_Name;
                    BinCardReserve.Location_Index = itemBin.Location_Index;
                    BinCardReserve.Location_Id = itemBin.Location_Id;
                    BinCardReserve.Location_Name = itemBin.Location_Name;
                    BinCardReserve.BinCardReserve_QtyBal = model.pick;


                    BinCardReserve.BinCardReserve_UnitWeightBal = itemBin.BinBalance_UnitWeightBal;
                    BinCardReserve.BinCardReserve_UnitWeightBal_Index = itemBin.BinBalance_UnitWeightBal_Index;
                    BinCardReserve.BinCardReserve_UnitWeightBal_Id = itemBin.BinBalance_UnitWeightBal_Id;
                    BinCardReserve.BinCardReserve_UnitWeightBal_Name = itemBin.BinBalance_UnitWeightBal_Name;
                    BinCardReserve.BinCardReserve_UnitWeightBalRatio = itemBin.BinBalance_UnitWeightBalRatio;
                    BinCardReserve.BinCardReserve_WeightBal = model.pick * (itemBin.BinBalance_UnitWeightBal ?? 0);
                    BinCardReserve.BinCardReserve_WeightBal_Index = itemBin.BinBalance_WeightBal_Index;
                    BinCardReserve.BinCardReserve_WeightBal_Id = itemBin.BinBalance_WeightBal_Id;
                    BinCardReserve.BinCardReserve_WeightBal_Name = itemBin.BinBalance_WeightBal_Name;
                    BinCardReserve.BinCardReserve_WeightBalRatio = itemBin.BinBalance_WeightBalRatio;

                    BinCardReserve.BinCardReserve_UnitNetWeightBal = itemBin.BinBalance_UnitNetWeightBal;
                    BinCardReserve.BinCardReserve_UnitNetWeightBal_Index = itemBin.BinBalance_UnitNetWeightBal_Index;
                    BinCardReserve.BinCardReserve_UnitNetWeightBal_Id = itemBin.BinBalance_UnitNetWeightBal_Id;
                    BinCardReserve.BinCardReserve_UnitNetWeightBal_Name = itemBin.BinBalance_UnitNetWeightBal_Name;
                    BinCardReserve.BinCardReserve_UnitNetWeightBalRatio = itemBin.BinBalance_UnitNetWeightBalRatio;
                    BinCardReserve.BinCardReserve_NetWeightBal = model.pick * (itemBin.BinBalance_UnitNetWeightBal ?? 0);
                    BinCardReserve.BinCardReserve_NetWeightBal_Index = itemBin.BinBalance_NetWeightBal_Index;
                    BinCardReserve.BinCardReserve_NetWeightBal_Id = itemBin.BinBalance_NetWeightBal_Id;
                    BinCardReserve.BinCardReserve_NetWeightBal_Name = itemBin.BinBalance_NetWeightBal_Name;
                    BinCardReserve.BinCardReserve_NetWeightBalRatio = itemBin.BinBalance_NetWeightBalRatio;

                    BinCardReserve.BinCardReserve_UnitGrsWeightBal = itemBin.BinBalance_UnitGrsWeightBal;
                    BinCardReserve.BinCardReserve_UnitGrsWeightBal_Index = itemBin.BinBalance_UnitGrsWeightBal_Index;
                    BinCardReserve.BinCardReserve_UnitGrsWeightBal_Id = itemBin.BinBalance_UnitGrsWeightBal_Id;
                    BinCardReserve.BinCardReserve_UnitGrsWeightBal_Name = itemBin.BinBalance_UnitGrsWeightBal_Name;
                    BinCardReserve.BinCardReserve_UnitGrsWeightBalRatio = itemBin.BinBalance_UnitGrsWeightBalRatio;
                    BinCardReserve.BinCardReserve_GrsWeightBal = model.pick * (itemBin.BinBalance_UnitGrsWeightBal ?? 0);
                    BinCardReserve.BinCardReserve_GrsWeightBal_Index = itemBin.BinBalance_GrsWeightBal_Index;
                    BinCardReserve.BinCardReserve_GrsWeightBal_Id = itemBin.BinBalance_GrsWeightBal_Id;
                    BinCardReserve.BinCardReserve_GrsWeightBal_Name = itemBin.BinBalance_GrsWeightBal_Name;
                    BinCardReserve.BinCardReserve_GrsWeightBalRatio = itemBin.BinBalance_GrsWeightBalRatio;

                    BinCardReserve.BinCardReserve_UnitWidthBal = itemBin.BinBalance_UnitWidthBal;
                    BinCardReserve.BinCardReserve_UnitWidthBal_Index = itemBin.BinBalance_UnitWidthBal_Index;
                    BinCardReserve.BinCardReserve_UnitWidthBal_Id = itemBin.BinBalance_UnitWidthBal_Id;
                    BinCardReserve.BinCardReserve_UnitWidthBal_Name = itemBin.BinBalance_UnitWidthBal_Name;
                    BinCardReserve.BinCardReserve_UnitWidthBalRatio = itemBin.BinBalance_UnitWidthBalRatio;
                    BinCardReserve.BinCardReserve_WidthBal = model.pick * (itemBin.BinBalance_UnitWidthBal ?? 0);
                    BinCardReserve.BinCardReserve_WidthBal_Index = itemBin.BinBalance_WidthBal_Index;
                    BinCardReserve.BinCardReserve_WidthBal_Id = itemBin.BinBalance_WidthBal_Id;
                    BinCardReserve.BinCardReserve_WidthBal_Name = itemBin.BinBalance_WidthBal_Name;
                    BinCardReserve.BinCardReserve_WidthBalRatio = itemBin.BinBalance_WidthBalRatio;

                    BinCardReserve.BinCardReserve_UnitLengthBal = itemBin.BinBalance_UnitLengthBal;
                    BinCardReserve.BinCardReserve_UnitLengthBal_Index = itemBin.BinBalance_UnitLengthBal_Index;
                    BinCardReserve.BinCardReserve_UnitLengthBal_Id = itemBin.BinBalance_UnitLengthBal_Id;
                    BinCardReserve.BinCardReserve_UnitLengthBal_Name = itemBin.BinBalance_UnitLengthBal_Name;
                    BinCardReserve.BinCardReserve_UnitLengthBalRatio = itemBin.BinBalance_UnitLengthBalRatio;
                    BinCardReserve.BinCardReserve_LengthBal = model.pick * (itemBin.BinBalance_UnitLengthBal ?? 0);
                    BinCardReserve.BinCardReserve_LengthBal_Index = itemBin.BinBalance_LengthBal_Index;
                    BinCardReserve.BinCardReserve_LengthBal_Id = itemBin.BinBalance_LengthBal_Id;
                    BinCardReserve.BinCardReserve_LengthBal_Name = itemBin.BinBalance_LengthBal_Name;
                    BinCardReserve.BinCardReserve_LengthBalRatio = itemBin.BinBalance_LengthBalRatio;

                    BinCardReserve.BinCardReserve_UnitHeightBal = itemBin.BinBalance_UnitHeightBal;
                    BinCardReserve.BinCardReserve_UnitHeightBal_Index = itemBin.BinBalance_UnitHeightBal_Index;
                    BinCardReserve.BinCardReserve_UnitHeightBal_Id = itemBin.BinBalance_UnitHeightBal_Id;
                    BinCardReserve.BinCardReserve_UnitHeightBal_Name = itemBin.BinBalance_UnitHeightBal_Name;
                    BinCardReserve.BinCardReserve_UnitHeightBalRatio = itemBin.BinBalance_UnitHeightBalRatio;
                    BinCardReserve.BinCardReserve_HeightBal = model.pick * (itemBin.BinBalance_UnitHeightBal ?? 0);
                    BinCardReserve.BinCardReserve_HeightBal_Index = itemBin.BinBalance_HeightBal_Index;
                    BinCardReserve.BinCardReserve_HeightBal_Id = itemBin.BinBalance_HeightBal_Id;
                    BinCardReserve.BinCardReserve_HeightBal_Name = itemBin.BinBalance_HeightBal_Name;
                    BinCardReserve.BinCardReserve_HeightBalRatio = itemBin.BinBalance_HeightBalRatio;

                    BinCardReserve.BinCardReserve_UnitVolumeBal = itemBin.BinBalance_UnitVolumeBal;
                    BinCardReserve.BinCardReserve_VolumeBal = model.pick * (itemBin.BinBalance_UnitVolumeBal ?? 0);

                    BinCardReserve.UnitPrice = itemBin.UnitPrice;
                    BinCardReserve.UnitPrice_Index = itemBin.UnitPrice_Index;
                    BinCardReserve.UnitPrice_Id = itemBin.UnitPrice_Id;
                    BinCardReserve.UnitPrice_Name = itemBin.UnitPrice_Name;
                    BinCardReserve.Price = model.pick * (itemBin.UnitPrice ?? 0);
                    BinCardReserve.Price_Index = itemBin.UnitPrice_Index;
                    BinCardReserve.Price_Id = itemBin.UnitPrice_Id;
                    BinCardReserve.Price_Name = itemBin.UnitPrice_Name;



                    BinCardReserve.Ref_Document_Index = Guid.Parse(model.ref_Document_Index);
                    BinCardReserve.Ref_DocumentItem_Index = Guid.Parse(model.ref_DocumentItem_Index);
                    BinCardReserve.Ref_Document_No = model.goodsIssue_No;
                    BinCardReserve.Ref_Wave_Index = model.wave_Index;

                    BinCardReserve.Create_By = model.create_By;
                    BinCardReserve.Create_Date = DateTime.Now;


                    dbBinbalance.wm_BinCardReserve.Add(BinCardReserve);

                    itemBin.BinBalance_QtyReserve = itemBin.BinBalance_QtyReserve + model.pick;

                    if (itemBin.BinBalance_WeightBegin != 0)
                    {
                        var WeightReserve = (model.pick * itemBin.BinBalance_UnitWeightBal);

                        itemBin.BinBalance_WeightReserve = itemBin.BinBalance_WeightReserve + WeightReserve;
                    }

                    if (itemBin.BinBalance_NetWeightBegin != 0)
                    {
                        var NetWeightReserve = (model.pick * itemBin.BinBalance_UnitNetWeightBal);
                        itemBin.BinBalance_NetWeightReserve = itemBin.BinBalance_NetWeightReserve + NetWeightReserve;
                    }


                    if (itemBin.BinBalance_GrsWeightBegin != 0)
                    {
                        var GrsWeightReserve = (model.pick * itemBin.BinBalance_UnitGrsWeightBal);
                        itemBin.BinBalance_GrsWeightReserve = itemBin.BinBalance_GrsWeightReserve + GrsWeightReserve;
                    }


                    if (itemBin.BinBalance_WidthBegin != 0)
                    {
                        var WidthReserve = (model.pick * itemBin.BinBalance_UnitWidthBal);
                        itemBin.BinBalance_WidthReserve = itemBin.BinBalance_WidthReserve + WidthReserve;
                    }


                    if (itemBin.BinBalance_LengthBegin != 0)
                    {
                        var LengthReserve = (model.pick * itemBin.BinBalance_UnitLengthBal);
                        itemBin.BinBalance_LengthReserve = itemBin.BinBalance_LengthReserve + LengthReserve;

                    }


                    if (itemBin.BinBalance_HeightBegin != 0)
                    {
                        var HeightReserve = (model.pick * itemBin.BinBalance_UnitHeightBal);
                        itemBin.BinBalance_HeightReserve = itemBin.BinBalance_HeightReserve + HeightReserve;
                    }

                    if (itemBin.BinBalance_VolumeBegin != 0)
                    {
                        var VolReserve = (model.pick * itemBin.BinBalance_UnitVolumeBal);
                        itemBin.BinBalance_VolumeReserve = itemBin.BinBalance_VolumeReserve + VolReserve;
                    }


                    if ((itemBin.UnitPrice ?? 0) != 0)
                    {
                        var VoltPrice = (model.pick * itemBin.UnitPrice);
                        itemBin.Price = itemBin.Price - VoltPrice;
                    }

                    State = "s.SaveChanges";

                    olog.logging("insertBinCardReserve", State + " - " + model.ref_DocumentItem_Index.ToString());


                    State = "s.SaveChanges";
                    olog.logging("insertBinCardReserve", State + " - " + model.ref_DocumentItem_Index.ToString());


                    var transactionx = dbBinbalance.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        dbBinbalance.SaveChanges();
                        transactionx.Commit();

                        State = "e.SaveChanges";
                        olog.logging("insertBinCardReserve", State + " - " + model.ref_DocumentItem_Index.ToString());


                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("insertBinCardReserve", msglog);
                        transactionx.Rollback();

                        throw exy;

                    }
                }
                else
                {
                    result.resultMsg = "สินค้าไม่พอ กรุณาลองใหม่อีกครั้ง";
                    result.resultIsUse = false;
                    return result;
                }

                model.binCardReserve_Index = BinCardReserve_Index.ToString();
                result.items = model;
                result.resultMsg = "รับสินค้าเรียบร้อยแล้ว";
                result.resultIsUse = true;
                return result;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("insertBinCardReserve", xx);
                var result = new actionResultPickbinbalanceFromGIViewModel();
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }

        }
        #endregion

        #endregion


    }
}
