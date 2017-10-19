using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.ActionResults
{
    public class InitializationResult : ResultBase
    {
        public InitializationResult(bool isSuccess, IEnumerable<string> errors = null) : base(isSuccess, errors)
        {
        }        
    }
}
