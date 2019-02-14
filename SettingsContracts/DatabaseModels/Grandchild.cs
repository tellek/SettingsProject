using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class Grandchild
    {
        private long cid { get; set; }
        private long gcid { get; set; }
        private string gc_name { get; set; }
        private string[] gc_values { get; set; }

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
