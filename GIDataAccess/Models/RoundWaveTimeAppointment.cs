using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{

    public partial class RoundWaveTimeAppointment
    {
        [Key]
        public Guid DockQoutaInterval_Index { get; set; }

        public Guid DockQouta_Index { get; set; }

        public Guid Dock_Index { get; set; }

        public string Dock_Id { get; set; }

        public string Dock_Name { get; set; }

        public string Interval_Start { get; set; }

        public string Interval_End { get; set; }

        public int Seq { get; set; }

        public bool IsBreakTime { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        public string Cancel_By { get; set; }
        public string checkIn_Limit_Before { get; set; }
        public string checkIn_Limit_After { get; set; }

        public DateTime? Cancel_Date { get; set; }
    }
}
