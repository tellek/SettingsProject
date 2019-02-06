using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts
{
    public class ProcessData
    {
        public long UserId { get; set; }
        public int AccountId { get; set; }
        public long Gpid { get; set; }
        public long Pid { get; set; }
        public long Cid { get; set; }
        public long Gcid { get; set; }

        public string AccountName { get; set; }
        public string Gpname { get; set; }
        public string Pname { get; set; }
        public string Cname { get; set; }
        public string Gcname { get; set; }
    }
}
