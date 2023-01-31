using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_PrintTagOut
    {
        [Key]
        public Guid TagOut_Index { get; set; }

        public string TagOut_No { get; set; }

        public string TagOutType { get; set; }

        public string LocationType { get; set; }

        public string TagOutRef_No1 { get; set; }

        public string TagOutRef_No2 { get; set; }

        public string TagOutRef_No3 { get; set; }

        public string TagOutRef_No4 { get; set; }

        public string TagOutRef_No5 { get; set; }

        public int? TagOut_Status { get; set; }

        public string UDF_1 { get; set; }

        public string UDF_2 { get; set; }

        public string UDF_3 { get; set; }

        public string UDF_4 { get; set; }

        public string UDF_5 { get; set; }

        public Guid? Zone_Index { get; set; }

        public Guid? Ref_Process_Index { get; set; }

        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

        public int? isPrint { get; set; }

        public Guid? Location_Confirm_Index { get; set; }

        public string Location_Confirm_Id { get; set; }

        public string Location_Confirm_Name { get; set; }
        public string StatusRollcage { get; set; }
    }
}
