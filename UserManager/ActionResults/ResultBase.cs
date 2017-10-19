using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.ActionResults
{
    public abstract class ResultBase
    {
        public ResultBase(bool isSucess, IEnumerable<string> errors)
        {
            IsSuccess = isSucess;
            Errors = errors ?? new List<string>();
        }
        public bool IsSuccess { get; protected set; }
        public IEnumerable<string> Errors { get; protected set; }
    }
}
