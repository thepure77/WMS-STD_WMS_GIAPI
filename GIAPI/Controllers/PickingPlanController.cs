using System;
using System.Net;
using System.Net.Http;
using GIBusiness.GoodIssue;
using GIBusiness.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GIAPI.Controllers
{
    [Route("api/PickingPlan")]
    [ApiController]
    public class PickingPlanController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public PickingPlanController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("ExportExcel")]
        public IActionResult ExportExcel([FromBody]JObject body)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            string StockMovementPath = "";
            try
            {
                PickingPlanService _appService = new PickingPlanService();
                var Models = new PickingPlanViewModel();
                Models = JsonConvert.DeserializeObject<PickingPlanViewModel>(body.ToString());
                StockMovementPath = _appService.ExportExcel(Models, _hostingEnvironment.ContentRootPath);

                if (!System.IO.File.Exists(StockMovementPath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(StockMovementPath), "application/octet-stream");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                System.IO.File.Delete(StockMovementPath);
            }
        }

    }
}