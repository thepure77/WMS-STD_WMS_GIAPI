using System;
using System.Collections.Generic;
using System.Text;

namespace GIBusiness.OverallPerformanceDashboard
{
    public class OrderByRoundViewModel
    {
        public string Round { get; set; }

        public int Order { get; set; }

        public int Done { get; set; }

        public int Canceled { get; set; }

        public int Remain { get; set; }
    }
}
