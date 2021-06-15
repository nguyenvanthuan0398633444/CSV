using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSVfile.Models
{
    public class FileModel
    {
        public IFormFile Csv { get; set; }
    }
}
