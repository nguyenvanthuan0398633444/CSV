using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Entity
{
    [Table(name: "Category", Schema = "public")]
    public class Category
    {
        [Column(name: "CategoryId")]
        [Key]
        public int CategoryId { get; set; }

        [Column(name: "CategoryName")]
        public string CategoryName { get; set; }
    }
}
