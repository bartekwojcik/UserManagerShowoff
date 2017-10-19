using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.ActionResults;

namespace UserManager.Contract
{
    public interface ILoginService
    {
        LoginResult Login(string login, string password);
    }
}
