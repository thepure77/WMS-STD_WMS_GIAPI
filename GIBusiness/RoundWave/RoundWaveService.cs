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
    public class RoundWaveService
    {
        private GIDbContext db;
        private MasterDbContext dbMaster;

        public RoundWaveService()
        {
            db = new GIDbContext();
            dbMaster = new MasterDbContext();
        }
        public RoundWaveService(GIDbContext db, MasterDbContext dbMaster)
        {
            this.db = db;
            this.dbMaster = dbMaster;
        }

        #region Appointtimefilter
        public List<Appointment_time> Appointtimefilter()
        {
            try
            {
                var result = new List<Appointment_time>();

                var RoundWaveTime = db.RoundWaveTimeAppointment.FromSql("sp_RoundWaveTimeAppointment").ToList();

                foreach (var item in RoundWaveTime)
                {
                    var resultItem = new Appointment_time();
                    resultItem.Appointment_Time = item.Interval_Start + " - "+item.Interval_End;

                    result.Add(resultItem);
                }
                return result.OrderBy(c => c.Appointment_Time).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Filter

        public actionResultRoundWaveViewModel filter(RoundWaveViewModel model)
        {
            actionResultRoundWaveViewModel result = new actionResultRoundWaveViewModel();
            try
            {
                db.Database.SetCommandTimeout(120);

                if (string.IsNullOrEmpty(model.Appointment_Date))
                {
                    result.resultIsUse = false;
                    result.resultMsg = "กรณาระบุค่าให้ครบก่อนค้นหา";
                    return result;
                }
                if (string.IsNullOrEmpty(model.Appointment_Time))
                {
                    result.resultIsUse = false;
                    result.resultMsg = "กรณาระบุค่าให้ครบก่อนค้นหา";
                    return result;
                }
                DateTime dateStart = (model.Appointment_Date.toBetweenDate().start);
                var Appointment_Date = new SqlParameter("@Appointment_Date", dateStart);
                var Appointment_Time = new SqlParameter("@Appointment_Time", model.Appointment_Time);
                var Result_round = db.RoundWave.FromSql("sp_RoundWave @Appointment_Date ,@Appointment_Time", Appointment_Date, Appointment_Time).ToList();
                var TM_NO = Result_round.GroupBy(c => c.TM_No).Select(c => c.Key).ToList();
                List<View_Checkstatus_truckload_order> showData = db.View_Checkstatus_truckload_order.Where(c => TM_NO.Contains(c.TruckLoad_No)).ToList();

                if (Result_round.Count == 0)
                {
                    result.resultIsUse = false;
                    result.resultMsg = " ไม่พบรถที่ทำการค้นหา";
                }
                else {
                    foreach (var item in Result_round)
                    {
                        RoundWaveViewModel round = new RoundWaveViewModel();

                        var is_map = showData.FirstOrDefault(c => c.TruckLoad_No == item.TM_No);

                        round.Appointment_Date = item.Appointment_Date.ToString();
                        round.Appointment_Time = item.Appointment_Time;
                        round.TM_No = item.TM_No;
                        round.Is_map = is_map.Document_Status == 1 ? true : false;
                        round.Dock_Name = item.Dock_Name;
                        round.Appointment_Id = item.Appointment_Id;
                        round.CountOrder = item.CountOrder;
                        round.VehicleType_Name = item.VehicleType_Name;

                        result.itemsDetail.Add(round);
                        result.resultIsUse = true;
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {

                result.resultMsg = ex.Message;
                result.resultIsUse = true;
                return result;
            }
        }
        #endregion

        #region updateRound

        public actionResultRoundWaveViewModel updateRound(RoundWaveViewModel model)
        {
            actionResultRoundWaveViewModel result = new actionResultRoundWaveViewModel();
            try
            {
                db.Database.SetCommandTimeout(120);

                if (string.IsNullOrEmpty(model.planGoodsIssue_Due_Date))
                {
                    result.resultIsUse = false;
                    result.resultMsg = "กรุณาเลือกวันที่ ที่จะทำการตั้งค่า";
                    return result;
                }
                if (string.IsNullOrEmpty(model.round_Index))
                {
                    result.resultIsUse = false;
                    result.resultMsg = "กรุณาเลือกรอบ ที่จะทำการตั้งค่า";
                    return result;
                }


                DateTime dateStart = (model.planGoodsIssue_Due_Date.toBetweenDate().start);
                List<string> truckload_list = model.itemsUpdate.Select(c => c.TM_No).ToList();

                List<View_Checkstatus_truckload_order> showData = db.View_Checkstatus_truckload_order.Where(c => truckload_list.Contains(c.TruckLoad_No) && c.Document_Status == 1).ToList();

                List<Guid> truckLoads_index = db.im_TruckLoad.Where(c => truckload_list.Contains(c.TruckLoad_No) && c.Document_Status == 0).GroupBy(c => c.TruckLoad_Index).Select(c => c.Key).ToList();
                List<Guid?> plan_index = db.im_TruckLoadItem.Where(c => truckLoads_index.Contains(c.TruckLoad_Index)).Select(c => c.PlanGoodsIssue_Index).ToList();

                List<im_PlanGoodsIssue> planGoodsIssues = db.IM_PlanGoodsIssue.Where(c => plan_index.Contains(c.PlanGoodsIssue_Index) && c.Round_Index == null && c.Document_Status == 0).ToList();
                List<im_PlanGoodsIssue> checkstatus = db.IM_PlanGoodsIssue.Where(c => plan_index.Contains(c.PlanGoodsIssue_Index) && c.Document_Status == 1).ToList();

                foreach (var item in truckload_list)
                {
                    var TM_No = new SqlParameter("@TM_NO", item);

                    var resultPickingplan = db.Database.ExecuteSqlCommand("EXEC sp_Update_ERP_location @TM_NO", TM_No);
                }

                if (planGoodsIssues.Count == 0)
                {
                    result.resultIsUse = false;
                    result.resultMsg = "Shipment ที่เลือก ทำการจัดรอบทั้งหมดแล้ว กรุณาตรวจสอบ Shipment ที่ท่านเลือก";
                    return result;
                }
                if (checkstatus.Count >= 1)
                {
                    var listitem = new List<string>();
                    foreach (var i in showData)
                    {

                        string convert = string.Join("  รอบ ", i.TruckLoad_No, i.Round_Name);
                        listitem.Add(convert);

                    }
                    string convertfinal = string.Join(",", listitem);
                    result.resultIsUse = false;
                    string test = "Shipment ต่อไปนี้ได้ทำการจัดรอบไปแล้ว ," + convertfinal + ", กรุณาตรวจสอบอีกครั้ง";
                    result.resultMsg = test;
                    return result;
                }
                foreach (var item in planGoodsIssues)
                {
                    item.Document_Status = 1;
                    item.Round_Index = Guid.Parse(model.round_Index);
                    item.Round_Id = model.round_id;
                    item.Round_Name = model.round_Name;
                    item.Update_By = model.userName;
                    item.Update_Date = DateTime.Now;
                    item.PlanGoodsIssue_Due_Date = dateStart;
                }
                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    transaction.Rollback();
                    result.resultIsUse = false;
                    result.resultMsg = exy.Message;
                }
                return result;
            }
            catch (Exception ex)
            {

                result.resultMsg = ex.Message;
                result.resultIsUse = true;
                return result;
            }
        }
        #endregion

        #region deleteRound
        public actionResultRoundWaveViewModel deleteRound(RoundWaveViewModel model)
        {
            actionResultRoundWaveViewModel result = new actionResultRoundWaveViewModel();
            try
            {
                db.Database.SetCommandTimeout(120);
                
                List<string> truckload_list = model.itemsUpdate.Select(c => c.TM_No).ToList();
                List<Guid> truckLoads_index = db.im_TruckLoad.Where(c => c.TruckLoad_No == model.TM_No && c.Document_Status == 0).GroupBy(c => c.TruckLoad_Index).Select(c => c.Key).ToList();
                List<Guid?> plan_index = db.im_TruckLoadItem.Where(c => truckLoads_index.Contains(c.TruckLoad_Index)).Select(c => c.PlanGoodsIssue_Index).ToList();

                List<im_PlanGoodsIssue> planGoodsIssues = db.IM_PlanGoodsIssue.Where(c => plan_index.Contains(c.PlanGoodsIssue_Index) && c.Round_Index != null && c.Document_Status == 1).ToList();

                var imdex_notin = planGoodsIssues.GroupBy(c => c.PlanGoodsIssue_Index).Select(c => c.Key).ToList();

                List<Guid?> goodsIssueItemLocations = db.IM_GoodsIssueItemLocation.Where(c => imdex_notin.Contains(c.Ref_Document_Index.GetValueOrDefault()) && c.Document_Status != -1).GroupBy(c => c.Ref_Document_Index).Select(c => c.Key).ToList();

                planGoodsIssues = planGoodsIssues.Where(c => !goodsIssueItemLocations.Contains(c.PlanGoodsIssue_Index)).ToList();

                if (planGoodsIssues.Count == 0)
                {
                    result.resultIsUse = false;
                    result.resultMsg = "Shipment ที่เลือก ทำการเวฟทั้งหมดแล้ว กรุณาตรวจสอบ Shipment ที่ท่านเลือก";
                    return result;
                }
                foreach (var item in planGoodsIssues)
                {
                    item.Document_Status = 0;
                    item.Round_Index = null;
                    item.Round_Id = null;
                    item.Round_Name = null;
                    item.Update_By = model.userName;
                    item.Update_Date = DateTime.Now;
                }
                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    transaction.Rollback();
                    result.resultIsUse = false;
                    result.resultMsg = exy.Message;
                }
                return result;
            }
            catch (Exception ex)
            {

                result.resultMsg = ex.Message;
                result.resultIsUse = true;
                return result;
            }
        }
        #endregion



    }
}
