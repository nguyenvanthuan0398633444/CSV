using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Models.Entity;
using System.Threading.Tasks;
using ProjectTeamNET.Models.Response;
using System.Collections.Generic;

namespace ProjectTeamNET.Controllers
{
    /// <summary>
    /// Delete, edit, manage hour by Day, Month or Year
    /// </summary>
    public class ManhourInputController : Controller
    {
    
        string userDefault = "CANHVN";
        string paramSt = "2021/06/23" + ";" + "CANHVN";

        private readonly IManhourInputService _service;
        public ManhourInputController(IManhourInputService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Init(string dateSt)
        {
            if (dateSt != null)
            {
                paramSt = dateSt + ";" + userDefault;
            }

            InitDataModel data = await _service.Init(paramSt);

            return View("Index",data);
        }
        /// <summary>
        /// return Day view
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Day(string dateSt)
        {
            if (dateSt != null)
            {
                paramSt = dateSt + ";" + userDefault;
            }

            InitDataModel data = await _service.Init(paramSt);

            return View("Index", data);
        }
        /// <summary>
        /// return week view 
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Week(string dateSt)
        {

            if (dateSt != null)
            {
                paramSt = dateSt + ";" + userDefault;
            }

            InitDataModel data = await _service.Init(paramSt);

            return View("Index", data);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Month(string dateSt)
        {
            if (dateSt != null)
            {
                paramSt = dateSt + ";" + userDefault;
            }

            InitDataModel data = await _service.Init(paramSt);
                
            return View("Index", data);
        }

        /// <summary>
        /// Return json to ajax called this
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> LoadDatas(string dateSt)
        {
            ManhourInputModel data = await _service.GetManhourData(dateSt + ";" + userDefault );
            return Json(data);
        }

        /// <summary>
        /// Get selected theme history from user screen item
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetHistoryThemes()
        {
            SelectThemeModel data = await _service.GetHistoryThemes(userDefault);
            return Json(data);
        }

        /// <summary>
        /// Search theme by param 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SearchThemes(SearchThemeParam param)
        {
            SelectThemeModel data = await _service.SearchThemes(param,userDefault);

            if(data.Themes.Count >= 1000)
            {
                return Json(Resources.Messages.ERR_001);
            }
            return Json(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Save([FromBody]List<Manhour> saveData)
        {
            var result = await _service.Save(saveData);
            if (result != true)
            {
                return Json("Can't Save!");
            }
            return Json("Save Successfuly!");

        }

    }
}
