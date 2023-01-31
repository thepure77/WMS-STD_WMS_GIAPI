using System;
using GIBusiness.TagOut;
using GIBusiness.TrackingLoading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GIAPI.Controllers
{
    [Route("api/ReturnToWB")]
    [ApiController]
    public class ReturnToWBController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ReturnToWBController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region scanorder
        [HttpPost("scanorder")]
        public IActionResult scanorder([FromBody]JObject body)
        {
            try
            {
                var service = new ReturnToWBService();
                var Models = new ReturnToWBViewModel();
                Models = JsonConvert.DeserializeObject<ReturnToWBViewModel>(body.ToString());
                var result = service.scanorder(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanLocaton 
        [HttpPost("ScanLocaton")]
        public IActionResult ScanLocaton([FromBody]JObject body)
        {
            try
            {
                var service = new ReturnToWBService();
                var Models = JsonConvert.DeserializeObject<ReturnToWBViewModel>(body.ToString());
                var result = service.ScanLocaton(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region saveorderlocation
        [HttpPost("saveorderlocation")]
        public IActionResult saveorderlocation([FromBody]JObject body)
        {
            try
            {
                var service = new ReturnToWBService();
                var Models = new ReturnToWBViewModel();
                Models = JsonConvert.DeserializeObject<ReturnToWBViewModel>(body.ToString());
                var result = service.saveorderlocation(Models);
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