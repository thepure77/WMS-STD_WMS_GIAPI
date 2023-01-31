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
    [Route("api/ScanPickItem")]
    [ApiController]
    public class ScanPickItemController : ControllerBase
    {
        #region GetDataScanTaskItem
        [HttpPost("GetDataScanTaskItem")]
        public IActionResult GetDataScanTaskItem([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemService();
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

        #region GetDataScanTaskLabelingItem
        [HttpPost("GetDataScanTaskLabelingItem")]
        public IActionResult GetDataScanTaskLabelingItem([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemService();
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

        #region GetDataScanTaskScanPickQtyItem
        [HttpPost("GetDataScanTaskScanPickQtyItem")]
        public IActionResult GetDataScanTaskScanPickItem([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemService();
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

        #region GetDataScanTaskLocationLabelingItem
        [HttpPost("GetDataScanTaskLocationLabelingItem")]
        public IActionResult GetDataScanTaskLocationLabelingItem([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemService();
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

        #region GetDataScanTaskMoveToselective
        [HttpPost("GetDataScanTaskMoveToselective")]
        public IActionResult GetDataScanTaskMoveToselective([FromBody]JObject body)
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

        #region GetDataScanTasStgToDock
        [HttpPost("GetDataScanTasStgToDock")]
        public IActionResult GetDataScanTasStgToDock([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickItemService();
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
