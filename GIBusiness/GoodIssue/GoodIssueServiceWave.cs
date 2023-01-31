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
using System.IO;
using System.Net;
namespace GIBusiness.GoodIssue
{
    public class GoodIssueServiceWave
    {
        private GIDbContext db;

        public GoodIssueServiceWave()
        {
            db = new GIDbContext();
        }
        public GoodIssueServiceWave(GIDbContext db)
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


        public Boolean LineNotify(string msg)
        {
            string token = "ozCYzfKZZyABflNSDsg0k4WTvWnhmWkhmVRCiKBOM2y";
            String State = "Start ";
            var olog = new logtxt();


    

            try
            {
                olog.logging("LineNoti", State);
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                olog.logging("LineNoti", msg);
                return true;

            }
            catch (Exception ex)
            {
                return false;

                olog.logging("LineNoti", ex.ToString());

            }
            return true;
        }

        public actionResultRunWaveV2ViewModelViewModel runwaveandHeaderThread0( RunWaveFilterV2ViewModel model, int threadnum)
        {

            String State = "Start " + threadnum.ToString();
            String msglog = "";
            bool chkdatawave = false;
            var olog = new logtxt();
            var process = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
            var strprocess = "2E026669-99BD-4DE0-8818-534F29F7B89D";
            var PlanGiRunWave = new List<Guid>();
            var ListPlanGi_Index = new List<Guid>();
            var listpgiinotinsert = new List<plangoodsissueitemViewModel>();
            bool CheckRunwavePast = false;
            long IsPA = 1;
            Boolean IsTote = false;
            olog.logging("runwave" + threadnum.ToString(), State);



            var dbThread = new GIDbContext();
            try
            {
                var listDataProduct2 = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), new { }.sJson());
                var listDataLocation2 = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("getLocationMaster"), new { }.sJson());


                #region create and update header
                dbThread.Database.SetCommandTimeout(360);
                Guid gi_index = !string.IsNullOrEmpty(model.goodsIssue_Index) ? new Guid(model.goodsIssue_Index) : new Guid("00000000-0000-0000-0000-000000000000");
                var gi = dbThread.IM_GoodsIssue.Find(gi_index);
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
                    dbThread.IM_GoodsIssue.Add(newGI);

                    model.goodsIssue_Index = newGI.GoodsIssue_Index.ToString();
                    model.goodsIssue_No = newGI.GoodsIssue_No;
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
                }

                var transactionx = dbThread.Database.BeginTransaction();
                try
                {
                    dbThread.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SavePlanGR", msglog);
                    transactionx.Rollback();

                    throw exy;

                }
                #endregion


                //int CheckRunwavePast = model.listGoodsIssueItemViewModel.Count();
                var result = new actionResultRunWaveV2ViewModelViewModel();

                dbThread.Database.SetCommandTimeout(360);


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




                //var planGI_Lot = dbThread.View_WaveCheckProductLot.Where(c => ListPlanGi_Index.Contains(c.PlanGoodsIssue_Index)).Select(s => new
                //{
                //    s.Product_Index
                //    ,s.Product_Id
                //    ,s.Product_Lot
                //});



                using (var db2 = new GIDbContext())
                {
                    var transaction = db2.Database.BeginTransaction();
                    try
                    {
                        //update status 10
                        var pgi = db2.IM_PlanGoodsIssueItem.Where(c => PlanGiRunWave.Contains(c.PlanGoodsIssueItem_Index) && c.Document_Status == 0).ToList();
                        foreach (var p in pgi)
                        {
                            //p.Ref_WavePick_index = new Guid(model.goodsIssue_Index);
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
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("runwave" + threadnum.ToString(), msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }
                #endregion



                var planGIResultx = dbThread.View_PLANWAVEV.Where(c => PlanGiRunWave.Contains(c.PlanGoodsIssueItem_Index) && c.ThreadNum == threadnum)
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
                        //g.sum(TotalQty) as TotalQty,
                        //g.isnull(sum(GITotalQty), 0) as GITotalQty,
                        //g.sum(TotalQty) - isnull(sum(GITotalQty), 0)  AS QtyWave,
                        //g.ROW_NUMBER() OVER(Order by Product_Id) as RowNum,
                        g.Owner_Index,
                        g.PlanGoodsIssue_UDF_1,
                        g.PlanGoodsIssue_UDF_2,
                        g.PlanGoodsIssue_UDF_3,
                        g.PlanGoodsIssue_UDF_4,
                        g.PlanGoodsIssue_UDF_5,
                        //  g.ERP_Location
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
                        //g.ROW_NUMBER() OVER(Order by Product_Id) as RowNum,
                        s.Key.Owner_Index,
                        s.Key.PlanGoodsIssue_UDF_1,
                        s.Key.PlanGoodsIssue_UDF_2,
                        s.Key.PlanGoodsIssue_UDF_3,
                        s.Key.PlanGoodsIssue_UDF_4,
                        s.Key.PlanGoodsIssue_UDF_5,
                        // s.Key.ERP_Location
                    }).ToList();

                State = "View_PLANWAVEV";
                olog.logging("runwave" + threadnum.ToString(), State);
                if (planGIResultx.Count == 0)
                {
                    throw new Exception("Plan GI not found..");
                }


                //find wave template
                var jsGetWaveRule = new { process_Index = process, wave_Index = model.wave_Index };
                var getWaveRule = utils.SendDataApi<List<WaveRuleViewModel>>(new AppSettingConfig().GetUrl("getWaveRule"), jsGetWaveRule.sJson());

                if (getWaveRule.Count == 0)
                {
                    throw new Exception("Wave Template not found.");
                }

                var getViewWaveTemplateEX = utils.SendDataApi<List<WaveTemplateViewModel>>(new AppSettingConfig().GetUrl("getViewWaveTemplate"), new { }.sJson());

                State = "getWaveRule";
                olog.logging("runwave" + threadnum.ToString(), State);
                bool isUseAttribute = false;

                foreach (var waveRule in getWaveRule.OrderBy(o => o.waveRule_Seq))
                {
                    var jsgetViewWaveTemplate = new { process_Index = process, wave_Index = model.wave_Index, rule_Index = waveRule.rule_Index };
                    //var getViewWaveTemplate = utils.SendDataApi<List<WaveTemplateViewModel>>(new AppSettingConfig().GetUrl("getViewWaveTemplate"), jsgetViewWaveTemplate.sJson());
                    var getViewWaveTemplate = getViewWaveTemplateEX.Where(c => c.process_Index == process.ToString() && c.wave_Index == model.wave_Index && c.rule_Index == waveRule.rule_Index);
                    State = "getViewWaveTemplate";
                    olog.logging("runwave" + threadnum.ToString(), State);
                    var planGIWaveResult = dbThread.View_PLANWAVEbyPLANGIV2.AsQueryable();
                    var check = planGIWaveResult.ToList();
                    planGIWaveResult = planGIWaveResult.Where(c => PlanGiRunWave.Contains(c.PlanGoodsIssueItem_Index));

                    #region for RuleSource
                    var RuleSourceList = getViewWaveTemplate.Where(c => c.isSource == 1 && c.isSearch == 1).ToList();

                    foreach (var itemRuleSourceList in RuleSourceList)
                    {
                        if (itemRuleSourceList.ruleConditionOperation == "IN")
                        {
                            var dataarray = itemRuleSourceList.ruleCondition_Param.Replace("'", "").Split(',');
                            if (itemRuleSourceList.ruleConditionField_Name == "DocumentType_Id")
                            {
                                planGIWaveResult = planGIWaveResult.Where(c => dataarray.Contains(c.DocumentType_Id));
                            }
                            if (itemRuleSourceList.ruleConditionField_Name == "Owner_Id")
                            {
                                planGIWaveResult = planGIWaveResult.Where(c => dataarray.Contains(c.Owner_Id));
                            }
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " IN (" + itemRuleSourceList.ruleCondition_Param + ") ";
                            //var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            //var predicate = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>(
                            //    Expression.Call(
                            //        Expression.PropertyOrField(param, itemRuleSourceList.ruleConditionField_Name),
                            //        "Contains", null, Expression.Constant(itemRuleSourceList.ruleCondition_Param)
                            //    ), param);
                            //planGIWaveResult = planGIWaveResult.Where(predicate);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "=")
                        {
                            if (itemRuleSourceList.ruleConditionField_Name == "UseAttribute")
                            {
                                if (itemRuleSourceList.ruleCondition_Param.ToString().ToUpper() == "TRUE")
                                {
                                    isUseAttribute = true;
                                }
                                else
                                {
                                    isUseAttribute = false;
                                }

                            }
                            else
                            {
                                //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " = '" + itemRuleSourceList.ruleCondition_Param + "' ";
                                var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                                var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.Equal(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                    , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                planGIWaveResult = planGIWaveResult.Where(lambda);
                            }


                        }
                        else if (itemRuleSourceList.ruleConditionOperation == ">")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " > '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.GreaterThan(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);

                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "<")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " < '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.LessThan(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "!=")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " != '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.NotEqual(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == ">=")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " >= '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.GreaterThanOrEqual(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "<=")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " <= '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.LessThanOrEqual(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "Like")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " Like '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var predicate = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>(
                                Expression.Call(
                                    Expression.PropertyOrField(param, itemRuleSourceList.ruleConditionField_Name),
                                    "Contains", null, Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""))
                                ), param);
                            planGIWaveResult = planGIWaveResult.Where(predicate);
                        }
                    }
                    #endregion

                    #region for RuleDesSort
                    var RuleSourceSortList = getViewWaveTemplate.Where(c => c.isSource == 1 && c.isSort == 1).ToList();

                    string setWhereSourceSort = "";

                    if (RuleSourceSortList.Count > 0)
                    {
                        setWhereSourceSort += " Order by ";
                    }

                    int iRowsSourceSort = 0;
                    foreach (var itemRuleSourceSortList in RuleSourceSortList)
                    {
                        if (iRowsSourceSort == 0)
                        {
                            setWhereSourceSort += itemRuleSourceSortList.ruleConditionField_Name + ' ' + itemRuleSourceSortList.ruleCondition_Param;
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, string>>(Expression.Property(param, itemRuleSourceSortList.ruleConditionField_Name), param);
                            if (itemRuleSourceSortList.ruleCondition_Param.ToUpper() == "DESC")
                            {
                                planGIWaveResult = planGIWaveResult.OrderByDescending(lambda);
                            }
                            else /*if (itemRuleSourceSortList.ruleCondition_Param.ToUpper() == "DESC")*/
                            {
                                planGIWaveResult = planGIWaveResult.OrderBy(lambda);
                            }
                        }
                        else
                        {
                            setWhereSourceSort += "," + itemRuleSourceSortList.ruleConditionField_Name + ' ' + itemRuleSourceSortList.ruleCondition_Param;
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, string>>(Expression.Property(param, itemRuleSourceSortList.ruleConditionField_Name), param);
                            if (itemRuleSourceSortList.ruleCondition_Param.ToUpper() == "DESC")
                            {
                                planGIWaveResult = planGIWaveResult.OrderByDescending(lambda);
                            }
                            else /*if (itemRuleSourceSortList.ruleCondition_Param.ToUpper() == "DESC")*/
                            {
                                planGIWaveResult = planGIWaveResult.OrderBy(lambda);
                            }
                        }
                        iRowsSourceSort = iRowsSourceSort + 1;

                    }
                    #endregion


                    String SqlWhere = "";
                    var planGIWaveResult2 = planGIWaveResult.ToList();
                    State = "View_PLANWAVEbyPLANGIV2";
                    olog.logging("runwave" + threadnum.ToString(), State);



                    // LOOP Group SUM PLAN GI
                    foreach (var item in planGIResultx)
                    {

                        olog.logging("runwave" + threadnum.ToString(), "planGIResultx : " + item.Product_Id.ToString());


                        if (item.Product_Id.ToString() == "C1004685WORD")
                        {
                            var aa = item.DocumentRef_No1;

                        }
                        var planGIWaveResult3 = planGIWaveResult2.AsQueryable();
                        var strwhere = new getViewBinbalanceViewModel();
                        //GET Condition  From Plan GI
                        #region query Plag Gi
                        if (item.Owner_Index.ToString() != "")
                        {
                            //SqlWhere += " And Convert(Nvarchar(200) ,Owner_Index) =  '" + item.Owner_Index.ToString() + "' ";
                            //SqlWhere += " And Owner_Index =  '" + item.Owner_Index + "' ";
                            strwhere.Owner_Index = item.Owner_Index;
                            planGIWaveResult3 = planGIWaveResult3.Where(c => c.Owner_Index == item.Owner_Index);
                        }
                        if (item.Product_Index.ToString() != "")
                        {
                            //SqlWhere += " And Convert(Nvarchar(200) , Product_Index ) = '" + item.Product_Index.ToString() + "' ";
                            //SqlWhere += " And Product_Index  = '" + item.Product_Index + "' ";
                            strwhere.Product_Index = item.Product_Index;
                            planGIWaveResult3 = planGIWaveResult3.Where(c => c.Product_Index == item.Product_Index);
                        }

                        //if (item.Product_Lot != null)
                        //{
                        //    if (item.Product_Lot.ToString() != "")
                        //    {
                        //        //SqlWhere += " And Product_Lot = '" + item.Product_Lot.ToString() + "' ";
                        //        strwhere.Product_Lot = item.Product_Lot;
                        //        planGIWaveResult3 = planGIWaveResult3.Where(c => c.Product_Lot == item.Product_Lot);
                        //    }
                        //}
                        if (item.ItemStatus_Index.ToString() != "")
                        {
                            //SqlWhere += " And Convert(Nvarchar(200) ,ItemStatus_Index) =  '" + item.ItemStatus_Index.ToString() + "' ";
                            //SqlWhere += " And ItemStatus_Index =  '" + item.ItemStatus_Index + "' ";
                            strwhere.ItemStatus_Index = item.ItemStatus_Index;
                            planGIWaveResult3 = planGIWaveResult3.Where(c => c.ItemStatus_Index == item.ItemStatus_Index);
                        }
                        if (item.MFG_Date != null)
                        {
                            if (item.MFG_Date.ToString() != "")
                            {
                                //strwhere.MFG_Date = item.MFG_Date;
                                // SqlWhere += " And MFG_Date = @MFG_Date ";
                            }
                        }
                        if (item.EXP_Date != null)
                        {
                            if (item.EXP_Date.ToString() != "")
                            {
                                //strwhere.EXP_Date = item.EXP_Date;
                                //SqlWhere += " And EXP_Date = @EXP_Date ";
                            }
                        }

                        if (item.Product_Lot != null)
                        {
                            if (item.Product_Lot.ToString() != "")
                            {
                                //strwhere.EXP_Date = item.EXP_Date;
                                strwhere.Product_Lot = item.Product_Lot;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.Product_Lot == item.Product_Lot);
                            }
                        }

                        //if (item.ERP_Location != null)
                        //{
                        //    if (item.ERP_Location.ToString() != "")
                        //    {
                        //        //strwhere.EXP_Date = item.EXP_Date;
                        //        strwhere.ERP_Location = item.ERP_Location;
                        //        planGIWaveResult3 = planGIWaveResult3.Where(c => c.ERP_Location == item.ERP_Location);
                        //    }
                        //}

                        //if (item.UDF_1 != null)
                        //{
                        //    //SqlWhere += " And Isnull(UDF_1,'') = '" + item.UDF_1.ToString() + "'";
                        //    strwhere.UDF_1 = item.UDF_1;
                        //    planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_1 == item.UDF_1);
                        //}

                        //if (item.UDF_2 != null)
                        //{
                        //    //SqlWhere += " And  Isnull( UDF_2,'') = '" + item.UDF_2.ToString() + "'";
                        //    strwhere.UDF_2 = item.UDF_2;
                        //    planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_2 == item.UDF_2);
                        //}

                        if (isUseAttribute == true)
                        {
                            // ADD UDF 1-5 
                            strwhere.isUseAttribute = isUseAttribute;
                            //if (item.UDF_1 != null)
                            //{
                            //    //SqlWhere += " And Isnull(UDF_1,'') = '" + item.UDF_1.ToString() + "'";
                            //    strwhere.UDF_1 = item.UDF_1;
                            //    planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_1 == item.UDF_1);
                            //}

                            //if (item.UDF_2 != null)
                            //{
                            //    //SqlWhere += " And  Isnull( UDF_2,'') = '" + item.UDF_2.ToString() + "'";
                            //    strwhere.UDF_2 = item.UDF_2;
                            //    planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_2 == item.UDF_2);
                            //}

                            if (item.UDF_3 != null)
                            {
                                //SqlWhere += " And  Isnull(UDF_3,'') = '" + item.UDF_3.ToString() + "'";
                                strwhere.UDF_3 = item.UDF_3;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_3 == item.UDF_3);
                            }

                            if (item.UDF_4 != null)
                            {
                                //SqlWhere += " And  Isnull(UDF_4,'') = '" + item.UDF_4.ToString() + "'";
                                strwhere.UDF_4 = item.UDF_4;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_4 == item.UDF_4);
                            }

                            if (item.UDF_5 != null)
                            {
                                //SqlWhere += " And  Isnull(UDF_5,'') = '" + item.UDF_5.ToString() + "'";
                                strwhere.UDF_5 = item.UDF_5;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_5 == item.UDF_5);
                            }
                        }
                        #endregion

                        //planGIWaveResult = planGIWaveResult.Where(setWhereSource);

                        if (planGIWaveResult3.OrderBy(c => c.LineNum).ToList().Count < 1)
                        {
                            continue;
                        }
                        #region update isuse and runwave status 20
                        strwhere.isuse = model.goodsIssue_Index;
                        var listDataBinbalance = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateIsuseViewBinbalance"), strwhere.sJson());
                        if (listDataBinbalance)
                        {
                            using (var db3 = new GIDbContext())
                            {
                                var transaction = db3.Database.BeginTransaction();
                                try
                                {
                                    var GI = db3.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 10).ToList();
                                    foreach (var g in GI)
                                    {
                                        g.RunWave_Status = 20;
                                    }
                                    db3.SaveChanges();
                                    transaction.Commit();
                                }
                                catch (Exception exy)
                                {
                                    msglog = State + " ex Rollback " + exy.Message.ToString();
                                    olog.logging("runwave" + threadnum.ToString(), msglog);
                                    transaction.Rollback();
                                    throw exy;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Update IsUse Error");
                        }
                        #endregion

                        // Assign Qty for  wave Loop
                        State = "planGIWaveResult3";
                        olog.logging("runwave" + threadnum.ToString(), State);
                        strwhere.isuse = model.goodsIssue_Index;
                        strwhere.isActive = true;
                        int? WhereQtyBal = null;
                        decimal? qty_Per_Tag = null;
                        qty_Per_Tag = listDataProduct2.FirstOrDefault(c => c.product_Id == planGIWaveResult3.FirstOrDefault().Product_Id).qty_Per_Tag;

                        //if (waveRule.rule_Name.ToUpper() == "FULL")
                        //{
                        //    strwhere.qtyPreTag = listDataProduct2.FirstOrDefault(c => c.product_Id == planGIWaveResult3.FirstOrDefault().Product_Id).qty_Per_Tag;
                        //    WhereQtyBal = Convert.ToInt16(Math.Floor((planGIWaveResult3.Sum(s => s.TotalQty) ?? 0) / (qty_Per_Tag ?? 1)));
                        //    var chkFull = CraterGILBy_Binbalance(WhereQtyBal, qty_Per_Tag, strwhere, listDataProduct2, model, getViewWaveTemplate.ToList(), planGIWaveResult3.ToList(), listDataLocation2);
                        //    #region update isuse = '' and runwave 50
                        //    strwhere.isActive = true;
                        //    State = "region update isuse = '' and runwave 50 1";
                        //    var updateIsuseViewBinbalance2 = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateIsuseViewBinbalance"), strwhere.sJson());
                        //    if (updateIsuseViewBinbalance2)
                        //    {
                        //        using (var db5 = new GIDbContext())
                        //        {
                        //            var transaction = db5.Database.BeginTransaction();
                        //            try
                        //            {
                        //                var GI = db5.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 40).ToList();
                        //                foreach (var g in GI)
                        //                {
                        //                    g.RunWave_Status = 50;
                        //                }
                        //                db5.SaveChanges();
                        //                transaction.Commit();
                        //            }
                        //            catch (Exception exy)
                        //            {
                        //                msglog = State + " ex Rollback " + exy.Message.ToString();
                        //                olog.logging("runwave" + threadnum.ToString(), msglog);
                        //                transaction.Rollback();
                        //                throw exy;
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        throw new Exception("Update IsUse By Error");
                        //    }
                        //    #endregion
                        //    continue;
                        //    //if (chkFull)
                        //    //{
                        //    //    continue;
                        //    //}
                        //    //else
                        //    //{
                        //    //    var resultFull = new actionResultRunWaveV2ViewModelViewModel();
                        //    //    resultFull.resultIsUse = false;
                        //    //    resultFull.resultMsg = "Error Full";
                        //    //    return result;
                        //    //}
                        //}

                        decimal? QtyPlanGIRemian = 0;
                        //     foreach (var itemPlanGI in planGIWaveResult3.OrderBy(c => c.LineNum).ThenBy(d => d.DocumentPriority_Status))
                        foreach (var itemPlanGI in planGIWaveResult3.OrderBy(c => c.LineNum))
                        {

                            if (itemPlanGI.ModPlanGI == 0)
                            {
                                IsPA = 0;

                            }
                            else
                            {
                                IsPA = 1;
                            }



                            //listDataProduct2



                            State = "listDataProduct2.ToList()";
                            olog.logging("runwave" + threadnum.ToString(), State);
                            var listProducttote = listDataProduct2.Where(c => c.product_Index == itemPlanGI.Product_Index).ToList();
                            if (listProducttote != null)
                            {
                                var checkProduct = listProducttote.Where(c => c.product_Index == itemPlanGI.Product_Index).FirstOrDefault();


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
                            //chkBinCardReserve = GIL

                            //}

                            #region view_waveBinbalance2
                            //strwhere.isuse = model.goodsIssue_Index;
                            //strwhere.isActive = true;
                            State = "getViewBinbalanceapi" + strwhere.sJson().ToString();
                            olog.logging("runwave" + threadnum.ToString(), State);

                            var listDataBinbalance2 = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getViewBinbalance"), strwhere.sJson());

                            var checklistDataBinbalance2 = listDataBinbalance2.ToList();

                            State = "CountgetViewBinbalance : " + checklistDataBinbalance2.Count().ToString(); ;
                            olog.logging("runwave" + threadnum.ToString(), State);



                            State = "View_WaveCheckProductLot";
                            olog.logging("runwave" + threadnum.ToString(), State);

                            var ListLot = new List<String>();
                            var ListLotNotWave = new List<String>();
                            // get All Product Lot Status not Wave by Product
                            var listAll_Lot = dbThread.View_WaveCheckProductLot.Where(c => c.Product_Index == itemPlanGI.Product_Index).ToList();

                            // get lot in plan gi amd product
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
                            olog.logging("runwave" + threadnum.ToString(), State);
                            var GIDate = model.goodsIssue_Date.toDate();
                            var View_WaveBinBalance2 = (from BB in listDataBinbalance2
                                                        join LC in listDataLocation2 on BB.location_Index equals LC.location_Index into gj
                                                        from L in gj.DefaultIfEmpty()
                                                        join Prd in listDataProduct2 on BB.product_Index equals Prd.product_Index
                                                        where (BB.goodsReceive_EXP_Date != null ? BB.goodsReceive_EXP_Date.sParse<DateTime>().Subtract(DateTime.Now.AddDays(-1)).Days : 1) > (Prd.ProductShelfLife_D ?? 0)
                                                        && !(L?.locationType_Index == Guid.Parse("14C5F85D-137D-470E-8C70-C1E535005DC3")
                                                            || L?.locationType_Index == Guid.Parse("2E9338D3-0931-4E36-B240-782BF9508641")
                                                            || L?.locationType_Index == Guid.Parse("65A2D25D-5520-47D3-8776-AE064D909285")
                                                            || L?.locationType_Index == Guid.Parse("94D86CEA-3D04-4304-9E97-28E954F03C35")
                                                            || L?.locationType_Index == Guid.Parse("64341969-E596-4B8B-8836-395061777490")
                                                            || L?.locationType_Index == Guid.Parse("6A1FB140-CC78-4C2B-BEC8-42B2D0AE62E9")
                                                            || L?.locationType_Index == Guid.Parse("F9EDDAEC-A893-4F63-A700-526C69CC0774")
                                                            || L?.locationType_Index == Guid.Parse("A1F7BFA0-1429-4010-863D-6A0EB01DB61D")
                                                            || L?.locationType_Index == Guid.Parse("472E5117-3A7A-4C23-B8C2-7FEA55B3E69C")
                                                            || L?.locationType_Index == Guid.Parse("7D30298A-8BA0-47ED-8342-E3F953E11D8C")
                                                            || L?.locationType_Index == Guid.Parse("A706D789-F5C9-41A6-BEC7-E57034DFC166")
                                                            || L?.locationType_Index == Guid.Parse("E4310B71-D6A7-4FF6-B4A8-EACBDFADAFFC")
                                                            || L?.locationType_Index == Guid.Parse("D4DFC92C-C5DC-4397-BF87-FEEEB579C0AF")
                                                            || L?.locationType_Index == Guid.Parse("3a7d807a-9f2c-4215-8703-f51846bcc4bd")
                                                            || L?.locationType_Index == Guid.Parse("DEA384FD-3EEF-49A2-A88C-04ABA5C114A7")
                                                            || L?.locationType_Index == Guid.Parse("8A545442-77A3-43A4-939A-6B9102DFE8C6")  // Replen
                                                            || L?.locationType_Index == Guid.Parse("1D2DF268-F004-4820-831F-B823FF9C7564")
                                                            || L?.locationType_Index == Guid.Parse("DEA384FD-3EEF-49A2-A88C-04ABA5C114A7")
                                                         //   || L?.locationType_Index == Guid.Parse("48F83BB5-7807-4B32-9E3C-74962CEF92E8") //LBL
                                                         //

                                                         //   || L?.locationType_Index == Guid.Parse("E77778D2-7A8E-448D-BA31-CD35FD938FC3")   // PA
                                                         //|| L?.locationType_Index == Guid.Parse("7F3E1BC2-F18B-4B16-80A9-2394EB8BBE63")   // VC
                                                         //|| L?.locationType_Index == Guid.Parse("E4F856EA-9685-45A4-995C-C05FF9E499C4")   // Partial

                                                         //

                                                         )


                                                        // && (waveRule.rule_Name.ToUpper() == "PATIAL" ? (BB.binBalance_QtyBal - BB.binBalance_QtyReserve) < qty_Per_Tag : BB.binBalance_QtyBal > 0)
                                                        && !(ListLotNotWave.Contains(BB.product_Lot))
                                                        //  && waveRule.rule_Name.ToUpper() == "PATIAL"
                                                        && (BB.binBalance_QtyBal) > 0
                                                        && (BB.binBalance_QtyReserve) >= 0
                                                        && (L?.BlockPick ?? 0) != 1
                                                        //  && (BB?.product_Lot ?? "") == itemPlanGI.Product_Lot
                                                        && BB.goodsReceive_Date.Date <= GIDate.Value.Date
                                                        && (string.IsNullOrEmpty(itemPlanGI.ERP_Location) ? (BB.erp_Location ?? "") == "" : BB.erp_Location == itemPlanGI.ERP_Location)
                                                        select new View_WaveBinBalanceViewModel
                                                        {
                                                            binBalance_Index = BB.binBalance_Index.ToString(),
                                                            owner_Index = BB.owner_Index.ToString(),
                                                            owner_Id = BB.owner_Id,
                                                            owner_Name = BB.owner_Name,
                                                            location_Index = BB.location_Index.ToString(),
                                                            location_Id = BB.location_Id,
                                                            location_Name = BB.location_Name,
                                                            goodsReceive_Index = BB.goodsReceive_Index.ToString(),
                                                            goodsReceive_No = BB.goodsReceive_No,
                                                            goodsReceive_Date = BB.goodsReceive_Date.ToString(),
                                                            goodsReceiveItem_Index = BB.goodsReceiveItem_Index.ToString(),
                                                            goodsReceiveItemLocation_Index = BB.goodsReceiveItemLocation_Index.ToString(),
                                                            tagItem_Index = BB.tagItem_Index.ToString(),
                                                            tag_Index = BB.tag_Index.ToString(),
                                                            tag_No = BB.tag_No,
                                                            product_Index = BB.product_Index.ToString(),
                                                            product_Id = BB.product_Id,
                                                            product_Name = BB.product_Name,
                                                            product_SecondName = BB.product_SecondName,
                                                            product_ThirdName = BB.product_ThirdName,
                                                            product_Lot = BB.product_Lot,
                                                            itemStatus_Index = BB.itemStatus_Index.ToString(),
                                                            itemStatus_Id = BB.itemStatus_Id,
                                                            itemStatus_Name = BB.itemStatus_Name,
                                                            goodsReceive_MFG_Date = BB.goodsReceive_MFG_Date.ToString(),
                                                            goodsReceive_EXP_Date = BB.goodsReceive_EXP_Date.ToString(),
                                                            goodsReceive_ProductConversion_Index = BB.goodsReceive_ProductConversion_Index.ToString(),
                                                            goodsReceive_ProductConversion_Id = BB.goodsReceive_ProductConversion_Id.ToString(),
                                                            goodsReceive_ProductConversion_Name = BB.goodsReceive_ProductConversion_Name.ToString(),


                                                            binBalance_Ratio = BB.binBalance_Ratio,
                                                            binBalance_QtyBegin = BB.binBalance_QtyBegin,
                                                            binBalance_WeightBegin = BB.binBalance_WeightBegin,
                                                            binBalance_WeightBegin_Index = BB.binBalance_WeightBegin_Index,
                                                            binBalance_WeightBegin_Id = BB.binBalance_WeightBegin_Id,
                                                            binBalance_WeightBegin_Name = BB.binBalance_WeightBegin_Name,
                                                            binBalance_WeightBeginRatio = BB.binBalance_WeightBeginRatio,
                                                            binBalance_NetWeightBegin = BB.binBalance_NetWeightBegin,
                                                            binBalance_NetWeightBegin_Index = BB.binBalance_NetWeightBegin_Index,
                                                            binBalance_NetWeightBegin_Id = BB.binBalance_NetWeightBegin_Id,
                                                            binBalance_NetWeightBegin_Name = BB.binBalance_NetWeightBegin_Name,
                                                            binBalance_NetWeightBeginRatio = BB.binBalance_NetWeightBeginRatio,
                                                            binBalance_GrsWeightBegin = BB.binBalance_GrsWeightBegin,
                                                            binBalance_GrsWeightBegin_Index = BB.binBalance_GrsWeightBegin_Index,
                                                            binBalance_GrsWeightBegin_Id = BB.binBalance_GrsWeightBegin_Id,
                                                            binBalance_GrsWeightBegin_Name = BB.binBalance_GrsWeightBegin_Name,
                                                            binBalance_GrsWeightBeginRatio = BB.binBalance_GrsWeightBeginRatio,
                                                            binBalance_WidthBegin = BB.binBalance_WidthBegin,
                                                            binBalance_WidthBegin_Index = BB.binBalance_WidthBegin_Index,
                                                            binBalance_WidthBegin_Id = BB.binBalance_WidthBegin_Id,
                                                            binBalance_WidthBegin_Name = BB.binBalance_WidthBegin_Name,
                                                            binBalance_WidthBeginRatio = BB.binBalance_WidthBeginRatio,
                                                            binBalance_LengthBegin = BB.binBalance_LengthBegin,
                                                            binBalance_LengthBegin_Index = BB.binBalance_LengthBegin_Index,
                                                            binBalance_LengthBegin_Id = BB.binBalance_LengthBegin_Id,
                                                            binBalance_LengthBegin_Name = BB.binBalance_LengthBegin_Name,
                                                            binBalance_LengthBeginRatio = BB.binBalance_LengthBeginRatio,
                                                            binBalance_HeightBegin = BB.binBalance_HeightBegin,
                                                            binBalance_HeightBegin_Index = BB.binBalance_HeightBegin_Index,
                                                            binBalance_HeightBegin_Id = BB.binBalance_HeightBegin_Id,
                                                            binBalance_HeightBegin_Name = BB.binBalance_HeightBegin_Name,
                                                            binBalance_HeightBeginRatio = BB.binBalance_HeightBeginRatio,
                                                            binBalance_UnitVolumeBegin = BB.binBalance_UnitVolumeBegin,
                                                            binBalance_VolumeBegin = BB.binBalance_VolumeBegin,
                                                            binBalance_QtyBal = BB.binBalance_QtyBal,
                                                            binBalance_UnitWeightBal = BB.binBalance_UnitWeightBal,
                                                            binBalance_UnitWeightBal_Index = BB.binBalance_UnitWeightBal_Index,
                                                            binBalance_UnitWeightBal_Id = BB.binBalance_UnitWeightBal_Id,
                                                            binBalance_UnitWeightBal_Name = BB.binBalance_UnitWeightBal_Name,
                                                            binBalance_UnitWeightBalRatio = BB.binBalance_UnitWeightBalRatio,
                                                            binBalance_WeightBal = BB.binBalance_WeightBal,
                                                            binBalance_WeightBal_Index = BB.binBalance_WeightBal_Index,
                                                            binBalance_WeightBal_Id = BB.binBalance_WeightBal_Id,
                                                            binBalance_WeightBal_Name = BB.binBalance_WeightBal_Name,
                                                            binBalance_WeightBalRatio = BB.binBalance_WeightBalRatio,
                                                            binBalance_UnitNetWeightBal = BB.binBalance_UnitNetWeightBal,
                                                            binBalance_UnitNetWeightBal_Index = BB.binBalance_UnitNetWeightBal_Index,
                                                            binBalance_UnitNetWeightBal_Id = BB.binBalance_UnitNetWeightBal_Id,
                                                            binBalance_UnitNetWeightBal_Name = BB.binBalance_UnitNetWeightBal_Name,
                                                            binBalance_UnitNetWeightBalRatio = BB.binBalance_UnitNetWeightBalRatio,
                                                            binBalance_NetWeightBal = BB.binBalance_NetWeightBal,
                                                            binBalance_NetWeightBal_Index = BB.binBalance_NetWeightBal_Index,
                                                            binBalance_NetWeightBal_Id = BB.binBalance_NetWeightBal_Id,
                                                            binBalance_NetWeightBal_Name = BB.binBalance_NetWeightBal_Name,
                                                            binBalance_NetWeightBalRatio = BB.binBalance_NetWeightBalRatio,
                                                            binBalance_UnitGrsWeightBal = BB.binBalance_UnitGrsWeightBal,
                                                            binBalance_UnitGrsWeightBal_Index = BB.binBalance_UnitGrsWeightBal_Index,
                                                            binBalance_UnitGrsWeightBal_Id = BB.binBalance_UnitGrsWeightBal_Id,
                                                            binBalance_UnitGrsWeightBal_Name = BB.binBalance_UnitGrsWeightBal_Name,
                                                            binBalance_UnitGrsWeightBalRatio = BB.binBalance_UnitGrsWeightBalRatio,
                                                            binBalance_GrsWeightBal = BB.binBalance_GrsWeightBal,
                                                            binBalance_GrsWeightBal_Index = BB.binBalance_GrsWeightBal_Index,
                                                            binBalance_GrsWeightBal_Id = BB.binBalance_GrsWeightBal_Id,
                                                            binBalance_GrsWeightBal_Name = BB.binBalance_GrsWeightBal_Name,
                                                            binBalance_GrsWeightBalRatio = BB.binBalance_GrsWeightBalRatio,
                                                            binBalance_UnitWidthBal = BB.binBalance_UnitWidthBal,
                                                            binBalance_UnitWidthBal_Index = BB.binBalance_UnitWidthBal_Index,
                                                            binBalance_UnitWidthBal_Id = BB.binBalance_UnitWidthBal_Id,
                                                            binBalance_UnitWidthBal_Name = BB.binBalance_UnitWidthBal_Name,
                                                            binBalance_UnitWidthBalRatio = BB.binBalance_UnitWidthBalRatio,
                                                            binBalance_WidthBal = BB.binBalance_WidthBal,
                                                            binBalance_WidthBal_Index = BB.binBalance_WidthBal_Index,
                                                            binBalance_WidthBal_Id = BB.binBalance_WidthBal_Id,
                                                            binBalance_WidthBal_Name = BB.binBalance_WidthBal_Name,
                                                            binBalance_WidthBalRatio = BB.binBalance_WidthBalRatio,
                                                            binBalance_UnitLengthBal = BB.binBalance_UnitLengthBal,
                                                            binBalance_UnitLengthBal_Index = BB.binBalance_UnitLengthBal_Index,
                                                            binBalance_UnitLengthBal_Id = BB.binBalance_UnitLengthBal_Id,
                                                            binBalance_UnitLengthBal_Name = BB.binBalance_UnitLengthBal_Name,
                                                            binBalance_UnitLengthBalRatio = BB.binBalance_UnitLengthBalRatio,
                                                            binBalance_LengthBal = BB.binBalance_LengthBal,
                                                            binBalance_LengthBal_Index = BB.binBalance_LengthBal_Index,
                                                            binBalance_LengthBal_Id = BB.binBalance_LengthBal_Id,
                                                            binBalance_LengthBal_Name = BB.binBalance_LengthBal_Name,
                                                            binBalance_LengthBalRatio = BB.binBalance_LengthBalRatio,
                                                            binBalance_UnitHeightBal = BB.binBalance_UnitHeightBal,
                                                            binBalance_UnitHeightBal_Index = BB.binBalance_UnitHeightBal_Index,
                                                            binBalance_UnitHeightBal_Id = BB.binBalance_UnitHeightBal_Id,
                                                            binBalance_UnitHeightBal_Name = BB.binBalance_UnitHeightBal_Name,
                                                            binBalance_UnitHeightBalRatio = BB.binBalance_UnitHeightBalRatio,
                                                            binBalance_HeightBal = BB.binBalance_HeightBal,
                                                            binBalance_HeightBal_Index = BB.binBalance_HeightBal_Index,
                                                            binBalance_HeightBal_Id = BB.binBalance_HeightBal_Id,
                                                            binBalance_HeightBal_Name = BB.binBalance_HeightBal_Name,
                                                            binBalance_HeightBalRatio = BB.binBalance_HeightBalRatio,
                                                            binBalance_UnitVolumeBal = BB.binBalance_UnitVolumeBal,
                                                            binBalance_VolumeBal = BB.binBalance_VolumeBal,
                                                            binBalance_QtyReserve = BB.binBalance_QtyReserve,
                                                            binBalance_WeightReserve = BB.binBalance_WeightReserve,
                                                            binBalance_WeightReserve_Index = BB.binBalance_WeightReserve_Index,
                                                            binBalance_WeightReserve_Id = BB.binBalance_WeightReserve_Id,
                                                            binBalance_WeightReserve_Name = BB.binBalance_WeightReserve_Name,
                                                            binBalance_WeightReserveRatio = BB.binBalance_WeightReserveRatio,
                                                            binBalance_NetWeightReserve = BB.binBalance_NetWeightReserve,
                                                            binBalance_NetWeightReserve_Index = BB.binBalance_NetWeightReserve_Index,
                                                            binBalance_NetWeightReserve_Id = BB.binBalance_NetWeightReserve_Id,
                                                            binBalance_NetWeightReserve_Name = BB.binBalance_NetWeightReserve_Name,
                                                            binBalance_NetWeightReserveRatio = BB.binBalance_NetWeightReserveRatio,
                                                            binBalance_GrsWeightReserve = BB.binBalance_GrsWeightReserve,
                                                            binBalance_GrsWeightReserve_Index = BB.binBalance_GrsWeightReserve_Index,
                                                            binBalance_GrsWeightReserve_Id = BB.binBalance_GrsWeightReserve_Id,
                                                            binBalance_GrsWeightReserve_Name = BB.binBalance_GrsWeightReserve_Name,
                                                            binBalance_GrsWeightReserveRatio = BB.binBalance_GrsWeightReserveRatio,
                                                            binBalance_WidthReserve = BB.binBalance_WidthReserve,
                                                            binBalance_WidthReserve_Index = BB.binBalance_WidthReserve_Index,
                                                            binBalance_WidthReserve_Id = BB.binBalance_WidthReserve_Id,
                                                            binBalance_WidthReserve_Name = BB.binBalance_WidthReserve_Name,
                                                            binBalance_WidthReserveRatio = BB.binBalance_WidthReserveRatio,
                                                            binBalance_LengthReserve = BB.binBalance_LengthReserve,
                                                            binBalance_LengthReserve_Index = BB.binBalance_LengthReserve_Index,
                                                            binBalance_LengthReserve_Id = BB.binBalance_LengthReserve_Id,
                                                            binBalance_LengthReserve_Name = BB.binBalance_LengthReserve_Name,
                                                            binBalance_LengthReserveRatio = BB.binBalance_LengthReserveRatio,
                                                            binBalance_HeightReserve = BB.binBalance_HeightReserve,
                                                            binBalance_HeightReserve_Index = BB.binBalance_HeightReserve_Index,
                                                            binBalance_HeightReserve_Id = BB.binBalance_HeightReserve_Id,
                                                            binBalance_HeightReserve_Name = BB.binBalance_HeightReserve_Name,
                                                            binBalance_HeightReserveRatio = BB.binBalance_HeightReserveRatio,
                                                            binBalance_UnitVolumeReserve = BB.binBalance_UnitVolumeReserve,
                                                            binBalance_VolumeReserve = BB.binBalance_VolumeReserve,


                                                            productConversion_Index = BB.productConversion_Index.ToString(),
                                                            productConversion_Id = BB.productConversion_Id,
                                                            productConversion_Name = BB.productConversion_Name,

                                                            unitPrice = BB.unitPrice,
                                                            unitPrice_Index = BB.unitPrice_Index,
                                                            unitPrice_Id = BB.unitPrice_Id,
                                                            unitPrice_Name = BB.unitPrice_Name,
                                                            price = BB.price,
                                                            price_Index = BB.price_Index,
                                                            price_Id = BB.price_Id,
                                                            price_Name = BB.price_Name,

                                                            udf_1 = BB.uDF_1,
                                                            udf_2 = BB.uDF_2,
                                                            udf_3 = BB.uDF_3,
                                                            udf_4 = BB.uDF_4,
                                                            udf_5 = BB.uDF_5,
                                                            create_By = BB.create_By,
                                                            create_Date = BB.create_Date.ToString(),
                                                            update_By = BB.update_By,
                                                            update_Date = BB.update_Date.ToString(),
                                                            cancel_By = BB.cancel_By,
                                                            cancel_Date = BB.cancel_Date.ToString(),
                                                            isUse = BB.isUse,
                                                            binBalance_Status = BB.binBalance_Status,
                                                            picking_Seq = L?.picking_Seq,
                                                            ageRemain = BB.ageRemain,

                                                            invoice_No = BB.invoice_No,
                                                            declaration_No = BB.declaration_No,
                                                            hs_Code = BB.hs_Code,
                                                            conutry_of_Origin = BB.conutry_of_Origin,
                                                            tax1 = BB.tax1,
                                                            tax1_Currency_Index = BB.tax1_Currency_Index,
                                                            tax1_Currency_Id = BB.tax1_Currency_Id,
                                                            tax1_Currency_Name = BB.tax1_Currency_Name,
                                                            tax2 = BB.tax2,
                                                            tax2_Currency_Index = BB.tax2_Currency_Index,
                                                            tax2_Currency_Id = BB.tax2_Currency_Id,
                                                            tax2_Currency_Name = BB.tax2_Currency_Name,
                                                            tax3 = BB.tax3,
                                                            tax3_Currency_Index = BB.tax3_Currency_Index,
                                                            tax3_Currency_Id = BB.tax3_Currency_Id,
                                                            tax3_Currency_Name = BB.tax3_Currency_Name,
                                                            tax4 = BB.tax4,
                                                            tax4_Currency_Index = BB.tax4_Currency_Index,
                                                            tax4_Currency_Id = BB.tax4_Currency_Id,
                                                            tax4_Currency_Name = BB.tax4_Currency_Name,
                                                            tax5 = BB.tax5,
                                                            tax5_Currency_Index = BB.tax5_Currency_Index,
                                                            tax5_Currency_Id = BB.tax5_Currency_Id,
                                                            tax5_Currency_Name = BB.tax5_Currency_Name,

                                                            erp_Location = BB.erp_Location,
                                                            productShelfLife_D = Prd.ProductShelfLife_D != null ? Prd.ProductShelfLife_D : 0,
                                                        }).AsQueryable();


                            #endregion

                            var CheckBinBalanceResult = View_WaveBinBalance2.ToList();

                            State = "CountgetView_WaveBinBalance2 : " + CheckBinBalanceResult.Count().ToString(); ;
                            olog.logging("runwave" + threadnum.ToString(), State);


                            #region for RuleDesSort
                            var RuleDesList2 = getViewWaveTemplate.Where(c => c.isDestination == 1 && c.isSearch == 1).ToList();

                            //var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>(e, param);

                            foreach (var itemRuleDesList in RuleDesList2)
                            {
                                if (itemRuleDesList.ruleConditionOperation == "IN")
                                {
                                    if (itemRuleDesList.ruleConditionField_Name == "Zone_Id")
                                    {
                                        //setWhereDes += " And  Location_Index   in (  select   Location_Index  from ms_ZoneLocation   where  IsDelete <> -1 and Zone_Index  in (select Zone_Index from ms_Zone where IsDelete <> -1 and " + itemRuleDesList.ruleConditionField_Name + " IN (" + itemRuleDesList.ruleCondition_Param + " )" + ")) ";
                                        var listDataZoneLocation = utils.SendDataApi<List<ZoneLocationViewModel>>(new AppSettingConfig().GetUrl("getZoneLocationMaster"), new { }.sJson());
                                        var listDataZone = utils.SendDataApi<List<ZoneViewModel>>(new AppSettingConfig().GetUrl("getZoneMaster"), new { }.sJson());

                                        //var param = Expression.Parameter(typeof(ZoneViewModel), "x");
                                        //var predicate = Expression.Lambda<Func<ZoneViewModel, bool>>(
                                        //    Expression.Call(
                                        //        Expression.PropertyOrField(param, itemRuleDesList.ruleConditionField_Name),
                                        //        "Contains", null, Expression.Constant(itemRuleDesList.ruleCondition_Param)
                                        //    ), param);
                                        var dataarray = itemRuleDesList.ruleCondition_Param.Replace("'", "").Split(',');
                                        var dataZone = listDataZone.Where(c => dataarray.Contains(c.zone_Id)).ToList();
                                        var zoneArray = new List<Guid?>();
                                        foreach (var z in dataZone)
                                        {
                                            zoneArray.Add(z.zone_Index);
                                        }
                                        var listLocation_index = listDataZoneLocation.Where(c => zoneArray.Contains(c.zone_Index)).Select(s => s.location_Index.ToString()).ToList();
                                        View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => listLocation_index.Contains(c.location_Index));
                                    }
                                    else
                                    {
                                        var dataarray = itemRuleDesList.ruleCondition_Param.Replace("'", "").Split(',');
                                        if (itemRuleDesList.ruleConditionField_Name == "GoodsReceive_Date")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.goodsReceive_Date));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "GoodsReceive_No")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.goodsReceive_No));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "ItemStatus_Id")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.itemStatus_Id));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Location_Name")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.location_Name));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Locationtype_Id")
                                        {
                                            //View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.location));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Product_Id")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.product_Id));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Product_Lot")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.product_Lot));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Warehouse")
                                        {
                                            //planGIWaveResult = planGIWaveResult.Where(c => dataarray.Contains(c.Warehouse));
                                        }

                                        //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " IN (" + itemRuleDesList.ruleCondition_Param + ") ";
                                        //var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                        //var predicate = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>(
                                        //    Expression.Call(
                                        //        Expression.PropertyOrField(param, itemRuleDesList.ruleConditionField_Name),
                                        //        "Contains", null, Expression.Constant(itemRuleDesList.ruleCondition_Param)
                                        //    ), param);
                                        //View_WaveBinBalance = View_WaveBinBalance.Where(predicate);
                                    }
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "=")
                                {
                                    if (itemRuleDesList.ruleConditionField_Name == "Zone_Id")
                                    {
                                        //setWhereDes += "  And  Location_Index   in (  select   Location_Index  from ms_ZoneLocation   where IsDelete <> -1 and Zone_Index  in (select Zone_Index from ms_Zone where IsDelete <> -1 and " + itemRuleDesList.ruleConditionField_Name + " = '" + itemRuleDesList.ruleCondition_Param + "' " + ")) ";
                                        var listDataZoneLocation = utils.SendDataApi<List<ZoneLocationViewModel>>(new AppSettingConfig().GetUrl("getZoneLocationMaster"), new { }.sJson());
                                        var listDataZone = utils.SendDataApi<List<ZoneViewModel>>(new AppSettingConfig().GetUrl("getZoneMaster"), new { }.sJson());

                                        var param = Expression.Parameter(typeof(ZoneViewModel), "x");
                                        var lambda = Expression.Lambda<Func<ZoneViewModel, bool>>((Expression)Expression.Equal(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                            , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                        var dataZone = listDataZone.AsQueryable().Where(lambda).ToList();
                                        var zoneArray = new List<Guid?>();
                                        foreach (var z in dataZone)
                                        {
                                            zoneArray.Add(z.zone_Index);
                                        }
                                        var listLocation_index = listDataZoneLocation.Where(c => zoneArray.Contains(c.zone_Index)).Select(s => s.location_Index.ToString()).ToList();
                                        View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => listLocation_index.Contains(c.location_Index));
                                    }
                                    else
                                    {
                                        //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " = '" + itemRuleDesList.ruleCondition_Param + "' ";
                                        var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                        var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.Equal(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                            , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                        View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                    }
                                }
                                else if (itemRuleDesList.ruleConditionOperation == ">")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " > '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.GreaterThan(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "<")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " < '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.LessThan(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "!=")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " != '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.NotEqual(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == ">=")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " >= '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.GreaterThanOrEqual(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "<=")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " <= '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.LessThanOrEqual(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "Like")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " Like '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var predicate = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>(
                                        Expression.Call(
                                            Expression.PropertyOrField(param, itemRuleDesList.ruleConditionField_Name),
                                            "Contains", null, Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""))
                                        ), param);
                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(predicate);
                                }
                                //setWhereDes += "";

                            }
                            #endregion

                            #region for RuleDesSortList
                            var RuleDesSortList2 = getViewWaveTemplate.Where(c => c.isDestination == 1 && c.isSort == 1).ToList();

                            int iRowsDesSort2 = 0;
                            foreach (var itemRuleDesSortList in RuleDesSortList2)
                            {
                                if (iRowsDesSort2 == 0)
                                {
                                    //setWhereDesSort += itemRuleDesSortList.ruleConditionField_Name + ' ' + itemRuleDesSortList.ruleCondition_Param;
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, string>>(Expression.Property(param, itemRuleDesSortList.ruleConditionField_Name), param);
                                    if (itemRuleDesSortList.ruleCondition_Param.ToUpper() == "DESC")
                                    {
                                        View_WaveBinBalance2 = View_WaveBinBalance2.OrderByDescending(lambda);
                                    }
                                    else /*if (itemRuleDesSortList.ruleCondition_Param.ToUpper() == "DESC")*/
                                    {
                                        View_WaveBinBalance2 = View_WaveBinBalance2.OrderBy(lambda);
                                    }
                                }
                                else
                                {
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, string>>(Expression.Property(param, itemRuleDesSortList.ruleConditionField_Name), param);
                                    if (itemRuleDesSortList.ruleCondition_Param.ToUpper() == "DESC")
                                    {
                                        View_WaveBinBalance2 = View_WaveBinBalance2.OrderByDescending(lambda);
                                    }
                                    else /*if (itemRuleDesSortList.ruleCondition_Param.ToUpper() == "DESC")*/
                                    {
                                        View_WaveBinBalance2 = View_WaveBinBalance2.OrderBy(lambda);
                                    }
                                }
                                iRowsDesSort2 = iRowsDesSort2 + 1;

                            }
                            #endregion




                            State = "View_WaveBinBalance2";
                            olog.logging("runwave" + threadnum.ToString(), State);
                            var BinBalanceResult = View_WaveBinBalance2.ToList();

                            //  foreach (var itemBin in BinBalanceResult.OrderBy(c => c.goodsReceive_EXP_Date).ThenBy(d => d.goodsReceive_Date).ThenBy(e => e.picking_Seq).ThenBy(f => f.location_Name))



                            State = "CountgetBinBalanceResult : " + BinBalanceResult.Count().ToString(); ;
                            olog.logging("runwave" + threadnum.ToString(), State);



                            var itemBinSort = new List<View_WaveBinBalanceViewModel>();

                            if (IsPA == 1 && IsTote == true)
                            {
                                itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenBy(f => f.location_Name).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ToList();

                            }
                            else if (IsPA == 0 && IsTote == true)
                            {
                                itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenByDescending(f => f.location_Name).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ToList();
                            }
                            else
                            {
                                itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name).ToList();

                            }



                            //  foreach (var itemBin in BinBalanceResult.OrderBy(c => c.picking_Seq).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name))
                            foreach (var itemBin in itemBinSort)
                            {

                                decimal? QtyBal = itemBin.binBalance_QtyBal - itemBin.binBalance_QtyReserve;
                                if (QtyPlanGIRemian <= 0)
                                {
                                    break;
                                }
                                if (QtyBal <= 0)
                                {
                                    continue;
                                }
                                if (QtyPlanGIRemian >= QtyBal && QtyBal > 0)
                                {
                                    State = "QtyPlanGIRemian >= QtyBal && QtyBal > 0";
                                    olog.logging("runwave" + threadnum.ToString(), State + " TAG_NO " + itemBin.tag_No + " Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);

                                    // Add GI ITEMLOCATION 
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
                                    //GoodsIssueItemLocation.ProductConversion_Index = new Guid(itemBin.productConversion_Index);
                                    //GoodsIssueItemLocation.ProductConversion_Id = itemBin.productConversion_Id;
                                    //GoodsIssueItemLocation.ProductConversion_Name = itemBin.productConversion_Name;
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
                                    GoodsIssueItemLocation.DocumentRef_No5 = threadnum.ToString(); //itemPlanGI.DocumentRef_No5;
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


                                    itemPlanGI.GITotalQty = itemPlanGI.GITotalQty + QtyBal;

                                    QtyPlanGIRemian = QtyPlanGIRemian - QtyBal;

                                }
                                else if (QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0)
                                {


                                    State = "QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0";
                                    olog.logging("runwave" + threadnum.ToString(), State + " TAG_NO " + itemBin.tag_No + " Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);

                                    var QtyPick = QtyPlanGIRemian;
                                    // Add GI ITEMLOCATION 
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
                                    //GoodsIssueItemLocation.ProductConversion_Index = new Guid(itemBin.productConversion_Index);
                                    //GoodsIssueItemLocation.ProductConversion_Id = itemBin.productConversion_Id;
                                    //GoodsIssueItemLocation.ProductConversion_Name = itemBin.productConversion_Name;
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
                                    GoodsIssueItemLocation.DocumentRef_No5 = threadnum.ToString();   // itemPlanGI.DocumentRef_No5;
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

                                    itemPlanGI.GITotalQty = itemPlanGI.GITotalQty + QtyPick;

                                    QtyPlanGIRemian = QtyPlanGIRemian - QtyPick;

                                }
                            }

                            #region inset GIL and runwave status 30
                            using (var db4 = new GIDbContext())
                            {
                                var transaction = db4.Database.BeginTransaction();
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
                                    olog.logging("runwave" + threadnum.ToString(), msglog);
                                    transaction.Rollback();
                                    throw exy;
                                }
                            }
                            #endregion
                            State = "inset GIL and runwave status 30";
                            olog.logging("runwave" + threadnum.ToString(), State);

                            #region insert bincardreserve and runwave status 40

                            using (var Contact = new GIDbContext())
                            {
                                var GIL = Contact.IM_GoodsIssueItemLocation.Where(c => c.Ref_Document_Index == itemPlanGI.PlanGoodsIssue_Index && c.Ref_DocumentItem_Index == itemPlanGI.PlanGoodsIssueItem_Index && c.Document_Status != -1).ToList();
                                foreach (var g in GIL)
                                {
                                    State = "insertBinCardReserve";

                                    olog.logging("runwave" + threadnum.ToString(), State + " GIIL_Index" + g.GoodsIssueItemLocation_Index.ToString());

                                    var insertBinCardReserve = new PickbinbalanceViewModel();

                                    insertBinCardReserve.ref_Document_Index = g.GoodsIssue_Index.ToString();
                                    insertBinCardReserve.ref_DocumentItem_Index = g.GoodsIssueItemLocation_Index.ToString();
                                    insertBinCardReserve.goodsIssue_No = model.goodsIssue_No;
                                    insertBinCardReserve.Process_Index = "22744590-55D8-4448-88EF-5997C252111F";
                                    //model.GIIL = GoodsIssueItemLocation;
                                    insertBinCardReserve.create_By = model.create_by;
                                    insertBinCardReserve.pick = g.TotalQty;
                                    insertBinCardReserve.binbalance_Index = g.BinBalance_Index.ToString();
                                    insertBinCardReserve.wave_Index = Contact.IM_GoodsIssue.FirstOrDefault(f => f.GoodsIssue_Index == g.GoodsIssue_Index)?.Wave_Index.ToString();

                                    State = "insetBinRe";
                                    olog.logging("runwave" + threadnum.ToString(), State);

                                    var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("insertBinCardReserve"), insertBinCardReserve.sJson());
                                    if (insetBinRe.resultIsUse)
                                    {
                                        State = "resultIsUse";
                                        olog.logging("runwave" + threadnum.ToString(), State);

                                        var transaction = Contact.Database.BeginTransaction();
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
                                            olog.logging("runwave" + threadnum.ToString(), msglog);
                                            transaction.Rollback();
                                            throw exy;
                                        }
                                    }
                                    else
                                    {
                                        State = "resultIsUse else";
                                        olog.logging("runwave" + threadnum.ToString(), State);


                                        var transaction = Contact.Database.BeginTransaction();
                                        try
                                        {
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
                                            olog.logging("runwave" + threadnum.ToString(), msglog);
                                            transaction.Rollback();
                                            throw exy;
                                        }
                                        msglog = State + " ex Rollback " + "Insert BinCardReserve Error";
                                        olog.logging("runwave" + threadnum.ToString(), msglog);
                                        result.resultMsg = "Insert BinCardReserve Error";
                                        result.resultIsUse = false;
                                        //return result;

                                    }
                                }
                            }
                            #endregion
                            State = "insert bincardreserve and runwave status 40";
                            olog.logging("runwave" + threadnum.ToString(), State);
                        }

                        #region update isuse = '' and runwave 50
                        strwhere.isActive = true;
                        State = "updateIsuseViewBinbalance ";
                        var updateIsuseViewBinbalance = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateIsuseViewBinbalance"), strwhere.sJson());
                        if (updateIsuseViewBinbalance)
                        {
                            using (var db5 = new GIDbContext())
                            {
                                var transaction = db5.Database.BeginTransaction();
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
                                    olog.logging("runwave" + threadnum.ToString(), msglog);
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
                        State = "region update isuse = '' and runwave 50";
                        olog.logging("runwave" + threadnum.ToString(), State);
                    }
                }


                result.goodsIssue_Index = model.goodsIssue_Index;
                result.goodsIssue_No = model.goodsIssue_No;

                result.resultIsUse = true;
                State = "end for";
                olog.logging("runwave" + threadnum.ToString(), State);

                //#region Check retrun status PGII 
                //foreach (var itemPGII in model.listGoodsIssueItemViewModel)
                //{
                //    var chkGIL2 = dbThread.IM_GoodsIssueItemLocation.Where(c => c.Ref_Document_Index == itemPGII.planGoodsIssue_Index && c.Ref_DocumentItem_Index == itemPGII.planGoodsIssueItem_Index && c.Document_Status != -1).ToList();
                //    var sumqty = chkGIL2.Sum(s => s.TotalQty);
                //    if (chkGIL2.Count == 0 || sumqty != itemPGII.totalQty)
                //    {

                //        var pgii = model.listGoodsIssueItemViewModel.Where(c => c.planGoodsIssueItem_Index == itemPGII.planGoodsIssueItem_Index).ToList();
                //        foreach (var resultpgii in pgii)
                //        {
                //            resultpgii.qtyPlan = (resultpgii.totalQty - sumqty);
                //            resultpgii.totalQty = (resultpgii.totalQty - sumqty);
                //            listpgiinotinsert.Add(resultpgii);
                //            CheckRunwavePast = true;
                //            using (var updatepginotProduct = new GIDbContext())
                //            {
                //                var transactionresultpgii = updatepginotProduct.Database.BeginTransaction();
                //                try
                //                {

                //                    var updateresultpgii = updatepginotProduct.IM_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssueItem_Index == resultpgii.planGoodsIssueItem_Index && c.Document_Status == 1).ToList();
                //                    foreach (var p in updateresultpgii)
                //                    {
                //                        p.Document_Status = 0;
                //                    }
                //                    updatepginotProduct.SaveChanges();
                //                    transactionresultpgii.Commit();
                //                }

                //                catch (Exception exy)
                //                {
                //                    msglog = State + " ex Rollback " + exy.Message.ToString();
                //                    olog.logging("runwave" + threadnum.ToString(), msglog);
                //                    transactionresultpgii.Rollback();
                //                    throw exy;
                //                }
                //            }
                //        }
                //    }
                //}
                //#endregion

                //State = "Check retrun status PGII";
                //olog.logging("runwave" + threadnum.ToString(), State);
                //#region update PI status 3 and runwave status 60
                //using (var db5 = new GIDbContext())
                //{
                //    var listPGI = new List<Guid>();
                //    if (model.listGoodsIssueItemViewModel.Count > 0)
                //    {
                //        foreach (var item in model.listGoodsIssueItemViewModel)
                //        {
                //            if (item.planGoodsIssue_Index != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                //            {
                //                listPGI.Add(item.planGoodsIssue_Index);
                //            }
                //        }
                //    }
                //    foreach (var item in listPGI)
                //    {
                //        var pgii = db5.IM_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssue_Index == item && c.Document_Status == 0).Count();
                //        if (pgii == 0)
                //        {
                //            var transaction = db5.Database.BeginTransaction();
                //            try
                //            {
                //                var pgi = db5.IM_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_Index == item).ToList();
                //                foreach (var p in pgi)
                //                {
                //                    p.Document_Status = 3;
                //                }
                //                //var GI = db5.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 50).ToList();
                //                //foreach (var g in GI)
                //                //{
                //                //    g.Wave_Index = new Guid(model.wave_Index);
                //                //    g.RunWave_Status = 60;
                //                //}
                //                db5.SaveChanges();
                //                transaction.Commit();
                //            }

                //            catch (Exception exy)
                //            {
                //                msglog = State + " ex Rollback " + exy.Message.ToString();
                //                olog.logging("runwave" + threadnum.ToString(), msglog);
                //                transaction.Rollback();
                //                throw exy;
                //            }
                //        }
                //    }
                //}
                //#endregion

                //State = "update PI status 3 and runwave status 60";
                //olog.logging("runwave" + threadnum.ToString(), State);

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
                olog.logging("runwave" + threadnum.ToString(), msglog);
                var result = new actionResultRunWaveV2ViewModelViewModel();
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }
            finally
            {
                dbThread.Dispose();

            }

        }
        public actionResultRunWaveV2ViewModelViewModel runwaveandHeaderThread1(RunWaveFilterV2ViewModel model, int threadnum)
        {

            String State = "Start " + threadnum.ToString();
            String msglog = "";
            bool chkdatawave = false;
            var olog = new logtxt();
            var process = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
            var strprocess = "2E026669-99BD-4DE0-8818-534F29F7B89D";
            var PlanGiRunWave = new List<Guid>();
            var ListPlanGi_Index = new List<Guid>();
            var listpgiinotinsert = new List<plangoodsissueitemViewModel>();
            bool CheckRunwavePast = false;
            long IsPA = 1;
            Boolean IsTote = false;
            olog.logging("runwave" + threadnum.ToString(), State);



            var dbThread = new GIDbContext();
            try
            {
                var listDataProduct2 = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), new { }.sJson());
                var listDataLocation2 = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("getLocationMaster"), new { }.sJson());


                #region create and update header
                dbThread.Database.SetCommandTimeout(360);
                Guid gi_index = !string.IsNullOrEmpty(model.goodsIssue_Index) ? new Guid(model.goodsIssue_Index) : new Guid("00000000-0000-0000-0000-000000000000");
                var gi = dbThread.IM_GoodsIssue.Find(gi_index);
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
                    dbThread.IM_GoodsIssue.Add(newGI);

                    model.goodsIssue_Index = newGI.GoodsIssue_Index.ToString();
                    model.goodsIssue_No = newGI.GoodsIssue_No;
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
                }

                var transactionx = dbThread.Database.BeginTransaction();
                try
                {
                    dbThread.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SavePlanGR", msglog);
                    transactionx.Rollback();

                    throw exy;

                }
                #endregion


                //int CheckRunwavePast = model.listGoodsIssueItemViewModel.Count();
                var result = new actionResultRunWaveV2ViewModelViewModel();

                dbThread.Database.SetCommandTimeout(360);


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




                //var planGI_Lot = dbThread.View_WaveCheckProductLot.Where(c => ListPlanGi_Index.Contains(c.PlanGoodsIssue_Index)).Select(s => new
                //{
                //    s.Product_Index
                //    ,s.Product_Id
                //    ,s.Product_Lot
                //});



                using (var db2 = new GIDbContext())
                {
                    var transaction = db2.Database.BeginTransaction();
                    try
                    {
                        //update status 10
                        var pgi = db2.IM_PlanGoodsIssueItem.Where(c => PlanGiRunWave.Contains(c.PlanGoodsIssueItem_Index) && c.Document_Status == 0).ToList();
                        foreach (var p in pgi)
                        {
                            //p.Ref_WavePick_index = new Guid(model.goodsIssue_Index);
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
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("runwave" + threadnum.ToString(), msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }
                #endregion



                var planGIResultx = dbThread.View_PLANWAVEV.Where(c => PlanGiRunWave.Contains(c.PlanGoodsIssueItem_Index) && c.ThreadNum == threadnum)
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
                        //g.sum(TotalQty) as TotalQty,
                        //g.isnull(sum(GITotalQty), 0) as GITotalQty,
                        //g.sum(TotalQty) - isnull(sum(GITotalQty), 0)  AS QtyWave,
                        //g.ROW_NUMBER() OVER(Order by Product_Id) as RowNum,
                        g.Owner_Index,
                        g.PlanGoodsIssue_UDF_1,
                        g.PlanGoodsIssue_UDF_2,
                        g.PlanGoodsIssue_UDF_3,
                        g.PlanGoodsIssue_UDF_4,
                        g.PlanGoodsIssue_UDF_5,
                        //  g.ERP_Location
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
                        //g.ROW_NUMBER() OVER(Order by Product_Id) as RowNum,
                        s.Key.Owner_Index,
                        s.Key.PlanGoodsIssue_UDF_1,
                        s.Key.PlanGoodsIssue_UDF_2,
                        s.Key.PlanGoodsIssue_UDF_3,
                        s.Key.PlanGoodsIssue_UDF_4,
                        s.Key.PlanGoodsIssue_UDF_5,
                        // s.Key.ERP_Location
                    }).ToList();

                State = "View_PLANWAVEV";
                olog.logging("runwave" + threadnum.ToString(), State);
                if (planGIResultx.Count == 0)
                {
                    throw new Exception("Plan GI not found..");
                }


                //find wave template
                var jsGetWaveRule = new { process_Index = process, wave_Index = model.wave_Index };
                var getWaveRule = utils.SendDataApi<List<WaveRuleViewModel>>(new AppSettingConfig().GetUrl("getWaveRule"), jsGetWaveRule.sJson());

                if (getWaveRule.Count == 0)
                {
                    throw new Exception("Wave Template not found.");
                }

                var getViewWaveTemplateEX = utils.SendDataApi<List<WaveTemplateViewModel>>(new AppSettingConfig().GetUrl("getViewWaveTemplate"), new { }.sJson());

                State = "getWaveRule";
                olog.logging("runwave" + threadnum.ToString(), State);
                bool isUseAttribute = false;

                foreach (var waveRule in getWaveRule.OrderBy(o => o.waveRule_Seq))
                {
                    var jsgetViewWaveTemplate = new { process_Index = process, wave_Index = model.wave_Index, rule_Index = waveRule.rule_Index };
                    //var getViewWaveTemplate = utils.SendDataApi<List<WaveTemplateViewModel>>(new AppSettingConfig().GetUrl("getViewWaveTemplate"), jsgetViewWaveTemplate.sJson());
                    var getViewWaveTemplate = getViewWaveTemplateEX.Where(c => c.process_Index == process.ToString() && c.wave_Index == model.wave_Index && c.rule_Index == waveRule.rule_Index);
                    State = "getViewWaveTemplate";
                    olog.logging("runwave" + threadnum.ToString(), State);
                    var planGIWaveResult = dbThread.View_PLANWAVEbyPLANGIV2.AsQueryable();
                    var check = planGIWaveResult.ToList();
                    planGIWaveResult = planGIWaveResult.Where(c => PlanGiRunWave.Contains(c.PlanGoodsIssueItem_Index));

                    #region for RuleSource
                    var RuleSourceList = getViewWaveTemplate.Where(c => c.isSource == 1 && c.isSearch == 1).ToList();

                    foreach (var itemRuleSourceList in RuleSourceList)
                    {
                        if (itemRuleSourceList.ruleConditionOperation == "IN")
                        {
                            var dataarray = itemRuleSourceList.ruleCondition_Param.Replace("'", "").Split(',');
                            if (itemRuleSourceList.ruleConditionField_Name == "DocumentType_Id")
                            {
                                planGIWaveResult = planGIWaveResult.Where(c => dataarray.Contains(c.DocumentType_Id));
                            }
                            if (itemRuleSourceList.ruleConditionField_Name == "Owner_Id")
                            {
                                planGIWaveResult = planGIWaveResult.Where(c => dataarray.Contains(c.Owner_Id));
                            }
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " IN (" + itemRuleSourceList.ruleCondition_Param + ") ";
                            //var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            //var predicate = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>(
                            //    Expression.Call(
                            //        Expression.PropertyOrField(param, itemRuleSourceList.ruleConditionField_Name),
                            //        "Contains", null, Expression.Constant(itemRuleSourceList.ruleCondition_Param)
                            //    ), param);
                            //planGIWaveResult = planGIWaveResult.Where(predicate);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "=")
                        {
                            if (itemRuleSourceList.ruleConditionField_Name == "UseAttribute")
                            {
                                if (itemRuleSourceList.ruleCondition_Param.ToString().ToUpper() == "TRUE")
                                {
                                    isUseAttribute = true;
                                }
                                else
                                {
                                    isUseAttribute = false;
                                }

                            }
                            else
                            {
                                //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " = '" + itemRuleSourceList.ruleCondition_Param + "' ";
                                var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                                var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.Equal(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                    , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                planGIWaveResult = planGIWaveResult.Where(lambda);
                            }


                        }
                        else if (itemRuleSourceList.ruleConditionOperation == ">")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " > '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.GreaterThan(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);

                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "<")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " < '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.LessThan(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "!=")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " != '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.NotEqual(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == ">=")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " >= '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.GreaterThanOrEqual(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "<=")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " <= '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>((Expression)Expression.LessThanOrEqual(Expression.Property(param, itemRuleSourceList.ruleConditionField_Name)
                                , Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                            planGIWaveResult = planGIWaveResult.Where(lambda);
                        }
                        else if (itemRuleSourceList.ruleConditionOperation == "Like")
                        {
                            //setWhereSource += " And " + itemRuleSourceList.ruleConditionField_Name + " Like '" + itemRuleSourceList.ruleCondition_Param + "' ";
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var predicate = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>(
                                Expression.Call(
                                    Expression.PropertyOrField(param, itemRuleSourceList.ruleConditionField_Name),
                                    "Contains", null, Expression.Constant(itemRuleSourceList.ruleCondition_Param.Replace("'", ""))
                                ), param);
                            planGIWaveResult = planGIWaveResult.Where(predicate);
                        }
                    }
                    #endregion

                    #region for RuleDesSort
                    var RuleSourceSortList = getViewWaveTemplate.Where(c => c.isSource == 1 && c.isSort == 1).ToList();

                    string setWhereSourceSort = "";

                    if (RuleSourceSortList.Count > 0)
                    {
                        setWhereSourceSort += " Order by ";
                    }

                    int iRowsSourceSort = 0;
                    foreach (var itemRuleSourceSortList in RuleSourceSortList)
                    {
                        if (iRowsSourceSort == 0)
                        {
                            setWhereSourceSort += itemRuleSourceSortList.ruleConditionField_Name + ' ' + itemRuleSourceSortList.ruleCondition_Param;
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, string>>(Expression.Property(param, itemRuleSourceSortList.ruleConditionField_Name), param);
                            if (itemRuleSourceSortList.ruleCondition_Param.ToUpper() == "DESC")
                            {
                                planGIWaveResult = planGIWaveResult.OrderByDescending(lambda);
                            }
                            else /*if (itemRuleSourceSortList.ruleCondition_Param.ToUpper() == "DESC")*/
                            {
                                planGIWaveResult = planGIWaveResult.OrderBy(lambda);
                            }
                        }
                        else
                        {
                            setWhereSourceSort += "," + itemRuleSourceSortList.ruleConditionField_Name + ' ' + itemRuleSourceSortList.ruleCondition_Param;
                            var param = Expression.Parameter(typeof(View_PLANWAVEbyPLANGIV2), "x");
                            var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, string>>(Expression.Property(param, itemRuleSourceSortList.ruleConditionField_Name), param);
                            if (itemRuleSourceSortList.ruleCondition_Param.ToUpper() == "DESC")
                            {
                                planGIWaveResult = planGIWaveResult.OrderByDescending(lambda);
                            }
                            else /*if (itemRuleSourceSortList.ruleCondition_Param.ToUpper() == "DESC")*/
                            {
                                planGIWaveResult = planGIWaveResult.OrderBy(lambda);
                            }
                        }
                        iRowsSourceSort = iRowsSourceSort + 1;

                    }
                    #endregion


                    String SqlWhere = "";
                    var planGIWaveResult2 = planGIWaveResult.ToList();
                    State = "View_PLANWAVEbyPLANGIV2";
                    olog.logging("runwave" + threadnum.ToString(), State);



                    // LOOP Group SUM PLAN GI
                    foreach (var item in planGIResultx)
                    {

                        olog.logging("runwave" + threadnum.ToString(), "planGIResultx : " + item.Product_Id.ToString());


                        if (item.Product_Id.ToString() == "C1004685WORD")
                        {
                            var aa = item.DocumentRef_No1;

                        }
                        var planGIWaveResult3 = planGIWaveResult2.AsQueryable();
                        var strwhere = new getViewBinbalanceViewModel();
                        //GET Condition  From Plan GI
                        #region query Plag Gi
                        if (item.Owner_Index.ToString() != "")
                        {
                            //SqlWhere += " And Convert(Nvarchar(200) ,Owner_Index) =  '" + item.Owner_Index.ToString() + "' ";
                            //SqlWhere += " And Owner_Index =  '" + item.Owner_Index + "' ";
                            strwhere.Owner_Index = item.Owner_Index;
                            planGIWaveResult3 = planGIWaveResult3.Where(c => c.Owner_Index == item.Owner_Index);
                        }
                        if (item.Product_Index.ToString() != "")
                        {
                            //SqlWhere += " And Convert(Nvarchar(200) , Product_Index ) = '" + item.Product_Index.ToString() + "' ";
                            //SqlWhere += " And Product_Index  = '" + item.Product_Index + "' ";
                            strwhere.Product_Index = item.Product_Index;
                            planGIWaveResult3 = planGIWaveResult3.Where(c => c.Product_Index == item.Product_Index);
                        }

                        //if (item.Product_Lot != null)
                        //{
                        //    if (item.Product_Lot.ToString() != "")
                        //    {
                        //        //SqlWhere += " And Product_Lot = '" + item.Product_Lot.ToString() + "' ";
                        //        strwhere.Product_Lot = item.Product_Lot;
                        //        planGIWaveResult3 = planGIWaveResult3.Where(c => c.Product_Lot == item.Product_Lot);
                        //    }
                        //}
                        if (item.ItemStatus_Index.ToString() != "")
                        {
                            //SqlWhere += " And Convert(Nvarchar(200) ,ItemStatus_Index) =  '" + item.ItemStatus_Index.ToString() + "' ";
                            //SqlWhere += " And ItemStatus_Index =  '" + item.ItemStatus_Index + "' ";
                            strwhere.ItemStatus_Index = item.ItemStatus_Index;
                            planGIWaveResult3 = planGIWaveResult3.Where(c => c.ItemStatus_Index == item.ItemStatus_Index);
                        }
                        if (item.MFG_Date != null)
                        {
                            if (item.MFG_Date.ToString() != "")
                            {
                                //strwhere.MFG_Date = item.MFG_Date;
                                // SqlWhere += " And MFG_Date = @MFG_Date ";
                            }
                        }
                        if (item.EXP_Date != null)
                        {
                            if (item.EXP_Date.ToString() != "")
                            {
                                //strwhere.EXP_Date = item.EXP_Date;
                                //SqlWhere += " And EXP_Date = @EXP_Date ";
                            }
                        }

                        if (item.Product_Lot != null)
                        {
                            if (item.Product_Lot.ToString() != "")
                            {
                                //strwhere.EXP_Date = item.EXP_Date;
                                strwhere.Product_Lot = item.Product_Lot;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.Product_Lot == item.Product_Lot);
                            }
                        }

                        //if (item.ERP_Location != null)
                        //{
                        //    if (item.ERP_Location.ToString() != "")
                        //    {
                        //        //strwhere.EXP_Date = item.EXP_Date;
                        //        strwhere.ERP_Location = item.ERP_Location;
                        //        planGIWaveResult3 = planGIWaveResult3.Where(c => c.ERP_Location == item.ERP_Location);
                        //    }
                        //}

                        //if (item.UDF_1 != null)
                        //{
                        //    //SqlWhere += " And Isnull(UDF_1,'') = '" + item.UDF_1.ToString() + "'";
                        //    strwhere.UDF_1 = item.UDF_1;
                        //    planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_1 == item.UDF_1);
                        //}

                        //if (item.UDF_2 != null)
                        //{
                        //    //SqlWhere += " And  Isnull( UDF_2,'') = '" + item.UDF_2.ToString() + "'";
                        //    strwhere.UDF_2 = item.UDF_2;
                        //    planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_2 == item.UDF_2);
                        //}

                        if (isUseAttribute == true)
                        {
                            // ADD UDF 1-5 
                            strwhere.isUseAttribute = isUseAttribute;
                            //if (item.UDF_1 != null)
                            //{
                            //    //SqlWhere += " And Isnull(UDF_1,'') = '" + item.UDF_1.ToString() + "'";
                            //    strwhere.UDF_1 = item.UDF_1;
                            //    planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_1 == item.UDF_1);
                            //}

                            //if (item.UDF_2 != null)
                            //{
                            //    //SqlWhere += " And  Isnull( UDF_2,'') = '" + item.UDF_2.ToString() + "'";
                            //    strwhere.UDF_2 = item.UDF_2;
                            //    planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_2 == item.UDF_2);
                            //}

                            if (item.UDF_3 != null)
                            {
                                //SqlWhere += " And  Isnull(UDF_3,'') = '" + item.UDF_3.ToString() + "'";
                                strwhere.UDF_3 = item.UDF_3;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_3 == item.UDF_3);
                            }

                            if (item.UDF_4 != null)
                            {
                                //SqlWhere += " And  Isnull(UDF_4,'') = '" + item.UDF_4.ToString() + "'";
                                strwhere.UDF_4 = item.UDF_4;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_4 == item.UDF_4);
                            }

                            if (item.UDF_5 != null)
                            {
                                //SqlWhere += " And  Isnull(UDF_5,'') = '" + item.UDF_5.ToString() + "'";
                                strwhere.UDF_5 = item.UDF_5;
                                planGIWaveResult3 = planGIWaveResult3.Where(c => c.UDF_5 == item.UDF_5);
                            }
                        }
                        #endregion

                        //planGIWaveResult = planGIWaveResult.Where(setWhereSource);

                        if (planGIWaveResult3.OrderBy(c => c.LineNum).ToList().Count < 1)
                        {
                            continue;
                        }
                        #region update isuse and runwave status 20
                        strwhere.isuse = model.goodsIssue_Index;
                        var listDataBinbalance = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateIsuseViewBinbalance"), strwhere.sJson());
                        if (listDataBinbalance)
                        {
                            using (var db3 = new GIDbContext())
                            {
                                var transaction = db3.Database.BeginTransaction();
                                try
                                {
                                    var GI = db3.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 10).ToList();
                                    foreach (var g in GI)
                                    {
                                        g.RunWave_Status = 20;
                                    }
                                    db3.SaveChanges();
                                    transaction.Commit();
                                }
                                catch (Exception exy)
                                {
                                    msglog = State + " ex Rollback " + exy.Message.ToString();
                                    olog.logging("runwave" + threadnum.ToString(), msglog);
                                    transaction.Rollback();
                                    throw exy;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Update IsUse Error");
                        }
                        #endregion

                        // Assign Qty for  wave Loop
                        State = "planGIWaveResult3";
                        olog.logging("runwave" + threadnum.ToString(), State);
                        strwhere.isuse = model.goodsIssue_Index;
                        strwhere.isActive = true;
                        int? WhereQtyBal = null;
                        decimal? qty_Per_Tag = null;
                        qty_Per_Tag = listDataProduct2.FirstOrDefault(c => c.product_Id == planGIWaveResult3.FirstOrDefault().Product_Id).qty_Per_Tag;

                        //if (waveRule.rule_Name.ToUpper() == "FULL")
                        //{
                        //    strwhere.qtyPreTag = listDataProduct2.FirstOrDefault(c => c.product_Id == planGIWaveResult3.FirstOrDefault().Product_Id).qty_Per_Tag;
                        //    WhereQtyBal = Convert.ToInt16(Math.Floor((planGIWaveResult3.Sum(s => s.TotalQty) ?? 0) / (qty_Per_Tag ?? 1)));
                        //    var chkFull = CraterGILBy_Binbalance(WhereQtyBal, qty_Per_Tag, strwhere, listDataProduct2, model, getViewWaveTemplate.ToList(), planGIWaveResult3.ToList(), listDataLocation2);
                        //    #region update isuse = '' and runwave 50
                        //    strwhere.isActive = true;
                        //    State = "region update isuse = '' and runwave 50 1";
                        //    var updateIsuseViewBinbalance2 = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateIsuseViewBinbalance"), strwhere.sJson());
                        //    if (updateIsuseViewBinbalance2)
                        //    {
                        //        using (var db5 = new GIDbContext())
                        //        {
                        //            var transaction = db5.Database.BeginTransaction();
                        //            try
                        //            {
                        //                var GI = db5.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 40).ToList();
                        //                foreach (var g in GI)
                        //                {
                        //                    g.RunWave_Status = 50;
                        //                }
                        //                db5.SaveChanges();
                        //                transaction.Commit();
                        //            }
                        //            catch (Exception exy)
                        //            {
                        //                msglog = State + " ex Rollback " + exy.Message.ToString();
                        //                olog.logging("runwave" + threadnum.ToString(), msglog);
                        //                transaction.Rollback();
                        //                throw exy;
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        throw new Exception("Update IsUse By Error");
                        //    }
                        //    #endregion
                        //    continue;
                        //    //if (chkFull)
                        //    //{
                        //    //    continue;
                        //    //}
                        //    //else
                        //    //{
                        //    //    var resultFull = new actionResultRunWaveV2ViewModelViewModel();
                        //    //    resultFull.resultIsUse = false;
                        //    //    resultFull.resultMsg = "Error Full";
                        //    //    return result;
                        //    //}
                        //}

                        decimal? QtyPlanGIRemian = 0;
                        //     foreach (var itemPlanGI in planGIWaveResult3.OrderBy(c => c.LineNum).ThenBy(d => d.DocumentPriority_Status))
                        foreach (var itemPlanGI in planGIWaveResult3.OrderBy(c => c.LineNum))
                        {

                            if (itemPlanGI.ModPlanGI == 0)
                            {
                                IsPA = 0;

                            }
                            else
                            {
                                IsPA = 1;
                            }



                            //listDataProduct2



                            State = "listDataProduct2.ToList()";
                            olog.logging("runwave" + threadnum.ToString(), State);
                            var listProducttote = listDataProduct2.Where(c => c.product_Index == itemPlanGI.Product_Index).ToList();
                            if (listProducttote != null)
                            {
                                var checkProduct = listProducttote.Where(c => c.product_Index == itemPlanGI.Product_Index).FirstOrDefault();


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
                            //chkBinCardReserve = GIL

                            //}

                            #region view_waveBinbalance2
                            //strwhere.isuse = model.goodsIssue_Index;
                            //strwhere.isActive = true;
                            State = "getViewBinbalanceapi" + strwhere.sJson().ToString();
                            olog.logging("runwave" + threadnum.ToString(), State);

                            var listDataBinbalance2 = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getViewBinbalance"), strwhere.sJson());

                            var checklistDataBinbalance2 = listDataBinbalance2.ToList();

                            State = "CountgetViewBinbalance : " + checklistDataBinbalance2.Count().ToString(); ;
                            olog.logging("runwave" + threadnum.ToString(), State);



                            State = "View_WaveCheckProductLot";
                            olog.logging("runwave" + threadnum.ToString(), State);

                            var ListLot = new List<String>();
                            var ListLotNotWave = new List<String>();
                            // get All Product Lot Status not Wave by Product
                            var listAll_Lot = dbThread.View_WaveCheckProductLot.Where(c => c.Product_Index == itemPlanGI.Product_Index).ToList();

                            // get lot in plan gi amd product
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
                            olog.logging("runwave" + threadnum.ToString(), State);
                            var GIDate = model.goodsIssue_Date.toDate();
                            var View_WaveBinBalance2 = (from BB in listDataBinbalance2
                                                        join LC in listDataLocation2 on BB.location_Index equals LC.location_Index into gj
                                                        from L in gj.DefaultIfEmpty()
                                                        join Prd in listDataProduct2 on BB.product_Index equals Prd.product_Index
                                                        where (BB.goodsReceive_EXP_Date != null ? BB.goodsReceive_EXP_Date.sParse<DateTime>().Subtract(DateTime.Now.AddDays(-1)).Days : 1) > (Prd.ProductShelfLife_D ?? 0)
                                                        && !(L?.locationType_Index == Guid.Parse("14C5F85D-137D-470E-8C70-C1E535005DC3")
                                                            || L?.locationType_Index == Guid.Parse("2E9338D3-0931-4E36-B240-782BF9508641")
                                                            || L?.locationType_Index == Guid.Parse("65A2D25D-5520-47D3-8776-AE064D909285")
                                                            || L?.locationType_Index == Guid.Parse("94D86CEA-3D04-4304-9E97-28E954F03C35")
                                                            || L?.locationType_Index == Guid.Parse("64341969-E596-4B8B-8836-395061777490")
                                                            || L?.locationType_Index == Guid.Parse("6A1FB140-CC78-4C2B-BEC8-42B2D0AE62E9")
                                                            || L?.locationType_Index == Guid.Parse("F9EDDAEC-A893-4F63-A700-526C69CC0774")
                                                            || L?.locationType_Index == Guid.Parse("A1F7BFA0-1429-4010-863D-6A0EB01DB61D")
                                                            || L?.locationType_Index == Guid.Parse("472E5117-3A7A-4C23-B8C2-7FEA55B3E69C")
                                                            || L?.locationType_Index == Guid.Parse("7D30298A-8BA0-47ED-8342-E3F953E11D8C")
                                                            || L?.locationType_Index == Guid.Parse("A706D789-F5C9-41A6-BEC7-E57034DFC166")
                                                            || L?.locationType_Index == Guid.Parse("E4310B71-D6A7-4FF6-B4A8-EACBDFADAFFC")
                                                            || L?.locationType_Index == Guid.Parse("D4DFC92C-C5DC-4397-BF87-FEEEB579C0AF")
                                                            || L?.locationType_Index == Guid.Parse("3a7d807a-9f2c-4215-8703-f51846bcc4bd")
                                                            || L?.locationType_Index == Guid.Parse("DEA384FD-3EEF-49A2-A88C-04ABA5C114A7")
                                                            || L?.locationType_Index == Guid.Parse("8A545442-77A3-43A4-939A-6B9102DFE8C6")  // Replen
                                                            || L?.locationType_Index == Guid.Parse("1D2DF268-F004-4820-831F-B823FF9C7564")
                                                            || L?.locationType_Index == Guid.Parse("DEA384FD-3EEF-49A2-A88C-04ABA5C114A7")
                                                         //   || L?.locationType_Index == Guid.Parse("48F83BB5-7807-4B32-9E3C-74962CEF92E8") //LBL
                                                         //

                                                         //   || L?.locationType_Index == Guid.Parse("E77778D2-7A8E-448D-BA31-CD35FD938FC3")   // PA
                                                         //|| L?.locationType_Index == Guid.Parse("7F3E1BC2-F18B-4B16-80A9-2394EB8BBE63")   // VC
                                                         //|| L?.locationType_Index == Guid.Parse("E4F856EA-9685-45A4-995C-C05FF9E499C4")   // Partial

                                                         //

                                                         )


                                                        // && (waveRule.rule_Name.ToUpper() == "PATIAL" ? (BB.binBalance_QtyBal - BB.binBalance_QtyReserve) < qty_Per_Tag : BB.binBalance_QtyBal > 0)
                                                        && !(ListLotNotWave.Contains(BB.product_Lot))
                                                        //  && waveRule.rule_Name.ToUpper() == "PATIAL"
                                                        && (BB.binBalance_QtyBal) > 0
                                                        && (BB.binBalance_QtyReserve) >= 0
                                                        && (L?.BlockPick ?? 0) != 1
                                                        //  && (BB?.product_Lot ?? "") == itemPlanGI.Product_Lot
                                                        && BB.goodsReceive_Date.Date <= GIDate.Value.Date
                                                        && (string.IsNullOrEmpty(itemPlanGI.ERP_Location) ? (BB.erp_Location ?? "") == "" : BB.erp_Location == itemPlanGI.ERP_Location)
                                                        select new View_WaveBinBalanceViewModel
                                                        {
                                                            binBalance_Index = BB.binBalance_Index.ToString(),
                                                            owner_Index = BB.owner_Index.ToString(),
                                                            owner_Id = BB.owner_Id,
                                                            owner_Name = BB.owner_Name,
                                                            location_Index = BB.location_Index.ToString(),
                                                            location_Id = BB.location_Id,
                                                            location_Name = BB.location_Name,
                                                            goodsReceive_Index = BB.goodsReceive_Index.ToString(),
                                                            goodsReceive_No = BB.goodsReceive_No,
                                                            goodsReceive_Date = BB.goodsReceive_Date.ToString(),
                                                            goodsReceiveItem_Index = BB.goodsReceiveItem_Index.ToString(),
                                                            goodsReceiveItemLocation_Index = BB.goodsReceiveItemLocation_Index.ToString(),
                                                            tagItem_Index = BB.tagItem_Index.ToString(),
                                                            tag_Index = BB.tag_Index.ToString(),
                                                            tag_No = BB.tag_No,
                                                            product_Index = BB.product_Index.ToString(),
                                                            product_Id = BB.product_Id,
                                                            product_Name = BB.product_Name,
                                                            product_SecondName = BB.product_SecondName,
                                                            product_ThirdName = BB.product_ThirdName,
                                                            product_Lot = BB.product_Lot,
                                                            itemStatus_Index = BB.itemStatus_Index.ToString(),
                                                            itemStatus_Id = BB.itemStatus_Id,
                                                            itemStatus_Name = BB.itemStatus_Name,
                                                            goodsReceive_MFG_Date = BB.goodsReceive_MFG_Date.ToString(),
                                                            goodsReceive_EXP_Date = BB.goodsReceive_EXP_Date.ToString(),
                                                            goodsReceive_ProductConversion_Index = BB.goodsReceive_ProductConversion_Index.ToString(),
                                                            goodsReceive_ProductConversion_Id = BB.goodsReceive_ProductConversion_Id.ToString(),
                                                            goodsReceive_ProductConversion_Name = BB.goodsReceive_ProductConversion_Name.ToString(),


                                                            binBalance_Ratio = BB.binBalance_Ratio,
                                                            binBalance_QtyBegin = BB.binBalance_QtyBegin,
                                                            binBalance_WeightBegin = BB.binBalance_WeightBegin,
                                                            binBalance_WeightBegin_Index = BB.binBalance_WeightBegin_Index,
                                                            binBalance_WeightBegin_Id = BB.binBalance_WeightBegin_Id,
                                                            binBalance_WeightBegin_Name = BB.binBalance_WeightBegin_Name,
                                                            binBalance_WeightBeginRatio = BB.binBalance_WeightBeginRatio,
                                                            binBalance_NetWeightBegin = BB.binBalance_NetWeightBegin,
                                                            binBalance_NetWeightBegin_Index = BB.binBalance_NetWeightBegin_Index,
                                                            binBalance_NetWeightBegin_Id = BB.binBalance_NetWeightBegin_Id,
                                                            binBalance_NetWeightBegin_Name = BB.binBalance_NetWeightBegin_Name,
                                                            binBalance_NetWeightBeginRatio = BB.binBalance_NetWeightBeginRatio,
                                                            binBalance_GrsWeightBegin = BB.binBalance_GrsWeightBegin,
                                                            binBalance_GrsWeightBegin_Index = BB.binBalance_GrsWeightBegin_Index,
                                                            binBalance_GrsWeightBegin_Id = BB.binBalance_GrsWeightBegin_Id,
                                                            binBalance_GrsWeightBegin_Name = BB.binBalance_GrsWeightBegin_Name,
                                                            binBalance_GrsWeightBeginRatio = BB.binBalance_GrsWeightBeginRatio,
                                                            binBalance_WidthBegin = BB.binBalance_WidthBegin,
                                                            binBalance_WidthBegin_Index = BB.binBalance_WidthBegin_Index,
                                                            binBalance_WidthBegin_Id = BB.binBalance_WidthBegin_Id,
                                                            binBalance_WidthBegin_Name = BB.binBalance_WidthBegin_Name,
                                                            binBalance_WidthBeginRatio = BB.binBalance_WidthBeginRatio,
                                                            binBalance_LengthBegin = BB.binBalance_LengthBegin,
                                                            binBalance_LengthBegin_Index = BB.binBalance_LengthBegin_Index,
                                                            binBalance_LengthBegin_Id = BB.binBalance_LengthBegin_Id,
                                                            binBalance_LengthBegin_Name = BB.binBalance_LengthBegin_Name,
                                                            binBalance_LengthBeginRatio = BB.binBalance_LengthBeginRatio,
                                                            binBalance_HeightBegin = BB.binBalance_HeightBegin,
                                                            binBalance_HeightBegin_Index = BB.binBalance_HeightBegin_Index,
                                                            binBalance_HeightBegin_Id = BB.binBalance_HeightBegin_Id,
                                                            binBalance_HeightBegin_Name = BB.binBalance_HeightBegin_Name,
                                                            binBalance_HeightBeginRatio = BB.binBalance_HeightBeginRatio,
                                                            binBalance_UnitVolumeBegin = BB.binBalance_UnitVolumeBegin,
                                                            binBalance_VolumeBegin = BB.binBalance_VolumeBegin,
                                                            binBalance_QtyBal = BB.binBalance_QtyBal,
                                                            binBalance_UnitWeightBal = BB.binBalance_UnitWeightBal,
                                                            binBalance_UnitWeightBal_Index = BB.binBalance_UnitWeightBal_Index,
                                                            binBalance_UnitWeightBal_Id = BB.binBalance_UnitWeightBal_Id,
                                                            binBalance_UnitWeightBal_Name = BB.binBalance_UnitWeightBal_Name,
                                                            binBalance_UnitWeightBalRatio = BB.binBalance_UnitWeightBalRatio,
                                                            binBalance_WeightBal = BB.binBalance_WeightBal,
                                                            binBalance_WeightBal_Index = BB.binBalance_WeightBal_Index,
                                                            binBalance_WeightBal_Id = BB.binBalance_WeightBal_Id,
                                                            binBalance_WeightBal_Name = BB.binBalance_WeightBal_Name,
                                                            binBalance_WeightBalRatio = BB.binBalance_WeightBalRatio,
                                                            binBalance_UnitNetWeightBal = BB.binBalance_UnitNetWeightBal,
                                                            binBalance_UnitNetWeightBal_Index = BB.binBalance_UnitNetWeightBal_Index,
                                                            binBalance_UnitNetWeightBal_Id = BB.binBalance_UnitNetWeightBal_Id,
                                                            binBalance_UnitNetWeightBal_Name = BB.binBalance_UnitNetWeightBal_Name,
                                                            binBalance_UnitNetWeightBalRatio = BB.binBalance_UnitNetWeightBalRatio,
                                                            binBalance_NetWeightBal = BB.binBalance_NetWeightBal,
                                                            binBalance_NetWeightBal_Index = BB.binBalance_NetWeightBal_Index,
                                                            binBalance_NetWeightBal_Id = BB.binBalance_NetWeightBal_Id,
                                                            binBalance_NetWeightBal_Name = BB.binBalance_NetWeightBal_Name,
                                                            binBalance_NetWeightBalRatio = BB.binBalance_NetWeightBalRatio,
                                                            binBalance_UnitGrsWeightBal = BB.binBalance_UnitGrsWeightBal,
                                                            binBalance_UnitGrsWeightBal_Index = BB.binBalance_UnitGrsWeightBal_Index,
                                                            binBalance_UnitGrsWeightBal_Id = BB.binBalance_UnitGrsWeightBal_Id,
                                                            binBalance_UnitGrsWeightBal_Name = BB.binBalance_UnitGrsWeightBal_Name,
                                                            binBalance_UnitGrsWeightBalRatio = BB.binBalance_UnitGrsWeightBalRatio,
                                                            binBalance_GrsWeightBal = BB.binBalance_GrsWeightBal,
                                                            binBalance_GrsWeightBal_Index = BB.binBalance_GrsWeightBal_Index,
                                                            binBalance_GrsWeightBal_Id = BB.binBalance_GrsWeightBal_Id,
                                                            binBalance_GrsWeightBal_Name = BB.binBalance_GrsWeightBal_Name,
                                                            binBalance_GrsWeightBalRatio = BB.binBalance_GrsWeightBalRatio,
                                                            binBalance_UnitWidthBal = BB.binBalance_UnitWidthBal,
                                                            binBalance_UnitWidthBal_Index = BB.binBalance_UnitWidthBal_Index,
                                                            binBalance_UnitWidthBal_Id = BB.binBalance_UnitWidthBal_Id,
                                                            binBalance_UnitWidthBal_Name = BB.binBalance_UnitWidthBal_Name,
                                                            binBalance_UnitWidthBalRatio = BB.binBalance_UnitWidthBalRatio,
                                                            binBalance_WidthBal = BB.binBalance_WidthBal,
                                                            binBalance_WidthBal_Index = BB.binBalance_WidthBal_Index,
                                                            binBalance_WidthBal_Id = BB.binBalance_WidthBal_Id,
                                                            binBalance_WidthBal_Name = BB.binBalance_WidthBal_Name,
                                                            binBalance_WidthBalRatio = BB.binBalance_WidthBalRatio,
                                                            binBalance_UnitLengthBal = BB.binBalance_UnitLengthBal,
                                                            binBalance_UnitLengthBal_Index = BB.binBalance_UnitLengthBal_Index,
                                                            binBalance_UnitLengthBal_Id = BB.binBalance_UnitLengthBal_Id,
                                                            binBalance_UnitLengthBal_Name = BB.binBalance_UnitLengthBal_Name,
                                                            binBalance_UnitLengthBalRatio = BB.binBalance_UnitLengthBalRatio,
                                                            binBalance_LengthBal = BB.binBalance_LengthBal,
                                                            binBalance_LengthBal_Index = BB.binBalance_LengthBal_Index,
                                                            binBalance_LengthBal_Id = BB.binBalance_LengthBal_Id,
                                                            binBalance_LengthBal_Name = BB.binBalance_LengthBal_Name,
                                                            binBalance_LengthBalRatio = BB.binBalance_LengthBalRatio,
                                                            binBalance_UnitHeightBal = BB.binBalance_UnitHeightBal,
                                                            binBalance_UnitHeightBal_Index = BB.binBalance_UnitHeightBal_Index,
                                                            binBalance_UnitHeightBal_Id = BB.binBalance_UnitHeightBal_Id,
                                                            binBalance_UnitHeightBal_Name = BB.binBalance_UnitHeightBal_Name,
                                                            binBalance_UnitHeightBalRatio = BB.binBalance_UnitHeightBalRatio,
                                                            binBalance_HeightBal = BB.binBalance_HeightBal,
                                                            binBalance_HeightBal_Index = BB.binBalance_HeightBal_Index,
                                                            binBalance_HeightBal_Id = BB.binBalance_HeightBal_Id,
                                                            binBalance_HeightBal_Name = BB.binBalance_HeightBal_Name,
                                                            binBalance_HeightBalRatio = BB.binBalance_HeightBalRatio,
                                                            binBalance_UnitVolumeBal = BB.binBalance_UnitVolumeBal,
                                                            binBalance_VolumeBal = BB.binBalance_VolumeBal,
                                                            binBalance_QtyReserve = BB.binBalance_QtyReserve,
                                                            binBalance_WeightReserve = BB.binBalance_WeightReserve,
                                                            binBalance_WeightReserve_Index = BB.binBalance_WeightReserve_Index,
                                                            binBalance_WeightReserve_Id = BB.binBalance_WeightReserve_Id,
                                                            binBalance_WeightReserve_Name = BB.binBalance_WeightReserve_Name,
                                                            binBalance_WeightReserveRatio = BB.binBalance_WeightReserveRatio,
                                                            binBalance_NetWeightReserve = BB.binBalance_NetWeightReserve,
                                                            binBalance_NetWeightReserve_Index = BB.binBalance_NetWeightReserve_Index,
                                                            binBalance_NetWeightReserve_Id = BB.binBalance_NetWeightReserve_Id,
                                                            binBalance_NetWeightReserve_Name = BB.binBalance_NetWeightReserve_Name,
                                                            binBalance_NetWeightReserveRatio = BB.binBalance_NetWeightReserveRatio,
                                                            binBalance_GrsWeightReserve = BB.binBalance_GrsWeightReserve,
                                                            binBalance_GrsWeightReserve_Index = BB.binBalance_GrsWeightReserve_Index,
                                                            binBalance_GrsWeightReserve_Id = BB.binBalance_GrsWeightReserve_Id,
                                                            binBalance_GrsWeightReserve_Name = BB.binBalance_GrsWeightReserve_Name,
                                                            binBalance_GrsWeightReserveRatio = BB.binBalance_GrsWeightReserveRatio,
                                                            binBalance_WidthReserve = BB.binBalance_WidthReserve,
                                                            binBalance_WidthReserve_Index = BB.binBalance_WidthReserve_Index,
                                                            binBalance_WidthReserve_Id = BB.binBalance_WidthReserve_Id,
                                                            binBalance_WidthReserve_Name = BB.binBalance_WidthReserve_Name,
                                                            binBalance_WidthReserveRatio = BB.binBalance_WidthReserveRatio,
                                                            binBalance_LengthReserve = BB.binBalance_LengthReserve,
                                                            binBalance_LengthReserve_Index = BB.binBalance_LengthReserve_Index,
                                                            binBalance_LengthReserve_Id = BB.binBalance_LengthReserve_Id,
                                                            binBalance_LengthReserve_Name = BB.binBalance_LengthReserve_Name,
                                                            binBalance_LengthReserveRatio = BB.binBalance_LengthReserveRatio,
                                                            binBalance_HeightReserve = BB.binBalance_HeightReserve,
                                                            binBalance_HeightReserve_Index = BB.binBalance_HeightReserve_Index,
                                                            binBalance_HeightReserve_Id = BB.binBalance_HeightReserve_Id,
                                                            binBalance_HeightReserve_Name = BB.binBalance_HeightReserve_Name,
                                                            binBalance_HeightReserveRatio = BB.binBalance_HeightReserveRatio,
                                                            binBalance_UnitVolumeReserve = BB.binBalance_UnitVolumeReserve,
                                                            binBalance_VolumeReserve = BB.binBalance_VolumeReserve,


                                                            productConversion_Index = BB.productConversion_Index.ToString(),
                                                            productConversion_Id = BB.productConversion_Id,
                                                            productConversion_Name = BB.productConversion_Name,

                                                            unitPrice = BB.unitPrice,
                                                            unitPrice_Index = BB.unitPrice_Index,
                                                            unitPrice_Id = BB.unitPrice_Id,
                                                            unitPrice_Name = BB.unitPrice_Name,
                                                            price = BB.price,
                                                            price_Index = BB.price_Index,
                                                            price_Id = BB.price_Id,
                                                            price_Name = BB.price_Name,

                                                            udf_1 = BB.uDF_1,
                                                            udf_2 = BB.uDF_2,
                                                            udf_3 = BB.uDF_3,
                                                            udf_4 = BB.uDF_4,
                                                            udf_5 = BB.uDF_5,
                                                            create_By = BB.create_By,
                                                            create_Date = BB.create_Date.ToString(),
                                                            update_By = BB.update_By,
                                                            update_Date = BB.update_Date.ToString(),
                                                            cancel_By = BB.cancel_By,
                                                            cancel_Date = BB.cancel_Date.ToString(),
                                                            isUse = BB.isUse,
                                                            binBalance_Status = BB.binBalance_Status,
                                                            picking_Seq = L?.picking_Seq,
                                                            ageRemain = BB.ageRemain,

                                                            invoice_No = BB.invoice_No,
                                                            declaration_No = BB.declaration_No,
                                                            hs_Code = BB.hs_Code,
                                                            conutry_of_Origin = BB.conutry_of_Origin,
                                                            tax1 = BB.tax1,
                                                            tax1_Currency_Index = BB.tax1_Currency_Index,
                                                            tax1_Currency_Id = BB.tax1_Currency_Id,
                                                            tax1_Currency_Name = BB.tax1_Currency_Name,
                                                            tax2 = BB.tax2,
                                                            tax2_Currency_Index = BB.tax2_Currency_Index,
                                                            tax2_Currency_Id = BB.tax2_Currency_Id,
                                                            tax2_Currency_Name = BB.tax2_Currency_Name,
                                                            tax3 = BB.tax3,
                                                            tax3_Currency_Index = BB.tax3_Currency_Index,
                                                            tax3_Currency_Id = BB.tax3_Currency_Id,
                                                            tax3_Currency_Name = BB.tax3_Currency_Name,
                                                            tax4 = BB.tax4,
                                                            tax4_Currency_Index = BB.tax4_Currency_Index,
                                                            tax4_Currency_Id = BB.tax4_Currency_Id,
                                                            tax4_Currency_Name = BB.tax4_Currency_Name,
                                                            tax5 = BB.tax5,
                                                            tax5_Currency_Index = BB.tax5_Currency_Index,
                                                            tax5_Currency_Id = BB.tax5_Currency_Id,
                                                            tax5_Currency_Name = BB.tax5_Currency_Name,

                                                            erp_Location = BB.erp_Location,
                                                            productShelfLife_D = Prd.ProductShelfLife_D != null ? Prd.ProductShelfLife_D : 0,
                                                        }).AsQueryable();


                            #endregion

                            var CheckBinBalanceResult = View_WaveBinBalance2.ToList();

                            State = "CountgetView_WaveBinBalance2 : " + CheckBinBalanceResult.Count().ToString(); ;
                            olog.logging("runwave" + threadnum.ToString(), State);


                            #region for RuleDesSort
                            var RuleDesList2 = getViewWaveTemplate.Where(c => c.isDestination == 1 && c.isSearch == 1).ToList();

                            //var lambda = Expression.Lambda<Func<View_PLANWAVEbyPLANGIV2, bool>>(e, param);

                            foreach (var itemRuleDesList in RuleDesList2)
                            {
                                if (itemRuleDesList.ruleConditionOperation == "IN")
                                {
                                    if (itemRuleDesList.ruleConditionField_Name == "Zone_Id")
                                    {
                                        //setWhereDes += " And  Location_Index   in (  select   Location_Index  from ms_ZoneLocation   where  IsDelete <> -1 and Zone_Index  in (select Zone_Index from ms_Zone where IsDelete <> -1 and " + itemRuleDesList.ruleConditionField_Name + " IN (" + itemRuleDesList.ruleCondition_Param + " )" + ")) ";
                                        var listDataZoneLocation = utils.SendDataApi<List<ZoneLocationViewModel>>(new AppSettingConfig().GetUrl("getZoneLocationMaster"), new { }.sJson());
                                        var listDataZone = utils.SendDataApi<List<ZoneViewModel>>(new AppSettingConfig().GetUrl("getZoneMaster"), new { }.sJson());

                                        //var param = Expression.Parameter(typeof(ZoneViewModel), "x");
                                        //var predicate = Expression.Lambda<Func<ZoneViewModel, bool>>(
                                        //    Expression.Call(
                                        //        Expression.PropertyOrField(param, itemRuleDesList.ruleConditionField_Name),
                                        //        "Contains", null, Expression.Constant(itemRuleDesList.ruleCondition_Param)
                                        //    ), param);
                                        var dataarray = itemRuleDesList.ruleCondition_Param.Replace("'", "").Split(',');
                                        var dataZone = listDataZone.Where(c => dataarray.Contains(c.zone_Id)).ToList();
                                        var zoneArray = new List<Guid?>();
                                        foreach (var z in dataZone)
                                        {
                                            zoneArray.Add(z.zone_Index);
                                        }
                                        var listLocation_index = listDataZoneLocation.Where(c => zoneArray.Contains(c.zone_Index)).Select(s => s.location_Index.ToString()).ToList();
                                        View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => listLocation_index.Contains(c.location_Index));
                                    }
                                    else
                                    {
                                        var dataarray = itemRuleDesList.ruleCondition_Param.Replace("'", "").Split(',');
                                        if (itemRuleDesList.ruleConditionField_Name == "GoodsReceive_Date")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.goodsReceive_Date));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "GoodsReceive_No")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.goodsReceive_No));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "ItemStatus_Id")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.itemStatus_Id));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Location_Name")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.location_Name));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Locationtype_Id")
                                        {
                                            //View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.location));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Product_Id")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.product_Id));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Product_Lot")
                                        {
                                            View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => dataarray.Contains(c.product_Lot));
                                        }
                                        if (itemRuleDesList.ruleConditionField_Name == "Warehouse")
                                        {
                                            //planGIWaveResult = planGIWaveResult.Where(c => dataarray.Contains(c.Warehouse));
                                        }

                                        //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " IN (" + itemRuleDesList.ruleCondition_Param + ") ";
                                        //var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                        //var predicate = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>(
                                        //    Expression.Call(
                                        //        Expression.PropertyOrField(param, itemRuleDesList.ruleConditionField_Name),
                                        //        "Contains", null, Expression.Constant(itemRuleDesList.ruleCondition_Param)
                                        //    ), param);
                                        //View_WaveBinBalance = View_WaveBinBalance.Where(predicate);
                                    }
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "=")
                                {
                                    if (itemRuleDesList.ruleConditionField_Name == "Zone_Id")
                                    {
                                        //setWhereDes += "  And  Location_Index   in (  select   Location_Index  from ms_ZoneLocation   where IsDelete <> -1 and Zone_Index  in (select Zone_Index from ms_Zone where IsDelete <> -1 and " + itemRuleDesList.ruleConditionField_Name + " = '" + itemRuleDesList.ruleCondition_Param + "' " + ")) ";
                                        var listDataZoneLocation = utils.SendDataApi<List<ZoneLocationViewModel>>(new AppSettingConfig().GetUrl("getZoneLocationMaster"), new { }.sJson());
                                        var listDataZone = utils.SendDataApi<List<ZoneViewModel>>(new AppSettingConfig().GetUrl("getZoneMaster"), new { }.sJson());

                                        var param = Expression.Parameter(typeof(ZoneViewModel), "x");
                                        var lambda = Expression.Lambda<Func<ZoneViewModel, bool>>((Expression)Expression.Equal(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                            , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                        var dataZone = listDataZone.AsQueryable().Where(lambda).ToList();
                                        var zoneArray = new List<Guid?>();
                                        foreach (var z in dataZone)
                                        {
                                            zoneArray.Add(z.zone_Index);
                                        }
                                        var listLocation_index = listDataZoneLocation.Where(c => zoneArray.Contains(c.zone_Index)).Select(s => s.location_Index.ToString()).ToList();
                                        View_WaveBinBalance2 = View_WaveBinBalance2.Where(c => listLocation_index.Contains(c.location_Index));
                                    }
                                    else
                                    {
                                        //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " = '" + itemRuleDesList.ruleCondition_Param + "' ";
                                        var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                        var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.Equal(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                            , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                        View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                    }
                                }
                                else if (itemRuleDesList.ruleConditionOperation == ">")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " > '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.GreaterThan(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "<")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " < '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.LessThan(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "!=")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " != '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.NotEqual(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == ">=")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " >= '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.GreaterThanOrEqual(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "<=")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " <= '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>((Expression)Expression.LessThanOrEqual(Expression.Property(param, itemRuleDesList.ruleConditionField_Name)
                                        , Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""), typeof(string))), param);

                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(lambda);
                                }
                                else if (itemRuleDesList.ruleConditionOperation == "Like")
                                {
                                    //setWhereDes += " And " + itemRuleDesList.ruleConditionField_Name + " Like '" + itemRuleDesList.ruleCondition_Param + "' ";
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var predicate = Expression.Lambda<Func<View_WaveBinBalanceViewModel, bool>>(
                                        Expression.Call(
                                            Expression.PropertyOrField(param, itemRuleDesList.ruleConditionField_Name),
                                            "Contains", null, Expression.Constant(itemRuleDesList.ruleCondition_Param.Replace("'", ""))
                                        ), param);
                                    View_WaveBinBalance2 = View_WaveBinBalance2.Where(predicate);
                                }
                                //setWhereDes += "";

                            }
                            #endregion

                            #region for RuleDesSortList
                            var RuleDesSortList2 = getViewWaveTemplate.Where(c => c.isDestination == 1 && c.isSort == 1).ToList();

                            int iRowsDesSort2 = 0;
                            foreach (var itemRuleDesSortList in RuleDesSortList2)
                            {
                                if (iRowsDesSort2 == 0)
                                {
                                    //setWhereDesSort += itemRuleDesSortList.ruleConditionField_Name + ' ' + itemRuleDesSortList.ruleCondition_Param;
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, string>>(Expression.Property(param, itemRuleDesSortList.ruleConditionField_Name), param);
                                    if (itemRuleDesSortList.ruleCondition_Param.ToUpper() == "DESC")
                                    {
                                        View_WaveBinBalance2 = View_WaveBinBalance2.OrderByDescending(lambda);
                                    }
                                    else /*if (itemRuleDesSortList.ruleCondition_Param.ToUpper() == "DESC")*/
                                    {
                                        View_WaveBinBalance2 = View_WaveBinBalance2.OrderBy(lambda);
                                    }
                                }
                                else
                                {
                                    var param = Expression.Parameter(typeof(View_WaveBinBalanceViewModel), "x");
                                    var lambda = Expression.Lambda<Func<View_WaveBinBalanceViewModel, string>>(Expression.Property(param, itemRuleDesSortList.ruleConditionField_Name), param);
                                    if (itemRuleDesSortList.ruleCondition_Param.ToUpper() == "DESC")
                                    {
                                        View_WaveBinBalance2 = View_WaveBinBalance2.OrderByDescending(lambda);
                                    }
                                    else /*if (itemRuleDesSortList.ruleCondition_Param.ToUpper() == "DESC")*/
                                    {
                                        View_WaveBinBalance2 = View_WaveBinBalance2.OrderBy(lambda);
                                    }
                                }
                                iRowsDesSort2 = iRowsDesSort2 + 1;

                            }
                            #endregion




                            State = "View_WaveBinBalance2";
                            olog.logging("runwave" + threadnum.ToString(), State);
                            var BinBalanceResult = View_WaveBinBalance2.ToList();

                            //  foreach (var itemBin in BinBalanceResult.OrderBy(c => c.goodsReceive_EXP_Date).ThenBy(d => d.goodsReceive_Date).ThenBy(e => e.picking_Seq).ThenBy(f => f.location_Name))



                            State = "CountgetBinBalanceResult : " + BinBalanceResult.Count().ToString(); ;
                            olog.logging("runwave" + threadnum.ToString(), State);



                            var itemBinSort = new List<View_WaveBinBalanceViewModel>();

                            if (IsPA == 1 && IsTote == true)
                            {
                                itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenBy(f => f.location_Name).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ToList();

                            }
                            else if (IsPA == 0 && IsTote == true)
                            {
                                itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenByDescending(f => f.location_Name).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ToList();
                            }
                            else
                            {
                                itemBinSort = BinBalanceResult.OrderBy(c => c.picking_Seq).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name).ToList();

                            }



                            //  foreach (var itemBin in BinBalanceResult.OrderBy(c => c.picking_Seq).ThenBy(d => d.ageRemain).ThenBy(e => e.goodsReceive_Date).ThenBy(f => f.location_Name))
                            foreach (var itemBin in itemBinSort)
                            {

                                decimal? QtyBal = itemBin.binBalance_QtyBal - itemBin.binBalance_QtyReserve;
                                if (QtyPlanGIRemian <= 0)
                                {
                                    break;
                                }
                                if (QtyBal <= 0)
                                {
                                    continue;
                                }
                                if (QtyPlanGIRemian >= QtyBal && QtyBal > 0)
                                {
                                    State = "QtyPlanGIRemian >= QtyBal && QtyBal > 0";
                                    olog.logging("runwave" + threadnum.ToString(), State + " TAG_NO " + itemBin.tag_No + " Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);

                                    // Add GI ITEMLOCATION 
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
                                    //GoodsIssueItemLocation.ProductConversion_Index = new Guid(itemBin.productConversion_Index);
                                    //GoodsIssueItemLocation.ProductConversion_Id = itemBin.productConversion_Id;
                                    //GoodsIssueItemLocation.ProductConversion_Name = itemBin.productConversion_Name;
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
                                    GoodsIssueItemLocation.DocumentRef_No5 = threadnum.ToString(); //itemPlanGI.DocumentRef_No5;
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


                                    itemPlanGI.GITotalQty = itemPlanGI.GITotalQty + QtyBal;

                                    QtyPlanGIRemian = QtyPlanGIRemian - QtyBal;

                                }
                                else if (QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0)
                                {


                                    State = "QtyPlanGIRemian < QtyBal && QtyPlanGIRemian > 0 && QtyBal > 0";
                                    olog.logging("runwave" + threadnum.ToString(), State + " TAG_NO " + itemBin.tag_No + " Product_Id " + itemBin.product_Id + " Bin_Index " + itemBin.binBalance_Index);

                                    var QtyPick = QtyPlanGIRemian;
                                    // Add GI ITEMLOCATION 
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
                                    //GoodsIssueItemLocation.ProductConversion_Index = new Guid(itemBin.productConversion_Index);
                                    //GoodsIssueItemLocation.ProductConversion_Id = itemBin.productConversion_Id;
                                    //GoodsIssueItemLocation.ProductConversion_Name = itemBin.productConversion_Name;
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
                                    GoodsIssueItemLocation.DocumentRef_No5 = threadnum.ToString();   // itemPlanGI.DocumentRef_No5;
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

                                    itemPlanGI.GITotalQty = itemPlanGI.GITotalQty + QtyPick;

                                    QtyPlanGIRemian = QtyPlanGIRemian - QtyPick;

                                }
                            }

                            #region inset GIL and runwave status 30
                            using (var db4 = new GIDbContext())
                            {
                                var transaction = db4.Database.BeginTransaction();
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
                                    olog.logging("runwave" + threadnum.ToString(), msglog);
                                    transaction.Rollback();
                                    throw exy;
                                }
                            }
                            #endregion
                            State = "inset GIL and runwave status 30";
                            olog.logging("runwave" + threadnum.ToString(), State);

                            #region insert bincardreserve and runwave status 40

                            using (var Contact = new GIDbContext())
                            {
                                var GIL = Contact.IM_GoodsIssueItemLocation.Where(c => c.Ref_Document_Index == itemPlanGI.PlanGoodsIssue_Index && c.Ref_DocumentItem_Index == itemPlanGI.PlanGoodsIssueItem_Index && c.Document_Status != -1).ToList();
                                foreach (var g in GIL)
                                {
                                    State = "insertBinCardReserve";

                                    olog.logging("runwave" + threadnum.ToString(), State + " GIIL_Index" + g.GoodsIssueItemLocation_Index.ToString());

                                    var insertBinCardReserve = new PickbinbalanceViewModel();

                                    insertBinCardReserve.ref_Document_Index = g.GoodsIssue_Index.ToString();
                                    insertBinCardReserve.ref_DocumentItem_Index = g.GoodsIssueItemLocation_Index.ToString();
                                    insertBinCardReserve.goodsIssue_No = model.goodsIssue_No;
                                    insertBinCardReserve.Process_Index = "22744590-55D8-4448-88EF-5997C252111F";
                                    //model.GIIL = GoodsIssueItemLocation;
                                    insertBinCardReserve.create_By = model.create_by;
                                    insertBinCardReserve.pick = g.TotalQty;
                                    insertBinCardReserve.binbalance_Index = g.BinBalance_Index.ToString();
                                    insertBinCardReserve.wave_Index = Contact.IM_GoodsIssue.FirstOrDefault(f => f.GoodsIssue_Index == g.GoodsIssue_Index)?.Wave_Index.ToString();

                                    State = "insetBinRe";
                                    olog.logging("runwave" + threadnum.ToString(), State);

                                    var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("insertBinCardReserve"), insertBinCardReserve.sJson());
                                    if (insetBinRe.resultIsUse)
                                    {
                                        State = "resultIsUse";
                                        olog.logging("runwave" + threadnum.ToString(), State);

                                        var transaction = Contact.Database.BeginTransaction();
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
                                            olog.logging("runwave" + threadnum.ToString(), msglog);
                                            transaction.Rollback();
                                            throw exy;
                                        }
                                    }
                                    else
                                    {
                                        State = "resultIsUse else";
                                        olog.logging("runwave" + threadnum.ToString(), State);


                                        var transaction = Contact.Database.BeginTransaction();
                                        try
                                        {
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
                                            olog.logging("runwave" + threadnum.ToString(), msglog);
                                            transaction.Rollback();
                                            throw exy;
                                        }
                                        msglog = State + " ex Rollback " + "Insert BinCardReserve Error";
                                        olog.logging("runwave" + threadnum.ToString(), msglog);
                                        result.resultMsg = "Insert BinCardReserve Error";
                                        result.resultIsUse = false;
                                        //return result;

                                    }
                                }
                            }
                            #endregion
                            State = "insert bincardreserve and runwave status 40";
                            olog.logging("runwave" + threadnum.ToString(), State);
                        }

                        #region update isuse = '' and runwave 50
                        strwhere.isActive = true;
                        State = "updateIsuseViewBinbalance ";
                        var updateIsuseViewBinbalance = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateIsuseViewBinbalance"), strwhere.sJson());
                        if (updateIsuseViewBinbalance)
                        {
                            using (var db5 = new GIDbContext())
                            {
                                var transaction = db5.Database.BeginTransaction();
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
                                    olog.logging("runwave" + threadnum.ToString(), msglog);
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
                        State = "region update isuse = '' and runwave 50";
                        olog.logging("runwave" + threadnum.ToString(), State);
                    }
                }


                result.goodsIssue_Index = model.goodsIssue_Index;
                result.goodsIssue_No = model.goodsIssue_No;

                result.resultIsUse = true;
                State = "end for";
                olog.logging("runwave" + threadnum.ToString(), State);

                //#region Check retrun status PGII 
                //foreach (var itemPGII in model.listGoodsIssueItemViewModel)
                //{
                //    var chkGIL2 = dbThread.IM_GoodsIssueItemLocation.Where(c => c.Ref_Document_Index == itemPGII.planGoodsIssue_Index && c.Ref_DocumentItem_Index == itemPGII.planGoodsIssueItem_Index && c.Document_Status != -1).ToList();
                //    var sumqty = chkGIL2.Sum(s => s.TotalQty);
                //    if (chkGIL2.Count == 0 || sumqty != itemPGII.totalQty)
                //    {

                //        var pgii = model.listGoodsIssueItemViewModel.Where(c => c.planGoodsIssueItem_Index == itemPGII.planGoodsIssueItem_Index).ToList();
                //        foreach (var resultpgii in pgii)
                //        {
                //            resultpgii.qtyPlan = (resultpgii.totalQty - sumqty);
                //            resultpgii.totalQty = (resultpgii.totalQty - sumqty);
                //            listpgiinotinsert.Add(resultpgii);
                //            CheckRunwavePast = true;
                //            using (var updatepginotProduct = new GIDbContext())
                //            {
                //                var transactionresultpgii = updatepginotProduct.Database.BeginTransaction();
                //                try
                //                {

                //                    var updateresultpgii = updatepginotProduct.IM_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssueItem_Index == resultpgii.planGoodsIssueItem_Index && c.Document_Status == 1).ToList();
                //                    foreach (var p in updateresultpgii)
                //                    {
                //                        p.Document_Status = 0;
                //                    }
                //                    updatepginotProduct.SaveChanges();
                //                    transactionresultpgii.Commit();
                //                }

                //                catch (Exception exy)
                //                {
                //                    msglog = State + " ex Rollback " + exy.Message.ToString();
                //                    olog.logging("UpdateUserAssign", msglog);
                //                    transactionresultpgii.Rollback();
                //                    throw exy;
                //                }
                //            }
                //        }
                //    }
                //}
                //#endregion

                //State = "Check retrun status PGII";
                //olog.logging("runwave" + threadnum.ToString(), State);
                //#region update PI status 3 and runwave status 60
                //using (var db5 = new GIDbContext())
                //{
                //    var listPGI = new List<Guid>();
                //    if (model.listGoodsIssueItemViewModel.Count > 0)
                //    {
                //        foreach (var item in model.listGoodsIssueItemViewModel)
                //        {
                //            if (item.planGoodsIssue_Index != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                //            {
                //                listPGI.Add(item.planGoodsIssue_Index);
                //            }
                //        }
                //    }
                //    foreach (var item in listPGI)
                //    {
                //        var pgii = db5.IM_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssue_Index == item && c.Document_Status == 0).Count();
                //        if (pgii == 0)
                //        {
                //            var transaction = db5.Database.BeginTransaction();
                //            try
                //            {
                //                var pgi = db5.IM_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_Index == item).ToList();
                //                foreach (var p in pgi)
                //                {
                //                    p.Document_Status = 3;
                //                }
                //                //var GI = db5.IM_GoodsIssue.Where(c => c.GoodsIssue_Index == Guid.Parse(model.goodsIssue_Index) && c.RunWave_Status == 50).ToList();
                //                //foreach (var g in GI)
                //                //{
                //                //    g.Wave_Index = new Guid(model.wave_Index);
                //                //    g.RunWave_Status = 60;
                //                //}
                //                db5.SaveChanges();
                //                transaction.Commit();
                //            }

                //            catch (Exception exy)
                //            {
                //                msglog = State + " ex Rollback " + exy.Message.ToString();
                //                olog.logging("runwave" + threadnum.ToString(), msglog);
                //                transaction.Rollback();
                //                throw exy;
                //            }
                //        }
                //    }
                //}
                //#endregion

                //State = "update PI status 3 and runwave status 60";
                //olog.logging("runwave" + threadnum.ToString(), State);

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
                olog.logging("runwave" + threadnum.ToString(), msglog);
                var result = new actionResultRunWaveV2ViewModelViewModel();
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }
            finally
            {
                dbThread.Dispose();

            }

        }


    }
}
