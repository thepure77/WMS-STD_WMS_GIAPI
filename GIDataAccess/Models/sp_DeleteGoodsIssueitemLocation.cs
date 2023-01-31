using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class sp_DeleteGoodsIssueitemLocation
    {
        [Key]
        public string GoodsIssueItemLocation_Index { get; set; }
    }
}
