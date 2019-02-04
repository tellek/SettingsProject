using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Grandparent
    {
        private int aid { get; set; }
        private long gpid { get; set; }
        private string name { get; set; }
        private string values { get; set; }

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
            get { return name; }
            set { name = value; }
        }

        public string Values
        {
            get { return values; }
            set { values = value; }
        }

    }
}
