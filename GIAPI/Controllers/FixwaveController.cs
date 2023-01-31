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

namespace GIAPI.Controllers
{
    [Route("api/GoodIssue")]
    [ApiController]
    public class FixwaveController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public FixwaveController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region ChecktagGi
        [HttpPost("ChecktagGi")]
        public IActionResult ChecktagGi([FromBody]JObject body)
        {
            try
            {
                var service = new FixwaveService();
                var Models = JsonConvert.DeserializeObject<ChecktagViewModel>(body.ToString());
                var result = service.ChecktagGi(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region Updatebinbalance
        [HttpPost("Updatebinbalance")]
        public IActionResult Updatebinbalance([FromBody]JObject body)
        {
            try
            {
                var service = new FixwaveService();
                var Models = JsonConvert.DeserializeObject<ChecktagViewModel>(body.ToString());
                var result = service.Updatebinbalance(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ChecktagByGi
        [HttpPost("ChecktagByGi")]
        public IActionResult ChecktagByGi([FromBody]JObject body)
        {
            try
            {
                var service = new FixwaveService();
                var Models = JsonConvert.DeserializeObject<ChecktagViewModel>(body.ToString());
                var result = service.ChecktagByGi(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region DeleteGoodsIssueitemlocation
        [HttpPost("DeleteGoodsIssueitemlocation")]
        public IActionResult DeleteGoodsIssueitemlocation([FromBody]JObject body)
        {
            try
            {
                var service = new FixwaveService();
                var Models = JsonConvert.DeserializeObject<ChecktagViewModel>(body.ToString());
                var result = service.DeleteGoodsIssueitemlocation(Models);
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