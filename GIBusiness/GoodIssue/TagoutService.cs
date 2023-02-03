using AspNetCore.Reporting;
using BomBusiness;
using Business.Library;
using Comone.Utils;
using DataAccess;
using GIBusiness.AutoNumber;
using GIBusiness.GoodsIssue;
using GIBusiness.PlanGoodIssue;
using GIBusiness.Reports;
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

namespace GIBusiness.GoodIssue
{
    public class TagoutService
    {
        private GIDbContext db;

        public TagoutService()
        {
            db = new GIDbContext();
        }
        public TagoutService(GIDbContext db)
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


        #region makeTagOut
        //public bool maketagOut(findtagViewModelItem data)
        //{
        //    try
        //    {
        //        List<View_GoodsIssuecount_tag> listgood_I_no = db.View_GoodsIssuecount_tag.Where(c => c.Ref_Document_No == data.GoodsIssue_No && c.LocationType == "Selective").OrderBy(c => c.Ref_Document_No).ToList();
        //        var Document_no = "";
        //        var branch = "";
        //        var count = 0;
        //        foreach (View_GoodsIssuecount_tag itxem in listgood_I_no)
        //        {


        //            var branch_code = itxem.Branch_Code == null ? "0000000" : itxem.Branch_Code;
        //            var ref_document_no = itxem.Ref_Document_No == null ? "0000000000" : itxem.Ref_Document_No;
        //            if (Document_no != ref_document_no)
        //            {
        //                count = 0;
        //                Document_no = ref_document_no;
        //            }
        //            if (branch != branch_code)
        //            {
        //                count = 0;
        //                branch = branch_code;
        //            }
        //            int count_item = (int)itxem.QTYCTN;
        //            for (int i = 0; i < count_item; i++)
        //            {
        //                var runing = "";
        //                count++;
        //                Guid tag = Guid.NewGuid();
        //                var tagnew = new wm_TagOut();
        //                if (count < 10)
        //                {
        //                    runing = "00" + count.ToString();
        //                }
        //                else if (count >= 100)
        //                {
        //                    runing = count.ToString();
        //                }
        //                else if (count >= 10)
        //                {
        //                    runing = "0" + count.ToString();
        //                }
        //                tagnew.TagOut_Index = tag;
        //                tagnew.TagOut_No = "00000" + ref_document_no + branch_code + runing;
        //                tagnew.TagOutRef_No1 = itxem.Branch_Code;
        //                tagnew.TagOutRef_No2 = itxem.Chute_ID;
        //                tagnew.TagOutRef_No3 = itxem.Branch_Name;
        //                tagnew.TagOutRef_No4 = runing;
        //                tagnew.TagOutRef_No5 = itxem.QTYCTN.GetValueOrDefault().ToString();
        //                tagnew.TagOut_Status = 0;
        //                tagnew.UDF_1 = itxem.Ref_Document_No;
        //                tagnew.UDF_2 = itxem.TotalQty.ToString();
        //                tagnew.UDF_3 = itxem.ShipTo_Index == null ? null : itxem.ShipTo_Index.Value.ToString();
        //                tagnew.UDF_4 = itxem.Ref_Document_Index == null ? null : itxem.Ref_Document_Index.GetValueOrDefault().ToString();
        //                tagnew.UDF_5 = itxem.Ref_Document_Index == null ? null : itxem.Ref_DocumentItem_Index.GetValueOrDefault().ToString();
        //                tagnew.Zone_Index = null;
        //                tagnew.Ref_Process_Index = Guid.Parse(itxem.Ref_Process_Index);
        //                tagnew.Ref_Document_No = itxem.GoodsIssue_No;
        //                tagnew.Ref_Document_Index = itxem.GoodsIssue_Index;
        //                tagnew.Ref_DocumentItem_Index = itxem.GoodsIssueItemLocation_Index;
        //                tagnew.Create_By = "";
        //                tagnew.Create_Date = DateTime.Now;
        //                tagnew.TagOutType = itxem.TagOutType;
        //                tagnew.LocationType = itxem.LocationType;


        //                db.WM_TagOut.Add(tagnew);

        //                //////////////////////
        //                ///
        //                Guid tagoutitem_index = Guid.NewGuid();
        //                var tagout = new wm_TagOutItem();
        //                tagout.TagOutItem_Index = tagoutitem_index;
        //                tagout.TagOut_Index = tagnew.TagOut_Index;
        //                tagout.TagOut_No = tagnew.TagOut_No;
        //                tagout.GoodsIssue_Index = itxem.GoodsIssue_Index;
        //                //tagout.GoodsIssueItem_Index = itxem.Product_id;
        //                tagout.GoodsIssueItemLocation_Index = itxem.GoodsIssueItemLocation_Index;
        //                // tagout.Carton_No = itxem.Product_id;
        //                tagout.Product_Index = itxem.Product_Index;// new Guid(itxem.Product_Index) ;
        //                tagout.Product_Id = itxem.Product_Id;
        //                tagout.Product_Name = itxem.Product_Name;
        //                //  tagout.Product_SecondName = itxem.Product_SecondName;
        //                //  tagout.Product_ThirdName = itxem.Product_ThirdName;
        //                tagout.Product_Lot = itxem.Product_Lot;
        //                tagout.EXP_Date = itxem.EXP_Date;
        //                tagout.ItemStatus_Index = itxem.ItemStatus_Index;
        //                tagout.ItemStatus_Id = itxem.ItemStatus_Id;
        //                tagout.ItemStatus_Name = itxem.ItemStatus_Name;
        //                tagout.Qty = 1;//itxem.Product_id;
        //                tagout.Ratio = itxem.SALE_Ratio; //itxem.Product_id;
        //                tagout.TotalQty = 1 * itxem.SALE_Ratio; //itxem.Product_id;
        //                tagout.ProductConversion_Index = itxem.ProductConversion_Index;
        //                tagout.ProductConversion_Id = itxem.ProductConversion_Id;
        //                tagout.ProductConversion_Name = itxem.ProductConversion_Name;
        //                //  tagout.Weight = itxem.Product_id;
        //                tagout.Volume = itxem.Vol_Per_Unit;
        //                tagout.TagOutRef_No1 = "";
        //                tagout.TagOut_Status = 0;
        //                tagout.TagOutRef_No2 = "";
        //                tagout.TagOutRef_No3 = itxem.Vol_Per_Unit.ToString();
        //                tagout.TagOutRef_No4 = ref_document_no;
        //                //tagout.Ref_Process_Index = itemvc.Product_id;
        //                //tagout.Ref_Document_No = itemvc.Product_id;
        //                //tagout.Ref_Document_Index = itemvc.Product_id;
        //                //tagout.Ref_DocumentItem_Index = itemvc.Product_id;
        //                tagout.Create_By = "";
        //                tagout.Create_Date = DateTime.Now;

        //                db.WM_TagOutItem.Add(tagout);
        //            }
        //        }
        //        var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
        //        try
        //        {
        //            db.SaveChanges();
        //            transaction.Commit();
        //        }
        //        catch (Exception exy)
        //        {
        //            transaction.Rollback();
        //            throw exy;
        //        }


        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        //public bool maketagOut_V2(findtagViewModelItem data)
        //{
        //    try
        //    {


        //        var listGIL = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssue_No == data.GoodsIssue_No).ToList();


        //        var GrouplistGIL = listGIL.GroupBy(c => new { c.GoodsIssue_Index, c.Ref_Document_Index, c.Ref_Document_No })
        //                           .Select(group => new
        //                           {
        //                               GIIndex = group.Key.GoodsIssue_Index,
        //                               RefDocIndex = group.Key.Ref_Document_Index,
        //                               RefDocNo = group.Key.Ref_Document_No
        //                           }).ToList();


        //        //decimal ToteCM3XL = 61280;  //76.608L
        //        //decimal ToteCM3S = 28800;  //36L



        //        decimal ToteCM3XL = 46000;  // 45,964.8  60%
        //        decimal ToteCM3S = 21600;  // 21600  60%


        //        string ToteName = "XL";  //76.608L
        //        bool flagSmallTote = false;

        //        decimal ToteRemainCM3 = 0;  //36L
        //        int TotalBoxcount = 0;
        //        string zero = "00000";
        //        Guid tagout_index;
        //        string tagout_no = "";
        //        Boolean NewTote = true;
        //        // Loop By Plan GI No
        //        var count = 0;
        //        string oldPlanGI_No = "";
        //        foreach (var item in GrouplistGIL)
        //        {

        //            zero = "00000";

        //            count = 0;
        //            TotalBoxcount = 0;
        //            var countTotalBox = 0;
        //            List<View_GoodsIssuecount_tag_VC> listgood_I_no_VC = db.View_GoodsIssuecount_tag_VC.Where(c => c.GoodsIssue_No == data.GoodsIssue_No && c.Ref_Document_Index == item.RefDocIndex).OrderBy(c => c.Ref_Document_No).ThenByDescending(n => n.Location_Name).ThenByDescending(v => v.Vol_Per_Unit).ToList();





        //            //var checkSumVol = listgood_I_no_VC.Select(c => c.QTYCTN * c.Vol_Per_Unit).Sum();
        //            //if (checkSumVol < ToteCM3S)
        //            //{
        //            //    flagSmallTote = true;
        //            //}

        //            //ToteRemainCM3 = ToteCM3XL;


        //            string oldLocation = "";
        //            foreach (View_GoodsIssuecount_tag_VC itemvc in listgood_I_no_VC)
        //            {

        //                var branch_code = itemvc.Branch_Code == null ? "0000000" : itemvc.Branch_Code;
        //                var ref_document_no = itemvc.Ref_Document_No == null ? "0000000000" : itemvc.Ref_Document_No;


        //                //TODO : Logic set totesize 
        //                //
        //                //
        //                //
        //                //if (itemvc.Location_Name.Contains("VC") == true)
        //                //{
        //                //    oldLocation = oldLocation;
        //                //}

        //                //if (itemvc.Location_Name.Contains("PA") == true)
        //                //{
        //                //    oldLocation = oldLocation;
        //                //}

        //                //if (itemvc.Location_Name.Contains("PB") ==true )
        //                //  {
        //                //    oldLocation = oldLocation;
        //                //}
        //                //if ((oldLocation.Contains("PB") == true && itemvc.Location_Name.Contains("PA") == true) || (oldLocation.Contains("PA") == true && itemvc.Location_Name.Contains("PB") == true) )
        //                //{
        //                //    oldLocation = oldLocation;
        //                //}

        //                if (ref_document_no == "2003993110")
        //                {
        //                    ref_document_no = "2003993110";

        //                }
        //                var QtyCTN = Convert.ToInt32(itemvc.QTYCTN);
        //                for (int i = 0; i < QtyCTN; i++)// Loop  ตาม QTY
        //                {

        //                    if ((ToteRemainCM3 - itemvc.Vol_Per_Unit) < 0 || (oldLocation.Contains("PB") == true && itemvc.Location_Name.Contains("PA") == true) || (oldLocation.Contains("PA") == true && itemvc.Location_Name.Contains("PB") == true || oldPlanGI_No != ref_document_no))   // เปิดกล่องใหม่
        //                    {

        //                        oldPlanGI_No = ref_document_no;
        //                        if (itemvc.Location_Name.Contains("VC") != true)
        //                        {
        //                            oldLocation = itemvc.Location_Name.ToString();
        //                        }


        //                        //if (oldLocation.Contains("PB") == true)
        //                        //{
        //                        //    zero = "0000B";
        //                        //}

        //                        //if (oldLocation.Contains("PA") == true)
        //                        //{
        //                        //    zero = "0000A";
        //                        //}

        //                        //TODO : Logic set totesize 
        //                        //
        //                        //
        //                        //

        //                        //TODO : Logic set totesize   
        //                        if (flagSmallTote == true)
        //                        {

        //                            ToteRemainCM3 = ToteCM3S;
        //                            ToteName = "M";
        //                        }
        //                        else
        //                        {
        //                            ToteRemainCM3 = ToteCM3XL;
        //                            ToteName = "XL";
        //                        }

        //                        // ToteRemainCM3 = ToteCM3XL;     // Set Max CAP Tote

        //                        ToteRemainCM3 = ToteRemainCM3 - (decimal)itemvc.Vol_Per_Unit;  // Cal Remian Vol


        //                        // Add Tagout
        //                        var runing = "";

        //                        TotalBoxcount++;

        //                        tagout_index = Guid.NewGuid();
        //                        var tagnew = new wm_TagOut();

        //                        if (TotalBoxcount < 10)
        //                        {
        //                            runing = "00" + TotalBoxcount;
        //                        }
        //                        else if (TotalBoxcount >= 100)
        //                        {
        //                            runing = TotalBoxcount.ToString();
        //                        }
        //                        else if (TotalBoxcount >= 10)
        //                        {
        //                            runing = "0" + TotalBoxcount;
        //                        }


        //                        tagnew.TagOut_Index = tagout_index;
        //                        tagnew.TagOut_No = zero + ref_document_no + branch_code + runing;
        //                        tagout_no = tagnew.TagOut_No;
        //                        tagnew.TagOutRef_No1 = itemvc.Branch_Code;
        //                        tagnew.TagOutRef_No2 = itemvc.Chute_ID;
        //                        tagnew.TagOutRef_No3 = itemvc.Branch_Name;
        //                        tagnew.TagOutRef_No4 = runing;
        //                        tagnew.TagOutRef_No5 = itemvc.QTYCTN.GetValueOrDefault().ToString();
        //                        tagnew.TagOut_Status = 0;
        //                        tagnew.UDF_1 = itemvc.Ref_Document_No;
        //                        tagnew.UDF_2 = itemvc.TotalQty.ToString();
        //                        tagnew.UDF_3 = itemvc.ShipTo_Index == null ? null : itemvc.ShipTo_Index.Value.ToString();
        //                        tagnew.UDF_4 = itemvc.Ref_Document_Index == null ? null : itemvc.Ref_Document_Index.GetValueOrDefault().ToString();
        //                        tagnew.UDF_5 = itemvc.Ref_Document_Index == null ? null : itemvc.Ref_DocumentItem_Index.GetValueOrDefault().ToString();
        //                        tagnew.Zone_Index = null;
        //                        tagnew.Ref_Process_Index = Guid.Parse(itemvc.Ref_Process_Index);
        //                        tagnew.Ref_Document_No = itemvc.GoodsIssue_No;
        //                        tagnew.Ref_Document_Index = itemvc.GoodsIssue_Index;
        //                        tagnew.Ref_DocumentItem_Index = itemvc.GoodsIssueItemLocation_Index;
        //                        tagnew.Create_By = "";
        //                        tagnew.Create_Date = DateTime.Now;
        //                        tagnew.TagOutType = itemvc.TagOutType;
        //                        tagnew.LocationType = itemvc.LocationType;


        //                        db.WM_TagOut.Add(tagnew);


        //                        // }
        //                        //------------------------------------------------
        //                        // Add item in Tagout Item 

        //                        Guid tagoutitem_index = Guid.NewGuid();
        //                        var tagout = new wm_TagOutItem();
        //                        tagout.TagOutItem_Index = tagoutitem_index;
        //                        tagout.TagOut_Index = tagout_index;
        //                        tagout.TagOut_No = tagout_no;
        //                        tagout.GoodsIssue_Index = itemvc.GoodsIssue_Index;
        //                        //tagout.GoodsIssueItem_Index = itemvc.Product_id;
        //                        tagout.GoodsIssueItemLocation_Index = itemvc.GoodsIssueItemLocation_Index;
        //                        // tagout.Carton_No = itemvc.Product_id;
        //                        tagout.Product_Index = itemvc.Product_Index;// new Guid(itemvc.Product_Index) ;
        //                        tagout.Product_Id = itemvc.Product_Id;
        //                        tagout.Product_Name = itemvc.Product_Name;
        //                        //  tagout.Product_SecondName = itemvc.Product_SecondName;
        //                        //  tagout.Product_ThirdName = itemvc.Product_ThirdName;
        //                        tagout.Product_Lot = itemvc.Product_Lot;
        //                        tagout.EXP_Date = itemvc.EXP_Date;
        //                        tagout.ItemStatus_Index = itemvc.ItemStatus_Index;
        //                        tagout.ItemStatus_Id = itemvc.ItemStatus_Id;
        //                        tagout.ItemStatus_Name = itemvc.ItemStatus_Name;
        //                        tagout.Qty = 1;//itemvc.Product_id;
        //                        tagout.Ratio = itemvc.SALE_Ratio; //itemvc.Product_id;
        //                        tagout.TotalQty = 1 * itemvc.SALE_Ratio; //itemvc.Product_id;
        //                        tagout.ProductConversion_Index = itemvc.ProductConversion_Index;
        //                        tagout.ProductConversion_Id = itemvc.ProductConversion_Id;
        //                        tagout.ProductConversion_Name = itemvc.ProductConversion_Name;
        //                        //  tagout.Weight = itemvc.Product_id;
        //                        tagout.Volume = itemvc.Vol_Per_Unit;

        //                        tagout.TagOut_Status = 0;
        //                        tagout.TagOutRef_No1 = itemvc.Location_Name;
        //                        tagout.TagOutRef_No2 = ToteName;
        //                        tagout.TagOutRef_No3 = itemvc.Vol_Per_Unit.ToString();
        //                        tagout.TagOutRef_No4 = ref_document_no;
        //                        //tagout.Ref_Process_Index = itemvc.Product_id;
        //                        //tagout.Ref_Document_No = itemvc.Product_id;
        //                        //tagout.Ref_Document_Index = itemvc.Product_id;
        //                        //tagout.Ref_DocumentItem_Index = itemvc.Product_id;
        //                        tagout.Create_By = "";
        //                        tagout.Create_Date = DateTime.Now;

        //                        db.WM_TagOutItem.Add(tagout);

        //                    }
        //                    else // ใส่ของได้อยู่ ใช้กล่องเดิม
        //                    {

        //                        oldLocation = itemvc.Location_Name.ToString();

        //                        ToteRemainCM3 = ToteRemainCM3 - (decimal)itemvc.Vol_Per_Unit;

        //                        Guid tagoutitem_index = Guid.NewGuid();
        //                        var tagout = new wm_TagOutItem();
        //                        tagout.TagOutItem_Index = tagoutitem_index;
        //                        tagout.TagOut_Index = tagout_index;
        //                        tagout.TagOut_No = tagout_no;
        //                        tagout.GoodsIssue_Index = itemvc.GoodsIssue_Index;
        //                        //tagout.GoodsIssueItem_Index = itemvc.Product_id;
        //                        tagout.GoodsIssueItemLocation_Index = itemvc.GoodsIssueItemLocation_Index;
        //                        // tagout.Carton_No = itemvc.Product_id;
        //                        tagout.Product_Index = itemvc.Product_Index;// new Guid(itemvc.Product_Index) ;
        //                        tagout.Product_Id = itemvc.Product_Id;
        //                        tagout.Product_Name = itemvc.Product_Name;
        //                        //  tagout.Product_SecondName = itemvc.Product_SecondName;
        //                        //  tagout.Product_ThirdName = itemvc.Product_ThirdName;
        //                        tagout.Product_Lot = itemvc.Product_Lot;
        //                        tagout.EXP_Date = itemvc.EXP_Date;
        //                        tagout.ItemStatus_Index = itemvc.ItemStatus_Index;
        //                        tagout.ItemStatus_Id = itemvc.ItemStatus_Id;
        //                        tagout.ItemStatus_Name = itemvc.ItemStatus_Name;
        //                        tagout.Qty = 1;//itemvc.Product_id;
        //                        tagout.Ratio = itemvc.SALE_Ratio; //itemvc.Product_id;
        //                        tagout.TotalQty = 1 * itemvc.SALE_Ratio; //itemvc.Product_id;
        //                        tagout.ProductConversion_Index = itemvc.ProductConversion_Index;
        //                        tagout.ProductConversion_Id = itemvc.ProductConversion_Id;
        //                        tagout.ProductConversion_Name = itemvc.ProductConversion_Name;
        //                        //  tagout.Weight = itemvc.Product_id;
        //                        tagout.Volume = itemvc.Vol_Per_Unit;
        //                        tagout.TagOutRef_No1 = itemvc.Location_Name;
        //                        tagout.TagOut_Status = 0;
        //                        tagout.TagOutRef_No2 = ToteName;
        //                        tagout.TagOutRef_No3 = itemvc.Vol_Per_Unit.ToString();
        //                        tagout.TagOutRef_No4 = ref_document_no;
        //                        //tagout.Ref_Process_Index = itemvc.Product_id;
        //                        //tagout.Ref_Document_No = itemvc.Product_id;
        //                        //tagout.Ref_Document_Index = itemvc.Product_id;
        //                        //tagout.Ref_DocumentItem_Index = itemvc.Product_id;
        //                        tagout.Create_By = "";
        //                        tagout.Create_Date = DateTime.Now;

        //                        db.WM_TagOutItem.Add(tagout);


        //                    }


        //                }




        //            }


        //            //////////////
        //            ///

        //            List<View_GoodsIssuecount_tag> listgood_I_no = db.View_GoodsIssuecount_tag.Where(c => c.GoodsIssue_No == data.GoodsIssue_No && c.Ref_Document_Index == item.RefDocIndex).OrderBy(c => c.Ref_Document_No).ToList();
        //            var Document_no = "";
        //            var branch = "";


        //            count = count + TotalBoxcount;
        //            foreach (View_GoodsIssuecount_tag itxem in listgood_I_no)
        //            {


        //                var branch_code = itxem.Branch_Code == null ? "0000000" : itxem.Branch_Code;
        //                var ref_document_no = itxem.Ref_Document_No == null ? "0000000000" : itxem.Ref_Document_No;
        //                if (Document_no != ref_document_no)
        //                {
        //                    count = 0 + TotalBoxcount;
        //                    Document_no = ref_document_no;
        //                }
        //                if (branch != branch_code)
        //                {
        //                    count = 0 + TotalBoxcount;
        //                    branch = branch_code;
        //                }
        //                int count_item = (int)itxem.QTYCTN;
        //                for (int i = 0; i < count_item; i++)
        //                {
        //                    var runing = "";
        //                    count++;
        //                    Guid tag = Guid.NewGuid();
        //                    var tagnew = new wm_TagOut();

        //                    if (count < 10)
        //                    {
        //                        runing = "00" + count;
        //                    }
        //                    else if (count >= 100)
        //                    {
        //                        runing = count.ToString();
        //                    }
        //                    else if (count >= 10)
        //                    {
        //                        runing = "0" + count;
        //                    }
        //                    tagnew.TagOut_Index = tag;
        //                    tagnew.TagOut_No = "00000" + ref_document_no + branch_code + runing;
        //                    tagnew.TagOutRef_No1 = itxem.Branch_Code;
        //                    tagnew.TagOutRef_No2 = itxem.Chute_ID;
        //                    tagnew.TagOutRef_No3 = itxem.Branch_Name;
        //                    tagnew.TagOutRef_No4 = runing;
        //                    tagnew.TagOutRef_No5 = itxem.QTYCTN.GetValueOrDefault().ToString();
        //                    tagnew.TagOut_Status = 0;
        //                    tagnew.UDF_1 = itxem.Ref_Document_No;
        //                    tagnew.UDF_2 = itxem.TotalQty.ToString();
        //                    tagnew.UDF_3 = itxem.ShipTo_Index == null ? null : itxem.ShipTo_Index.Value.ToString();
        //                    tagnew.UDF_4 = itxem.Ref_Document_Index == null ? null : itxem.Ref_Document_Index.GetValueOrDefault().ToString();
        //                    tagnew.UDF_5 = itxem.Ref_Document_Index == null ? null : itxem.Ref_DocumentItem_Index.GetValueOrDefault().ToString();
        //                    tagnew.Zone_Index = null;
        //                    tagnew.Ref_Process_Index = Guid.Parse(itxem.Ref_Process_Index);
        //                    tagnew.Ref_Document_No = itxem.GoodsIssue_No;
        //                    tagnew.Ref_Document_Index = itxem.GoodsIssue_Index;
        //                    tagnew.Ref_DocumentItem_Index = itxem.GoodsIssueItemLocation_Index;
        //                    tagnew.Create_By = "";
        //                    tagnew.Create_Date = DateTime.Now;
        //                    tagnew.TagOutType = itxem.TagOutType;
        //                    tagnew.LocationType = itxem.LocationType;


        //                    db.WM_TagOut.Add(tagnew);

        //                    //////////////////////
        //                    ///
        //                    Guid tagoutitem_index = Guid.NewGuid();
        //                    var tagout = new wm_TagOutItem();
        //                    tagout.TagOutItem_Index = tagoutitem_index;
        //                    tagout.TagOut_Index = tagnew.TagOut_Index;
        //                    tagout.TagOut_No = tagnew.TagOut_No;
        //                    tagout.GoodsIssue_Index = itxem.GoodsIssue_Index;
        //                    //tagout.GoodsIssueItem_Index = itxem.Product_id;
        //                    tagout.GoodsIssueItemLocation_Index = itxem.GoodsIssueItemLocation_Index;
        //                    // tagout.Carton_No = itxem.Product_id;
        //                    tagout.Product_Index = itxem.Product_Index;// new Guid(itxem.Product_Index) ;
        //                    tagout.Product_Id = itxem.Product_Id;
        //                    tagout.Product_Name = itxem.Product_Name;
        //                    //  tagout.Product_SecondName = itxem.Product_SecondName;
        //                    //  tagout.Product_ThirdName = itxem.Product_ThirdName;
        //                    tagout.Product_Lot = itxem.Product_Lot;
        //                    tagout.EXP_Date = itxem.EXP_Date;
        //                    tagout.ItemStatus_Index = itxem.ItemStatus_Index;
        //                    tagout.ItemStatus_Id = itxem.ItemStatus_Id;
        //                    tagout.ItemStatus_Name = itxem.ItemStatus_Name;
        //                    tagout.Qty = 1;//itxem.Product_id;
        //                    tagout.Ratio = itxem.SALE_Ratio; //itxem.Product_id;
        //                    tagout.TotalQty = 1 * itxem.SALE_Ratio; //itxem.Product_id;
        //                    tagout.ProductConversion_Index = itxem.ProductConversion_Index;
        //                    tagout.ProductConversion_Id = itxem.ProductConversion_Id;
        //                    tagout.ProductConversion_Name = itxem.ProductConversion_Name;
        //                    //  tagout.Weight = itxem.Product_id;
        //                    tagout.Volume = itxem.Vol_Per_Unit;
        //                    tagout.TagOutRef_No1 = "";
        //                    tagout.TagOut_Status = 0;
        //                    tagout.TagOutRef_No2 = "";
        //                    tagout.TagOutRef_No3 = itxem.Vol_Per_Unit.ToString();
        //                    tagout.TagOutRef_No4 = ref_document_no;
        //                    //tagout.Ref_Process_Index = itemvc.Product_id;
        //                    //tagout.Ref_Document_No = itemvc.Product_id;
        //                    //tagout.Ref_Document_Index = itemvc.Product_id;
        //                    //tagout.Ref_DocumentItem_Index = itemvc.Product_id;
        //                    tagout.Create_By = "";
        //                    tagout.Create_Date = DateTime.Now;

        //                    db.WM_TagOutItem.Add(tagout);
        //                }
        //            }


        //        }// Loop Plan GI

        //        var GI = db.IM_GoodsIssue.FirstOrDefault(c => c.GoodsIssue_No == data.GoodsIssue_No);
        //        GI.TagOut_status = 1;

        //        var transactionX = db.Database.BeginTransaction(IsolationLevel.Serializable);
        //        try
        //        {
        //            db.SaveChanges();
        //            transactionX.Commit();

        //            try
        //            {
        //                var pgi_index = new SqlParameter("@GoodsIssue_Index", GI.GoodsIssue_Index);
        //                var resultTask = db.Database.ExecuteSqlCommand("EXEC sp_UpdateToteSize @GoodsIssue_Index", pgi_index);



        //                string cmd1 = "";


        //                cmd1 += "       Update WMSDB_Outbound..wm_TagOutitem set                           ";
        //                cmd1 += "            TagOut_No = '0000B' + SUBSTRING(TagOut_No, 6, 20)                           ";
        //                cmd1 += "        where TagOut_Index   in   (                                                      ";
        //                cmd1 += "                                select    TagOut_Index                                                  ";
        //                cmd1 += "                                from WMSDB_Outbound..wm_TagOutitem                               ";
        //                cmd1 += "                                where TagOutRef_No1 like 'PB%'                         ";
        //                cmd1 += "                                 and SUBSTRING(TagOut_No , 1,5) = '00000'                   ";
        //                cmd1 += "                                 and GoodsIssue_Index = '" + GI.GoodsIssue_Index + "'";
        //                cmd1 += "                                )                                                           ";

        //                string cmd2 = "";


        //                cmd2 += "       Update WMSDB_Outbound..wm_TagOutitem set                           ";
        //                cmd2 += "            TagOut_No = '0000A' + SUBSTRING(TagOut_No, 6, 20)                           ";
        //                cmd2 += "        where TagOut_Index   in   (                                                      ";
        //                cmd2 += "                                select    TagOut_Index                                                  ";
        //                cmd2 += "                                from WMSDB_Outbound..wm_TagOutitem                               ";
        //                cmd2 += "                                where TagOutRef_No1 like 'PA%'                         ";
        //                cmd2 += "                                 and SUBSTRING(TagOut_No , 1,5) = '00000'                   ";
        //                cmd2 += "                                 and GoodsIssue_Index = '" + GI.GoodsIssue_Index + "'";
        //                cmd2 += "                                )    ";


        //                string cmd3 = "";

        //                cmd3 += "       Update WMSDB_Outbound..wm_TagOutitem set                           ";
        //                cmd3 += "            TagOut_No = '0000A' + SUBSTRING(TagOut_No, 6, 20)                           ";
        //                cmd3 += "        where TagOut_Index   in   (                                                      ";
        //                cmd3 += "                                select    TagOut_Index                                                  ";
        //                cmd3 += "                                from WMSDB_Outbound..wm_TagOutitem                               ";
        //                cmd3 += "                                where TagOutRef_No1 like 'VC%'                         ";
        //                cmd3 += "                                 and SUBSTRING(TagOut_No , 1,5) = '00000'                   ";
        //                cmd3 += "                                 and GoodsIssue_Index = '" + GI.GoodsIssue_Index + "'";
        //                cmd3 += "                                )    ";

        //                var r1 = db.Database.ExecuteSqlCommand(cmd1);
        //                var r2 = db.Database.ExecuteSqlCommand(cmd2);
        //                var r3 = db.Database.ExecuteSqlCommand(cmd3);



        //            }
        //            catch (Exception exxx)
        //            {

        //                //throw exxx;
        //            }
        //        }
        //        catch (Exception exy)
        //        {
        //            transactionX.Rollback();
        //            throw exy;
        //        }



        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public bool maketagOut(findtagViewModelItem data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            olog.logging("maketagOut", State);
            olog.logging("loop_tagbyplan", State);

            try
            {

                db.Database.SetCommandTimeout(360);

                //olog.logging("maketagOut", "Check WM_TagOut");

                //var listTAGOUT = db.WM_TagOut.Where(c => c.Ref_Document_No == data.GoodsIssue_No ).ToList();

                //if (listTAGOUT.Count() != 0 )
                //{
                //    olog.logging("maketagOut", "listTAGOUT Have to DATA ");

                //    return false;
                //}

                olog.logging("maketagOut", "get sp_CheckB4Tote");

                var pstrGoodsIssue_No = new SqlParameter("@GoodsIssue_No", data.GoodsIssue_No );
                var CheckB4ToteResult = db.CheckB4Wave.FromSql("sp_CheckB4Tote @GoodsIssue_No ", pstrGoodsIssue_No).ToList();


                foreach (var item in CheckB4ToteResult)
                {
                    if (item.CountRows > 0)
                    {
                        //resultWave.resultIsUse = false;
                        //resultWave.resultMsg = "กรุณาตรวจสอบ : " + item.msgCheck;
                        //return resultWave;
                        olog.logging("maketagOut", "sp_CheckB4Tote : " + item.msgCheck.ToString ());
                        return false;
                    }

                }


                olog.logging("maketagOut", "get IM_GoodsIssueItemLocation");

                var listGIL = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssue_No == data.GoodsIssue_No).ToList();


                var GrouplistGIL = listGIL.GroupBy(c => new { c.GoodsIssue_Index, c.Ref_Document_Index, c.Ref_Document_No })
                                   .Select(group => new
                                   {
                                       GIIndex = group.Key.GoodsIssue_Index,
                                       RefDocIndex = group.Key.Ref_Document_Index,
                                       RefDocNo = group.Key.Ref_Document_No
                                   }).ToList();


                olog.logging("maketagOut", "GrouplistGIL : " + GrouplistGIL.Count().ToString ());

                //decimal ToteCM3XL = 61280;  //76.608L
                //decimal ToteCM3S = 28800;  //36L

                decimal ToteCM3XL = 46000;  // 45,964.8  60%
                decimal ToteCM3S = 21600;  // 21600  60%
                string ToteName = "XL";  //76.608L
                bool flagSmallTote = false;

                decimal ToteRemainCM3 = 0;  //36L
                int TotalBoxcount = 0;
                string zero = "00000";
                Guid tagout_index;
                string tagout_no = "";
                Boolean NewTote = true;
                // Loop By Plan GI No
                var count = 0;
                var countplan = 0;
                string oldPlanGI_No = "";
                string checkPlangGI = "";
                string PlangGIChuteNo = "";
                foreach (var item in GrouplistGIL)
                {

                    zero = "000";

                    count = 0;
                    TotalBoxcount = 0;
                    ToteRemainCM3 = 0;
                    var countTotalBox = 0;
                    countplan = countplan+1;
                    olog.logging("loop_tagbyplan", "countplan : " + countplan);

                    // Order by PlanGI  , Vol , Location
                    //  List<View_GoodsIssuecount_tag_VC> listgood_I_no_VC = db.View_GoodsIssuecount_tag_VC.Where(c => c.GoodsIssue_No == data.GoodsIssue_No && c.Ref_Document_Index == item.RefDocIndex).OrderBy(c => c.Ref_Document_No).ThenByDescending(v => v.Vol_Per_Unit).ThenByDescending(n => n.Location_Name).ToList();

                    olog.logging("maketagOut", "PlanGI NO : " + item.RefDocNo);

                    checkPlangGI = item.RefDocNo;
                    List<View_GoodsIssuecount_tag_VC> listgood_I_no_VC = db.View_GoodsIssuecount_tag_VC.Where(c => c.GoodsIssue_No == data.GoodsIssue_No && c.Ref_Document_Index == item.RefDocIndex).ToList();

                    olog.logging("maketagOut", "select 1 : " + item.RefDocNo);
                    #region Loop Tote Qty By PlanGI

                    /// Loop Item 1 Qty In List
                    var listTagoutItem = new List<wm_TagOutItem>();
                    foreach (View_GoodsIssuecount_tag_VC itemvc in listgood_I_no_VC.OrderBy(c => c.Ref_Document_No).ThenByDescending(v => v.Vol_Per_Unit).ThenBy(m => m.Product_Id).ThenByDescending(n => n.Location_Name))
                    {


                        if (itemvc.Chute_No.Length  == 2)
                        {
                            PlangGIChuteNo = itemvc.Chute_No;
                        }
                        else if (itemvc.Chute_No.Length == 1)
                        {
                            PlangGIChuteNo = "0" +  itemvc.Chute_No;
                        }
                        else
                        {
                            PlangGIChuteNo = "00";

                        }
                   


                        var QtyCTN = Convert.ToInt32(itemvc.QTYCTN);
                        for (int i = 0; i < QtyCTN; i++)// Loop  ตาม QTY
                        {


                            var branch_code = itemvc.Branch_Code == null ? "0000000" : itemvc.Branch_Code;
                            var ref_document_no = itemvc.Ref_Document_No == null ? "0000000000" : itemvc.Ref_Document_No;

                            Guid tagoutitem_index = Guid.NewGuid();
                            var tagout = new wm_TagOutItem();
                            tagout.TagOutItem_Index = tagoutitem_index;
                            tagout.TagOut_Index = tagout_index;
                            tagout.TagOut_No = tagout_no;
                            tagout.GoodsIssue_Index = itemvc.GoodsIssue_Index;
                            tagout.GoodsIssueItemLocation_Index = itemvc.GoodsIssueItemLocation_Index;

                            tagout.Carton_No = QtyCTN.ToString();
                            tagout.Product_Index = itemvc.Product_Index;// new Guid(itemvc.Product_Index) ;
                            tagout.Product_Id = itemvc.Product_Id;
                            tagout.Product_Name = itemvc.Product_Name;
                            //  tagout.Product_SecondName = itemvc.Product_SecondName;
                            //  tagout.Product_ThirdName = itemvc.Product_ThirdName;
                            tagout.Product_Lot = itemvc.Product_Lot;
                            tagout.EXP_Date = itemvc.EXP_Date;
                            tagout.ItemStatus_Index = itemvc.ItemStatus_Index;
                            tagout.ItemStatus_Id = itemvc.ItemStatus_Id;
                            tagout.ItemStatus_Name = itemvc.ItemStatus_Name;
                            tagout.Qty = 1;//itemvc.Product_id;
                            tagout.Ratio = itemvc.SALE_Ratio; //itemvc.Product_id;
                            tagout.TotalQty = 1 * itemvc.SALE_Ratio; //itemvc.Product_id;
                            tagout.ProductConversion_Index = itemvc.ProductConversion_Index;
                            tagout.ProductConversion_Id = itemvc.ProductConversion_Id;
                            tagout.ProductConversion_Name = itemvc.ProductConversion_Name;
                            //  tagout.Weight = itemvc.Product_id;
                            tagout.Weight = itemvc.ProductConversion_Weight;
                            tagout.Volume = itemvc.Vol_Per_Unit;
                            tagout.UDF_1 = itemvc.ShipTo_Index.ToString();
                            tagout.UDF_2 = itemvc.Branch_Name;
                            tagout.UDF_3 = itemvc.Chute_ID;

                            tagout.UDF_4 = itemvc.MaxToteM;
                            tagout.UDF_5 = itemvc.MaxToteL;
                            tagout.TagOut_Status = 0;
                            tagout.TagOutRef_No1 = itemvc.Location_Name;
                            tagout.TagOutRef_No2 = ToteName;
                            tagout.TagOutRef_No3 = itemvc.Vol_Per_Unit.ToString();
                            tagout.TagOutRef_No4 = ref_document_no;
                            tagout.TagOutRef_No5 = itemvc.Branch_Code;
                            tagout.Create_By = itemvc.GoodsIssue_No;
                            tagout.Create_Date = DateTime.Now;
                            tagout.Ref_Document_Index = itemvc.Ref_Document_Index;
                            tagout.Ref_DocumentItem_Index = itemvc.Ref_DocumentItem_Index;
                            tagout.Ref_Document_No = itemvc.Ref_Document_No;
                            listTagoutItem.Add(tagout);



                        }
                    }



                    #endregion

                    //------------------------------------------------------------------------------------------------//
                    olog.logging("maketagOut", "Chute NO : " + PlangGIChuteNo);


                    var newTote = true;
                    var countTAG = listTagoutItem.Count();
                    countTAG = countTAG + 5;
                    int iTote = 0;
                    string oldLocation = "";

                    olog.logging("maketagOut", "CountSKUtote : " + countTAG.ToString());

                    for (int iLoop = 0; iLoop < countTAG; iLoop++)// Loop  ตาม QTY
                    {

                        // ToteRemainCM3 = ToteCM3XL;     // Set Max CAP Tote

                        var CheckCount = listTagoutItem.Where(c => c.TagOut_Status == 0).ToList();
                        if (CheckCount.Count() <= 0)
                        {
                            olog.logging("maketagOut", "CheckCount break  ");

                            break;
                        }

                        ToteRemainCM3 = 0;
                        oldLocation = "";

                        DataTable tbTagOutItem = new DataTable();
                        tbTagOutItem.Columns.Add("TagOut_Index", typeof(string));
                        tbTagOutItem.Columns.Add("TagOut_No", typeof(string));
                        tbTagOutItem.Columns.Add("Product_Id", typeof(string));
                        tbTagOutItem.Columns.Add("Product_Name", typeof(string));
                        tbTagOutItem.Columns.Add("ProductConversion_Id", typeof(string));
                        tbTagOutItem.Columns.Add("ProductConversion_Name", typeof(string));
                        tbTagOutItem.Columns.Add("Qty", typeof(decimal));
                        tbTagOutItem.Columns.Add("Ratio", typeof(decimal));
                        tbTagOutItem.Columns.Add("TotalQty", typeof(decimal));
                        tbTagOutItem.Columns.Add("Location_Name", typeof(string));

                        int icount = 0;
                        //    var listTagoutItemZero = listTagoutItem.Where(c => c.TagOut_Status == 0).ToList();
                        foreach (wm_TagOutItem itemTAG in listTagoutItem.Where(c => c.TagOut_Status == 0).OrderBy(c => c.Volume).ThenBy(v => v.Product_Id).ThenBy(n => n.TagOutRef_No1))
                        {

                            var branch_code = itemTAG.TagOutRef_No5 == null ? "0000000" : itemTAG.TagOutRef_No5;
                            var ref_document_no = itemTAG.Ref_Document_No == null ? "0000000000" : itemTAG.Ref_Document_No;


                            var Tag = itemTAG.TagOutItem_Index;
                            var Loc = itemTAG.TagOutRef_No1;
                            var PRD = itemTAG.Product_Id;

                            icount = icount + 1;

                            olog.logging("maketagOut", "Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());



                            if ((ToteRemainCM3 - itemTAG.Volume) < 0 || (oldPlanGI_No != ref_document_no))   // เปิดกล่องใหม่
                            {

                                olog.logging("maketagOut", " OpenTote  Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());


                                if ((oldLocation.Contains("PB") == true && itemTAG.TagOutRef_No1.Contains("PA") == true))
                                {

                                    olog.logging("maketagOut", " PB continue  Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());
                                    continue;
                                }

                                if ((oldLocation.Contains("PA") == true && itemTAG.TagOutRef_No1.Contains("PB") == true))
                                {

                                    olog.logging("maketagOut", " PA continue  Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());
                                    continue;
                                }

                                Boolean breakSKU = false;

                                //Check SKU Lit
                                if (tbTagOutItem.Rows.Count > 0)
                                {
                                    var sku_ID = itemTAG.Product_Id;
                                    var MaxSkuToteL = Convert.ToInt32(itemTAG.UDF_5);

                                    var expression = " Product_Id = '" + sku_ID + "'";
                                    DataRow[] foundRows = tbTagOutItem.Select(expression);

                                    var CountRows = foundRows.Count();
                                    if (MaxSkuToteL < (CountRows + 1) && MaxSkuToteL != 0)
                                    {
                                        breakSKU = true;

                                    }

                                }
                                if (breakSKU == true)
                                {
                                    olog.logging("maketagOut", " breakSKU  Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());

                                    continue;
                                }





                                ToteRemainCM3 = ToteCM3XL;
                                ToteName = "XL";


                                oldPlanGI_No = ref_document_no;

                                if (itemTAG.TagOutRef_No1.Contains("VC") != true)
                                {
                                    oldLocation = itemTAG.TagOutRef_No1.ToString();
                                }


                                // ToteRemainCM3 = ToteCM3XL;     // Set Max CAP Tote



                                ToteRemainCM3 = ToteRemainCM3 - (decimal)itemTAG.Volume;  // Cal Remian Vol

                                itemTAG.TagOut_Status = 1;



                                olog.logging("maketagOut", " ToteRemainCM3 " + ToteRemainCM3.ToString() + " Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());


                                // Add Tagout
                                var runing = "";

                                TotalBoxcount = TotalBoxcount + 1;

                                tagout_index = Guid.NewGuid();
                                var tagnew = new wm_TagOut();

                                if (TotalBoxcount < 10)
                                {
                                    runing = "00" + TotalBoxcount.ToString();
                                }
                                else if (TotalBoxcount >= 100)
                                {
                                    runing = TotalBoxcount.ToString();
                                }
                                else if (TotalBoxcount >= 10)
                                {
                                    runing = "0" + TotalBoxcount.ToString();
                                }


                                tagnew.TagOut_Index = tagout_index;
                                tagnew.TagOut_No = zero + ref_document_no + branch_code  + PlangGIChuteNo  + runing;
                                tagout_no = tagnew.TagOut_No;

                                olog.logging("maketagOut", " tagout_no " + tagout_no.ToString() + " Plan :" + ref_document_no + " / " + checkPlangGI + " Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());


                                tagnew.TagOutRef_No1 = itemTAG.TagOutRef_No5;  // Branch_Code
                                tagnew.TagOutRef_No2 = itemTAG.UDF_3;   //Chute_ID
                                tagnew.TagOutRef_No3 = itemTAG.UDF_2;   //Branch_Name
                                tagnew.TagOutRef_No4 = runing;
                                tagnew.TagOutRef_No5 = itemTAG.Carton_No;
                                tagnew.TagOut_Status = 0;
                                tagnew.UDF_1 = itemTAG.Ref_Document_No;
                                tagnew.UDF_2 = itemTAG.TotalQty.ToString();
                                tagnew.UDF_3 = itemTAG.UDF_1;  //ShipTo_Index
                                tagnew.UDF_4 = itemTAG.Ref_Document_Index == null ? null : itemTAG.Ref_Document_Index.GetValueOrDefault().ToString();
                                tagnew.UDF_5 = itemTAG.Ref_DocumentItem_Index == null ? null : itemTAG.Ref_DocumentItem_Index.GetValueOrDefault().ToString();
                                tagnew.Zone_Index = null;
                                tagnew.Ref_Process_Index = Guid.Parse("22744590-55D8-4448-88EF-5997C252111F");
                                tagnew.Ref_Document_No = itemTAG.Create_By;  //  GoodsIssue_No
                                tagnew.Ref_Document_Index = itemTAG.GoodsIssue_Index;
                                tagnew.Ref_DocumentItem_Index = itemTAG.GoodsIssueItemLocation_Index;
                                tagnew.Create_By = "";
                                tagnew.Create_Date = DateTime.Now;
                                tagnew.TagOutType = "Manual"; // itemTAG.TagOutType;
                                tagnew.LocationType = "CartonFlowRack";// itemTAG.LocationType;


                                db.WM_TagOut.Add(tagnew);


                                // }
                                //------------------------------------------------
                                // Add item in Tagout Item 

                                Guid tagoutitem_index = Guid.NewGuid();
                                var tagout = new wm_TagOutItem();
                                tagout.TagOutItem_Index = tagoutitem_index;
                                tagout.TagOut_Index = tagout_index;
                                tagout.TagOut_No = tagout_no;
                                tagout.GoodsIssue_Index = itemTAG.GoodsIssue_Index;
                                //tagout.GoodsIssueItem_Index = itemTAG.Product_id;
                                tagout.GoodsIssueItemLocation_Index = itemTAG.GoodsIssueItemLocation_Index;
                                // tagout.Carton_No = itemTAG.Product_id;
                                tagout.Product_Index = itemTAG.Product_Index;// new Guid(itemTAG.Product_Index) ;
                                tagout.Product_Id = itemTAG.Product_Id;
                                tagout.Product_Name = itemTAG.Product_Name;
                                //  tagout.Product_SecondName = itemTAG.Product_SecondName;
                                //  tagout.Product_ThirdName = itemTAG.Product_ThirdName;
                                tagout.Product_Lot = itemTAG.Product_Lot;
                                tagout.EXP_Date = itemTAG.EXP_Date;
                                tagout.ItemStatus_Index = itemTAG.ItemStatus_Index;
                                tagout.ItemStatus_Id = itemTAG.ItemStatus_Id;
                                tagout.ItemStatus_Name = itemTAG.ItemStatus_Name;
                                tagout.Qty = 1;//itemTAG.Product_id;
                                tagout.Ratio = itemTAG.Ratio; //itemTAG.Product_id;
                                tagout.TotalQty = itemTAG.TotalQty; //itemTAG.Product_id;
                                tagout.ProductConversion_Index = itemTAG.ProductConversion_Index;
                                tagout.ProductConversion_Id = itemTAG.ProductConversion_Id;
                                tagout.ProductConversion_Name = itemTAG.ProductConversion_Name;
                                tagout.Weight = itemTAG.Weight;
                                tagout.Volume = itemTAG.Volume;

                                tagout.TagOut_Status = 0;
                                tagout.TagOutRef_No1 = itemTAG.TagOutRef_No1;
                                tagout.TagOutRef_No2 = ToteName;
                                tagout.TagOutRef_No3 = "";//itemTAG.Vol_Per_Unit.ToString();
                                tagout.TagOutRef_No4 = ref_document_no;
                                //tagout.Ref_Process_Index = itemTAG.Product_id;
                                //tagout.Ref_Document_No = itemTAG.Product_id;
                                //tagout.Ref_Document_Index = itemTAG.Product_id;
                                //tagout.Ref_DocumentItem_Index = itemTAG.Product_id;
                                tagout.Create_By = "";
                                tagout.Create_Date = DateTime.Now;

                                db.WM_TagOutItem.Add(tagout);



                                olog.logging("maketagOut", " db.WM_TagOutItem.Add  Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());



                                tbTagOutItem.Rows.Add(tagout_index.ToString()
                                                     , tagout_no.ToString()
                                                     , itemTAG.Product_Id
                                                     , itemTAG.Product_Name
                                                     , itemTAG.ProductConversion_Id
                                                     , itemTAG.ProductConversion_Name
                                                     , itemTAG.Qty
                                                     , itemTAG.Ratio
                                                     , itemTAG.TotalQty
                                                     , itemTAG.TagOutRef_No1
                                                     );


                            }
                            else
                            {


                                if ((oldLocation.Contains("PB") == true && itemTAG.TagOutRef_No1.Contains("PA") == true))
                                {

                                    olog.logging("maketagOut", "Oldtote PB continue  Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());
                                    continue;
                                }

                                if ((oldLocation.Contains("PA") == true && itemTAG.TagOutRef_No1.Contains("PB") == true))
                                {

                                    olog.logging("maketagOut", "Oldtote PA continue  Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());
                                    continue;
                                }



                                // oldLocation = itemTAG.TagOutRef_No1.ToString();

                                if (itemTAG.TagOutRef_No1.Contains("VC") != true)
                                {
                                    oldLocation = itemTAG.TagOutRef_No1.ToString();
                                }

                                ToteRemainCM3 = ToteRemainCM3 - (decimal)itemTAG.Volume;

                                itemTAG.TagOut_Status = 1;

                                olog.logging("maketagOut", "Oldtote ToteRemainCM3 " + ToteRemainCM3.ToString() + " Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());


                                Guid tagoutitem_index = Guid.NewGuid();
                                var tagout = new wm_TagOutItem();
                                tagout.TagOutItem_Index = tagoutitem_index;
                                tagout.TagOut_Index = tagout_index;
                                tagout.TagOut_No = tagout_no;



                                olog.logging("maketagOut", "Oldtote tagout_no " + tagout_no.ToString() + " Plan :" + ref_document_no + " / " + checkPlangGI + " Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());


                                tagout.GoodsIssue_Index = itemTAG.GoodsIssue_Index;
                                //tagout.GoodsIssueItem_Index = itemTAG.Product_id;
                                tagout.GoodsIssueItemLocation_Index = itemTAG.GoodsIssueItemLocation_Index;
                                // tagout.Carton_No = itemTAG.Product_id;
                                tagout.Product_Index = itemTAG.Product_Index;// new Guid(itemTAG.Product_Index) ;
                                tagout.Product_Id = itemTAG.Product_Id;
                                tagout.Product_Name = itemTAG.Product_Name;
                                //  tagout.Product_SecondName = itemTAG.Product_SecondName;
                                //  tagout.Product_ThirdName = itemTAG.Product_ThirdName;
                                tagout.Product_Lot = itemTAG.Product_Lot;
                                tagout.EXP_Date = itemTAG.EXP_Date;
                                tagout.ItemStatus_Index = itemTAG.ItemStatus_Index;
                                tagout.ItemStatus_Id = itemTAG.ItemStatus_Id;
                                tagout.ItemStatus_Name = itemTAG.ItemStatus_Name;
                                tagout.Qty = 1;//itemTAG.Product_id;
                                tagout.Ratio = itemTAG.Ratio; //itemTAG.Product_id;
                                tagout.TotalQty = itemTAG.TotalQty; //itemTAG.Product_id;
                                tagout.ProductConversion_Index = itemTAG.ProductConversion_Index;
                                tagout.ProductConversion_Id = itemTAG.ProductConversion_Id;
                                tagout.ProductConversion_Name = itemTAG.ProductConversion_Name;
                                tagout.Weight = itemTAG.Weight;
                                tagout.Volume = itemTAG.Volume;
                                tagout.TagOut_Status = 0;
                                tagout.TagOutRef_No1 = itemTAG.TagOutRef_No1;
                                tagout.TagOutRef_No2 = ToteName;
                                tagout.TagOutRef_No3 = "";//itemTAG.Vol_Per_Unit.ToString();
                                tagout.TagOutRef_No4 = ref_document_no;
                                //tagout.Ref_Process_Index = itemTAG.Product_id;
                                //tagout.Ref_Document_No = itemTAG.Product_id;
                                //tagout.Ref_Document_Index = itemTAG.Product_id;
                                //tagout.Ref_DocumentItem_Index = itemTAG.Product_id;
                                tagout.Create_By = "";
                                tagout.Create_Date = DateTime.Now;

                                db.WM_TagOutItem.Add(tagout);


                                olog.logging("maketagOut", " Oldtote  db.WM_TagOutItem.Add  Loop : " + icount.ToString() + " " + PRD.ToString() + " " + Loc.ToString() + " " + Tag.ToString());



                                tbTagOutItem.Rows.Add(tagout_index.ToString()
                                                    , tagout_no.ToString()
                                                    , itemTAG.Product_Id
                                                    , itemTAG.Product_Name
                                                    , itemTAG.ProductConversion_Id
                                                    , itemTAG.ProductConversion_Name
                                                    , itemTAG.Qty
                                                    , itemTAG.Ratio
                                                    , itemTAG.TotalQty
                                                    , itemTAG.TagOutRef_No1
                                                    );


                            }
                            //  ToteRemainCM3 = ToteRemainCM3 - (decimal)itemTAG.Volume;  // Cal Remian Vol


                        }

                    } // iLoop 


                    #region Loop Carton Qty By PlanGI


                    List<View_GoodsIssuecount_tag> listgood_I_no = db.View_GoodsIssuecount_tag.Where(c => c.GoodsIssue_No == data.GoodsIssue_No && c.Ref_Document_Index == item.RefDocIndex).OrderBy(c => c.Ref_Document_No).ToList();
                    var Document_no = "";
                    var branch = "";


                    olog.logging("maketagOut", " Carton  Loop Ref_Document_Index : " + item.RefDocIndex.ToString() );




                    count = count + TotalBoxcount;
                    foreach (View_GoodsIssuecount_tag itxem in listgood_I_no)
                    {

                        olog.logging("maketagOut", " Carton  Loop Ref_Document_No : " + itxem.Ref_Document_No.ToString());


                        if (itxem.Chute_No.Length == 2)
                        {
                            PlangGIChuteNo = itxem.Chute_No;
                        }
                        else if (itxem.Chute_No.Length == 1)
                        {
                            PlangGIChuteNo = "0" + itxem.Chute_No;
                        }
                        else
                        {
                            PlangGIChuteNo = "00";

                        }


                        var branch_code = itxem.Branch_Code == null ? "0000000" : itxem.Branch_Code;
                        var ref_document_no = itxem.Ref_Document_No == null ? "0000000000" : itxem.Ref_Document_No;
                        if (Document_no != ref_document_no)
                        {
                            count = 0 + TotalBoxcount;
                            Document_no = ref_document_no;
                        }
                        if (branch != branch_code)
                        {
                            count = 0 + TotalBoxcount;
                            branch = branch_code;
                        }
                        int count_item = (int)itxem.QTYCTN;
                        for (int i = 0; i < count_item; i++)
                        {
                            olog.logging("maketagOut", " Carton  Loop Ref_Document_No : " + itxem.Ref_Document_No.ToString() + " Product ID : " + itxem.Product_Id.ToString());

                            var runing = "";
                            count++;
                            Guid tag = Guid.NewGuid();
                            var tagnew = new wm_TagOut();

                            if (count < 10)
                            {
                                runing = "00" + count.ToString();
                            }
                            else if (count >= 100)
                            {
                                runing = count.ToString();
                            }
                            else if (count >= 10)
                            {
                                runing = "0" + count.ToString();
                            }
                            tagnew.TagOut_Index = tag;
                            tagnew.TagOut_No = "000" + ref_document_no + branch_code  + PlangGIChuteNo  + runing;
                            tagnew.TagOutRef_No1 = itxem.Branch_Code;
                            tagnew.TagOutRef_No2 = itxem.Chute_ID;
                            tagnew.TagOutRef_No3 = itxem.Branch_Name;
                            tagnew.TagOutRef_No4 = runing;
                            tagnew.TagOutRef_No5 = itxem.QTYCTN.GetValueOrDefault().ToString();
                            tagnew.TagOut_Status = 0;
                            tagnew.UDF_1 = itxem.Ref_Document_No;
                            tagnew.UDF_2 = itxem.TotalQty.ToString();
                            tagnew.UDF_3 = itxem.ShipTo_Index == null ? null : itxem.ShipTo_Index.Value.ToString();
                            tagnew.UDF_4 = itxem.Ref_Document_Index == null ? null : itxem.Ref_Document_Index.GetValueOrDefault().ToString();
                            tagnew.UDF_5 = itxem.Ref_Document_Index == null ? null : itxem.Ref_DocumentItem_Index.GetValueOrDefault().ToString();
                            tagnew.Zone_Index = null;
                            tagnew.Ref_Process_Index = Guid.Parse(itxem.Ref_Process_Index);
                            tagnew.Ref_Document_No = itxem.GoodsIssue_No;
                            tagnew.Ref_Document_Index = itxem.GoodsIssue_Index;
                            tagnew.Ref_DocumentItem_Index = itxem.GoodsIssueItemLocation_Index;
                            tagnew.Create_By = "";
                            tagnew.Create_Date = DateTime.Now;
                            tagnew.TagOutType = itxem.TagOutType;
                            tagnew.LocationType = itxem.LocationType;


                            db.WM_TagOut.Add(tagnew);

                            olog.logging("maketagOut", "Carton   db.WM_TagOut.Add(tagnew) : " + tagnew.TagOut_No.ToString()); 
                            //////////////////////
                            ///
                            Guid tagoutitem_index = Guid.NewGuid();
                            var tagout = new wm_TagOutItem();
                            tagout.TagOutItem_Index = tagoutitem_index;
                            tagout.TagOut_Index = tagnew.TagOut_Index;
                            tagout.TagOut_No = tagnew.TagOut_No;
                            tagout.GoodsIssue_Index = itxem.GoodsIssue_Index;
                            //tagout.GoodsIssueItem_Index = itxem.Product_id;
                            tagout.GoodsIssueItemLocation_Index = itxem.GoodsIssueItemLocation_Index;
                            // tagout.Carton_No = itxem.Product_id;
                            tagout.Product_Index = itxem.Product_Index;// new Guid(itxem.Product_Index) ;
                            tagout.Product_Id = itxem.Product_Id;
                            tagout.Product_Name = itxem.Product_Name;
                            //  tagout.Product_SecondName = itxem.Product_SecondName;
                            //  tagout.Product_ThirdName = itxem.Product_ThirdName;
                            tagout.Product_Lot = itxem.Product_Lot;
                            tagout.EXP_Date = itxem.EXP_Date;
                            tagout.ItemStatus_Index = itxem.ItemStatus_Index;
                            tagout.ItemStatus_Id = itxem.ItemStatus_Id;
                            tagout.ItemStatus_Name = itxem.ItemStatus_Name;
                            tagout.Qty = 1;//itxem.Product_id;
                            tagout.Ratio = itxem.SALE_Ratio; //itxem.Product_id;
                            tagout.TotalQty = 1 * itxem.SALE_Ratio; //itxem.Product_id;
                            tagout.ProductConversion_Index = itxem.ProductConversion_Index;
                            tagout.ProductConversion_Id = itxem.ProductConversion_Id;
                            tagout.ProductConversion_Name = itxem.ProductConversion_Name;
                            tagout.Weight = itxem.ProductConversion_Weight;
                            tagout.Volume = itxem.Vol_Per_Unit;
                            tagout.TagOutRef_No1 = "";
                            tagout.TagOut_Status = 0;
                            tagout.TagOutRef_No2 = "";
                            tagout.TagOutRef_No3 = itxem.Vol_Per_Unit.ToString();
                            tagout.TagOutRef_No4 = ref_document_no;
                            //tagout.Ref_Process_Index = itemvc.Product_id;
                            //tagout.Ref_Document_No = itemvc.Product_id;
                            //tagout.Ref_Document_Index = itemvc.Product_id;
                            //tagout.Ref_DocumentItem_Index = itemvc.Product_id;
                            tagout.Create_By = "";
                            tagout.Create_Date = DateTime.Now;

                            db.WM_TagOutItem.Add(tagout);

                         //   db.Entry(listgood_I_no).State = EntityState.Detached;

                            olog.logging("maketagOut", "Carton   db.WM_TagOutItem.Add(tagout) : " + tagnew.TagOut_No.ToString() );
                        }
                    }

                    #endregion

                 //   db.Entry(GrouplistGIL).State = EntityState.Detached;

                }// Loop Plan GI

                var GI = db.IM_GoodsIssue.FirstOrDefault(c => c.GoodsIssue_No == data.GoodsIssue_No);
                GI.TagOut_status = 1;


                db.Database.SetCommandTimeout(360);

                var transactionX = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    msglog = State + " S. db.SaveChanges()";
                    olog.logging("maketagOut", msglog);


                    db.SaveChanges();
                    transactionX.Commit();

                    msglog = State + " E. db.SaveChanges()";
                    olog.logging("maketagOut", msglog);
                    try
                    {
                        olog.logging("maketagOut", "s.sp_UpdateToteSize  : " + GI.GoodsIssue_Index.ToString ());

                        var pgi_index = new SqlParameter("@GoodsIssue_Index", GI.GoodsIssue_Index);
                        var resultTask = db.Database.ExecuteSqlCommand("EXEC sp_UpdateToteSize @GoodsIssue_Index", pgi_index);

                        olog.logging("maketagOut", "E.sp_UpdateToteSize  : " + GI.GoodsIssue_Index.ToString());

                        string cmd1 = "";


                        cmd1 += "       Update WMSDB_Outbound..wm_TagOutitem set                           ";
                        cmd1 += "            TagOut_No = '00B' + SUBSTRING(TagOut_No, 4, 22)                           ";
                        cmd1 += "        where TagOut_Index   in   (                                                      ";
                        cmd1 += "                                select    TagOut_Index                                                  ";
                        cmd1 += "                                from WMSDB_Outbound..wm_TagOutitem                               ";
                        cmd1 += "                                where TagOutRef_No1 like 'PB%'                         ";
                        cmd1 += "                                 and SUBSTRING(TagOut_No , 1,3) = '000'                   ";
                        cmd1 += "                                 and GoodsIssue_Index = '" + GI.GoodsIssue_Index + "'";
                        cmd1 += "                                )                                                           ";

                        string cmd2 = "";


                        cmd2 += "       Update WMSDB_Outbound..wm_TagOutitem set                           ";
                        cmd2 += "            TagOut_No = '00A' + SUBSTRING(TagOut_No, 4, 22)                           ";
                        cmd2 += "        where TagOut_Index   in   (                                                      ";
                        cmd2 += "                                select    TagOut_Index                                                  ";
                        cmd2 += "                                from WMSDB_Outbound..wm_TagOutitem                               ";
                        cmd2 += "                                where TagOutRef_No1 like 'PA%'                         ";
                        cmd2 += "                                 and SUBSTRING(TagOut_No , 1,3) = '000'                   ";
                        cmd2 += "                                 and GoodsIssue_Index = '" + GI.GoodsIssue_Index + "'";
                        cmd2 += "                                )    ";


                        string cmd3 = "";

                        cmd3 += "       Update WMSDB_Outbound..wm_TagOutitem set                           ";
                        cmd3 += "            TagOut_No = '00A' + SUBSTRING(TagOut_No, 4, 22)                           ";
                        cmd3 += "        where TagOut_Index   in   (                                                      ";
                        cmd3 += "                                select    TagOut_Index                                                  ";
                        cmd3 += "                                from WMSDB_Outbound..wm_TagOutitem                               ";
                        cmd3 += "                                where TagOutRef_No1 like 'VC%'                         ";
                        cmd3 += "                                 and SUBSTRING(TagOut_No , 1,3) = '000'                   ";
                        cmd3 += "                                 and GoodsIssue_Index = '" + GI.GoodsIssue_Index + "'";
                        cmd3 += "                                )    ";


                        string cmd4 = "";
                        cmd4 += "       Update WMSDB_Outbound..wm_TagOut set   ";
                        cmd4 += "           TagOut_No = (select  top 1 TagOut_No from  WMSDB_Outbound..wm_TagOutItem TAOI where TAOI.TagOut_Index = WMSDB_Outbound..wm_TagOut.TagOut_Index )   ";

                        cmd4 += "       where Ref_Document_Index = '" + GI.GoodsIssue_Index + "'";
                        cmd4 += "       and LocationType = 'CartonFlowRack'    ";

                        string cmd5 = "";
                        cmd5 += "       Update [WMSDB_Outbound]..im_GoodsIssue set ";
                        cmd5 += "            TagOut_status  = 1 ";

                        cmd5 += "       where GoodsIssue_Index = '" + GI.GoodsIssue_Index + "'";
                        cmd5 += "       and  TagOut_status  = 0   ";



                        olog.logging("maketagOut", "ExecuteSqlCommand R1 : " + cmd1 );
                        var r1 = db.Database.ExecuteSqlCommand(cmd1);
                        olog.logging("maketagOut", "ExecuteSqlCommand R2 : " + cmd2);
                        var r2 = db.Database.ExecuteSqlCommand(cmd2);
                        olog.logging("maketagOut", "ExecuteSqlCommand R3 : " + cmd3);
                        var r3 = db.Database.ExecuteSqlCommand(cmd3);
                        olog.logging("maketagOut", "ExecuteSqlCommand R4 : " + cmd4);
                        var r4 = db.Database.ExecuteSqlCommand(cmd4);
                        olog.logging("maketagOut", "ExecuteSqlCommand R5 : " + cmd5);
                        var r5 = db.Database.ExecuteSqlCommand(cmd5);


                        olog.logging("maketagOut", "gen sp_CreatePickingplan");
                        var pstrGoodsIssue_Index = new SqlParameter("@GoodsIssue_Index", GI.GoodsIssue_Index);
                        var resultPickingplan = db.Database.ExecuteSqlCommand("EXEC sp_CreatePickingplan @GoodsIssue_Index", pstrGoodsIssue_Index);




                    }
                    catch (Exception exxx)
                    {

                        msglog = State + " exxx " + exxx.Message.ToString();
                        olog.logging("maketagOut", msglog);
                        //throw exxx;
                        olog.logging("maketagOut", "exxx inner - " + exxx.InnerException.Message.ToString());
                    }
                }
                catch (Exception exy)
                {


                    msglog = State + " exy Rollback " + exy.Message.ToString();
                    olog.logging("maketagOut", msglog);
                    transactionX.Rollback();

                    olog.logging("maketagOut", "exy inner - " + exy.InnerException.Message.ToString());

                    throw exy;
                }



                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        public bool maketagOut_V4(findtagViewModelItem data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            olog.logging("maketagOut_V4", State);

            try
            {


                var listGIL = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssue_No == data.GoodsIssue_No).ToList();


                var GrouplistGIL = listGIL.GroupBy(c => new { c.GoodsIssue_Index, c.Ref_Document_Index, c.Ref_Document_No })
                                   .Select(group => new
                                   {
                                       GIIndex = group.Key.GoodsIssue_Index,
                                       RefDocIndex = group.Key.Ref_Document_Index,
                                       RefDocNo = group.Key.Ref_Document_No
                                   }).ToList();


                //decimal ToteCM3XL = 61280;  //76.608L
                //decimal ToteCM3S = 28800;  //36L

                decimal ToteCM3XL = 46000;  // 45,964.8  60%
                decimal ToteCM3S = 21600;  // 21600  60%
                string ToteName = "XL";  //76.608L
                bool flagSmallTote = false;

                decimal ToteRemainCM3 = 0;  //36L
                int TotalBoxcount = 0;
                string zero = "00000";
                Guid tagout_index;
                string tagout_no = "";
                Boolean NewTote = true;
                // Loop By Plan GI No
                var count = 0;
                string oldPlanGI_No = "";
                string checkPlangGI = "";
                string PlangGIChuteNo = "";
                foreach (var item in GrouplistGIL)
                {


                    zero = "000";

                    count = 500;
                    TotalBoxcount = 0;
                    ToteRemainCM3 = 0;
                    var countTotalBox = 0;

                    #region Loop Carton Qty By PlanGI


                    List<View_GoodsIssuecount_tag> listgood_I_no = db.View_GoodsIssuecount_tag.Where(c => c.GoodsIssue_No == data.GoodsIssue_No && c.Ref_Document_Index == item.RefDocIndex).OrderBy(c => c.Ref_Document_No).ToList();
                    var Document_no = "";
                    var branch = "";



                    count = count + TotalBoxcount;
                    foreach (View_GoodsIssuecount_tag itxem in listgood_I_no)
                    {


                        if (itxem.Chute_No.Length == 2)
                        {
                            PlangGIChuteNo = itxem.Chute_No;
                        }
                        else if (itxem.Chute_No.Length == 1)
                        {
                            PlangGIChuteNo = "0" + itxem.Chute_No;
                        }
                        else
                        {
                            PlangGIChuteNo = "00";

                        }


                        var branch_code = itxem.Branch_Code == null ? "0000000" : itxem.Branch_Code;
                        var ref_document_no = itxem.Ref_Document_No == null ? "0000000000" : itxem.Ref_Document_No;

                        if (Document_no != ref_document_no)
                        {
                            count = 500 + TotalBoxcount;
                            Document_no = ref_document_no;
                        }
                        if (branch != branch_code)
                        {
                            count = 500 + TotalBoxcount;
                            branch = branch_code;
                        }

                        int count_item = (int)itxem.QTYCTN;
                        for (int i = 0; i < count_item; i++)
                        {
                            var runing = "";
                            count++;
                            Guid tag = Guid.NewGuid();
                            var tagnew = new wm_TagOut();

                            if (count < 10)
                            {
                                runing = "00" + count.ToString();
                            }
                            else if (count >= 100)
                            {
                                runing = count.ToString();
                            }
                            else if (count >= 10)
                            {
                                runing = "0" + count.ToString();
                            }
                            tagnew.TagOut_Index = tag;
                            tagnew.TagOut_No = "000" + ref_document_no + branch_code + PlangGIChuteNo + runing;
                            tagnew.TagOutRef_No1 = itxem.Branch_Code;
                            tagnew.TagOutRef_No2 = itxem.Chute_ID;
                            tagnew.TagOutRef_No3 = itxem.Branch_Name;
                            tagnew.TagOutRef_No4 = runing;
                            tagnew.TagOutRef_No5 = itxem.QTYCTN.GetValueOrDefault().ToString();
                            tagnew.TagOut_Status = 0;
                            tagnew.UDF_1 = itxem.Ref_Document_No;
                            tagnew.UDF_2 = itxem.TotalQty.ToString();
                            tagnew.UDF_3 = itxem.ShipTo_Index == null ? null : itxem.ShipTo_Index.Value.ToString();
                            tagnew.UDF_4 = itxem.Ref_Document_Index == null ? null : itxem.Ref_Document_Index.GetValueOrDefault().ToString();
                            tagnew.UDF_5 = itxem.Ref_Document_Index == null ? null : itxem.Ref_DocumentItem_Index.GetValueOrDefault().ToString();
                            tagnew.Zone_Index = null;
                            tagnew.Ref_Process_Index = Guid.Parse(itxem.Ref_Process_Index);
                            tagnew.Ref_Document_No = itxem.GoodsIssue_No;
                            tagnew.Ref_Document_Index = itxem.GoodsIssue_Index;
                            tagnew.Ref_DocumentItem_Index = itxem.GoodsIssueItemLocation_Index;
                            tagnew.Create_By = "FIX";
                            tagnew.Create_Date = DateTime.Now;
                            tagnew.TagOutType = itxem.TagOutType;
                            tagnew.LocationType = itxem.LocationType;


                            db.WM_TagOut.Add(tagnew);

                            //////////////////////
                            ///
                            Guid tagoutitem_index = Guid.NewGuid();
                            var tagout = new wm_TagOutItem();
                            tagout.TagOutItem_Index = tagoutitem_index;
                            tagout.TagOut_Index = tagnew.TagOut_Index;
                            tagout.TagOut_No = tagnew.TagOut_No;
                            tagout.GoodsIssue_Index = itxem.GoodsIssue_Index;
                            //tagout.GoodsIssueItem_Index = itxem.Product_id;
                            tagout.GoodsIssueItemLocation_Index = itxem.GoodsIssueItemLocation_Index;
                            // tagout.Carton_No = itxem.Product_id;
                            tagout.Product_Index = itxem.Product_Index;// new Guid(itxem.Product_Index) ;
                            tagout.Product_Id = itxem.Product_Id;
                            tagout.Product_Name = itxem.Product_Name;
                            //  tagout.Product_SecondName = itxem.Product_SecondName;
                            //  tagout.Product_ThirdName = itxem.Product_ThirdName;
                            tagout.Product_Lot = itxem.Product_Lot;
                            tagout.EXP_Date = itxem.EXP_Date;
                            tagout.ItemStatus_Index = itxem.ItemStatus_Index;
                            tagout.ItemStatus_Id = itxem.ItemStatus_Id;
                            tagout.ItemStatus_Name = itxem.ItemStatus_Name;
                            tagout.Qty = 1;//itxem.Product_id;
                            tagout.Ratio = itxem.SALE_Ratio; //itxem.Product_id;
                            tagout.TotalQty = 1 * itxem.SALE_Ratio; //itxem.Product_id;
                            tagout.ProductConversion_Index = itxem.ProductConversion_Index;
                            tagout.ProductConversion_Id = itxem.ProductConversion_Id;
                            tagout.ProductConversion_Name = itxem.ProductConversion_Name;
                            tagout.Weight = itxem.ProductConversion_Weight;
                            tagout.Volume = itxem.Vol_Per_Unit;
                            tagout.TagOutRef_No1 = "";
                            tagout.TagOut_Status = 0;
                            tagout.TagOutRef_No2 = "";
                            tagout.TagOutRef_No3 = itxem.Vol_Per_Unit.ToString();
                            tagout.TagOutRef_No4 = ref_document_no;
                            //tagout.Ref_Process_Index = itemvc.Product_id;
                            //tagout.Ref_Document_No = itemvc.Product_id;
                            //tagout.Ref_Document_Index = itemvc.Product_id;
                            //tagout.Ref_DocumentItem_Index = itemvc.Product_id;
                            tagout.Create_By = "FIX";
                            tagout.Create_Date = DateTime.Now;

                            db.WM_TagOutItem.Add(tagout);
                        }
                    }

                    #endregion

                }// Loop Plan GI

                var GI = db.IM_GoodsIssue.FirstOrDefault(c => c.GoodsIssue_No == data.GoodsIssue_No);
                GI.TagOut_status = 1;


                db.Database.SetCommandTimeout(360);

                var transactionX = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionX.Commit();


                }
                catch (Exception exy)
                {
                    transactionX.Rollback();
                    throw exy;
                }



                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #region Task Make Tag
        public bool maketagOutTask()
        {
            logtxt log = new logtxt();
            try
            {
                log.DataLogLines("Task_GenTagout", "Task_GenTagout", "--------------------------------------------------------------------------------");
                log.DataLogLines("Task_GenTagout", "Task_GenTagout", "Start :" + DateTime.Now);
                var Gi_tag = db.IM_GoodsIssue.Where(c => c.GI_status == 1 && c.TaskGI_status == 1).ToList();
                foreach (im_GoodsIssue item in Gi_tag)
                {
                    findtagViewModelItem Gi_no = new findtagViewModelItem();
                    Gi_no.GoodsIssue_No = item.GoodsIssue_No;
                    var tagout = maketagOut(Gi_no);

                    if (tagout)
                    {
                        item.TagOut_status = 1;
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
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
