using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{

    public partial class RoundWave
    {
        [Key]
        public long RowIndex { get; set; }
        public DateTime? Appointment_Date { get; set; }

        public string Appointment_Time { get; set; }

        public string TM_No { get; set; }

        public string Dock_Name { get; set; }

        public string Appointment_Id { get; set; }
        public string VehicleType_Name { get; set; }

        public int CountOrder { get; set; }
    }
}
