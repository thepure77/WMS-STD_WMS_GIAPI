using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_Taskitem_MoveOnGround
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid task_Index { get; set; }

        public string task_No { get; set; }

        public Guid? tag_Index { get; set; }

        public string tag_No { get; set; }

        public Guid? product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string product_SecondName { get; set; }

        public string product_ThirdName { get; set; }

        public string product_Lot { get; set; }

        public Guid? itemStatus_Index { get; set; }

        public string itemStatus_Id { get; set; }

        public string itemStatus_Name { get; set; }

        public Guid? location_Index { get; set; }

        public string location_Id { get; set; }

        public string location_Name { get; set; }

        public decimal? ratio { get; set; }

        public Guid? productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public string ref_Document_No { get; set; }

        public Guid? ref_Document_Index { get; set; }

        public decimal? Qty { get; set; }

        public decimal? TotalQty { get; set; }

        public string imageUri { get; set; }
    }
}
