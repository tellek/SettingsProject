using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Grandchild
    {
        private int aid { get; set; }
        private long gpid { get; set; }
        private long pid { get; set; }
        private long cid { get; set; }
        private long gcid { get; set; }
        private string gc_name { get; set; }
        private string[] gc_values { get; set; }

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
        public long Cid
        {
            get { return cid; }
            set { cid = value; }
        }

        [Required]
        public long Id
        {
            get { return gcid; }
            set { gcid = value; }
        }

        [Required]
        public string Name
        {
            get { return gc_name; }
            set { gc_name = value; }
        }

        public string[] Values
        {
            get { return gc_values; }
            set { gc_values = value; }
        }
    }
}
