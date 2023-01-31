using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{

    public partial class CheckB4Wave
    {
        [Key]
        public long? RowIndex { get; set; }

        public string msgCheck { get; set; }

        public int CountRows { get; set; }
    }
}
