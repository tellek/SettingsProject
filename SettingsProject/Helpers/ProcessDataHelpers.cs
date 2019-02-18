using SettingsContracts;
using SettingsContracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SettingsProject.Helpers
{
    public static class ProcessDataHelpers
    {
        public static ProcessData InitiateProcessData(Permissions access, Resource resource, string aid, string gpid, string pid, string cid, string gcid)
        {
            var n_aid = TryToParseToInt(aid);
            var n_gpid = TryToParseToLong(gpid);
            var n_pid = TryToParseToLong(pid);
            var n_cid = TryToParseToLong(cid);
            var n_gcid = TryToParseToLong(gcid);

            var pd = new ProcessData();
            // Only populate data that is numerical.
            pd.AccountId = (n_aid.isNotNull && n_aid.isLong) ? n_aid.value : null;
            pd.Gpid = (n_gpid.isNotNull && n_gpid.isLong) ? n_gpid.value : null;
            pd.Pid = (n_pid.isNotNull && n_pid.isLong) ? n_pid.value : null;
            pd.Cid = (n_cid.isNotNull && n_cid.isLong) ? n_cid.value : null;
            pd.Gcid = (n_gcid.isNotNull && n_gcid.isLong) ? n_gcid.value : null;
            // Only populate data that is a string.
            pd.AccountName = (n_aid.isNotNull && n_aid.isString) ? n_aid.value : null;
            pd.Gpname = (n_gpid.isNotNull && n_gpid.isString) ? n_gpid.value : null;
            pd.Pname = (n_pid.isNotNull && n_pid.isString) ? n_pid.value : null;
            pd.Cname = (n_cid.isNotNull && n_cid.isString) ? n_cid.value : null;
            pd.Gcname = (n_gcid.isNotNull && n_gcid.isString) ? n_gcid.value : null;

            pd.Resource = resource;
            pd.Access = access;

            return pd;
        }

        private static (bool isNotNull, bool isString, bool isLong, dynamic value) TryToParseToLong(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return (false, false, false, null);
            bool success = Int64.TryParse(value, out long number);
            if (success) return (true, false, true, number);
            else return (true, true, false, value);
        }

        private static (bool isNotNull, bool isString, bool isLong, dynamic value) TryToParseToInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return (false, false, false, null);
            bool success = int.TryParse(value, out int number);
            if (success) return (true, false, true, number);
            else return (true, true, false, value);
        }
    }
}
