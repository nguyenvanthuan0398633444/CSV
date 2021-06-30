using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Repository.Interface;
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
        private const string MANHOURREPORT = "工数集計";
        private const string FILE = ".csv";
        public ManhourReportService(IBaseRepository<UserScreenItem> userscreenRepository, ProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.userscreenRepository = userscreenRepository;
        }

        public async Task<int> Delete(string id)
        {
            return await userscreenRepository.Delete(id);
        }

        public async Task<List<UserScreenItem>> GetsManHourReportSaved(string userScreenName)
        {
            var result = dbContext.UserScreenItems.Where(e => e.Surrogate_key.Contains(userScreenName));
            return await result.ToListAsync();
        }

        //get GroupName by GroupCode
        public async Task<string> GetsGroupName(string GroupCode)
        {
            var result = await (from gr in dbContext.Groups
                                where gr.Group_code == GroupCode
                       select (gr.Group_code +"["+ gr.Group_name+"]")
                       ).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<GroupNames>> GetsGroupName()
        {
            var result = (from Gr in dbContext.Groups
                       select ( new GroupNames()
                       {
                           GroupName = Gr.Group_code + "[" + Gr.Group_name + "  "+ Gr.Accounting_group_name +"]",
                           group_code = Gr.Group_code                           
                       }));
            return await result.ToListAsync();
        }

        //init has condition saved and all group
        public async Task<ManhourReportViewModel> Init()
        {
            var model = new ManhourReportViewModel();
            model.Users = dbContext.UserScreenItems.Where(e=>e.Screen_url== "ManhourReport").Select(s => new SelectListItem { Value = s.Surrogate_key.Substring(0,18), Text = s.Save_name }).Distinct().ToList();
            model.GroupName = (from th in dbContext.Groups
                               select (new GroupNames()
                               {
                                   group_code = th.Group_code,
                                   GroupName = th.Group_code.ToString() + "[" + th.Group_name.ToString() + " " + th.Accounting_group_name.ToString() + "]"
                               })).ToList();
            return model;
        }

        // get Manhour(database) with condition in manhour screen
        public async Task<List<ManhourReport>> SetManhourReport(ManHourReportSearch data)
        {
            string[] groups = data.Groups.Split(',');
            string[] workContentCodes = data.workContentCodes.Split(',');
            string[] workContentDetails = data.workContentDetails.Split(',');

            var toDate = DateTime.ParseExact(data.toDate.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var fromDate = DateTime.ParseExact(data.fromDate.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture);

            var workContent = "";
            var groupContent = "";

            for(var i = 0; i < workContentCodes.Length; i++)
            {
                if(i==0)
                    workContent += workContentCodes[i] + workContentDetails[i];
                else
                    workContent += ","+ workContentCodes[i] + workContentDetails[i];

                for(var j=0;j<groups.Length; j++)
                {
                    if(groupContent=="")
                        groupContent += groups[j]+ workContentCodes[i] + workContentDetails[i];
                    else
                        groupContent += ","+groups[j] + workContentCodes[i] + workContentDetails[i];
                }
            }

            List<string> AddList = new List<string>();

            //check diffence between todate and fromDate to add suitable condition
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

            if (!string.IsNullOrEmpty(data.workContentCodes))
            {
                AddList.Add("WorkContent");
            }
            if (!string.IsNullOrEmpty(data.Groups))
            {
                AddList.Add("GroupContent");
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
                groupContent = groupContent,
                workContent = workContent
            };
            List<ManhourReport> result = await userscreenRepository.Select<ManhourReport>(query, param);
            //add columns monthly and daily
            foreach(var manhourScr in result)
            {
                manhourScr.FromDate = fromDate;
                manhourScr.ToDate = toDate;
                manhourScr.Monthly = ManhourReportMonthly(manhourScr.GroupCode, manhourScr.UserCode, manhourScr.ThemeCode, toDate, fromDate, manhourScr.WorkContentCode, manhourScr.WorkContentDetail);
                if((toDate - fromDate).TotalDays +1 <= 31)
                {
                    manhourScr.Daily = ManhourReportDaily(manhourScr.GroupCode, manhourScr.UserCode, manhourScr.ThemeCode, toDate, fromDate, manhourScr.WorkContentCode, manhourScr.WorkContentDetail);
                }
            }
            return result;
        }

        /// <summary>
        /// check validate
        /// </summary>
        /// <param name="data"></param>
        /// <returns>messager validate or not</returns>
        public IDictionary<string, string> ValidateReport(ManHourReportSearch data)
        {
            var messagers = new Dictionary<string, string>();
            messagers.Add("fromDate", "");
            messagers.Add("toDate", "");
            messagers.Add("savename", "");
            messagers.Add("Date", "");
            messagers.Add("DateCalculate", "");
            try
            {
                if (string.IsNullOrEmpty(data.Save)|| data.Save.Length > 40)
                {
                    messagers["savename"] = "Save Name is required and <= 40 characters";
                }

                if (string.IsNullOrEmpty(data.fromDate))
                {
                    messagers["fromDate"] = "FromDate is required";
                }
                else if (!Regex.IsMatch(data.fromDate, @"^\d{4}/((0\d)|(1[012]))/(([012]\d)|3[01])$"))
                {
                    messagers["fromDate"] = "FromDate not match format yyyy/MM/dd";
                }

                if (string.IsNullOrEmpty(data.toDate))
                {
                    messagers["toDate"] = "toDate is required";
                }
                else if (!Regex.IsMatch(data.toDate, @"^\d{4}/((0\d)|(1[012]))/(([012]\d)|3[01])$"))
                {
                    messagers["toDate"] = "ToDate not match format yyyy/MM/dd";
                }

                //check fromDate - toDate has many Days and ToDate much bigger FromDate
                if (messagers["fromDate"] == "" && messagers["toDate"] == "")
                {
                    if (!(data.fromDate == "" || data.fromDate == null) || (data.toDate == "" || data.toDate == null))
                    {
                        var fromDate = DateTime.ParseExact(data.fromDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        var toDate = DateTime.ParseExact(data.toDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        if (DateTime.Compare(toDate, fromDate) < 0)
                        {
                            messagers["Date"] = "ToDate much bigger FromDate";
                        }
                        if (data.selectedHeaderItems.Contains("dailytotal"))
                        {
                            if((toDate - fromDate).TotalDays > 31)
                            {
                                messagers["DateCalculate"] = "between 2 dates over 31 days";
                            }
                        }
                    }
                }
                if (messagers["fromDate"] != "" || messagers["toDate"] != "" || messagers["savename"] != "" || messagers["Date"] != "" || messagers["DateCalculate"] != "")
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
            var result = (from th in dbContext.Themes
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
            var result = (from un in dbContext.Users
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
            var result = (from t in dbContext.Themes where t.Theme_no == ThemeNo
                       join wc in dbContext.WorkContents on t.Work_contents_class equals wc.Work_contents_class
                       select (new WorkContent()
                       {
                           WorkCode = wc.Work_contents_code,
                           Work_Content = wc.Work_contents_code +"["+ wc.Work_contents_code_name +"]"
                       })
                       );
            return await result.ToListAsync();
        }

        // find hour daily from fromDate to toDate
        public List<double> ManhourReportDaily(string group, string user, string theme, DateTime toDate, DateTime fromDate, string workContentCodes, string workContentDetails)
        {
            var manhourtoDate = dbContext.Manhours.Where(e => e.Year == toDate.Year
                            && e.Month == toDate.Month && e.Group_code == group && e.User_no == user && e.Theme_no == theme
                            && e.Work_contents_code == workContentCodes && e.Work_contents_detail == workContentDetails).FirstOrDefault(); 
            var manhourtoDate1 = dbContext.Manhours.Where(e => e.Year == toDate.Year
                             && e.Month == toDate.Month && e.Group_code == group && e.User_no == user && e.Theme_no == theme
                             && e.Work_contents_code == workContentCodes && e.Work_contents_detail == workContentDetails);
            var manhourfromDate = dbContext.Manhours.Where(e => e.Year == toDate.Year
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
            var manhourList = dbContext.Manhours.Where(e => e.Group_code == group && e.User_no == user && 
                                    e.Theme_no == theme && (e.Year == toDate.Year|| e.Year == fromDate.Year)
                                    && e.Work_contents_code == workContentCodes && e.Work_contents_detail== workContentDetails).ToList();
            var MonthsSum = new Dictionary<string,Double>();
            var index = 1;
            foreach (var manhour in manhourList)
            {
                Double sumMonth = 0;
                var manHour = new Double[] { manhour.Day1,manhour.Day2, manhour.Day3, manhour.Day4, manhour.Day5, manhour.Day6, manhour.Day7, manhour.Day8, manhour.Day9, manhour.Day10,
                                    manhour.Day11,manhour.Day12, manhour.Day13, manhour.Day14, manhour.Day15, manhour.Day16, manhour.Day17, manhour.Day18, manhour.Day19, manhour.Day20,
                                    manhour.Day21,manhour.Day22, manhour.Day23, manhour.Day24, manhour.Day25, manhour.Day26, manhour.Day27, manhour.Day28, manhour.Day29, manhour.Day30, manhour.Day31};
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
                    if (index == 1 && manhour.Month == fromDate.Month)
                    {
                        for (var i = fromDate.Day - 1; i < 31; i++)
                        {
                            sumMonth += manHour[i];
                        }
                    }
                    else if (index == manhourList.Count() && manhour.Month == toDate.Month)
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
                    index++;
                }
                MonthsSum.Add($"{manhour.Month}/{manhour.Year}",sumMonth);
            }
            var month = fromDate.Month;
            var year = fromDate.Year;
            var result = new List<Double>();
            for(var i = 0; i <= CommonFunction.GetMonthBetWeenTwoDate(fromDate, toDate); i++)
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
                        paramList.Add("ThemeNo");
                        paramList.Add("ThemeName");
                    }
                    else if (select == "user")
                    {
                        paramList.Add("User");
                        paramList.Add("UserName");
                    }
                    else if (select == "affiliation")
                    {
                        paramList.Add("GroupCode");
                        paramList.Add("GroupName");
                    }
                    else if(select == "workcontent")
                    {
                        paramList.Add("WorkContentCodeName");
                        paramList.Add("WorkContentCode");
                    }
                    else if(select == "detailworkcontent")
                        paramList.Add(select);
                }
                if ((toDate - fromDate).TotalDays <= 31)
                {
                    if (select == "monthlytotal")
                    {
                        if (fromDate.Month == toDate.Month)
                        {
                            paramList.Add($"Thang{fromDate.Month}/{fromDate.Year}");
                        }
                        else
                        {
                            for (var i = fromDate.Month; i <= toDate.Month; i++)
                                paramList.Add($"Thang{i}/{fromDate.Year}");
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
                                paramList.Add($"{i}/{ fromDate.Month}");

                            for (var i = 1; i <= toDate.Day; i++)
                                paramList.Add($"{i}/{ toDate.Month}");
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
                                paramList.Add($"{i}/{ fromDate.Year}");
                        }
                        else if (fromDate.Year + 1 == toDate.Year)
                        {
                            for (var i = fromDate.Month; i <= 12; i++)
                                paramList.Add($"{i}/{ fromDate.Year}");
                            for (var i = 1; i <= toDate.Month; i++)
                                paramList.Add($"{i}/{ toDate.Year}");
                        }
                        else
                        {
                            for (var i = fromDate.Month; i <= 12; i++)
                                paramList.Add($"{i}/{ fromDate.Year}");
                            int tmp = fromDate.Year + 1;
                            while (tmp < toDate.Year)
                            {
                                for (var i = 1; i <= 12; i++)
                                {
                                     paramList.Add($"{i}/{ tmp}");
                                }
                                tmp++;
                            }
                            for (var i = 1; i <= toDate.Month; i++)
                                paramList.Add($"{i}/{ toDate.Year}");
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
            //add content has found
            List<object> total = new List<object>() { "Tong"};
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
                        else
                        {
                            if (select == "dailytotal")
                            {
                                foreach (var i in manhourScr.Daily)
                                {
                                    content.Add(i);
                                }
                            }
                        }
                    }
                }

                //add row Total
                if(data.isTotal == "1")
                {
                    for (var i = 1; i < numbertd; i++)
                    {
                        total.Add(null);
                    }
                    foreach (var valueItems in data.selectedHeaderItems.Split(','))
                    {
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

        public Task<int> Create(UserScreenItem model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Edit(UserScreenItem model)
        {
            throw new NotImplementedException();
        }
    }
}
