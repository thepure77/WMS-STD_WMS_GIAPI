using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class __overall_status
    {
        [Key]
        public int RowIndex { get; set; }

        [StringLength(1000)]
        public string StatusName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DocDate { get; set; }

        [StringLength(1000)]
        public string Qty { get; set; }
    }
}
