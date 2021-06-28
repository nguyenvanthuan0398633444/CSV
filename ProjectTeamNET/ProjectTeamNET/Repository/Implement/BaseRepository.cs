using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectTeamNET.Models;
using ProjectTeamNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Npgsql;
using System.Threading.Tasks;

namespace ProjectTeamNET.Repository.Implement
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private DbSet<T> dbset;
        private readonly ProjectDbContext context;

        public BaseRepository(ProjectDbContext context)
        {
            dbset = context.Set<T>();
            this.context = context;
        }

        public async Task<T> Get(object id)
        {
            var result = await dbset.FindAsync(id);
            return result;
        }


        public void Delete(T obj)
        {
            dbset.Remove(obj);
        }
        public async Task<int> Create(T obj)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    dbset.Add(obj);
                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return context.SaveChanges();
        }

        public async Task<List<T>> Gets()
        {
            var result = await dbset.ToListAsync();
            return result;
        }

        public async Task<List<M>> Search<M>(string sql, object param)
        {
            var query = await context.Database.GetDbConnection().QueryAsync<M>(sql, param);
            return query.ToList();
        }
        public async Task<M> MenuSearch<M>(string sql, object param)
        {
            var query = await context.Database.GetDbConnection().QueryAsync<M>(sql, param);
            return query.FirstOrDefault();
        }

        public async Task<List<T1>> Select<T1>(string sql)
        {
            var query = await context.Database.GetDbConnection().QueryAsync<T1>(sql);
            return query.ToList();
        }
        public async Task<List<T1>> Select<T1>(string sql, object param)
        {
            var query = await context.Database.GetDbConnection().QueryAsync<T1>(sql, param);
            return query.ToList();
        }
        public async Task<int> Update(string sql, object param)
        {
            var result = 0;
            try
            {
                result = await context.Database.GetDbConnection().ExecuteAsync(sql, param);
            }
            catch
            {
                result = 0;
            }
            return result;
        }
        public void Update<M>(string sql, object param)
        {
            context.Database.GetDbConnection().QueryAsync<M>(sql, param);
        }
        public void Insert<M>(string sql, object param)
        {
            context.Database.GetDbConnection().QueryAsync<M>(sql, param);
        }


    }

}
