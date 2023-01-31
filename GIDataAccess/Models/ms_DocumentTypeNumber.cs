using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class ms_DocumentTypeNumber
    {
        [Key]
        public Guid DocumentTypeNumber_Index { get; set; }

        public Guid DocumentType_Index { get; set; }

        [StringLength(200)]
        public string DocumentTypeNumber_Year { get; set; }

        [StringLength(200)]
        public string DocumentTypeNumber_Month { get; set; }

        [StringLength(200)]
        public string DocumentTypeNumber_Day { get; set; }

        public int? DocumentTypeNumber_Running { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }

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

        //public virtual ms_DocumentType ms_DocumentType { get; set; }

    }
}
