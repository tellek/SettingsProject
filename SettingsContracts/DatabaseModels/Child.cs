using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Child : BaseEntity
    {
        private Int64 gpid { get; set; }
        private Int64 pid { get; set; }
        private Int64 cid { get; set; }
        private string c_name { get; set; }

        public Int64 Gpid
        {
            get { return gpid; }
            set { gpid = value; }
        }
        public Int64 Pid
        {
            get { return pid; }
            set { pid = value; }
        }
        public Int64 Id
        {
            get { return cid; }
            set { cid = value; }
        }
        public string Name
        {
            get { return c_name; }
            set { c_name = value; }
        }
    }
}
