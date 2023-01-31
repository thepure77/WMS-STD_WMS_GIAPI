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
    public class GoodsIssueViewModel
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

        public string warehouse_Index { get; set; }
        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }
        public string document_Date { get; set; }
        public int? documentPriority_Status { get; set; }

        public List<GoodIssueViewModelItem> listGoodsIssueItemViewModel { get; set; }

        public bool isDemand_Payment { get; set; }
        public string demand_Payment_Name { get; set; }
        public string demand_Payment_pos_Name { get; set; }
        public string demand_first_Name { get; set; }
        public string demand_last_Name { get; set; }
        public string demand_user_Index { get; set; }
        public string demand_user_Id { get; set; }
        public string demand_user_Name { get; set; }
        public string demand_position_Code { get; set; }

        public bool isPayment { get; set; }
        public string payment_Name { get; set; }
        public string payment_pos_Name { get; set; }
        public string payment_first_Name { get; set; }
        public string payment_last_Name { get; set; }
        public string payment_user_Index { get; set; }
        public string payment_user_Id { get; set; }
        public string payment_user_Name { get; set; }
        public string payment_position_Code { get; set; }

        public bool isRecipent { get; set; }
        public string recipent_Name { get; set; }
        public string recipent_pos_Name { get; set; }
        public string recipent_first_Name { get; set; }
        public string recipent_last_Name { get; set; }
        public string recipent_user_Index { get; set; }
        public string recipent_user_Id { get; set; }
        public string recipent_user_Name { get; set; }
        public string recipent_position_Code { get; set; }

        public bool isRecorder { get; set; }
        public string recorder_Name { get; set; }
        public string recorder_pos_Name { get; set; }
        public string recorder_first_Name { get; set; }
        public string recorder_last_Name { get; set; }
        public string recorder_user_Index { get; set; }
        public string recorder_user_Id { get; set; }
        public string recorder_user_Name { get; set; }
        public string recorder_position_Code { get; set; }


        public string user { get; set; }
        public bool isUpdate { get; set; }
    }

    public class ListGoodsIssueViewModel
    {
        public List<GoodsIssueViewModel> items { get; set; }
        public string userName { get; set; }
    }
}
