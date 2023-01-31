using GIBusiness.PlanGoodIssue;
using GIDataAccess.Models;
using MasterDataBusiness.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class SendToSapViewModel
    {
        public string goodsIssue_Index { get; set; }
        public string goodsIssue_No { get; set; }
        public string goodsIssue_Date { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string owner_Index { get;set;}
        public string owner_Id { get;set;}
        public string owner_Name { get;set;}
        public string create_By { get; set; }
        public bool selected { get; set; }
    }
    public class ListSendToSapViewModel
    {
        public List<SendToSapViewModel> items { get; set; }
    }
}
