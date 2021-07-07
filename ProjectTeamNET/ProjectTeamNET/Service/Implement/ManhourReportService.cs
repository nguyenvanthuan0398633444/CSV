using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Resources;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Implement
{
    public class ManhourReportService : IManhourReportService
    {
        private readonly IBaseRepository<UserScreenItem> userscreenRepository;
        private readonly ProjectDbContext dbContext;
        private readonly DbSet<UserScreenItem> dbUserScreenItems;
        private readonly DbSet<Models.Entity.Group> dbGroups;
        private readonly DbSet<User> dbUsers;
        private readonly DbSet<Theme> dbThemes;
        private readonly DbSet<Manhour> dbManhours;
        private readonly DbSet<WorkContents> dbWorkContents;

        private const string MANHOURREPORT = "工数集計";
        private const string FILE = ".csv";
        private const string SCREEN_URL = "ManhourReport";
        public ManhourReportService(IBaseRepository<UserScreenItem> userscreenRepository, ProjectDbContext dbContext)
        {
            dbUserScreenItems = dbContext.Set<UserScreenItem>();
            dbGroups = dbContext.Set<Models.Entity.Group>();
            dbUsers = dbContext.Set<User>();
            dbThemes = dbContext.Set<Theme>();
            dbManhours = dbContext.Set<Manhour>();
            dbWorkContents = dbContext.Set<WorkContents>();
            this.userscreenRepository = userscreenRepository;
            this.dbContext = dbContext;
        }

        public async Task<int> Delete(string id)
        {
            dbUserScreenItems.RemoveRange(dbUserScreenItems.Where(e => e.Surrogate_key.Contains(id)).ToList());
            return await dbContext.SaveChangesAsync();
        }

        public async Task<List<UserScreenItem>> GetsManHourReportSaved(string userScreenName)
        {
            var result = dbUserScreenItems.Where(e => e.Surrogate_key.Contains(userScreenName)).OrderBy(e => e.Surrogate_key);
            return await result.ToListAsync();
        }

        //get GroupName by GroupCode
        public async Task<string> GetsGroupName(string GroupCode)
        {
            var result = await (from gr in dbGroups
                                where gr.Group_code == GroupCode
                       select (gr.Group_code +"["+ gr.Group_name+"]")
                       ).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<GroupNames>> GetsGroupName()
        {
            var result = (from gr in dbGroups where gr.Del_flg == false
                          select ( new GroupNames()
                           {
                               GroupName = gr.Group_code + "[" + gr.Group_name + "  "+ gr.Accounting_group_name +"]",
                               group_code = gr.Group_code                           
                           }));
            return await result.ToListAsync();
        }

        //init has condition saved, theme and all group
        public ManhourReportViewModel Init(string userName)
        {
            var model = new ManhourReportViewModel();
            var Users = dbUserScreenItems.Where(e => e.Screen_url == SCREEN_URL && e.Save_name != "" && e.User_no == userName.ToUpper()).Select(s => new SelectListItem { Value = s.Surrogate_key.Remove(s.Surrogate_key.Length -3), Text = s.Save_name });
            model.Users = Users.AsEnumerable().GroupBy(x => x.Value).Select(x => x.First()).ToList();
            model.GroupName = (from th in dbGroups
                               select (new GroupNames()
                               {
                                   group_code = th.Group_code,
                                   GroupName = string.Concat(th.Group_code,"[",th.Group_name," ",th.Accounting_group_name,"]")
                               })).ToList();
            model.Themes = dbThemes.Select(s => new SelectListItem { Value = s.Theme_no, Text =$"{s.Theme_no}[{s.Theme_name1}]"}).ToList();
            return model;
        }

        // get Manhour(database) with condition in manhour screen
        public async Task<List<ManhourReport>> SetManhourReport(ManHourReportSearch data)
        {
            string[] groups = data.Groups.Split(",");
            string[] workContentCodes = data.workContentCodes.Split(",");
            string[] workContentDetails = new string[] { };
            if (string.IsNullOrEmpty(data.workContentDetails))
            {
                workContentDetails = new string[]{ };
            }
            else
            {
                workContentDetails = data.workContentDetails.Split(",");
            }
            string[] themes = data.themeNos.Split(",");
            string[] users = data.Users.Split(",");

            var toDate = DateTime.ParseExact(data.toDate.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var fromDate = DateTime.ParseExact(data.fromDate.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture);

            List<ManhourReport> manhourReports = new List<ManhourReport>() { };

            //check diffence between todate and fromDate to add suitable condition
            for (var countGroup = 1; countGroup < groups.Length; countGroup++)
            {
                List<string> AddList = new List<string>() { };
                if (toDate.Year == fromDate.Year)
                {
                    AddList.Add("SameYear");
                }
                else if (toDate.Year - fromDate.Year == 1)
                {
                    AddList.Add("More1YearDiffrence");
                }
                else if (toDate.Year - fromDate.Year > 1)
                {
                    AddList.Add("More2YearDiffrence");
                }
                for(var user = 1; user < users.Length; user++)
                {
                    for(var workContentD = 0; workContentD < workContentCodes.Length; workContentD++)
                    {
                        if (!string.IsNullOrEmpty(users[user]))
                        {
                            AddList.Add("Group_User");
                        }
                        if (workContentDetails.Length>0)
                        {
                            AddList.Add("Theme_WorkContent");
                        }
                        else
                        {
                            AddList.Add("Theme_WorkContent_nodetail");
                        }
                        var query = QueryLoader.GetQuery("ManhourReport", "queryManReport", AddList);
                        var param = new
                        {
                            toDay = toDate.Day,
                            fromDay = fromDate.Day,
                            toYear = toDate.Year,
                            fromYear = fromDate.Year,
                            toMonth = toDate.Month,
                            fromMonth = fromDate.Month,
                            user = users[user],
                            groupCode = groups[countGroup],
                            workThemeContent = workContentDetails.Length > 0 ? string.Concat(themes[workContentD].Split("[")[0], workContentCodes[workContentD], workContentDetails[workContentD]) :
                                                        string.Concat(themes[workContentD].Split("[")[0], workContentCodes[workContentD])
                        };
                        List<ManhourReport> manhouReport = await userscreenRepository.Select<ManhourReport>(query, param);
                        if(manhouReport.Count >0)
                            manhourReports.AddRange(manhouReport);
                    }
                }
            }
            var result = manhourReports.GroupBy(x => x.GroupCode).Select(x => x.First()).OrderBy(e => e.GroupCode).ToList();

            //add columns monthly and daily
            foreach(var manhourScr in result)
            {
                manhourScr.FromDate = fromDate;
                manhourScr.ToDate = toDate;
                manhourScr.Overalltotal = ManhourReportMonth(manhourScr.GroupCode, manhourScr.UserCode, manhourScr.ThemeCode, toDate, fromDate, manhourScr.WorkContentCode, manhourScr.WorkContentDetail);
                manhourScr.Monthly = ManhourReportMonthly(manhourScr.GroupCode, manhourScr.UserCode, manhourScr.ThemeCode, toDate, fromDate, manhourScr.WorkContentCode, manhourScr.WorkContentDetail);
                if((toDate - fromDate).TotalDays +1 <= 31)
                {
                    manhourScr.Daily = ManhourReportDaily(manhourScr.GroupCode, manhourScr.UserCode, manhourScr.ThemeCode, toDate, fromDate, manhourScr.WorkContentCode, manhourScr.WorkContentDetail);
                }
            }
            return result;
        }

        /// <summary>
        /// check validate (not saveName)
        /// </summary>
        /// <param name="data"></param>
        /// <returns>messager validate or not</returns>
        public IDictionary<string, string> ValidateReport(ManHourReportSearch data)
        {
            var messagers = new Dictionary<string, string>();
            messagers.Add("fromDate", "");
            messagers.Add("toDate", "");
            messagers.Add("date", "");
            messagers.Add("dateCalculate", "");
            messagers.Add("theme", "");
            messagers.Add("user", "");
            messagers.Add("headerItems", "");
            try
            {
                if (data.numberUser.Split(",").Contains("0"))
                {
                    messagers["user"] = Messages.ERR_025;
                }
                if (string.IsNullOrEmpty(data.selectedHeaderItems))
                {
                    messagers["headerItems"] = String.Format(Messages.ERR_024, "headerItems");
                }
                if (string.IsNullOrEmpty(data.themeNos))
                {
                    messagers["theme"] = String.Format(Messages.ERR_024, "Theme");
                }
                else if(data.themeNos.Split(",").Contains(""))
                {
                    messagers["theme"] = String.Format(Messages.ERR_024, "ThemeNo");
                }
                if (string.IsNullOrEmpty(data.fromDate))
                {
                    messagers["fromDate"] = String.Format(Messages.ERR_024, "fromDate");
                }
                else if (!Regex.IsMatch(data.fromDate, @"^\d{4}/((0\d)|(1[012]))/(([012]\d)|3[01])$"))
                {
                    messagers["fromDate"] = String.Format(Messages.ERR_020, "fromDate");
                }

                if (string.IsNullOrEmpty(data.toDate))
                {
                    messagers["toDate"] = String.Format(Messages.ERR_024, "toDate");
                }
                else if (!Regex.IsMatch(data.toDate, @"^\d{4}/((0\d)|(1[012]))/(([012]\d)|3[01])$"))
                {
                    messagers["toDate"] = String.Format(Messages.ERR_020, "toDate");
                }

                //check fromDate - toDate has many Days and ToDate much bigger FromDate
                if (messagers["fromDate"] == "" && messagers["toDate"] == "")
                {
                    if (!string.IsNullOrEmpty(data.fromDate) || !string.IsNullOrEmpty(data.toDate))
                    {
                        var fromDate = DateTime.ParseExact(data.fromDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        var toDate = DateTime.ParseExact(data.toDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        if (DateTime.Compare(toDate, fromDate) < 0)
                        {
                            messagers["date"] = Messages.ERR_012;
                        }
                        if (!string.IsNullOrEmpty(data.selectedHeaderItems))
                        {
                            if (data.selectedHeaderItems.Contains("dailytotal"))
                            {
                                if ((toDate - fromDate).TotalDays > 31)
                                {
                                    messagers["dateCalculate"] = Messages.ERR_013;
                                }
                            }
                        }
                    }
                }
                if (string.Concat(messagers["fromDate"],messagers["toDate"], messagers["headerItems"],
                    messagers["date"],messagers["dateCalculate"], messagers["user"], messagers["theme"]) != "")
                    return messagers;
                return new Dictionary<string, string>() { };
            }
            catch (Exception)
            {
                messagers["Error"] = "Something wrong! Please try later";
                return messagers;
            }
        }

        /// <summary>
        /// check validate
        /// </summary>
        /// <param name="data"></param>
        /// <returns>messager validate or not</returns>
        public IDictionary<string, string> ValidateSaveName(ManHourReportSearch data)
        {
            var messagers = new Dictionary<string, string>();
            messagers.Add("fromDate", "");
            messagers.Add("toDate", "");
            messagers.Add("savename", "");
            messagers.Add("date", "");
            messagers.Add("dateCalculate", "");
            messagers.Add("theme", "");
            messagers.Add("user", "");
            try
            {
                if (data.numberUser.Split(",").Contains("0"))
                {
                    messagers["user"] = Messages.ERR_025;
                }
                if (string.IsNullOrEmpty(data.themeNos))
                {
                    messagers["theme"] = String.Format(Messages.ERR_024, "Theme");
                }
                else if (data.themeNos.Split(",").Contains(""))
                {
                    messagers["theme"] = String.Format(Messages.ERR_024, "ThemeNo");
                }
                if (string.IsNullOrEmpty(data.Save) || data.Save.Length > 40)
                {
                    messagers["savename"] = Messages.ERR_026;
                }

                if (string.IsNullOrEmpty(data.fromDate))
                {
                    messagers["fromDate"] = String.Format(Messages.ERR_024, "fromDate");
                }
                else if (!Regex.IsMatch(data.fromDate, @"^\d{4}/((0\d)|(1[012]))/(([012]\d)|3[01])$"))
                {
                    messagers["fromDate"] = String.Format(Messages.ERR_020, "fromDate");
                }

                if (string.IsNullOrEmpty(data.toDate))
                {
                    messagers["toDate"] = String.Format(Messages.ERR_024, "toDate");
                }
                else if (!Regex.IsMatch(data.toDate, @"^\d{4}/((0\d)|(1[012]))/(([012]\d)|3[01])$"))
                {
                    messagers["toDate"] = String.Format(Messages.ERR_020, "toDate");
                }

                //check fromDate - toDate has many Days and ToDate much bigger FromDate
                if (messagers["fromDate"] == "" && messagers["toDate"] == "")
                {
                    if (!string.IsNullOrEmpty(data.fromDate) || !string.IsNullOrEmpty(data.toDate))
                    {
                        var fromDate = DateTime.ParseExact(data.fromDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        var toDate = DateTime.ParseExact(data.toDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        if (DateTime.Compare(toDate, fromDate) < 0)
                        {
                            messagers["date"] = Messages.ERR_012;
                        }
                        if (data.selectedHeaderItems.Contains("dailytotal"))
                        {
                            if ((toDate - fromDate).TotalDays > 31)
                            {
                                messagers["dateCalculate"] = Messages.ERR_013;
                            }
                        }
                    }
                }
                if (string.Concat(messagers["fromDate"], messagers["toDate"], messagers["savename"],
                    messagers["date"], messagers["dateCalculate"], messagers["user"], messagers["theme"]) != "")
                    return messagers;
                return new Dictionary<string, string>() { };
            }
            catch (Exception)
            {
                messagers["Error"] = "Something wrong! Please try later";
                return messagers;
            }
        }

        public Task<int> SaveManHourReport(List<Manhour> manhours)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ThemeName>> GetsThemeName()
        {
            var result = (from th in dbThemes
                       select (new ThemeName()
                       {
                           Theme_Name = th.Theme_no +"["+ th.Theme_name1 +"]",
                           ThemeCode = th.Theme_no
                       })
                        );
            return await result.ToListAsync();
        }

        public async Task<List<UserName>> GetsUserName(string GroupCode)
        {
            var result = (from un in dbUsers
                       where un.Group_code == GroupCode
                       select (new UserName()
                       {
                           UserCode = un.User_no,
                           User_Name = un.User_name
                       })
                       );
            return await result.ToListAsync();
        }

        public async Task<List<WorkContent>> GetsWorkContent(string ThemeNo)
        {
            var result = (from t in dbThemes where t.Theme_no == ThemeNo where t.Del_flg == false
                          join wc in dbWorkContents on t.Work_contents_class equals wc.Work_contents_class
                            select (new WorkContent()
                            {
                                WorkCode = wc.Work_contents_code,
                                Work_Content = wc.Work_contents_code +"["+ wc.Work_contents_code_name +"]"
                            }));
            return await result.ToListAsync();
        }

        // find hour daily from fromDate to toDate (if todate-from <= 31 days)
        public List<double> ManhourReportDaily(string group, string user, string theme, DateTime toDate, DateTime fromDate, string workContentCodes, string workContentDetails)
        {
            var manhourtoDate = dbManhours.Where(e => e.Year == toDate.Year
                            && e.Month == toDate.Month && e.Group_code == group && e.User_no == user && e.Theme_no == theme
                            && e.Work_contents_code == workContentCodes && e.Work_contents_detail == workContentDetails).FirstOrDefault(); 
            var manhourtoDate1 = dbManhours.Where(e => e.Year == toDate.Year
                             && e.Month == 2 && e.Group_code == group && e.User_no == user && e.Theme_no == theme
                             && e.Work_contents_code == workContentCodes && e.Work_contents_detail == workContentDetails).FirstOrDefault();
            var manhourfromDate = dbManhours.Where(e => e.Year == toDate.Year
                            && e.Month == toDate.Month && e.Group_code == group && e.User_no == user && e.Theme_no == theme
                            && e.Work_contents_code == workContentCodes && e.Work_contents_detail == workContentDetails).FirstOrDefault();
            var result = new List<Double>();
            var manHour = new Double[] { manhourfromDate.Day1,manhourfromDate.Day2, manhourfromDate.Day3, manhourfromDate.Day4, manhourfromDate.Day5, manhourfromDate.Day6, manhourfromDate.Day7, manhourfromDate.Day8, manhourfromDate.Day9, manhourfromDate.Day10,
                                manhourfromDate.Day11,manhourfromDate.Day12, manhourfromDate.Day13, manhourfromDate.Day14, manhourfromDate.Day15, manhourfromDate.Day16, manhourfromDate.Day17, manhourfromDate.Day18, manhourfromDate.Day19, manhourfromDate.Day20,
                                manhourfromDate.Day21,manhourfromDate.Day22, manhourfromDate.Day23, manhourfromDate.Day24, manhourfromDate.Day25, manhourfromDate.Day26, manhourfromDate.Day27, manhourfromDate.Day28, manhourfromDate.Day29, manhourfromDate.Day30, manhourfromDate.Day31};
            if(toDate.Month == fromDate.Month)
            {
                for (var i = fromDate.Day - 1; i < toDate.Day; i++)
                {
                    result.Add(manHour[i]);
                }
            }
            else
            {
                for (var i = fromDate.Day - 1; i < DateTime.DaysInMonth(fromDate.Year, fromDate.Month); i++)
                {
                    result.Add(manHour[i]);
                }
                if(manhourtoDate1 != null && toDate.Month - fromDate.Month > 1)
                {
                    manHour = new Double[] { manhourtoDate1.Day1,manhourtoDate1.Day2, manhourtoDate1.Day3, manhourtoDate1.Day4, manhourtoDate1.Day5, manhourtoDate1.Day6, manhourtoDate1.Day7, manhourtoDate1.Day8, manhourtoDate1.Day9, manhourtoDate1.Day10,
                            manhourtoDate1.Day11,manhourtoDate1.Day12, manhourtoDate1.Day13, manhourtoDate1.Day14, manhourtoDate1.Day15, manhourtoDate1.Day16, manhourtoDate1.Day17, manhourtoDate1.Day18, manhourtoDate1.Day19, manhourtoDate1.Day20,
                            manhourtoDate1.Day21,manhourtoDate1.Day22, manhourtoDate1.Day23, manhourtoDate1.Day24, manhourtoDate1.Day25, manhourtoDate1.Day26, manhourtoDate1.Day27, manhourtoDate1.Day28, manhourtoDate1.Day29, manhourtoDate1.Day30, manhourtoDate1.Day31};
                    for (var i = 0; i < DateTime.DaysInMonth(toDate.Year, toDate.Month - 1); i++)
                    {
                        result.Add(manHour[i]);
                    }
                }
                else if(manhourtoDate1 == null && toDate.Month - fromDate.Month > 1)
                {
                    for (var i = 0; i < DateTime.DaysInMonth(toDate.Year, toDate.Month); i++)
                    {
                        result.Add(0);
                    }
                }
                if (toDate.Month != fromDate.Month)
                {
                    manHour = new Double[] { manhourtoDate.Day1,manhourtoDate.Day2, manhourtoDate.Day3, manhourtoDate.Day4, manhourtoDate.Day5, manhourtoDate.Day6, manhourtoDate.Day7, manhourtoDate.Day8, manhourtoDate.Day9, manhourtoDate.Day10,
                            manhourtoDate.Day11,manhourtoDate.Day12, manhourtoDate.Day13, manhourtoDate.Day14, manhourtoDate.Day15, manhourtoDate.Day16, manhourtoDate.Day17, manhourtoDate.Day18, manhourtoDate.Day19, manhourtoDate.Day20,
                            manhourtoDate.Day21,manhourtoDate.Day22, manhourtoDate.Day23, manhourtoDate.Day24, manhourtoDate.Day25, manhourtoDate.Day26, manhourtoDate.Day27, manhourtoDate.Day28, manhourtoDate.Day29, manhourtoDate.Day30, manhourtoDate.Day31};
                    for (var i = 0; i < toDate.Day; i++)
                    {
                        result.Add(manHour[i]);
                    }
                }
                else
                {
                    for (var i = 0; i < toDate.Day; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        // get hour Monthly from fromDate to toDate
        public List<double> ManhourReportMonthly(string group, string user, string theme, DateTime toDate, DateTime fromDate, string workContentCodes, string workContentDetails)
        {
            var manhourList = dbManhours.Where(e => e.Group_code == group && e.User_no == user &&
                                    e.Theme_no == theme && (e.Year <= toDate.Year && e.Year >= fromDate.Year)
                                    && e.Work_contents_code == workContentCodes && e.Work_contents_detail == workContentDetails).ToList();
            var manhours = new List<Manhour>() {  };
            if (toDate.Year == fromDate.Year)
            {
                if (toDate.Month == fromDate.Month)
                {
                    manhours.AddRange(manhourList.Where(e => e.Month == toDate.Month));
                }
                else
                {
                    manhours.AddRange(manhourList.Where(e => e.Month <= toDate.Month && e.Month >= fromDate.Month));
                }
            }
            else if (toDate.Year == fromDate.Year + 1)
            {
                manhours.AddRange(manhourList.Where(e => e.Month <= 12 && e.Month >= fromDate.Month && e.Year == fromDate.Year));
                manhours.AddRange(manhourList.Where(e => e.Month <= toDate.Month && e.Month >= 1 && e.Year == toDate.Year));
            }
            else
            {
                manhours.AddRange(manhourList.Where(e => e.Month <= 12 && e.Month >= fromDate.Month && e.Year == fromDate.Year));
                manhours.AddRange(manhourList.Where(e => e.Year > fromDate.Year && e.Year < toDate.Year));
                manhours.AddRange(manhourList.Where(e => e.Month <= toDate.Month && e.Month >= 1 && e.Year == toDate.Year));
            }
            var MonthsSum = new Dictionary<string,Double>();

            //add manhour has data to manhourList Temporary
            foreach (var manhour in manhours)
            {
                Double sumMonth = 0;
               
                var manHour = new List<Double>();
                for (int i = 1; i <= 31; i++)
                {
                    manHour.Add(Double.Parse(manhour.GetType().GetProperty("Day" + i).GetValue(manhour).ToString()));
                }
                if (toDate.Month == fromDate.Month && toDate.Year == fromDate.Year)
                {
                    for (var i = fromDate.Day - 1; i < toDate.Day; i++)
                    {
                        sumMonth+= manHour[i];
                    }
                    return new List<double> { sumMonth };
                }
                else
                {
                    if (fromDate.Year == manhour.Year && manhour.Month == fromDate.Month)
                    {
                        for (var i = fromDate.Day - 1; i < 31; i++)
                        {
                            sumMonth += manHour[i];
                        }
                    }
                    else if (manhour.Month == toDate.Month)
                    {
                        for (var i = 0; i < toDate.Day; i++)
                        {
                            sumMonth += manHour[i];
                        }
                    }
                    else
                    {
                        sumMonth = manHour.Sum();
                    }
                }
                MonthsSum.Add($"{manhour.Month}/{manhour.Year}",sumMonth);
            }
            var month = fromDate.Month;
            var year = fromDate.Year;
            var result = new List<Double>();
            //add all manhour to manhourList
            for (var i = 0; i <= CommonFunction.GetMonthBetWeenTwoDate(fromDate, toDate); i++)
            {
                var check = true;
                foreach(var item in MonthsSum)
                {
                    if ($"{month}/{year}"== item.Key)
                    {
                        result.Add(item.Value);
                        check = false;
                        break;
                    }
                }
                if(check)
                    result.Add(0);
                if (12 == month)
                {
                    month = 1;year++;
                }
                else
                {
                    month++;
                }
            }
            return result;
        }

        // get hour total hour from fromDate to toDate
        public double ManhourReportMonth(string group, string user, string theme, DateTime toDate, DateTime fromDate, string workContentCodes, string workContentDetails)
        {
            var manhourList = dbManhours.Where(e => e.Group_code == group && e.User_no == user &&
                                    e.Theme_no == theme && (e.Year <= toDate.Year && e.Year >= fromDate.Year)
                                    && e.Work_contents_code == workContentCodes && e.Work_contents_detail == workContentDetails).ToList();
            var manhours = new List<Manhour>() { };
            if (toDate.Year == fromDate.Year)
            {
                if (toDate.Month == fromDate.Month)
                {
                    manhours.AddRange(manhourList.Where(e => e.Month == toDate.Month));
                }
                else
                {
                    manhours.AddRange(manhourList.Where(e => e.Month <= toDate.Month && e.Month >= fromDate.Month));
                }
            }
            else if (toDate.Year == fromDate.Year + 1)
            {
                manhours.AddRange(manhourList.Where(e => e.Month <= 12 && e.Month >= fromDate.Month && e.Year == fromDate.Year));
                manhours.AddRange(manhourList.Where(e => e.Month <= toDate.Month && e.Month >= 1 && e.Year == toDate.Year));
            }
            else
            {
                manhours.AddRange(manhourList.Where(e => e.Month <= 12 && e.Month >= fromDate.Month && e.Year == fromDate.Year));
                manhours.AddRange(manhourList.Where(e => e.Year > fromDate.Year && e.Year < toDate.Year));
                manhours.AddRange(manhourList.Where(e => e.Month <= toDate.Month && e.Month >= 1 && e.Year == toDate.Year));
            }
            double MonthsSum = 0;
            foreach (var manhour in manhours)
            {
                MonthsSum += manhour.Total;
            }
            return MonthsSum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>content Report and fileName</returns>
        public async Task<ExportModel> GetDataReportCSV(ManHourReportSearch data)
        {
            ExportModel exportModel = new ExportModel();
            DateTime today = DateTime.Now;

            var toDate = DateTime.ParseExact(data.toDate.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var fromDate = DateTime.ParseExact(data.fromDate.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture);

            string name = MANHOURREPORT + today.ToString("yyyyMMddHHmmss") + FILE;
            StringBuilder builder = new StringBuilder();
            List<ManhourReport> result = await SetManhourReport(data);
            int numbertd = 0;

            List<string> paramList = new List<string> { };

            //add itemheader selected to header (day and month)
            foreach(var select in data.selectedHeaderItems.Split(','))
            {
                if (select != "monthlytotal" && select != "dailytotal")
                {
                    if (select == "theme")
                    {
                        paramList.Add("テーマNo");
                        paramList.Add("テーマ名");
                    }
                    else if (select == "user")
                    {
                        paramList.Add("ユーザNo	");
                        paramList.Add("ユーザ名");
                    }
                    else if (select == "affiliation")
                    {
                        paramList.Add("所属コード");
                        paramList.Add("所属名");
                    }
                    else if(select == "workcontent")
                    {
                        paramList.Add("作業内容詳細");
                        paramList.Add("作業内容名");
                    }
                    else if(select == "detailworkcontent")
                        paramList.Add("作業内容コード");
                    else if(select == "overalltotal")
                    {
                        paramList.Add("工数合計");
                    }
                }
                if ((toDate - fromDate).TotalDays <= 31)
                {
                    if (select == "monthlytotal")
                    {
                        if (fromDate.Month == toDate.Month)
                        {
                            paramList.Add($"{fromDate.Month}月合計");
                        }
                        else
                        {
                            for (var i = fromDate.Month; i <= toDate.Month; i++)
                                paramList.Add($"{i}月合計");
                        }
                    }
                    if (select == "dailytotal")
                    {
                        if (fromDate.Month == toDate.Month)
                        {
                            for (var i = fromDate.Day; i <= toDate.Day; i++)
                                paramList.Add($"{i}/{ fromDate.Month}");
                        }
                        else
                        {
                            for (var i = fromDate.Day; i <= DateTime.DaysInMonth(fromDate.Year, fromDate.Month); i++)
                                paramList.Add($"{i}日合計");
                            if(toDate.Month- fromDate.Month > 1)
                            {
                                for(var i =1;i<= DateTime.DaysInMonth(fromDate.Year, 2); i++)
                                {
                                    paramList.Add($"{i}日合計");
                                }
                            }
                            for (var i = 1; i <= toDate.Day; i++)
                                paramList.Add($"{i}日合計");
                        }
                    }
                }
                else
                {
                    if (select == "monthlytotal")
                    {
                        if (fromDate.Year == toDate.Year)
                        {
                            for (var i = fromDate.Month; i <= toDate.Month; i++)
                                paramList.Add($"{i}月合計");
                        }
                        else if (fromDate.Year + 1 == toDate.Year)
                        {
                            for (var i = fromDate.Month; i <= 12; i++)
                                paramList.Add($"{i}月合計");
                            for (var i = 1; i <= toDate.Month; i++)
                                paramList.Add($"{i}月合計");
                        }
                        else
                        {
                            for (var i = fromDate.Month; i <= 12; i++)
                                paramList.Add($"{i}月合計");

                            int year = fromDate.Year + 1;

                            while (year < toDate.Year)
                            {
                                for (var i = 1; i <= 12; i++)
                                {
                                     paramList.Add($"{i}月合計");
                                }
                                year++;
                            }
                            for (var i = 1; i <= toDate.Month; i++)
                                paramList.Add($"{i}月合計");
                        }
                    }
                }
            }
            if(data.typeDelimiter == "1")
            {
                if(data.isSingleQuote == "1")
                {
                    builder.AppendLine(string.Join(",", paramList
                                            .Select(x => string.Format("'{0}'", x))));
                }
                else
                builder.AppendLine(string.Join(",", paramList));
            }
            else if(data.typeDelimiter == "0")
            {
                if (data.isSingleQuote == "1")
                {
                    builder.AppendLine(string.Join("    ", paramList
                                            .Select(x => string.Format("'{0}'", x))));
                }
                else
                    builder.AppendLine(string.Join("    ", paramList));
            }
            if(result.Count > 0)
            {
                //add content has found
                List<object> total = new List<object>() { "合計" };
                foreach (var manhourScr in result)
                {
                    numbertd = 0;
                    var content = new List<object>();
                    foreach (var select in data.selectedHeaderItems.Split(','))
                    {
                        if (select != "monthlytotal" && select != "dailytotal")
                        {
                            if (select == "theme")
                            {
                                content.Add(manhourScr.ThemeCode);
                                content.Add(manhourScr.ThemeName);
                                numbertd += 2;
                            }
                            else if (select == "user")
                            {
                                content.Add(manhourScr.UserCode);
                                content.Add(manhourScr.UserName);
                                numbertd += 2;
                            }
                            else if (select == "detailworkcontent")
                            {
                                numbertd++;
                                content.Add(manhourScr.WorkContentDetail);
                            }
                            else if (select == "workcontent")
                            {
                                content.Add(manhourScr.WorkContentCodeName);
                                content.Add(manhourScr.WorkContentCode);
                                numbertd += 2;
                            }
                            else if (select == "affiliation")
                            {
                                content.Add(manhourScr.GroupCode);
                                content.Add(manhourScr.GroupName);
                                numbertd += 2;
                            }
                            else if(select == "overalltotal")
                            {
                                content.Add(manhourScr.Overalltotal);
                            }
                        }
                        if ((toDate - fromDate).TotalDays <= 31)
                        {
                            if (select == "monthlytotal")
                            {
                                foreach (var i in manhourScr.Monthly)
                                {
                                    content.Add(i);
                                }
                            }
                            else if (select == "dailytotal")
                            {
                                foreach (var i in manhourScr.Daily)
                                {
                                    content.Add(i);
                                }
                            }
                        }
                        else
                        {
                            if (select == "monthlytotal")
                            {
                                foreach (var i in manhourScr.Monthly)
                                {
                                    content.Add(i);
                                }
                            }
                        }
                    }

                    //add row Total
                    if(data.isTotal == "1")
                    {
                        //add space ' '
                        for (var i = 1; i < numbertd; i++)
                        {
                            total.Add(null);
                        }
                        foreach (var valueItems in data.selectedHeaderItems.Split(','))
                        {
                            if (valueItems == "overalltotal")
                            {
                                double overralltotal = 0;
                                foreach (var manhourRp in result)
                                {
                                    overralltotal += manhourRp.Overalltotal;
                                }
                                total.Add(overralltotal);
                            }
                            if ((toDate - fromDate).TotalDays <= 31)
                            {
                                if (valueItems == "monthlytotal")
                                {
                                    for (var i = 0; i < manhourScr.Monthly.Count; i++)
                                    {
                                        double monthtotal = 0;
                                        foreach (var manhourRp in result)
                                        {
                                            var monthly = manhourRp.Monthly.ToArray();
                                            monthtotal += monthly[i];
                                        }
                                        total.Add(monthtotal);
                                    }
                                }
                                else if (valueItems == "dailytotal")
                                {
                                    for (var i = 0; i < manhourScr.Daily.Count; i++)
                                    {
                                        double daytotal = 0;
                                        foreach (var manhourRp in result)
                                        {
                                            var daily = manhourRp.Daily.ToArray();
                                            daytotal += daily[i];
                                        }
                                        total.Add(daytotal);
                                    }
                                }
                            }
                            else
                            {
                                if (valueItems == "monthlytotal")
                                {
                                    for (var i = 0; i < manhourScr.Monthly.Count; i++)
                                    {
                                        double monthtotal = 0;
                                        foreach (var manhourRp in result)
                                        {
                                            var monthly = manhourRp.Monthly.ToArray();
                                            monthtotal += monthly[i];
                                        }
                                        total.Add(monthtotal);
                                    }
                                }
                            }
                        }
                    }
                    //check typeDelimiter to add
                    if (data.typeDelimiter == "1")
                    {
                        if (data.isSingleQuote == "1")
                        {
                            builder.AppendLine(string.Join(",", content
                                                    .Select(x => string.Format("'{0}'", x))));
                        }
                        else
                            builder.AppendLine(string.Join(",", content));
                    }
                    else if (data.typeDelimiter == "0")
                    {
                        if (data.isSingleQuote == "1")
                        {
                            builder.AppendLine(string.Join("    ", content
                                                    .Select(x => string.Format("'{0}'", x))));
                        }
                        else
                            builder.AppendLine(string.Join("    ", content));
                    }
                }
                //add istotal to add
                if (data.isTotal == "1")
                {
                    if (data.typeDelimiter == "1")
                    {
                        if (data.isSingleQuote == "1")
                        {
                            builder.AppendLine(string.Join(",", total
                                                    .Select(x => string.Format("'{0}'", x))));
                        }
                        else
                        builder.AppendLine(string.Join(",", total));
                    }
                    else if (data.typeDelimiter == "0")
                    {
                        if (data.isSingleQuote == "1")
                        {
                            builder.AppendLine(string.Join("    ", total
                                                    .Select(x => string.Format("'{0}'", x))));
                        }
                        else
                            builder.AppendLine(string.Join("    ", total));
                    }
                }
                exportModel.builder = builder;
                exportModel.nameFile = name;
            }
            else
            {
                exportModel.builder = new StringBuilder() { };
                exportModel.nameFile = "";
            }
            return exportModel;

        }

        public Task<List<UserScreenItem>> Gets()
        {
            throw new NotImplementedException();
        }

        public Task<UserScreenItem> Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Create(UserScreenItem model)
        {
            var result = userscreenRepository.Create(model);
            return result;
        }

        public Task<int> Edit(UserScreenItem model)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// insert into list saveScreen has condition
        /// </summary>
        /// <param name="data"></param>
        /// <returns>alert string</returns>
        public string SaveScreen(ManHourReportSearch data, string userName)
        {
            try
            {
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                int count = 0;
                CreateReport(userName, data.Save, data.isTotal, "isTotal", date, ref count);
                CreateReport(userName, data.Save, data.typeDelimiter, "typeDelimiter", date, ref count);
                CreateReport(userName, data.Save, data.isSingleQuote, "isSingleQuote", date, ref count);
                CreateReport(userName, data.Save, data.numberSelectedHeader, "numberSelectedHeader", date, ref count);
                var selectedHeaders = data.selectedHeaderItems.Split(",");

                for (var i = 0; i < Int32.Parse(data.numberSelectedHeader); i++)
                {
                    CreateReport(userName, data.Save, selectedHeaders[i], string.Concat("selectedHeaderItem_", i), date, ref count);
                }

                CreateReport(userName, data.Save, data.fromDate, "fromDate", date, ref count);
                CreateReport(userName, data.Save, data.toDate, "toDate", date, ref count);
                CreateReport(userName, data.Save, data.numberTheme, "numberTheme", date, ref count);

                var themes = data.themeNos.Split(",");
                var workContentCodes = data.workContentCodes.Split(",");
                var workContentDetails = new string[] {""};
                if(data.workContentDetails=="")
                    workContentDetails = data.workContentDetails.Split(",");

                for (var i = 0; i < Int32.Parse(data.numberTheme); i++)
                {
                    CreateReport(userName, data.Save, themes[i], string.Concat("themeNo_", i), date, ref count);
                    CreateReport(userName, data.Save, workContentCodes[i], string.Concat("workContentCode_", i), date, ref count);
                    CreateReport(userName, data.Save, workContentDetails[i], string.Concat("workContentDetail_", i), date, ref count);
                }

                CreateReport(userName, data.Save, data.numberGroup, "numberGroup", date, ref count);

                var groups = data.Groups.Split(",");
                var users = data.Users.Split(",");
                var numberUsers = data.numberUser.Split(",");
                int countUser = 1;
                for (var i = 1; i <= Int32.Parse(data.numberGroup); i++)
                {
                    CreateReport(userName, data.Save, groups[i], string.Concat("group_", i), date, ref count);
                    CreateReport(userName, data.Save, numberUsers[i], string.Concat("numberUserGroup_", i), date, ref count);
                    for (var j = 1; j <= Int32.Parse(numberUsers[i]); j++)
                    {
                        CreateReport(userName, data.Save, users[countUser], string.Concat("group_", i, "_", j), date, ref count);
                        countUser++;
                    }
                }
                return Messages.INF_001;
            }
            catch (Exception)
            {
                return "error";
            }
            
        }
        /// <summary>
        /// insert into 1 screenUserItem 
        /// </summary>
        /// <param name="saveName"></param>
        /// <param name="screenInput"></param>
        /// <param name="screenItem"></param>
        /// <param name="date"></param>
        /// <param name="count"></param>
        public void CreateReport(string userName, string saveName, string screenInput,string screenItem, string date, ref int count)
        {
            UserScreenItem toDate = new UserScreenItem()
            {
                Surrogate_key = string.Concat(userName, date, count.ToString("000")),
                User_no = userName,
                Screen_url = SCREEN_URL,
                Screen_item = screenItem,
                Screen_input = screenInput,
                Save_name = saveName
            };
            Create(toDate);
            count++;
        }

        public List<SelectListItem> GetsScreen(string userName)
        {
            return dbUserScreenItems.Where(e => e.Screen_url == "ManhourReport" && e.Save_name != "" && e.User_no == userName.ToUpper()).Select(s => new SelectListItem { Value = s.Surrogate_key.Substring(0,18), Text = s.Save_name }).Distinct().ToList();
        }
    }
}
