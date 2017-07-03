using System.Collections;
using System.Collections.Generic;
using BMSGatchaEngine.Utils;
using System;
using System.IO;
using System.Text;

namespace BMSGatchaEngine.Runtime.Utils
{
    public class EngineLogger : IEngineLogger
    {
        #region Private Data
        /// <summary>
        /// The log location of this object
        /// </summary>
        private const string LOG_LOCATION = "Logs/CoreLog.txt";

        /// <summary>
        /// The stringbuilder for this core object
        /// </summary>
        private StringBuilder _internalStringBuilder = new StringBuilder();
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EngineLogger()
        {
            //Application.logMessageReceived += _handleUnityLog;
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~EngineLogger()
        {
            //Application.logMessageReceived -= _handleUnityLog;
        }
        #endregion

        #region Public API
        /// <summary>
        /// Generic Log
        /// </summary>
        /// <param name="log">Object to log</param>
        public void Log(object log)
        {
            Log(LogType.LOG, log, Environment.StackTrace);
        }

        /// <summary>
        /// Generic Log Format
        /// </summary>
        /// <param name="format">Format to follow</param>
        /// <param name="log">Logging arguments</param>
        public void LogFormat(string format, params object[] log)
        {
            Log(LogType.LOG, string.Format(format, log), Environment.StackTrace);
        }

        /// <summary>
        /// Log Warning
        /// </summary>
        /// <param name="log">Object to log</param>
        public void LogWarning(object log)
        {
            Log(LogType.WARNING, log, Environment.StackTrace);
        }

        /// <summary>
        /// Log Warning Format
        /// </summary>
        /// <param name="format">Format to follow</param>
        /// <param name="log">Logging arguments</param>
        public void LogWarningFormat(string format, params object[] log)
        {
            Log(LogType.WARNING, string.Format(format, log), Environment.StackTrace);
        }

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="log">Object to log</param>
        public void LogError(object log)
        {
            Log(LogType.ERROR, log, Environment.StackTrace);
        }

        /// <summary>
        /// Log Error Format
        /// </summary>
        /// <param name="format">Format to follow</param>
        /// <param name="log">Logging arguments</param>
        public void LogErrorFormat(string format, params object[] log)
        {
            Log(LogType.ERROR, string.Format(format, log), Environment.StackTrace);
        }

        /// <summary>
        /// Write a log to a file
        /// </summary>
        /// <param name="logType">The log type</param>
        /// <param name="log">The logged object</param>
        /// <param name="stackTrace">The stacktrace for the log</param>
        public void Log(LogType logType, object log, string stackTrace)
        {
            switch (logType)
            {
                case LogType.LOG:
                    UnityEngine.Debug.Log(log);
                    break;
                case LogType.WARNING:
                    UnityEngine.Debug.LogWarning(log);
                    break;
                case LogType.ERROR:
                    UnityEngine.Debug.LogError(log);
                    break;
            }
            _writeLog(logType, log, stackTrace);
        }
        #endregion

        #region Private API
        /// <summary>
        /// The Log location
        /// </summary>
        private string LogLocation
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(UnityEngine.Application.dataPath), LOG_LOCATION);
            }
        }

        /// <summary>
        /// Handles any logging events from unity
        /// </summary>
        /// <param name="logString">The log string</param>
        /// <param name="stackTrace">The stacktrace for the log</param>
        /// <param name="type">The log type</param>
        private void _handleUnityLog(string logString, string stackTrace, UnityEngine.LogType type)
        {
            switch (type)
            {
                case UnityEngine.LogType.Assert:
                case UnityEngine.LogType.Log:
                    _handleLog(logString, stackTrace, LogType.LOG);
                    break;
                case UnityEngine.LogType.Warning:
                    _handleLog(logString, stackTrace, LogType.WARNING);
                    break;
                case UnityEngine.LogType.Error:
                case UnityEngine.LogType.Exception:
                    _handleLog(logString, stackTrace, LogType.ERROR);
                    break;
            }
        }

        /// <summary>
        /// Handles any logging events
        /// </summary>
        /// <param name="logString">The log string</param>
        /// <param name="stackTrace">The stacktrace for the log</param>
        /// <param name="type">The log type</param>
        private void _handleLog(string logString, string stackTrace, LogType type)
        {
            _writeLog(type, logString, stackTrace);
        }

        /// <summary>
        /// Writes the log internally to the build location
        /// </summary>
        /// <param name="logType">The log type</param>
        /// <param name="log">The logged object</param>
        /// <param name="stackTrace">The stacktrace for the log</param>
        private void _writeLog(LogType logType, object log, string stackTrace)
        {
            string location = LogLocation;

            //_internalStringBuilder.Clear();
            string pathHead = Path.GetDirectoryName(location);

            if (!Directory.Exists(pathHead))
                Directory.CreateDirectory(pathHead);

            if (!File.Exists(location))
            {
                using (File.CreateText(location))
                {
                }
            }
            else
            {
                using (StreamReader sr = new StreamReader(location))
                {
                    _internalStringBuilder.Append(sr.ReadToEnd());
                }
            }

            _internalStringBuilder.AppendLine(string.Format("{0:F}", System.DateTime.UtcNow));
            _internalStringBuilder.Append(logType.ToString());
            _internalStringBuilder.AppendLine(" Message:");
            _internalStringBuilder.AppendLine(log.ToString());

            if (UnityEngine.Application.isEditor)
            {
                string[] lines = stackTrace.Split('\n');
                for (int i = lines.Length - 1; i >= 0; --i)
                {
                    string strippedLine = lines[i].Trim().Replace(" ", string.Empty);
                    if (strippedLine.StartsWith("atSystem.Environment.get_StackTrace") ||
                        strippedLine.StartsWith("atCore.CoreObject._writeLog") ||
                        strippedLine.StartsWith("atCore.CoreObject._handleLog") ||
                        strippedLine.StartsWith("atUnityEngine.Application.CallLogCallback") ||
                        strippedLine.StartsWith("atUnityEngine.DebugLogHandler.Internal_Log") ||
                        strippedLine.StartsWith("atUnityEngine.DebugLogHandler.LogFormat") ||
                        strippedLine.StartsWith("atUnityEngine.Logger.Log") ||
                        strippedLine.StartsWith("atCore.CoreObject.Log") ||
                        strippedLine.StartsWith("UnityEngine.Debug:Log") ||
                        strippedLine.StartsWith("UnityEngine.Debug:LogWarning") ||
                        strippedLine.StartsWith("UnityEngine.Debug:LogError"))
                        continue;

                    _internalStringBuilder.AppendLine(lines[i]);
                }
            }
            else
                _internalStringBuilder.AppendLine(stackTrace);

            _internalStringBuilder.Append(Environment.NewLine);
            _internalStringBuilder.AppendLine("====================================================================");

            using (StreamWriter sw = new StreamWriter(location))
            {
                sw.Write(_internalStringBuilder.ToString());
            }
        }
        #endregion
    }
}