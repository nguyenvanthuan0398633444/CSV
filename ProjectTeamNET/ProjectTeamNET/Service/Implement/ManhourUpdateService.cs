using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Response;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ProjectTeamNET.Service.Implement
{
    public class ManhourUpdateService : IManhourUpdateService
    {
        private readonly IBaseRepository<WorkContents> workContentRepository;
        private readonly IBaseRepository<Manhour> manhourRepository;
        private readonly IBaseRepository<Group> groupRepository;
        private readonly IBaseRepository<SalesObject> saleRepository;
        private readonly IBaseRepository<Theme> themeRepository;
        private readonly IBaseRepository<UserScreenItem> screenRepository;

        private readonly IBaseRepository<Manhour> manhourUpdateReponsitory;
        private readonly ProjectDbContext context;
        private readonly IWebHostEnvironment hosting;
        private readonly IBaseRepository<UserScreenItem> userScreenItemRepository;
        const string header = "年,月,ユーザNo,ユーザ名,テーマＮｏ,テーマ名,内容コード,内容名,内容詳細コード,合計," +
                        "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31";
        public ManhourUpdateService(IBaseRepository<Manhour> manhourUpdateReponsitory, 
                                    ProjectDbContext context, IWebHostEnvironment hosting,
                                    IBaseRepository<UserScreenItem> userScreenItemRepository,
                                    IBaseRepository<WorkContents> workContentRepository,
                                    IBaseRepository<Manhour> manhourRepository,
                                    IBaseRepository<Group> groupRepository,
                                    IBaseRepository<SalesObject> saleRepository,
                                    IBaseRepository<Theme> themeRepository,
                                    IBaseRepository<UserScreenItem> screenRepository)
        {
            this.manhourUpdateReponsitory = manhourUpdateReponsitory;
            this.context = context;
            this.hosting = hosting;
            this.userScreenItemRepository = userScreenItemRepository;
            this.workContentRepository = workContentRepository;
            this.manhourRepository = manhourRepository;
            this.groupRepository = groupRepository;
            this.saleRepository = saleRepository;
            this.themeRepository = themeRepository;
            this.screenRepository = screenRepository;
        }
       
        public async Task<string> GetRole(string userId)
        {
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectRole");
            List<string> functionClass = await manhourUpdateReponsitory.Search<string>(query, new { user_no = userId });
            return functionClass.FirstOrDefault();
        }
        public async Task<List<int>> GetHoliday(ManhourUpdateSearch keySearch)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(keySearch.Year.ToString()))
            {
                list.Add("Year");
            }
            if (!string.IsNullOrEmpty(keySearch.Month.ToString()))
            {
                list.Add("Month");
            }
            if (!string.IsNullOrEmpty(keySearch.User))
            {
                list.Add("User");
            }
            if (!string.IsNullOrEmpty(keySearch.Group))
            {
                list.Add("Group");
            }
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectHoliday", list);

            if (string.IsNullOrEmpty(keySearch.User))
            {
                keySearch.User = "";
            }
            var param = new
            {
                Year = Int32.Parse(keySearch.Year.Trim()),
                Month = Int32.Parse(keySearch.Month.Trim()),
                Group = keySearch.Group.ToString().Trim(),
                User = keySearch.User.ToString().Trim()
            };
            List<int> resultSearch = await manhourUpdateReponsitory.Search<int>(query, param);
            return resultSearch;
        }
        public async Task<List<ManhourUpdateViewModel>> GetDataSearch(ManhourUpdateSearch keySearch)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(keySearch.Year.ToString()))
            {
                list.Add("Year");
            }
            if (!string.IsNullOrEmpty(keySearch.Month.ToString()))
            {
                list.Add("Month");
            }
            if (!string.IsNullOrEmpty(keySearch.User))
            {
                list.Add("User");
            }
            if (!string.IsNullOrEmpty(keySearch.Group))
            {
                list.Add("Group");
            }
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectManhourData", list);

            if (string.IsNullOrEmpty(keySearch.User))
            {
                keySearch.User = "";
            }
            var param = new
            {
                Year = Int32.Parse(keySearch.Year.Trim()),
                Month = Int32.Parse(keySearch.Month.Trim()),
                Group = keySearch.Group.ToString().Trim(),
                User = keySearch.User.ToString().Trim()
            };          
            List<ManhourUpdateViewModel> resultSearch = await manhourUpdateReponsitory.Search<ManhourUpdateViewModel>(query, param);
            foreach(var item in resultSearch){
                item.Total = item.Day1 + item.Day2 + item.Day3 + item.Day4 + item.Day5 + item.Day6 + item.Day7 + item.Day8 + item.Day9 + item.Day10
                            + item.Day11 + item.Day12 + item.Day13 + item.Day14 + item.Day15 + item.Day16 + item.Day17 + item.Day18 + item.Day19 + item.Day20
                            + item.Day21 + item.Day22 + item.Day23 + item.Day24 + item.Day25 + item.Day26 + item.Day27 + item.Day28 + item.Day29 + item.Day30 + item.Day31;
            }
            return resultSearch;
        }
        public void UpdeteHistory(string screenItem, string screenInput, string userNo)
        {
            var param = new
            {
                screenItem = screenItem,
                screenInput = screenInput,
                userNo = userNo,
                
            };
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "UpdateUserScreenItems");
            manhourUpdateReponsitory.Update<UserScreenItem>(query, param);
           
        }
        public void InsertHistory(ManhourUpdateSearch keySearch, string userId)
        {
            DateTime today = DateTime.Now;
            if (keySearch.User != null)
            {
                string surrogateUserKey = userId + today.ToString("yyyyMMddHHmmss") + "001";
                UserScreenItem historyGroup = new UserScreenItem()
                {
                    Surrogate_key = surrogateUserKey,
                    User_no = userId,
                    Screen_url = "Manhourupdate",
                    Screen_item = "USER_NO",
                    Screen_input = keySearch.User,
                    Save_name = ""
                };
                userScreenItemRepository.Create(historyGroup);
            }
            if (keySearch.Group != null)
            {              
                string surrogateGroupKey = userId + today.ToString("yyyyMMddHHmmss") + "000";
                UserScreenItem historyGroup = new UserScreenItem() {
                    Surrogate_key = surrogateGroupKey,
                    User_no = userId,
                    Screen_url = "Manhourupdate",
                    Screen_item = "GROUP_CODE",
                    Screen_input = keySearch.Group,
                    Save_name = ""
                };
                userScreenItemRepository.Create(historyGroup);
            }
           
        }
        public async Task<ManHourUpdateSearchModel> Search(ManhourUpdateSearch keySearch,string userId)
        {
            List<UserScreenItem> history = await GetHistorySearch(userId);
            if(history.Count != 0)
            {
                foreach (var item in history)
                {
                    if (item.Screen_item == "GROUP_CODE")
                    {
                        UpdeteHistory("GROUP_CODE", keySearch.Group, userId);
                    }
                    if (item.Screen_item == "USER_NO")
                    {
                        UpdeteHistory("USER_NO", keySearch.User, userId);
                    }

                }
            }
            else
            {
                InsertHistory(keySearch, userId);
            }
            ManHourUpdateSearchModel model = new ManHourUpdateSearchModel();
            model.models = await GetDataSearch(keySearch);
            model.holiday = await GetHoliday(keySearch);
            return model;
        }

        //Get History search
        public async Task<List<UserScreenItem>> GetHistorySearch(string userId)
        {
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectHistory");
           
            List<UserScreenItem> userScreenItems = await manhourUpdateReponsitory.Search<UserScreenItem>(query, new { user_no = userId });            
            
            return userScreenItems;
        }

        // get user in group
        public ManhourUpdateUserSelectList GetUser(string groupId)
        {
            ManhourUpdateUserSelectList resultUser = new ManhourUpdateUserSelectList();
            List<User> listUsers = (List<User>)context.Users.Where(e => e.Group_code == groupId).ToList();         
            List<SelectListItem> userSelectList = new List<SelectListItem>();
            foreach (var item in listUsers)
            {
                userSelectList.Add(new SelectListItem() { Value = item.User_no, Text = item.User_no + "[" + item.User_name + "]" });
            }
            resultUser.listUser = userSelectList;
            return resultUser ;
        }

        public async Task<ManhourUpdate> GetGroupAndUser(string userId)
        {
            ManhourUpdate model = new ManhourUpdate();
            string role = await GetRole(userId);
            if (role != "0")
            {
                DateTime today = DateTime.Now;
                model.today = today.ToString("yyyy/MM/dd").Substring(0,7);
                // get group_code no history
                string groupId = context.Users.FirstOrDefault(e => e.User_no == userId).Group_code;
                // get History
                List<UserScreenItem> historySearch = await GetHistorySearch(userId);
                // get all group
                List<Group> groups = context.Groups.OrderBy(e => e.Group_name).ToList();
                List<SalesObject> salesObjects = context.SaleObjects.ToList();
                List<WorkContents> workContents = await GetWorkContents();
                List<SelectListItem> groupSelectList = new List<SelectListItem>();
                List<SelectListItem> userSelectList = new List<SelectListItem>();
                List<SelectListItem> saleObjSelectlist = new List<SelectListItem>();
                List<SelectListItem> groupSelectListTheme = new List<SelectListItem>();
                List<SelectListItem> wordContentSelectList = new List<SelectListItem>();
                foreach(var item in workContents)
                {
                    wordContentSelectList.Add(new SelectListItem() { 
                        Value = item.Work_contents_code , Text = item.Work_contents_code + "[" + item.Work_contents_code_name + "]"
                    });
                }
                foreach (var item in salesObjects)
                {
                    saleObjSelectlist.Add(new SelectListItem()
                    {
                        Value = item.Sales_object_code, Text = item.Sales_object_code + "[" + item.Sales_object_name + "]"
                    });
                   
                }
                foreach (var item in groups)
                {
                    groupSelectList.Add(new SelectListItem()
                    { 
                        Value = item.Group_code, Text = item.Group_code + "[" + item.Group_name + "]" 
                    });
                    groupSelectListTheme.Add(new SelectListItem()
                    {
                        Value = item.Group_code, Text = item.Accounting_group_code + "[" + item.Accounting_group_name + "]"
                    });
                }
                model.wordContents = wordContentSelectList;
                model.groupThemes = groupSelectListTheme;
                model.salesObject = saleObjSelectlist;
                model.groups = groupSelectList;

                // if have history
                if (historySearch.Count != 0)
                {
                    string userNo = null;
                    string groupCode = null;
                   
                    foreach (UserScreenItem item in historySearch)
                    {
                        if (item.Screen_item == "GROUP_CODE")
                        {
                            groupCode = item.Screen_input;
                        }
                        if (item.Screen_item == "USER_NO")
                        {
                            userNo = item.Screen_input;
                        }
                    }
                    List<User> users = context.Users.Where(e => e.Group_code == groupCode).ToList();
                    foreach (var item in users)
                    {
                        userSelectList.Add(new SelectListItem() { Value = item.User_no, Text = item.User_no + "[" + item.User_name + "]" });
                    }
                    model.groupId = groupCode;
                    model.userId = userNo;
                    model.users = userSelectList;
                    return model;
                }
                // if not have history
                else
                {
                    List<User> users = context.Users.Where(e => e.Group_code == userId).ToList();
                    foreach (var item in users)
                    {
                        userSelectList.Add(new SelectListItem() { Value = item.User_no, Text = item.User_no + "[" + item.User_name + "]" });
                    }
                    model.groupId = groupId;
                    model.userId = userId;
                    model.users = userSelectList;
                    return model;
                }
            }
            return null;
        }

        public async Task<ExportModel> ExportCSV(string user, string group)
        {
            ExportModel exportModel = new ExportModel();
            DateTime today = DateTime.Now;
            ManhourUpdateSearch key = new ManhourUpdateSearch()
            {
                Year = today.Year.ToString(),
                Month = today.Month.ToString(),
                Group = group,
                User = user
            };
            List<ManhourUpdateViewModel> result = await GetDataSearch(key);
            string name = "DL Manhour " + user + " " + today.ToString("yyyyMMddHHmmss") + ".csv";
            StringBuilder buider = new StringBuilder();          
            buider.AppendLine(header);
            foreach (var item in result)
            {
                buider.AppendLine($"{item.Year}, {item.Month}, {item.User_no}, {item.User_name}, {item.Theme_no}, {item.Theme_name1}, " +
                                  $"{item.Work_contents_code},{item.Work_contents_code_name},{item.Work_contents_detail}, " +
                                  $"{item.Total.ToString("0.0")},{item.Day1.ToString("0.0")}, {item.Day2.ToString("0.0")}, " +
                                  $"{item.Day2.ToString("0.0")}, {item.Day3.ToString("0.0")},{item.Day4.ToString("0.0")}, " +
                                  $"{item.Day5.ToString("0.0")}, {item.Day6.ToString("0.0")}, {item.Day7.ToString("0.0")}, " +
                                  $"{item.Day8.ToString("0.0")}, {item.Day9.ToString("0.0")}, {item.Day10.ToString("0.0")}, " +
                                  $"{item.Day11.ToString("0.0")}, {item.Day12.ToString("0.0")}, {item.Day13.ToString("0.0")}," +
                                  $"{item.Day14.ToString("0.0")}, {item.Day15.ToString("0.0")}, {item.Day16.ToString("0.0")}, " +
                                  $"{item.Day17.ToString("0.0")}, {item.Day18.ToString("0.0")}, {item.Day19.ToString("0.0")}," +
                                  $"{item.Day20.ToString("0.0")}, {item.Day21.ToString("0.0")}, {item.Day22.ToString("0.0")}," +
                                  $"{item.Day23.ToString("0.0")},{item.Day24.ToString("0.0")}, {item.Day25.ToString("0.0")}," +
                                  $"{item.Day26.ToString("0.0")}, {item.Day27.ToString("0.0")}, {item.Day28.ToString("0.0")}," +
                                  $"{item.Day29.ToString("0.0")}, {item.Day30.ToString("0.0")}, {item.Day31.ToString("0.0")}");
            }
            exportModel.builder = buider;
            exportModel.nameFile = name;
            return exportModel;
        }

        // check file exists and file = csv 
        public bool CheckFileCSV(string fileName)
        {
            var fileExt = Path.GetExtension(fileName).Substring(1).ToLower();
            if (!File.Exists(fileName))
            {
                return false;
            }
            else if(fileExt != "csv")
            {
                return false;
            }
            return true;
        }

        // check header file csv duplicate header data
        public bool CheckHeaderFileCSV(string header, string headerCSV)
        {
            string[] listHeader = header.Split(",");
            string[] listHeaderCSV = headerCSV.Split(",");

            if (listHeader.Length == listHeaderCSV.Length)
            {
                for (int i = 0; i < listHeaderCSV.Length; i++)
                {
                    if (listHeader[i] != listHeaderCSV[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<ManhourUpdateViewModel> GetDataImportCSV(List<string> listDataCsv)
        {
            List<ManhourUpdateViewModel> dataExport = new List<ManhourUpdateViewModel>();
            ManhourUpdateViewModel CsvData = null;
            for (int i = 1; i < listDataCsv.Count; i++)
            {
                string[] dataCSV = listDataCsv[i].Split(",");
                CsvData = new ManhourUpdateViewModel();
                CsvData.Year = Int16.Parse(dataCSV[0]);
                CsvData.Month = Int16.Parse(dataCSV[1]);
                CsvData.User_no = dataCSV[2];
                CsvData.User_name = dataCSV[3];
                CsvData.Theme_no = dataCSV[4];
                CsvData.Theme_name1 = dataCSV[5];
                CsvData.Work_contents_code = dataCSV[6];
                CsvData.Work_contents_code = dataCSV[7];
                CsvData.Work_contents_detail = dataCSV[8];
                CsvData.Total = Double.Parse(dataCSV[9]);
                CsvData.Day1 = Double.Parse(dataCSV[10]);
                CsvData.Day2 = Double.Parse(dataCSV[11]);
                CsvData.Day3 = Double.Parse(dataCSV[12]);
                CsvData.Day4 = Double.Parse(dataCSV[13]);
                CsvData.Day5 = Double.Parse(dataCSV[14]);
                CsvData.Day6 = Double.Parse(dataCSV[15]);
                CsvData.Day7 = Double.Parse(dataCSV[16]);
                CsvData.Day8 = Double.Parse(dataCSV[17]);
                CsvData.Day9 = Double.Parse(dataCSV[18]);
                CsvData.Day10 = Double.Parse(dataCSV[19]);
                CsvData.Day11 = Double.Parse(dataCSV[20]);
                CsvData.Day12 = Double.Parse(dataCSV[21]);
                CsvData.Day13 = Double.Parse(dataCSV[22]);
                CsvData.Day14 = Double.Parse(dataCSV[23]);
                CsvData.Day15 = Double.Parse(dataCSV[24]);
                CsvData.Day16 = Double.Parse(dataCSV[25]);
                CsvData.Day17 = Double.Parse(dataCSV[26]);
                CsvData.Day18 = Double.Parse(dataCSV[27]);
                CsvData.Day19 = Double.Parse(dataCSV[28]);
                CsvData.Day20 = Double.Parse(dataCSV[29]);
                CsvData.Day21 = Double.Parse(dataCSV[30]);
                CsvData.Day22 = Double.Parse(dataCSV[31]);
                CsvData.Day23 = Double.Parse(dataCSV[32]);
                CsvData.Day24 = Double.Parse(dataCSV[33]);
                CsvData.Day25 = Double.Parse(dataCSV[34]);
                CsvData.Day26 = Double.Parse(dataCSV[35]);
                CsvData.Day27 = Double.Parse(dataCSV[36]);
                CsvData.Day28 = Double.Parse(dataCSV[37]);
                CsvData.Day29 = Double.Parse(dataCSV[38]);
                CsvData.Day30 = Double.Parse(dataCSV[39]);
                CsvData.Day31 = Double.Parse(dataCSV[40]);
                dataExport.Add(CsvData);
            }
            return dataExport;
        }

        public List<ManhourUpdateViewModel> ImportCSV(IFormFile files)
        {
            List<Manhour> manhourTrue = new List<Manhour>();
            List<ManhourUpdateViewModel> dataExport = new List<ManhourUpdateViewModel>();
            string uniqueFileName = null;
            string fname = files.FileName;            
            string uploadFld = Path.Combine(this.hosting.WebRootPath, "csv");
            uniqueFileName = Guid.NewGuid().ToString() + "_" +fname;
            string filePath = Path.Combine(uploadFld, uniqueFileName);
            List<string> lines = new List<string>();
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                files.CopyTo(fileStream);
                fileStream.Flush();
            }
            
            if (CheckFileCSV(filePath)){
                lines  = CommonFunction.ReadCSVFile(filePath);
                if (CheckHeaderFileCSV(header, lines[0]))
                {
                    dataExport = GetDataImportCSV(lines);                
                    //List<Manhour> manhour = context.Manhours.OrderBy(e =>e.Year).ToList();                   
                    //foreach (var item in dataExport)
                    //{ 
                    //    foreach(var check in manhour)
                    //    {
                    //        if(item.Year == check.Year && item.Month == check.Month && item.User_no == check.User_no && item.Group_code == check.Group_code && item.Theme_no == check.Theme_no && item.Work_contents_code == check.Work_contents_code)
                    //        {
                    //            manhourTrue.Add(item);
                    //        }
                    //    }
                    //}
                }
            }                                       
            return dataExport;
        }

        public async Task<int> UpdateManhours(List<Manhour> manhours)
        {
            var result = 0;
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "UpdateManhour");

            foreach (Manhour mh in manhours)
            {
                //Set param for update query
                var param = new
                {
                    Year = mh.Year,
                    Month = mh.Month,
                    User_no = mh.User_no,
                    Theme_no = mh.Theme_no,
                    Work_contents_class = mh.Work_contents_class,
                    Work_contents_code = mh.Work_contents_code,
                    Work_contents_detail = mh.Work_contents_detail,
                    Pin_flg = mh.Pin_flg,
                    Total = mh.Total,
                    Day1 = mh.Day1,
                    Day2 = mh.Day2,
                    Day3 = mh.Day3,
                    Day4 = mh.Day4,
                    Day5 = mh.Day5,
                    Day6 = mh.Day6,
                    Day7 = mh.Day7,
                    Day8 = mh.Day8,
                    Day9 = mh.Day9,
                    Day10 = mh.Day10,
                    Day11 = mh.Day11,
                    Day12 = mh.Day12,
                    Day13 = mh.Day13,
                    Day14 = mh.Day14,
                    Day15 = mh.Day15,
                    Day16 = mh.Day16,
                    Day17 = mh.Day17,
                    Day18 = mh.Day18,
                    Day19 = mh.Day19,
                    Day20 = mh.Day20,
                    Day21 = mh.Day21,
                    Day22 = mh.Day22,
                    Day23 = mh.Day23,
                    Day24 = mh.Day24,
                    Day25 = mh.Day25,
                    Day26 = mh.Day26,
                    Day27 = mh.Day27,
                    Day28 = mh.Day28,
                    Day29 = mh.Day29,
                    Day30 = mh.Day30,
                    Day31 = mh.Day31,
                    Fix_date = mh.Fix_date
                };
                result = await manhourRepository.Update(query, param);
            }
            return result;
        }
        public async Task<bool> Save(List<Manhour> saveDatas)
        {
           

            return true ;
        }
        public async Task<SelectThemeModel> SearchThemes(SearchThemeParam param, string user_no)
        {
            var addListTheme = new List<string>();

            //update screen item
            var updateScreenItem = QueryLoader.GetQuery("ManhourInput", "UpdateUserScreenItem");

            List<string> items = new List<string>
            {
                    "ThemeNo",
                    "AccountingGroupCode",
                    "ThemeName",
                    "SalesObjectCode",
                    "SoldFlg"
            };

            //update new item input in to user screen item
            foreach (string item in items)
            {
                string screenInput = (string)param.GetType().GetProperty(item).GetValue(param);

                var updateParam = new
                {
                    ScreenItem = item,
                    ScreenUrl = "SelectTheme",
                    UserNo = user_no,
                    ScreenInput = screenInput
                };

                await screenRepository.Update(updateScreenItem, updateParam);
            }

            // check condition for where to get data
            if (!string.IsNullOrEmpty(param.ThemeNo))
            {
                addListTheme.Add("ThemeNo");
            }
            if (!string.IsNullOrEmpty(param.ThemeName))
            {
                addListTheme.Add("ThemeName");
            }
            if (!string.IsNullOrEmpty(param.SalesObjectCode))
            {
                addListTheme.Add("ObjectCode");
            }
            if (!string.IsNullOrEmpty(param.AccountingGroupCode))
            {
                addListTheme.Add("GroupCode");
            }
            if (!string.IsNullOrEmpty(param.SoldFlg) && !param.SoldFlg.Equals("全て"))
            {
                addListTheme.Add("SoldFlg");
            }

            var query = QueryLoader.GetQuery("ManhourInput", "SelectHistoryThemeSelected", addListTheme);

            //if sold flag is null or empty set default value is empty
            if (string.IsNullOrEmpty(param.SoldFlg))
            {
                param.SoldFlg = "";
            }

            var paramQuery = new
            {
                ThemeNo = param.ThemeNo,
                ThemeName = param.ThemeName,
                ObjectCode = param.SalesObjectCode,
                GroupCode = param.AccountingGroupCode,
                SoldFlg = param.SoldFlg.Equals("未売上") ? false : true
            };

            SelectThemeModel model = new SelectThemeModel();
            model.Themes = await themeRepository.Select<Theme>(query, paramQuery);
            // set history input to return view 
            model.HistoryInput = param;

            return model;
        }
        public async Task<List<WorkContents>> GetWorkContents()
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectWorkContent");
            List<WorkContents> result = await groupRepository.Select<WorkContents>(query);
            return result;
        }
    }

}
