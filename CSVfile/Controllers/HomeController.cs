using CSVfile.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CSVfile.Controllers
{
    

    public class HomeController : Controller
    {
        IWebHostEnvironment hosting;
        public HomeController(IWebHostEnvironment hostEnvironment)
        {
            this.hosting = hostEnvironment;
        }
        private List<Employer> employers = new List<Employer>
        {
            new Employer{id= 1, name= "thuan", city="hue"},
            new Employer{id= 2, name= "thuan1", city="hue1"},
            new Employer{id= 3, name= "thuan2", city="hue2"},
            new Employer{id= 4, name= "thuan3", city="hue3"},
            new Employer{id= 5, name= "thuan4", city="hue4"}
        };
           
  
     
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index1()
        {
            return View(employers);
        }
        public IActionResult Index2()
        {
            var buider =new StringBuilder();
            buider.AppendLine("id,name,city");
            foreach(var item in employers)
            {
                buider.AppendLine($"{item.id}, {item.name}, {item.city}");
            }
            return File(Encoding.UTF8.GetBytes(buider.ToString()), "text/csv", "employer.csv");
        }
        public IActionResult Index3()
        {
            return View();
        }
        public IActionResult PetList(FileModel model)
        {
            List<PetsModel> ojb = new List<PetsModel>();
            PetsModel petsModel = null;
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Csv != null)
                {
                    string uploadFld = Path.Combine(this.hosting.WebRootPath, "csv");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Csv.FileName;
                    string filePath = Path.Combine(uploadFld, uniqueFileName);
                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        model.Csv.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    string[] lines = System.IO.File.ReadAllLines(filePath);
                    for(int i = 1; i < lines.Length; i++)
                    {
                        string[] fileds = lines[i].Split(",");
                        petsModel = new PetsModel();
                        petsModel.PetName = fileds[0];
                        petsModel.Age = Convert.ToInt32(fileds[1]);
                        petsModel.Geder = fileds[2];
                        ojb.Add(petsModel);
                    }
                }
            }
            return View(ojb);
        }
        [HttpPost]
        public IActionResult UploadCsvFile(FileModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if(model.Csv != null)
                {
                    string uploadFld = Path.Combine(this.hosting.WebRootPath,"csv");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Csv.FileName;
                    string filePath = Path.Combine(uploadFld, uniqueFileName);
                    model.Csv.CopyToAsync(new FileStream(filePath, FileMode.Create));
                }
            }
            return RedirectToAction("PetList");
        }
    }
}
