using System;
using System.Collections.Generic;
using System.Text;

namespace Cursach.Model
{
    public class playerModel
    {
        public string name { get; set; }
        public double account_id { get; set; }
        //public double steamid { get; set; }
        public string avatarfull { get; set; }
        public string personaname { get; set; }
        public string loccountrycode { get; set; }
        public string last_match_time { get; set; }
        public int team_id { get; set; }
        public string team_name { get; set; }
        public string team_tag { get; set; }
        public bool is_pro { get; set; }
        public int fantasy_role { get; set; }
        public string profileurl { get; set; }
    }
}
