using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.TagOut
{
    public class TagOutitemViewModel
    {
        public TagOutitemViewModel()
        {
            listTagOutViewModel = new List<listTagOutViewModel>();

        }

        public Guid? tagOut_Index { get; set; }
        public Guid? tagOutItem_Index { get; set; }


        public string tagOut_No { get; set; }
        public string goodsIssue_No { get; set; }
        public string plangi_no { get; set; }
        public string product_barcode { get; set; }
        //public string userAssign { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string create_Time { get; set; }
        public string assign_By { get; set; }
        public string value { get; set; }
        public int qtylabel { get; set; }
        public string resultMsg { get; set; }
        public bool resultCheckSorter { get; set; }



        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public decimal? qty { get; set; }
        public string status { get; set; }
        public string productConversion_Name { get; set; }
        public string size { get; set; }
        public decimal? total_qty { get; set; }

        public ResponseMessage message { get; set; }

        public List<listTagOutViewModel> listTagOutViewModel { get; set; }

        public class actionResultTask
        {
            public string msg { get; set; }
        }

        public class ResponseMessage
        {
            public string description { get; set; }
            public List<PTLPickingModel> models { get; set; }
        }

        public class PTLPickingModel
        {
            public string productId { get; set; }
            public string productName { get; set; }
            public decimal quantity { get; set; }
            public string productConversionName { get; set; }
        }
    }
}
