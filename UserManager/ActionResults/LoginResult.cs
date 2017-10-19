using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Contract;

namespace UserManager.ActionResults
{
    public class LoginResult : ResultBase
    {
        public LoginResult(bool isSucess, string token, DateTime tokenExpirationTime, IEnumerable<string> errors = null)
            : base(isSucess, errors)
        {
            Token = token;
            TokenExpiratioDate = tokenExpirationTime;
        }


        public string Token { get; }
        public DateTime TokenExpiratioDate { get; }
    }
}
