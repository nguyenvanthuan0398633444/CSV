using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ProjectTeamNET.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace ProjectTeamNET.Controllers
{
    public class ManhourUpdateController : Controller
    {
        private readonly IWebHostEnvironment hosting;
        private readonly IManhourUpdateService manhourUpdateService;
        private const string user = "THUYHV";
        public ManhourUpdateController(IManhourUpdateService manhourUpdateService, IWebHostEnvironment hosting)
        {
            this.manhourUpdateService = manhourUpdateService;
            this.hosting = hosting;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // BAOTQ =1  , THUYHV, SSE802615 =2, SSE803243=0 
            
            ManhourUpdate model = await manhourUpdateService.GetGroupAndUser(user);
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index", "HOME");
        }

        [HttpPost("/ManhourUpdate/Search")]
        public async Task<JsonResult> Search(ManhourUpdateSearch keySearch)
        {
            ManHourUpdateSearchModel result = new ManHourUpdateSearchModel();
            result = await manhourUpdateService.Search(keySearch,user);
            return Json(new { data = result });
        }

        // get user in group
        [HttpPost("/ManhourUpdate/GetUser/{group}")]
        public JsonResult GetUser(string group)
        {
            ManhourUpdateUserSelectList user = manhourUpdateService.GetUser(group);
            return Json(new { data = user });
        }

        [HttpGet("/ManhourUpdate/ExportCSV")]
        public async Task<IActionResult> ExportCSV(string user, string group)
        {
            ExportModel exportModel = await manhourUpdateService.ExportCSV(user, group);
            return File(Encoding.UTF8.GetBytes(exportModel.builder.ToString()), "text/csv", exportModel.nameFile);
        }

        [HttpPost("/ManhourUpdate/ImportCSV")]
        public IActionResult ImportCSV(IFormFile file)
        {
            List<ManhourUpdateViewModel> result = manhourUpdateService.ImportCSV(file);
            return Json(new { data = result });

        }
        [HttpPost]
        public async Task<JsonResult> SearchThemes(SearchThemeParam param)
        {
            SelectThemeModel data = await manhourUpdateService.SearchThemes(param, user);

            if (data.Themes.Count >= 1000)
            {
                return Json(Resources.Messages.ERR_001);
            }
            return Json(data);
        }
        [HttpPost]
        public async Task<JsonResult> Save([FromBody] List<Manhour> saveData)
        {
            var result = false;
            if (result != true)
            {
                return Json("Can't Save!");
            }
            return Json("Save Successfuly!");

        }
    }
}
