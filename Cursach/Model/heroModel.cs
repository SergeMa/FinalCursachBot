using System;
using System.Collections.Generic;
using System.Text;

namespace Cursach.Model
{
    public class heroModel
    {
        public int id { get; set; }
        public string localized_name { get; set; }
        public string primary_attr { get; set; }
        public string attack_type { get; set; }
        public List<string> roles { get; set; }
        public string img { get; set; }
        public string icon { get; set; }
        public int base_health { get; set; }
        public double base_health_regen { get; set; }
        public int base_mana { get; set; }
        public double base_mana_regen { get; set; }
        public double base_armor { get; set; }
        public int base_mr { get; set; }
        public int base_attack_min { get; set; }
        public int base_attack_max { get; set; }
        public int base_str { get; set; }
        public int base_agi { get; set; }
        public int base_int { get; set; }
        public double str_gain { get; set; }
        public double agi_gain { get; set; }
        public double int_gain { get; set; }
        public int attack_range { get; set; }
        public int projectile_speed { get; set; }
        public double attack_rate { get; set; }
        public int move_speed { get; set; }
        public int turbo_picks { get; set; }
        public int turbo_wins { get; set; }
        public int pro_ban { get; set; }
        public int pro_win { get; set; }
        public int pro_pick { get; set; }
    }
}
