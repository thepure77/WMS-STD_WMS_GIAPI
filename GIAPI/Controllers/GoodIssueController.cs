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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GIAPI.Controllers
{
    [Route("api/GoodIssue")]
    [ApiController]
    public class GoodIssueController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GoodIssueController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region filter
        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new SearchDetailModel();
                Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.filter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region find
        [HttpGet("find/{id}")]
        public IActionResult find(Guid id)
        {
            try
            {
                var service = new GoodIssueService();
                var result = service.find(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region goodsissueHeader
        [HttpPost("goodsissueHeader")]
        public IActionResult goodsissueHeader([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<GoodsIssueHeaderViewModel>(body.ToString());
                var result = service.goodsissueHeader(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region runwave
        [HttpPost("runwave")]
        public IActionResult runwave([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<RunWaveFilterViewModel>(body.ToString());
                var result = service.runwave(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region runwaveHeader
        [HttpPost("runwaveandHeader")]
        public IActionResult runwaveandHeader([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<RunWaveFilterV2ViewModel>(body.ToString());
                var result = service.runwaveandHeader(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region runwaveHeaderBom
        [HttpPost("runwaveandHeaderBom")]
        public IActionResult runwaveandHeaderBom([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<RunWaveFilterV2ViewModel>(body.ToString());
                var result = service.runwaveandHeaderBom(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region pickProduct
        [HttpPost("pickProduct")]
        public IActionResult pickProduct([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<PickbinbalanceViewModel>(body.ToString());
                var result = service.pickProduct(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region pickProductUnpack
        [HttpPost("pickProductUnpack")]
        public IActionResult pickProductUnpack([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<PickbinbalanceViewModel>(body.ToString());
                var result = service.pickProductUnpack(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region deletePickProduct
        [HttpPost("deletePickProduct")]
        public IActionResult deletePickProduct([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<PickbinbalanceViewModel>(body.ToString());
                var result = service.deletePickProduct(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ListdeletePickProduct
        [HttpPost("ListdeletePickProduct")]
        public IActionResult ListdeletePickProduct([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<ListPickbinbalanceViewModel>(body.ToString());
                var result = service.ListdeletePickProduct(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region PopupGIRunWave
        [HttpPost("PopupGIRunWave")]
        public IActionResult PopupGI([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new SearchDetailModel();
                Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.PopupGIRunWave(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region updateStatusAddData
        [HttpPost("updateStatusAddData")]
        public IActionResult updateStatusAddData([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<GoodsIssueViewModel>(body.ToString());
                var result = service.updateStatusAddData(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CheckPickGI
        [HttpPost("CheckPickGI")]
        public IActionResult CheckPickGI([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new goodsIssueItemLocationViewModel();
                Models = JsonConvert.DeserializeObject<goodsIssueItemLocationViewModel>(body.ToString());
                var result = service.CheckPickGI(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CutSlots
        [HttpPost("CutSlots")]
        public IActionResult CutSlots([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<CutSlotsViewModel>(body.ToString());
                var result = service.ConfirmCutSlots(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ConfirmStatus
        [HttpPost("ConfirmStatus")]
        public IActionResult ConfirmStatus([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<GoodsIssueViewModel>(body.ToString());
                var result = service.ConfirmStatus(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region DeleteDocument
        [HttpPost("DeleteDocument")]
        public IActionResult DeleteDocument([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<GoodsIssueViewModel>(body.ToString());
                var result = service.DeleteDocument(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ReportGi
        [HttpPost("PrintGI")]
        public IActionResult PrintReceipt([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<GoodsIssueViewModel>(body.ToString());
                localFilePath = service.PrintGi(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }
        #endregion

        [HttpPost("SentToSap")]
        public IActionResult SentToSap([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new ListGoodsIssueViewModel();
                Models = JsonConvert.DeserializeObject<ListGoodsIssueViewModel>(body.ToString());
                var result = service.SentToSap(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("SentToSapGetJson/{id}")]
        public IActionResult SentToSapGetJson(string id)
        {
            try
            {
                var service = new GoodIssueService();
                var result = service.SentToSapGetJson(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        #region CloseDocument
        [HttpPost("CloseDocument")]
        public IActionResult CloseDocument([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<GoodsIssueViewModel>(body.ToString());
                var result = service.CloseDocument(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CancelPGI
        [HttpPost("CancelPGI")]
        public IActionResult CancelPGI([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<GoodsIssueViewModel>(body.ToString());
                var result = service.CancelPGI(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region findUser
        [HttpPost("findUser")]
        public IActionResult findUser([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new im_Signatory_logViewModel();
                Models = JsonConvert.DeserializeObject<im_Signatory_logViewModel>(body.ToString());
                var result = service.findUser(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ListCutSlots
        [HttpPost("ListCutSlots")]
        public IActionResult ListCutSlots([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<ListCutSlotsViewModel>(body.ToString());
                var result = service.ListConfirmCutSlots(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region sendToSap
        [HttpPost("sendToSap")]
        public IActionResult sendToSap([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<ListSendToSapViewModel>(body.ToString());
                var result = service.sendToSap(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ReportPrintOutGI
        [HttpPost("ReportPrintOutGI")]
        public IActionResult ReportPrintOutGI([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new GoodIssueService();
                var Models = new ReportPrintOutGIViewModel();
                Models = JsonConvert.DeserializeObject<ReportPrintOutGIViewModel>(body.ToString());
                localFilePath = service.ReportPrintOutGI(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }
        #endregion

        #region findtag
        [HttpPost("findtag")]
        public IActionResult findtag([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new findtagViewModelItem();
                Models = JsonConvert.DeserializeObject<findtagViewModelItem>(body.ToString());
                var result = service.findtag(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
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

        #region autoWaveByRound
        [HttpPost("autoWaveByRound")]
        public IActionResult autoWaveByRound([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<WaveByRoundViewModel>(body.ToString());
                var result = service.AutoWaveByRound(Models.round_id,Models.planGoodsIssue_Due_Date,Models.goodsIssue_Index, Models.create_by);
                

                //if (result.goodsIssue_No != null)
                //{
                //    var saveresult = service.SaveAutoWaveByRound(result.goodsIssue_Index, result.resultMsg);

                //}

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region RunWaveByRound
        [HttpPost("RunWaveByRound")]
        public IActionResult RunWaveByRound([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<WaveByRoundViewModel>(body.ToString());
                var result = service.RunWaveByRound(Models);
                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region Update_pickingplan
        [HttpPost("Update_pickingplan")]
        public IActionResult Update_pickingplan([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<WaveByRoundViewModel>(body.ToString());
                var result = service.Update_pickingplan(Models);
                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CheckbyWave
        [HttpPost("CheckbyWave")]
        public IActionResult CheckbyWave([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<WaveByRoundViewModel>(body.ToString());
                var result = service.CheckbyWave(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion



        #region checkWaveWCS
        [HttpPost("checkWaveWCS")]
        public IActionResult checkWaveWCS([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<ResultRunWave>(body.ToString());
                var result = service.checkWaveWCS(Models);
                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion



        //[HttpPost("autoWaveByRoundThread")]
        //public IActionResult autoWaveByRoundThread([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new GoodIssueService();
        //        var Models = JsonConvert.DeserializeObject<WaveByRoundViewModel>(body.ToString());
        //        var result = service.AutoWaveByRoundThread(Models.round_id, Models.planGoodsIssue_Due_Date);


        //        //if (result.goodsIssue_No != null)
        //        //{
        //        //    var saveresult = service.SaveAutoWaveByRound(result.goodsIssue_Index, result.resultMsg);

        //        //}

        //        return Ok(result);


        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        [HttpPost("autoWaveByRound1")]
        public IActionResult autoWaveByRound1([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new WaveByRoundViewModel();

                var date = DateTime.Now.ToString("yyyyMMdd");
                Models.round_id = "01";
                Models.wave_Index = "7EFA474A-8FF4-439E-A5D7-8C844CB53B56";
                Models.planGoodsIssue_Due_Date = date;
                Models.goodsIssue_Index = "";
    
                var result = service.AutoWaveByRound(Models.round_id, Models.planGoodsIssue_Due_Date, Models.goodsIssue_Index, Models.create_by);

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoWaveByRound2")]
        public IActionResult autoWaveByRound2([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new WaveByRoundViewModel();

                var date = DateTime.Now.ToString("yyyyMMdd");
                Models.round_id = "02";
                Models.wave_Index = "7EFA474A-8FF4-439E-A5D7-8C844CB53B56";
                Models.planGoodsIssue_Due_Date = date;
                Models.goodsIssue_Index = "";

                var result = service.AutoWaveByRound(Models.round_id, Models.planGoodsIssue_Due_Date, Models.goodsIssue_Index, Models.create_by);

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("autoWaveByRound3")]
        public IActionResult autoWaveByRound3([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new WaveByRoundViewModel();

                var date = DateTime.Now.ToString("yyyyMMdd");
                Models.round_id = "03";
                Models.wave_Index = "7EFA474A-8FF4-439E-A5D7-8C844CB53B56";
                Models.planGoodsIssue_Due_Date = date;
                Models.goodsIssue_Index = "";

                var result = service.AutoWaveByRound(Models.round_id, Models.planGoodsIssue_Due_Date, Models.goodsIssue_Index, Models.create_by);

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("autoWaveByRound4")]
        public IActionResult autoWaveByRound4([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new WaveByRoundViewModel();

                var date = DateTime.Now.ToString("yyyyMMdd");
                Models.round_id = "04";
                Models.wave_Index = "7EFA474A-8FF4-439E-A5D7-8C844CB53B56";
                Models.planGoodsIssue_Due_Date = date;
                Models.goodsIssue_Index = "";

                var result = service.AutoWaveByRound(Models.round_id, Models.planGoodsIssue_Due_Date, Models.goodsIssue_Index, Models.create_by);

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpPost("autoWaveByRound5")]
        public IActionResult autoWaveByRound5([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new WaveByRoundViewModel();

                var date = DateTime.Now.ToString("yyyyMMdd");
                Models.round_id = "05";
                Models.wave_Index = "7EFA474A-8FF4-439E-A5D7-8C844CB53B56";
                Models.planGoodsIssue_Due_Date = date;
                Models.goodsIssue_Index = "";

                var result = service.AutoWaveByRound(Models.round_id, Models.planGoodsIssue_Due_Date, Models.goodsIssue_Index, Models.create_by);

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpPost("autoWaveByRound6")]
        public IActionResult autoWaveByRound6([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = new WaveByRoundViewModel();

                var date = DateTime.Now.ToString("yyyyMMdd");
                Models.round_id = "06";
                Models.wave_Index = "7EFA474A-8FF4-439E-A5D7-8C844CB53B56";
                Models.planGoodsIssue_Due_Date = date;
                Models.goodsIssue_Index = "";

                var result = service.AutoWaveByRound(Models.round_id, Models.planGoodsIssue_Due_Date, Models.goodsIssue_Index, Models.create_by);

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("autoTaskByRound")]
        public IActionResult autoTaskByRound([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<WaveByRoundViewModel>(body.ToString());
        
                    var result = service.SaveAutoWaveByRound(Models.goodsIssue_Index, "");


                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        //[HttpPost("CheckBom")]
        //public IActionResult CheckBom([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new GoodIssueService();
        //        var Models = JsonConvert.DeserializeObject<GoodsIssueViewModel>(body.ToString());

        //        var result = service.updateCheckBOM(Models);


        //        return Ok(result);


        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        #endregion


        #region StartWave
        [HttpPost("StartWave")]
        public IActionResult StartWave([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                GoodsIssueHeaderViewModel Models = JsonConvert.DeserializeObject<GoodsIssueHeaderViewModel>(body.ToString());
                Result result = service.Start_wave(Models);
                //result.resultIsUse = true;
                //result.resultMsg = "OK";
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion


        #region CloseWave
        [HttpPost("CloseWave")]
        public IActionResult CloseWave([FromBody]JObject body)
        {
            try
            {
                //var service = new TagoutService();
                GoodsIssueHeaderViewModel Models = JsonConvert.DeserializeObject<GoodsIssueHeaderViewModel>(body.ToString());
                //var result = service.maketagOut(Models);
                var result = new Result();
                result.resultIsUse = true;
                result.resultMsg = "OK";

                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region checkWaveWCS_location
        [HttpPost("checkWaveWCS_location")]
        public IActionResult checkWaveWCS_location([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueService();
                var Models = JsonConvert.DeserializeObject<ResultRunWave>(body.ToString());
                var result = service.checkWaveWCS_location(Models);
                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpPost("sendMsg")]
        public IActionResult sendMsg([FromBody]JObject body)
        {
            try
            {
                var service = new GoodIssueServiceWave();
             //   var Models = new sendMsgModel();
                var Models = JsonConvert.DeserializeObject<sendMsgModel>(body.ToString());


                var result = service.LineNotify(Models.msg);

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
