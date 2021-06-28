using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Interface
{
    public interface IManhourUpdateService 
    {
        Task<List<ManhourUpdateViewModel>> GetDataSearch(ManhourUpdateSearch keySearch);
        Task<string> GetRole(string funtioncClass);
        Task<List<UserScreenItem>> GetHistorySearch(string funtioncClass);
        Task<ManhourUpdate> GetGroupAndUser(string userId);
        Task<ExportModel> ExportCSV(string user, string group);
        List<ManhourUpdateViewModel> ImportCSV(IFormFile files);
        ManhourUpdateUserSelectList GetUser(string groupId);   
        Task<List<int>> GetHoliday(ManhourUpdateSearch keySearch);
        Task<ManHourUpdateSearchModel> Search(ManhourUpdateSearch keySearch, string userId);
        Task<SelectThemeModel> SearchThemes(SearchThemeParam param, string user_no);


    }
}
