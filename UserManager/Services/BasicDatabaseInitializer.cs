using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.ActionResults;
using UserManager.Contract;
using UserManager.Misc;
using UserManager.Helpers;

namespace UserManager.Services
{
    public class BasicDatabaseInitializer : IDatabaseInitializer
    {
        private readonly string _initScript;
        private readonly string _connectionString;


        public BasicDatabaseInitializer()
        {
            _initScript = SqlHelper.FileToSql(DefaultConfig.InitScriptPath);
            _connectionString = DefaultConfig.DefaultConnectionString;
        }

        public BasicDatabaseInitializer(string connectionString) : this()
        {
            this._connectionString = connectionString;
        }

        public virtual InitializationResult Initialize()
        {
            var con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand(_initScript, con))
                {
                    var result = cmd.ExecuteNonQuery();
                }
                return new InitializationResult(true);
            }
            catch (Exception e)
            {
                var messages = e.FlattenMessages();
                return new InitializationResult(false, messages);
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

