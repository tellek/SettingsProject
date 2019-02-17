using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.ApiTransaction.ResponseModels
{
    public class SingleSuccessResponse<T>
    {
        public T Result { get; set; }
        public bool Cached { get; set; }
    }
}
