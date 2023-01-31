using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using MasterBusiness.GoodsIssue;
using MasterDataBusiness.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GIAPI.Controllers
{
    [Route("api/ScanPickExport")]
    [ApiController]
    public class ScanPickExportController : ControllerBase
    {

        #region Filter

        #region FilterTaskExport
        [HttpPost("FilterTaskExport")]
        public IActionResult FilterTask([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickServiceExport();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTask(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskLabelingExport
        [HttpPost("FilterTaskLabelingExport")]
        public IActionResult FilterTaskLabeling([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickServiceExport();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskLabeling(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskPickQtyExport
        [HttpPost("FilterTaskPickQtyExport")]
        public IActionResult FilterTaskPickQty([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickServiceExport();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskPickQty(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskLocationLabelingExport
        [HttpPost("FilterTaskLocationLabelingExport")]
        public IActionResult FilterTaskLocationLabeling([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickServiceExport();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskLocationLabeling(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskMoveToSelectiveExport
        [HttpPost("FilterTaskMoveToSelectiveExport")]
        public IActionResult FilterTaskMoveToSelective([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickServiceExport();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskMoveToSelective(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskStgtodockExport
        [HttpPost("FilterTaskStgtodockExport")]
        public IActionResult FilterTaskStgtodock([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickServiceExport();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskStgtodock(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion
        
        #endregion

    }
}
