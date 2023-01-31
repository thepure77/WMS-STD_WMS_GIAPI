using GIBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDataBusiness.ViewModels
{


    public class SearchProductConversionBarcodeViewModel
    {
        public Guid productConversionBarcode_Index { get; set; }
        public string productConversionBarcode_Id { get; set; }
        public string productConversionBarcode { get; set; }

        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }

        public Guid? productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public decimal? productConversion_Ratio { get; set; }

        public Guid? owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }

        public int? isActive { get; set; }

        public int? isDelete { get; set; }

        public int? isSystem { get; set; }

        public int? status_Id { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public string key { get; set; }

    }
    public class actionResultProductConversionBarcodeViewModel
    {
        public IList<SearchProductConversionBarcodeViewModel> itemsProductConversionBarcode { get; set; }
        public Pagination pagination { get; set; }
    }
}
