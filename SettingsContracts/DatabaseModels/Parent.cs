using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Parent
    {
        private int aid { get; set; }
        private long gpid { get; set; }
        private long pid { get; set; }
        private string p_name { get; set; }
        private string[] p_values { get; set; }

        [Required]
        public int AccountId
        {
            get { return aid; }
            set { aid = value; }
        }

        [Required]
        public long Gpid
        {
            get { return gpid; }
            set { gpid = value; }
        }

        [Required]
        public long Id
        {
            get { return pid; }
            set { pid = value; }
        }

        [Required]
        public string Name
        {
            get { return p_name; }
            set { p_name = value; }
        }

        public string[] Values
        {
            get { return p_values; }
            set { p_values = value; }
        }
    }
}
