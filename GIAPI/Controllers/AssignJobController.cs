using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using GIBusiness.Reports;
using MasterBusiness.GoodsIssue;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GIAPI.Controllers
{
    [Route("api/AssignJob")]
    [ApiController]
    public class AssignJobController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public AssignJobController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
       

        #region Assign
        [HttpPost("assign")]
        public IActionResult autobasicSuggestion([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new AssignJobViewModel();
                Models = JsonConvert.DeserializeObject<AssignJobViewModel>(body.ToString());
                var result = service.assign(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region taskfilter
        [HttpPost("taskfilter")]
        public IActionResult taskfilter([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.taskfilter(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region confirmTask
        [HttpPost("confirmTask")]
        public IActionResult confirmTask([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.confirmTask(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region taskPopup
        [HttpPost("taskPopup")]
        public IActionResult taskPopup([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new GoodIssueViewModel();
                Models = JsonConvert.DeserializeObject<GoodIssueViewModel>(body.ToString());
                var result = service.taskPopup(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region autoGoodIssueNo
        [HttpPost("autoGoodIssueNo")]
        public IActionResult autoGoodIssueNo([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
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

        #region ReportPickingTag
        [HttpPost("PrintPickTag")]
        public IActionResult PrintPickTag([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new AssignService();
                var Models = JsonConvert.DeserializeObject<ReportPickingTagViewModel>(body.ToString());
                localFilePath = service.printReportPickingTag(Models, _hostingEnvironment.ContentRootPath);
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

        #region ReportPick
        [HttpPost("PrintPick")]
        public IActionResult PrintPick([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new AssignService();
                var Models = JsonConvert.DeserializeObject<ReportPickViewModel>(body.ToString());
                localFilePath = service.printReportPick(Models, _hostingEnvironment.ContentRootPath);
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

        #region PrintList
        [HttpPost("PrintList")]
        public IActionResult PrintList([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new AssignService();
                var Models = JsonConvert.DeserializeObject<ListReportPickingTagViewModel>(body.ToString());
                localFilePath = service.printListReportPickingTag(Models, _hostingEnvironment.ContentRootPath);
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
        #region deleteTask
        [HttpPost("deleteTask")]
        public IActionResult deleteTask([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.deleteTask(Models);
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
