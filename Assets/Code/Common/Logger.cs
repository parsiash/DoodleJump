using UnityEngine;

namespace DoodleJump.Common
{
    public enum LogLevel
    {
        Error = 0,
        Info = 1,
        Debug = 2,
        Warning = 3,
        Trace = 4
    }

    public interface ILogger
    {
        void Log(LogLevel logLevel, string message);
    }

    public static class ILoggerExtensions
    {
        public static void Log(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Info, message);
        }

        public static void LogError(this ILogger logger, string errorMessage)
        {
            logger.Log(LogLevel.Error, errorMessage);
        }
    }

    public class Logger : ILogger
    {
        private static Logger instance;
        public static Logger Instance
        {
            get
            {
                instance = instance ?? new Logger();
                return instance;
            }
        }

        public void Log(LogLevel logLevel, string message)
        {
            switch(logLevel)
            {
                case LogLevel.Info:
                    Debug.Log(message);
                    break;

                case LogLevel.Error:
                    Debug.LogError(message);
                    break;

                case LogLevel.Warning:
                    Debug.LogWarning(message);
                    break;

                case LogLevel.Debug:
                    Debug.Log(message);
                    break;

                case LogLevel.Trace:
                    Debug.Log(message);
                    break;
            }
        }
    }
}