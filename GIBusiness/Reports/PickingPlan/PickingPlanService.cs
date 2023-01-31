using AspNetCore.Reporting;
using Business.Library;
using Comone.Utils;
using DataAccess;
using GIBusiness.Reports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace GIBusiness.GoodIssue
{
    public class PickingPlanService
    {
        #region  GIDbContext
        private GIDbContext db;

        public PickingPlanService()
        {
            db = new GIDbContext();
        }
        public PickingPlanService(GIDbContext db)
        {
            this.db = db;
        }
        #endregion

        #region ExportExcel
        public string ExportExcel(PickingPlanViewModel data, string rootPath = "")
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            logtxt olog = new logtxt();
            List<PickingPlanViewModel> result = new List<PickingPlanViewModel>();
            rootPath = rootPath.Replace("\\GIAPI", "");
            string reportPath = rootPath + new AppSettingConfig().GetUrl("PickingPlan");

            try
            {
                db.Database.SetCommandTimeout(360);
                var GoodsIssue_Index = new SqlParameter("@GoodsIssue_Index", data.GoodsIssue_Index);
                //var dataset = db.View_RPT_PickingPlan.Where(c => c.GoodsIssue_Index == data.GoodsIssue_Index).ToList();
                var dataset = db.View_RPT_PickingPlan.FromSql("sp_View_RPT_PickingPlan @GoodsIssue_Index", GoodsIssue_Index).ToList();

                foreach (var item in dataset.OrderBy(c => int.Parse(c.Chute_No)))
                {
                    PickingPlanViewModel resultitem = new PickingPlanViewModel();
                    resultitem.GoodsIssue_No = item.GoodsIssue_No;
                    resultitem.Date = item.GoodsIssue_Date;
                    resultitem.PrdAvailable = item.PrdAvailable;
                    resultitem.Time = item.Time;
                    resultitem.CountTagASRS = item.CountTagASRS;
                    resultitem.CountCTNASRS = item.CountCTNASRS;
                    resultitem.CountTote = item.CountTote;
                    resultitem.SumQtyVC = item.SumQtyVC;
                    resultitem.CountPrdVC = item.CountPrdVC;
                    resultitem.SumQtyCFR = item.SumQtyCFR;
                    resultitem.CountPrdCFR = item.CountPrdCFR;
                    resultitem.CountTagLBL = item.CountTagLBL;
                    resultitem.CountTagSTG = item.CountTagSTG;
                    resultitem.CountTagBUF = item.CountTagBUF;
                    resultitem.CountCTNSEL = item.CountCTNSEL;
                    resultitem.Chute_No = item.Chute_No;
                    resultitem.CountCTNChute = item.CountCTNChute;
                    resultitem.CountCTNLBL = item.CountCTNLBL;
                    resultitem.Dock_Name = item.Dock_Name;
                    resultitem.ASRSPercent = item.ASRSPercent;
                    resultitem.SELPercent = item.SELPercent;
                    resultitem.VCPercent = item.VCPercent;
                    resultitem.CFRPercent = item.CFRPercent;
                    resultitem.CountCTNBUF = item.CountCTNBUF;
                    resultitem.CountCTNSTG = item.CountCTNSTG;
                    resultitem.SKUAvailable = item.SKUAvailable;
                    resultitem.TruckLoad_No = item.TruckLoad_No;
                    resultitem.Count = dataset.Count == 1 && item.Chute_No == "0" ? 0 : dataset.Count;
                    resultitem.RollCageAll       = item.RollCageAll;
                    resultitem.RollCageUse       = item.RollCageUse;
                    resultitem.RollCageNotUse    = item.RollCageNotUse;
                    resultitem.MaxToteM          = item.MaxToteM;
                    resultitem.MaxToteL          = item.MaxToteL;
                    resultitem.UseToteM          = item.UseToteM;
                    resultitem.UseToteL          = item.UseToteL;
                    resultitem.AvailableToteM    = item.AvailableToteM;
                    resultitem.AvailableToteL    = item.AvailableToteL;
                    resultitem.PendingToteM      = item.PendingToteM;
                    resultitem.PendingToteL      = item.PendingToteL;
                    resultitem.ReturnToteM       = item.ReturnToteM;
                    resultitem.ReturnToteL = item.ReturnToteL;
                    resultitem.SKUAvailable_VC = item.SKUAvailable_VC;
                    resultitem.PrdAvailable_VC = item.PrdAvailable_VC;
                    resultitem.TotalUseRollcage = item.TotalUseRollcage;
                    resultitem.RollcageCompleted = item.RollcageCompleted;
                    resultitem.ToteLCompleted = item.ToteLCompleted;
                    resultitem.ToteMCompleted = item.ToteMCompleted;


                    result.Add(resultitem);
                }

              

                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "Picking_Plan";

                var renderedBytes = report.Execute(RenderType.Excel);
                fullPath = saveReport(renderedBytes.MainStream, fileName + ".xls", rootPath);


                string saveLocation = rootPath + fullPath;
                return saveLocation;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region saveReport
        public string saveReport(byte[] file, string name, string rootPath)
        {
            var saveLocation = PhysicalPath(name, rootPath);
            FileStream fs = new FileStream(saveLocation, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                try
                {
                    bw.Write(file);
                }
                finally
                {
                    fs.Close();
                    bw.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return VirtualPath(name);
        }
        #endregion

        #region PhysicalPath
        public string PhysicalPath(string name, string rootPath)
        {
            var filename = name;
            var vPath = ReportPath;
            var path = rootPath + vPath;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            var saveLocation = System.IO.Path.Combine(path, filename);
            return saveLocation;
        }
        #endregion

        #region ReportPath
        private string ReportPath
        {
            get
            {
                var url = "\\ReportGenerator\\";
                return url;
            }
        }
        #endregion

        #region VirtualPath
        public string VirtualPath(string name)
        {
            var filename = name;
            var vPath = ReportPath;
            vPath = vPath.Replace("~", "");
            return vPath + filename;
        }
        #endregion
    }
}
