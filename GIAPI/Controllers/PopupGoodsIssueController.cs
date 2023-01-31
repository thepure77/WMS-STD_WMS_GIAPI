using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using MasterBusiness.GoodsIssue;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GIAPI.Controllers
{
    [Route("api/PopupGoodsIssue")]
    [ApiController]
    public class PopupGoodsIssueController : ControllerBase
    {


        #region PopupGoodsIssuefilter
        [HttpPost("popupGoodsIssuefilter")]
        public IActionResult popupGoodsIssuefilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new SearchDetailModel();
                Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.popupGoodsIssuefilter(Models);
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
