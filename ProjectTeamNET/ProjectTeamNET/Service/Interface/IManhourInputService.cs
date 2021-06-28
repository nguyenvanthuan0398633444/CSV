using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManhourInputService
    {   
        /// <summary>
        /// Get manhour data for table
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<ManhourInputModel> GetManhourData(string data);
        /// <summary>
        /// Init data form db
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<InitDataModel> Init(string data);
        Task<List<Group>> GetGroups();
        Task<List<WorkContents>> GetWorkContents();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<List<ManhourInput>> GetManhours(string paramSt);

        /// <summary>
        /// Get Screen item input history
        /// </summary>
        /// <param name="user_no"></param>
        /// <param name="screen_url">String of screen url need get history</param>
        /// <returns></returns>
        Task<List<UserScreenItem>> GetsScreenItemHistory(string user_no,string screen_url);

        /// <summary>
        /// Get pined theme in last month to check to insert for now month
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<List<ManhourInput>> GetPinTheme(string data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        Task<SelectThemeModel> GetHistoryThemes(string user_no);
        /// <summary>
        /// Search Theme by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SelectThemeModel> SearchThemes(SearchThemeParam param, string user_no);

        /// <summary>
        /// Handle save data from client 
        /// </summary>
        /// <param name="saveDatas"></param>
        /// <returns></returns>
        Task<bool> Save(List<Manhour> saveDatas);
        /// <summary>
        /// Get list date of week 
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        List<string> GetListDayOfWeek(DateTime date);
        /// <summary>
        /// update manhour data form client 
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        Task<int> UpdateManhours(List<Manhour> manhours);
        /// <summary>
        /// delete data if it deleted in data form client
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        Task<int> DeleteManhours(List<Manhour> manhours);

        //Task<int> CreateManhours(List<Manhour> manhours);
    }
}
