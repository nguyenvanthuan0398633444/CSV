using m_user_screen_item;
using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Repository.Implement;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Repository.Implement
{
    public class ManhourReport
    {
        private readonly IBaseRepository<UserScreenItem> userScreenItemRepository;
        private readonly ProjectDbContext dbContext;

        public ManhourReport(IBaseRepository<UserScreenItem> userScreenItemRepository, ProjectDbContext dbContext)
        {
            this.userScreenItemRepository = userScreenItemRepository;
            this.dbContext = dbContext;
        }

        public async Task<List<GroupNames>> GroupNames()
        {
            return await userScreenItemRepository.Select<GroupNames>(QueryLoader.GetQuery("ManhourReport", "GroupName"));
        }

        public async Task<List<UserName>> UserNames(string GroupCode)
        {
            var tmp = (from un in dbContext.Users
                       where un.Group_code == GroupCode
                       select (new UserName()
                       {
                           UserCode = un.User_no,
                           User_Name = un.User_name
                       })
                       );
            return tmp.ToList();
        }
    }
}
