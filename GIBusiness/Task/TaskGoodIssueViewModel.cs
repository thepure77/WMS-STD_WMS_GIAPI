using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{
    public class TaskGoodIssueViewModel
    {
        [Key]
        public Guid goodsIssue_Index { get; set; }

        public string tag_No { get; set; }

        public string goodsIssue_No { get; set; }

        public string task_No { get; set; }

        public Guid? product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string product_SecondName { get; set; }
        public string location_Name { get; set; }
        public string productConversion_Name { get; set; }
        public decimal? qty { get; set; }



    }
}
