using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using MasterDataBusiness.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanGIAPI.Controllers
{
    [Route("api/DropdownGoodsIssue")]
    [ApiController]
    public class DropdownGoodsIssueController : ControllerBase
    {
        #region DropdownDocumentType
        [HttpPost("dropdownDocumentType")]
        public IActionResult dropdownDocumentType([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new DocumentTypeViewModel();
                Models = JsonConvert.DeserializeObject<DocumentTypeViewModel>(body.ToString());
                var result = service.DropdownDocumentType(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region DropdownStatus
        [HttpPost("dropdownStatus")]
        public IActionResult DropdownStatus([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new ProcessStatusViewModel();
                Models = JsonConvert.DeserializeObject<ProcessStatusViewModel>(body.ToString());
                var result = service.dropdownStatus(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region DropdownDocumentTypeWave
        [HttpPost("DropdownDocumentTypeWave")]
        public IActionResult DropdownDocumentTypeWave([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new WaveViewModel();
                Models = JsonConvert.DeserializeObject<WaveViewModel>(body.ToString());
                var result = service.DropdownDocumentTypeWave(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownUser
        [HttpPost("dropdownUser")]
        public IActionResult dropdownUser([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new UserViewModel();
                Models = JsonConvert.DeserializeObject<UserViewModel>(body.ToString());
                var result = service.dropdownUser(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region dropdownWarehouse
        [HttpPost("dropdownWarehouse")]
        public IActionResult dropdownWarehouse([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new warehouseDocViewModel();
                Models = JsonConvert.DeserializeObject<warehouseDocViewModel>(body.ToString());
                var result = service.dropdownWarehouse(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownCurrency
        [HttpPost("dropdownCurrency")]
        public IActionResult dropdownCurrency([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new CurrencyViewModel();
                Models = JsonConvert.DeserializeObject<CurrencyViewModel>(body.ToString());
                var result = service.dropdownCurrency(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownOwner
        [HttpPost("dropdownOwner")]
        public IActionResult dropdownOwner([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new OwnerViewModel();
                Models = JsonConvert.DeserializeObject<OwnerViewModel>(body.ToString());
                var result = service.dropdownOwner(Models);
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
