using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using MasterBusiness.GoodsIssue;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GIAPI.Controllers
{
    [Route("api/ScanPickItemExport")]
    [ApiController]
    public class ScanPickItemExportController : ControllerBase
    {
        #region GetDataScanTaskItemExport
        [HttpPost("GetDataScanTaskItemExport")]
        public IActionResult GetDataScanTaskItemExport([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemExportService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.GetDataScanTaskItem(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetDataScanTaskLabelingItemExport
        [HttpPost("GetDataScanTaskLabelingItemExport")]
        public IActionResult GetDataScanTaskLabelingItemExport([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemExportService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.GetDataScanTaskLabelingItem(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetDataScanTaskScanPickQtyItemExport
        [HttpPost("GetDataScanTaskScanPickQtyItemExport")]
        public IActionResult GetDataScanTaskScanPickItemExport([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemExportService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.GetDataScanTaskScanPickQtyItem(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetDataScanTaskLocationLabelingItemExport
        [HttpPost("GetDataScanTaskLocationLabelingItemExport")]
        public IActionResult GetDataScanTaskLocationLabelingItemExport([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemExportService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.GetDataScanTaskLocationLabelingItem(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetDataScanTaskMoveToselectiveExport
        [HttpPost("GetDataScanTaskMoveToselectiveExport")]
        public IActionResult GetDataScanTaskMoveToselectiveExport([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.GetDataScanTaskMoveToselective(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetDataScanTasStgToDockExport
        [HttpPost("GetDataScanTasStgToDockExport")]
        public IActionResult GetDataScanTasStgToDockExport([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemExportService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.GetDataScanTasStgToDock(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
