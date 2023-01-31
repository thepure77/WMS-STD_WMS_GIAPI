using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public partial class RoundWaveViewModel
    {
        public RoundWaveViewModel()
        {
            itemsUpdate = new List<RoundWaveViewModel>();
        }

        public long RowIndex { get; set; }

        public string Appointment_Date { get; set; }

        public string planGoodsIssue_Due_Date { get; set; }

        public string Appointment_Time { get; set; }

        public string TM_No { get; set; }
        public bool Is_map { get; set; }

        public string Dock_Name { get; set; }

        public string Appointment_Id { get; set; }
        public string VehicleType_Name { get; set; }

        public int? CountOrder { get; set; }

        public string round_Index { get; set; }

        public string round_id { get; set; }

        public string round_Name { get; set; }

        public string userName { get; set; }

        public List<RoundWaveViewModel> itemsUpdate { get; set; }
    }

    public partial class actionResultRoundWaveViewModel : Result
    {
        public actionResultRoundWaveViewModel()
        {
            itemsDetail = new List<RoundWaveViewModel>();
        }

        public List<RoundWaveViewModel> itemsDetail { get; set; }
    }

    public partial class Appointment_time
    {
        public string Appointment_Time { get; set; }

    }

}
