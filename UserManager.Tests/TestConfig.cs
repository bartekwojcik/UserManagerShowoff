using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Tests
{
    public static class TestConfig
    {
        public static string TestConnectionString => @"Data Source=.\SQLEXPRESS;
                          AttachDbFilename=|DataDirectory|\TestData\TestData.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;
                          User Instance=True";
    }
}
