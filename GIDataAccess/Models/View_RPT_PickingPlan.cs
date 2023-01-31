using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_RPT_PickingPlan
    {
        [Key]
        public long? RowIndex { get; set; }
        public Guid GoodsIssue_Index { get; set; }

        public DateTime? GoodsIssue_Date { get; set; }

        public string SKUAvailable { get; set; }
        public string SKUAvailable_VC { get; set; }

        public decimal? PrdAvailable { get; set; }
        public decimal? PrdAvailable_VC { get; set; }

        public string Time { get; set; }

        public string GoodsIssue_No { get; set; }
        public string TruckLoad_No { get; set; }

        public int? CountTagASRS { get; set; }

        public int? CountCTNASRS { get; set; }

        public int? CountTote { get; set; }

    
        public decimal? SumQtyVC { get; set; }

        public int? CountPrdVC { get; set; }


        public decimal? SumQtyCFR { get; set; }

        public int? CountPrdCFR { get; set; }

        public int? CountTagLBL { get; set; }

        public int? CountTagSTG { get; set; }

        public int? CountTagBUF { get; set; }

        public int? CountCTNSTG { get; set; }

        public decimal? CountCTNSEL { get; set; }

        public decimal? CountCTNBUF { get; set; }

        public string Chute_No { get; set; }

        public int? CountCTNChute { get; set; }

        public int? CountCTNLBL { get; set; }


        public string Dock_Name { get; set; }


        public decimal? ASRSPercent { get; set; }


        public decimal? SELPercent { get; set; }


        public decimal? VCPercent { get; set; }


        public decimal? CFRPercent { get; set; }

        public int? RollCageAll       { get; set; }
        public int? RollCageUse       { get; set; }
        public int? RollCageNotUse    { get; set; }
        public int? MaxToteM          { get; set; }
        public int? MaxToteL          { get; set; }
        public int? UseToteM          { get; set; }
        public int? UseToteL          { get; set; }
        public int? AvailableToteM    { get; set; }
        public int? AvailableToteL    { get; set; }
        public int? PendingToteM      { get; set; }
        public int? PendingToteL      { get; set; }
        public int? ReturnToteM       { get; set; }
        public int? ReturnToteL       { get; set; }
        public int? TotalUseRollcage { get; set; }
        public int? RollcageCompleted { get; set; }
        public int? ToteLCompleted { get; set; }
        public int? ToteMCompleted { get; set; }
    }
}
