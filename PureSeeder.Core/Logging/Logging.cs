using System;
using System.ComponentModel;

namespace PureSeeder.Core.Logging
{
    public class Logger
    {
        private static Logger _instance;

        private BindingList<LogMessage> _log; 

        private Logger()
        {
            _log = new BindingList<LogMessage>();
        }

        private static Logger GetInstance()
        {
            if(_instance == null)
                _instance = new Logger();

            return _instance;
        }

        public static BindingList<LogMessage> LogList
        {
            get { return GetInstance()._log; }
        } 

        public static void Log(string logMessage)
        {
            var logger = GetInstance();
            logger.LogMessage(logMessage);
        }

        public void LogMessage(string logMessage)
        {
            _log.Add(new LogMessage(logMessage));
        }
    }

    public class LogMessage
    {
        public LogMessage(string message)
        {
            Message = message;
            Time = DateTime.Now;
        }
        public string Message { get; private set; }
        public DateTime Time { get; private set; }
    }
}