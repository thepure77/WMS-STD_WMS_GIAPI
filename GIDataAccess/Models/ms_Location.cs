using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{


    public partial class ms_Location
    {
        [Key]
        public Guid? Location_Index { get; set; }

        public Guid? Warehouse_Index { get; set; }

        public Guid? Room_Index { get; set; }

        public Guid? LocationType_Index { get; set; }

        [StringLength(50)]
        public string Location_Id { get; set; }

        [StringLength(200)]
        public string Location_Name { get; set; }

        public Guid? LocationAisle_Index { get; set; }

        public int? Location_Bay { get; set; }

        public int? Location_Depth { get; set; }

        public int? Location_Level { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Max_Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Max_Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Max_Volume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Max_Pallet { get; set; }

        public int? PutAway_Seq { get; set; }

        public int? Picking_Seq { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Cancel_Date { get; set; }

        public int? BlockPut { get; set; }
        public int? BlockPick { get; set; }

        public string Location_Bay_Desc { get; set; }
        public string Location_Level_Desc { get; set; }
        public int? Location_Position { get; set; }
        public string Location_Position_Desc { get; set; }
        public string Location_Prefix_Desc { get; set; }

        public string Ref_No1 { get; set; }
        public string Ref_No2 { get; set; }
        public string Ref_No3 { get; set; }
        public string Ref_No4 { get; set; }
        public string Ref_No5 { get; set; }
        public string Document_Remark { get; set; }
    }
}
