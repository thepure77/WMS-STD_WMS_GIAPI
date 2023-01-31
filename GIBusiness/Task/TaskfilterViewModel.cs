using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{


    public class TaskfilterViewModel 
    {
        public TaskfilterViewModel()
        {
            listTaskViewModel = new List<listTaskViewModel>();

        }

        public Guid? task_Index { get; set; }


        public string task_No { get; set; }
        public string goodsIssue_No { get; set; }
        public string userAssign { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string create_Time { get; set; }
        public string assign_By { get; set; }

        public List<listTaskViewModel> listTaskViewModel { get; set; }

        public class actionResultTask
        {
            public string msg { get; set; }
        }

    }



}
