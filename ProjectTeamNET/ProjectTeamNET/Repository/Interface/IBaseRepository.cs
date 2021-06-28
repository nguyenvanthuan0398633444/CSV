﻿using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectTeamNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Repository.Interface
{
    public interface IBaseRepository<T> where T : class
    {

        Task<T> Get(object id);
        Task<List<T>> Gets();
        void Delete(T obj);
        Task<int> Create(T obj);
        Task<List<T1>> Select<T1>(string sql);
        Task<int> Update(string sql, object param);
        Task<List<T1>> Select<T1>(string sql, object param);
        Task<List<M>> Search<M>(string sql, object param);
        Task<M> MenuSearch<M>(string sql, object param);
 
        
        void Insert<M>(string sql, object param);
        void Update<M>(string sql, object param);
       
       
    }
}
