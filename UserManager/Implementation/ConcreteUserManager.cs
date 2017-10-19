using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.ActionResults;
using UserManager.Contract;

namespace UserManager.Implementation
{
    public class ConcreteUserManager : IUserManager
    {
        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;

        public ConcreteUserManager(IRegisterService registerService, ILoginService loginService)
        {
            this._registerService = registerService;
            this._loginService = loginService;
        }

        public InitializationResult InitializeDatabase(IDatabaseInitializer initializeService)
        {
            return initializeService.Initialize();
        }

        public LoginResult Login(string login, string password)
        {
            return _loginService.Login(login, password);
        }

        public RegisterResult RegisterUser(string login, string password, string passwordConfirmation)
        {
            return _registerService.Register(login, password, passwordConfirmation);
        }
    }
}
