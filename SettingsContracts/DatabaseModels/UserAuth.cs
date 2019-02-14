using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsContracts.DatabaseModels
{
    public class UserAuth
    {
        public long UserId { get; set; }
        public string SecretPass { get; set; }
        public string Email { get; set; }
        public Permissions Access { get; set; }

        public UserAuth()
        {
            Access = new Permissions();
        }

        private int account
        {
            set { Access.Account = value; }
        }
        private long[] gpids
        {
            set { Access.Gpids = value; }
        }
        private long[] pids
        {
            set { Access.Pids = value; }
        }
        private long[] cids
        {
            set { Access.Cids = value; }
        }
        private long[] gcids
        {
            set { Access.Gcids = value; }
        }
        private string[] scopes
        {
            set { Access.Scopes = value; }
        }
    }

    public class Permissions
    {
        public int Account { get; set; }
        public long[] Gpids { get; set; }
        public long[] Pids { get; set; }
        public long[] Cids { get; set; }
        public long[] Gcids { get; set; }
        public string[] Scopes { get; set; }
    }
}
