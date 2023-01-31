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
    [Route("api/AutoGoodsIssue")]
    [ApiController]
    public class AutoPlanGoodsIssueController : ControllerBase
    {
        #region AutobasicSuggestion
        [HttpPost("autobasicSuggestion")]
        public IActionResult autobasicSuggestion([FromBody]JObject body)

        {
            try
            {
                var service = new GoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autobasicSuggestion(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region autoPlanGoodIssueNo
        [HttpPost("autoPlanGoodIssueNo")]
        public IActionResult autoPlanGoodIssueNo([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoPlanGoodIssueNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region autoGoodIssueNo
        [HttpPost("autoGoodIssueNo")]
        public IActionResult autoGoodIssueNo([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGoodIssueNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region AutoOwnerfilter
        [HttpPost("autoOwnerfilter")]
        public IActionResult autoOwnerfilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoOwnerfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoUser
        [HttpPost("autoUser")]
        public IActionResult autoUser([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoUser(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoGI
        [HttpPost("autoGI")]
        public IActionResult autoGI([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGI(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region autoGI_SUB
        [HttpPost("autoGI_SUB")]
        public IActionResult autoGI_SUB([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGI_SUB(Models);
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
