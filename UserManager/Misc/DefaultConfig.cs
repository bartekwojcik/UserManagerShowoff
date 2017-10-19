using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Misc
{
    internal static class DefaultConfig
    {
        internal static string DefaultConnectionString => @"Data Source=.\SQLEXPRESS;
                          AttachDbFilename=|DataDirectory|SampleDatabase.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;
                          User Instance=True";

        internal static string InitScriptPath => @"Scripts/InititDb.sql";
        internal static string RegisterScriptPath => @"Scripts/RegisterCommand.sql";
        internal static string GetUserByLoginScriptPath => @"Scripts/GetUserByLogin.sql";
    }
}
