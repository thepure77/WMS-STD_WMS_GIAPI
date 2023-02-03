using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using GIBusiness.PlanGoodIssue;
using GIBusiness.TagOut;
using MasterBusiness.GoodsIssue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GIAPI.Controllers
{
    [Route("api/MakeTagOut")]
    [ApiController]
    public class MakeTagOutController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public MakeTagOutController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region
        [HttpPost("maketagOut_no_DO")]
        public IActionResult maketagOut_no_DO([FromBody]JObject body)
        {
            try
            {
                var service = new TagoutService();
                var Models = new findtagViewModelItem();
                Models = JsonConvert.DeserializeObject<findtagViewModelItem>(body.ToString());
                var result = service.maketagOut(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

    


        #region
        //[HttpPost("maketagOut_no_V2")]
        //public IActionResult maketagOut_no([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new TagoutService();
        //        var Models = new findtagViewModelItem();
        //        Models = JsonConvert.DeserializeObject<findtagViewModelItem>(body.ToString());
        //        var result = service.maketagOut_V2(Models);
        //        return Ok(result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message);
        //    }
        //}
        #endregion

        #region
        [HttpPost("maketagOut_no")]
        public IActionResult maketagOut_no([FromBody]JObject body)
        {
            try
            {
                var service = new TagoutService();
                var Models = new findtagViewModelItem();
                Models = JsonConvert.DeserializeObject<findtagViewModelItem>(body.ToString());
                var result = service.maketagOut(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region
        [HttpPost("maketagOut_no_V4")]
        public IActionResult maketagOut_no_V4([FromBody]JObject body)
        {
            try
            {
                var service = new TagoutService();
                var Models = new findtagViewModelItem();
                Models = JsonConvert.DeserializeObject<findtagViewModelItem>(body.ToString());
                var result = service.maketagOut_V4(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region
        [HttpPost("maketagOutTask")]
        public IActionResult maketagOutTask()
        {
            try
            {
                var service = new TagoutService();
                var result = service.maketagOutTask();
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