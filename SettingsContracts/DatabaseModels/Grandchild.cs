using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Grandchild : BaseEntity
    {
        private Int64 gpid { get; set; }
        private Int64 pid { get; set; }
        private Int64 cid { get; set; }
        private Int64 gcid { get; set; }
        private string gc_name { get; set; }

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
        public Int64 Cid
        {
            get { return cid; }
            set { cid = value; }
        }
        public Int64 Id
        {
            get { return gcid; }
            set { gcid = value; }
        }
        public string Name
        {
            get { return gc_name; }
            set { gc_name = value; }
        }
    }
}
