using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMSGatchaEngine.Utils
{
    public enum LogType
    {
        LOG = 0,
        WARNING = 1,
        ERROR = 2
    }

    public interface IEngineLogger
    {
        /// <summary>
		/// Write a log to a file
		/// </summary>
		/// <param name="log">The logged object</param>
		void Log(object log);
        /// <summary>
		/// Write a log to a file using a format
		/// </summary>
		/// <param name="format">The format</param>
		/// <param name="log">The logged object</param>
		void LogFormat(string format, params object[] log);
        /// <summary>
		/// Write a warning log to a file
		/// </summary>
		/// <param name="log">The logged object</param>
		void LogWarning(object log);
        /// <summary>
		/// Write a warning log to a file using a format
		/// </summary>
		/// <param name="format">The format</param>
		/// <param name="log">The logged object</param>
		void LogWarningFormat(string format, params object[] log);
        /// <summary>
		/// Write a error log to a file
		/// </summary>
		/// <param name="log">The logged object</param>
		void LogError(object log);
        /// <summary>
		/// Write a error log to a file using a format
		/// </summary>
		/// <param name="format">The format</param>
		/// <param name="log">The logged object</param>
		void LogErrorFormat(string format, params object[] log);
        /// <summary>
        /// Write a log to a file
        /// </summary>
        /// <param name="logType">The log type</param>
        /// <param name="log">The logged object</param>
        /// <param name="stackTrace">The stacktrace for the log</param>
        void Log(LogType logType, object log, string stackTrace);
    }
}
