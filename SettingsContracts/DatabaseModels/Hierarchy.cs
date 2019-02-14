using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Hierarchy
    {
        public int aid { get; set; }
        public long gpid { get; set; }
        public long pid { get; set; }
        public long cid { get; set; }
        public long gcid { get; set; }

        public string gp_name { get; set; }
        public string p_name { get; set; }
        public string c_name { get; set; }
        public string gc_name { get; set; }
    }
}
