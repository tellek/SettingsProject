using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.Models
{
    public class ErrorDetails
    {
        public string Error { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
