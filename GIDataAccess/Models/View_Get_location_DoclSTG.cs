using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_Get_location_DoclSTG
    {
        [Key]

        public long? RowIndex { get; set; }

        public Guid STGDock_Index { get; set; }
        public Guid? Dock_Index { get; set; }
        public Guid? DockType_Index { get; set; }
        public Guid? Location_Index { get; set; }
        public Guid? LocationType_Index { get; set; }
        public Guid? Location_Labeling_Index { get; set; }
        public Guid? LocationType_Labeling_Index { get; set; }
        public string Dock_Id { get; set; }
        public string Dock_Name { get; set; }
        public string Location_Id { get; set; }
        public string Location_Name { get; set; }
        public string Location_Labeling_Id { get; set; }
        public string Location_Labeling_Name { get; set; }

    
    }
}
