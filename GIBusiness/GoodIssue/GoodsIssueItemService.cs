using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using GIBusiness.PlanGoodIssue;
using Business.Library;
using Comone.Utils;
using MasterDataBusiness.ViewModels;

namespace GIBusiness.GoodIssue
{
    public class GoodsIssueItemService
    {
        private GIDbContext db;

        public GoodsIssueItemService()
        {
            db = new GIDbContext();
        }
        public GoodsIssueItemService(GIDbContext db)
        {
            this.db = db;
        }
        public List<GoodIssueViewModelItem> find(Guid id)
        {
            try
            {
                var result = new List<GoodIssueViewModelItem>();

                var query = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssue_Index == id && c.Document_Status != -1).OrderBy(o => o.Create_Date).ToList();

                foreach (var q in query)
                {
                    var productRefNo2 = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), new { product_Index = q.Product_Index }.sJson());
                    var model = new { binbalance_Index = q.BinBalance_Index };
                    var dataBinbalance = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), model.sJson());
                    var item = new GoodIssueViewModelItem();
                    item.goodsIssueItemLocation_Index = q?.GoodsIssueItemLocation_Index.ToString();
                    item.goodsIssue_Index = q?.GoodsIssue_Index.ToString();
                    item.lineNum = q.LineNum;
                    item.tagItem_Index = q?.TagItem_Index.ToString();
                    item.tag_Index = q?.Tag_Index.ToString();
                    item.tag_No = q.Tag_No;
                    item.product_Index = q?.Product_Index.ToString();
                    item.product_Id = q.Product_Id;
                    item.product_Name = q.Product_Name;
                    item.product_SecondName = q.Product_SecondName;
                    item.product_ThirdName = q.Product_ThirdName;
                    item.product_Lot = q.Product_Lot;
                    item.itemStatus_Index = q?.ItemStatus_Index.ToString();
                    item.itemStatus_Id = q.ItemStatus_Id;
                    item.itemStatus_Name = q.ItemStatus_Name;
                    item.location_Index = q?.Location_Index.ToString();
                    item.location_Id = q.Location_Id;
                    item.location_Name = q.Location_Name;
                    item.qtyPlan = q.QtyPlan;
                    item.qty = q.Qty;
                    item.ratio = q.Ratio;
                    item.totalQty = q.TotalQty;
                    item.productConversion_Index = q?.ProductConversion_Index.ToString();
                    item.productConversion_Id = q.ProductConversion_Id;
                    item.productConversion_Name = q.ProductConversion_Name;
                    item.productConversion_Base = productRefNo2[0].productConversion_Name;
                    item.mfg_Date = q?.MFG_Date.ToString();
                    item.exp_Date = q?.EXP_Date.ToString();
                    item.unitWeight = q.UnitWeight;
                    item.weight = q.Weight;
                    item.unitWidth = q.UnitWidth;
                    item.unitLength = q.UnitLength;
                    item.unitHeight = q.UnitHeight;
                    item.unitVolume = q.UnitVolume;
                    item.volume = q.Volume;
                    item.unitPrice = q.UnitPrice;
                    item.price = q.Price;
                    item.documentRef_No1 = q.DocumentRef_No1;
                    item.documentRef_No2 = q.DocumentRef_No2;
                    item.documentRef_No3 = q.DocumentRef_No3;
                    item.documentRef_No4 = q.DocumentRef_No4;
                    item.documentRef_No5 = q.DocumentRef_No5;
                    item.document_Status = q.Document_Status;
                    item.udf_1 = q.UDF_1;
                    item.udf_2 = q.UDF_2;
                    item.udf_3 = q.UDF_3;
                    item.udf_4 = q.UDF_4;
                    item.udf_5 = q.UDF_5;
                    item.ref_Process_Index = q?.Ref_Process_Index.ToString();
                    item.ref_Document_No = q.Ref_Document_No;
                    item.ref_Document_LineNum = q.Ref_Document_LineNum;
                    item.ref_Document_Index = q?.Ref_Document_Index.ToString();
                    item.ref_DocumentItem_Index = q?.Ref_DocumentItem_Index.ToString();
                    item.goodsReceiveItem_Index = q?.GoodsReceiveItem_Index.ToString();
                    item.create_By = q.Create_By;
                    item.create_Date = q?.Create_Date.ToString();
                    item.update_By = q.Update_By;
                    item.update_Date = q?.Update_Date.ToString();
                    item.cancel_By = q.Cancel_By;
                    item.cancel_Date = q?.Cancel_Date.ToString();
                    item.picking_Status = q.Picking_Status;
                    item.picking_By = q.Picking_By;
                    item.picking_Date = q?.Picking_Date.ToString();
                    item.picking_Ref1 = q.Picking_Ref1;
                    item.picking_Ref2 = q.Picking_Ref2;
                    item.picking_Qty = q.Picking_Qty;
                    item.picking_Ratio = q.Picking_Ratio;
                    item.picking_TotalQty = q.Picking_TotalQty;
                    item.picking_ProductConversion_Index = q?.Picking_ProductConversion_Index.ToString();
                    item.mashall_Status = q.Mashall_Status;
                    item.mashall_Qty = q.Mashall_Qty;
                    item.cancel_Status = q.Cancel_Status;
                    item.goodsIssue_No = q.GoodsIssue_No;
                    item.goodsReceive_No = dataBinbalance?.goodsReceive_No;
                    item.goodsReceive_date = dataBinbalance?.goodsReceive_Date.toString();
                    item.goodsReceive_Index = dataBinbalance?.goodsReceive_Index.ToString();
                    item.goodsReceiveItem_Index = dataBinbalance?.goodsReceiveItem_Index.ToString();
                    item.binBalance_Index = q?.BinBalance_Index.ToString();
                    item.warehouse_Name_To = !string.IsNullOrEmpty(q?.Ref_Document_Index?.ToString()) ? db.IM_PlanGoodsIssue.Find(q.Ref_Document_Index)?.Warehouse_Name_To : "";
                    item.documentItem_Remark = !string.IsNullOrEmpty(q?.Ref_DocumentItem_Index?.ToString()) ? db.IM_PlanGoodsIssue.Find(q.Ref_Document_Index)?.Document_Remark : "";
                    item.create_date_balance = dataBinbalance?.create_Date.toString();

                    item.invoice_No = q.Invoice_No;
                    item.declaration_No = q.Declaration_No;
                    item.invoice_No_Out = q.Invoice_No_Out;
                    item.declaration_No_Out = q.Declaration_No_Out;
                    item.hs_Code = q.HS_Code;
                    item.conutry_of_Origin = q.Conutry_of_Origin;
                    item.tax1 = q.Tax1;
                    item.tax1_Currency_Index = q.Tax1_Currency_Index;
                    item.tax1_Currency_Id = q.Tax1_Currency_Id;
                    item.tax1_Currency_Name = q.Tax1_Currency_Name;
                    item.tax2 = q.Tax2;
                    item.tax2_Currency_Index = q.Tax2_Currency_Index;
                    item.tax2_Currency_Id = q.Tax2_Currency_Id;
                    item.tax2_Currency_Name = q.Tax2_Currency_Name;
                    item.tax3 = q.Tax3;
                    item.tax3_Currency_Index = q.Tax3_Currency_Index;
                    item.tax3_Currency_Id = q.Tax3_Currency_Id;
                    item.tax3_Currency_Name = q.Tax3_Currency_Name;
                    item.tax4 = q.Tax4;
                    item.tax4_Currency_Index = q.Tax4_Currency_Index;
                    item.tax4_Currency_Id = q.Tax4_Currency_Id;
                    item.tax4_Currency_Name = q.Tax4_Currency_Name;
                    item.tax5 = q.Tax5;
                    item.tax5_Currency_Index = q.Tax5_Currency_Index;
                    item.tax5_Currency_Id = q.Tax5_Currency_Id;
                    item.tax5_Currency_Name = q.Tax5_Currency_Name;

                    item.product_Id_RefNo2 = productRefNo2?.FirstOrDefault()?.Ref_No2;
                    item.ERP_location = q.ERP_Location;

                    result.Add(item);
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool checkPGIHasGI(GoodIssueViewModelItem model)
        {
            try
            {
                bool result = false;
                if (!string.IsNullOrEmpty(model.ref_Document_Index))
                {

                    var query = db.IM_GoodsIssueItemLocation.Where(c => c.Ref_Document_Index == new Guid(model.ref_Document_Index) && c.Document_Status != 4 && c.Document_Status != -1).ToList();

                    if (query.Count > 0)
                    {
                        result = true;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
