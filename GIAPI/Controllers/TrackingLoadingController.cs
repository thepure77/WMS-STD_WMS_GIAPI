using System;
using GIBusiness.TagOut;
using GIBusiness.TrackingLoading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GIAPI.Controllers
{
    [Route("api/TrackingLoading")]
    [ApiController]
    public class TrackingLoadingController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public TrackingLoadingController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region findShipment
        [HttpPost("findShipment")]
        public IActionResult ScantagOutitem([FromBody]JObject body)
        {
            try
            {
                var service = new TrackingLoadingService();
                var Models = new TrackingLoadingViewModel();
                Models = JsonConvert.DeserializeObject<TrackingLoadingViewModel>(body.ToString());
                var result = service.findShipment(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region savetracking
        [HttpPost("savetracking")]
        public IActionResult savetracking([FromBody]JObject body)
        {
            try
            {
                var service = new TrackingLoadingService();
                var Models = new TrackingLoadingViewModel();
                Models = JsonConvert.DeserializeObject<TrackingLoadingViewModel>(body.ToString());
                var result = service.savetracking(Models);
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