using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
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

        private readonly IBaseRepository<Manhour> manhourRepository;
        private readonly IBaseRepository<SalesObject> saleRepository;
        private readonly IBaseRepository<UserScreenItem> screenRepository;

        public ManhourInputService(IBaseRepository<Manhour> manhourRepository, 
                                   IBaseRepository<SalesObject> saleRepository, 
                                   IBaseRepository<UserScreenItem> screenRepository)
        {
            this.manhourRepository      =   manhourRepository;
            this.saleRepository         =   saleRepository;
            this.screenRepository       =   screenRepository;
        }
        /// <summary>
        /// Get first data to load page
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public async Task<InitDataModel> Init(InputParamModel pModel)
        { 
            DateTime date = DateTime.Parse(pModel.dateStr);          
            InitDataModel model = new InitDataModel();
            model.Groups = await GetGroups();
            model.Objects = await saleRepository.Gets();
            model.Contents = await GetWorkContents();
            model.DateSelect = date.ToString(FORMATDATE);
            //Check screen item history button to return view Day/week/month
            List<UserScreenItem> pageHistory = await GetsScreenItemHistory(pModel.userNo, "ManhourInput");

            if (pageHistory.FirstOrDefault().Screen_item != null)
            {
                model.pageHistory = pageHistory.FirstOrDefault().Screen_item;
            }
            else
            {
                //Insert new a user screen item into database
                string key = DateTime.Now.ToString("yyyyMMddHHmmss");
                key += 0.ToString("D3");
                UserScreenItem user = new UserScreenItem
                {
                        Surrogate_key = key,
                        User_no = pModel.userNo,
                        Screen_url = "ManhourInput",
                        Screen_item = null,
                        Screen_input = null,
                        Save_name = null
                };
                screenRepository.Create(user);
                model.pageHistory = null;
            }
            
            return model;
        }
        /// <summary>
        /// Get Manhour data from database and insert pintheme if not exist
        /// </summary>
        /// <param name="paramSt">dateSt + user_no</param>
        /// <returns></returns>
        public async Task<ManhourInputModel> GetManhourData(InputParamModel pModel)
        {
            DateTime date = DateTime.Parse(pModel.dateStr);
            HorlidayParam hparam = new HorlidayParam();
            hparam.Year = date.Year;
            hparam.Month = date.Month;
            hparam.SiteCode = "HP";
            //set data for manhourinput model
            ManhourInputModel model = new ManhourInputModel();
            model.ManhourDatas   = await GetManhours(pModel);
            model.DateSelect     = date.ToString(FORMATDATE);
            model.ListDateOfWeek = GetListDayOfWeek(date);
            model.Horlidays      = await GetHorliday(hparam);

            //Check pin themes and insert if not exits in manhour
            List<ManhourInput> pinThemes = await GetPinTheme(pModel);
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
                       
                        manhourRepository.Create(pinMh);    
                    }
                }

                // Select again manhour data if had insert new record
                model.ManhourDatas = await GetManhours(pModel);
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
            var pQuery = new
            {
                User_no = user_no,
                Screen_url = screen_url
            };
            List<UserScreenItem> result = await manhourRepository.Select<UserScreenItem>(query, pQuery);

            return result;
        }

        /// <summary>
        /// Get pin theme by param 
        /// </summary>
        /// <param name="paramSt"></param>
        /// <returns></returns>
        public async Task<List<ManhourInput>> GetPinTheme(InputParamModel pModel)
        {
            //get date and user_no from paramSt
            DateTime date = DateTime.Parse(pModel.dateStr);

            //get query by xml file
            var query = QueryLoader.GetQuery("ManhourInput","SelectPinTheme");

            //add param for query
            var pQuery = new
            {
                Year = date.Year,
                Month = date.Month-1,
                User_no = pModel.userNo
            };
            List<ManhourInput> results = await manhourRepository.Select<ManhourInput>(query, pQuery);
            return results;
        }

        /// <summary>
        /// Get param search theme history
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        public async Task<SearchThemeParam> GetHistoryThemes(string user_no)
        {

            // Get History Theme selected
            List<UserScreenItem> searchHistory = await GetsScreenItemHistory(user_no,"SelectTheme");
           
            if(searchHistory == null)
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
                    screenRepository.Create(user);
                    i++;
                }
                return null;
            }

            SearchThemeParam historyParma = new SearchThemeParam();
            // Get value exist in db for search param
            foreach (UserScreenItem item in searchHistory)
            {
                if (item.Screen_item.Equals("ThemeNo"))
                    historyParma.ThemeNo = item.Screen_input;
                if (item.Screen_item.Equals("ThemeName"))
                    historyParma.ThemeName = item.Screen_input;
                if (item.Screen_item.Equals("AccountingGroupCode"))
                    historyParma.AccountingGroupCode = item.Screen_input;
                if (item.Screen_item.Equals("SalesObjectCode"))
                    historyParma.SalesObjectCode = item.Screen_input;
                if (item.Screen_item.Equals("SoldFlg"))
                    historyParma.SoldFlg = item.Screen_input;
            }

            return historyParma;              
        }
        /// <summary>
        /// Search Theme by param 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<SelectThemeModel> SearchThemes(SearchThemeParam pSearchQuery, string user_no)
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
                string screenInput =(string)pSearchQuery.GetType().GetProperty(item).GetValue(pSearchQuery);

                var updateParam = new
                {
                    ScreenItem = item,
                    ScreenUrl = "SelectTheme",
                    UserNo = user_no,
                    ScreenInput = screenInput
                };

               await manhourRepository.Update(updateScreenItem, updateParam);
            }
                         
            // check condition for where to get data
            if (!string.IsNullOrEmpty(pSearchQuery.ThemeNo))
            {
                addListTheme.Add("ThemeNo");
            }
            if (!string.IsNullOrEmpty(pSearchQuery.ThemeName))
            {
                addListTheme.Add("ThemeName");
            }
            if (!string.IsNullOrEmpty(pSearchQuery.SalesObjectCode))
            {
                addListTheme.Add("ObjectCode");
            }
            if (!string.IsNullOrEmpty(pSearchQuery.AccountingGroupCode))
            {
                addListTheme.Add("GroupCode");
            }
            if (!string.IsNullOrEmpty(pSearchQuery.SoldFlg) && !pSearchQuery.SoldFlg.Equals("全て"))
            {
                addListTheme.Add("SoldFlg");
            }

            var query = QueryLoader.GetQuery("ManhourInput", "SelectHistoryThemeSelected", addListTheme);

            //if sold flag is null or empty set default value is empty
            if( string.IsNullOrEmpty(pSearchQuery.SoldFlg))
            {
                pSearchQuery.SoldFlg = "";
            }

            var paramQuery = new
            {
                ThemeNo = pSearchQuery.ThemeNo,
                ThemeName = pSearchQuery.ThemeName,
                ObjectCode = pSearchQuery.SalesObjectCode,
                GroupCode = pSearchQuery.AccountingGroupCode,
                SoldFlg = pSearchQuery.SoldFlg.Equals("未売上") ? false : true
            };

            SelectThemeModel model = new SelectThemeModel();
            model.Themes = await manhourRepository.Select<Theme>(query, paramQuery);
            // set history input to return view 
            model.HistoryInput = pSearchQuery;

            return model;
        }

        /// <summary>
        /// Handle save data form client to db
        /// </summary>
        /// <param name="screenItems"></param>
        public async Task<bool> Save(SaveData saveDatas)
        {
            var result = 0;
            //Delete in list delete
            if (saveDatas.Delete.Count != 0)
            {
                result = await DeleteManhours(saveDatas.Delete);
                if (result < 0)
                {
                    return false;
                }
            }
            //Insert new record in save list 
            if (saveDatas.Insert.Count != 0)
            {
                result = Create(saveDatas.Insert);
                if (result <= 0)
                { 
                    return false;
                }
            }
            //update information 
            if (saveDatas.Update.Count != 0)
            {
                result = await UpdateManhours(saveDatas.Update);
                if (result <= 0)
                {
                    return false;
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
        /// 
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        public int Create(List<Manhour> manhours)
        {
            var result = 0;
            foreach (Manhour mh in manhours)
            {
                Manhour newMh = new Manhour
                {
                    Year = mh.Year,
                    Month = mh.Month,
                    User_no = mh.User_no,
                    Theme_no = mh.Theme_no,
                    Work_contents_class = mh.Work_contents_class,
                    Work_contents_code = mh.Work_contents_code,
                    Work_contents_detail = mh.Work_contents_detail
                };

                result =  manhourRepository.Create(newMh);
            }
            return result;
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
                    mh.Year,mh.Month, mh.User_no,mh.Theme_no,mh.Work_contents_class,mh.Work_contents_code,
                    mh.Work_contents_detail,mh.Pin_flg,mh.Total,
                    mh.Day1,mh.Day2,mh.Day3,mh.Day4,mh.Day5,mh.Day6,mh.Day7,mh.Day8,mh.Day9,mh.Day10,
                    mh.Day11,mh.Day12,mh.Day13,mh.Day14,mh.Day15,mh.Day16,mh.Day17,mh.Day18,mh.Day19,
                    mh.Day20,mh.Day21,mh.Day22,mh.Day23,mh.Day24,mh.Day25,mh.Day26,mh.Day27,mh.Day28,
                    mh.Day29,mh.Day30,mh.Day31,mh.Fix_date
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
                    mh.Year,
                    mh.Month,
                    mh.User_no,
                    mh.Theme_no,
                    mh.Work_contents_class,
                    mh.Work_contents_code,
                    mh.Work_contents_detail
                };
                result = await manhourRepository.Update(query, keys);
            }
            return result;
        }
        /// <summary>
        /// Get list of manhour data
        /// </summary>
        /// <param name="paramModel"></param>
        /// <returns></returns>
        public async Task<List<ManhourInput>> GetManhours(InputParamModel paramModel)
        {
            
            DateTime date = DateTime.Parse(paramModel.dateStr);

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
            if (!string.IsNullOrEmpty(paramModel.userNo))
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
                User_no = paramModel.userNo
            };
            return await manhourRepository.Select<ManhourInput>(query, param);
        }
        /// <summary>
        /// get Accounting group for combobox
        /// </summary>
        /// <returns></returns>
        public async Task<List<Group>> GetGroups()
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectGroupCode");
            List<Group> result = await manhourRepository.Select<Group>(query);
            return result;
        }
        /// <summary>
        /// get work contents for combobox
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkContents>> GetWorkContents()
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectWorkContent");
            List<WorkContents> result = await manhourRepository.Select<WorkContents>(query);
            return result;
        }
        /// <summary>
        /// Save screen item cliked
        /// </summary>
        /// <param name="pModel"></param>
        public async void SavePageHistory(InputParamModel pModel)
        {
            var updateScreenItem = QueryLoader.GetQuery("ManhourInput", "UpdatePage");
            var updateParam = new
            {
                ScreenItem = pModel.pageName,
                ScreenUrl = "ManhourInput",
                UserNo = pModel.userNo,
            };

            await screenRepository.Update(updateScreenItem, updateParam);
        }
        /// <summary>
        /// get list horliday by sitecode month and year
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        public async Task<List<int>> GetHorliday(HorlidayParam param)
        {

            var query = QueryLoader.GetQuery("ManhourInput", "SelectHorliday");

            List<int> horlidays = await manhourRepository.Search<int>(query, param);
            return horlidays;
        }
    }
}
