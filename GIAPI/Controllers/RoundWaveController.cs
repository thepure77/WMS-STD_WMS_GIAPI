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
    [Route("api/RoundWave")]
    [ApiController]
    public class RoundWaveController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public RoundWaveController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region filter
        [HttpPost("Appointtimefilter")]
        public IActionResult Appointtimefilter([FromBody]JObject body)
        {
            try
            {
                var service = new RoundWaveService();
                var Models = new RoundWaveViewModel();
                Models = JsonConvert.DeserializeObject<RoundWaveViewModel>(body.ToString());
                var result = service.Appointtimefilter();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region filter
        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new RoundWaveService();
                var Models = new RoundWaveViewModel();
                Models = JsonConvert.DeserializeObject<RoundWaveViewModel>(body.ToString());
                var result = service.filter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }
        #endregion

        #region Update_Round
        [HttpPost("Update_Round")]
        public IActionResult Update_Round([FromBody]JObject body)
        {
            try
            {
                var service = new RoundWaveService();
                var Models = new RoundWaveViewModel();
                Models = JsonConvert.DeserializeObject<RoundWaveViewModel>(body.ToString());
                var result = service.updateRound(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        #endregion

        #region Delete_Round
        [HttpPost("Delete_Round")]
        public IActionResult Delete_Round([FromBody]JObject body)
        {
            try
            {
                var service = new RoundWaveService();
                var Models = new RoundWaveViewModel();
                Models = JsonConvert.DeserializeObject<RoundWaveViewModel>(body.ToString());
                var result = service.deleteRound(Models);
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
