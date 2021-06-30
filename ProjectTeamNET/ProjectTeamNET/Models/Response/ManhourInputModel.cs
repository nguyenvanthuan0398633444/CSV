using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{ 
    public class ManhourInputModel
    {
        public List<ManhourInput> ManhourDatas { get; set; }
        public string DateSelect { get; set; }
        public List<string> ListDateOfWeek { get; set; }
        public string ScreenOutput { get; set; }
        public List<int> Horlidays { get; set; }

    }
}
