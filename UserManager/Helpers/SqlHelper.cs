using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Helpers
{
    internal static class SqlHelper
    {
        internal static string FileToSql(string filePath)
        {
            string script = File.ReadAllText(filePath);
            return script;
        }
    }
}
