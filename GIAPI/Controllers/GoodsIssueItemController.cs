using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using GIBusiness.PlanGoodIssue;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GIAPI.Controllers
{
    [Route("api/GoodsIssueItem")]
    [ApiController]
    public class GoodsIssueItemController : ControllerBase
    {
        #region find
        [HttpGet("find/{id}")]
        public IActionResult find(Guid id)
        {
            try
            {
                var service = new GoodsIssueItemService();
                var result = service.find(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region checkPGIHasGI
        [HttpPost("checkPGIHasGI")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsIssueItemService();
                var Models = new GoodIssueViewModelItem();
                Models = JsonConvert.DeserializeObject<GoodIssueViewModelItem>(body.ToString());
                var result = service.checkPGIHasGI(Models);
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
