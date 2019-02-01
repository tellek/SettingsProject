using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Parent : BaseEntity
    {
        private Int64 gpid { get; set; }
        private Int64 pid { get; set; }
        private string p_name { get; set; }

        public Int64 Gpid
        {
            get { return gpid; }
            set { gpid = value; }
        }
        public Int64 Id
        {
            get { return pid; }
            set { pid = value; }
        }
        public string Name
        {
            get { return p_name; }
            set { p_name = value; }
        }
    }
}
