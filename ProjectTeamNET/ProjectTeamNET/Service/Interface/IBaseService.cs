using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Interface
{
    public interface IBaseService<T>
    {
        Task<List<T>> Gets();
        Task<T> Get(int id);
        Task<int> Create(T model);
        Task<int> Edit(T model);
        void Delete(T obj);
    }
}
