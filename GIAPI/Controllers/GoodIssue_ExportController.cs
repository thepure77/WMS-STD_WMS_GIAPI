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
    [Route("api/GoodIssue_Export")]
    [ApiController]
    public class GoodIssue_ExportController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GoodIssue_ExportController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

    }
}
