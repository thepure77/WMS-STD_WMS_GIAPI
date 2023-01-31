using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.TagOut
{
    public partial class listTagOutViewModel
    {
        public Guid? tagOut_Index { get; set; }

        public string update_By { get; set; }

        public Guid? goodsIssue_Index { get; set; }

        public string document_Status { get; set; }
        public string goodsIssue_No { get; set; }
        //public string userAssign { get; set; }
    }
}
