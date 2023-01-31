using GIBusiness.OverallPerformanceDashboard;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIAPI.Controllers
{
    [Route("api/OverallPerformance")]
    [ApiController]
    public class OverallPerformanceController : ControllerBase
    {
        [HttpPost("OverallPerformanceSearch")]
        public IActionResult OverallPerformanceSearch([FromBody]JObject body)
        {
            try
            {
                var service = new OverallPerformanceService();
                var Models = new OverallPerformanceSearchViewModel();
                Models = JsonConvert.DeserializeObject<OverallPerformanceSearchViewModel>(body.ToString());
                var result = service.OverallPerformanceSearch(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("OverallInsert")]
        public IActionResult OverallInsert()
        {
            try
            {
                var service = new OverallPerformanceService();
                service.Overall_Insert();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }

   
}
