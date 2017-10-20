using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Tests.TempClasses
{
    public static class DbHelper
    {
        public const string TruncateCommandScript = @"TRUNCATE TABLE [dbo].[Users]";
        /// <summary>
        /// Truncates test user table
        /// </summary>        
        /// <returns>True if succeeded</returns>
        public static bool NukeAllTestUsers(string connectionString, string nukeScript)
        {
            var con = new SqlConnection();
            con.ConnectionString = connectionString;
            con.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand(nukeScript, con))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }              
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
}
