using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using MasterBusiness.GoodsIssue;
using MasterDataBusiness.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GIAPI.Controllers
{
    [Route("api/ScanPick")]
    [ApiController]
    public class ScanPickController : ControllerBase
    {

        #region Filter

        #region FilterTask
        [HttpPost("FilterTask")]
        public IActionResult FilterTask([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTask(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskLabeling
        [HttpPost("FilterTaskLabeling")]
        public IActionResult FilterTaskLabeling([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskLabeling(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskPickQty
        [HttpPost("FilterTaskPickQty")]
        public IActionResult FilterTaskPickQty([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskPickQty(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskLocationLabeling
        [HttpPost("FilterTaskLocationLabeling")]
        public IActionResult FilterTaskLocationLabeling([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskLocationLabeling(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskMoveToSelective
        [HttpPost("FilterTaskMoveToSelective")]
        public IActionResult FilterTaskMoveToSelective([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskMoveToSelective(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region FilterTaskStgtodock
        [HttpPost("FilterTaskStgtodock")]
        public IActionResult FilterTaskStgtodock([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.FilterTaskStgtodock(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion



        #endregion

        #region Scantag

        #region Scantag_Dock
        [HttpPost("Scantag_Dock")]
        public IActionResult Scantag_Dock([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.Scantag_Dock(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region Scantag
        [HttpPost("Scantag")]
        public IActionResult Scantag([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.Scantag(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScantagMoveToSTG
        [HttpPost("ScantagMoveToSTG")]
        public IActionResult ScantagMoveToSTG([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScantagMoveToSTG(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #endregion

        #region ScanLocaton

        #region ScanLocaton 
        [HttpPost("ScanLocaton")]
        public IActionResult ScanLocaton([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanLocatonScanPick(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion
        
        #region ScanLocatonLabeling 
        [HttpPost("ScanLocatonLabeling")]
        public IActionResult ScanLocatonLabeling([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanLocatonLabeling(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanLocatonQty 
        [HttpPost("ScanLocatonQty")]
        public IActionResult ScanLocatonQty([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanLocatonQty(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanLocatonQty 
        [HttpPost("ScanLocatonQtyCool")]
        public IActionResult ScanLocatonQtyCool([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanLocatonQtyCool(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanLocatonDock 
        [HttpPost("ScanLocatonDock")]
        public IActionResult ScanLocatonDock([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanLocatonDock(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanLocationstgtoDock 
        [HttpPost("ScanLocationstgtoDock")]
        public IActionResult ScanLocationstgtoDock([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanLocationstgtoDock(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanLocatonMoveToSTG
        [HttpPost("ScanLocatonMoveToSTG")]
        public IActionResult ScanLocatonMoveToSTG([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanLocatonMoveToSTG(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #endregion
        
        #region GetTagOut 
        [HttpPost("GetTagOut")]
        public IActionResult GetTagOut([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.GetTagOut(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanCarton
        [HttpPost("ScanCarton")]
        public IActionResult ScanCarton([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanCarton(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirm 
        [HttpPost("ScanConfirm")]
        public IActionResult ScanConfirm([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel>(body.ToString());
                var result = service.ScanConfirm(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirmforFilmcutting 
        [HttpPost("ScanConfirmforFilmcutting")]
        public IActionResult ScanConfirmforFilmcutting([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel>(body.ToString());
                var result = service.ScanConfirmforFilmcutting(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion


        #region ScanConfirmforError
        [HttpPost("ScanConfirmforError")]
        public IActionResult ScanConfirmforError([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel>(body.ToString());
                var result = service.ScanConfirmforError(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirmforPickingLocation 
        [HttpPost("ScanConfirmforPickingLocation")]
        public IActionResult ScanConfirmforPickingLocation([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel>(body.ToString());
                var result = service.ScanConfirmforPickingLocation(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirmforStgToDock 
        [HttpPost("ScanConfirmforStgToDock")]
        public IActionResult ScanConfirmforStgToDock([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel>(body.ToString());
                var result = service.ScanConfirmforStgToDock(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirmLocaton 
        [HttpPost("ScanConfirmLocaton")]
        public IActionResult ScanConfirmLocaton([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanConfirmLocaton(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion
        
        #region ScanConfirmLabeling 
        [HttpPost("ScanConfirmLabeling")]
        public IActionResult ScanConfirmLabeling([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel>(body.ToString());
                var result = service.ScanConfirmLabeling(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanCheckTagout 
        [HttpPost("ScanCheckTagout")]
        public IActionResult ScanCheckTagout([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel_Qty>(body.ToString());
                var result = service.ScanCheckTagout(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirmPickQty 
        [HttpPost("ScanConfirmPickQty")]
        public IActionResult ScanConfirmPickQty([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel_Qty>(body.ToString());
                var result = service.ScanConfirmPickQty(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirmPickQty 
        [HttpPost("ScanConfirmPickQtycool")]
        public IActionResult ScanConfirmPickQtycool([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel_Qty>(body.ToString());
                var result = service.ScanConfirmPickQtycool(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        
        #endregion

        #region ScanConfirmPickQty_bypassLBL 
        [HttpPost("ScanConfirmPickQty_bypassLBL")]
        public IActionResult ScanConfirmPickQty_bypassLBL([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel_Qty>(body.ToString());
                var result = service.ScanConfirmPickQty_bypassLBL(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        
        #endregion

        #region ScanQty_print_tag 
        [HttpPost("ScanQty_print_tag")]
        public IActionResult ScanQty_print_tag([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel_Qty>(body.ToString());
                var result = service.ScanQty_print_tag(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region Dropdown Printer 
        [HttpPost("dropdown_printer")]
        public IActionResult dropdown_printer()
        {
            try
            {
                var service = new ScanPickService();
                var result = service.dropdown_printer();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirmforMoveToSTG 
        [HttpPost("ScanConfirmforMoveToSTG")]
        public IActionResult ScanConfirmforMoveToSTG([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPickViewModel>(body.ToString());
                var result = service.ScanConfirmforMoveToSTG(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirmLocatonFilmcutting 
        [HttpPost("ScanConfirmLocatonFilmcutting")]
        public IActionResult ScanConfirmLocatonFilmcutting([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ScanPicksearchViewModel>(body.ToString());
                var result = service.ScanConfirmLocatonFilmcutting(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetReasonCode
        [HttpPost("GetReasonCode")]
        public IActionResult GetReasonCode([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<ReasonCodeFilterViewModel>(body.ToString());
                var result = service.GetReasonCode(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanConfirmAuto
        [HttpPost("ScanConfirmAuto")]
        public IActionResult ScanConfirmAuto([FromBody]JObject body)
        {
            try
            {
                var service = new ScanPickService();
                var Models = JsonConvert.DeserializeObject<GoodIssueViewModel>(body.ToString());
                var result = service.ScanConfirmAuto(Models.goodsIssue_No);
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
