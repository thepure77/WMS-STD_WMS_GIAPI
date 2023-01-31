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

namespace GIBusiness.GoodIssue
{
    public class FixwaveService
    {
        private GIDbContext db;

        public FixwaveService()
        {
            db = new GIDbContext();
        }
        public FixwaveService(GIDbContext db)
        {
            this.db = db;
        }

        #region ChecktagGi
        public Checktaggi ChecktagGi(ChecktagViewModel model)
        {
            var resultWave = new Checktaggi();
            try
            {
                var db = new GIDbContext();
                var Tag_No = new SqlParameter("@Tag_No", model.Tag_No);
                var ChecktagGi = db.sp_ChecktagGi.FromSql("sp_ChecktagGi @Tag_No", Tag_No).ToList();
                var ItemGi = new List<sp_ChecktagGi>();
                ItemGi = ChecktagGi.ToList();

                foreach (var item in ItemGi)
                {
                    checktaggimodel checktag = new checktaggimodel();
                    checktag.Tag_No = item.Tag_No;
                    checktag.GoodsIssue_No = item.GoodsIssue_No;
                    checktag.Location_Name = item.Location_Name;
                    checktag.GITotalQty = item.GITotalQty;

                    resultWave.checktaggimodel.Add(checktag);
                }
                //resultWave.resultIsUse = true;

                //return resultWave;

                //var db = new GIDbContext();
                var ChecktagTf = db.sp_ChecktagTf.FromSql("sp_ChecktagTf @Tag_No", Tag_No).ToList();
                var ItemTf = new List<sp_ChecktagTf>();
                ItemTf = ChecktagTf.ToList();

                foreach (var item in ItemTf)
                {
                    checktagtfmodel checktag = new checktagtfmodel();
                    checktag.Document_Status = item.Document_Status;
                    checktag.Item_Document_Status = item.Item_Document_Status;
                    checktag.GoodsTransfer_No = item.GoodsTransfer_No;
                    checktag.DocumentType_Name = item.DocumentType_Name;
                    checktag.Create_Date = item.Create_Date;
                    checktag.Create_By = item.Create_By;
                    checktag.Tag_No = item.Tag_No;
                    checktag.Product_Id = item.Product_Id;
                    checktag.Product_Name = item.Product_Name;
                    checktag.Qty = item.Qty;
                    checktag.TotalQty = item.TotalQty;
                    checktag.ProductConversion_Name = item.ProductConversion_Name;
                    checktag.Location_Name = item.Location_Name;
                    checktag.Location_Name_To = item.Location_Name_To;
                    checktag.ItemStatus_Name = item.ItemStatus_Name;
                    checktag.ItemStatus_Name_To = item.ItemStatus_Name_To;
                    checktag.ERP_Location = item.ERP_Location;
                    checktag.ERP_Location_To = item.ERP_Location_To;

                    resultWave.checktagtfmodel.Add(checktag);
                }
                resultWave.resultIsUse = true;

                return resultWave;
            }
            catch (Exception ex)
            {
                resultWave.resultIsUse = false;
                resultWave.resultMsg = "เช็ค Tag ไม่สำเร็จ";
                return resultWave;
            }
        }
        #endregion

        #region Updatebinbalance
        public ResultRunWave Updatebinbalance(ChecktagViewModel model)
        {
            var resultWave = new ResultRunWave();
            try
            {
                var BinBalance_Index = new SqlParameter("@BinBalance_Index", model.BinBalance_Index);
                var BinBalance_QtyBal = new SqlParameter("@BinBalance_QtyBal", model.BinBalance_QtyBal);
                var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", model.GI_TotalQty);
                var resultx = db.Database.ExecuteSqlCommand("EXEC sp_UpdateBinBalance @BinBalance_Index , @BinBalance_QtyBal , @BinBalance_QtyReserve", BinBalance_Index, BinBalance_QtyBal, BinBalance_QtyReserve);

                resultWave.resultIsUse = true;
                resultWave.resultMsg = "Update BinBalance สำเร็จ";
                return resultWave;
            }
            catch (Exception ex)
            {
                resultWave.resultIsUse = false;
                resultWave.resultMsg = "Update BinBalance ไม่สำเร็จ";
                return resultWave;
            }
        }
        #endregion

        #region ChecktagByGi
        public Checktaggi ChecktagByGi(ChecktagViewModel model)
        {
            var resultWave = new Checktaggi();
            try
            {
                var db = new GIDbContext();
                var GoodsIssue_No = new SqlParameter("@GoodsIssue_No", model.goodsIssue_No);
                var Tag_No = new SqlParameter("@Tag_No", model.Tag_No);
                var ChecktagByGi = db.sp_ChecktagByGi.FromSql("sp_ChecktagByGi @GoodsIssue_No , @Tag_No", GoodsIssue_No, Tag_No).ToList();
                var ItemGi = new List<sp_ChecktagByGi>();
                ItemGi = ChecktagByGi.ToList();

                foreach (var item in ItemGi)
                {
                    checktagbygimodel checktag = new checktagbygimodel();
                    checktag.Tag_No = item.Tag_No;
                    checktag.Product_Id = item.Product_Id;
                    checktag.Product_Name = item.Product_Name;
                    checktag.TotalQty = item.TotalQty;
                    checktag.ProductConversion_Name = item.ProductConversion_Name;
                    checktag.GoodsIssue_No = item.GoodsIssue_No;
                    checktag.GoodsIssue_Index = item.GoodsIssue_Index;
                    checktag.GoodsIssueItemLocation_Index = item.GoodsIssueItemLocation_Index;

                    resultWave.checktagbygimodel.Add(checktag);
                }
                resultWave.resultIsUse = true;
                return resultWave;
            }
            catch (Exception ex)
            {
                resultWave.resultIsUse = false;
                resultWave.resultMsg = "เช็ค Tag ไม่สำเร็จ";
                return resultWave;
            }
        }
        #endregion

        #region DeleteGoodsIssueitemlocation
        public ResultRunWave DeleteGoodsIssueitemlocation(ChecktagViewModel model)
        {
            var resultWave = new ResultRunWave();
            try
            {
                var GoodsIssueItemLocation_Index = new SqlParameter("@GoodsIssueItemLocation_Index", model.GoodsIssueItemLocation_Index);
                var resultx = db.Database.ExecuteSqlCommand("EXEC sp_DeleteGoodsIssueitemLocation @GoodsIssueItemLocation_Index", GoodsIssueItemLocation_Index);

                resultWave.resultIsUse = true;
                resultWave.resultMsg = "Delete GoodsIssueItemLocation สำเร็จ";
                return resultWave;
            }
            catch (Exception ex)
            {
                resultWave.resultIsUse = false;
                resultWave.resultMsg = "Delete GoodsIssueItemLocation ไม่สำเร็จ";
                return resultWave;
            }
        }
        #endregion

        /*#region Updatebinbalance
        public Updatebinbalance Updatebinbalance(ChecktagViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var result = new Result();
                var GI = db.IM_GoodsIssue.Find(Guid.Parse(model.goodsIssue_Index));

                var GI_Index = new SqlParameter("@GI_Index", model.goodsIssue_Index);
                var resultx = db.Database.ExecuteSqlCommand("EXEC sp_GetCheckAfterTaskWave @GI_Index", GI_Index);

                foreach (var item in model.listGoodsIssueItemViewModel)
                {
                    var GIL = db.IM_GoodsIssueItemLocation.Find(Guid.Parse(item.goodsIssueItemLocation_Index));

                    GIL.Invoice_No = item.invoice_No;
                    GIL.Invoice_No_Out = item.invoice_No_Out;
                    GIL.Declaration_No = item.declaration_No;
                    GIL.Declaration_No_Out = item.declaration_No_Out;
                    GIL.HS_Code = item.hs_Code;
                    GIL.Conutry_of_Origin = item.conutry_of_Origin;
                    GIL.Tax1 = item.tax1;
                    GIL.Tax1_Currency_Index = item.tax1_Currency_Index;
                    GIL.Tax1_Currency_Id = item.tax1_Currency_Id;
                    GIL.Tax1_Currency_Name = item.tax1_Currency_Name;
                    GIL.Tax2 = item.tax2;
                    GIL.Tax2_Currency_Index = item.tax2_Currency_Index;
                    GIL.Tax2_Currency_Id = item.tax2_Currency_Id;
                    GIL.Tax2_Currency_Name = item.tax2_Currency_Name;
                    GIL.Tax3 = item.tax3;
                    GIL.Tax3_Currency_Index = item.tax3_Currency_Index;
                    GIL.Tax3_Currency_Id = item.tax3_Currency_Id;
                    GIL.Tax3_Currency_Name = item.tax3_Currency_Name;
                    GIL.Tax4 = item.tax4;
                    GIL.Tax4_Currency_Index = item.tax4_Currency_Index;
                    GIL.Tax4_Currency_Id = item.tax4_Currency_Id;
                    GIL.Tax4_Currency_Name = item.tax4_Currency_Name;
                    GIL.Tax5 = item.tax5;
                    GIL.Tax5_Currency_Index = item.tax5_Currency_Index;
                    GIL.Tax5_Currency_Id = item.tax5_Currency_Id;
                    GIL.Tax5_Currency_Name = item.tax5_Currency_Name;

                    GIL.Document_Status = 0;
                    GIL.Update_Date = DateTime.Now;
                    GIL.Update_By = model.create_by;
                }

                GI.Document_Remark = model.document_Remark;
                GI.DocumentPriority_Status = model.documentPriority_Status;
                GI.Warehouse_Index = !string.IsNullOrEmpty(model.warehouse_Index) ? new Guid(model.warehouse_Index) : (Guid?)null;
                GI.Warehouse_Id = !string.IsNullOrEmpty(model.warehouse_Index) ? model.warehouse_Id : null;
                GI.Warehouse_Name = !string.IsNullOrEmpty(model.warehouse_Index) ? model.warehouse_Name : null;

                GI.Document_Status = 0;
                GI.GI_status = 1;
                if (model.isUpdate)
                {
                    GI.Update_Date = DateTime.Now;
                    GI.Update_By = model.create_by;
                }
                var transactionGI = db.Database.BeginTransaction();
                try
                {
                    db.SaveChanges();
                    transactionGI.Commit();
                    result.resultIsUse = true;
                }

                catch (Exception exy)
                {
                    result.resultIsUse = false;
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("UpdateUserAssign", msglog);
                    transactionGI.Rollback();
                    throw exy;
                }


                result.resultMsg = "Success";
                return result;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("runwave", msglog);
                var result = new Result();
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }
        }
        #endregion

        /* ลบ
                    #region DeleteDocument
        public string DeleteDocument(GoodsIssueViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            bool ischkPGI = false;
            try
            {
                db.Database.SetCommandTimeout(360);
                var GoodsIssue = db.IM_GoodsIssue.Find(new Guid(model.goodsIssue_Index));

                if (GoodsIssue != null)
                {
                    if (GoodsIssue.Document_Status != 0 && GoodsIssue.Document_Status != 1 && GoodsIssue.Document_Status != -2)
                    {
                        if (GoodsIssue.Document_Status == 2)
                        {
                            return "ไม่สามารถยกเลิกใบ PO ที่มีสถานนะมอบหมายงานแล้วได้";
                        }
                        else if (GoodsIssue.Document_Status == 2)
                        {
                            return "ไม่สามารถยกเลิกใบ PO ที่มีสถานนะเสร็จสินแล้วได้";
                        }
                        else if (GoodsIssue.Document_Status == -1)
                        {
                            return "ใบ PO ถูกยกเลิกไปแล้ว";
                        }
                        else
                        {
                            return "ไม่สามารถยกเลิกใบ PO ได้";
                        }
                    }

                    GoodsIssue.Document_Status = GoodsIssue.Document_Status == -2 ? -3 : -1;
                    GoodsIssue.Cancel_Date = DateTime.Now;
                    GoodsIssue.Cancel_By = model.create_by;


                    var GIL = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssue_Index == new Guid(model.goodsIssue_Index) && (c.Document_Status == 0 || c.Document_Status == -2)).ToList();
                    foreach (var G in GIL)
                    {


                        if (!string.IsNullOrEmpty(G?.Ref_DocumentItem_Index?.ToString()) && !string.IsNullOrEmpty(G?.Ref_Document_Index?.ToString()))
                        {
                            if ((GoodsIssue.DocumentType_Index ?? Guid.Empty) == new Guid("D7C596E9-BDFF-4759-91DB-FCEC709E16B8"))
                            {
                                ischkPGI = true;
                                var modelPGI = new { BOMItem_Index = G?.Ref_DocumentItem_Index, BOM_Index = G?.Ref_Document_Index };
                                var updatePGIIStatus = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateStatusBomRunWave"), modelPGI.sJson());
                            }
                            else
                            {
                                ischkPGI = true;
                                var modelPGI = new { planGoodsIssueItem_Index = G?.Ref_DocumentItem_Index, planGoodsIssue_Index = G?.Ref_Document_Index };
                                var updatePGIIStatus = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateStatusPlanGIRunWave"), modelPGI.sJson());
                            }
                        }

                        var m = new { ref_DocumentItem_Index = G.GoodsIssueItemLocation_Index, ref_Document_Index = G.GoodsIssue_Index };
                        var updateBinCardReserveStatus = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateBinCardReserve"), m.sJson());
                        if (updateBinCardReserveStatus)
                        {
                            G.Document_Status = -1;
                            G.Cancel_Date = DateTime.Now;
                            G.Cancel_By = model.create_by;
                        }


                    }

                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("DeleteGI", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return ischkPGI ? "SUCCESShavePGI" : "ยกเลิกข้อมูลสำเร็จ";
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        */

        /***
    *               ii.                                         ;9ABH,          
    *              SA391,                                    .r9GG35&G          
    *              &#ii13Gh;                               i3X31i;:,rB1         
    *              iMs,:,i5895,                         .5G91:,:;:s1:8A         
    *               33::::,,;5G5,                     ,58Si,,:::,sHX;iH1        
    *                Sr.,:;rs13BBX35hh11511h5Shhh5S3GAXS:.,,::,,1AG3i,GG        
    *                .G51S511sr;;iiiishS8G89Shsrrsh59S;.,,,,,..5A85Si,h8        
    *               :SB9s:,............................,,,.,,,SASh53h,1G.       
    *            .r18S;..,,,,,,,,,,,,,,,,,,,,,,,,,,,,,....,,.1H315199,rX,       
    *          ;S89s,..,,,,,,,,,,,,,,,,,,,,,,,....,,.......,,,;r1ShS8,;Xi       
    *        i55s:.........,,,,,,,,,,,,,,,,.,,,......,.....,,....r9&5.:X1       
    *       59;.....,.     .,,,,,,,,,,,...        .............,..:1;.:&s       
    *      s8,..;53S5S3s.   .,,,,,,,.,..      i15S5h1:.........,,,..,,:99       
    *      93.:39s:rSGB@A;  ..,,,,.....    .SG3hhh9G&BGi..,,,,,,,,,,,,.,83      
    *      G5.G8  9#@@@@@X. .,,,,,,.....  iA9,.S&B###@@Mr...,,,,,,,,..,.;Xh     
    *      Gs.X8 S@@@@@@@B:..,,,,,,,,,,. rA1 ,A@@@@@@@@@H:........,,,,,,.iX:    
    *     ;9. ,8A#@@@@@@#5,.,,,,,,,,,... 9A. 8@@@@@@@@@@M;    ....,,,,,,,,S8    
    *     X3    iS8XAHH8s.,,,,,,,,,,...,..58hH@@@@@@@@@Hs       ...,,,,,,,:Gs   
    *    r8,        ,,,...,,,,,,,,,,.....  ,h8XABMMHX3r.          .,,,,,,,.rX:  
    *   :9, .    .:,..,:;;;::,.,,,,,..          .,,.               ..,,,,,,.59  
    *  .Si      ,:.i8HBMMMMMB&5,....                    .            .,,,,,.sMr
    *  SS       :: h@@@@@@@@@@#; .                     ...  .         ..,,,,iM5
    *  91  .    ;:.,1&@@@@@@MXs.                            .          .,,:,:&S
    *  hS ....  .:;,,,i3MMS1;..,..... .  .     ...                     ..,:,.99
    *  ,8; ..... .,:,..,8Ms:;,,,...                                     .,::.83
    *   s&: ....  .sS553B@@HX3s;,.    .,;13h.                            .:::&1
    *    SXr  .  ...;s3G99XA&X88Shss11155hi.                             ,;:h&,
    *     iH8:  . ..   ,;iiii;,::,,,,,.                                 .;irHA  
    *      ,8X5;   .     .......                                       ,;iihS8Gi
    *         1831,                                                 .,;irrrrrs&@
    *           ;5A8r.                                            .:;iiiiirrss1H
    *             :X@H3s.......                                .,:;iii;iiiiirsrh
    *              r#h:;,...,,.. .,,:;;;;;:::,...              .:;;;;;;iiiirrss1
    *             ,M8 ..,....,.....,,::::::,,...         .     .,;;;iiiiiirss11h
    *             8B;.,,,,,,,.,.....          .           ..   .:;;;;iirrsss111h
    *            i@5,:::,,,,,,,,.... .                   . .:::;;;;;irrrss111111
    *            9Bi,:,,,,......                        ..r91;;;;;iirrsss1ss1111
    */


    }
}
