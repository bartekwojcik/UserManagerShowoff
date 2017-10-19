using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserManager.ActionResults;
using UserManager.Contract;
using UserManager.Helpers;
using UserManager.Misc;

namespace UserManager.Implementation
{
    public class BasicRegisterService : IRegisterService
    {
        private readonly string _registerCommand;
        private readonly string _connectionString;
        private readonly string _getUserByLoginCommand;

        public BasicRegisterService(string connectionString)
        {
            this._registerCommand = SqlHelper.FileToSql(DefaultConfig.RegisterScriptPath);
            this._connectionString = connectionString;
            _getUserByLoginCommand = SqlHelper.FileToSql(DefaultConfig.GetUserByLoginScriptPath);
        }

        public RegisterResult Register(string login, string password, string passwordConfirmation)
        {
            if (password != passwordConfirmation)
            {
                return new RegisterResult(false, new List<string>() { "Passwords does not match" });
            }

            var validateResult = ValidateUser(login);
            if (!validateResult.IsSuccess)
            {
                return new RegisterResult(validateResult.IsSuccess, validateResult.Errors);
            }

            var hashedPassword = CryptoHelper.HashPassword(password);

            var con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();
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

        private ValidateResult ValidateUser(string login)
        {
            var con = new SqlConnection();
            con.ConnectionString = _connectionString;
            con.Open();
            try
            {
                using (SqlCommand cmd = new SqlCommand(_getUserByLoginCommand, con))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string column = reader["Login"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(column))
                        {
                            return new ValidateResult(false, new List<string>() { $"User {login} exists" });
                        }
                    }
                    //if (reader.GetString(0) != null)
                    //{
                    //    return new ValidateResult(false, new List<string>() { $"User {login} exists" });
                    //}
                }
                return new ValidateResult(true);
            }
            catch (Exception e)
            {
                var messages = e.FlatternMessages();
                messages.Insert(0, "Error occurred!");
                return new ValidateResult(false, messages);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private class ValidateResult : ResultBase
        {
            public ValidateResult(bool isSucess, IEnumerable<string> errors = null) : base(isSucess, errors)
            {
            }
        }
    }
}
