using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<Category> categoryRepository;
        public CategoryService(IBaseRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public Task<int> Create(Category model)
        {
            throw new NotImplementedException();
        }

        public void Delete(Category category)
        {
            categoryRepository.Delete(category);
        }

        public Task<int> Edit(Category model)
        {
            throw new NotImplementedException();
        }


        public async Task<Category> Get(int id)
        {
            var category = await categoryRepository.Get(id);
            return category;
        }

        public async Task<List<Category>> Gets()
        {
            return await categoryRepository.Gets();
        }
    }
}
