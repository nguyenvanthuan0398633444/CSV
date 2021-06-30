using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Models.Entity;
using System.Threading.Tasks;
using ProjectTeamNET.Models.Response;
using System.Collections.Generic;
using ProjectTeamNET.Models.Request;

namespace ProjectTeamNET.Controllers
{
    /// <summary>
    /// Delete, edit, manage hour by Day, Month or Year
    /// </summary>
    public class ManhourInputController : Controller
    {
    
        string userDefault = "BAOTQ";
        string dateDefault = "2021/06/23"; 

        private readonly IManhourInputService _service;
        public ManhourInputController(IManhourInputService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Init(string dateSt)
        {
            InputParamModel pModel = new InputParamModel();
            pModel.userNo = userDefault;
            if (dateSt != null)
            {
                pModel.dateStr = dateSt;
            }
            else
            {
                pModel.dateStr = dateDefault;
            }

            InitDataModel data = await _service.Init(pModel);

            return View("Index", data);
        }
        /// <summary>
        /// return Day view
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Day(string dateSt)
        {
            InputParamModel pModel = new InputParamModel();
            pModel.userNo = userDefault;
            if (dateSt != null)
            {
                pModel.dateStr = dateSt;
            }
            else
            {
                pModel.dateStr = dateDefault;
            }

            InitDataModel data = await _service.Init(pModel);

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

            InputParamModel pModel = new InputParamModel();
            pModel.userNo = userDefault;
            if (dateSt != null)
            {
                pModel.dateStr = dateSt;
            }
            else
            {
                pModel.dateStr = dateDefault;
            }

            InitDataModel data = await _service.Init(pModel);

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
            InputParamModel pModel = new InputParamModel();
            pModel.userNo = userDefault;
            if (dateSt != null)
            {
                pModel.dateStr = dateSt;
            }
            else
            {
                pModel.dateStr = dateDefault;
            }
            
            InitDataModel data = await _service.Init(pModel);

            return View("Index", data);
        }

        /// <summary>
        /// Return json to ajax called this
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> LoadDatas(string dateStr)
        {
            InputParamModel pModel = new InputParamModel();
            pModel.userNo = userDefault;
            if (dateStr != null)
            {
                pModel.dateStr = dateStr;
            }
            ManhourInputModel data = await _service.GetManhourData(pModel);
            return Json(data);
        }
        /// <summary>
        /// save page history
        /// </summary>
        /// <param name="pageName"></param>
        [HttpGet]
        public OkObjectResult SavePageHistory(string pageName)
        {
            InputParamModel pModel = new InputParamModel();
            pModel.userNo = userDefault;
            pModel.pageName = pageName;
            _service.SavePageHistory(pModel);
            return Ok("Saved");
        }

        /// <summary>
        /// Get selected theme history from user screen item
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetHistoryThemes()
        {
            SearchThemeParam data = await _service.GetHistoryThemes(userDefault);
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
        public async Task<JsonResult> Save([FromBody]SaveData Data)
        {
            var result = await _service.Save(Data);
            if (result == false)
            {   
                return Json(string.Format(Resources.Messages.INF_001, "エラー"));
            }
            return Json(string.Format(Resources.Messages.INF_001,"更新"));

        }

    }
}
