using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Grandparent : BaseEntity
    {
        private Int64 gpid { get; set; }
        private string gp_name { get; set; }

        public Int64 Id
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
