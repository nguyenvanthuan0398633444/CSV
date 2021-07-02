using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using ProjectTeamNET.Models.Entity;
using Microsoft.AspNetCore.Http;
namespace ProjectTeamNET.Controllers
{
    public class ManhourUpdateController : Controller
    {
        private readonly IManhourUpdateService manhourUpdateService;
        private const string user = "BAOTQ";
        public ManhourUpdateController(IManhourUpdateService manhourUpdateService)
        {
            this.manhourUpdateService = manhourUpdateService;           
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {            
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
            ManhourUpdateUserSelectList user = manhourUpdateService.GetUserInGroup(group);
            return Json(new { data = user });
        }

        [HttpGet("/ManhourUpdate/ExportCSV")]
        public async Task<IActionResult> ExportCSV(string user, string group)
        {
            ExportModel exportModel = await manhourUpdateService.ExportCSV(user, group);
            return File(Encoding.UTF8.GetBytes(exportModel.builder.ToString()), "text/csv", exportModel.nameFile);
        }

        [HttpPost("/ManhourUpdate/ImportCSV")]
        public async Task<IActionResult> ImportCSV(IFormFile file)
        {
            List<Manhour> result = await manhourUpdateService.ImportCSV(file);
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

        [HttpPost("/ManhourUpdate/Save")]
        public Task<JsonResult> Save([FromBody] ManhourUpdateSave saveData)
        {
            if(saveData != null)
            {
                manhourUpdateService.Save(saveData);
                return Task.FromResult(Json("Save Successfuly!"));
            }
            return Task.FromResult(Json("Save Error"));

        }
    }
}
