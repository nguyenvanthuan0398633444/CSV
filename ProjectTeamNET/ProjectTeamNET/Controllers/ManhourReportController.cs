using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectTeamNET.Controllers
{
    public class ManhourReportController : Controller
    {
        private readonly IManhourReportService manhourReportService;

        public ManhourReportController(IManhourReportService manhourReportService)
        {
            this.manhourReportService = manhourReportService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await manhourReportService.Init();
            return View(model);
        }
        [HttpGet("/ManhourReport/AddTheme/{count}")]
        public IActionResult AddTheme(int count)
        {
            var result = PartialView("AddTheme", count);
            return result;
        }

        [HttpPost("/ManhourReport/CheckReport")]
        public OkObjectResult CheckReport(ManHourReportSearch data)
        {
            var tmp = manhourReportService.ValidateReport(data);

            return Ok(tmp);
        }

        [HttpPost("/ManhourReport/GetDataCSV")]
        public async Task<IActionResult> GetDataCSV(ManHourReportSearch data)
        {
            var exportModel = await manhourReportService.GetDataReportCSV(data);

            return Ok(new {data= exportModel.builder.ToString(),fileName= exportModel.nameFile });
        }

        [HttpPost("/ManhourReport/CreateData")]
        public async Task<OkObjectResult> CreateData(ManHourReportSearch data)
        {
            var draw = await manhourReportService.SetManhourReport(data);
            return Ok(draw);
        }

        public IActionResult ShowReport()
        {
            return View();
        }

        [HttpDelete("/ManhourReport/DeleteScreenUser/{surrogate}")]
        public async Task<OkObjectResult> DeleteScreenUser(string surrogate)
        {
            var result = await manhourReportService.Delete(surrogate);
            return Ok(result);
        }

        [HttpGet("/ManhourReport/GetsUserName/{GroupCode}")]
        public async Task<OkObjectResult> GetsUserName(string GroupCode)
        {
            var result = await manhourReportService.GetsUserName(GroupCode);
            return Ok(result);
        }

        [HttpGet("/ManhourReport/GetsGroupName/{GroupCode}")]
        public async Task<OkObjectResult> GetsGroupName(string GroupCode)
        {
            var result = await manhourReportService.GetsGroupName(GroupCode);
            return Ok(result);
        }

        [HttpGet("/ManhourReport/GetsThemeName")]
        public async Task<OkObjectResult> GetsThemeName()
        {
            var result = await manhourReportService.GetsThemeName();
            return Ok(result);
        }

        [HttpGet("/ManhourReport/GetsGroupName")]
        public async Task<OkObjectResult> GetsGroupName()
        {
            var result = await manhourReportService.GetsGroupName();
            return Ok(result);
        }

        [HttpGet("/ManhourReport/WorkContents/{themeNo}")]
        public async Task<OkObjectResult> WorkContents(string themeNo)
        {
            var result = await manhourReportService.GetsWorkContent(themeNo);
            return Ok(result);
        }

        [HttpGet("/ManhourReport/GetManhourReport/{surrogatekey}")]
        public async Task<OkObjectResult> GetManhourReport(string surrogatekey)
        {
            var result = await manhourReportService.GetsManHourReportSaved(surrogatekey);
            return Ok(result);
        }
    }
}
