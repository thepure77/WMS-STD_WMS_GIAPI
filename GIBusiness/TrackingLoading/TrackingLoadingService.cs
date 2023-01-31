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
    public class TrackingLoadingService
    {
        private GIDbContext db;

        public TrackingLoadingService()
        {
            db = new GIDbContext();
        }
        public TrackingLoadingService(GIDbContext db)
        {
            this.db = db;
        }

        #region findShipment
        public Result findShipment(TrackingLoadingViewModel model)
        {
            try
            {
                var result = new Result();
                im_TruckLoad truckload = db.im_TruckLoad.FirstOrDefault(c=> c.TruckLoad_No == model.shipment_no && c.Document_Status != -1);
                if (truckload == null)
                {
                    result.resultIsUse = false;
                    result.resultMsg = "ไม่พบเลข Shipment ที่ค้นหา";
                }
                else {
                    result.resultMsg = truckload.TruckLoad_Index.ToString();
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

        #region savetracking
        public Result savetracking(TrackingLoadingViewModel model)
        {
            try
            {
                var result = new Result();
                List<Guid?> truckload = db.im_TruckLoadItem.Where(c => c.TruckLoad_Index == model.truckLoad_Index && c.Document_Status != -1).GroupBy(c=> c.PlanGoodsIssue_Index).Select(c=> c.Key).ToList();
                if (truckload.Count > 0)
                {
                    List<Guid> GIL = db.IM_GoodsIssueItemLocation.Where(c => truckload.Contains(c.Ref_Document_Index) && c.Document_Status != -1).GroupBy(c=> c.GoodsIssue_Index).Select(c=> c.Key).ToList();
                    if (GIL.Count > 0)
                    {
                        var getindextaskitem_index = db.View_Taskitem_with_Truckload.Where(c => c.TruckLoad_Index == model.truckLoad_Index).Select(c => c.TaskItem_Index).ToList();
                        var taskitem = db.IM_TaskItem.Where(c => GIL.Contains(c.Ref_Document_Index.GetValueOrDefault()) && getindextaskitem_index.Contains(c.TaskItem_Index) && c.Document_StatusTracking != 2).ToList();
                        if (taskitem.Count > 0)
                        {
                            var task = taskitem.GroupBy(c => c.Task_Index).Select(c => c.Key).ToList();
                            var updatetask = db.IM_Task.Where(c => task.Contains(c.Task_Index) && c.Document_StatusTracking != 2).ToList();

                            foreach (im_Task item in updatetask)
                            {
                                item.Document_StatusTracking = 1;
                            }

                            foreach (im_TaskItem item in taskitem)
                            {
                                item.Document_StatusTracking = 1;
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
                                result.resultMsg = "ไม่ Tracking Task ได้";
                                transaction.Rollback();
                                return result;
                            }
                        }
                    }
                    else {
                        result.resultIsUse = false;
                        result.resultMsg = "ไม่ Tracking Task ได้";
                        return result;
                    }
                }
                else {
                    result.resultIsUse = false;
                    result.resultMsg = "ไม่พบ Shipment ที่ต้อง tracking";
                    return result;
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
