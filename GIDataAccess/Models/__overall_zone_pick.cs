using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class __overall_zone_pick
    {
        [Key]
        public int RowIndex { get; set; }

        [StringLength(1000)]
        public string Zone_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DocDate { get; set; }

        [StringLength(1000)]
        public string pickQty { get; set; }

        [StringLength(1000)]
        public string fulfilled { get; set; }

        [StringLength(1000)]
        public string unFulfilled { get; set; }

        [StringLength(1000)]
        public string Remain { get; set; }
    }
}
