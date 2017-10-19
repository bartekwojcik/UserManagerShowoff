using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.ActionResults;

namespace UserManager.Contract
{
    public interface IUserManager
    {    
        LoginResult Login(string login, string password);
        RegisterResult RegisterUser(string login, string password, string passwordConfirmation);
        InitializationResult InitializeDatabase(IDatabaseInitializer initializeService);
    }
}
