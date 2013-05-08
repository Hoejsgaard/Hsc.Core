using System;

namespace Hsc.Foundation.Log
{
    /// <summary>
    ///     Convenient builder for fluent logging. Remember to end your train with a call to Write().
    /// </summary>
    /// <remarks>Fluent interfaces break SRP and DRY for general convenience.</remarks>
    public class LogEntryBuilder
    {
        private readonly ILogger _logger;

        public LogEntryBuilder(ILogger logger)
        {
            _logger = logger;
            LogEntry = new LogEntry();
        }

        public LogEntry LogEntry { get; private set; }

        public LogEntryBuilder WithMessage(string format, params object[] formatArgs)
        {
            LogEntry.Message = string.Format(format, formatArgs);
            return this;
        }

        public LogEntryBuilder AsError()
        {
            LogEntry.Level = LogLevel.Error;
            return this;
        }

        public LogEntryBuilder AsTrace()
        {
            LogEntry.Level = LogLevel.Trace;
            return this;
        }

        public LogEntryBuilder AsDebug()
        {
            LogEntry.Level = LogLevel.Debug;
            return this;
        }

        public LogEntryBuilder AsInfo()
        {
            LogEntry.Level = LogLevel.Info;
            return this;
        }

        public LogEntryBuilder AsWarning()
        {
            LogEntry.Level = LogLevel.Warn;
            return this;
        }

        public LogEntryBuilder AsFatal()
        {
            LogEntry.Level = LogLevel.Fatal;
            return this;
        }

        public LogEntryBuilder WithResolution(string resolution)
        {
            LogEntry.Resolution = resolution;
            return this;
        }

        public LogEntryBuilder WithCause(string cause)
        {
            LogEntry.Cause = cause;
            return this;
        }

        public LogEntryBuilder WithEventId(int eventId)
        {
            LogEntry.EventId = eventId;

            return this;
        }

        public LogEntryBuilder WithException(Exception exception)
        {
            LogEntry.Exception = exception;
            return this;
        }

        public void Write()
        {
            _logger.Write(LogEntry);
        }
    }
}