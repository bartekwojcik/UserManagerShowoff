using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.ActionResults;
using UserManager.Contract;

namespace UserManager.Services
{
    public class LoggableDatabaseInitializer : IDatabaseInitializer
    {
        private readonly IDatabaseInitializer _initializer;
        public IList<string> LoggedErrors { get; private set; }

        public LoggableDatabaseInitializer(IDatabaseInitializer initializer)
        {
            this._initializer = initializer;
            LoggedErrors = new List<string>();
        }

        public InitializationResult Initialize()
        {
            var result = _initializer.Initialize();
            if (!result.IsSuccess)
            {
                var messageToLog = new StringBuilder();
                messageToLog.AppendLine("Alas! Something went wrong!");
                foreach (var message in result.Errors)
                {
                    messageToLog.AppendLine(message);
                    LoggedErrors.Add(message);
                }
                Debug.WriteLine(messageToLog.ToString());
            }
            else
            {
                Debug.WriteLine("Initialization succeeded. Sacrebleu!");
            }
            return result;
        }
    }
}
