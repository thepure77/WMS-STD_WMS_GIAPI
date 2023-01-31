using GIDataAccess.Models;
using MasterDataBusiness.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class GoodsIssueHeaderViewModel
    {
        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get;set;}
        public string goodsIssue_Due_Date { get;set;}
        public string goodsIssue_Time { get;set;}
        public string owner_Index { get;set;}
        public string owner_Name { get;set;}
        public string owner_Id { get;set;}
        public string documentRef_No1 { get;set;}
        public string document_Remark { get;set;}
        public string documentType_Index { get;set;}
        public string documentType_Id { get;set;}
        public string documentType_Name { get;set;}
        public string warehouse_Index { get; set; }
        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }
        public string document_Date { get; set; }
        public string create_by { get; set; }
        public int? documentPriority_Status { get; set; }
    }
    public class actionResultGoodsIssueHeaderViewModel : Result
    {
        public GoodsIssueHeaderViewModel items { get; set; }
    }
    public class ResultGoodsIssueHeaderViewModel
    {
        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }
        public string goodsIssue_Due_Date { get; set; }
        public string goodsIssue_Time { get; set; }
        public string owner_Index { get; set; }
        public string owner_Name { get; set; }
        public string owner_Id { get; set; }
        public string documentRef_No1 { get; set; }
        public string document_Remark { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string create_by { get; set; }
    }
}
