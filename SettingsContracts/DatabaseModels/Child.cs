using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Child
    {
        private int aid { get; set; }
        private long gpid { get; set; }
        private long pid { get; set; }
        private long cid { get; set; }
        private string c_name { get; set; }
        private string[] c_values { get; set; }

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
        public long Pid
        {
            get { return pid; }
            set { pid = value; }
        }

        [Required]
        public long Id
        {
            get { return cid; }
            set { cid = value; }
        }

        [Required]
        public string Name
        {
            get { return c_name; }
            set { c_name = value; }
        }

        public string[] Values
        {
            get { return c_values; }
            set { c_values = value; }
        }
    }
}
