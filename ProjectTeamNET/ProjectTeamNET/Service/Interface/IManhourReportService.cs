using m_user_screen_item;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using t_manhour;

namespace ProjectTeamNET.Service.Interface
{
    public interface IManhourReportService : IBaseService<UserScreenItem>
    {
        Task<ManhourReportViewModel> Init();
        Task<int> Delete(string id);
        Task<List<UserName>> GetsUserName(string groupCode);
        Task<string> GetsGroupName(string groupCode);
        Task<List<ThemeName>> GetsThemeName();
        Task<List<GroupNames>> GetsGroupName();
        Task<List<WorkContent>> GetsWorkContent(string ThemeNo);
        Task<int> SaveManHourReport(List<Manhour> manhours);
        Task<List<UserScreenItem>> GetsManHourReportSaved(string userScreenName);
        IDictionary<string, string> ValidateReport(ManHourReportSearch data);
        Task<List<ManhourReport>> SetManhourReport(ManHourReportSearch data);
        List<double> ManhourReportMonthly(string group, string user, string theme, DateTime toDate, DateTime fromDate, string workContentCodes, string workContentDetails);
        List<double> ManhourReportDaily(string group, string user, string theme, DateTime toDate, DateTime fromDate, string workContentCodes, string workContentDetails);
        Task<ExportModel> GetDataReportCSV(ManHourReportSearch data);
    }
}
