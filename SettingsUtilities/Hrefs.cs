using SettingsContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsUtilities
{
    public static class Hrefs
    {
        public static object BuildParameters(ProcessData pData, long createdRecordId)
        {
            switch (pData.Resource)
            {
                case Resource.None:
                    return null;
                case Resource.Account:
                    throw new NotImplementedException();
                case Resource.Grandparent:
                    return new { accountId = pData.AccountId, gpid = createdRecordId };
                case Resource.Parent:
                    return new { accountId = pData.AccountId, gpid = pData.Gpid, pid = createdRecordId };
                case Resource.Child:
                    return new { accountId = pData.AccountId, gpid = pData.Gpid, pid = pData.Pid, cid = createdRecordId };
                case Resource.Grandchild:
                    return new { accountId = pData.AccountId, gpid = pData.Gpid, pid = pData.Pid, cid = pData.Cid, gcid = createdRecordId };
                case Resource.User:
                    throw new NotImplementedException();
                default:
                    return null;
            }
        }
    }
}
