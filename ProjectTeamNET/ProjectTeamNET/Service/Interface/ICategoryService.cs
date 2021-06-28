using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Interface
{
    public interface ICategoryService
    {
        Task<List<Category>> Gets();
        Task<Category> Get(int id);
        Task<int> Create(Category model);
        Task<int> Edit(Category model);
        void Delete(Category obj);
    }
}
