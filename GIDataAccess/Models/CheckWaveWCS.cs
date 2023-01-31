using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class CheckWaveWCS   
    {
        [Key]
  
        public string header { get; set; }

        public string Remaining { get; set; }

        public string descip { get; set; }

      
        

    }
}
