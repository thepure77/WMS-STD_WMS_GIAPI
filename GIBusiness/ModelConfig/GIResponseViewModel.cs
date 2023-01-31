using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceBusiness
{
    public class GIResponseViewModel
    {
        public string status { get; set; }
        //public string message { get; set; }
        public GIMessage message { get; set; }
    }

    public class GIMessage
    {
        public string eFiDocumentField { get; set; }
        public string eMaterailDocField { get; set; }
        public string eMessageField { get; set; }
    }
}
