using System;
using GIBusiness.TagOut;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GIAPI.Controllers
{
    [Route("api/PickToLight")]
    [ApiController]
    public class PickToLightController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public PickToLightController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region ScantagOutitem
        [HttpPost("ScantagOutitem")]
        public IActionResult ScantagOutitem([FromBody]JObject body)
        {
            try
            {
                var service = new TagOutitemService();
                var Models = new TagOutitemViewModel();
                Models = JsonConvert.DeserializeObject<TagOutitemViewModel>(body.ToString());
                var result = service.ScantagOutitem(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region scanBarcode_no
        [HttpPost("scanBarcode_no")]
        public IActionResult scanBarcode_no([FromBody]JObject body)
        {
            try
            {
                var service = new TagOutitemService();
                var Models = new TagOutitemViewModel();
                Models = JsonConvert.DeserializeObject<TagOutitemViewModel>(body.ToString());
                var result = service.scanBarcode_no(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region confirmPicktoLight
        [HttpPost("confirmPicktoLight")]
        public IActionResult confirmPicktoLight([FromBody]JObject body)
        {
            try
            {
                var service = new TagOutitemService();
                var Models = new TagOutitemViewModel();
                Models = JsonConvert.DeserializeObject<TagOutitemViewModel>(body.ToString());
                var result = service.confirmPicktoLight(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region confirmPicktoLight
        [HttpPost("confirmPicktoLight_Cut")]
        public IActionResult confirmPicktoLight_Cut()
        {
            try
            {
                var service = new TagOutitemService();
                var result = service.confirmPicktoLight_CUT();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region DetailScanPicktolight
        [HttpPost("DetailScanPicktolight")]
        public IActionResult DetailScanPicktolight([FromBody]JObject body)
        {
            try
            {
                var service = new TagOutitemService();
                var Models = new TagOutitemViewModel();
                Models = JsonConvert.DeserializeObject<TagOutitemViewModel>(body.ToString());
                var result = service.DetailScanPicktolight(Models);
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