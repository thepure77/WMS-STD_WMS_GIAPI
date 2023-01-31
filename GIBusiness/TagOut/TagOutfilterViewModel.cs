using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.TagOut
{
    public class TagOutfilterViewModel
    {
        public TagOutfilterViewModel()
        {
            listTagOutViewModel = new List<listTagOutViewModel>();

        }

        public Guid? tagOut_Index { get; set; }


        public string tagOut_No { get; set; }
        public string goodsIssue_No { get; set; }
        public string plangi_no { get; set; }
        //public string userAssign { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string create_Time { get; set; }
        public string assign_By { get; set; }
        public string value { get; set; }
        public int qtylabel { get; set; }

        public List<listTagOutViewModel> listTagOutViewModel { get; set; }

        public class actionResultTask
        {
            public string msg { get; set; }
        }
    }
}
