using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Grandparent
    {
        private int aid { get; set; }
        private long gpid { get; set; }
        private string gp_name { get; set; }

        public int AccountId
        {
            get { return aid; }
            set { aid = value; }
        }

        public long Id
        {
            get { return gpid; }
            set { gpid = value; }
        }

        public string Name
        {
            get { return gp_name; }
            set { gp_name = value; }
        }

    }
}
