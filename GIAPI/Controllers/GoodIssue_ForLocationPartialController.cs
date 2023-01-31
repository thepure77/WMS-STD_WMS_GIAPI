using System;
using GIBusiness;
using GIBusiness.GoodIssue;
using GIBusiness.GoodsIssue;
using GIBusiness.PlanGoodIssue;
using GIBusiness.Reports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GIAPI.Controllers
{
    [Route("api/GoodIssue_ForLocationPartial")]
    [ApiController]
    public class GoodIssue_ForLocationPartialController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GoodIssue_ForLocationPartialController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region checkWaveWCS
        [HttpPost("checkWaveWCS")]
        public IActionResult checkWaveWCS([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<ResultRunWave>(body.ToString());
                var result = service.checkWaveWCS(Models);
                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

    }
}
