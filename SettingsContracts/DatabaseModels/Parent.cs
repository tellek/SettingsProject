using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Parent
    {
        private int aid { get; set; }
        private long gpid { get; set; }
        private long pid { get; set; }
        private string p_name { get; set; }

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

        public long Id
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
