using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Child
    {
        private int aid { get; set; }
        private long gpid { get; set; }
        private long pid { get; set; }
        private long cid { get; set; }
        private string name { get; set; }
        private string[] values { get; set; }

        public int AccountId
        {
            get { return aid; }
            set { aid = value; }
        }

        public long Gpid
        {
            get { return gpid; }
            set { gpid = value; }
        }

        public long Pid
        {
            get { return pid; }
            set { pid = value; }
        }

        public long Id
        {
            get { return cid; }
            set { cid = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string[] Values
        {
            get { return values; }
            set { values = value; }
        }
    }
}
