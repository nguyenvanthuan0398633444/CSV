using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var model = await manhourReportService.Init(HttpContext.Session.GetString("userName").ToUpper());
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
            var result = manhourReportService.ValidateReport(data);
            return Ok(result);
        }
        [HttpPost("/ManhourReport/CheckSave")]
        public OkObjectResult CheckSave(ManHourReportSearch data)
        {
            var result = manhourReportService.ValidateSaveName(data);
            return Ok(result);
        }

        [HttpPost("/ManhourReport/GetDataCSV")]
        public async Task<IActionResult> GetDataCSV(ManHourReportSearch data)
        {
            var exportModel = await manhourReportService.GetDataReportCSV(data);
            if (exportModel.nameFile == "")
            {
                return Ok(new { messenge = Resources.Messages.ERR_005 });
            }
            return Ok(new { data = exportModel.builder.ToString(), fileName = exportModel.nameFile, messenge = ""});
        }
        [HttpPost("/ManhourReport/SaveScreen")]
        public async Task<IActionResult> SaveScreen(ManHourReportSearch data)
        {
            var result = await manhourReportService.SaveScreen(data, HttpContext.Session.GetString("userName").ToUpper());
            return Ok(result);
        }
        [HttpGet("/ManhourReport/GetsScreen")]
        public async Task<IActionResult> GetsScreen()
        {
            var result = await manhourReportService.GetsScreen(HttpContext.Session.GetString("userName").ToUpper());
            return Ok(result);
        }

        [HttpPost("/ManhourReport/CreateData")]
        public async Task<OkObjectResult> CreateData(ManHourReportSearch data)
        {
            var result = await manhourReportService.SetManhourReport(data);
            if(result.Count >0)
                return Ok(new {data = result,messenge="" });
            return Ok(new { messenge = Resources.Messages.ERR_005 });
        }

        public IActionResult ShowReport()
        {
            return View();
        }

        [HttpDelete("/ManhourReport/DeleteScreenUser/{surrogate}")]
        public async Task<OkObjectResult> DeleteScreenUser(string surrogate)
        {
            var result = await manhourReportService.Delete(surrogate);
            if(result > 0)
            {
                return Ok(Resources.Messages.INF_001);
            }
            return Ok("Error");
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
