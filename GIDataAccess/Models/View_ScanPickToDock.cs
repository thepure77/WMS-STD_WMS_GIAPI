using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_ScanPickToDock
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid TruckLoad_Index { get; set; }
        public string TruckLoad_No { get; set; }

        public string Ref_Document_No { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public string Dock_Name { get; set; }
        public int? PickingToDock_Status { get; set; }
       
        
    }
}
