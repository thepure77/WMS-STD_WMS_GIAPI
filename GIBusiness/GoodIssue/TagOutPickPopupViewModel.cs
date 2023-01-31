using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class TagOutPickPopupViewModel : Pagination
    {
        [Key]
        public Guid TagOutPick_Index { get; set; }

        [StringLength(50)]
        public string TagOutPick_No { get; set; }

        [StringLength(200)]
        public string TagOutPickRef_No1 { get; set; }

        [StringLength(200)]
        public string TagOutPickRef_No2 { get; set; }

        [StringLength(200)]
        public string TagOutPickRef_No3 { get; set; }

        [StringLength(200)]
        public string TagOutPickRef_No4 { get; set; }

        [StringLength(200)]
        public string TagOutPickRef_No5 { get; set; }

        public int? TagOutPick_Status { get; set; }

        [StringLength(200)]
        public string UDF_1 { get; set; }

        [StringLength(200)]
        public string UDF_2 { get; set; }

        [StringLength(200)]
        public string UDF_3 { get; set; }

        [StringLength(200)]
        public string UDF_4 { get; set; }

        [StringLength(200)]
        public string UDF_5 { get; set; }

        public Guid? Zone_Index { get; set; }

        public Guid? Ref_Process_Index { get; set; }

        [StringLength(200)]
        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Cancel_Date { get; set; }

        public class actionResultTagOutPickPopupViewModel
        {
            public IList<TagOutPickPopupViewModel> itemsTag { get; set; }
            public Pagination pagination { get; set; }
        }

    }
}
