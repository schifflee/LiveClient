using Microsoft.Practices.Prism.Logging;
using System;

namespace PowerCreator.LiveClient.Log
{
    public static class LoggerExtensions
    {
        public static void Info(this ILoggerFacade logger, string msg)
        {
            logger.Log(msg, Category.Info, Priority.None);
        }
        public static void Error(this ILoggerFacade logger, string msg)
        {
            logger.Log(msg, Category.Exception, Priority.None);
        }
        public static void Error(this ILoggerFacade logger, Exception exception)
        {
            logger.Log(string.Format("ErrMsg:{0},StackTrace:{1},Source:{2}", exception.Message, exception.StackTrace, exception.Source),Category.Exception,Priority.None);
        }
    }
}
