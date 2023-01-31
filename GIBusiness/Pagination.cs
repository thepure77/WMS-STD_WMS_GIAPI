using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness
{
    public class Pagination
    {
        public int CurrentPage { get; set; }

        public int PerPage { get; set; }

        public int TotalRow { get; set; }

        public string key { get; set; }

        public bool AdvanceSearch { get; set; }
    }
}
