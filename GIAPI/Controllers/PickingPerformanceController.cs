using GIBusiness.PickingOrderPerformance;
using GIBusiness.PlanGoodIssue;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIAPI.Controllers
{
    [Route("api/PickingPerformance")]
    [ApiController]
    public class PickingPerformanceController : ControllerBase
    {
        [HttpPost("PickingPerformanceSearchOld")]
        public IActionResult OverallPerformanceSearchOld([FromBody]JObject body)
        {
            try
            {
                var service = new PickingPerformanceService();
                var Models = new PickingSearchViewModel();
                Models = JsonConvert.DeserializeObject<PickingSearchViewModel>(body.ToString());
                var result = service.PickingPerformanceSearch(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("PickingPerformanceSearch")]
        public IActionResult OverallPerformanceSearch([FromBody]JObject body)
        {
            try
            {
                var service = new PickingPerformanceService();
                var Models = new PickingSearchViewModel();
                Models = JsonConvert.DeserializeObject<PickingSearchViewModel>(body.ToString());
                var result = service.PickingPerformanceSearch(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("RoundList")]
        public IActionResult Get()
        {
            try
            {
                var service = new PickingPerformanceService();
                var result = service.FilterRound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("DashboardInsert")]
        public IActionResult DashboardInsert()
        {
            try
            {
                var service = new PickingPerformanceService();
                service.Dastboard_Insert();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //[HttpPost("PickingPerformanceExpressSearch")]
        //public IActionResult OverallPerformanceExpressSearch([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new PickingPerformanceService();
        //        var Models = new PickingSearchViewModel();
        //        Models = JsonConvert.DeserializeObject<PickingSearchViewModel>(body.ToString());
        //        var result = service.PickingPerformanceExpressSearch(Models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
    }
}
