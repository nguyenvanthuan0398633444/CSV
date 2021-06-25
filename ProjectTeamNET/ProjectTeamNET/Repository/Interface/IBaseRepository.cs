using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectTeamNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Repository.Interface
{
    public abstract class IBaseRepository<T> where T : class
    {
        public DbSet<T> dbset;
        public ProjectDbContext context;

        public async Task<T> Get(object id)
        {
            var result = await dbset.FindAsync(id);
            return result;
        }

        public async Task<int> Delete(object id)
        {
            var tmp = await Get(id);
            dbset.Remove(tmp);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Create(T obj)
        {
            var result = -1;
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    await dbset.AddAsync(obj);
                    result = await context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return result;
        }

        public async Task<List<T>> Gets()
        {
            var result = await dbset.ToListAsync();
            return result;
        }

        public List<object> Search(object obj)
        {
            var result = context.Database.GetDbConnection().Query<object>("", obj).ToList();
            return result;
        }
        public async Task<List<M>> Select<M>(string sql)
        {
            var query = await context.Database.GetDbConnection().QueryAsync<M>(sql);
            return query.ToList();
        }
        public async Task<List<M>> Search<M>(string sql, object param)
        {
            var query = await context.Database.GetDbConnection().QueryAsync<M>(sql, param);
            return query.ToList();
        }
    }
}
