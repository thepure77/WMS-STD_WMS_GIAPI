using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{

    public partial class SearchDetailModel : Pagination
    {
        public SearchDetailModel()
        {
            sort = new List<sortViewModel>();

            status = new List<statusViewModel>();

        }

        [Key]
        public Guid? goodsIssue_Index { get; set; }

        public string goodsIssue_No { get; set; }


        public string goodsIssue_Date { get; set; }
        public string goodsIssue_Date_To { get; set; }

        public string planGoodsIssue_Date { get; set; }

        public string planGoodsIssue_Due_Date { get; set; }
        public string planGoodsIssue_Due_Date_To { get; set; }


        public Guid? owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }

        public int? document_Status { get; set; }


        public string planGoodsIssue_No { get; set; }


        public Guid? documentType_Index { get; set; }


        public string documentType_Id { get; set; }


        public string documentType_Name { get; set; }

        public decimal? weight { get; set; }

        public decimal? qty { get; set; }

        public string create_By { get; set; }
        public string document_Remark { get; set; }
        public string update_By { get; set; }
        public string cancel_By { get; set; }

        public string processStatus_Name { get; set; }
        public bool advanceSearch { get; set; }

        public string round_Name { get; set; }

        public string route_Name { get; set; }
        public string documentRef_No1 { get; set; }
        public string documentRef_No2 { get; set; }
        public int? GI_status { get; set; }
        public int? WCS_status { get; set; }

        public List<sortViewModel> sort { get; set; }
        public List<statusViewModel> status { get; set; }
        public class actionResultViewModel
        {
            public IList<SearchDetailModel> itemsGI { get; set; }
            public Pagination pagination { get; set; }
        }

        public class sortViewModel
        {
            public string value { get; set; }
            public string display { get; set; }
            public int seq { get; set; }
        }

        public class statusViewModel
        {
            public int? value { get; set; }
            public string display { get; set; }
            public int seq { get; set; }
        }

        public class SortModel
        {
            public string ColId { get; set; }
            public string Sort { get; set; }

            public string PairAsSqlExpression
            {
                get
                {
                    return $"{ColId} {Sort}";
                }
            }
        }

        public class statusModel
        {
            public string Name { get; set; }
        }

        public bool isConfirmStock { get; set; }
    }
}
