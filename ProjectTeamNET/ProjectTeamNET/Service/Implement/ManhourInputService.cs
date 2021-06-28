using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProjectTeamNET.Service.Implement
{
    
    /// <summary>
    /// Handle events ManhourInput
    /// </summary>
    public class ManhourInputService : IManhourInputService
    {

        const string FORMATDATE = "yyyy/MM/dd";

        private readonly IBaseRepository<WorkContents> workContentRepository;
        private readonly IBaseRepository<Manhour> manhourRepository;
        private readonly IBaseRepository<Group> groupRepository;
        private readonly IBaseRepository<SalesObject> saleRepository;
        private readonly IBaseRepository<Theme> themeRepository;
        private readonly IBaseRepository<UserScreenItem> screenRepository;

        public ManhourInputService(IBaseRepository<WorkContents> workContentRepository, 
                                   IBaseRepository<Manhour> manhourRepository, 
                                   IBaseRepository<Group> groupRepository, 
                                   IBaseRepository<SalesObject> saleRepository, 
                                   IBaseRepository<Theme> themeRepository, 
                                   IBaseRepository<UserScreenItem> screenRepository)
        {
            this.workContentRepository  =   workContentRepository;
            this.manhourRepository      =   manhourRepository;
            this.groupRepository        =   groupRepository;
            this.saleRepository         =   saleRepository;
            this.themeRepository        =   themeRepository;
            this.screenRepository       =   screenRepository;
        }
        public async Task<InitDataModel> Init(string paramSt)
        {
            string[] data = paramSt.Split(";");
            DateTime date = DateTime.Parse(data[0]);
            string user_no = data[1];

            InitDataModel model = new InitDataModel();
            model.Groups = await GetGroups();
            model.Objects = await saleRepository.Gets();
            model.Contents = await GetWorkContents();
            model.DateSelect = date.ToString(FORMATDATE);
            //Check screen item history button to return view Day/week/month
            List<UserScreenItem> pageHistory = await GetsScreenItemHistory(user_no, "ManhourInput");
            if (pageHistory.FirstOrDefault().Screen_item != null)
            {
                model.pageHistory = pageHistory.FirstOrDefault().Screen_item;
            }
            
            return model;
        }
        /// <summary>
        /// Get Manhour data from database 
        /// </summary>
        /// <param name="paramSt">dateSt + user_no</param>
        /// <returns></returns>
        public async Task<ManhourInputModel> GetManhourData(string paramSt)
        {
            string[] data = paramSt.Split(";");
            DateTime date = DateTime.Parse(data[0]);
            string user_no = data[1];

            //set data for manhourinput model
            ManhourInputModel model = new ManhourInputModel();
            model.ManhourDatas   = await GetManhours(paramSt);
            model.DateSelect     = date.ToString(FORMATDATE);
            model.ListDateOfWeek = GetListDayOfWeek(date);

            //Check pin themes and insert if not exits in manhour
            List<ManhourInput> pinThemes = await GetPinTheme(paramSt);
            if (pinThemes.Count() != 0)
            {
                foreach (ManhourInput pinManhour in pinThemes) 
                {
                    var invalidNum = 0;
                    var numManhour = 0;
                    foreach (ManhourInput manhour in model.ManhourDatas) 
                    {
                        //increatement row number approved
                        numManhour++;

                        //Check exit PinTheme in Manhour table by keys
                        if (   pinManhour.Year == manhour.Year
                            && pinManhour.User_no.Equals(manhour.User_no)
                            && pinManhour.Theme_no.Equals(manhour.Theme_no)
                            && pinManhour.Work_contents_class.Equals(manhour.Work_contents_class)
                            && pinManhour.Work_contents_code.Equals(manhour.Work_contents_code)
                            && pinManhour.Work_contents_detail.Equals(manhour.Work_contents_detail)
                          )
                          {
                                //if exist 
                                break;
                          }

                        //increatement row invalid
                        invalidNum++;
                    }

                    //If row not exits in manhour -> Insert to manhour table
                    if (invalidNum == numManhour)
                    {
                        Manhour pinMh = new Manhour();
                        pinMh.Year = pinManhour.Year;
                        pinMh.Month = (Int16)(pinManhour.Month + 1);
                        pinMh.User_no = pinManhour.User_no;
                        pinMh.Theme_no = pinManhour.Theme_no;
                        pinMh.Group_code = pinManhour.Group_code;
                        pinMh.Pin_flg = false; //set default value is false for Pin
                        pinMh.Site_code = pinManhour.Site_code;
                        pinMh.Work_contents_class = pinManhour.Work_contents_class;
                        pinMh.Work_contents_code = pinManhour.Work_contents_code;
                        pinMh.Work_contents_detail = pinManhour.Work_contents_detail;
                       
                        await manhourRepository.Create(pinMh);    
                    }
                }

                // Select again manhour data if had insert new record
                model.ManhourDatas = await GetManhours(paramSt);
            }
           
            return model;
        }
        /// <summary>
        /// Get screen item by user_no and screen_url
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        public async Task<List<UserScreenItem>> GetsScreenItemHistory(string user_no, string screen_url)
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectScreenItem");
            var param = new
            {
                User_no = user_no,
                Screen_url = screen_url
            };
            List<UserScreenItem> result = await manhourRepository.Select<UserScreenItem>(query, param);

            return result;
        }

        /// <summary>
        /// Get pin theme by param 
        /// </summary>
        /// <param name="paramSt"></param>
        /// <returns></returns>
        public async Task<List<ManhourInput>> GetPinTheme(string paramSt)
        {
            //get date and user_no from paramSt
            string[] data = paramSt.Split(";");
            DateTime date = DateTime.Parse(data[0]);
            string user_no = data[1];

            //get query by xml file
            var query = QueryLoader.GetQuery("ManhourInput","SelectPinTheme");

            //add param for query
            var param = new
            {
                Year = date.Year,
                Month = date.Month-1,
                User_no = user_no
            };
            List<ManhourInput> results = await manhourRepository.Select<ManhourInput>(query, param);
            return results;
        }

        /// <summary>
        /// Get param search theme history
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        public async Task<SelectThemeModel> GetHistoryThemes(string user_no)
        {

            // Get History Theme selected
            List<UserScreenItem> selectedTheme = await GetsScreenItemHistory(user_no,"SelectTheme");
           
            if(selectedTheme == null)
            {
                List<string> items = new List<string>
                {
                    "ThemeNo",
                    "AccountingGroupCode",
                    "ThemeName",
                    "SalesObjectCode",
                    "SoldFlg"
                };
                
                //if not exist insert new  user screen item select theme
                var i = 0;
                string key = DateTime.Now.ToString("yyyyMMddHHmmss");
                foreach (string item in items)
                {
                    key += i.ToString("D3");
                    UserScreenItem user = new UserScreenItem 
                    {
                        Surrogate_key = key,
                        User_no       = user_no,
                        Screen_url    = "SelectTheme",
                        Screen_item   = item,
                        Screen_input  = null,
                        Save_name     = null
                    };  
                    await screenRepository.Create(user);
                    i++;
                }
                return null;
            }

            SearchThemeParam param = new SearchThemeParam();
            // Get value exist in db for search param
            foreach (UserScreenItem item in selectedTheme)
            {
                if (item.Screen_item.Equals("ThemeNo"))
                    param.ThemeNo = item.Screen_input;
                if (item.Screen_item.Equals("ThemeName"))
                    param.ThemeName = item.Screen_input;
                if (item.Screen_item.Equals("AccountingGroupCode"))
                    param.AccountingGroupCode = item.Screen_input;
                if (item.Screen_item.Equals("SalesObjectCode"))
                    param.SalesObjectCode = item.Screen_input;
                if (item.Screen_item.Equals("SoldFlg"))
                    param.SoldFlg = item.Screen_input;
            }

            // return search result
            return await SearchThemes(param,user_no);
              
        }
        /// <summary>
        /// Search Theme by param 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
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
            foreach(string item in items)
            {
                string screenInput =(string)param.GetType().GetProperty(item).GetValue(param);

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
            if( string.IsNullOrEmpty(param.SoldFlg))
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

        /// <summary>
        /// Handle save data form client to db
        /// </summary>
        /// <param name="screenItems"></param>
        public async Task<bool> Save(List<Manhour> saveDatas)
        {
            //add param for query
            var param = new
            {
                Year = saveDatas[0].Year,
                Month = saveDatas[0].Month,
                User_no = saveDatas[0].User_no
            };

            var query = QueryLoader.GetQuery("ManhourInput", "SelectManhour");
            List<Manhour> mhDBDatas = await manhourRepository.Select<Manhour>(query, param);

            List<Manhour> listUpdate = new List<Manhour>();
            foreach (Manhour saveItem in saveDatas)
            {

                foreach (Manhour manhour in mhDBDatas)
                {
                    //Check key of manhour from client and DB if same then add list update
                    if (    saveItem.Year == manhour.Year
                        && saveItem.Month == manhour.Month
                        && saveItem.User_no.Equals(manhour.User_no)
                        && saveItem.Theme_no.Equals(manhour.Theme_no)
                        && saveItem.Work_contents_class.Equals(manhour.Work_contents_class)
                        && saveItem.Work_contents_code.Equals(manhour.Work_contents_code)
                        && saveItem.Work_contents_detail.Equals(manhour.Work_contents_detail)
                      ){
                        listUpdate.Add(saveItem);
                        mhDBDatas.Remove(manhour);
                        break;
                    }
                   
                }
            }
            //remove mh exist in saveDatas form client
            foreach (Manhour mh in listUpdate)
            {
                saveDatas.Remove(mh);
            }

            var result = 0;
            //Update list record
            if (listUpdate != null)
            {
                result = await UpdateManhours(listUpdate);
            }

            //Delete list record rest from DB
            if (mhDBDatas.Count != 0) 
            {
                result = await DeleteManhours(mhDBDatas);
            }

            //Insert new record in save list 
            if (saveDatas.Count != 0)
            {
                foreach (Manhour mh in saveDatas)
                {
                    Manhour newMh = new Manhour();
                    newMh.Year = mh.Year;
                    newMh.Month = mh.Month;
                    newMh.User_no = mh.User_no;
                    newMh.Theme_no = mh.Theme_no;
                    newMh.Group_code = mh.Group_code;
                    newMh.Pin_flg =mh.Pin_flg;
                    newMh.Site_code = mh.Site_code;
                    newMh.Work_contents_class = mh.Work_contents_class;
                    newMh.Work_contents_code = mh.Work_contents_code;
                    newMh.Work_contents_detail = mh.Work_contents_detail;

                    await manhourRepository.Create(newMh);
                }

            }
           
            return result > 0;
        }

        /// <summary>
        /// Return list dayofweek need get
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        public List<string> GetListDayOfWeek(DateTime date)
        {
            DateTime dateSelected = date;

            int currentDayOfWeek = (int) dateSelected.DayOfWeek;
         
            DateTime sunday = dateSelected.AddDays(-currentDayOfWeek);

            DateTime monday = sunday.AddDays(1);

            if( currentDayOfWeek  == 0)
            {
                monday = monday.AddDays(-7);
            }

            //get list date form monday to sunday
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            List<string> dateOfWeekList = new List<string>();

            foreach (var data in dates)
            {
                //if month of this date is in month selected
                if (DateTime.Parse(data.ToString()).Month == dateSelected.Month)
                {
                    dateOfWeekList.Add(String.Format("{0:yyyy/MM/dd}", data));
                }
                
            }
            return dateOfWeekList;
        }
        /// <summary>
        /// Update records in manhour table
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        public async Task<int> UpdateManhours(List<Manhour> manhours)
        {
            var result = 0;
            var query = QueryLoader.GetQuery("ManhourInput", "UpdateManhour");

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        public async Task<int> DeleteManhours(List<Manhour> manhours)
        {
            var result = 0;
            var query = QueryLoader.GetQuery("ManhourInput", "DeleteManhour");

            foreach (Manhour mh in manhours)
            {
                //Set keys param for where of delete query
                var keys = new
                {
                    Year = mh.Year,
                    Month = mh.Month,
                    User_no = mh.User_no,
                    Theme_no = mh.Theme_no,
                    Work_contents_class = mh.Work_contents_class,
                    Work_contents_code = mh.Work_contents_code,
                    Work_contents_detail = mh.Work_contents_detail
                };
                result = await manhourRepository.Update(query, keys);
            }
            return result;
        }

        public async Task<List<ManhourInput>> GetManhours(string paramSt)
        {
            string[] data = paramSt.Split(";");
            DateTime date = DateTime.Parse(data[0]);
            string user_no = data[1];

            //add list for where select if value of condition not null or empty
            var addList = new List<string>();
            if (!string.IsNullOrEmpty(date.Year.ToString()))
            {
                addList.Add("Year");
            }
            if (!string.IsNullOrEmpty(date.Month.ToString()))
            {
                addList.Add("Month");
            }
            if (!string.IsNullOrEmpty(user_no))
            {
                addList.Add("User_no");
            }

            // Get query select 
            var query = string.Format(QueryLoader.GetQuery("ManhourInput", "SelectManhourDatas", addList));

            // Set param for manhour query
            var param = new
            {
                Year = date.Year,
                Month = date.Month,
                User_no = user_no
            };
            return await manhourRepository.Select<ManhourInput>(query, param);
        }

        public async Task<List<Group>> GetGroups()
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectGroupCode");
            List<Group> result = await groupRepository.Select<Group>(query);
            return result;
        }

        public async Task<List<WorkContents>> GetWorkContents()
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectWorkContent");
            List<WorkContents> result = await groupRepository.Select<WorkContents>(query);
            return result;
        }

    }
}
