using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBusiness.GoodIssue;
using GIBusiness.TagOut;
using MasterBusiness.GoodsIssue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GIAPI.Controllers
{
    [Route("api/TagOut")]
    [ApiController]
    public class TagOutController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public TagOutController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region tagOutfilter
        [HttpPost("tagOutfilter")]
        public IActionResult tagOutfilter([FromBody]JObject body)
        {
            try
            {
                var service = new TagOutService();
                var Models = new TagOutfilterViewModel();
                Models = JsonConvert.DeserializeObject<TagOutfilterViewModel>(body.ToString());
                var result = service.tagOutfilter(Models);
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
                var service = new TagOutService();
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

        #region tagOutPopup
        [HttpPost("tagOutPopup")]
        public IActionResult tagOutPopup([FromBody]JObject body)
        {
            try
            {
                var service = new TagOutService();
                var Models = new GoodIssueViewModel();
                Models = JsonConvert.DeserializeObject<GoodIssueViewModel>(body.ToString());
                var result = service.tagOutPopup(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region printtote
        [HttpPost("printtote")]
        public IActionResult printtote([FromBody]JObject body)
        {
            try
            {
                var service = new TagOutService();
                var Models = new TagViewModel();
                Models = JsonConvert.DeserializeObject<TagViewModel>(body.ToString());
                var result = service.printtote(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
            
        }
        #endregion

        [HttpPost("ReportTagout")]
        public IActionResult ReportTagout([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new TagOutService();
                var Models = new TagViewModel();
                Models = JsonConvert.DeserializeObject<TagViewModel>(body.ToString());
                localFilePath = service.ReportTagout(Models, _hostingEnvironment.ContentRootPath);
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


        [HttpPost("Savetag")]
        public IActionResult Savetagout([FromBody]JObject body)
        {
            try
            {
                var service = new TagOutService();
                var Models = new TagOutfilterViewModel();
                Models = JsonConvert.DeserializeObject<TagOutfilterViewModel>(body.ToString());
                var result = service.Savetagout(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost("Deletetag")]
        public IActionResult Deletetag([FromBody]JObject body)
        {
            try
            {
                var service = new TagOutService();
                var Models = new TagOutfilterViewModel();
                Models = JsonConvert.DeserializeObject<TagOutfilterViewModel>(body.ToString());
                var result = service.Deletetag(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}