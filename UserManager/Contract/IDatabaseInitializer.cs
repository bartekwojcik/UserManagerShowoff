using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.ActionResults;

namespace UserManager.Contract
{
    public interface IDatabaseInitializer
    {
        InitializationResult Initialize();
        //string RegisterCommand();
        //todo for instance two method to return data or scripts for login and register, or whatever
    }
}
