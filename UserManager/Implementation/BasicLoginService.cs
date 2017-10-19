using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.ActionResults;
using UserManager.Contract;

namespace UserManager.Implementation
{
    public class BasicLoginService : ILoginService
    {
        private string _connectionString;

        public BasicLoginService(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public LoginResult Login(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return new LoginResult(false, new List<string> { "Password or login is empty" });
            }
            //sprawdz czy jest taki juser
            ValidateUserResult userResult = CheckIfUserExists(login);

            //sprawdz czy hash hasla sie zgadza
            //jak sie zgadza wszystko to zwróc token i jego expiration date, zapisz go też w bazie
        }

        private ValidateUserResult CheckIfUserExists(string login)
        {
            var con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();
            var getUserByLoginCommand = @"select * from [dbo].[Users] where "
            try
            {
                using (SqlCommand cmd = new SqlCommand(_registerCommand, con))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@SaltedPassword", hashedPassword);
                    var result = cmd.ExecuteNonQuery();
                }
                return new RegisterResult(true);
            }
            catch (Exception e)
            {
                var messages = e.FlatternMessages();
                return new RegisterResult(false, messages);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private class ValidateUserResult : ResultBase
        {
            public ValidateUserResult(bool isSucess, string hashedPasswordFromDb, IEnumerable<string> errors = null) : base(isSucess, errors)
            {
                HashedPasswordFromDb = hashedPasswordFromDb;
            }

            public string HashedPasswordFromDb { get; }
        }
    }
}
