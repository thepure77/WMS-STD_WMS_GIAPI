using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class __overall_route
    {
        [Key]
        public int RowIndex { get; set; }

        [StringLength(1000)]
        public string Route_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DocDate { get; set; }

        [StringLength(1000)]
        public string orderQty { get; set; }
    }
}
