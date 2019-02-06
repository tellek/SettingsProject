using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Grandparent
    {
        private int aid { get; set; }
        private long gpid { get; set; }
        private string gp_name { get; set; }
        private string[] gp_values { get; set; }

        [Required]
        public int AccountId
        {
            get { return aid; }
            set { aid = value; }
        }

        [Required]
        public long Id
        {
            get { return gpid; }
            set { gpid = value; }
        }

        [Required]
        public string Name
        {
            get { return gp_name; }
            set { gp_name = value; }
        }

        public string[] Values
        {
            get { return gp_values; }
            set { gp_values = value; }
        }

    }
}
