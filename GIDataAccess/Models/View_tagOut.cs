using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_tagOut
    {
        [Key]

        public long? RowIndex { get; set; }

        public Guid TagOut_Index { get; set; }


        public string UDF_1 { get; set; }
        public string locationtype { get; set; }

        public string moblie { get; set; }

  
        public string TagOut_No { get; set; }

 
        public string TagOutRef_No4 { get; set; }

        public int? total_Box { get; set; }

  
        public string Country_Name { get; set; }


        public string Province_Name { get; set; }

  
        public string Ref_No1 { get; set; }


        public string ShipTo_Name { get; set; }

        public string Ref_Document_No { get; set; }
        public string Drop_Seq { get; set; }
        public string Product_Id { get; set; }
        public string Order_seq { get; set; }
        public string Loc { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public string barcodeTracking { get; set; }
    }
}
