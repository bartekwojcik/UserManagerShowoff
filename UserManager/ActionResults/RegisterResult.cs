using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Contract;

namespace UserManager.ActionResults
{
    public class RegisterResult : ResultBase
    {
        public RegisterResult(bool isSuccess, IEnumerable<string> errors = null) : base(isSuccess, errors)
        {            
        }       
    }
}
