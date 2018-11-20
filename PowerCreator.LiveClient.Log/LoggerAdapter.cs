using log4net;
using Microsoft.Practices.Prism.Logging;

namespace PowerCreator.LiveClient.Log
{
    public class LoggerAdapter : ILoggerFacade
    {
        private readonly ILog _infoLogger = null;
        private readonly ILog _errorLogger = null;

        public LoggerAdapter()
        {
            if (_infoLogger == null)
            {
                _infoLogger = LogManager.GetLogger("loginfo");
            }
            if (_errorLogger == null)
            {
                _errorLogger = LogManager.GetLogger("logerror");
            }
            log4net.Config.XmlConfigurator.Configure();

        }
        public void Log(string message, Category category, Priority priority)
        {
            if (string.IsNullOrEmpty(message))
                return;

            switch (category)
            {
                case Category.Exception:
                    this.Error(message);
                    break;
                case Category.Info:
                    this.Info(message);
                    break;
            }
        }
        private void Info(string message)
        {
            _infoLogger.Info(message);
        }

        private void Error(string message)
        {
            _errorLogger.Error(message);
        }
    }
}
