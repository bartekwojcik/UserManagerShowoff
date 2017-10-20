using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.ActionResults;
using UserManager.Contract;
using UserManager.Helpers;
using UserManager.Misc;

namespace UserManager.Implementation
{
    public class BasicLoginService : ILoginService
    {
        private string _connectionString;
        private readonly TimeSpan _tokenValidityTime;
        private readonly string _saveTokenScript;

        public BasicLoginService(string connectionString, TimeSpan tokenValidityTime)
        {
            this._connectionString = connectionString;
            this._tokenValidityTime = tokenValidityTime;
            this._saveTokenScript = SqlHelper.FileToSql(DefaultConfig.SaveTokenScript);
        }
        public virtual LoginResult Login(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return new LoginResult(false, new List<string> { "Password or login is empty" });
            }
            //sprawdz czy jest taki juser
            ValidateUserResult userResult = CheckIfUserExists(login);
            if (!userResult.IsSuccess || string.IsNullOrWhiteSpace(userResult.HashedPasswordFromDb))
            {
                return new LoginResult(userResult.IsSuccess, userResult.Errors);
            }
            //sprawdz czy hash hasla sie zgadza

            bool isPasswordOk = CheckPasswords(password, userResult.HashedPasswordFromDb);
            if (!isPasswordOk)
            {
                return new LoginResult(false, new List<string> { "Login or password incorrect" });
            }

            return LogInUser(login);

          
        }

        private LoginResult LogInUser(string login)
        {
            var expirationTime = DateTime.UtcNow.Add(_tokenValidityTime);
            string token = CryptoHelper.GenerateToken(expirationTime);

            var con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand(_saveTokenScript, con))
                {
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@TokenExpirationDate", expirationTime);
                    cmd.Parameters.AddWithValue("@Login", login);
                    var result = cmd.ExecuteNonQuery();
                }
                return new LoginResult(true, token, expirationTime);
            }
            catch (Exception e)
            {
                var messages = e.FlattenMessages();
                return new LoginResult(false, messages);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private bool CheckPasswords(string password, string hashedPasswordFromDb)
        {
            return CryptoHelper.ComparePasswords(password, hashedPasswordFromDb);
        }

        private ValidateUserResult CheckIfUserExists(string login)
        {
            var con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();
            //todo switch to script from file
            var getUserByLoginCommand = @"select * from [dbo].[Users] where Login = @Login";
            try
            {
                using (SqlCommand cmd = new SqlCommand(getUserByLoginCommand, con))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string hasedPassword = reader["SaltedPassword"]?.ToString();
                        return new ValidateUserResult(true, hasedPassword);
                    }
                }
                return new ValidateUserResult(new List<string>() { "User does not exists" });
            }
            catch (Exception e)
            {
                var messages = e.FlattenMessages();
                return new ValidateUserResult(messages);
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
            public ValidateUserResult(IEnumerable<string> errors, bool isSucess = false) : base(isSucess, errors)
            {
            }

            public string HashedPasswordFromDb { get; }
        }
    }
}
