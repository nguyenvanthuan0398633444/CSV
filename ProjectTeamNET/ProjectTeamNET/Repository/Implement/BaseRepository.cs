using ProjectTeamNET.Models;
using ProjectTeamNET.Repository.Interface;

namespace ProjectTeamNET.Repository.Implement
{
    public class BaseRepository<T>: IBaseRepository<T> where T : class
    {
        public BaseRepository(ProjectDbContext context)
        {
            dbset = context.Set<T>();
            this.context = context;
        }
    }
}
