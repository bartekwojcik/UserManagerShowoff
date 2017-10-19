using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Helpers
{

    internal static class ExceptionHelper
    {
        internal static List<string> FlatternMessages(this Exception e, List<string> flatteredMessages = null)
        {
            if (flatteredMessages == null)
            {
                flatteredMessages = new List<string>();
            }
            flatteredMessages.Add(e.Message);
            if (e.InnerException != null)
            {
                return e.InnerException.FlatternMessages(flatteredMessages);
            }
            else
            {
                return flatteredMessages;
            }
        }
    }
}
