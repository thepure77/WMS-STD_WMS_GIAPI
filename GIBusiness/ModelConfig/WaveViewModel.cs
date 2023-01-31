using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDataBusiness.ViewModels
{


    public  class WaveViewModel
    {
        [Key]
        public Guid wave_Index { get; set; }

        [StringLength(50)]
        public string wave_Id { get; set; }

        [StringLength(200)]
        public string wave_Name { get; set; }

        [StringLength(200)]
        public string wave_Case { get; set; }

        public int? isActive { get; set; }

        public int? isDelete { get; set; }

    }
}
