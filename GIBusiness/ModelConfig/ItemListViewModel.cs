using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MasterBusiness.GoodsIssue
{

    public partial class ItemListViewModel
    {
        public Guid? index { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public int chk { get; set; }


    }
}
