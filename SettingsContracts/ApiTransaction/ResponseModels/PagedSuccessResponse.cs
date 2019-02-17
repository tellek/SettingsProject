using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.ApiTransaction.ResponseModels
{
    public class PagedSuccessResponse<T>
    {
        public IEnumerable<T> Results { get; set; }
        public bool Cached { get; set; }
    }
}
