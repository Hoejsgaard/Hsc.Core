using System;
using System.Collections.Generic;
using NLog;

namespace Hsc.Foundation.Log.Nlog
{
    /// <summary>
    ///     NLog implementation of <see cref="ILogger" />.
    /// </summary>
    public class NLogLogger : ILogger
    {
        private readonly ILogEventInfoSplitter _logEventInfoSplitter;
        private readonly INLogConfigurationRepository _nlogConfigurationRepository;
        private Logger _logger;


        public NLogLogger(INLogConfigurationRepository nlogConfigurationRepository,
                          ILogEventInfoSplitter logEventInfoSplitter)
        {
            _nlogConfigurationRepository = nlogConfigurationRepository;
            _nlogConfigurationRepository.OnConfigurationChanged += OnConfigurationRepositoryChanged;
            _logEventInfoSplitter = logEventInfoSplitter;

            GetLoggerFromRepository();
        }

        public void Flush()
        {
            LogManager.Flush();
        }

        public void Write(LogEntry entry)
        {
            if (entry == null)
            {
                return;
            }

            LogEventInfo eventInfo = Convert(entry);
            List<LogEventInfo> logEventInfos = _logEventInfoSplitter.GetEventFragments(eventInfo);

            Write(logEventInfos);
        }

        private void GetLoggerFromRepository()
        {
            LogManager.Configuration = _nlogConfigurationRepository.GetConfiguration();
            _logger = LogManager.GetCurrentClassLogger();
        }

        private void OnConfigurationRepositoryChanged(object sender, EventArgs args)
        {
            GetLoggerFromRepository();
        }

        private void Write(IEnumerable<LogEventInfo> events)
        {
            foreach (LogEventInfo logEventInfo in events)
            {
                _logger.Log(typeof (NLogLogger), logEventInfo);
            }
        }

        private LogEventInfo Convert(LogEntry entry)
        {
            var logEventInfo = new LogEventInfo {Message = entry.ToString()};
            switch (entry.Level)
            {
                case LogLevel.Trace:
                    logEventInfo.Level = NLog.LogLevel.Trace;
                    break;
                case LogLevel.Debug:
                    logEventInfo.Level = NLog.LogLevel.Debug;
                    break;
                case LogLevel.Info:
                    logEventInfo.Level = NLog.LogLevel.Info;
                    break;
                case LogLevel.Warn:
                    logEventInfo.Level = NLog.LogLevel.Warn;
                    break;
                case LogLevel.Error:
                    logEventInfo.Level = NLog.LogLevel.Error;
                    break;
                case LogLevel.Fatal:
                    logEventInfo.Level = NLog.LogLevel.Fatal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (entry.Exception != null)
            {
                logEventInfo.Exception = entry.Exception;
            }
            if (entry.EventId.HasValue)
            {
                logEventInfo.Properties["eventID"] = entry.EventId.Value;
            }
            logEventInfo.TimeStamp = DateTime.UtcNow;

            return logEventInfo;
        }
    }
}