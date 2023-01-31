using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GIDataAccess.Models
{
    public class View_Checkstatus_truckload_order
    {
        [Key]
        public string TruckLoad_No { get; set; }
        public int Document_Status { get; set; }
        public string Round_Name { get; set; }
    }
}
