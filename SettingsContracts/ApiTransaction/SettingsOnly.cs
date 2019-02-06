using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SettingsContracts.ApiTransaction
{
    public class SettingsOnly
    {
        [Required]
        public string Name { get; set; }

        public string[] Values { get; set; }
    }
}
