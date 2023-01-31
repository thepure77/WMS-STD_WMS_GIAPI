using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceBusiness
{
    public class GIRequestViewModel
    {
        public GIRequestViewModel()
        {
            Detail = new List<GIRequestDetail>();
        }
        public string PstngDate { get; set; }
        public string DocDate { get; set; }
        public string RefDocNo { get; set; }
        //public string GrGiSlipNo { get; set; }
        public string GiNo { get; set; }
        public string HeaderTxt { get; set; }
        public string GmCode { get; set; }
        public List<GIRequestDetail> Detail { get; set; }
    }

    public class GIRequestDetail
    {
        public string Material { get; set; }
        public string Plant { get; set; }
        public string StgeLoc { get; set; }
        public string Batch { get; set; }
        public string MoveType { get; set; }
        public decimal EntryQnt { get; set; }
        public string EntryUom { get; set; }
        public string ItemText { get; set; }
        public string GrRcpt { get; set; }
        public string Costcenter { get; set; }
        public string StckType { get; set; }
        public string Orderid { get; set; }
        public string OrderItno { get; set; }
        public string AssetNo { get; set; }
        public string SubNumber { get; set; }
        public string WbsElem { get; set; }
        public string GlAccount { get; set; }
        public string ReservNo { get; set; }
        public string ResItem { get; set; }
        public string MoveMat { get; set; }
        public string MovePlant { get; set; }
        public string MoveStloc { get; set; }
        public string MoveBatch { get; set; }
    }
}
