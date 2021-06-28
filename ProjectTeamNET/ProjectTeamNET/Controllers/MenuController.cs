using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Service.Interface;
using System.Threading.Tasks;

namespace ProjectTeamNET.Controllers
{
    public class MenuController : Controller
    {

        private readonly IMenuService service;

        public MenuController(IMenuService service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
          
            var result = await service.SendDataToController();
          
            return View(result);
        }


    }
}
