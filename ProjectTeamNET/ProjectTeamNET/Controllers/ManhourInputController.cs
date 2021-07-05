using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Models.Entity;
using System.Threading.Tasks;
using ProjectTeamNET.Models.Response;
using System.Collections.Generic;
using ProjectTeamNET.Models.Request;
using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ProjectTeamNET.Controllers
{
    /// <summary>
    /// Delete, edit, manage hour by Day, Month or Year
    /// </summary>
    public class ManhourInputController : Controller
    {
        readonly string localDate = DateTime.Now.ToString("yyyy/MM/dd");
        private readonly IManhourInputService _service;
        public ManhourInputController(IManhourInputService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string dateSt)
        {
            if (HttpContext.Session.GetString("userNo")==null)
            {
                return RedirectToAction("Index", "Login");
            }
            InputParamModel pModel = new InputParamModel
            {
                UserNo = HttpContext.Session.GetString("userNo")
            };
            if (dateSt != null)
            {
                pModel.DateStr = dateSt;
            }
            else
            {
                pModel.DateStr = localDate;
            }
           
            InitDataModel data = await _service.Init(pModel);

            return View("Index", data);
        }
        /// <summary>
        /// return manhour day view
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Day(string dateSt)
        {
            if (HttpContext.Session.GetString("userNo") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            InputParamModel pModel = new InputParamModel
            {
                UserNo = HttpContext.Session.GetString("userNo")
            };
            if (dateSt != null)
            {
                pModel.DateStr = dateSt;
            }
            else
            {
                pModel.DateStr = localDate;
            }

            InitDataModel data = await _service.Init(pModel);
            data.pageHistory = "Day";

            return View("Index", data);
        }
        /// <summary>
        /// return manhour week view 
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Week(string dateSt)
        {

            if (HttpContext.Session.GetString("userNo") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            InputParamModel pModel = new InputParamModel
            {
                UserNo = HttpContext.Session.GetString("userNo")
            };
            if (dateSt != null)
            {
                pModel.DateStr = dateSt;
            }
            else
            {
                pModel.DateStr = localDate;
            }

            InitDataModel data = await _service.Init(pModel);
            return View("Index", data);

        }
        /// <summary>
        /// return manhour month view
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Month(string dateSt)
        {
            if (HttpContext.Session.GetString("userNo") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            InputParamModel pModel = new InputParamModel
            {
                UserNo = HttpContext.Session.GetString("userNo")
            };
            if (dateSt != null)
            {
                pModel.DateStr = dateSt;
            }
            else
            {
                pModel.DateStr = localDate;
            }
            
            InitDataModel data = await _service.Init(pModel);

            return View("Index", data);
        }
        /// <summary>
        /// Return manhour data  to ajax 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> LoadDatas(string dateStr)
        {
            InputParamModel pModel = new InputParamModel();
            pModel.UserNo = HttpContext.Session.GetString("userNo");
            pModel.SiteCode = HttpContext.Session.GetString("siteCode");
            if (dateStr != null)
            {
                pModel.DateStr = dateStr;
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
            pModel.UserNo = HttpContext.Session.GetString("userNo");
            pModel.PageName = pageName;
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
            string userNo = HttpContext.Session.GetString("userNo");
            SearchThemeParam data = await _service.GetHistoryThemes(userNo);
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
            string userNo = HttpContext.Session.GetString("userNo");
            SelectThemeModel data = await _service.SearchThemes(param,userNo);

            if(data.Themes.Count >= 1000)
            {
                return Json(Resources.Messages.ERR_001);
            }
            return Json(data);
        }
        /// <summary>
        /// Save data from client 
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Save([FromBody]SaveData Data)
        {
            UserInfo user = new UserInfo
            {
                User_no = HttpContext.Session.GetString("userNo"),
                Group_code = HttpContext.Session.GetString("groupCode"),
                Site_code = HttpContext.Session.GetString("siteCode")
            };

            var result = await _service.Save(Data,user);
            if (result == false)
            {   
                return Json(string.Format(Resources.Messages.INF_001, "エラー"));
            }
            return Json(string.Format(Resources.Messages.INF_001,"更新"));

        }
        /// <summary>
        /// return work contents by class
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetWorkContentByClass(string classCode)
        {
            var result = await _service.GetWorkContentsByClass(classCode);
            return Json(result);

        }
        /// <summary>
        /// Export csv file by format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ExportCSV(string dateStr)
        {
            string userNo = HttpContext.Session.GetString("userNo");
            ExportModel exportModel = await _service.ExportCSV(userNo, dateStr);
            return File(Encoding.UTF8.GetBytes(exportModel.builder.ToString()), "text/csv", exportModel.nameFile);
        }

        [HttpPost]
        public async Task<IActionResult> CheckExistTheme(ManhourKeys keys)
        {
            string userNo = HttpContext.Session.GetString("userNo");
            keys.User_no = userNo;
            return Json(await _service.CheckThemeExist(keys));
        }

    }
}
