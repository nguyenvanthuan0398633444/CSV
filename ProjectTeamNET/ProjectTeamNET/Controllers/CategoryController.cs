using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace ProjectTeamNET.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

      

        public CategoryController(ICategoryService categoryService)
        { 
            this.categoryService = categoryService;
         
        }

      

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await categoryService.Gets();
            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await categoryService.Get(3);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            if (ModelState.IsValid)
            {
           
                int categoy = await categoryService.Create(model);
                if (categoy > 0)
                {
                    return Redirect("~/Category/Index");
                }
            }
            return View();
        }

    }
}
