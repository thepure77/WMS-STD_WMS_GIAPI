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
using Newtonsoft.Json;
using PlanGIBusiness.Libs;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace GIBusiness.TagOut
{
    public class TagOutService
    {
        private GIDbContext db;

        public TagOutService()
        {
            db = new GIDbContext();
        }
        public TagOutService(GIDbContext db)
        {
            this.db = db;
        }

        #region tagOutfilter
        public List<TagOutViewModel> tagOutfilter(TagOutfilterViewModel model)
        {
            var result = new List<TagOutViewModel>();
            try
            {
                var query = db.View_PrintTagOut.AsQueryable();
                if (!string.IsNullOrEmpty(model.goodsIssue_No))
                {
                    query = query.Where(c => c.Ref_Document_No == model.goodsIssue_No);
                }
                else {
                    return result;
                }
                if (!string.IsNullOrEmpty(model.plangi_no))
                {
                    query = query.Where(c => c.UDF_1 == model.plangi_no);
                }
                if (!string.IsNullOrEmpty(model.value))
                {
                    query = query.Where(c => c.LocationType == model.value);
                }
                if (!string.IsNullOrEmpty(model.tagOut_No))
                {
                    query = query.Where(c => c.TagOut_No == model.tagOut_No);
                }
                var Item = query.ToList();
                

                foreach (var item in Item)
                {
                    var resultItem = new TagOutViewModel();
                    resultItem.TagOut_Index = item.TagOut_Index;
                    resultItem.TagOut_No = item.TagOut_No;
                    resultItem.Ref_Document_No = item.Ref_Document_No;
                    resultItem.Create_By = item.Create_By;
                    resultItem.Create_Date = item.Create_Date;
                    resultItem.LocationType = item.LocationType;
                    resultItem.TagOutType = item.TagOutType;
                    resultItem.StatusRollcage = item.StatusRollcage;
                    resultItem.TagOut_Status = item.TagOut_Status;

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


        #region tagOutPopup
        public List<GoodIssueViewModel> tagOutPopup(GoodIssueViewModel model)
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

        #region Savetagout
        public string Savetagout(TagOutfilterViewModel data)
        {
            try
            {
                var listtag_I_no = db.WM_TagOut.Where(c => c.Ref_Document_No == data.goodsIssue_No && c.UDF_1 == data.plangi_no).ToList();
                if (listtag_I_no.Count <= 0)
                {
                    return "can not find PlanGI";
                }
                var count = listtag_I_no.Count();
                for (int i = 0; i < data.qtylabel; i++)
                {
                    
                    var branch_code = listtag_I_no[0].TagOutRef_No1 == null ? "0000000" : listtag_I_no[0].TagOutRef_No1;
                    var ref_document_no = listtag_I_no[0].UDF_1 == null ? "0000000000" : listtag_I_no[0].UDF_1;

                   
                    var runing = "";
                    count++;
                    Guid tag = Guid.NewGuid();
                    var tagnew = new wm_TagOut();

                    if (count < 10)
                    {
                        runing = "00" + count;
                    }
                    else if (count >= 10)
                    {
                        runing = "0" + count;
                    }
                    else if (count >= 100)
                    {
                        runing = count.ToString();
                    }
                    tagnew.TagOut_Index = tag;
                    tagnew.TagOut_No = "00000" + ref_document_no + branch_code + runing;
                    tagnew.TagOutRef_No1 = listtag_I_no[0].TagOutRef_No1;
                    tagnew.TagOutRef_No2 = listtag_I_no[0].TagOutRef_No2;
                    tagnew.TagOutRef_No3 = listtag_I_no[0].TagOutRef_No3;
                    tagnew.TagOutRef_No4 = runing;
                    tagnew.TagOutType = "Manual";
                    tagnew.LocationType = data.value;
                    tagnew.TagOutRef_No5 = listtag_I_no[0].TagOutRef_No5;
                    tagnew.TagOut_Status = 0;
                    tagnew.UDF_1 = listtag_I_no[0].UDF_1;
                    tagnew.UDF_2 = listtag_I_no[0].UDF_2;
                    tagnew.UDF_3 = listtag_I_no[0].UDF_3 == null ? null : listtag_I_no[0].UDF_3;
                    tagnew.UDF_4 = listtag_I_no[0].UDF_4 == null ? null : listtag_I_no[0].UDF_4;
                    tagnew.UDF_5 = listtag_I_no[0].UDF_4 == null ? null : listtag_I_no[0].UDF_4;
                    tagnew.Zone_Index = null;
                    tagnew.Ref_Process_Index = listtag_I_no[0].Ref_Process_Index;
                    tagnew.Ref_Document_No = listtag_I_no[0].Ref_Document_No;
                    tagnew.Ref_Document_Index = listtag_I_no[0].Ref_Document_Index;
                    tagnew.Ref_DocumentItem_Index = listtag_I_no[0].Ref_DocumentItem_Index;
                    tagnew.Create_By = "";
                    tagnew.Create_Date = DateTime.Now;


                    db.WM_TagOut.Add(tagnew);
                    
                }
               
                
                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception exy)
                {
                    transaction.Rollback();
                    throw exy;
                }


                return "Success";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Deletetag
        public Result Deletetag(TagOutfilterViewModel data)
        {
            Result result = new Result();
            try
            {
                var listtag_I_no = db.WM_TagOut.FirstOrDefault(c => c.TagOut_Index == data.tagOut_Index);
                if (listtag_I_no == null)
                {
                    result.resultIsUse = false;
                    result.resultMsg = "ไม่สามารถทำการลบ Tag ได้";
                    return result;
                }
                else {
                    listtag_I_no.TagOut_Status = -1;
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
                    throw exy;
                }


                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


        #region ReportTagout
        public string ReportTagout(TagViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(360);

                var resTagItem = new List<ReportTagoutViewModel>();
                var GI_NO = data.listtagoutViewModel.Select(w => w.ref_Document_No).FirstOrDefault();
                var GoodsIssue_No = new SqlParameter("@GoodsIssue_No", GI_NO);
                var gettag = db.View_tagOut.FromSql("sp_View_tagOut @GoodsIssue_No", GoodsIssue_No).ToList();
                var xxx = data.listtagoutViewModel.Select(w => w.TagOut_Index).ToList();
                gettag = gettag.Where(c => xxx.Contains(c.TagOut_Index)).OrderBy(c => c.UDF_1).ThenBy(c => c.TagOutRef_No4).ToList();

                string DatePrint = DateTime.Now.ToString("dd/MM/yyyy", culture);
                var time = DateTime.Now.ToString("HH:mm");
                List<View_tagOut> tagOuts = gettag.ToList();
                foreach (var item in tagOuts.OrderBy(c=> c.Product_Id).ThenBy(c => c.Loc).ThenBy(c=> c.UDF_1))
                {
                    var TY = item.locationtype == "CartonFlowRack" ? 2 : 1;
                    for (int i = 0; i < TY; i++)
                    {
                        var resultItem = new ReportTagoutViewModel();

                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(item.TagOut_No, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);

                        resultItem.moblie = item.moblie;
                        resultItem.date = DateTime.Now.ToString("dd-MM-yyyy");
                        resultItem.UDF_1 = item.UDF_1;
                        resultItem.qrcode = Convert.ToBase64String(BitmapToBytes(qrCodeImage));
                        resultItem.TagOut_No = item.TagOut_No;
                        
                        resultItem.total_Box = item.total_Box;
                        resultItem.Country_Name = item.Country_Name;
                        resultItem.Province_Name = item.Province_Name;
                        resultItem.Ref_No1 = item.Ref_No1;
                        resultItem.ShipTo_Name = item.ShipTo_Name;
                        resultItem.Drop_Seq = item.Drop_Seq;
                        resultItem.Order_seq = item.Order_seq;
                        resultItem.barcodeTracking = new NetBarcode.Barcode(item.barcodeTracking, NetBarcode.Type.Code128B).GetBase64Image();
                        resultItem.barcodeText = item.barcodeTracking;

                        if (item.locationtype == "ByPass" || item.locationtype == "Selective")
                        {
                            resultItem.Loc = item.Loc;
                        }
                        

                        var tagitem = db.WM_TagOutItem.FirstOrDefault(c => c.TagOut_Index == item.TagOut_Index);
                        if (tagitem.TagOutRef_No2 != "")
                        {
                            resultItem.TagOutRef_No4 = "T"+item.TagOutRef_No4;
                            resultItem.Size = tagitem.TagOutRef_No2 == "XL" ? "L" : tagitem.TagOutRef_No2;
                            resultItem.Product_Id = "";
                        }
                        else {
                            resultItem.TagOutRef_No4 = "C"+item.TagOutRef_No4;
                            resultItem.Product_Id = "SKU : "+item.Product_Id;
                        }


                        resTagItem.Add(resultItem);
                    }
                    
                }
                resTagItem.ToList();

                rootPath = rootPath.Replace("\\GIAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportTagPutaway");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", resTagItem);

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

        #region printtote
        public Result printtote(TagViewModel data)
        {
            Result result = new Result();
            var olog = new logtxt();
            try
            {
                olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Start Json: "+ JsonConvert.SerializeObject(data)+"_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                try
                {
                    var GI_NO_ = data.listtagoutViewModel.Select(w => w.ref_Document_No).FirstOrDefault();
                    im_logTote logTote = new im_logTote();
                    logTote.logTote_Index = Guid.NewGuid();
                    logTote.logTote_No = GI_NO_ + "_tote";
                    logTote.GoodsIssue_No = GI_NO_;
                    logTote.Create_By = data.create_By;
                    logTote.Create_Date = DateTime.Now;

                    db.im_logTote.Add(logTote);

                    olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Save Log : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Save Log : C" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    }
                    catch (Exception exyz)
                    {
                        olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Save Log : E_exyz " + JsonConvert.SerializeObject(exyz) + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        transaction.Rollback();
                        throw exyz;
                    }
                }
                catch (Exception exy)
                {
                    olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Save Log : E_exy  " + JsonConvert.SerializeObject(exy) + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    throw exy;
                }
                db.Database.SetCommandTimeout(360);
                var GI_NO = data.listtagoutViewModel.Select(w => w.ref_Document_No).FirstOrDefault();
                var GoodsIssue_No = new SqlParameter("@GoodsIssue_No", GI_NO);
                olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "GI Log : "+ GoodsIssue_No+ "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var gettag = db.View_tagOut.FromSql("sp_View_tagOut @GoodsIssue_No", GoodsIssue_No).ToList();
                olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "GI After select  : " + gettag.Count() + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var xxx = data.listtagoutViewModel.Select(w => w.TagOut_Index).ToList();
                gettag = gettag.Where(c => xxx.Contains(c.TagOut_Index)).OrderBy(c => c.UDF_1).ThenBy(c => c.TagOutRef_No4).ToList();
                olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "GI After fillet  : " + gettag.Count() + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                List<Send_TagWCS> send_TagWCs = new List<Send_TagWCS>();
                int count = 0;
                foreach (var item in gettag)
                {
                    count++;
                    Send_TagWCS tagWCS = new Send_TagWCS();
                    tagWCS.TagOut_Index = item.TagOut_Index;
                    tagWCS.TagOut_No = item.TagOut_No;
                    tagWCS.printer_id = "12";
                    tagWCS.user_id = "Kasco";
                    send_TagWCs.Add(tagWCS);
                    olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "No.  : " + count + "_____" + JsonConvert.SerializeObject(tagWCS) + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                }

                olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Send Wcs.  : " + JsonConvert.SerializeObject(send_TagWCs) + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var printer_result = utils.SendDataApi<result_WCS>(new AppSettingConfig().GetUrl("Send_printertote_WCS"), JsonConvert.SerializeObject(send_TagWCs));
                olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Return Wcs.  : " + JsonConvert.SerializeObject(printer_result) + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                if (printer_result.status != 10)
                {
                    olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Return Wcs.  :  E " + printer_result.message.description + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    result.resultIsUse = false;
                    result.resultMsg = printer_result.message.description;
                    return result;
                }
                olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "End :  " + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                result.resultIsUse = true;
                return result;

            }
            catch (Exception ex)
            {
                olog.DataLogLines("printtote", "printtote" + DateTime.Now.ToString("yyyy-MM-dd"), "Error  :  E_ex " + JsonConvert.SerializeObject(ex) + "_____" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                throw ex;
            }
        }
        #endregion

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
