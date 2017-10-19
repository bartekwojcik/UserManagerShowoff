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

        public ConcreteUserManager(IRegisterService registerService)
        {
            this._registerService = registerService;
        }

        public InitializationResult InitializeDatabase(IDatabaseInitializer initializeService)
        {
            return initializeService.Initialize();
        }

        public LoginResult Login(string login, string password)
        {
            throw new NotImplementedException();
        }

        public RegisterResult RegisterUser(string login, string password, string passwordConfirmation)
        {
            return _registerService.Register(login, password, passwordConfirmation);
        }
    }
}
