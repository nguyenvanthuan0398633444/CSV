using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class ManhourUpdateViewModel : Manhour
    {

        public string User_name { get; set; }

        public string Theme_name1 { get; set; }
        public string Work_contents_code_name { get; set; }
        public void setTotal()
        {
                this.Total = this.Day1 + this.Day2 + this.Day3 + this.Day4 + this.Day5 + this.Day6 + this.Day7 + this.Day8 + this.Day9 + this.Day10
               + this.Day11 + this.Day12 + this.Day13 + this.Day14 + this.Day15 + this.Day16 + this.Day17 + this.Day18 + this.Day19 + this.Day20
               + this.Day21 + this.Day22 + this.Day23 + this.Day24 + this.Day25 + this.Day26 + this.Day27 + this.Day28 + this.Day29 + this.Day30 + this.Day31;
               
        }
        List<int> Holiday { get; set; }
    }
}
